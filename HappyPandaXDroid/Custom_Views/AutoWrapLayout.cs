/*
 * Copyright 2015 Hippo Seven
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Graphics;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace HappyPandaXDroid.Custom_Views
{
    public class AutoWrapLayout : ViewGroup
    {
        private readonly List<Rect> rectList = new List<Rect>();

        private Alignment mAlignment;

        private static readonly Alignment[] sBaseLineArray = { Alignment.TOP,
            Alignment.CENTER, Alignment.BOTTOM };


        public enum Alignment
        {
            TOP = 0,
            CENTER = 1,
            BOTTOM = 2
        };

        


        public AutoWrapLayout(Context context) :
                base(context)
        {

        }

        public AutoWrapLayout(Context context, IAttributeSet attrs) :
                base(context, attrs)
        {
            
        }

        public AutoWrapLayout(Context context, IAttributeSet attrs, int defStyle) :
                base(context, attrs, defStyle)
        {
            TypedArray a = context.ObtainStyledAttributes(attrs,
                Resource.Styleable.AutoWrapLayout, defStyle, 0);

           int index = a.GetInt(Resource.Styleable.AutoWrapLayout_alignment, -1);
            if (index >= 0)
            {
                
                SetAlignment(sBaseLineArray[index]);
            }

            a.Recycle();
        }

        public void SetAlignment(Alignment baseLine)
        {
            if (baseLine == null)
            {
                return;
            }

            if (mAlignment != baseLine)
            {
                mAlignment = baseLine;

                RequestLayout();
                Invalidate();
            }
        }

        public Alignment GetAlignment()
        {
            return mAlignment;
        }

        private void AdjustBaseLine(int lineHeight, int startIndex, int endIndex)
        {
            if (mAlignment == Alignment.TOP)
                return;

            for (int index = startIndex; index < endIndex; index++)
            {
                View child = GetChildAt(index);
                var lay = child.LayoutParameters;
                MarginLayoutParams lp =
                        (MarginLayoutParams)child.LayoutParameters;
                lp.SetMargins(5, 5, 5, 5);
                Rect rect = rectList[index];
                int offsetRaw = lineHeight - rect.Height() - lp.TopMargin - lp.BottomMargin;
                if (mAlignment == Alignment.CENTER)
                    rect.Offset(0, offsetRaw / 2);
                else if (mAlignment == Alignment.BOTTOM)
                    rect.Offset(0, offsetRaw);
            }
        }


        /// <summary>
        /// each row or line at least show one child
        /// 
        /// horizontal only show child can show or partly show in parent
        /// </summary>
        /// <param name="widthMeasureSpec"></param>
        /// <param name="heightMeasureSpec"></param>
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var widthMode =  MeasureSpec.GetMode(widthMeasureSpec);
            var heightMode = MeasureSpec.GetMode(heightMeasureSpec);

            int maxWidth = MeasureSpec.GetSize(widthMeasureSpec);
            int maxHeight = MeasureSpec.GetSize(heightMeasureSpec);

            if (widthMode == MeasureSpecMode.Unspecified)
                maxWidth = int.MaxValue;
            if (heightMode == MeasureSpecMode.Unspecified)
                maxHeight = int.MaxValue;

            int paddingLeft = PaddingLeft;
            int paddingTop = PaddingTop;
            int paddingRight = PaddingRight;
            int paddingBottom = PaddingBottom;

            int maxRightBound = maxWidth - paddingRight;
            int maxBottomBound = maxHeight - paddingBottom;

            int left;
            int top;
            int right;
            int bottom;
            int rightBound = paddingLeft;
            int maxRightNoPadding = rightBound;
            int bottomBound;
            int lastMaxBottom = paddingTop;
            int maxBottom = lastMaxBottom;
            int childWidth;
            int childHeight;

            int lineStartIndex = 0;
            int lineEndIndex; // endIndex + 1

            rectList.Clear();
            int childCount = ChildCount;
            for (int index = 0; index < childCount; index++)
            {
                View child = GetChildAt(index);
                child.Measure((int)MeasureSpecMode.Unspecified,(int)MeasureSpecMode.Unspecified);
                if (child.Visibility == ViewStates.Gone)
                    continue;
                MarginLayoutParams lp = new MarginLayoutParams(child.LayoutParameters);
                        //(MarginLayoutParamschild.LayoutParameters;
                childWidth = child.MeasuredWidth;
                childHeight = child.MeasuredHeight;
                lp.SetMargins(5, 5, 5, 5);
                left = rightBound + lp.LeftMargin;
                right = left + childWidth;
                rightBound = right + lp.RightMargin;
                if (rightBound > maxRightBound)
                { // Go to next row
                    lineEndIndex = index;
                    // Adjust child position base on baseline
                    AdjustBaseLine(maxBottom - lastMaxBottom, lineStartIndex, lineEndIndex);

                    // If child can't show in parent begin this line
                    if (maxBottom >= maxBottomBound)
                        break;

                    // If it is first item in line, try to show it all
                    if (lineEndIndex == lineStartIndex)
                    {
                        child.Measure(MeasureSpec.MakeMeasureSpec(
                                maxWidth - paddingLeft - paddingRight
                                        - lp.LeftMargin - lp.RightMargin, MeasureSpecMode.AtMost),
                                (int)MeasureSpecMode.Unspecified);
                        childWidth = child.MeasuredWidth;
                        childHeight = child.MeasuredHeight;
                    }
                    left = paddingLeft + lp.LeftMargin;
                    right = left + childWidth;
                    rightBound = right + lp.RightMargin;

                    lastMaxBottom = maxBottom;
                    top = lastMaxBottom + lp.TopMargin;
                    bottom = top + childHeight;
                    bottomBound = bottom + lp.BottomMargin;

                    lineStartIndex = index;
                }
                else
                {
                    top = lastMaxBottom + lp.TopMargin;
                    bottom = top + childHeight;
                    bottomBound = bottom + lp.BottomMargin;
                }
                // Update max
                if (rightBound > maxRightNoPadding)
                    maxRightNoPadding = rightBound;
                if (bottomBound > maxBottom)
                    maxBottom = bottomBound;
                Rect rect = new Rect
                {
                    Left = left,
                    Top = top,
                    Right = right,
                    Bottom = bottom
                };
                rectList.Add(rect);
            }

            // Handle last line baseline
            AdjustBaseLine(maxBottom - lastMaxBottom, lineStartIndex, rectList.Count);

            int measuredWidth;
            int measuredHeight;

            if (widthMode == MeasureSpecMode.Exactly)
                measuredWidth = maxWidth;
            else
                measuredWidth = maxRightNoPadding + paddingRight;
            if (heightMode == MeasureSpecMode.Exactly)
                measuredHeight = maxHeight;
            else
            {
                measuredHeight = maxBottom + paddingBottom;
                if (heightMode == MeasureSpecMode.AtMost)
                    measuredHeight = measuredHeight > maxHeight ? maxHeight : measuredHeight;
            }

            SetMeasuredDimension(measuredWidth, measuredHeight);
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            int count = rectList.Count;
            for (int i = 0; i < count; i++)
            {
                View child = this.GetChildAt(i);
                if (child.Visibility == ViewStates.Gone)
                    continue;
                Rect rect = rectList[i];
                child.Layout(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }
        }

        public override LayoutParams GenerateLayoutParams(IAttributeSet attrs)
        {
            return new LayoutParams(Context,attrs);
        }

        protected override LayoutParams GenerateDefaultLayoutParams()
        {
            return new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
        }

        protected override LayoutParams GenerateLayoutParams(LayoutParams p)
        {
            return new LayoutParams(p);
        }


        private void Initialize()
        {
        }
    }
}
