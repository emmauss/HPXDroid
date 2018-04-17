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
    class TrashScene : ViewScene
    {
        public override Core.Gallery.ViewType ViewType => Core.Gallery.ViewType.Trash;

        public TrashScene(string title, string query) : base(title, query)
        {

        }
    }
}