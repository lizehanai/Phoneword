﻿using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;

namespace Phoneword
{
    [Activity(Label = "Phone Word", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            //Get the UI controls from the loaded layout;
            EditText phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            Button translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            Button callButton = FindViewById<Button>(Resource.Id.CallButton);

            //Disable the "Call" button
            callButton.Enabled = false;

            //Add code to translate number
            string translateNumber = string.Empty;

            translateButton.Click += (object sender, EventArgs e) =>
              {
                //Translate user's alphannumeric phone number to numeric
                translateNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
                  if (String.IsNullOrWhiteSpace(translateNumber))
                  {
                      callButton.Text = "Call";
                      callButton.Enabled = false;
                  }
                  else
                  {
                      callButton.Text = "Call" + translateNumber;
                      callButton.Enabled = true;
                  }
              };
            callButton.Click += (object sender, EventArgs e) =>
              {
                //On "Call" button click, try to dial phone number.
                var callDialog = new AlertDialog.Builder(this);
                  callDialog.SetMessage("Call" + translateNumber + "?");
                  callDialog.SetNegativeButton("Call", delegate
                  {
                    //Create intent to dial phone
                    var calllntent = new Intent(Intent.ActionCall);
                    calllntent.SetData(Android.Net.Uri.Parse("tel:" + translateNumber));
                    StartActivity(calllntent);
                  });
                  callDialog.SetNegativeButton("Cancel", delegate { });

                //Show the alert dialog to the user and wait for respoonse.
                callDialog.Show();
              };

        }
    }
}

