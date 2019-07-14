using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Com.Hippo.Drawerlayout;

namespace HappyPandaXDroid.Custom_Views
{
    public class DrawerContent : LinearLayout, IDrawerLayoutChild
    {
        public DrawerContent(Context context) : base(context)
        {
        }

        public DrawerContent(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }
        public int AdditionalBottomMargin { get; private set; }

        public int AdditionalTopMargin { get; private set; }

        public void OnGetWindowPadding(int p0, int p1)
        {
            AdditionalTopMargin = p0;
            AdditionalBottomMargin = p1;
        }
    }
}