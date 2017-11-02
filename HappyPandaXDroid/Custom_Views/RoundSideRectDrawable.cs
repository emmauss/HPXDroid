/*
 * Copyright 2016 Hippo Seven
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
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace HappyPandaXDroid.Custom_Views
{
    public class RoundSideRectDrawable : Drawable
    {
        private  Paint mPaint;
        private  Path mPath;
        private  RectF mTempRectF;
        public override int Opacity {
            get
            {
                return (int)Format.Opaque;
            }
        }

        public RoundSideRectDrawable(int color)
        {
            mPaint = new Paint(PaintFlags.AntiAlias | PaintFlags.Dither)
            {
                Color = new Color(color)
            };
            mPath = new Path();
            mTempRectF = new RectF();
        }

        protected override void OnBoundsChange(Rect bounds)
        {
            int width = bounds.Width();
            int height = bounds.Height();
            RectF rectF = mTempRectF;
            Path path = mPath;

            path.Reset();
            if (width > height)
            {
                int radius = height / 2;
                path.MoveTo(bounds.Right - radius, bounds.Top);
                rectF.Set(bounds.Right - height, bounds.Top, bounds.Right, bounds.Bottom);
                path.ArcTo(rectF, -90.0f, 180.0f, false);
                path.LineTo(bounds.Left + radius, bounds.Bottom);
                rectF.Set(bounds.Left, bounds.Top, bounds.Left + height, bounds.Bottom);
                path.ArcTo(rectF, 90.0f, 180.0f, false);
                path.LineTo(bounds.Right - radius, bounds.Top);
            }
            else if (width == height)
            {
                path.AddCircle(bounds.CenterX(), bounds.CenterY(), width / 2, Path.Direction.Cw);
            }
            else
            {
                // TODO
            }
        }

        public override void Draw(Canvas canvas)
        {
            canvas.DrawPath(mPath, mPaint);
        }

        public override void SetAlpha(int alpha)
        {
            mPaint.Alpha = alpha;
        }

        public override void SetColorFilter(ColorFilter colorFilter)
        {
            mPaint.SetColorFilter(colorFilter);
        }
    }
}