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

namespace HappyPandaXDroid.CustomViews
{
    public class ProgressDialog :Android.Support.V4.App.DialogFragment
    {
        AlertDialog.Builder builder;
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            if (builder == null)
                CreateDialogBuilder();
            AlertDialog dialog = builder.Create();
            return dialog;
        }

        void CreateDialogBuilder()
        {
            builder = new AlertDialog.Builder(Activity);

            LayoutInflater inflater = Activity.LayoutInflater;
            View deleteDialog = inflater.Inflate(Resource.Layout.ProgressDialogLayout, null);
            builder.SetView(deleteDialog);
        }

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            try
            {
                if (builder == null)
                    CreateDialogBuilder();
            }
            catch (Java.Lang.ClassCastException e)
            {
                // The activity doesn't implement the interface, throw exception
                throw new Java.Lang.ClassCastException(context.ToString()
                        + " must implement NoticeDialogListener");
            }
        }
    }
}