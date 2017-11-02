using System;
using System.Collections.Generic;
using Android.Runtime;

namespace Com.Davemorrissey.Labs.Subscaleview.Decoder {

	// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='SkiaImageDecoder']"
	[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/decoder/SkiaImageDecoder", DoNotGenerateAcw=true)]
	public partial class SkiaImageDecoder : global::Java.Lang.Object, global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageDecoder {

		internal static new IntPtr java_class_handle;
		internal static new IntPtr class_ref {
			get {
				return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/decoder/SkiaImageDecoder", ref java_class_handle);
			}
		}

		protected override IntPtr ThresholdClass {
			get { return class_ref; }
		}

		protected override global::System.Type ThresholdType {
			get { return typeof (SkiaImageDecoder); }
		}

		protected SkiaImageDecoder (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

		static IntPtr id_ctor;
		// Metadata.xml XPath constructor reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='SkiaImageDecoder']/constructor[@name='SkiaImageDecoder' and count(parameter)=0]"
		[Register (".ctor", "()V", "")]
		public unsafe SkiaImageDecoder ()
			: base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
		{
			if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
				return;

			try {
				if (((object) this).GetType () != typeof (SkiaImageDecoder)) {
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
		// Metadata.xml XPath constructor reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='SkiaImageDecoder']/constructor[@name='SkiaImageDecoder' and count(parameter)=1 and parameter[1][@type='android.graphics.Bitmap.Config']]"
		[Register (".ctor", "(Landroid/graphics/Bitmap$Config;)V", "")]
		public unsafe SkiaImageDecoder (global::Android.Graphics.Bitmap.Config p0)
			: base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
		{
			if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
				return;

			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				if (((object) this).GetType () != typeof (SkiaImageDecoder)) {
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

		static Delegate cb_decode_Landroid_content_Context_Landroid_net_Uri_;
#pragma warning disable 0169
		static Delegate GetDecode_Landroid_content_Context_Landroid_net_Uri_Handler ()
		{
			if (cb_decode_Landroid_content_Context_Landroid_net_Uri_ == null)
				cb_decode_Landroid_content_Context_Landroid_net_Uri_ = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, IntPtr, IntPtr, IntPtr>) n_Decode_Landroid_content_Context_Landroid_net_Uri_);
			return cb_decode_Landroid_content_Context_Landroid_net_Uri_;
		}

		static IntPtr n_Decode_Landroid_content_Context_Landroid_net_Uri_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0, IntPtr native_p1)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.Decoder.SkiaImageDecoder __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.Decoder.SkiaImageDecoder> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			global::Android.Content.Context p0 = global::Java.Lang.Object.GetObject<global::Android.Content.Context> (native_p0, JniHandleOwnership.DoNotTransfer);
			global::Android.Net.Uri p1 = global::Java.Lang.Object.GetObject<global::Android.Net.Uri> (native_p1, JniHandleOwnership.DoNotTransfer);
			IntPtr __ret = JNIEnv.ToLocalJniHandle (__this.Decode (p0, p1));
			return __ret;
		}
#pragma warning restore 0169

		static IntPtr id_decode_Landroid_content_Context_Landroid_net_Uri_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='SkiaImageDecoder']/method[@name='decode' and count(parameter)=2 and parameter[1][@type='android.content.Context'] and parameter[2][@type='android.net.Uri']]"
		[Register ("decode", "(Landroid/content/Context;Landroid/net/Uri;)Landroid/graphics/Bitmap;", "GetDecode_Landroid_content_Context_Landroid_net_Uri_Handler")]
		public virtual unsafe global::Android.Graphics.Bitmap Decode (global::Android.Content.Context p0, global::Android.Net.Uri p1)
		{
			if (id_decode_Landroid_content_Context_Landroid_net_Uri_ == IntPtr.Zero)
				id_decode_Landroid_content_Context_Landroid_net_Uri_ = JNIEnv.GetMethodID (class_ref, "decode", "(Landroid/content/Context;Landroid/net/Uri;)Landroid/graphics/Bitmap;");
			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);

				global::Android.Graphics.Bitmap __ret;
				if (((object) this).GetType () == ThresholdType)
					__ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.Bitmap> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_decode_Landroid_content_Context_Landroid_net_Uri_, __args), JniHandleOwnership.TransferLocalRef);
				else
					__ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.Bitmap> (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "decode", "(Landroid/content/Context;Landroid/net/Uri;)Landroid/graphics/Bitmap;"), __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

	}
}
