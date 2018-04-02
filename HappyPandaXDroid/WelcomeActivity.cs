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
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace HappyPandaXDroid
{
    [Activity(Label = "WelcomeActivity" , NoHistory =true, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation
        | Android.Content.PM.ConfigChanges.ScreenSize, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class WelcomeActivity : AppCompatActivity
    {
        bool LoggedIn = false;
        TextView Username;
        TextView Password;
        TextView ServerIP;
        TextView ServerPort;
        TextView WebPort;
        TableRow UserNameRow;
        TableRow PasswordRow;
        CheckBox GuestCheckBox;
        Button   LoginButton;
        CardView LoginCard;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.WelcomeLayout);
            Initialize();
        }

        protected override void OnDestroy()
        {
            if (LoggedIn)
                Core.App.Settings.IsFirstRun = false;
            base.OnDestroy();
        }

        void Initialize()
        {
            LoginButton = FindViewById<Button>(Resource.Id.loginButton);
            Username = FindViewById<TextView>(Resource.Id.usernameBox);
            Password = FindViewById<TextView>(Resource.Id.passBox);
            ServerIP = FindViewById<TextView>(Resource.Id.ipBox);
            ServerPort = FindViewById<TextView>(Resource.Id.portBox);
            WebPort = FindViewById<TextView>(Resource.Id.webPortBox);
            UserNameRow = FindViewById<TableRow>(Resource.Id.tableRowWUsername);
            PasswordRow = FindViewById<TableRow>(Resource.Id.tableRowWPassword);
            GuestCheckBox = FindViewById<CheckBox>(Resource.Id.guestCheckBox);
            ServerIP.Text = Core.App.Settings.Server_IP;
            ServerPort.Text = Core.App.Settings.Server_Port;
            WebPort.Text = Core.App.Settings.WebClient_Port;
            Username.Text = Core.App.Settings.Username;
            LoginCard = FindViewById<CardView>(Resource.Id.loginCard);
            LoginButton.Click += LoginButton_Click;
            GuestCheckBox.CheckedChange += GuestCheckBox_CheckedChange;
        }

        private void GuestCheckBox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            UserNameRow.Enabled = !e.IsChecked;
            PasswordRow.Enabled = !e.IsChecked;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(ServerIP.Text) || string.IsNullOrWhiteSpace(ServerIP.Text))
            {
                ServerIP.RequestFocus();
                ServerIP.Error= "Please enter the HPX server address";
            }
            else if (string.IsNullOrEmpty(ServerPort.Text) || string.IsNullOrWhiteSpace(ServerPort.Text) 
                || !int.TryParse(ServerPort.Text,out int port))
            {
                ServerPort.RequestFocus();
                ServerPort.Error = "Please enter the HPX server port";
            }
            else if (string.IsNullOrEmpty(WebPort.Text) || string.IsNullOrWhiteSpace(WebPort.Text)
                || !int.TryParse(ServerPort.Text, out int webport))
            {
                WebPort.RequestFocus();
                WebPort.Error = "Please enter the HPX webport";
            }
            else if (GuestCheckBox.Checked)
            {
                Core.App.Settings.Server_IP = ServerIP.Text;
                Core.App.Settings.Server_Port = ServerPort.Text;
                Core.App.Settings.WebClient_Port = WebPort.Text;
                Core.App.Settings.IsGuest = true;
                LoginCard.Enabled = false;
                if (Core.Net.IsServerReachable())
                {
                    if (Core.Net.Reconnect())
                    {
                        Toast.MakeText(this, "Authenticated. Loading App", ToastLength.Long).Show();
                        LoadMainApp();
                        return;
                    }
                    else
                    {
                        Toast.MakeText(this, "Server Not Found or Access Restricted. Please make sure that server is running and guests" +
                            " are allowed", ToastLength.Long).Show();
                    }
                }
                else
                    Toast.MakeText(this, "Server Not Reachable. Please make sure that the target machine is active and can be reached",
                        ToastLength.Long).Show();
                Core.App.Settings.IsGuest = false;
                LoginCard.Enabled = true;
                return;
            }
            else if (string.IsNullOrEmpty(Username.Text) || string.IsNullOrWhiteSpace(Username.Text))
            {
                Username.RequestFocus();
                Username.Error = "Please enter your username";
            }
            else if (string.IsNullOrEmpty(Username.Text) || string.IsNullOrWhiteSpace(Username.Text))
            {
                Password.RequestFocus();
                Password.Error = "Please enter your password";
            }
            else
            {
                Core.App.Settings.Server_IP = ServerIP.Text;
                Core.App.Settings.Server_Port = ServerPort.Text;
                Core.App.Settings.WebClient_Port = WebPort.Text;
                Core.App.Settings.Username = Username.Text;
                Core.App.Settings.Password = Password.Text;
                Core.App.Settings.IsGuest = false;
                LoginCard.Enabled = false;
                if (Core.Net.IsServerReachable())
                {
                    if (Core.Net.Reconnect())
                    {
                        Toast.MakeText(this, "Authenticated. Loading App", ToastLength.Long).Show();
                        LoadMainApp();
                        return;
                    }
                    else
                    {
                        Toast.MakeText(this, "Server Not Found or Access Restricted. Please make sure that server is running " +
                            "and login credentials are correct", ToastLength.Long).Show();
                    }
                }
                else
                    Toast.MakeText(this, "Server Not Reachable. Please make sure that the target machine is active and can be reached",
                        ToastLength.Long).Show();
                LoginCard.Enabled = true;
                return;
            }

        }

        void LoadMainApp()
        {
            var intent = new Intent(this, typeof(HPXSceneActivity));
            intent.PutExtra("connected", Core.Net.Connected);
            intent.PutExtra("query", string.Empty);
            StartActivity(intent);
            Core.App.Settings.IsFirstRun = false;
        }
    }
}