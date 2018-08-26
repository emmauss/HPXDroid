using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Hippo.Stage;
using HappyPandaXDroid.Core;

namespace HappyPandaXDroid.Scenes
{
    public abstract class HPXScene : Scene
    {
        public RequestToken RequestToken { get; set; }

        public abstract void OnConfigurationChanged(Configuration newConfig);
        
        public HPXScene()
        {
            SetWillRetainView(true);
        }

        protected override void OnDestroyView(View p0)
        {
            RequestToken.IsPaused = false;
            RequestToken = null;
            base.OnDestroyView(p0);
        }
    }
}