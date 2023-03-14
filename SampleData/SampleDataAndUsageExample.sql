

--create users and activities

SELECT as_upsert_objectstorage(
    'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', -- object ID 1
    59.9311, -- latitude
    30.3609, -- longitude
    'user', -- object type
    '{"name": "Alice", "age": 27, "email": "alice@example.com", "picture_url": "https://example.com/pictures/alice.jpg"}' -- object data in JSON format
);
SELECT as_upsert_objectstorage(
    'cc7ebda2-019c-4387-925c-352f7e1f0b10', -- object ID 2
    59.9428, -- latitude
    30.3071, -- longitude
    'user', -- object type
    '{"name": "Bob", "age": 33, "email": "bob@example.com", "picture_url": "https://example.com/pictures/bob.jpg"}' -- object data in JSON format
);

SELECT as_upsert_objectstorage(
    '99875f15-49ee-4e6d-b356-cbab4f4e4a4c', -- object ID 3
    59.9375, -- latitude
    30.3086, -- longitude
    'user', -- object type
    '{"name": "Charlie", "age": 42, "email": "charlie@example.com", "picture_url": "https://example.com/pictures/charlie.jpg"}' -- object data in JSON format
);

SELECT as_upsert_objectstorage(
    '34f6c0a5-5d5e-463f-a2cf-11b7529a92a1', -- object ID 4
    59.9167, -- latitude
    30.25, -- longitude
    'user', -- object type
    '{"name": "Dave", "age": 29, "email": "dave@example.com", "picture_url": "https://example.com/pictures/dave.jpg"}' -- object data in JSON format
);

SELECT as_upsert_objectstorage(
    '8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54', -- object ID 5
    59.9391, -- latitude
    30.3158, -- longitude
    'user', -- object type
    '{"name": "Eve", "age": 25, "email": "eve@example.com", "picture_url": "https://example.com/pictures/eve.jpg"}' -- object data in JSON format
);

--create ads

-- Bob's ad
SELECT as_upsert_objectstorage(
'f6c7599e-8161-4d54-82ec-faa13bb8cbf7', -- object ID
59.9428, -- latitude (near Saint Petersburg)
30.3071, -- longitude (near Saint Petersburg)
'ad', -- object type
'{"description": "Vintage bicycle, good condition", "ad_type": "sale", "picture_url": "https://example.com/pictures/bicycle.jpg"}' -- object data in JSON format
);

SELECT as_upsert_activity(
gen_random_uuid(), -- activity ID
'post', -- verb
'cc7ebda2-019c-4387-925c-352f7e1f0b10', -- actor ID (Bob)
'f6c7599e-8161-4d54-82ec-faa13bb8cbf7', -- object ID (Bob's ad)
NULL, -- target ID (no target)
59.9428, -- latitude (near Saint Petersburg)
30.3071 -- longitude (near Saint Petersburg)
);

-- Charlie's ad
SELECT as_upsert_objectstorage(
'41f7c558-1cf8-4f2b-b4b4-4d4e4df50843', -- object ID
59.9375, -- latitude (near Saint Petersburg)
30.3086, -- longitude (near Saint Petersburg)
'ad', -- object type
'{"description": "Smartphone, unlocked", "ad_type": "sale", "picture_url": "https://example.com/pictures/smartphone.jpg"}' -- object data in JSON format
);

SELECT as_upsert_activity(
gen_random_uuid(), -- activity ID
'post', -- verb
'99875f15-49ee-4e6d-b356-cbab4f4e4a4c', -- actor ID (Charlie)
'41f7c558-1cf8-4f2b-b4b4-4d4e4df50843', -- object ID (Charlie's ad)
NULL, -- target ID (no target)
59.9375, -- latitude (near Saint Petersburg)
30.3086 -- longitude (near Saint Petersburg)
);


-- Dave's ad
SELECT as_upsert_objectstorage(
'c3dd7b47-1bba-4916-8a6a-8b5f2b50ba88', -- object ID
59.9139, -- latitude (near Saint Petersburg)
30.3341, -- longitude (near Saint Petersburg)
'ad', -- object type
'{"description": "Vintage camera, working condition", "ad_type": "exchange", "picture_url": "https://example.com/pictures/camera.jpg"}' -- object data in JSON format
);

SELECT as_upsert_activity(
gen_random_uuid(), -- activity ID
'post', -- verb
'34f6c0a5-5d5e-463f-a2cf-11b7529a92a1', -- actor ID (Dave)
'c3dd7b47-1bba-4916-8a6a-8b5f2b50ba88', -- object ID (Dave's ad)
NULL, -- target ID (no target)
59.9139, -- latitude (near Saint Petersburg)
30.3341 -- longitude (near Saint Petersburg)
);

-- Eve's ad
SELECT as_upsert_objectstorage(
'3453f3c1-296d-47a5-a5a5-f5db5ed3f3b3', -- object ID
59.9375, -- latitude (near Saint Petersburg)
30.3141, -- longitude (near Saint Petersburg)
'ad', -- object type
'{"description": "Plants, various types", "ad_type": "want", "picture_url": "https://example.com/pictures/plants.jpg"}' -- object data in JSON format
);

SELECT as_upsert_activity(
gen_random_uuid(), -- activity ID
'post', -- verb
'8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54', -- actor ID (Eve)
'3453f3c1-296d-47a5-a5a5-f5db5ed3f3b3', -- object ID (Eve's ad)
NULL, -- target ID (no target)
59.9375, -- latitude (near Saint Petersburg)
30.3141 -- longitude (near Saint Petersburg)
);

-- Alice's ad
SELECT as_upsert_objectstorage(
    'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c02', -- new object ID for Alice's ad
    59.9311, -- latitude
    30.3609, -- longitude
    'ad', -- object type
    '{"description": "Used bicycle, good condition", "ad_type": "sell", "picture_url": "https://example.com/pictures/bicycle.jpg"}' -- ad data in JSON format
);

SELECT as_upsert_activity(
    gen_random_uuid(), -- activity ID
    'post', -- verb
    'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', -- actor ID (Alice)
    'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c02', -- object ID (Alice's ad)
    NULL, -- target ID (no target)
    59.9311, -- latitude
    30.3609 -- longitude
);
-- Charly's ad 
SELECT as_upsert_objectstorage(
    '99875f15-49ee-4e6d-b356-cbab4f4e4a4d', -- new object ID for Charlie's ad
    59.9375, -- latitude
    30.3086, -- longitude
    'ad', -- object type
    '{"description": "Books, various genres", "ad_type": "sell", "picture_url": "https://example.com/pictures/books.jpg"}' -- ad data in JSON format
);

SELECT as_upsert_activity(
    gen_random_uuid(), -- activity ID
    'post', -- verb
    '99875f15-49ee-4e6d-b356-cbab4f4e4a4c', -- actor ID (Charlie)
    '99875f15-49ee-4e6d-b356-cbab4f4e4a4d', -- object ID (Charlie's ad)
    NULL, -- target ID (no target)
    59.9428, -- latitude
    30.3071 -- longitude
);
-- Bob's ad
SELECT as_upsert_objectstorage(
    'cc7ebda2-019c-4387-925c-352f7e1f0b12', -- new object ID for Bob's ad
    59.9428, -- latitude
    30.3071, -- longitude
    'ad', -- object type
    '{"description": "Vintage record player, needs repair", "ad_type": "exchange", "picture_url": "https://example.com/pictures/record_player.jpg"}' -- ad data in JSON format
);
SELECT as_upsert_activity(
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
SELECT as_follow_user('8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54', '8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54');
SELECT as_follow_user('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', 'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01');
-- Follow Eve and Alice to Bob, Charlie, and Dave
SELECT as_follow_user('8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54', 'cc7ebda2-019c-4387-925c-352f7e1f0b10');
SELECT as_follow_user('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', 'cc7ebda2-019c-4387-925c-352f7e1f0b10');
SELECT as_follow_user('8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54', '99875f15-49ee-4e6d-b356-cbab4f4e4a4c');
SELECT as_follow_user('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', '99875f15-49ee-4e6d-b356-cbab4f4e4a4c');
SELECT as_follow_user('8d7685d5-5b1f-4a7a-835e-b89e7d3a3b54', '34f6c0a5-5d5e-463f-a2cf-11b7529a92a1');
SELECT as_follow_user('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', '34f6c0a5-5d5e-463f-a2cf-11b7529a92a1');
-- Follow data


--Examples

--Get the alice followees
SELECT as_get_following_ids('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01')

--Get the activities of alice followees as a table
SELECT * FROM as_get_activities_by_following(1, 2, ARRAY(SELECT as_get_following_ids('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01')));

--Get the activities of alice followees as a table and select only the json data of the activity
SELECT self_data FROM as_get_activities_by_following(1, 2, ARRAY(SELECT as_get_following_ids('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01')));

--Get the activities of alice followees as a table and select only the json data of the activity and create an aggregated json  https://www.postgresql.org/docs/9.5/functions-aggregate.html
SELECT json_agg(self_data) FROM as_get_activities_by_following(1, 10, ARRAY(SELECT as_get_following_ids('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01')));

--Get the activities of alice followees as a json array
SELECT * FROM as_get_activities_by_following_as_json(1, 2, 'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01');

--Will return all the activities within 4000 radius of the specified lat and lon 
SELECT as_get_activities_by_distance_as_json(59.9343, 30.3351, 5000, 1, 10);

--Create query
SELECT * FROM as_get_objects_by_criteria_query('user', '2022-01-01', NULL, '"name" = "Bob"', 1, 10, '59.942800,30.307100,1000');


SELECT * FROM as_get_objects_by_criteria_query_parameters(
    'user', 
    '2022-01-01', 
    NULL, 
    '{"name": "Bob"}', 
    1, 
    10, 
    '59.942800,30.307100,1000'
);