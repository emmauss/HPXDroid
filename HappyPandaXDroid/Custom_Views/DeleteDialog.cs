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
using NLog;

namespace HappyPandaXDroid.Custom_Views
{
    public class DeleteDialog : Android.Support.V4.App.DialogFragment
    {
        public bool ShouldTrashed => TrashCheckBox.Checked;
        AlertDialog.Builder builder;
        public CheckBox TrashCheckBox;
        public TextView Summary;
        Scenes.GalleryScene mscene;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            if (builder == null)
                CreateDialogBuilder();
            AlertDialog dialog = builder.Create();
            return dialog;
        }

        public DeleteDialog(Scenes.GalleryScene scene)
        {
            mscene = scene;
        }

        public class ClickListener : Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            INoticeDialogListener listener;
            private static Logger logger = LogManager.GetCurrentClassLogger();
            DeleteDialog dd;
            public ClickListener(INoticeDialogListener listener,DeleteDialog dd)
            {
                this.dd = dd;
                this.listener = listener;
            }
            public void OnClick(IDialogInterface dialog, int which)
            {
                switch ((DialogButtonType)which)
                {
                    case DialogButtonType.Positive: 
                            listener.OnDialogPositiveClick(dd);
                        break;
                    case DialogButtonType.Negative:
                        listener.OnDialogNegativeClick(dd);
                        break;
                }
            }
        }
        public INoticeDialogListener mDialogListener;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnResume()
        {
            base.OnResume();
        }

        void CreateDialogBuilder()
        {
            builder = new AlertDialog.Builder(Activity);
            mDialogListener = mscene.dialogeventlistener;
            builder.SetPositiveButton("OK", new ClickListener(mDialogListener, this));
            builder.SetNegativeButton("Cancel", new ClickListener(mDialogListener, this));

            LayoutInflater inflater = mscene.Activity.LayoutInflater;
            View deleteDialog = inflater.Inflate(Resource.Layout.DeleteDialog,null);
            TrashCheckBox = deleteDialog.FindViewById<CheckBox>(Resource.Id.trashCheckBox);
            Summary = deleteDialog.FindViewById<TextView>(Resource.Id.dialogSummary);
            builder.SetView(deleteDialog);
        }

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            try
            {
                if (builder == null)
                    CreateDialogBuilder();
                // Instantiate the NoticeDialogListener so we can send events to the host
                mDialogListener = mscene.dialogeventlistener;
                builder.SetPositiveButton("OK", new ClickListener(mDialogListener, this));
                builder.SetNegativeButton("Cancel", new ClickListener(mDialogListener, this));
            }
            catch (Java.Lang.ClassCastException e)
            {
                // The activity doesn't implement the interface, throw exception
                throw new Java.Lang.ClassCastException(context.ToString()
                        + " must implement NoticeDialogListener");
            }
        }
    }

    public interface INoticeDialogListener
    {
        void OnDialogPositiveClick(Android.Support.V4.App.DialogFragment dialog);
        void OnDialogNegativeClick(Android.Support.V4.App.DialogFragment dialog);

    }
}