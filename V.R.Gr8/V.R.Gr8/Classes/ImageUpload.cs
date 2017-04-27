using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;

namespace V.R.Gr8.Classes {
    public static class ImageUpload {

        //public static string PostItem() {

        //    HttpClient client = new HttpClient();
        //    //http://vrgr820170426094509.azurewebsites.net/api/image/5
        //    client.BaseAddress = new Uri("http://WKNAWAF:3947/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    string responseContent = string.Empty; ;
        //    //using (var client = CreateClient()) {
        //    //var da = new ByteArrayContent(item);

        //    try {
        //        //var multi = new MultipartContent();
        //        //multi.Add(da);
        //        var response = client.GetAsync("api/image/5").Result;
        //        //var response = await client.PostAsync("api/image", multi);
        //        responseContent = response.Content.ReadAsStringAsync().Result;

        //    } catch (Exception ex) {
        //        Console.Write(ex.Message);
        //    }
        //    //}
        //    return responseContent;
        //}


        //public static  string PostItem(byte[] item) {

        //    HttpClient client = new HttpClient();
        //    //http://vrgr820170426094509.azurewebsites.net/api/image/5
        //    client.BaseAddress = new Uri("http://www.bbc.co.uk");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //string responseContent = string.Empty; ;
        //    //using (var client = CreateClient()) {
        //        var da = new ByteArrayContent(item);

        //        try {
        //            //var multi = new MultipartContent();
        //            //multi.Add(da);
        //        var response = client.GetAsync("").ContinueWith(task => {
        //            var content = task.Result;

        //        });
        //            //client.GetAsync()
        //            //response.RunSynchronously();
        //            //var x = response.Result;

        //            //var response = await client.PostAsync("api/image", multi);
        //            //responseContent = x.Content.ReadAsStringAsync().Result;

        //        } catch (Exception ex) {
        //            Console.Write(ex.Message);
        //        }
        //    //}
        //    return "";
        //}

        public static async void PostItem(byte[] item) {
            HttpClient client = new HttpClient();            
            //http://vrgr820170426094509.azurewebsites.net/api/image/5
            client.BaseAddress = new Uri("http://www.bbc.co.uk");
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //string responseContent = string.Empty; ;
            //using (var client = CreateClient()) {
            var da = new ByteArrayContent(item); try {
                using (HttpResponseMessage response = await client.GetAsync(""))
                using (HttpContent content = response.Content) {
                    string result = await content.ReadAsStringAsync();
                    if (result != null) {
                        Console.WriteLine(result.Substring(0, 50) + "...");
                    }
                }
            } catch (Exception ex) {
                Console.Write(ex.Message);
            }
        }

    }
}