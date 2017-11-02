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
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;

namespace HappyPandaXDroid.Custom_Views
{
    public class SafeCoordinatorLayout : CoordinatorLayout
    {
        public SafeCoordinatorLayout(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            
        }

        public SafeCoordinatorLayout(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
           
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            try
            {
                return base.OnInterceptTouchEvent(ev);
            }
            catch
            {
                return false;
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            try
            {
                return base.OnTouchEvent(e);
            }catch(Exception ex)
            {
                return false;
            }
        }
    }
}