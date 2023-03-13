--The database must be created before you run this script
--CREATE DATABASE ActivityStream;

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
    -- Check if a row with the same follower_id and followee_id values already exists
    IF EXISTS (
        SELECT 1
        FROM follow
        WHERE follower_id = p_follower_id AND followee_id = p_followee_id
    ) THEN
        RETURN;
    END IF;
    
    -- If the row does not exist, insert a new row into the follow table
    INSERT INTO follow (follower_id, followee_id)
    VALUES (p_follower_id, p_followee_id);
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

CREATE OR REPLACE FUNCTION get_followees_by_object_id(
  p_object_id UUID,
  p_page_number INTEGER,
  p_page_size INTEGER
) RETURNS TABLE (
  followee_id UUID,
  followee_data JSONB
) AS $$
DECLARE
  v_offset INTEGER;
BEGIN
  v_offset := (p_page_number - 1) * p_page_size;

  RETURN QUERY
  SELECT f.followee_id, o.object_data
  FROM follow f
  INNER JOIN objectstorage o ON f.followee_id = o.id
  WHERE f.follower_id = p_object_id
  ORDER BY o.created_at DESC
  LIMIT p_page_size
  OFFSET v_offset;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION get_followees_by_object_id_as_json(
  p_object_id UUID,
  p_page_number INTEGER,
  p_page_size INTEGER
) 
RETURNS JSON AS $$
BEGIN
    RETURN (SELECT json_agg(followee_data) FROM get_followees_by_object_id(p_object_id, p_page_number, p_page_size));
END;
$$ LANGUAGE plpgsql;

