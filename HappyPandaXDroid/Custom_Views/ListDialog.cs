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
            ImageButton button = listDialog.FindViewById<ImageButton>(Resource.Id.deletebutton);
            button.SetImageResource(Resource.Drawable.v_delete_x24);
            button.Visibility = ViewStates.Invisible;
            MainList.LongClick += MainList_LongClick;
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
                case "search":
                    button.Visibility = ViewStates.Visible;
                    var searches = Core.Media.QuickSearch.Searches;
                    list = searches;
                    button.Click += Button_Click;
                    break;
            }
            items = list;
            MainList.NothingSelected += MainList_NothingSelected;
            ArrayAdapter<String> adapter = new ArrayAdapter<string>(mscene.Context,
               Android.Resource.Layout.SimpleListItem1, items);
            MainList.Adapter = adapter;
            MainList.OnItemClickListener = this;

            AlertDialog dialog = builder.Create();
            return dialog;
        }

        private void MainList_NothingSelected(object sender, AdapterView.NothingSelectedEventArgs e)
        {
            MainList.ChoiceMode = ChoiceMode.Single;
        }

        private void MainList_LongClick(object sender, View.LongClickEventArgs e)
        {
            if (tag == "search")
            {
                MainList.ChoiceMode = ChoiceMode.Multiple;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var selecteditems = MainList.CheckedItemPositions;
            for(int i = 0; i < items.Count; i++)
            {
                if (selecteditems.Get(i))
                {
                    Core.Media.QuickSearch.RemoveFromQuicSearch(items[i]);
                }
            }
        }
        public override void OnStart()
        {
            base.OnStart();
            if (items.Count == 0)
            {
                Toast.MakeText(mscene.Context, "No list to display", ToastLength.Short).Show();
                Dismiss();
            }
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
                case "search":
                    if (MainList.ChoiceMode != ChoiceMode.Multiple)
                    {
                        mscene.Current_Query = Scenes.LibraryScene.Parse(item,false);
                        Dismiss();
                    }
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