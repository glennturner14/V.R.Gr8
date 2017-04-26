using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CSHttpClientSample
{
    static class Program
    {
        static string apikey = "ddba18c532a94edcaa8226ee92155d43";
        static string ClientGroupId = "1";
        static string EmployeeGroupId = "2";


        static void Main()
        {
            Console.Write("Starting...");
            string imageFilePath = @"E:\Users\nawaf.baraya\Downloads\IMG-20170423-WA0007.jpg";
            string json = "{\"name\":\"My Clients\", \"userData\":\"Client Lists.\"}";
            string json2 = "{\"name\":\"My Employees\", \"userData\":\"Employees Lists.\"}";
            //createPersonGroup(ClientGroupId, json);
            //createPersonGroup(EmployeeGroupId, json2);
           
            //string id = CreatPerson(ClientGroupId, "Nawaf Baraya", "VIP Client").Result;
            AddFace(ClientGroupId, id);

            string returnList = MakeDetectRequest(@"E:\Users\nawaf.baraya\Downloads\2017-04-03 22.50.46.jpg").Result;

            dynamic personList = JsonConvert.DeserializeObject(returnList);
            string idArray=string.Empty;
            foreach( var person in personList)
            {
                idArray += "\""+person.faceId+"\",";
            }
            idArray = idArray.Substring(0, idArray.Length - 1);
            RecogniseFace(idArray);
            Console.WriteLine("\n\n\nWait for the result below, then hit ENTER to exit...\n\n\n");
            Console.ReadLine();
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        static async Task<string> MakeDetectRequest(string imageFilePath)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apikey);

            // Request parameters and URI string.
            string queryString = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender";
            string uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/detect?" + queryString;

            HttpResponseMessage response;
            string responseContent;

            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;
            }

            //A peak at the JSON response.
            Console.WriteLine(responseContent);
            return responseContent;
        }

        static async void createPersonGroup(string id,string json)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apikey);

            // Request URI string.
            string uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + id;

            // Here "name" is for display and doesn't have to be unique. Also, "userData" is optional.
           
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PutAsync(uri, content);

            // If the group was created successfully, you'll see "OK".
            // Otherwise, if a group with the same personGroupId has been created before, you'll see "Conflict".
            Console.WriteLine("Response status: " + response.StatusCode);
        }


        static async Task<string> CreatPerson(string id,string PersonName, string details)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apikey);

            var uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/persongroups/"+id+"/persons?" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("{\"name\":\""+ PersonName + "\", \"userData\":\""+ details + "\"}");
            string responseContent;
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;
            }

            Console.WriteLine(responseContent);
            dynamic person = JsonConvert.DeserializeObject(responseContent);
            return person.personId;
          
        }

        //306944be-0b8e-4e37-ad3e-75a478a6d434 tony
        // 0eba6194-6db4-4525-9bdd-3310de4b6bf0 nawaf

        static async void AddFace(string id, string PersionId)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apikey);

            var uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + id + "/persons/" + PersionId+ "/persistedFaces";

            HttpResponseMessage response;
            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(@"E:\Users\nawaf.baraya\Downloads\2017-04-03 22.50.46.jpg");

           string responseContent;
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;
            }
        }


        static async void RecogniseFace(string idList)
        {
            string body =
                @"{
                ""personGroupId"":""1"",
                ""faceIds"":[" + idList
                + @"], ""maxNumOfCandidatesReturned"":1,""confidenceThreshold"": 0.5}";

            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apikey);

            var uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/findsimilars";
         


            byte[] byteData = Encoding.UTF8.GetBytes(body);
            HttpResponseMessage response;
            string responseContent;
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;
            }

            Console.WriteLine(responseContent);
        }
    }
}