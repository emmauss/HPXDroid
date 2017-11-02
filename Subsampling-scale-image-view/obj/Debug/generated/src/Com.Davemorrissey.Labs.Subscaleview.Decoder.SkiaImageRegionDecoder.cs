using System;
using System.Collections.Generic;
using Android.Runtime;

namespace Com.Davemorrissey.Labs.Subscaleview.Decoder {

	// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='SkiaImageRegionDecoder']"
	[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/decoder/SkiaImageRegionDecoder", DoNotGenerateAcw=true)]
	public partial class SkiaImageRegionDecoder : global::Java.Lang.Object, global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoder {

		internal static new IntPtr java_class_handle;
		internal static new IntPtr class_ref {
			get {
				return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/decoder/SkiaImageRegionDecoder", ref java_class_handle);
			}
		}

		protected override IntPtr ThresholdClass {
			get { return class_ref; }
		}

		protected override global::System.Type ThresholdType {
			get { return typeof (SkiaImageRegionDecoder); }
		}

		protected SkiaImageRegionDecoder (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

		static IntPtr id_ctor;
		// Metadata.xml XPath constructor reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='SkiaImageRegionDecoder']/constructor[@name='SkiaImageRegionDecoder' and count(parameter)=0]"
		[Register (".ctor", "()V", "")]
		public unsafe SkiaImageRegionDecoder ()
			: base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
		{
			if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
				return;

			try {
				if (((object) this).GetType () != typeof (SkiaImageRegionDecoder)) {
					SetHandle (
							global::Android.Runtime.JNIEnv.StartCreateInstance (((object) this).GetType (), "()V"),
							JniHandleOwnership.TransferLocalRef);
					global::Android.Runtime.JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, "()V");
					return;
				}

				if (id_ctor == IntPtr.Zero)
					id_ctor = JNIEnv.GetMethodID (class_ref, "<init>", "()V");
				SetHandle (
						global::Android.Runtime.JNIEnv.StartCreateInstance (class_ref, id_ctor),
						JniHandleOwnership.TransferLocalRef);
				JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, class_ref, id_ctor);
			} finally {
			}
		}

		static IntPtr id_ctor_Landroid_graphics_Bitmap_Config_;
		// Metadata.xml XPath constructor reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='SkiaImageRegionDecoder']/constructor[@name='SkiaImageRegionDecoder' and count(parameter)=1 and parameter[1][@type='android.graphics.Bitmap.Config']]"
		[Register (".ctor", "(Landroid/graphics/Bitmap$Config;)V", "")]
		public unsafe SkiaImageRegionDecoder (global::Android.Graphics.Bitmap.Config p0)
			: base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
		{
			if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
				return;

			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				if (((object) this).GetType () != typeof (SkiaImageRegionDecoder)) {
					SetHandle (
							global::Android.Runtime.JNIEnv.StartCreateInstance (((object) this).GetType (), "(Landroid/graphics/Bitmap$Config;)V", __args),
							JniHandleOwnership.TransferLocalRef);
					global::Android.Runtime.JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, "(Landroid/graphics/Bitmap$Config;)V", __args);
					return;
				}

				if (id_ctor_Landroid_graphics_Bitmap_Config_ == IntPtr.Zero)
					id_ctor_Landroid_graphics_Bitmap_Config_ = JNIEnv.GetMethodID (class_ref, "<init>", "(Landroid/graphics/Bitmap$Config;)V");
				SetHandle (
						global::Android.Runtime.JNIEnv.StartCreateInstance (class_ref, id_ctor_Landroid_graphics_Bitmap_Config_, __args),
						JniHandleOwnership.TransferLocalRef);
				JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, class_ref, id_ctor_Landroid_graphics_Bitmap_Config_, __args);
			} finally {
			}
		}

		static Delegate cb_isReady;
#pragma warning disable 0169
		static Delegate GetIsReadyHandler ()
		{
			if (cb_isReady == null)
				cb_isReady = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, bool>) n_IsReady);
			return cb_isReady;
		}

		static bool n_IsReady (IntPtr jnienv, IntPtr native__this)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.Decoder.SkiaImageRegionDecoder __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.Decoder.SkiaImageRegionDecoder> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			return __this.IsReady;
		}
#pragma warning restore 0169

		static IntPtr id_isReady;
		public virtual unsafe bool IsReady {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='SkiaImageRegionDecoder']/method[@name='isReady' and count(parameter)=0]"
			[Register ("isReady", "()Z", "GetIsReadyHandler")]
			get {
				if (id_isReady == IntPtr.Zero)
					id_isReady = JNIEnv.GetMethodID (class_ref, "isReady", "()Z");
				try {

					if (((object) this).GetType () == ThresholdType)
						return JNIEnv.CallBooleanMethod (((global::Java.Lang.Object) this).Handle, id_isReady);
					else
						return JNIEnv.CallNonvirtualBooleanMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "isReady", "()Z"));
				} finally {
				}
			}
		}

		static Delegate cb_decodeRegion_Landroid_graphics_Rect_I;
#pragma warning disable 0169
		static Delegate GetDecodeRegion_Landroid_graphics_Rect_IHandler ()
		{
			if (cb_decodeRegion_Landroid_graphics_Rect_I == null)
				cb_decodeRegion_Landroid_graphics_Rect_I = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, IntPtr, int, IntPtr>) n_DecodeRegion_Landroid_graphics_Rect_I);
			return cb_decodeRegion_Landroid_graphics_Rect_I;
		}

		static IntPtr n_DecodeRegion_Landroid_graphics_Rect_I (IntPtr jnienv, IntPtr native__this, IntPtr native_p0, int p1)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.Decoder.SkiaImageRegionDecoder __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.Decoder.SkiaImageRegionDecoder> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			global::Android.Graphics.Rect p0 = global::Java.Lang.Object.GetObject<global::Android.Graphics.Rect> (native_p0, JniHandleOwnership.DoNotTransfer);
			IntPtr __ret = JNIEnv.ToLocalJniHandle (__this.DecodeRegion (p0, p1));
			return __ret;
		}
#pragma warning restore 0169

		static IntPtr id_decodeRegion_Landroid_graphics_Rect_I;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='SkiaImageRegionDecoder']/method[@name='decodeRegion' and count(parameter)=2 and parameter[1][@type='android.graphics.Rect'] and parameter[2][@type='int']]"
		[Register ("decodeRegion", "(Landroid/graphics/Rect;I)Landroid/graphics/Bitmap;", "GetDecodeRegion_Landroid_graphics_Rect_IHandler")]
		public virtual unsafe global::Android.Graphics.Bitmap DecodeRegion (global::Android.Graphics.Rect p0, int p1)
		{
			if (id_decodeRegion_Landroid_graphics_Rect_I == IntPtr.Zero)
				id_decodeRegion_Landroid_graphics_Rect_I = JNIEnv.GetMethodID (class_ref, "decodeRegion", "(Landroid/graphics/Rect;I)Landroid/graphics/Bitmap;");
			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);

				global::Android.Graphics.Bitmap __ret;
				if (((object) this).GetType () == ThresholdType)
					__ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.Bitmap> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_decodeRegion_Landroid_graphics_Rect_I, __args), JniHandleOwnership.TransferLocalRef);
				else
					__ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.Bitmap> (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "decodeRegion", "(Landroid/graphics/Rect;I)Landroid/graphics/Bitmap;"), __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

		static Delegate cb_init_Landroid_content_Context_Landroid_net_Uri_;
#pragma warning disable 0169
		static Delegate GetInit_Landroid_content_Context_Landroid_net_Uri_Handler ()
		{
			if (cb_init_Landroid_content_Context_Landroid_net_Uri_ == null)
				cb_init_Landroid_content_Context_Landroid_net_Uri_ = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, IntPtr, IntPtr, IntPtr>) n_Init_Landroid_content_Context_Landroid_net_Uri_);
			return cb_init_Landroid_content_Context_Landroid_net_Uri_;
		}

		static IntPtr n_Init_Landroid_content_Context_Landroid_net_Uri_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0, IntPtr native_p1)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.Decoder.SkiaImageRegionDecoder __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.Decoder.SkiaImageRegionDecoder> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			global::Android.Content.Context p0 = global::Java.Lang.Object.GetObject<global::Android.Content.Context> (native_p0, JniHandleOwnership.DoNotTransfer);
			global::Android.Net.Uri p1 = global::Java.Lang.Object.GetObject<global::Android.Net.Uri> (native_p1, JniHandleOwnership.DoNotTransfer);
			IntPtr __ret = JNIEnv.ToLocalJniHandle (__this.Init (p0, p1));
			return __ret;
		}
#pragma warning restore 0169

		static IntPtr id_init_Landroid_content_Context_Landroid_net_Uri_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='SkiaImageRegionDecoder']/method[@name='init' and count(parameter)=2 and parameter[1][@type='android.content.Context'] and parameter[2][@type='android.net.Uri']]"
		[Register ("init", "(Landroid/content/Context;Landroid/net/Uri;)Landroid/graphics/Point;", "GetInit_Landroid_content_Context_Landroid_net_Uri_Handler")]
		public virtual unsafe global::Android.Graphics.Point Init (global::Android.Content.Context p0, global::Android.Net.Uri p1)
		{
			if (id_init_Landroid_content_Context_Landroid_net_Uri_ == IntPtr.Zero)
				id_init_Landroid_content_Context_Landroid_net_Uri_ = JNIEnv.GetMethodID (class_ref, "init", "(Landroid/content/Context;Landroid/net/Uri;)Landroid/graphics/Point;");
			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);

				global::Android.Graphics.Point __ret;
				if (((object) this).GetType () == ThresholdType)
					__ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.Point> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_init_Landroid_content_Context_Landroid_net_Uri_, __args), JniHandleOwnership.TransferLocalRef);
				else
					__ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.Point> (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "init", "(Landroid/content/Context;Landroid/net/Uri;)Landroid/graphics/Point;"), __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

		static Delegate cb_recycle;
#pragma warning disable 0169
		static Delegate GetRecycleHandler ()
		{
			if (cb_recycle == null)
				cb_recycle = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_Recycle);
			return cb_recycle;
		}

		static void n_Recycle (IntPtr jnienv, IntPtr native__this)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.Decoder.SkiaImageRegionDecoder __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.Decoder.SkiaImageRegionDecoder> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			__this.Recycle ();
		}
#pragma warning restore 0169

		static IntPtr id_recycle;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='SkiaImageRegionDecoder']/method[@name='recycle' and count(parameter)=0]"
		[Register ("recycle", "()V", "GetRecycleHandler")]
		public virtual unsafe void Recycle ()
		{
			if (id_recycle == IntPtr.Zero)
				id_recycle = JNIEnv.GetMethodID (class_ref, "recycle", "()V");
			try {

				if (((object) this).GetType () == ThresholdType)
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_recycle);
				else
					JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "recycle", "()V"));
			} finally {
			}
		}

	}
}
