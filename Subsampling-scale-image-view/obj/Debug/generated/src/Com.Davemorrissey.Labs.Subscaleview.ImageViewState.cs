using System;
using System.Collections.Generic;
using Android.Runtime;

namespace Com.Davemorrissey.Labs.Subscaleview {

	// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageViewState']"
	[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/ImageViewState", DoNotGenerateAcw=true)]
	public partial class ImageViewState : global::Java.Lang.Object, global::Java.IO.ISerializable {

		internal static new IntPtr java_class_handle;
		internal static new IntPtr class_ref {
			get {
				return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/ImageViewState", ref java_class_handle);
			}
		}

		protected override IntPtr ThresholdClass {
			get { return class_ref; }
		}

		protected override global::System.Type ThresholdType {
			get { return typeof (ImageViewState); }
		}

		protected ImageViewState (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

		static IntPtr id_ctor_FLandroid_graphics_PointF_I;
		// Metadata.xml XPath constructor reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageViewState']/constructor[@name='ImageViewState' and count(parameter)=3 and parameter[1][@type='float'] and parameter[2][@type='android.graphics.PointF'] and parameter[3][@type='int']]"
		[Register (".ctor", "(FLandroid/graphics/PointF;I)V", "")]
		public unsafe ImageViewState (float p0, global::Android.Graphics.PointF p1, int p2)
			: base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
		{
			if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
				return;

			try {
				JValue* __args = stackalloc JValue [3];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				__args [2] = new JValue (p2);
				if (((object) this).GetType () != typeof (ImageViewState)) {
					SetHandle (
							global::Android.Runtime.JNIEnv.StartCreateInstance (((object) this).GetType (), "(FLandroid/graphics/PointF;I)V", __args),
							JniHandleOwnership.TransferLocalRef);
					global::Android.Runtime.JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, "(FLandroid/graphics/PointF;I)V", __args);
					return;
				}

				if (id_ctor_FLandroid_graphics_PointF_I == IntPtr.Zero)
					id_ctor_FLandroid_graphics_PointF_I = JNIEnv.GetMethodID (class_ref, "<init>", "(FLandroid/graphics/PointF;I)V");
				SetHandle (
						global::Android.Runtime.JNIEnv.StartCreateInstance (class_ref, id_ctor_FLandroid_graphics_PointF_I, __args),
						JniHandleOwnership.TransferLocalRef);
				JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, class_ref, id_ctor_FLandroid_graphics_PointF_I, __args);
			} finally {
			}
		}

		static Delegate cb_getCenter;
#pragma warning disable 0169
		static Delegate GetGetCenterHandler ()
		{
			if (cb_getCenter == null)
				cb_getCenter = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, IntPtr>) n_GetCenter);
			return cb_getCenter;
		}

		static IntPtr n_GetCenter (IntPtr jnienv, IntPtr native__this)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.ImageViewState __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageViewState> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			return JNIEnv.ToLocalJniHandle (__this.Center);
		}
#pragma warning restore 0169

		static IntPtr id_getCenter;
		public virtual unsafe global::Android.Graphics.PointF Center {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageViewState']/method[@name='getCenter' and count(parameter)=0]"
			[Register ("getCenter", "()Landroid/graphics/PointF;", "GetGetCenterHandler")]
			get {
				if (id_getCenter == IntPtr.Zero)
					id_getCenter = JNIEnv.GetMethodID (class_ref, "getCenter", "()Landroid/graphics/PointF;");
				try {

					if (((object) this).GetType () == ThresholdType)
						return global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_getCenter), JniHandleOwnership.TransferLocalRef);
					else
						return global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "getCenter", "()Landroid/graphics/PointF;")), JniHandleOwnership.TransferLocalRef);
				} finally {
				}
			}
		}

		static Delegate cb_getOrientation;
#pragma warning disable 0169
		static Delegate GetGetOrientationHandler ()
		{
			if (cb_getOrientation == null)
				cb_getOrientation = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, int>) n_GetOrientation);
			return cb_getOrientation;
		}

		static int n_GetOrientation (IntPtr jnienv, IntPtr native__this)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.ImageViewState __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageViewState> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			return __this.Orientation;
		}
#pragma warning restore 0169

		static IntPtr id_getOrientation;
		public virtual unsafe int Orientation {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageViewState']/method[@name='getOrientation' and count(parameter)=0]"
			[Register ("getOrientation", "()I", "GetGetOrientationHandler")]
			get {
				if (id_getOrientation == IntPtr.Zero)
					id_getOrientation = JNIEnv.GetMethodID (class_ref, "getOrientation", "()I");
				try {

					if (((object) this).GetType () == ThresholdType)
						return JNIEnv.CallIntMethod (((global::Java.Lang.Object) this).Handle, id_getOrientation);
					else
						return JNIEnv.CallNonvirtualIntMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "getOrientation", "()I"));
				} finally {
				}
			}
		}

		static Delegate cb_getScale;
#pragma warning disable 0169
		static Delegate GetGetScaleHandler ()
		{
			if (cb_getScale == null)
				cb_getScale = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, float>) n_GetScale);
			return cb_getScale;
		}

		static float n_GetScale (IntPtr jnienv, IntPtr native__this)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.ImageViewState __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageViewState> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			return __this.Scale;
		}
#pragma warning restore 0169

		static IntPtr id_getScale;
		public virtual unsafe float Scale {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageViewState']/method[@name='getScale' and count(parameter)=0]"
			[Register ("getScale", "()F", "GetGetScaleHandler")]
			get {
				if (id_getScale == IntPtr.Zero)
					id_getScale = JNIEnv.GetMethodID (class_ref, "getScale", "()F");
				try {

					if (((object) this).GetType () == ThresholdType)
						return JNIEnv.CallFloatMethod (((global::Java.Lang.Object) this).Handle, id_getScale);
					else
						return JNIEnv.CallNonvirtualFloatMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "getScale", "()F"));
				} finally {
				}
			}
		}

	}
}
