using System;
using System.Collections.Generic;
using Android.Runtime;

namespace Com.Davemorrissey.Labs.Subscaleview.Decoder {

	// Metadata.xml XPath interface reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/interface[@name='ImageDecoder']"
	[Register ("com/davemorrissey/labs/subscaleview/decoder/ImageDecoder", "", "Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageDecoderInvoker")]
	public partial interface IImageDecoder : IJavaObject {

		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/interface[@name='ImageDecoder']/method[@name='decode' and count(parameter)=2 and parameter[1][@type='android.content.Context'] and parameter[2][@type='android.net.Uri']]"
		[Register ("decode", "(Landroid/content/Context;Landroid/net/Uri;)Landroid/graphics/Bitmap;", "GetDecode_Landroid_content_Context_Landroid_net_Uri_Handler:Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageDecoderInvoker, Subsampling-scale-image-view")]
		global::Android.Graphics.Bitmap Decode (global::Android.Content.Context p0, global::Android.Net.Uri p1);

	}

	[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/decoder/ImageDecoder", DoNotGenerateAcw=true)]
	internal class IImageDecoderInvoker : global::Java.Lang.Object, IImageDecoder {

		static IntPtr java_class_ref = JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/decoder/ImageDecoder");

		protected override IntPtr ThresholdClass {
			get { return class_ref; }
		}

		protected override global::System.Type ThresholdType {
			get { return typeof (IImageDecoderInvoker); }
		}

		IntPtr class_ref;

		public static IImageDecoder GetObject (IntPtr handle, JniHandleOwnership transfer)
		{
			return global::Java.Lang.Object.GetObject<IImageDecoder> (handle, transfer);
		}

		static IntPtr Validate (IntPtr handle)
		{
			if (!JNIEnv.IsInstanceOf (handle, java_class_ref))
				throw new InvalidCastException (string.Format ("Unable to convert instance of type '{0}' to type '{1}'.",
							JNIEnv.GetClassNameFromInstance (handle), "com.davemorrissey.labs.subscaleview.decoder.ImageDecoder"));
			return handle;
		}

		protected override void Dispose (bool disposing)
		{
			if (this.class_ref != IntPtr.Zero)
				JNIEnv.DeleteGlobalRef (this.class_ref);
			this.class_ref = IntPtr.Zero;
			base.Dispose (disposing);
		}

		public IImageDecoderInvoker (IntPtr handle, JniHandleOwnership transfer) : base (Validate (handle), transfer)
		{
			IntPtr local_ref = JNIEnv.GetObjectClass (((global::Java.Lang.Object) this).Handle);
			this.class_ref = JNIEnv.NewGlobalRef (local_ref);
			JNIEnv.DeleteLocalRef (local_ref);
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
			global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageDecoder __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IImageDecoder> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			global::Android.Content.Context p0 = global::Java.Lang.Object.GetObject<global::Android.Content.Context> (native_p0, JniHandleOwnership.DoNotTransfer);
			global::Android.Net.Uri p1 = global::Java.Lang.Object.GetObject<global::Android.Net.Uri> (native_p1, JniHandleOwnership.DoNotTransfer);
			IntPtr __ret = JNIEnv.ToLocalJniHandle (__this.Decode (p0, p1));
			return __ret;
		}
#pragma warning restore 0169

		IntPtr id_decode_Landroid_content_Context_Landroid_net_Uri_;
		public unsafe global::Android.Graphics.Bitmap Decode (global::Android.Content.Context p0, global::Android.Net.Uri p1)
		{
			if (id_decode_Landroid_content_Context_Landroid_net_Uri_ == IntPtr.Zero)
				id_decode_Landroid_content_Context_Landroid_net_Uri_ = JNIEnv.GetMethodID (class_ref, "decode", "(Landroid/content/Context;Landroid/net/Uri;)Landroid/graphics/Bitmap;");
			JValue* __args = stackalloc JValue [2];
			__args [0] = new JValue (p0);
			__args [1] = new JValue (p1);
			global::Android.Graphics.Bitmap __ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.Bitmap> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_decode_Landroid_content_Context_Landroid_net_Uri_, __args), JniHandleOwnership.TransferLocalRef);
			return __ret;
		}

	}

}
