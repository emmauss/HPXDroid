using Android.Content;
using Android.Support.V7.Widget;
namespace HappyPandaXDroid.Helpers
{
    public class Layouts
    {
        public class ExtraGridLayoutManager : GridLayoutManager
        {
            private static readonly int DEFAULT_EXTRA_LAYOUT_SPACE = 800;
            private int extraLayoutSpace = -1;
            private Context context;


            public ExtraGridLayoutManager(Context context, int columns) : base(context, columns)
            {
                this.context = context;
            }

            public ExtraGridLayoutManager(Context context, int columns, int extraLayoutSpace) : base(context, columns)
            {
                this.context = context;
                this.extraLayoutSpace = extraLayoutSpace;
            }



            public ExtraGridLayoutManager(Context context, int columns, int orientation, bool reverseLayout)
                : base(context, columns, orientation, reverseLayout)
            {
                this.context = context;
            }

            public void SetExtraLayoutSpace(int extraLayoutSpace)
            {
                this.extraLayoutSpace = extraLayoutSpace;
            }

            protected override int GetExtraLayoutSpace(RecyclerView.State state)
            {
                if (extraLayoutSpace > 0)
                {
                    return extraLayoutSpace;
                }
                else
                    return DEFAULT_EXTRA_LAYOUT_SPACE;
            }
        }

    }
}