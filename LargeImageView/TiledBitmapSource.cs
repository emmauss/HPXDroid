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
using Android.Graphics;
using Android.Util;

using Java.Util;
using Java.Lang;

namespace LargeImageView
{
    public class TiledBitmapSource : ImageSource
    {

        public class Tile :  Java.Lang.Object
        {
            public Rect rect;
            public Bitmap bitmap;
            public bool loading;
            public bool visible;
            public bool failed;
        }

        private RegionDecoder mDecoder;
        // The width of parent view
        private int mWindowWidth;
        // The height of parent view
        private int mWindowHeight;
        // The max width and height for tile
        private int mMaxTileSize;
        // Indicate whether animator is running
        private bool mAnimating;
        // Indicate whether finger is on the View
        private bool mTouching;

        private Paint mPaint;

        // Sample for current rendered image
        private int mCurrentSample;
        // Sample for image fill windows
        private int mFullSample;
        private List<Tile> mFullTiles;
        private readonly SparseArray<List<Tile>> mTilesMap = new SparseArray<List<Tile>>();

        private FullTileTask mFullTileTask;
        private readonly List<LoadTileTask> mLoadTileTaskList = new List<LoadTileTask>();

        private readonly Matrix mMatrix = new Matrix();

        private readonly RectF mTempRectF1 = new RectF();
        private readonly RectF mTempRectF2 = new RectF();
        private readonly RectF mTempRectF3 = new RectF();
        private readonly RectF mTempRectF4 = new RectF();
        private readonly List<Tile> mTempTileList = new List<Tile>();

        public TiledBitmapSource(RegionDecoder decoder)
        {
            mDecoder = decoder;
            mPaint = new Paint(PaintFlags.FilterBitmap | PaintFlags.Dither);
        }



        public override bool Ready => true;

        public override int Width => mDecoder.Width;

        public override int Height => mDecoder.Height;

        public override void Draw(Canvas canvas, RectF src, RectF dst)
        {
            if (mFullTiles == null)
            {
                // If mFullTiles is null, means first decode step
                // is not done. Wait for it.
                return;
            }

            int sample = CalculateSample((int)(src.Width() / dst.Width()),
                (int)(src.Height() / dst.Height()));
            // Full sample must be the biggest sample
            sample = System.Math.Min(mFullSample, sample);
            // Update current sample
            mCurrentSample = sample;

            if (sample == mFullSample)
            {
                DrawFullTiles(canvas, src, dst, mFullTiles, mFullSample);
            }
            else
            {
                List<Tile> list = mTilesMap.Get(sample);
                if (list == null)
                {
                    list = GenerateTileList(sample);
                    mTilesMap.Put(sample, list);
                }
                DrawMapTiles(canvas, src, dst, list, sample);
            }

            // Always gc after draw tiles
            GC();
        }

        public override void Initialize()
        {

        }

        public override void Recycle()
        {
            mPaint = null;

            // Recycle all tiles
            if (mFullTiles != null)
            {
                RecycleTiles(mFullTiles);
                mFullTiles = null;
            }
            for (int i = 0, len = mTilesMap.Size(); i < len; i++)
            {
                RecycleTiles(mTilesMap.ValueAt(i));
            }
            mTilesMap.Clear();

            bool recycled = false;

            // Cancel all tasks
            if (mFullTileTask != null)
            {
                recycled = true;
                mFullTileTask.Recycle();
                mFullTileTask.Cancel(false);
            }
            foreach (LoadTileTask task in mLoadTileTaskList)
            {
                recycled = true;
                task.Recycle();
                task.Cancel(false);
            }
            mLoadTileTaskList.Clear();

            if (!recycled)
            {
                mDecoder.Recycle();
            }
            mDecoder = null;
        }

        public override void SetMaxBitmapSize(int maxSize)
        {
            // I think MaxBitmapSize / 4 is suitable for max tile size
            mMaxTileSize = maxSize / 4;
            EnsureFullTiles();
        }

        public override void SetWindowSize(int w, int h)
        {
            mWindowWidth = w;
            mWindowHeight = h;
            EnsureFullTiles();
        }

        public override void OnAnimatorStart()
        {
            mAnimating = true;
        }

        public override void OnAnimatorEnd()
        {
            mAnimating = false;
            // Trigger loading missing tiles
            InvalidateSelf();
        }

        public override void OnTouchStart()
        {
            mTouching = true;
        }

        public override void OnTouchEnd()
        {
            mTouching = false;
            // Trigger loading missing tiles
            InvalidateSelf();
        }

        private static int CalculateSample(int scaleX, int scaleY)
        {
            int sample = System.Math.Max(scaleX, scaleY);
            sample = System.Math.Max(1, sample);
            return PrevPow2(sample);
        }

        private static void RecycleTiles(List<Tile> tiles)
        {
            if (tiles == null)
            {
                return;
            }

            foreach (Tile tile in tiles)
            {
                Bitmap bitmap = tile.bitmap;
                if (bitmap != null)
                {
                    bitmap.Recycle();
                    tile.bitmap = null;
                    // Reset failed flag
                    tile.failed = false;
                }
            }
        }

        private void EnsureFullTiles()
        {
            if (mWindowWidth == 0 || mWindowHeight == 0 || mMaxTileSize == 0)
            {
                return;
            }
            // Get full sample
            int fullSample = CalculateSample(mDecoder.Width / mWindowWidth,
                    mDecoder.Height / mWindowHeight);
            if (mFullSample == fullSample)
            {
                // full sample is still the same
                return;
            }

            mFullSample = fullSample;

            // Recycle all tiles
            if (mFullTiles != null)
            {
                RecycleTiles(mFullTiles);
                mFullTiles = null;
            }
            for (int i = 0, len = mTilesMap.Size(); i < len; i++)
            {
                RecycleTiles(mTilesMap.ValueAt(i));
            }
            mTilesMap.Clear();

            // Cancel all tasks
            if (mFullTileTask != null)
            {
                mFullTileTask.Cancel(false);
                mFullTileTask = null;
            }
            foreach (LoadTileTask task in mLoadTileTaskList)
            {
                task.Cancel(false);
            }
            mLoadTileTaskList.Clear();

            // Start FullTileTask
            mFullTileTask = new FullTileTask(this);
            mFullTileTask.Execute();

            InvalidateSelf();
        }

        private void OnFullTileDone(List<Tile> tiles)
        {
            mFullTileTask = null;
            mFullTiles = tiles;
            InvalidateSelf();
        }

        private void OnLoadTileDone(LoadTileTask task, int sample)
        {
            mLoadTileTaskList.Remove(task);
            if (sample == mCurrentSample)
            {
                InvalidateSelf();
            }
        }

        private static void MapRect(RectF src, RectF dst, RectF s, RectF d)
        {
            float sX = src.Left;
            float sY = src.Top;
            float dX = dst.Left;
            float dY = dst.Top;
            float scaleX = dst.Width() / src.Width();
            float scaleY = dst.Height() / src.Height();
            d.Set(dX + (s.Left - sX) * scaleX,
                    dY + (s.Top - sY) * scaleY,
                    dX + (s.Right - sX) * scaleX,
                    dY + (s.Bottom - sY) * scaleY);
        }

        private static void MapRect(RectF rect, int sample)
        {
            rect.Left /= sample;
            rect.Top /= sample;
            rect.Right /= sample;
            rect.Bottom /= sample;
        }

        // For full tiles
        private void DrawFullTiles(Canvas canvas, RectF src, RectF dst, List<Tile> tiles, int sample)
        {
            RectF s = mTempRectF1;
            RectF d = mTempRectF2;
            Matrix matrix = mMatrix;
            Paint paint = mPaint;

            foreach (Tile t in tiles)
            {
                s.Set(t.rect);
                if (!s.Intersect(src)) { continue; }
                Bitmap bitmap = t.bitmap;
                if (bitmap == null) { continue; }
                MapRect(src, dst, s, d);
                s.Offset(-t.rect.Left, -t.rect.Top);
                MapRect(s, sample);
                matrix.SetRectToRect(s, d, Matrix.ScaleToFit.Fill);
                canvas.DrawBitmap(bitmap, matrix, paint);
            }
        }

        // For not full tiles
        private void DrawMapTiles(Canvas canvas, RectF src, RectF dst, List<Tile> tiles, int sample)
        {
            List<Tile> list = mTempTileList;
            RectF s = mTempRectF1;
            RectF src2 = mTempRectF2;

            bool firstMiss = true;
            // Get missing tiles
            foreach (Tile t in tiles)
            {
                s.Set(t.rect);
                if (s.Intersect(src))
                {
                    t.visible = true;
                    // Check bitmap in this tile
                    if (t.bitmap == null)
                    {
                        // Missing bitmap, add to miss rect
                        if (firstMiss)
                        {
                            firstMiss = false;
                            src2.Set(s);
                        }
                        else
                        {
                            src2.Union(s);
                        }

                        if (!t.loading && !t.failed && !mAnimating && !mTouching)
                        {
                            // It is not animating now and
                            // the tile is not loading, not have failed,
                            // start load tile task now
                            LoadTileTask task = new LoadTileTask(this, t, sample);
                            mLoadTileTaskList.Add(task);
                            task.Execute();
                        }
                    }
                    else
                    {
                        // Add this tile to render list
                        list.Add(t);
                    }
                }
                else
                {
                    t.visible = false;
                }
            }

            RectF d = mTempRectF3;
            Matrix matrix = mMatrix;
            Paint paint = mPaint;

            // Draw full tiles to fill missing rect
            if (!firstMiss)
            {
                RectF dst2 = mTempRectF4;
                MapRect(src, dst, src2, dst2);
                foreach (Tile t in mFullTiles)
                {
                    s.Set(t.rect);
                    if (!s.Intersect(src2)) { continue; }
                    Bitmap bitmap = t.bitmap;
                    if (bitmap == null) { continue; }
                    MapRect(src2, dst2, s, d);
                    s.Offset(-t.rect.Left, -t.rect.Top);
                    MapRect(s, mFullSample);
                    matrix.SetRectToRect(s, d, Matrix.ScaleToFit.Fill);
                    canvas.DrawBitmap(bitmap, matrix, paint);
                }
            }

            // Draw tile in list
            foreach (Tile t in list)
            {
                s.Set(t.rect);
                if (!s.Intersect(src)) { continue; }
                Bitmap bitmap = t.bitmap;
                if (bitmap == null) { continue; }
                MapRect(src, dst, s, d);
                s.Offset(-t.rect.Left, -t.rect.Top);
                MapRect(s, sample);
                matrix.SetRectToRect(s, d, Matrix.ScaleToFit.Fill);
                canvas.DrawBitmap(bitmap, matrix, paint);
            }
            list.Clear();
        }

        // Gen tile list for the sample
        private List<Tile> GenerateTileList(int sample)
        {
            int width = mDecoder.Width;
            int height = mDecoder.Height;
            int step = mMaxTileSize * sample;
            List<Tile> list = new List<Tile>(CeilDiv(width, step) * CeilDiv(height, step));

            for (int y = 0; y < height; y += step)
            {
                for (int x = 0; x < width; x += step)
                {
                    int w = System.Math.Min(step, width - x);
                    int h = System.Math.Min(step, height - y);
                    Rect rect = new Rect(x, y, x + w, y + h);
                    Tile tile = new Tile();
                    tile.rect = rect;
                    list.Add(tile);
                }
            }

            return list;
        }

        private void GC()
        {
            for (int i = 0, len = mTilesMap.Size(); i < len; i++)
            {
                int sample = mTilesMap.KeyAt(i);
                List<Tile> list = mTilesMap.ValueAt(i);
                if (sample != mCurrentSample)
                {
                    // Recycle all tiles for non current sample
                    RecycleTiles(list);
                }
                else
                {
                    // Only recycle invisible tile for current sample
                    foreach (Tile tile in list)
                    {
                        if (!tile.visible)
                        {
                            Bitmap bitmap = tile.bitmap;
                            if (bitmap != null)
                            {
                                bitmap.Recycle();
                                tile.bitmap = null;
                                // Don't reset failed flag for current sample
                            }
                        }
                    }
                }
            }
        }

        // The BaseTask for all the other tasks.
        // Handle mImageSource recycle.
        public abstract class BaseTask<Params, Progress, Result> : AsyncTask<Params, Progress, Result>
        {

            protected readonly RegionDecoder mDecoder;
            private bool mRecycle;

            public BaseTask(RegionDecoder decoder)
            {
                mDecoder = decoder;
            }

            // Call it before {@link #cancel(boolean)} if you want to
            // recycle the ImageSource.
            public void Recycle()
            {
                mRecycle = true;
            }

            protected override void OnCancelled(Java.Lang.Object result)
            {
                if (mRecycle)
                {
                    mDecoder.Recycle();
                }
            }


            

            

        }

        private class FullTileTask : BaseTask<object, object, List<Tile>>
        {

            private WeakReference<TiledBitmapSource> mSource;
            private new RegionDecoder mDecoder;
            private int mFullSample;
            private int mMaxTileSize;

            public FullTileTask(TiledBitmapSource source) : base(source.mDecoder)
            {
                mSource = new WeakReference<TiledBitmapSource>(source);
                mDecoder = source.mDecoder;
                mFullSample = source.mFullSample;
                mMaxTileSize = source.mMaxTileSize;
            }

            protected override void OnPostExecute(List<Tile> result)
            {
                mSource.TryGetTarget(out TiledBitmapSource source);
                if (source == null)
                {
                    RecycleTiles(result);
                }
                else
                {
                    // Callback
                    source.OnFullTileDone(result);
                }
            }


            protected override List<Tile> RunInBackground(params object[] @params)
            {
                int width = mDecoder.Width;
                int height = mDecoder.Height;
                int step = mMaxTileSize * mFullSample;
                List<Tile> list = new List<Tile>(CeilDiv(width, step) * CeilDiv(height, step));

                for (int y = 0; y < height; y += step)
                {
                    for (int x = 0; x < width; x += step)
                    {
                        if (IsCancelled)
                        {
                            break;
                        }
                        int w = System.Math.Min(step, width - x);
                        int h = System.Math.Min(step, height - y);
                        Rect rect = new Rect(x, y, x + w, y + h);
                        Bitmap bitmap = mDecoder.DecodeRegion(rect, mFullSample);
                        Tile tile = new Tile();
                        tile.rect = rect;
                        tile.bitmap = bitmap;
                        if (bitmap == null)
                        {
                            tile.failed = true;
                        }
                        list.Add(tile);
                    }
                }

                return list;
            }

            protected override void OnCancelled(Java.Lang.Object result)
            {
                base.OnCancelled(result);
                RecycleTiles(Cast<List<Tile>>(result));
            }

            
        }

        public static T Cast<T>(Java.Lang.Object obj) where T : List<Tile>
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
        }

        public class LoadTileTask : BaseTask<object, object, Bitmap>
        {

            private WeakReference<TiledBitmapSource> mSource;
            private WeakReference<Tile> mTile;
            private new RegionDecoder mDecoder;
            private int mSample;

            public LoadTileTask(TiledBitmapSource source, Tile tile, int sample) : base(source.mDecoder)
            {
                mSource = new WeakReference<TiledBitmapSource>(source);
                mTile = new WeakReference<Tile>(tile);
                mDecoder = source.mDecoder;
                mSample = sample;
                tile.loading = true;
            }

            protected override void OnPostExecute(Bitmap result)
            {
                mSource.TryGetTarget(out TiledBitmapSource source);
                mTile.TryGetTarget(out Tile tile);
                if (source == null || tile == null)
                {
                    if (result != null)
                    {
                        result.Recycle();
                    }
                }
                else
                {
                    tile.loading = false;
                    tile.bitmap = result;
                    if (result == null)
                    {
                        tile.failed = true;
                    }
                    // Callback
                    source.OnLoadTileDone(this, mSample);
                }
            }

            protected override void OnCancelled(Java.Lang.Object result)
            {
                base.OnCancelled(result);
                if (result != null)
                {
                    ((Android.Graphics.Bitmap)result).Recycle();
                }
            }

            protected override Bitmap RunInBackground(params object[] @params)
            {
                mTile.TryGetTarget(out Tile tile);
                if (tile != null)
                {
                    return mDecoder.DecodeRegion(tile.rect, mSample);
                }
                else
                {
                    return null;
                }
            }
        }

        private static int CeilDiv(int a, int b)
        {
            return (a + b - 1) / b;
        }

        private static int PrevPow2(int n)
        {
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            return n - (n >> 1);
        }
    }
}