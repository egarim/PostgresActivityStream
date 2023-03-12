CREATE DATABASE ActivityStream;

CREATE EXTENSION IF NOT EXISTS postgis; -- Enable PostGIS extension

CREATE TABLE objectstorage (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    latitude DECIMAL(9,6) NOT NULL,
    longitude DECIMAL(9,6) NOT NULL,
    location GEOMETRY(Point, 4326), -- 4326 is the SRID for WGS 84, a common coordinate system for GPS data
    object_type TEXT NOT NULL,
    object_data JSONB NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION update_location() RETURNS TRIGGER AS $$
BEGIN
    NEW.location := ST_SetSRID(ST_MakePoint(NEW.longitude, NEW.latitude), 4326);
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER set_location
    BEFORE INSERT OR UPDATE
    ON objectstorage
    FOR EACH ROW
    EXECUTE FUNCTION update_location();
   
CREATE OR REPLACE FUNCTION upsert_objectstorage(
    p_id UUID, 
    p_latitude DECIMAL(9,6), 
    p_longitude DECIMAL(9,6),
    p_object_type TEXT,
    p_object_data JSONB
) RETURNS VOID AS $$
BEGIN
    -- Try to update the existing row
    UPDATE objectstorage SET
        latitude = p_latitude,
        longitude = p_longitude,
        location = ST_SetSRID(ST_MakePoint(p_longitude, p_latitude), 4326),
        object_type = p_object_type,
        object_data = p_object_data,
        updated_at = CURRENT_TIMESTAMP
    WHERE id = p_id;
    
    -- If no row was updated, insert a new one
    IF NOT FOUND THEN
        INSERT INTO objectstorage (id, latitude, longitude, location, object_type, object_data)
        VALUES (p_id, p_latitude, p_longitude, ST_SetSRID(ST_MakePoint(p_longitude, p_latitude), 4326), p_object_type, p_object_data);
    END IF;
END;
$$ LANGUAGE plpgsql;


CREATE TABLE activity (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    verb TEXT NOT NULL,
    actor_id UUID NOT NULL REFERENCES objectstorage(id),
    object_id UUID NOT NULL REFERENCES objectstorage(id),
    target_id UUID REFERENCES objectstorage(id),
    latitude DECIMAL(9,6) NOT NULL,
    longitude DECIMAL(9,6) NOT NULL,
    location GEOMETRY(Point, 4326) NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION update_activity_location() RETURNS TRIGGER AS $$
BEGIN
    NEW.location := ST_SetSRID(ST_MakePoint(NEW.longitude, NEW.latitude), 4326);
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER set_activity_location
    BEFORE INSERT OR UPDATE
    ON activity
    FOR EACH ROW
    EXECUTE FUNCTION update_activity_location();


CREATE OR REPLACE FUNCTION upsert_activity(
    p_id UUID,
    p_verb TEXT,
    p_actor_id UUID,
    p_object_id UUID,
    p_target_id UUID,
    p_latitude DECIMAL(9,6),
    p_longitude DECIMAL(9,6)
) RETURNS VOID AS $$
BEGIN
    -- Try to update the existing row
    UPDATE activity SET
        verb = p_verb,
        actor_id = p_actor_id,
        object_id = p_object_id,
        target_id = p_target_id,
        latitude = p_latitude,
        longitude = p_longitude,
        location = ST_SetSRID(ST_MakePoint(p_longitude, p_latitude), 4326),
        updated_at = CURRENT_TIMESTAMP
    WHERE id = p_id;
    
    -- If no row was updated, insert a new one
    IF NOT FOUND THEN
        INSERT INTO activity (id, verb, actor_id, object_id, target_id, latitude, longitude, location)
        VALUES (p_id, p_verb, p_actor_id, p_object_id, p_target_id, p_latitude, p_longitude, ST_SetSRID(ST_MakePoint(p_longitude, p_latitude), 4326));
    END IF;
END;
$$ LANGUAGE plpgsql;


---
ALTER TABLE activity ADD COLUMN self JSON;

CREATE OR REPLACE FUNCTION update_activity_self() RETURNS TRIGGER AS $$
BEGIN
    NEW.self = json_build_object(
        'id', NEW.id,
        'verb', NEW.verb,
        'actor_id',NEW.actor_id,
        'actor', (SELECT object_data FROM objectstorage WHERE id = NEW.actor_id),
        'object_id',NEW.object_id,
        'object', (SELECT object_data FROM objectstorage WHERE id = NEW.object_id),
        'target_id',NEW.target_id,
        'target', (SELECT object_data FROM objectstorage WHERE id = NEW.target_id),
        'latitude', NEW.latitude,
        'longitude', NEW.longitude,
        'created_at', NEW.created_at,
        'updated_at', NEW.updated_at
    )::jsonb;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER activity_self_trigger
    BEFORE INSERT OR UPDATE ON activity
    FOR EACH ROW
    EXECUTE FUNCTION update_activity_self();

CREATE OR REPLACE FUNCTION get_activities_by_distance_as_json(
    p_lat NUMERIC,
    p_long NUMERIC,
    p_distance INTEGER,
    p_page_num INTEGER,
    p_page_size INTEGER
) 
RETURNS JSON
AS $$
DECLARE
    activities_json JSON;
BEGIN
    SELECT json_agg(a.self) INTO activities_json
    FROM (
        SELECT a.self
        FROM activity a
        WHERE ST_DWithin(location::geography, ST_SetSRID(ST_Point(p_long, p_lat), 4326)::geography, p_distance)
        ORDER BY created_at DESC
        LIMIT p_page_size
        OFFSET (p_page_num - 1) * p_page_size
    ) a;
    
    RETURN activities_json;
END;
$$ LANGUAGE plpgsql;
---


CREATE TABLE follow (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    follower_id UUID NOT NULL REFERENCES objectstorage(id),
    followee_id UUID NOT NULL REFERENCES objectstorage(id),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION follow_user(
    p_follower_id UUID,
    p_followee_id UUID
) RETURNS VOID AS $$
BEGIN
    -- Try to insert a new row into the follow table
    -- If the row already exists, do nothing
    BEGIN
        INSERT INTO follow (follower_id, followee_id)
        VALUES (p_follower_id, p_followee_id);
    EXCEPTION WHEN unique_violation THEN
        RETURN;
    END;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION unfollow_user(
    p_follower_id UUID,
    p_followee_id UUID
) RETURNS VOID AS $$
BEGIN
    -- Delete the row from the follow table where the follower_id and followee_id match
    DELETE FROM follow
    WHERE follower_id = p_follower_id AND followee_id = p_followee_id;
END;
$$ LANGUAGE plpgsql;

--Activities
CREATE OR REPLACE FUNCTION get_following_ids(p_user_id UUID)
RETURNS UUID[] AS $$
DECLARE
  following_ids UUID[];
BEGIN
  SELECT ARRAY_AGG(followee_id) INTO following_ids
  FROM follow
  WHERE follower_id = p_user_id;
  
  RETURN following_ids;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION get_activities_by_followed_ids(
  p_page_number INTEGER,
  p_page_size INTEGER,
  p_followed_ids UUID[]
) RETURNS TABLE (
  activity_id UUID,
  verb TEXT,
  actor_id UUID,
  object_id UUID,
  target_id UUID,
  latitude DECIMAL(9,6),
  longitude DECIMAL(9,6),
  created_at TIMESTAMP WITH TIME ZONE,
  updated_at TIMESTAMP WITH TIME ZONE
) AS $$
DECLARE
  v_offset INTEGER;
BEGIN
  v_offset := (p_page_number - 1) * p_page_size;
  
  RETURN QUERY
  SELECT a.id AS activity_id,
         a.verb,
         a.actor_id,
         a.object_id,
         a.target_id,
         a.latitude,
         a.longitude,
         a.created_at,
         a.updated_at
  FROM activity a
  WHERE a.actor_id = ANY(p_followed_ids)
  ORDER BY a.created_at DESC, a.id DESC
  LIMIT p_page_size
  OFFSET v_offset;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_activities_by_following(p_page_num INTEGER, p_page_size INTEGER, p_following_ids UUID[])
    RETURNS TABLE (
        id UUID,
        verb TEXT,
        actor_id UUID,
        object_id UUID,
        target_id UUID,
        latitude DECIMAL(9,6),
        longitude DECIMAL(9,6),
        location GEOMETRY(Point, 4326),
        self_data JSON,
        created_at TIMESTAMP WITH TIME ZONE,
        updated_at TIMESTAMP WITH TIME ZONE
    ) AS $$
BEGIN
    RETURN QUERY
    SELECT a.id, a.verb, a.actor_id, a.object_id, a.target_id, a.latitude, a.longitude, a.location, a."self" , a.created_at, a.updated_at
    FROM activity a
    WHERE a.actor_id = ANY(p_following_ids)
    ORDER BY a.created_at DESC
    LIMIT p_page_size
    OFFSET (p_page_num - 1) * p_page_size;
END;
$$ LANGUAGE plpgsql;

---
CREATE OR REPLACE FUNCTION get_activities_by_following_as_json(p_page_num INTEGER, p_page_size INTEGER, p_user_id UUID)
RETURNS JSON AS $$
DECLARE
    following_ids UUID[] := ARRAY(SELECT get_following_ids(p_user_id));
BEGIN
    RETURN (SELECT json_agg(self_data) FROM get_activities_by_following(p_page_num, p_page_size, following_ids));
END;
$$ LANGUAGE plpgsql;

---



--create users and activities

SELECT upsert_objectstorage(
    'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', -- object ID 1
    59.9311, -- latitude
    30.3609, -- longitude
    'user', -- object type
    '{"name": "Alice", "age": 27, "email": "alice@example.com", "picture_url": "https://example.com/pictures/alice.jpg"}' -- object data in JSON format
);
SELECT upsert_objectstorage(
    'cc7ebda2-019c-4387-925c-352f7e1f0b10', -- object ID 2
    59.9428, -- latitude
    30.3071, -- longitude
    'user', -- object type
    '{"name": "Bob", "age": 33, "email": "bob@example.com", "picture_url": "https://example.com/pictures/bob.jpg"}' -- object data in JSON format
);

SELECT upsert_objectstorage(
    '99875f15-49ee-4e6d-b356-cbab4f4e4a4c', -- object ID 3
    59.9375, -- latitude
    30.3086, -- longitude
    'user', -- object type
    '{"name": "Charlie", "age": 42, "email": "charlie@example.com", "picture_url": "https://example.com/pictures/charlie.jpg"}' -- object data in JSON format
);

SELECT upsert_objectstorage(
    '34f6c0a5-5d5e-463f-a2cf-11b7529a92a1', -- object ID 4
    59.9167, -- latitude
    30.25, -- longitude
    'user', -- object type
    '{"name": "Dave", "age": 29, "email": "dave@example.com", "picture_url": "https://example.com/pictures/dave.jpg"}' -- object data in JSON format
);

SELECT upsert_objectstorage(
    '8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54', -- object ID 5
    59.9391, -- latitude
    30.3158, -- longitude
    'user', -- object type
    '{"name": "Eve", "age": 25, "email": "eve@example.com", "picture_url": "https://example.com/pictures/eve.jpg"}' -- object data in JSON format
);

--create ads

-- Bob's ad
SELECT upsert_objectstorage(
'f6c7599e-8161-4d54-82ec-faa13bb8cbf7', -- object ID
59.9428, -- latitude (near Saint Petersburg)
30.3071, -- longitude (near Saint Petersburg)
'ad', -- object type
'{"description": "Vintage bicycle, good condition", "ad_type": "sale", "picture_url": "https://example.com/pictures/bicycle.jpg"}' -- object data in JSON format
);

SELECT upsert_activity(
gen_random_uuid(), -- activity ID
'post', -- verb
'cc7ebda2-019c-4387-925c-352f7e1f0b10', -- actor ID (Bob)
'f6c7599e-8161-4d54-82ec-faa13bb8cbf7', -- object ID (Bob's ad)
NULL, -- target ID (no target)
59.9428, -- latitude (near Saint Petersburg)
30.3071 -- longitude (near Saint Petersburg)
);

-- Charlie's ad
SELECT upsert_objectstorage(
'41f7c558-1cf8-4f2b-b4b4-4d4e4df50843', -- object ID
59.9375, -- latitude (near Saint Petersburg)
30.3086, -- longitude (near Saint Petersburg)
'ad', -- object type
'{"description": "Smartphone, unlocked", "ad_type": "sale", "picture_url": "https://example.com/pictures/smartphone.jpg"}' -- object data in JSON format
);

SELECT upsert_activity(
gen_random_uuid(), -- activity ID
'post', -- verb
'99875f15-49ee-4e6d-b356-cbab4f4e4a4c', -- actor ID (Charlie)
'41f7c558-1cf8-4f2b-b4b4-4d4e4df50843', -- object ID (Charlie's ad)
NULL, -- target ID (no target)
59.9375, -- latitude (near Saint Petersburg)
30.3086 -- longitude (near Saint Petersburg)
);


-- Dave's ad
SELECT upsert_objectstorage(
'c3dd7b47-1bba-4916-8a6a-8b5f2b50ba88', -- object ID
59.9139, -- latitude (near Saint Petersburg)
30.3341, -- longitude (near Saint Petersburg)
'ad', -- object type
'{"description": "Vintage camera, working condition", "ad_type": "exchange", "picture_url": "https://example.com/pictures/camera.jpg"}' -- object data in JSON format
);

SELECT upsert_activity(
gen_random_uuid(), -- activity ID
'post', -- verb
'34f6c0a5-5d5e-463f-a2cf-11b7529a92a1', -- actor ID (Dave)
'c3dd7b47-1bba-4916-8a6a-8b5f2b50ba88', -- object ID (Dave's ad)
NULL, -- target ID (no target)
59.9139, -- latitude (near Saint Petersburg)
30.3341 -- longitude (near Saint Petersburg)
);

-- Eve's ad
SELECT upsert_objectstorage(
'3453f3c1-296d-47a5-a5a5-f5db5ed3f3b3', -- object ID
59.9375, -- latitude (near Saint Petersburg)
30.3141, -- longitude (near Saint Petersburg)
'ad', -- object type
'{"description": "Plants, various types", "ad_type": "want", "picture_url": "https://example.com/pictures/plants.jpg"}' -- object data in JSON format
);

SELECT upsert_activity(
gen_random_uuid(), -- activity ID
'post', -- verb
'8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54', -- actor ID (Eve)
'3453f3c1-296d-47a5-a5a5-f5db5ed3f3b3', -- object ID (Eve's ad)
NULL, -- target ID (no target)
59.9375, -- latitude (near Saint Petersburg)
30.3141 -- longitude (near Saint Petersburg)
);

-- Alice's ad
SELECT upsert_objectstorage(
    'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c02', -- new object ID for Alice's ad
    59.9311, -- latitude
    30.3609, -- longitude
    'ad', -- object type
    '{"description": "Used bicycle, good condition", "ad_type": "sell", "picture_url": "https://example.com/pictures/bicycle.jpg"}' -- ad data in JSON format
);

SELECT upsert_activity(
    gen_random_uuid(), -- activity ID
    'post', -- verb
    'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', -- actor ID (Alice)
    'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c02', -- object ID (Alice's ad)
    NULL, -- target ID (no target)
    59.9311, -- latitude
    30.3609 -- longitude
);
-- Charly's ad 
SELECT upsert_objectstorage(
    '99875f15-49ee-4e6d-b356-cbab4f4e4a4d', -- new object ID for Charlie's ad
    59.9375, -- latitude
    30.3086, -- longitude
    'ad', -- object type
    '{"description": "Books, various genres", "ad_type": "sell", "picture_url": "https://example.com/pictures/books.jpg"}' -- ad data in JSON format
);

SELECT upsert_activity(
    gen_random_uuid(), -- activity ID
    'post', -- verb
    '99875f15-49ee-4e6d-b356-cbab4f4e4a4c', -- actor ID (Charlie)
    '99875f15-49ee-4e6d-b356-cbab4f4e4a4d', -- object ID (Charlie's ad)
    NULL, -- target ID (no target)
    59.9428, -- latitude
    30.3071 -- longitude
);
-- Bob's ad
SELECT upsert_objectstorage(
    'cc7ebda2-019c-4387-925c-352f7e1f0b12', -- new object ID for Bob's ad
    59.9428, -- latitude
    30.3071, -- longitude
    'ad', -- object type
    '{"description": "Vintage record player, needs repair", "ad_type": "exchange", "picture_url": "https://example.com/pictures/record_player.jpg"}' -- ad data in JSON format
);
SELECT upsert_activity(
    gen_random_uuid(), -- activity ID
    'post', -- verb
    'cc7ebda2-019c-4387-925c-352f7e1f0b10', -- actor ID (Bob)
    'cc7ebda2-019c-4387-925c-352f7e1f0b12', -- object ID (Bob's ad)
    NULL, -- target ID (no target)
    59.9428, -- latitude
    30.3071 -- longitude
);

--create ads

-- Follow data

-- Follow Eve and Alice to themselves
SELECT follow_user('8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54', '8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54');
SELECT follow_user('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', 'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01');
-- Follow Eve and Alice to Bob, Charlie, and Dave
SELECT follow_user('8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54', 'cc7ebda2-019c-4387-925c-352f7e1f0b10');
SELECT follow_user('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', 'cc7ebda2-019c-4387-925c-352f7e1f0b10');
SELECT follow_user('8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54', '99875f15-49ee-4e6d-b356-cbab4f4e4a4c');
SELECT follow_user('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', '99875f15-49ee-4e6d-b356-cbab4f4e4a4c');
SELECT follow_user('8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54', '34f6c0a5-5d5e-463f-a2cf-11b7529a92a1');
SELECT follow_user('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', '34f6c0a5-5d5e-463f-a2cf-11b7529a92a1');
-- Follow data


--test 

SELECT get_following_ids('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01')


SELECT * FROM get_activities_by_following(1, 2, ARRAY(SELECT get_following_ids('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01')));

SELECT self_data FROM get_activities_by_following(1, 2, ARRAY(SELECT get_following_ids('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01')));

SELECT json_agg(self_data) FROM get_activities_by_following(1, 10, ARRAY(SELECT get_following_ids('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01')));

SELECT * FROM get_activities_by_following_as_json(1, 2, 'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01');

SELECT get_activities_by_distance_as_json(59.9343, 30.3351, 1600, 1, 10);

