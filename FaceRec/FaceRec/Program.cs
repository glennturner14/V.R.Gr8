using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace CSHttpClientSample
{


    static class Program
    {
        private enum RequestType
        {
            Get,
            Post,
            Delete
        }

        static string apikey = "ddba18c532a94edcaa8226ee92155d43";
        static string ClientGroupId = "1";
        static string EmployeeGroupId = "2";
        static string baseFolder = @"C:\Temp\VRGr8";
        static string Find_Folder = @"FindList";
        static string Training_Folder = @"TrainingList";

        static void Main()
        {
            string groupId = EmployeeGroupId;
            bool startFromScratch = true;

            ////Create two sample groups...
            //string json = "{\"name\":\"My Clients\", \"userData\":\"Client Lists.\"}";

            //createPersonGroup(ClientGroupId, json);
            //createPersonGroup(EmployeeGroupId, json2);

            Console.WriteLine("Starting...");

            if (startFromScratch)
            {
                Console.WriteLine("Delete PersonGroup...");
                DeleteGroup(groupId);

                string json2 = "{\"name\":\"My Employees\", \"userData\":\"Employees Lists.\"}";
                createPersonGroup(EmployeeGroupId, json2);

                Console.WriteLine("Loading PersonGroup...");
                LoadPersonGroup(groupId);

                Console.WriteLine("Training PersonGroup...");
                TrainGroup(groupId);
            }

            Console.WriteLine("Check PersonGroup Training Status...");
            GetGroupTrainingStatus(groupId);

            string findPath = GetFindPicture(Path.Combine(baseFolder, Find_Folder));
            string returnList = MakeDetectRequest(findPath).Result;

            dynamic personList = JsonConvert.DeserializeObject(returnList);
            string idArray = string.Empty;
            foreach (var person in personList)
            {
                idArray += "\"" + person.faceId + "\",";
            }
            idArray = idArray.Substring(0, idArray.Length - 1);
            RecogniseFace(groupId, idArray);
            Console.WriteLine("\n\n\nWait for the result below, then hit ENTER to exit...\n\n\n");

            //Who do we need to delete???
            //DeleteAPersonInAGroup(EmployeeGroupId, "6d15999b-1b57-41e0-967b-8774ac8a55af");
            //DeleteAPersonInAGroup(EmployeeGroupId, "eb055a0b-4e5e-42a2-956b-8ef48cf3a206");

            //string id = CreatPerson(ClientGroupId, "Nawaf Baraya", "VIP Client").Result;
            //string[] imageList = new string[] {
            //    @"E:\Users\nawaf.baraya\Downloads\2017-03-29 13.23.36.jpg",
            //    @"E:\Users\nawaf.baraya\Downloads\2017-03-29 15.13.17.jpg",
            //    @"E:\Users\nawaf.baraya\Downloads\2017-04-03 22.50.46.jpg",
            //    @"E:\Users\nawaf.baraya\Downloads\IMG_20161028_150357.jpg" };

            //foreach (var image in imageList) {
            //    AddFace(ClientGroupId, id, image);
            //}

            //string returnList = MakeDetectRequest(@"C:\Temp\VRGr8\Alan Reed\headshot3.jpg").Result;

            ////Create PeteM
            //string id = CreatPerson(ClientGroupId, "Pete MUlvaney", "VIP Client").Result;
            //string[] imageList = new string[] {
            //    @"C:\Temp\Nawaf\Archive-1d81\PetM1.jpg",
            //    @"C:\Temp\Nawaf\Archive-1d81\PeteM2.jpg",
            //    @"C:\Temp\Nawaf\Archive-1d81\PeteM3.jpg",
            //    @"C:\Temp\Nawaf\Archive-1d81\PeteM4.jpg" };

            //foreach (var image in imageList)
            //{
            //    AddFace(ClientGroupId, id, image);
            //}

            //MakeFaceListRequest();

            ////Detect PM
            //string returnList = MakeDetectRequest(@"C:\Temp\Nawaf\Archive-1d81\PeteM1.jpg").Result;

            ////Create PeteM
            //string id = CreatPerson(ClientGroupId, "Pascal Meyer", "VIP Client").Result;
            //string[] imageList = new string[] {
            //    @"C:\Temp\Nawaf\Archive-1d81\PascalM1.jpg",
            //    @"C:\Temp\Nawaf\Archive-1d81\PascalM2.jpg",
            //    @"C:\Temp\Nawaf\Archive-1d81\PascalM3.jpg",
            //    @"C:\Temp\Nawaf\Archive-1d81\PascalM4.jpg" };

            //foreach (var image in imageList)
            //{
            //    AddFace(ClientGroupId, id, image);
            //}

            //string id = CreatPerson(EmployeeGroupId, "PeteM", "VIP Client").Result;
            //string[] imageList = new string[] {
            //    @"C:\Temp\Nawaf\Archive-1d81\PeteM1.jpg",
            //    @"C:\Temp\Nawaf\Archive-1d81\PeteM2.jpg",
            //    @"C:\Temp\Nawaf\Archive-1d81\PeteM3.jpg",
            //    @"C:\Temp\Nawaf\Archive-1d81\PeteM4.jpg" };

            //foreach (var image in imageList)
            //{
            //    AddFace(EmployeeGroupId, id, image);
            //}

            //////Write out persons list detail
            //WritePersonsInGroup(EmployeeGroupId);

            ////Get the group training status...
            //GetGroupTrainingStatus(EmployeeGroupId);

            ////Get the group training status...
            //TrainGroup(EmployeeGroupId);

            //MakeFaceListRequest();

            ////Detect PM
            //string returnList = MakeDetectRequest(@"C:\Temp\Nawaf\Archive-1d81\PeteM1.jpg").Result;

            //dynamic personList = JsonConvert.DeserializeObject(returnList);
            //string idArray = string.Empty;
            //foreach (var person in personList)
            //{
            //    idArray += "\"" + person.faceId + "\",";
            //}
            //idArray = idArray.Substring(0, idArray.Length - 1);
            //RecogniseFace(groupId, idArray);
            //Console.WriteLine("\n\n\nWait for the result below, then hit ENTER to exit...\n\n\n");

            Console.ReadLine();
        }

        private static string GetFindPicture(string path)
        {
            string firstPicturePath = null;

            if (!string.IsNullOrEmpty(path))
            {
                DirectoryInfo directory = new DirectoryInfo(path);

                //@"C:\Temp\VRGr8Unknown\GlennTUnknown.jpg"
                FileInfo[] files = directory.GetFiles("*.jpg");
                if (files != null)
                {
                    firstPicturePath = files[0].FullName;
                    Console.WriteLine("Found picture in '{0}' path", Find_Folder);

                }
            }

            string prefix = (firstPicturePath != null) ? prefix = "Found" : "Cannot find";

            Console.WriteLine("{0} picture in '{1}' path", prefix, Find_Folder);

            return firstPicturePath;
        }

        private static void LoadPersonGroup(string groupId)
        {
            //IEnumerable<string> listPersons = (IEnumerable<string>)Directory.EnumerateDirectories(baseFolder);
            DirectoryInfo di = new DirectoryInfo(Path.Combine(baseFolder, Training_Folder));
            DirectoryInfo[] directories = di.GetDirectories();
            //try
            //{

            //}
            //catch (AggregateException ex )
            //{

            //    throw;
            //}

            Dictionary<string, string> personsInGroup = GetPersonsInGroup(groupId).Result;

            foreach (DirectoryInfo directory in directories)
            {
                string personName = directory.Name;
                if (personsInGroup != null && personsInGroup.ContainsKey(personName)) {
                    string persionId = personsInGroup[personName];
                    //Delete the person...
                    DeleteAPersonInAGroup(groupId, persionId);
                }
                
                AddPersonIntoGroup(groupId, personName, directory);
            }
        }



        private static void AddPersonIntoGroup(string groupId, string personName, DirectoryInfo directoryInfo) {
            string details = string.Empty;
            List<string> photos = new List<string>();

            foreach (FileInfo info in directoryInfo.GetFiles()) {
                switch (info.Extension)
                {
                    case ".txt":
                        
                        FileStream fs = info.OpenRead();
                        fs.Position = 0;
                        using (StreamReader reader = new StreamReader(fs)) {
                            details = reader.ReadToEnd();
                        }
                        break;
                    case ".jpg":
                        photos.Add(info.FullName);
                        break;  
                    default:
                        break;
                }
            }

            string id = CreatPerson(groupId, personName, details);

            foreach (string photo in photos) {
                AddFace(groupId, id, photo);
            }

        }

        //private static bool PersonInAGroup(string direcory)
        //{




        //}

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

            //HttpResponseMessage response = await client.PutAsync(uri, content);
            HttpResponseMessage response = client.PutAsync(uri, content).Result;

            // If the group was created successfully, you'll see "OK".
            // Otherwise, if a group with the same personGroupId has been created before, you'll see "Conflict".
            Console.WriteLine("Response status: " + response.StatusCode);
        }


        static string CreatPerson(string groupId,string PersonName, string details)
        {
            string personId = null;

            try
            {
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apikey);

                var uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + groupId + "/persons?" + queryString;

                HttpResponseMessage response;

                // Request body
                byte[] byteData = Encoding.UTF8.GetBytes("{\"name\":\"" + PersonName + "\", \"userData\":\"" + details + "\"}");
                string responseContent;

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    bool foundId = false;
                    while (!foundId)
                    {
                        //response = await client.PostAsync(uri, content);
                        response = client.PostAsync(uri, content).Result;
                        responseContent = response.Content.ReadAsStringAsync().Result;
                        dynamic person = JsonConvert.DeserializeObject(responseContent);
                        if (person.Count != 0)
                        {
                            personId = person.personId;
                            Console.WriteLine(responseContent);
                            foundId = true;
                        }
                        else
                        {
                            Console.WriteLine("Going to sleep for a bit...");
                            Thread.Sleep(26000);
                        }
                    }
                }


            }
            catch (AggregateException ae)
            {

              //  throw;
            }

            return personId;

        }

        //tony - 306944be-0b8e-4e37-ad3e-75a478a6d434 
        //nawaf - 0eba6194-6db4-4525-9bdd-3310de4b6bf0 
        //PeteM - ???
        //Pascal - baf73b88-d53a-4df7-a6cd-ff66d34546df


        static async void AddFace(string id, string PersionId,string filename)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apikey);

            var uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + id + "/persons/" + PersionId+ "/persistedFaces";

            HttpResponseMessage response;
            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(filename);

           string responseContent;
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //response = await client.PostAsync(uri, content);
                response = client.PostAsync(uri, content).Result;
                responseContent = response.Content.ReadAsStringAsync().Result;
            }
        }


        static async void RecogniseFace(string groupId, string idList)
        {
            //string body =
            //    @"{
            //    ""personGroupId"":""1"",
            //    ""faceIds"":[" + idList
            //    + @"], ""maxNumOfCandidatesReturned"":1,""confidenceThreshold"": 0.5}";

            string newgroupId = @"""" + groupId + @"""";
            string body =
                @"{
                ""personGroupId"":" + newgroupId + @",
                ""faceIds"":[" + idList
                + @"], ""maxNumOfCandidatesReturned"":1,""confidenceThreshold"": 0.5}";

            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apikey);

            var uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/identify";
         


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

        static async void MakeFaceListRequest()
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apikey);

            // Request parameters and URI string.
            string uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/facelists?";

            HttpResponseMessage response;
            string responseContent;

            response = await client.GetAsync(uri);
            responseContent = response.Content.ReadAsStringAsync().Result;

            //A peak at the JSON response.
            Console.WriteLine(responseContent);
            
        }

        //Write out persons in PersonGroup (hard coded to client Group1) and associated PersistedFaceIds (against a picture)...
        static async void WritePersonsInGroup(string GroupId)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apikey);

            // Request parameters and URI string.
            string uri = string.Format("https://westus.api.cognitive.microsoft.com/face/v1.0/persongroups/{0}/persons?", GroupId);

            HttpResponseMessage response;
            string responseContent;

            response = await client.GetAsync(uri);
            responseContent = response.Content.ReadAsStringAsync().Result;

            //A peak at the JSON response.
            Console.WriteLine(responseContent);

        }

        //Write out persons in PersonGroup (hard coded to client Group1) and associated PersistedFaceIds (against a picture)...
        static async Task<Dictionary<string, string>> GetPersonsInGroup(string GroupId)
        {

            // Request parameters and URI string.
            string uri = string.Format("https://westus.api.cognitive.microsoft.com/face/v1.0/persongroups/{0}/persons?", GroupId);

            string returnList = await MakeRequest(RequestType.Get, uri);

            dynamic personList = JsonConvert.DeserializeObject(returnList);

            Dictionary<string, string> personsInGroup = null;
            if (personList.Count != 0)
            {
                string idArray = string.Empty;

                personsInGroup = new Dictionary<string, string>();

                foreach (var person in personList)
                {
                    string name = person.name;
                    string persionId = person.persionId;
                    personsInGroup.Add(name, persionId);
                }
            }

            return personsInGroup;

        }


        //Train a PersonGroup ...
        static async void TrainGroup(string GroupId)
        {
            // Request parameters and URI string.
            string uri = string.Format("https://westus.api.cognitive.microsoft.com/face/v1.0/persongroups/{0}/train?", GroupId);

            string response = MakeRequest(RequestType.Post, uri, "{Body}").Result;

            Console.WriteLine(response);

        }

        //Write out persons in PersonGroup (hard coded to client Group1) and associated PersistedFaceIds (against a picture)...
        static async void GetGroupTrainingStatus(string GroupId)
        {
            // Request parameters and URI string.
            string uri = string.Format("https://westus.api.cognitive.microsoft.com/face/v1.0/persongroups/{0}/training?", GroupId);

            string response = MakeRequest(RequestType.Get, uri).Result;

            Console.WriteLine(response);

        }

        //Delete a PersonGroup ...
        static async void DeleteGroup(string GroupId)
        {
            // Request parameters and URI string.
            string uri = string.Format("https://westus.api.cognitive.microsoft.com/face/v1.0/persongroups/{0}?", GroupId);

            string response = MakeRequest(RequestType.Delete, uri).Result;

        }

        //Write out persons in PersonGroup (hard coded to client Group1) and associated PersistedFaceIds (against a picture)...
        static async void DeleteAPersonInAGroup(string GroupId, string personId)
        {
            // Request parameters and URI string.
            string uri= string.Format("https://westus.api.cognitive.microsoft.com/face/v1.0/persongroups/{0}/persons/{1}?", GroupId, personId);

            string response = MakeRequest(RequestType.Delete, uri).Result;

        }

        //Delete request...
        static async Task<string> MakeRequest(RequestType requestType, string uri, string body = null)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apikey);

            HttpResponseMessage response = null;
            string responseContent;

            switch (requestType)
            {
                case RequestType.Get:
                    //response = await client.GetAsync(uri);
                    response = client.GetAsync(uri).Result;
                    break;

                case RequestType.Post:

                    byte[] byteData = Encoding.UTF8.GetBytes(body);

                    using (var content = new ByteArrayContent(byteData))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        //response = await client.PostAsync(uri, content);
                        response = client.PostAsync(uri, content).Result;
                    }

                    break;
                case RequestType.Delete:
                    //response = await client.DeleteAsync(uri);
                    response = client.DeleteAsync(uri).Result;
                    break;
                default:
                    break;
            }

            responseContent = response.Content.ReadAsStringAsync().Result;

            ////A peak at the JSON response.
            //Console.WriteLine(responseContent);

            return responseContent;

        }

    }
}