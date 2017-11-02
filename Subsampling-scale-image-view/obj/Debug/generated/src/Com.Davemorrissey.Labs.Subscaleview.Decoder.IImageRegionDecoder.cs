using System;
using System.Collections.Generic;
using Android.Runtime;

namespace Com.Davemorrissey.Labs.Subscaleview.Decoder {

	// Metadata.xml XPath interface reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/interface[@name='ImageRegionDecoder']"
	[Register ("com/davemorrissey/labs/subscaleview/decoder/ImageRegionDecoder", "", "Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoderInvoker")]
	public partial interface IImageRegionDecoder : IJavaObject {

		bool IsReady {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/interface[@name='ImageRegionDecoder']/method[@name='isReady' and count(parameter)=0]"
			[Register ("isReady", "()Z", "GetIsReadyHandler:Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoderInvoker, Subsampling-scale-image-view")] get;
		}

		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/interface[@name='ImageRegionDecoder']/method[@name='decodeRegion' and count(parameter)=2 and parameter[1][@type='android.graphics.Rect'] and parameter[2][@type='int']]"
		[Register ("decodeRegion", "(Landroid/graphics/Rect;I)Landroid/graphics/Bitmap;", "GetDecodeRegion_Landroid_graphics_Rect_IHandler:Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoderInvoker, Subsampling-scale-image-view")]
		global::Android.Graphics.Bitmap DecodeRegion (global::Android.Graphics.Rect p0, int p1);

		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/interface[@name='ImageRegionDecoder']/method[@name='init' and count(parameter)=2 and parameter[1][@type='android.content.Context'] and parameter[2][@type='android.net.Uri']]"
		[Register ("init", "(Landroid/content/Context;Landroid/net/Uri;)Landroid/graphics/Point;", "GetInit_Landroid_content_Context_Landroid_net_Uri_Handler:Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoderInvoker, Subsampling-scale-image-view")]
		global::Android.Graphics.Point Init (global::Android.Content.Context p0, global::Android.Net.Uri p1);

		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/interface[@name='ImageRegionDecoder']/method[@name='recycle' and count(parameter)=0]"
		[Register ("recycle", "()V", "GetRecycleHandler:Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoderInvoker, Subsampling-scale-image-view")]
		void Recycle ();

	}

	[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/decoder/ImageRegionDecoder", DoNotGenerateAcw=true)]
	internal class IImageRegionDecoderInvoker : global::Java.Lang.Object, IImageRegionDecoder {

		static IntPtr java_class_ref = JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/decoder/ImageRegionDecoder");

		protected override IntPtr ThresholdClass {
			get { return class_ref; }
		}

		protected override global::System.Type ThresholdType {
			get { return typeof (IImageRegionDecoderInvoker); }
		}

		IntPtr class_ref;

		public static IImageRegionDecoder GetObject (IntPtr handle, JniHandleOwnership transfer)
		{
			return global::Java.Lang.Object.GetObject<IImageRegionDecoder> (handle, transfer);
		}

		static IntPtr Validate (IntPtr handle)
		{
			if (!JNIEnv.IsInstanceOf (handle, java_class_ref))
				throw new InvalidCastException (string.Format ("Unable to convert instance of type '{0}' to type '{1}'.",
							JNIEnv.GetClassNameFromInstance (handle), "com.davemorrissey.labs.subscaleview.decoder.ImageRegionDecoder"));
			return handle;
		}

		protected override void Dispose (bool disposing)
		{
			if (this.class_ref != IntPtr.Zero)
				JNIEnv.DeleteGlobalRef (this.class_ref);
			this.class_ref = IntPtr.Zero;
			base.Dispose (disposing);
		}

		public IImageRegionDecoderInvoker (IntPtr handle, JniHandleOwnership transfer) : base (Validate (handle), transfer)
		{
			IntPtr local_ref = JNIEnv.GetObjectClass (((global::Java.Lang.Object) this).Handle);
			this.class_ref = JNIEnv.NewGlobalRef (local_ref);
			JNIEnv.DeleteLocalRef (local_ref);
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
			global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoder __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoder> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			return __this.IsReady;
		}
#pragma warning restore 0169

		IntPtr id_isReady;
		public unsafe bool IsReady {
			get {
				if (id_isReady == IntPtr.Zero)
					id_isReady = JNIEnv.GetMethodID (class_ref, "isReady", "()Z");
				return JNIEnv.CallBooleanMethod (((global::Java.Lang.Object) this).Handle, id_isReady);
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
			global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoder __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoder> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			global::Android.Graphics.Rect p0 = global::Java.Lang.Object.GetObject<global::Android.Graphics.Rect> (native_p0, JniHandleOwnership.DoNotTransfer);
			IntPtr __ret = JNIEnv.ToLocalJniHandle (__this.DecodeRegion (p0, p1));
			return __ret;
		}
#pragma warning restore 0169

		IntPtr id_decodeRegion_Landroid_graphics_Rect_I;
		public unsafe global::Android.Graphics.Bitmap DecodeRegion (global::Android.Graphics.Rect p0, int p1)
		{
			if (id_decodeRegion_Landroid_graphics_Rect_I == IntPtr.Zero)
				id_decodeRegion_Landroid_graphics_Rect_I = JNIEnv.GetMethodID (class_ref, "decodeRegion", "(Landroid/graphics/Rect;I)Landroid/graphics/Bitmap;");
			JValue* __args = stackalloc JValue [2];
			__args [0] = new JValue (p0);
			__args [1] = new JValue (p1);
			global::Android.Graphics.Bitmap __ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.Bitmap> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_decodeRegion_Landroid_graphics_Rect_I, __args), JniHandleOwnership.TransferLocalRef);
			return __ret;
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
			global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoder __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoder> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			global::Android.Content.Context p0 = global::Java.Lang.Object.GetObject<global::Android.Content.Context> (native_p0, JniHandleOwnership.DoNotTransfer);
			global::Android.Net.Uri p1 = global::Java.Lang.Object.GetObject<global::Android.Net.Uri> (native_p1, JniHandleOwnership.DoNotTransfer);
			IntPtr __ret = JNIEnv.ToLocalJniHandle (__this.Init (p0, p1));
			return __ret;
		}
#pragma warning restore 0169

		IntPtr id_init_Landroid_content_Context_Landroid_net_Uri_;
		public unsafe global::Android.Graphics.Point Init (global::Android.Content.Context p0, global::Android.Net.Uri p1)
		{
			if (id_init_Landroid_content_Context_Landroid_net_Uri_ == IntPtr.Zero)
				id_init_Landroid_content_Context_Landroid_net_Uri_ = JNIEnv.GetMethodID (class_ref, "init", "(Landroid/content/Context;Landroid/net/Uri;)Landroid/graphics/Point;");
			JValue* __args = stackalloc JValue [2];
			__args [0] = new JValue (p0);
			__args [1] = new JValue (p1);
			global::Android.Graphics.Point __ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.Point> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_init_Landroid_content_Context_Landroid_net_Uri_, __args), JniHandleOwnership.TransferLocalRef);
			return __ret;
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
			global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoder __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageRegionDecoder> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			__this.Recycle ();
		}
#pragma warning restore 0169

		IntPtr id_recycle;
		public unsafe void Recycle ()
		{
			if (id_recycle == IntPtr.Zero)
				id_recycle = JNIEnv.GetMethodID (class_ref, "recycle", "()V");
			JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_recycle);
		}

	}

}
