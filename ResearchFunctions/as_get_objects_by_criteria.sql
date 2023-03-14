CREATE OR REPLACE FUNCTION as_get_objects_by_criteria(
    p_object_type TEXT,
    p_created_at TIMESTAMP WITH TIME ZONE,
    p_updated_at TIMESTAMP WITH TIME ZONE,
    p_page_num INTEGER,
    p_page_size INTEGER
) RETURNS SETOF as_objectstorage AS $$
DECLARE
    v_offset INTEGER := (p_page_num - 1) * p_page_size;
BEGIN
    RETURN QUERY
    SELECT *
    FROM as_objectstorage
    WHERE (p_object_type IS NULL OR object_type = p_object_type)
        AND (p_created_at IS NULL OR created_at >= p_created_at)
        AND (p_updated_at IS NULL OR updated_at >= p_updated_at)
    ORDER BY created_at DESC, id DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION as_get_objects_by_criteria3(
    p_object_type TEXT,
    p_created_at TIMESTAMP WITH TIME ZONE,
    p_updated_at TIMESTAMP WITH TIME ZONE,
    p_object_data_where TEXT,
    p_page_num INTEGER,
    p_page_size INTEGER
) RETURNS SETOF as_objectstorage AS $$
DECLARE
    v_offset INTEGER := (p_page_num - 1) * p_page_size;
BEGIN
    RETURN QUERY
    SELECT *
    FROM as_objectstorage
    WHERE (p_object_type IS NULL OR object_type = p_object_type)
        AND (p_created_at IS NULL OR created_at >= p_created_at)
        AND (p_updated_at IS NULL OR updated_at >= p_updated_at)
        AND (p_object_data_where IS NULL OR object_data @> jsonb_build_object(p_object_data_where))
    ORDER BY created_at DESC, id DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$ LANGUAGE plpgsql;

--this function has debug information
CREATE OR REPLACE FUNCTION as_get_objects_by_criteria2(
    p_object_type TEXT,
    p_created_at TIMESTAMP WITH TIME ZONE,
    p_updated_at TIMESTAMP WITH TIME ZONE,
    p_object_data_where TEXT,
    p_page_number INTEGER,
    p_page_size INTEGER
) RETURNS TABLE (
    id UUID,
    latitude DECIMAL(9,6),
    longitude DECIMAL(9,6),
    location GEOMETRY(Point, 4326),
    object_type TEXT,
    object_data JSONB,
    created_at TIMESTAMP WITH TIME ZONE,
    updated_at TIMESTAMP WITH TIME ZONE
) AS $$
DECLARE
    offset INTEGER := (p_page_number - 1) * p_page_size;
    where_clause TEXT;
    object_data_json JSONB;
BEGIN
    -- Construct the WHERE clause based on the function parameters
    where_clause := 'WHERE (p_object_type IS NULL OR object_type = p_object_type)';
    IF p_created_at IS NOT NULL THEN
        where_clause := where_clause || ' AND created_at >= p_created_at';
    END IF;
    IF p_updated_at IS NOT NULL THEN
        where_clause := where_clause || ' AND updated_at >= p_updated_at';
    END IF;
    IF p_object_data_where IS NOT NULL THEN
        object_data_json := jsonb_build_object(p_object_data_where);
        RAISE NOTICE 'Constructed JSON object: %', object_data_json;
        where_clause := where_clause || ' AND object_data @> ' || quote_literal(object_data_json::text);
    END IF;

    RETURN QUERY EXECUTE format('
        SELECT id, latitude, longitude, location, object_type, object_data, created_at, updated_at
        FROM as_objectstorage
        %s
        ORDER BY created_at DESC, id DESC
        LIMIT %s
        OFFSET %s', where_clause, p_page_size, offset);
END;
$$ LANGUAGE plpgsql;


--get recommendation
CREATE OR REPLACE FUNCTION as_get_recommendations(
    p_user_id UUID,
    p_max_recommendations INTEGER
) RETURNS TABLE (
    id UUID,
    username TEXT,
    location GEOMETRY(Point, 4326),
    shared_interests INTEGER
) AS $$
BEGIN
    RETURN QUERY
    WITH 
    following_ids AS (
        SELECT followee_id
        FROM as_follow
        WHERE follower_id = p_user_id
    ),
    user_interactions AS (
        SELECT object_id
        FROM as_activity
        WHERE actor_id = p_user_id
    ),
    user_tags AS (
        SELECT DISTINCT tag
        FROM (
            SELECT jsonb_array_elements(object_data->'tags') AS tag
            FROM as_objectstorage
            WHERE id IN (SELECT object_id FROM user_interactions)
        ) AS tags
    ),
    users_with_similar_tags AS (
        SELECT o.id, o.username, o.location, COUNT(jsonb_array_elements(o.object_data->'tags') & user_tags.tag) AS shared_interests
        FROM as_objectstorage o
        JOIN user_tags ON o.object_data @> jsonb_build_object('tags', jsonb_agg(user_tags.tag))
        WHERE o.id NOT IN (SELECT followee_id FROM following_ids) -- exclude users already being followed
        AND o.id <> p_user_id -- exclude the logged-in user
        AND o.id IN (
            SELECT DISTINCT actor_id
            FROM as_activity
            WHERE object_id IN (SELECT object_id FROM user_interactions)
        )
        GROUP BY o.id, o.username, o.location
        ORDER BY shared_interests DESC
        LIMIT p_max_recommendations
    ),
    users_nearby AS (
        SELECT o.id, o.username, o.location, ST_Distance(o.location::geography, u.location::geography) AS distance
        FROM as_objectstorage o
        JOIN as_objectstorage u ON u.id = p_user_id
        WHERE o.id NOT IN (SELECT followee_id FROM following_ids) -- exclude users already being followed
        AND o.id <> p_user_id -- exclude the logged-in user
        AND ST_DWithin(o.location::geography, u.location::geography, 50000) -- limit to users within 50 km
        ORDER BY distance ASC
        LIMIT p_max_recommendations
    )
    SELECT id, username, location, shared_interests FROM users_with_similar_tags
    UNION
    SELECT id, username, location, NULL AS shared_interests FROM users_nearby
    ORDER BY shared_interests DESC NULLS LAST, distance ASC NULLS LAST
    LIMIT p_max_recommendations;
END;
$$ LANGUAGE plpgsql;

--data for recommendations
SELECT as_upsert_objectstorage(
    '11111111-1111-1111-1111-111111111111', -- user 1
    '39.7392',
    '-104.9903',
    'user',
    '{"name": "Alice", "age": 25, "tags": ["art", "music", "travel"]}'
);

SELECT as_upsert_objectstorage(
    '22222222-2222-2222-2222-222222222222', -- user 2
    '40.7128',
    '-74.0060',
    'user',
    '{"name": "Bob", "age": 30, "tags": ["food", "sports", "movies"]}'
);

SELECT as_upsert_objectstorage(
    '33333333-3333-3333-3333-333333333333', -- user 3
    '37.7749',
    '-122.4194',
    'user',
    '{"name": "Charlie", "age": 35, "tags": ["technology", "reading", "photography"]}'
);



--recommendations test
SELECT *
FROM as_get_recommendations(
    '11111111-1111-1111-1111-111111111111', -- user ID to recommend for
    'user', -- object type to recommend
    'tags', -- JSON field to match against
    3 -- maximum number of recommendations to return
);
