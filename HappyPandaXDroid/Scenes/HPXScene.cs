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

namespace HappyPandaXDroid.Scenes
{
    public abstract class HPXScene : Scene
    {
        public abstract void OnConfigurationChanged(Configuration newConfig);
        
        public HPXScene()
        {
            SetWillRetainView(true);
        }

    }
}