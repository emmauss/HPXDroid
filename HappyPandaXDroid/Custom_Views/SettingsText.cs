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

namespace HappyPandaXDroid.Custom_Views
{
    public class SettingsText : LinearLayout
    {
        public SettingsText(Context context) :
            base(context)
        {
            Initialize();
        }

        public SettingsText(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public SettingsText(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
        }
    }
}