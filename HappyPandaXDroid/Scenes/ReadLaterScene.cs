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
using HappyPandaXDroid.Core;

namespace HappyPandaXDroid.Scenes
{
    class ReadLaterScene : ViewScene
    {
        public override Core.Gallery.ItemType ItemType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReadLaterScene(string title, string query) : base(title, query)
        {

        }

    }
}