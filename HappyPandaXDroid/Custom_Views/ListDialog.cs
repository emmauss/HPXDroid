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
using NLog;

namespace HappyPandaXDroid.Custom_Views
{
    class ListDialog : DialogFragment, AdapterView.IOnItemClickListener
    {
        AlertDialog.Builder builder;
        Scenes.LibraryScene mscene;
        ListView MainList;
        List<string> items;
        string tag;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            builder = new AlertDialog.Builder(Activity);
            LayoutInflater inflater = mscene.Activity.LayoutInflater;
            View listDialog = inflater.Inflate(Resource.Layout.DialogList, null);
            MainList = listDialog.FindViewById<ListView>(Resource.Id.list);
            builder.SetView(listDialog);
            List<string> list = new List<string>();
            switch (tag)
            {
                case "sort":
                    var strings = Enum.GetNames(typeof(Core.Gallery.Sort));
                    for (int i = 0; i < strings.Length; i++)
                        strings[i] = Helpers.StringHelpers.PascalSplit(strings[i]);
                    list = new List<string>(strings);
                    break;
                case "order":
                    list = new List<string>(new string[] { "Ascending", "Descending" });
                    break;
            }
            items = list;

            ArrayAdapter<String> adapter = new ArrayAdapter<string>(mscene.Context,
               Android.Resource.Layout.SimpleListItem1, items);
            MainList.Adapter = adapter;
            MainList.OnItemClickListener = this;

            AlertDialog dialog = builder.Create();
            return dialog;
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            string item = items[position];
            switch (tag)
            {
                case "sort":
                    item = item.Replace(" ", string.Empty);
                    var enumv = (Core.Gallery.Sort)Enum.Parse(typeof(Core.Gallery.Sort), item);
                    Core.App.Settings.Default_Sort = enumv;
                    Dismiss();
                    mscene.Refresh();
                    break;
                case "order":
                    if (item == "Ascending")
                        Core.App.Settings.Sort_Decending = false;
                    else if (item == "Descending")
                        Core.App.Settings.Sort_Decending = true;
                    Dismiss();
                    mscene.Refresh();
                    break;
            }
        }

        public ListDialog(Scenes.LibraryScene scene, string tag)
        {
            mscene = scene;
            this.tag = tag;
        }
    }
}