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

        public static void Test() {
            HttpClient client;
            client = new HttpClient();

            /*client.GetAsync("")*/

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }

        public static void SetUp(string username, string password) {
            var authData = string.Format("{0}:{1}", username, password);
            authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

    }
}