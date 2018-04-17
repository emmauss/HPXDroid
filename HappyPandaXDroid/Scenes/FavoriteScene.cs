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
    class FavoriteScene : ViewScene
    {
        public override Core.Gallery.ViewType ViewType => Core.Gallery.ViewType.Favorite;

        public FavoriteScene(string title, string query) : base(title,query)
        {
            
        }
    }
}