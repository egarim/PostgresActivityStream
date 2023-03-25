-- You should run this test in a clean activity stream database


--alice follow alice 

SELECT follow_user('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', 'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01');

--Get ALice followees

SELECT get_following_ids('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01')

--Get activities of Alice followees (will return nothing at this moment)

SELECT * FROM get_activities_by_following_as_json(1, 10, 'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01');


--create bobs ad and write the activity

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

--Alice start following bob 
SELECT follow_user('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', 'cc7ebda2-019c-4387-925c-352f7e1f0b10');


--Get activities of Alice followees (will return one activity from bob)

SELECT * FROM get_activities_by_following_as_json(1, 10, 'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01');

--Alice stop following bob 
SELECT unfollow_user('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', 'cc7ebda2-019c-4387-925c-352f7e1f0b10');

--Get activities of Alice followees (will not return anything since alice stop following bob)

SELECT * FROM get_activities_by_following_as_json(1, 10, 'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01');

--bob creates a second ad and write the activity


SELECT upsert_objectstorage(
'f6c7599e-8161-4d54-82ec-faa13bb8c333', -- object ID
59.9428, -- latitude (near Saint Petersburg)
30.3071, -- longitude (near Saint Petersburg)
'ad', -- object type
'{"description": "House plant", "ad_type": "sale", "picture_url": "https://example.com/pictures/plant.jpg"}' -- object data in JSON format
);

SELECT upsert_activity(
gen_random_uuid(), -- activity ID
'post', -- verb
'cc7ebda2-019c-4387-925c-352f7e1f0b10', -- actor ID (Bob)
'f6c7599e-8161-4d54-82ec-faa13bb8c333', -- object ID (Bob's ad)
NULL, -- target ID (no target)
59.9428, -- latitude (near Saint Petersburg)
30.3071 -- longitude (near Saint Petersburg)
);

--Alice start following bob again
SELECT follow_user('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', 'cc7ebda2-019c-4387-925c-352f7e1f0b10');

--Get activities of Alice followees (will return 2 activities from bob) 
--first will return the new ad about the plant and second the ad about the Vintage bicycle

SELECT * FROM get_activities_by_following_as_json(1, 10, 'b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01');


--Will retur all the activities within 4000 radius of the specified lat and lon 
SELECT get_activities_by_distance_as_json(59.9343, 30.3351, 4000, 1, 10);

--Will retur alice followees as a data table
SELECT *
FROM get_followees_by_object_id('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', 1, 2);

--Will retur alice followees as JSON
SELECT *
FROM get_followees_by_object_id_as_json('b8dcbf13-cb01-4a35-93d5-5a5f5a2f6c01', 1, 2);

--experimental
SELECT as_get_objects_by_criteria_query_parameters(
'object_type1',
'2022-01-01 00:00:00+00',
'2022-12-31 23:59:59+00',
'object_data_column = value1',
1,
10,
'0,0,100',
ARRAY['7b52a0b8-3d0e-4c2c-9f1b-8ec6f17ed6c9'::uuid, '4b52a0b8-3d0e-4c2c-9f1b-8ec6f17ed6c9'::uuid, '2b52a0b8-3d0e-4c2c-9f1b-8ec6f17ed6c9'::uuid],
'OR'
);
