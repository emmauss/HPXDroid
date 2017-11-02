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
    public class OptionDialogPreference : Android.Preferences.DialogPreference
    {
        public event EventHandler OnPositiveClick;
        public OptionDialogPreference(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public OptionDialogPreference(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {

        }

        public override void OnClick(IDialogInterface dialog, int which)
        {

            OnPositiveClick(this, EventArgs.Empty);
            base.OnClick(dialog, which);
        }
    }
}