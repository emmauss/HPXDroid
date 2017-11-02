using System;
using System.Collections.Generic;
using Android.Runtime;

namespace Com.Davemorrissey.Labs.Subscaleview.Decoder {

	// Metadata.xml XPath interface reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/interface[@name='DecoderFactory']"
	[Register ("com/davemorrissey/labs/subscaleview/decoder/DecoderFactory", "", "Com.Davemorrissey.Labs.Subscaleview.Decoder.IDecoderFactoryInvoker")]
	[global::Java.Interop.JavaTypeParameters (new string [] {"T"})]
	public partial interface IDecoderFactory : IJavaObject {

		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/interface[@name='DecoderFactory']/method[@name='make' and count(parameter)=0]"
		[Register ("make", "()Ljava/lang/Object;", "GetMakeHandler:Com.Davemorrissey.Labs.Subscaleview.Decoder.IDecoderFactoryInvoker, Subsampling-scale-image-view")]
		global::Java.Lang.Object Make ();

	}

	[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/decoder/DecoderFactory", DoNotGenerateAcw=true)]
	internal class IDecoderFactoryInvoker : global::Java.Lang.Object, IDecoderFactory {

		static IntPtr java_class_ref = JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/decoder/DecoderFactory");

		protected override IntPtr ThresholdClass {
			get { return class_ref; }
		}

		protected override global::System.Type ThresholdType {
			get { return typeof (IDecoderFactoryInvoker); }
		}

		IntPtr class_ref;

		public static IDecoderFactory GetObject (IntPtr handle, JniHandleOwnership transfer)
		{
			return global::Java.Lang.Object.GetObject<IDecoderFactory> (handle, transfer);
		}

		static IntPtr Validate (IntPtr handle)
		{
			if (!JNIEnv.IsInstanceOf (handle, java_class_ref))
				throw new InvalidCastException (string.Format ("Unable to convert instance of type '{0}' to type '{1}'.",
							JNIEnv.GetClassNameFromInstance (handle), "com.davemorrissey.labs.subscaleview.decoder.DecoderFactory"));
			return handle;
		}

		protected override void Dispose (bool disposing)
		{
			if (this.class_ref != IntPtr.Zero)
				JNIEnv.DeleteGlobalRef (this.class_ref);
			this.class_ref = IntPtr.Zero;
			base.Dispose (disposing);
		}

		public IDecoderFactoryInvoker (IntPtr handle, JniHandleOwnership transfer) : base (Validate (handle), transfer)
		{
			IntPtr local_ref = JNIEnv.GetObjectClass (((global::Java.Lang.Object) this).Handle);
			this.class_ref = JNIEnv.NewGlobalRef (local_ref);
			JNIEnv.DeleteLocalRef (local_ref);
		}

		static Delegate cb_make;
#pragma warning disable 0169
		static Delegate GetMakeHandler ()
		{
			if (cb_make == null)
				cb_make = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, IntPtr>) n_Make);
			return cb_make;
		}

		static IntPtr n_Make (IntPtr jnienv, IntPtr native__this)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IDecoderFactory __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IDecoderFactory> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			return JNIEnv.ToLocalJniHandle (__this.Make ());
		}
#pragma warning restore 0169

		IntPtr id_make;
		public unsafe global::Java.Lang.Object Make ()
		{
			if (id_make == IntPtr.Zero)
				id_make = JNIEnv.GetMethodID (class_ref, "make", "()Ljava/lang/Object;");
			return (Java.Lang.Object) global::Java.Lang.Object.GetObject<global::Java.Lang.Object> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_make), JniHandleOwnership.TransferLocalRef);
		}

	}

}
