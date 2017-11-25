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

namespace TekorMobil
{
    [Application]
    class TekorApplication : Application
    {
        private string email;
        private string password;
        public string Token { get => password; set => password = value; }
        public string Email { get => email; set => email = value; }

        public TekorApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            email = "";
            password = "";
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
    }
}