using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Ultra.ActivityStream.Contracts;

namespace Ultra.ActivityStream.Client
{
    public class ActivityStreamClient
    {
        private readonly HttpClient _httpClient;

        public ActivityStreamClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri("https://example.com/"); // replace with the actual base address of your API
        }
        public static Stream GenerateRandomStream(int length)
        {
            var stream = new MemoryStream();

            var buffer = new byte[length];
            new Random().NextBytes(buffer);

            stream.Write(buffer, 0, buffer.Length);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
        public async Task UploadFilesAsync(StreamObject actor, StreamObject obj, StreamObject target, double latitude, double longitude, List<Stream> files)
        {
            using (var content = new MultipartFormDataContent())
            {
                // Add the actor, obj, target, latitude, and longitude as form data
                content.Add(new StringContent(JsonConvert.SerializeObject(actor)), "Actor");
                content.Add(new StringContent(JsonConvert.SerializeObject(obj)), "Object");
                content.Add(new StringContent(JsonConvert.SerializeObject(target)), "Target");
                content.Add(new StringContent(latitude.ToString()), "Latitude");
                content.Add(new StringContent(longitude.ToString()), "Longitude");

                files.Add(null);
                // Add the files as binary data
                foreach (var file in files)
                {
                    //var fileContent = new StreamContent(file);
                    var fileContent = new StreamContent(GenerateRandomStream(100));
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "files",
                        FileName = Guid.NewGuid().ToString() // generate a unique filename for each file
                    };
                    content.Add(fileContent);
                }

                //var GetResponse= await _httpClient.GetAsync("simple/get");
                //GetResponse.EnsureSuccessStatusCode(); 
                try
                {
                    var response = await _httpClient.PostAsync("files/upload", content);
                    //var response = await _httpClient.PostAsync("activityStream/upload", content);
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