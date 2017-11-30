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
using Android.Views.Animations;
using Android.Hardware;

namespace LargeImageView
{
    public class Fling
    {
        private static readonly float DECELERATION_RATE = (float)(Math.Log(0.78) / Math.Log(0.9));
        private static readonly float INFLEXION = 0.35f; // Tension lines cross at (INFLEXION, 1)
        private static readonly float START_TENSION = 0.5f;
        private static readonly float END_TENSION = 1.0f;
        private static readonly float P1 = START_TENSION * INFLEXION;
        private static readonly float P2 = 1.0f - END_TENSION * (1.0f - INFLEXION);
        private static readonly float FLING_FRICTION = ViewConfiguration.ScrollFriction;

        private static readonly int NB_SAMPLES = 100;
        private static readonly float[] SPLINE_POSITION = new float[NB_SAMPLES + 1];
        private static readonly float[] SPLINE_TIME = new float[NB_SAMPLES + 1];

        private readonly float mPhysicalCoeff;

        static Fling()
        {
            float x_min = 0.0f;
            float y_min = 0.0f;
            for (int i = 0; i < NB_SAMPLES; i++)
            {
                float alpha = (float)i / NB_SAMPLES;

                float x_max = 1.0f;
                float x, tx, coef;
                while (true)
                {
                    x = x_min + (x_max - x_min) / 2.0f;
                    coef = 3.0f * x * (1.0f - x);
                    tx = coef * ((1.0f - x) * P1 + x * P2) + x * x * x;
                    if (Math.Abs(tx - alpha) < 1E-5) break;
                    if (tx > alpha) x_max = x;
                    else x_min = x;
                }
                SPLINE_POSITION[i] = coef * ((1.0f - x) * START_TENSION + x) + x * x * x;

                float y_max = 1.0f;
                float y, dy;
                while (true)
                {
                    y = y_min + (y_max - y_min) / 2.0f;
                    coef = 3.0f * y * (1.0f - y);
                    dy = coef * ((1.0f - y) * START_TENSION + y) + y * y * y;
                    if (Math.Abs(dy - alpha) < 1E-5) break;
                    if (dy > alpha) y_max = y;
                    else y_min = y;
                }
                SPLINE_TIME[i] = coef * ((1.0f - y) * P1 + y * P2) + y * y * y;
            }
            SPLINE_POSITION[NB_SAMPLES] = SPLINE_TIME[NB_SAMPLES] = 1.0f;
        }

        class FlingInterpolator : Java.Lang.Object, IInterpolator
        {
            public float GetInterpolation(float input)
            {
                int index = (int)(NB_SAMPLES * input);
                float distanceCoef = 1.0f;
                float velocityCoef;
                if (index < NB_SAMPLES)
                {
                    float t_inf = (float)index / NB_SAMPLES;
                    float t_sup = (float)(index + 1) / NB_SAMPLES;
                    float d_inf = SPLINE_POSITION[index];
                    float d_sup = SPLINE_POSITION[index + 1];
                    velocityCoef = (d_sup - d_inf) / (t_sup - t_inf);
                    distanceCoef = d_inf + (input - t_inf) * velocityCoef;
                }
                return distanceCoef;
            }
        }

        public static IInterpolator FLING_INTERPOLATOR = new FlingInterpolator();


        public Fling(Context context)
        {
            float ppi = context.Resources.DisplayMetrics.Density * 160.0f;
            mPhysicalCoeff = SensorManager.GravityEarth // g (m/s^2)
                    * 39.37f // inch/meter
                    * ppi
                    * 0.84f; // look and feel tuning
        }

        public double GetSplineDeceleration(float velocity)
        {
            return Math.Log(INFLEXION * Math.Abs(velocity) / (FLING_FRICTION * mPhysicalCoeff));
        }

        /* Returns the duration, expressed in milliseconds */
        public int GetSplineFlingDuration(float velocity)
        {
            double l = GetSplineDeceleration(velocity);
            double decelMinusOne = DECELERATION_RATE - 1.0;
            return (int)(1000.0 * Math.Exp(l / decelMinusOne));
        }

        public double GetSplineFlingDistance(float velocity)
        {
            double l = GetSplineDeceleration(velocity);
            double decelMinusOne = DECELERATION_RATE - 1.0;
            return FLING_FRICTION * mPhysicalCoeff * Math.Exp(DECELERATION_RATE / decelMinusOne * l);
        }

        /**
         *  Modifies mDuration to the duration it takes to get from start to newreadonly using the
         *  spline interpolation. The previous duration was needed to get to oldreadonly.
         **/
        public int AdjustDuration(float start, float oldreadonly, float newreadonly, int duration)
        {
            float oldDistance = oldreadonly - start;
            float newDistance = newreadonly - start;
            float x = Math.Abs(newDistance / oldDistance);
            int index = (int)(NB_SAMPLES * x);
            if (index < NB_SAMPLES)
            {
                float x_inf = (float)index / NB_SAMPLES;
                float x_sup = (float)(index + 1) / NB_SAMPLES;
                float t_inf = SPLINE_TIME[index];
                float t_sup = SPLINE_TIME[index + 1];
                float timeCoef = t_inf + (x - x_inf) / (x_sup - x_inf) * (t_sup - t_inf);
                duration = (int)(duration * timeCoef);
            }
            return duration;
        }
    }
}
