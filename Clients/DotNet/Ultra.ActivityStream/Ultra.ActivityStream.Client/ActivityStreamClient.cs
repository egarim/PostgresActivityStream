﻿using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Ultra.ActivityStream.Contracts;
using Ultra.ActivityStream.Contracts.Operations;

namespace Ultra.ActivityStream.Client
{
    public class ActivityStreamClient: IFollowObjectFromIds,IUnfollowFromIds
    {
        private readonly HttpClient _httpClient;

        public ActivityStreamClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri("https://example.com/"); // replace with the actual base address of your API
        }
        public ActivityStreamClient(string BaseAddress)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseAddress);
        }
        public async Task CreateObjectAsync(StreamObject obj, List<Stream> files)
        {
            using (var content = new MultipartFormDataContent())
            {
                // Add the actor, obj, target, latitude, and longitude as form data

                content.Add(new StringContent(JsonConvert.SerializeObject(obj)), "Obj");


                // Add the files as binary data
                foreach (var file in files)
                {
                    var fileContent = new StreamContent(file);

                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "files",
                        FileName = Guid.NewGuid().ToString() // generate a unique filename for each file
                    };
                    content.Add(fileContent);
                }


                try
                {
                    var response = await _httpClient.PostAsync("ActivityStream/CreateObject", content);
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    throw;
                }
            }

        }
        public async Task CreateActivity(StreamObject actor, StreamObject obj, StreamObject target, double latitude, double longitude, List<Stream> files)
        {
            using (var content = new MultipartFormDataContent())
            {
                // Add the actor, obj, target, latitude, and longitude as form data
                content.Add(new StringContent(JsonConvert.SerializeObject(actor)), "Actor");
                content.Add(new StringContent(JsonConvert.SerializeObject(obj)), "Object");
                content.Add(new StringContent(JsonConvert.SerializeObject(target)), "Target");
                content.Add(new StringContent(latitude.ToString()), "Latitude");
                content.Add(new StringContent(longitude.ToString()), "Longitude");


                // Add the files as binary data
                foreach (var file in files)
                {
                    var fileContent = new StreamContent(file);

                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "files",
                        FileName = Guid.NewGuid().ToString() // generate a unique filename for each file
                    };
                    content.Add(fileContent);
                }


                try
                {
                    var response = await _httpClient.PostAsync("ActivityStream/CreateActivity", content);
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    throw;
                }



            }
        }

        public async Task FollowObjectFromIdsAsync(Guid Follower, Guid Followee)
        {
            using (var content = new MultipartFormDataContent())
            {
                // Add the actor, obj, target, latitude, and longitude as form data
                content.Add(new StringContent(Follower.ToString()), nameof(Follower));
                content.Add(new StringContent(Followee.ToString()), nameof(Followee));

                try
                {
                    var response = await _httpClient.PostAsync("ActivityStream/FollowObjectFromIdsAsync", content);
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    throw;
                }
            }
        }

        public async Task UnfollowFromIdsAsync(Guid Follower, Guid Followee)
        {
            using (var content = new MultipartFormDataContent())
            {
                // Add the actor, obj, target, latitude, and longitude as form data
                content.Add(new StringContent(Follower.ToString()), nameof(Follower));
                content.Add(new StringContent(Followee.ToString()), nameof(Followee));

                try
                {
                    var response = await _httpClient.PostAsync("ActivityStream/UnfollowFromIdsAsync", content);
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    throw;
                }
            }
        }
    }
}