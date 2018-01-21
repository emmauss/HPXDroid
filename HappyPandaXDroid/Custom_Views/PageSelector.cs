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
    public class PageSelector : DialogFragment
    {
        public int PageCount = 0;
        public int PageSelected = 0;
        public EditText PageInput;
        AlertDialog.Builder builder;
        public Android.Support.Design.Widget.TextInputLayout FloatingTextLayout;
        Scenes.LibraryScene mscene;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            builder = new AlertDialog.Builder(Activity);
            mDialogListener = mscene.dialogeventlistener;
            builder.SetPositiveButton("OK", new ClickListener(mDialogListener, this));
            builder.SetNegativeButton("Cancel", new ClickListener(mDialogListener, this));

            LayoutInflater inflater = mscene.Activity.LayoutInflater;
            View pageseletor = inflater.Inflate(Resource.Layout.PageSelector,null);
            PageInput = pageseletor.FindViewById<EditText>(Resource.Id.setpage);
            FloatingTextLayout = pageseletor
                .FindViewById<Android.Support.Design.Widget.TextInputLayout>(Resource.Id.textInputLayout1);
            PageCount = (int)Math.Round((double)mscene.count / 25);
            FloatingTextLayout.Hint = mscene.CurrentPage + 1 + " of " + PageCount;
            builder.SetView(pageseletor);
            AlertDialog dialog = builder.Create();
            return dialog;
        }

        public PageSelector(Scenes.LibraryScene scene)
        {
            mscene = scene;
        }

        public class ClickListener : Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            INoticeDialogListener listener;
            private static Logger logger = LogManager.GetCurrentClassLogger();
            PageSelector pg;
            public ClickListener(INoticeDialogListener listener,PageSelector pg)
            {
                this.pg = pg;
                this.listener = listener;
            }
            public void OnClick(IDialogInterface dialog, int which)
            {
                switch ((DialogButtonType)which)
                {
                    case DialogButtonType.Positive:
                        if (int.TryParse(pg.PageInput.Text, out pg.PageSelected))
                            if (pg.PageSelected > 0 && pg.PageSelected !=pg.mscene.CurrentPage+1 ) 
                            listener.OnDialogPositiveClick(pg);
                        break;
                    case DialogButtonType.Negative:
                        listener.OnDialogNegativeClick(pg);
                        break;
                }
            }
        }

        public interface INoticeDialogListener
        {
            void OnDialogPositiveClick(DialogFragment dialog);
            void OnDialogNegativeClick(DialogFragment dialog);

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
            PageCount = (int)Math.Round((double)mscene.count / 25);
            FloatingTextLayout.Hint = mscene.CurrentPage + 1 + " of " + PageCount;
        }

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            try
            {
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
}