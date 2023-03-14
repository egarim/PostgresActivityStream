--The database must be created before you run this script
--CREATE DATABASE ActivityStream;

CREATE EXTENSION IF NOT EXISTS postgis; -- Enable PostGIS extension

CREATE TABLE as_objectstorage (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    latitude DECIMAL(9,6) NOT NULL,
    longitude DECIMAL(9,6) NOT NULL,
    location GEOMETRY(Point, 4326), -- 4326 is the SRID for WGS 84, a common coordinate system for GPS data
    object_type TEXT NOT NULL,
    object_data JSONB NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION as_update_location() RETURNS TRIGGER AS $$
BEGIN
    NEW.location := ST_SetSRID(ST_MakePoint(NEW.longitude, NEW.latitude), 4326);
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER as_set_location
    BEFORE INSERT OR UPDATE
    ON as_objectstorage
    FOR EACH ROW
    EXECUTE FUNCTION as_update_location();
   
CREATE OR REPLACE FUNCTION as_upsert_objectstorage(
    p_id UUID, 
    p_latitude DECIMAL(9,6), 
    p_longitude DECIMAL(9,6),
    p_object_type TEXT,
    p_object_data JSONB
) RETURNS VOID AS $$
BEGIN
    -- Try to update the existing row
    UPDATE as_objectstorage SET
        latitude = p_latitude,
        longitude = p_longitude,
        location = ST_SetSRID(ST_MakePoint(p_longitude, p_latitude), 4326),
        object_type = p_object_type,
        object_data = p_object_data,
        updated_at = CURRENT_TIMESTAMP
    WHERE id = p_id;
    
    -- If no row was updated, insert a new one
    IF NOT FOUND THEN
        INSERT INTO as_objectstorage (id, latitude, longitude, location, object_type, object_data)
        VALUES (p_id, p_latitude, p_longitude, ST_SetSRID(ST_MakePoint(p_longitude, p_latitude), 4326), p_object_type, p_object_data);
    END IF;
END;
$$ LANGUAGE plpgsql;


CREATE TABLE as_activity (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    verb TEXT NOT NULL,
    actor_id UUID NOT NULL REFERENCES as_objectstorage(id),
    object_id UUID NOT NULL REFERENCES as_objectstorage(id),
    target_id UUID REFERENCES as_objectstorage(id),
    latitude DECIMAL(9,6) NOT NULL,
    longitude DECIMAL(9,6) NOT NULL,
    location GEOMETRY(Point, 4326) NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);



CREATE OR REPLACE FUNCTION as_update_activity_location() RETURNS TRIGGER AS $$
BEGIN
    NEW.location := ST_SetSRID(ST_MakePoint(NEW.longitude, NEW.latitude), 4326);
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;



CREATE TRIGGER as_set_activity_location
    BEFORE INSERT OR UPDATE
    ON as_activity
    FOR EACH ROW
    EXECUTE FUNCTION as_update_activity_location();


CREATE OR REPLACE FUNCTION as_upsert_activity(
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
    UPDATE as_activity SET
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
        INSERT INTO as_activity (id, verb, actor_id, object_id, target_id, latitude, longitude, location)
        VALUES (p_id, p_verb, p_actor_id, p_object_id, p_target_id, p_latitude, p_longitude, ST_SetSRID(ST_MakePoint(p_longitude, p_latitude), 4326));
    END IF;
END;
$$ LANGUAGE plpgsql;


---
ALTER TABLE as_activity ADD COLUMN self JSON;

CREATE OR REPLACE FUNCTION as_update_activity_self() RETURNS TRIGGER AS $$
BEGIN
    NEW.self = json_build_object(
        'id', NEW.id,
        'verb', NEW.verb,
        'actor_id',NEW.actor_id,
        'actor', (SELECT object_data FROM as_objectstorage WHERE id = NEW.actor_id),
        'object_id',NEW.object_id,
        'object', (SELECT object_data FROM as_objectstorage WHERE id = NEW.object_id),
        'target_id',NEW.target_id,
        'target', (SELECT object_data FROM as_objectstorage WHERE id = NEW.target_id),
        'latitude', NEW.latitude,
        'longitude', NEW.longitude,
        'created_at', NEW.created_at,
        'updated_at', NEW.updated_at
    )::jsonb;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER as_activity_self_trigger
    BEFORE INSERT OR UPDATE ON as_activity
    FOR EACH ROW
    EXECUTE FUNCTION as_update_activity_self();

CREATE OR REPLACE FUNCTION as_get_activities_by_distance_as_json(
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
        FROM as_activity a
        WHERE ST_DWithin(location::geography, ST_SetSRID(ST_Point(p_long, p_lat), 4326)::geography, p_distance)
        ORDER BY created_at DESC
        LIMIT p_page_size
        OFFSET (p_page_num - 1) * p_page_size
    ) a;
    
    RETURN activities_json;
END;
$$ LANGUAGE plpgsql;
---


CREATE TABLE as_follow (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    follower_id UUID NOT NULL REFERENCES as_objectstorage(id),
    followee_id UUID NOT NULL REFERENCES as_objectstorage(id),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION as_follow_user(
    p_follower_id UUID,
    p_followee_id UUID
) RETURNS VOID AS $$
BEGIN
    -- Check if a row with the same follower_id and followee_id values already exists
    IF EXISTS (
        SELECT 1
        FROM as_follow
        WHERE follower_id = p_follower_id AND followee_id = p_followee_id
    ) THEN
        RETURN;
    END IF;
    
    -- If the row does not exist, insert a new row into the follow table
    INSERT INTO as_follow (follower_id, followee_id)
    VALUES (p_follower_id, p_followee_id);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION as_unfollow_user(
    p_follower_id UUID,
    p_followee_id UUID
) RETURNS VOID AS $$
BEGIN
    -- Delete the row from the follow table where the follower_id and followee_id match
    DELETE FROM as_follow
    WHERE follower_id = p_follower_id AND followee_id = p_followee_id;
END;
$$ LANGUAGE plpgsql;

--Activities
CREATE OR REPLACE FUNCTION as_get_following_ids(p_user_id UUID)
RETURNS UUID[] AS $$
DECLARE
  following_ids UUID[];
BEGIN
  SELECT ARRAY_AGG(followee_id) INTO following_ids
  FROM as_follow
  WHERE follower_id = p_user_id;
  
  RETURN following_ids;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION as_get_activities_by_followed_ids(
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
  FROM as_activity a
  WHERE a.actor_id = ANY(p_followed_ids)
  ORDER BY a.created_at DESC, a.id DESC
  LIMIT p_page_size
  OFFSET v_offset;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION as_get_activities_by_following(p_page_num INTEGER, p_page_size INTEGER, p_following_ids UUID[])
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
    FROM as_activity a
    WHERE a.actor_id = ANY(p_following_ids)
    ORDER BY a.created_at DESC
    LIMIT p_page_size
    OFFSET (p_page_num - 1) * p_page_size;
END;
$$ LANGUAGE plpgsql;

---
CREATE OR REPLACE FUNCTION as_get_activities_by_following_as_json(p_page_num INTEGER, p_page_size INTEGER, p_user_id UUID)
RETURNS JSON AS $$
DECLARE
    following_ids UUID[] := ARRAY(SELECT as_get_following_ids(p_user_id));
BEGIN
    RETURN (SELECT json_agg(self_data) FROM as_get_activities_by_following(p_page_num, p_page_size, following_ids));
END;
$$ LANGUAGE plpgsql;

---

CREATE OR REPLACE FUNCTION as_get_followees_by_object_id(
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
  FROM as_follow f
  INNER JOIN as_objectstorage o ON f.followee_id = o.id
  WHERE f.follower_id = p_object_id
  ORDER BY o.created_at DESC
  LIMIT p_page_size
  OFFSET v_offset;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION as_get_followees_by_object_id_as_json(
  p_object_id UUID,
  p_page_number INTEGER,
  p_page_size INTEGER
) 
RETURNS JSON AS $$
BEGIN
    RETURN (SELECT json_agg(followee_data) FROM as_get_followees_by_object_id(p_object_id, p_page_number, p_page_size));
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION as_get_objects_by_criteria_query(
    p_object_type TEXT,
    p_created_at TIMESTAMP WITH TIME ZONE,
    p_updated_at TIMESTAMP WITH TIME ZONE,
    p_object_data_where TEXT,
    p_page_number INTEGER,
    p_page_size INTEGER,
    p_location_radius TEXT
) RETURNS TEXT AS $$
DECLARE
    v_offset INTEGER := (p_page_number - 1) * p_page_size;
    v_where_clause TEXT := '';
    v_location_radius FLOAT;
BEGIN
    -- Construct the WHERE clause based on the function parameters
    IF p_object_type IS NOT NULL THEN
        v_where_clause := v_where_clause || 'object_type = ''' || p_object_type || ''' AND ';
    END IF;

    IF p_created_at IS NOT NULL THEN
        v_where_clause := v_where_clause || 'created_at >= ''' || p_created_at || ''' AND ';
    END IF;

    IF p_updated_at IS NOT NULL THEN
        v_where_clause := v_where_clause || 'updated_at >= ''' || p_updated_at || ''' AND ';
    END IF;

    IF p_object_data_where IS NOT NULL THEN
        v_where_clause := v_where_clause || 'object_data ->> ' || replace(p_object_data_where, '"', '''') || ' AND ';
    END IF;

    -- Parse the location radius parameter
    IF p_location_radius IS NOT NULL THEN
        v_location_radius := CAST(split_part(p_location_radius, ',', 3) AS FLOAT);
    END IF;

    -- Add the location filter to the WHERE clause
    IF p_location_radius IS NOT NULL THEN
        v_where_clause := v_where_clause || 'ST_DWithin(location, ST_MakePoint(' || split_part(p_location_radius, ',', 2) || ', ' || split_part(p_location_radius, ',', 1) || ')::geography, ' || v_location_radius || ') AND ';
    END IF;

    -- Remove the trailing 'AND' from the WHERE clause
    v_where_clause := LEFT(v_where_clause, LENGTH(v_where_clause) - 5);

    RETURN 'SELECT id, latitude, longitude, location, object_type, object_data, created_at, updated_at
            FROM as_objectstorage WHERE ' || v_where_clause || '
            ORDER BY created_at DESC, id DESC
            LIMIT ' || p_page_size || ' OFFSET ' || v_offset;
END;
$$ LANGUAGE plpgsql;
