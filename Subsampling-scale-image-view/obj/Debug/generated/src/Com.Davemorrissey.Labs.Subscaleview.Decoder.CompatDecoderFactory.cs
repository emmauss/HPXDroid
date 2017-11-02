using System;
using System.Collections.Generic;
using Android.Runtime;

namespace Com.Davemorrissey.Labs.Subscaleview.Decoder {

	// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='CompatDecoderFactory']"
	[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/decoder/CompatDecoderFactory", DoNotGenerateAcw=true)]
	[global::Java.Interop.JavaTypeParameters (new string [] {"T"})]
	public partial class CompatDecoderFactory : global::Java.Lang.Object, global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IDecoderFactory {

		internal static new IntPtr java_class_handle;
		internal static new IntPtr class_ref {
			get {
				return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/decoder/CompatDecoderFactory", ref java_class_handle);
			}
		}

		protected override IntPtr ThresholdClass {
			get { return class_ref; }
		}

		protected override global::System.Type ThresholdType {
			get { return typeof (CompatDecoderFactory); }
		}

		protected CompatDecoderFactory (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

		static IntPtr id_ctor_Ljava_lang_Class_;
		// Metadata.xml XPath constructor reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='CompatDecoderFactory']/constructor[@name='CompatDecoderFactory' and count(parameter)=1 and parameter[1][@type='java.lang.Class&lt;? extends T&gt;']]"
		[Register (".ctor", "(Ljava/lang/Class;)V", "")]
		public unsafe CompatDecoderFactory (global::Java.Lang.Class p0)
			: base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
		{
			if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
				return;

			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				if (((object) this).GetType () != typeof (CompatDecoderFactory)) {
					SetHandle (
							global::Android.Runtime.JNIEnv.StartCreateInstance (((object) this).GetType (), "(Ljava/lang/Class;)V", __args),
							JniHandleOwnership.TransferLocalRef);
					global::Android.Runtime.JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, "(Ljava/lang/Class;)V", __args);
					return;
				}

				if (id_ctor_Ljava_lang_Class_ == IntPtr.Zero)
					id_ctor_Ljava_lang_Class_ = JNIEnv.GetMethodID (class_ref, "<init>", "(Ljava/lang/Class;)V");
				SetHandle (
						global::Android.Runtime.JNIEnv.StartCreateInstance (class_ref, id_ctor_Ljava_lang_Class_, __args),
						JniHandleOwnership.TransferLocalRef);
				JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, class_ref, id_ctor_Ljava_lang_Class_, __args);
			} finally {
			}
		}

		static IntPtr id_ctor_Ljava_lang_Class_Landroid_graphics_Bitmap_Config_;
		// Metadata.xml XPath constructor reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='CompatDecoderFactory']/constructor[@name='CompatDecoderFactory' and count(parameter)=2 and parameter[1][@type='java.lang.Class&lt;? extends T&gt;'] and parameter[2][@type='android.graphics.Bitmap.Config']]"
		[Register (".ctor", "(Ljava/lang/Class;Landroid/graphics/Bitmap$Config;)V", "")]
		public unsafe CompatDecoderFactory (global::Java.Lang.Class p0, global::Android.Graphics.Bitmap.Config p1)
			: base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
		{
			if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
				return;

			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				if (((object) this).GetType () != typeof (CompatDecoderFactory)) {
					SetHandle (
							global::Android.Runtime.JNIEnv.StartCreateInstance (((object) this).GetType (), "(Ljava/lang/Class;Landroid/graphics/Bitmap$Config;)V", __args),
							JniHandleOwnership.TransferLocalRef);
					global::Android.Runtime.JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, "(Ljava/lang/Class;Landroid/graphics/Bitmap$Config;)V", __args);
					return;
				}

				if (id_ctor_Ljava_lang_Class_Landroid_graphics_Bitmap_Config_ == IntPtr.Zero)
					id_ctor_Ljava_lang_Class_Landroid_graphics_Bitmap_Config_ = JNIEnv.GetMethodID (class_ref, "<init>", "(Ljava/lang/Class;Landroid/graphics/Bitmap$Config;)V");
				SetHandle (
						global::Android.Runtime.JNIEnv.StartCreateInstance (class_ref, id_ctor_Ljava_lang_Class_Landroid_graphics_Bitmap_Config_, __args),
						JniHandleOwnership.TransferLocalRef);
				JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, class_ref, id_ctor_Ljava_lang_Class_Landroid_graphics_Bitmap_Config_, __args);
			} finally {
			}
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
			global::Com.Davemorrissey.Labs.Subscaleview.Decoder.CompatDecoderFactory __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.Decoder.CompatDecoderFactory> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			return JNIEnv.ToLocalJniHandle (__this.Make ());
		}
#pragma warning restore 0169

		static IntPtr id_make;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview.decoder']/class[@name='CompatDecoderFactory']/method[@name='make' and count(parameter)=0]"
		[Register ("make", "()Ljava/lang/Object;", "GetMakeHandler")]
		public virtual unsafe global::Java.Lang.Object Make ()
		{
			if (id_make == IntPtr.Zero)
				id_make = JNIEnv.GetMethodID (class_ref, "make", "()Ljava/lang/Object;");
			try {

				if (((object) this).GetType () == ThresholdType)
					return (Java.Lang.Object) global::Java.Lang.Object.GetObject<global::Java.Lang.Object> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_make), JniHandleOwnership.TransferLocalRef);
				else
					return (Java.Lang.Object) global::Java.Lang.Object.GetObject<global::Java.Lang.Object> (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "make", "()Ljava/lang/Object;")), JniHandleOwnership.TransferLocalRef);
			} finally {
			}
		}

	}
}
