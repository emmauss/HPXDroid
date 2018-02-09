using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Com.Hippo.Stage;

namespace HappyPandaXDroid.Custom_Views
{
    class TagsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<TagsAdapterClickEventArgs> ItemClick;
        public event EventHandler<TagsAdapterClickEventArgs> ItemLongClick;
        List<Core.Gallery.TagNamespace> tags = new List<Core.Gallery.TagNamespace>();
        Scene scene;

        public TagsAdapter(Scene parentScene)
        {
            scene = parentScene;
        }

        public void SetList(List<Core.Gallery.TagNamespace> list)
        {
            if (list != null)
            {
                tags.Clear();
                tags.AddRange(list);
                NotifyDataSetChanged();
            }
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            //var id = Resource.Layout.__YOUR_ITEM_HERE;
            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.tag_group_template, parent, false);
            //       Inflate(id, parent, false);
            var vh = new TagsAdapterViewHolder(itemView, scene);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = tags[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as TagsAdapterViewHolder;
            holder.SetTags(item);
        }

        public override int ItemCount => tags.Count;

    }

    public class TagsAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        Core.Gallery.TagNamespace tagNamespace;
        Scene scene;

        public TagsAdapterViewHolder(View itemView, Scene parent) : base(itemView)
        {
            scene = parent;
        }

        public void SetTags(Core.Gallery.TagNamespace tag)
        {
            tagNamespace = tag;

            DrawLayout();
        }

        void DrawLayout()
        {
            
            int color_header = ItemView.Context.Resources.GetColor(Resource.Color.colorPrimary);
            int color_tag = ItemView.Context.Resources.GetColor(Resource.Color.purple_a700);

            if (tagNamespace.tags.Count > 0)
            {
                var rtg = (LinearLayout)ItemView;
                rtg.Orientation = Android.Widget.Orientation.Horizontal;
                
                var inflater = LayoutInflater.From(rtg.Context);
                TextView tag_header = (TextView)inflater.Inflate(Resource.Layout.tag_template, rtg, false);
                tag_header.Text = tagNamespace.name.ToLower();
                tag_header.SetBackgroundDrawable(new Custom_Views.RoundSideRectDrawable(color_header));
                rtg.AddView(tag_header);
                Custom_Views.AutoWrapLayout awl = new Custom_Views.AutoWrapLayout(ItemView.Context);
                rtg.AddView(awl, ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                tagNamespace.tags.Sort((a, b) => a.name.CompareTo(b.name));
                foreach (var tag in tagNamespace.tags)
                {
                    TextView tag_item = (TextView)inflater.Inflate(Resource.Layout.tag_template, awl, false);
                    tag_item.Text = tag.name;
                    tag_item.SetBackgroundDrawable(new Custom_Views.RoundSideRectDrawable(color_tag));
                    tag_item.Click += Tag_item_Click;
                    string fullTagName = tagNamespace.name + ":" + "\"" + tag.name + "\"";
                    tag_item.Tag = fullTagName;
                    tag_item.Clickable = true;
                    awl.AddView(tag_item);
                }
            }
        }

        private void Tag_item_Click(object sender, EventArgs e)
        {
            TextView tag_item = sender as TextView;
            if (tag_item == null)
                return;
            scene.Stage.PushScene(new Scenes.LibraryScene((string)tag_item.Tag, (string)tag_item.Tag));
        }
    }

    public class TagsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}