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
using System.Net.Http;
using System.Net.Http.Headers;

namespace TekorMobil
{
    class RestService
    {
        static string authHeaderValue;

        public static void test() {
            HttpClient client;
            client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }

        public static void setUp(string username, string password) {
            var authData = string.Format("{0}:{1}", username, password);
            authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
        }
    }
}