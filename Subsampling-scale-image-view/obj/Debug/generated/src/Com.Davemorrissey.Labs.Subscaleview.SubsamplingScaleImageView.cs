using System;
using System.Collections.Generic;
using Android.Runtime;

namespace Com.Davemorrissey.Labs.Subscaleview {

	// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']"
	[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView", DoNotGenerateAcw=true)]
	public partial class SubsamplingScaleImageView : global::Android.Views.View {


		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='EASE_IN_OUT_QUAD']"
		[Register ("EASE_IN_OUT_QUAD")]
		public const int EaseInOutQuad = (int) 2;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='EASE_OUT_QUAD']"
		[Register ("EASE_OUT_QUAD")]
		public const int EaseOutQuad = (int) 1;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='ORIENTATION_0']"
		[Register ("ORIENTATION_0")]
		public const int Orientation0 = (int) 0;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='ORIENTATION_180']"
		[Register ("ORIENTATION_180")]
		public const int Orientation180 = (int) 180;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='ORIENTATION_270']"
		[Register ("ORIENTATION_270")]
		public const int Orientation270 = (int) 270;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='ORIENTATION_90']"
		[Register ("ORIENTATION_90")]
		public const int Orientation90 = (int) 90;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='ORIENTATION_USE_EXIF']"
		[Register ("ORIENTATION_USE_EXIF")]
		public const int OrientationUseExif = (int) -1;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='ORIGIN_ANIM']"
		[Register ("ORIGIN_ANIM")]
		public const int OriginAnim = (int) 1;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='ORIGIN_DOUBLE_TAP_ZOOM']"
		[Register ("ORIGIN_DOUBLE_TAP_ZOOM")]
		public const int OriginDoubleTapZoom = (int) 4;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='ORIGIN_FLING']"
		[Register ("ORIGIN_FLING")]
		public const int OriginFling = (int) 3;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='ORIGIN_TOUCH']"
		[Register ("ORIGIN_TOUCH")]
		public const int OriginTouch = (int) 2;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='PAN_LIMIT_CENTER']"
		[Register ("PAN_LIMIT_CENTER")]
		public const int PanLimitCenter = (int) 3;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='PAN_LIMIT_INSIDE']"
		[Register ("PAN_LIMIT_INSIDE")]
		public const int PanLimitInside = (int) 1;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='PAN_LIMIT_OUTSIDE']"
		[Register ("PAN_LIMIT_OUTSIDE")]
		public const int PanLimitOutside = (int) 2;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='SCALE_TYPE_CENTER_CROP']"
		[Register ("SCALE_TYPE_CENTER_CROP")]
		public const int ScaleTypeCenterCrop = (int) 2;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='SCALE_TYPE_CENTER_INSIDE']"
		[Register ("SCALE_TYPE_CENTER_INSIDE")]
		public const int ScaleTypeCenterInside = (int) 1;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='SCALE_TYPE_CUSTOM']"
		[Register ("SCALE_TYPE_CUSTOM")]
		public const int ScaleTypeCustom = (int) 3;

		static IntPtr TILE_SIZE_AUTO_jfieldId;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='TILE_SIZE_AUTO']"
		[Register ("TILE_SIZE_AUTO")]
		public static int TileSizeAuto {
			get {
				if (TILE_SIZE_AUTO_jfieldId == IntPtr.Zero)
					TILE_SIZE_AUTO_jfieldId = JNIEnv.GetStaticFieldID (class_ref, "TILE_SIZE_AUTO", "I");
				return JNIEnv.GetStaticIntField (class_ref, TILE_SIZE_AUTO_jfieldId);
			}
			set {
				if (TILE_SIZE_AUTO_jfieldId == IntPtr.Zero)
					TILE_SIZE_AUTO_jfieldId = JNIEnv.GetStaticFieldID (class_ref, "TILE_SIZE_AUTO", "I");
				try {
					JNIEnv.SetStaticField (class_ref, TILE_SIZE_AUTO_jfieldId, value);
				} finally {
				}
			}
		}

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='ZOOM_FOCUS_CENTER']"
		[Register ("ZOOM_FOCUS_CENTER")]
		public const int ZoomFocusCenter = (int) 2;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='ZOOM_FOCUS_CENTER_IMMEDIATE']"
		[Register ("ZOOM_FOCUS_CENTER_IMMEDIATE")]
		public const int ZoomFocusCenterImmediate = (int) 3;

		// Metadata.xml XPath field reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/field[@name='ZOOM_FOCUS_FIXED']"
		[Register ("ZOOM_FOCUS_FIXED")]
		public const int ZoomFocusFixed = (int) 1;
		// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.Anim']"
		[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$Anim", DoNotGenerateAcw=true)]
		public partial class Anim : global::Java.Lang.Object {

			internal static new IntPtr java_class_handle;
			internal static new IntPtr class_ref {
				get {
					return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$Anim", ref java_class_handle);
				}
			}

			protected override IntPtr ThresholdClass {
				get { return class_ref; }
			}

			protected override global::System.Type ThresholdType {
				get { return typeof (Anim); }
			}

			protected Anim (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

		}

		// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.AnimationBuilder']"
		[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder", DoNotGenerateAcw=true)]
		public sealed partial class AnimationBuilder : global::Java.Lang.Object {

			internal static new IntPtr java_class_handle;
			internal static new IntPtr class_ref {
				get {
					return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder", ref java_class_handle);
				}
			}

			protected override IntPtr ThresholdClass {
				get { return class_ref; }
			}

			protected override global::System.Type ThresholdType {
				get { return typeof (AnimationBuilder); }
			}

			internal AnimationBuilder (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

			static IntPtr id_start;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.AnimationBuilder']/method[@name='start' and count(parameter)=0]"
			[Register ("start", "()V", "")]
			public unsafe void Start ()
			{
				if (id_start == IntPtr.Zero)
					id_start = JNIEnv.GetMethodID (class_ref, "start", "()V");
				try {
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_start);
				} finally {
				}
			}

			static IntPtr id_withDuration_J;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.AnimationBuilder']/method[@name='withDuration' and count(parameter)=1 and parameter[1][@type='long']]"
			[Register ("withDuration", "(J)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;", "")]
			public unsafe global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder WithDuration (long p0)
			{
				if (id_withDuration_J == IntPtr.Zero)
					id_withDuration_J = JNIEnv.GetMethodID (class_ref, "withDuration", "(J)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (p0);
					return global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_withDuration_J, __args), JniHandleOwnership.TransferLocalRef);
				} finally {
				}
			}

			static IntPtr id_withEasing_I;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.AnimationBuilder']/method[@name='withEasing' and count(parameter)=1 and parameter[1][@type='int']]"
			[Register ("withEasing", "(I)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;", "")]
			public unsafe global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder WithEasing (int p0)
			{
				if (id_withEasing_I == IntPtr.Zero)
					id_withEasing_I = JNIEnv.GetMethodID (class_ref, "withEasing", "(I)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (p0);
					return global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_withEasing_I, __args), JniHandleOwnership.TransferLocalRef);
				} finally {
				}
			}

			static IntPtr id_withInterruptible_Z;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.AnimationBuilder']/method[@name='withInterruptible' and count(parameter)=1 and parameter[1][@type='boolean']]"
			[Register ("withInterruptible", "(Z)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;", "")]
			public unsafe global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder WithInterruptible (bool p0)
			{
				if (id_withInterruptible_Z == IntPtr.Zero)
					id_withInterruptible_Z = JNIEnv.GetMethodID (class_ref, "withInterruptible", "(Z)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (p0);
					return global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_withInterruptible_Z, __args), JniHandleOwnership.TransferLocalRef);
				} finally {
				}
			}

			static IntPtr id_withOnAnimationEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnAnimationEventListener_;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.AnimationBuilder']/method[@name='withOnAnimationEventListener' and count(parameter)=1 and parameter[1][@type='com.davemorrissey.labs.subscaleview.SubsamplingScaleImageView.OnAnimationEventListener']]"
			[Register ("withOnAnimationEventListener", "(Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnAnimationEventListener;)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;", "")]
			public unsafe global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder WithOnAnimationEventListener (global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnAnimationEventListener p0)
			{
				if (id_withOnAnimationEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnAnimationEventListener_ == IntPtr.Zero)
					id_withOnAnimationEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnAnimationEventListener_ = JNIEnv.GetMethodID (class_ref, "withOnAnimationEventListener", "(Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnAnimationEventListener;)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (p0);
					global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder __ret = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_withOnAnimationEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnAnimationEventListener_, __args), JniHandleOwnership.TransferLocalRef);
					return __ret;
				} finally {
				}
			}

		}

		// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.BitmapLoadTask']"
		[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$BitmapLoadTask", DoNotGenerateAcw=true)]
		public partial class BitmapLoadTask : global::Android.OS.AsyncTask {

			internal static new IntPtr java_class_handle;
			internal static new IntPtr class_ref {
				get {
					return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$BitmapLoadTask", ref java_class_handle);
				}
			}

			protected override IntPtr ThresholdClass {
				get { return class_ref; }
			}

			protected override global::System.Type ThresholdType {
				get { return typeof (BitmapLoadTask); }
			}

			protected BitmapLoadTask (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

			static Delegate cb_doInBackground_arrayLjava_lang_Object_;
#pragma warning disable 0169
			static Delegate GetDoInBackground_arrayLjava_lang_Object_Handler ()
			{
				if (cb_doInBackground_arrayLjava_lang_Object_ == null)
					cb_doInBackground_arrayLjava_lang_Object_ = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, IntPtr, IntPtr>) n_DoInBackground_arrayLjava_lang_Object_);
				return cb_doInBackground_arrayLjava_lang_Object_;
			}

			static IntPtr n_DoInBackground_arrayLjava_lang_Object_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.BitmapLoadTask __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.BitmapLoadTask> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Java.Lang.Object[] p0 = (global::Java.Lang.Object[]) JNIEnv.GetArray (native_p0, JniHandleOwnership.DoNotTransfer, typeof (global::Java.Lang.Object));
				IntPtr __ret = JNIEnv.ToLocalJniHandle (__this.DoInBackground (p0));
				if (p0 != null)
					JNIEnv.CopyArray (p0, native_p0);
				return __ret;
			}
#pragma warning restore 0169

			static IntPtr id_doInBackground_arrayLjava_lang_Object_;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.BitmapLoadTask']/method[@name='doInBackground' and count(parameter)=1 and parameter[1][@type='java.lang.Object[]']]"
			[Register ("doInBackground", "([Ljava/lang/Object;)Ljava/lang/Object;", "GetDoInBackground_arrayLjava_lang_Object_Handler")]
			protected override unsafe global::Java.Lang.Object DoInBackground (global::Java.Lang.Object[] p0)
			{
				if (id_doInBackground_arrayLjava_lang_Object_ == IntPtr.Zero)
					id_doInBackground_arrayLjava_lang_Object_ = JNIEnv.GetMethodID (class_ref, "doInBackground", "([Ljava/lang/Object;)Ljava/lang/Object;");
				IntPtr native_p0 = JNIEnv.NewArray (p0);
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (native_p0);

					global::Java.Lang.Object __ret;
					if (((object) this).GetType () == ThresholdType)
						__ret = global::Java.Lang.Object.GetObject<global::Java.Lang.Object> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_doInBackground_arrayLjava_lang_Object_, __args), JniHandleOwnership.TransferLocalRef);
					else
						__ret = global::Java.Lang.Object.GetObject<global::Java.Lang.Object> (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "doInBackground", "([Ljava/lang/Object;)Ljava/lang/Object;"), __args), JniHandleOwnership.TransferLocalRef);
					return __ret;
				} finally {
					if (p0 != null) {
						JNIEnv.CopyArray (native_p0, p0);
						JNIEnv.DeleteLocalRef (native_p0);
					}
				}
			}

			static Delegate cb_doInBackground_arrayLjava_lang_Void_;
#pragma warning disable 0169
			static Delegate GetDoInBackground_arrayLjava_lang_Void_Handler ()
			{
				if (cb_doInBackground_arrayLjava_lang_Void_ == null)
					cb_doInBackground_arrayLjava_lang_Void_ = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, IntPtr, IntPtr>) n_DoInBackground_arrayLjava_lang_Void_);
				return cb_doInBackground_arrayLjava_lang_Void_;
			}

			static IntPtr n_DoInBackground_arrayLjava_lang_Void_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.BitmapLoadTask __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.BitmapLoadTask> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Java.Lang.Void[] p0 = (global::Java.Lang.Void[]) JNIEnv.GetArray (native_p0, JniHandleOwnership.DoNotTransfer, typeof (global::Java.Lang.Void));
				IntPtr __ret = JNIEnv.ToLocalJniHandle (__this.DoInBackground (p0));
				if (p0 != null)
					JNIEnv.CopyArray (p0, native_p0);
				return __ret;
			}
#pragma warning restore 0169

			static IntPtr id_doInBackground_arrayLjava_lang_Void_;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.BitmapLoadTask']/method[@name='doInBackground' and count(parameter)=1 and parameter[1][@type='java.lang.Void...']]"
			[Register ("doInBackground", "([Ljava/lang/Void;)Ljava/lang/Integer;", "GetDoInBackground_arrayLjava_lang_Void_Handler")]
			protected virtual unsafe global::Java.Lang.Integer DoInBackground (params global:: Java.Lang.Void[] p0)
			{
				if (id_doInBackground_arrayLjava_lang_Void_ == IntPtr.Zero)
					id_doInBackground_arrayLjava_lang_Void_ = JNIEnv.GetMethodID (class_ref, "doInBackground", "([Ljava/lang/Void;)Ljava/lang/Integer;");
				IntPtr native_p0 = JNIEnv.NewArray (p0);
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (native_p0);

					global::Java.Lang.Integer __ret;
					if (((object) this).GetType () == ThresholdType)
						__ret = global::Java.Lang.Object.GetObject<global::Java.Lang.Integer> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_doInBackground_arrayLjava_lang_Void_, __args), JniHandleOwnership.TransferLocalRef);
					else
						__ret = global::Java.Lang.Object.GetObject<global::Java.Lang.Integer> (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "doInBackground", "([Ljava/lang/Void;)Ljava/lang/Integer;"), __args), JniHandleOwnership.TransferLocalRef);
					return __ret;
				} finally {
					if (p0 != null) {
						JNIEnv.CopyArray (native_p0, p0);
						JNIEnv.DeleteLocalRef (native_p0);
					}
				}
			}

			static Delegate cb_onPostExecute_Ljava_lang_Integer_;
#pragma warning disable 0169
			static Delegate GetOnPostExecute_Ljava_lang_Integer_Handler ()
			{
				if (cb_onPostExecute_Ljava_lang_Integer_ == null)
					cb_onPostExecute_Ljava_lang_Integer_ = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_OnPostExecute_Ljava_lang_Integer_);
				return cb_onPostExecute_Ljava_lang_Integer_;
			}

			static void n_OnPostExecute_Ljava_lang_Integer_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.BitmapLoadTask __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.BitmapLoadTask> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Java.Lang.Integer p0 = global::Java.Lang.Object.GetObject<global::Java.Lang.Integer> (native_p0, JniHandleOwnership.DoNotTransfer);
				__this.OnPostExecute (p0);
			}
#pragma warning restore 0169

			static IntPtr id_onPostExecute_Ljava_lang_Integer_;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.BitmapLoadTask']/method[@name='onPostExecute' and count(parameter)=1 and parameter[1][@type='java.lang.Integer']]"
			[Register ("onPostExecute", "(Ljava/lang/Integer;)V", "GetOnPostExecute_Ljava_lang_Integer_Handler")]
			protected virtual unsafe void OnPostExecute (global::Java.Lang.Integer p0)
			{
				if (id_onPostExecute_Ljava_lang_Integer_ == IntPtr.Zero)
					id_onPostExecute_Ljava_lang_Integer_ = JNIEnv.GetMethodID (class_ref, "onPostExecute", "(Ljava/lang/Integer;)V");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (p0);

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onPostExecute_Ljava_lang_Integer_, __args);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onPostExecute", "(Ljava/lang/Integer;)V"), __args);
				} finally {
				}
			}

		}

		// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnAnimationEventListener']"
		[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$DefaultOnAnimationEventListener", DoNotGenerateAcw=true)]
		public partial class DefaultOnAnimationEventListener : global::Java.Lang.Object, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnAnimationEventListener {

			internal static new IntPtr java_class_handle;
			internal static new IntPtr class_ref {
				get {
					return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$DefaultOnAnimationEventListener", ref java_class_handle);
				}
			}

			protected override IntPtr ThresholdClass {
				get { return class_ref; }
			}

			protected override global::System.Type ThresholdType {
				get { return typeof (DefaultOnAnimationEventListener); }
			}

			protected DefaultOnAnimationEventListener (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

			static IntPtr id_ctor;
			// Metadata.xml XPath constructor reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnAnimationEventListener']/constructor[@name='SubsamplingScaleImageView.DefaultOnAnimationEventListener' and count(parameter)=0]"
			[Register (".ctor", "()V", "")]
			public unsafe DefaultOnAnimationEventListener ()
				: base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
			{
				if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
					return;

				try {
					if (((object) this).GetType () != typeof (DefaultOnAnimationEventListener)) {
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

			static Delegate cb_onComplete;
#pragma warning disable 0169
			static Delegate GetOnCompleteHandler ()
			{
				if (cb_onComplete == null)
					cb_onComplete = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnComplete);
				return cb_onComplete;
			}

			static void n_OnComplete (IntPtr jnienv, IntPtr native__this)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnAnimationEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnAnimationEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnComplete ();
			}
#pragma warning restore 0169

			static IntPtr id_onComplete;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnAnimationEventListener']/method[@name='onComplete' and count(parameter)=0]"
			[Register ("onComplete", "()V", "GetOnCompleteHandler")]
			public virtual unsafe void OnComplete ()
			{
				if (id_onComplete == IntPtr.Zero)
					id_onComplete = JNIEnv.GetMethodID (class_ref, "onComplete", "()V");
				try {

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onComplete);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onComplete", "()V"));
				} finally {
				}
			}

			static Delegate cb_onInterruptedByNewAnim;
#pragma warning disable 0169
			static Delegate GetOnInterruptedByNewAnimHandler ()
			{
				if (cb_onInterruptedByNewAnim == null)
					cb_onInterruptedByNewAnim = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnInterruptedByNewAnim);
				return cb_onInterruptedByNewAnim;
			}

			static void n_OnInterruptedByNewAnim (IntPtr jnienv, IntPtr native__this)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnAnimationEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnAnimationEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnInterruptedByNewAnim ();
			}
#pragma warning restore 0169

			static IntPtr id_onInterruptedByNewAnim;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnAnimationEventListener']/method[@name='onInterruptedByNewAnim' and count(parameter)=0]"
			[Register ("onInterruptedByNewAnim", "()V", "GetOnInterruptedByNewAnimHandler")]
			public virtual unsafe void OnInterruptedByNewAnim ()
			{
				if (id_onInterruptedByNewAnim == IntPtr.Zero)
					id_onInterruptedByNewAnim = JNIEnv.GetMethodID (class_ref, "onInterruptedByNewAnim", "()V");
				try {

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onInterruptedByNewAnim);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onInterruptedByNewAnim", "()V"));
				} finally {
				}
			}

			static Delegate cb_onInterruptedByUser;
#pragma warning disable 0169
			static Delegate GetOnInterruptedByUserHandler ()
			{
				if (cb_onInterruptedByUser == null)
					cb_onInterruptedByUser = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnInterruptedByUser);
				return cb_onInterruptedByUser;
			}

			static void n_OnInterruptedByUser (IntPtr jnienv, IntPtr native__this)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnAnimationEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnAnimationEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnInterruptedByUser ();
			}
#pragma warning restore 0169

			static IntPtr id_onInterruptedByUser;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnAnimationEventListener']/method[@name='onInterruptedByUser' and count(parameter)=0]"
			[Register ("onInterruptedByUser", "()V", "GetOnInterruptedByUserHandler")]
			public virtual unsafe void OnInterruptedByUser ()
			{
				if (id_onInterruptedByUser == IntPtr.Zero)
					id_onInterruptedByUser = JNIEnv.GetMethodID (class_ref, "onInterruptedByUser", "()V");
				try {

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onInterruptedByUser);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onInterruptedByUser", "()V"));
				} finally {
				}
			}

		}

		// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnImageEventListener']"
		[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$DefaultOnImageEventListener", DoNotGenerateAcw=true)]
		public partial class DefaultOnImageEventListener : global::Java.Lang.Object, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener {

			internal static new IntPtr java_class_handle;
			internal static new IntPtr class_ref {
				get {
					return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$DefaultOnImageEventListener", ref java_class_handle);
				}
			}

			protected override IntPtr ThresholdClass {
				get { return class_ref; }
			}

			protected override global::System.Type ThresholdType {
				get { return typeof (DefaultOnImageEventListener); }
			}

			protected DefaultOnImageEventListener (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

			static IntPtr id_ctor;
			// Metadata.xml XPath constructor reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnImageEventListener']/constructor[@name='SubsamplingScaleImageView.DefaultOnImageEventListener' and count(parameter)=0]"
			[Register (".ctor", "()V", "")]
			public unsafe DefaultOnImageEventListener ()
				: base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
			{
				if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
					return;

				try {
					if (((object) this).GetType () != typeof (DefaultOnImageEventListener)) {
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

			static Delegate cb_onImageLoadError_Ljava_lang_Exception_;
#pragma warning disable 0169
			static Delegate GetOnImageLoadError_Ljava_lang_Exception_Handler ()
			{
				if (cb_onImageLoadError_Ljava_lang_Exception_ == null)
					cb_onImageLoadError_Ljava_lang_Exception_ = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_OnImageLoadError_Ljava_lang_Exception_);
				return cb_onImageLoadError_Ljava_lang_Exception_;
			}

			static void n_OnImageLoadError_Ljava_lang_Exception_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnImageEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnImageEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Java.Lang.Exception p0 = global::Java.Lang.Object.GetObject<global::Java.Lang.Exception> (native_p0, JniHandleOwnership.DoNotTransfer);
				__this.OnImageLoadError (p0);
			}
#pragma warning restore 0169

			static IntPtr id_onImageLoadError_Ljava_lang_Exception_;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnImageEventListener']/method[@name='onImageLoadError' and count(parameter)=1 and parameter[1][@type='java.lang.Exception']]"
			[Register ("onImageLoadError", "(Ljava/lang/Exception;)V", "GetOnImageLoadError_Ljava_lang_Exception_Handler")]
			public virtual unsafe void OnImageLoadError (global::Java.Lang.Exception p0)
			{
				if (id_onImageLoadError_Ljava_lang_Exception_ == IntPtr.Zero)
					id_onImageLoadError_Ljava_lang_Exception_ = JNIEnv.GetMethodID (class_ref, "onImageLoadError", "(Ljava/lang/Exception;)V");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (p0);

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onImageLoadError_Ljava_lang_Exception_, __args);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onImageLoadError", "(Ljava/lang/Exception;)V"), __args);
				} finally {
				}
			}

			static Delegate cb_onImageLoaded;
#pragma warning disable 0169
			static Delegate GetOnImageLoadedHandler ()
			{
				if (cb_onImageLoaded == null)
					cb_onImageLoaded = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnImageLoaded);
				return cb_onImageLoaded;
			}

			static void n_OnImageLoaded (IntPtr jnienv, IntPtr native__this)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnImageEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnImageEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnImageLoaded ();
			}
#pragma warning restore 0169

			static IntPtr id_onImageLoaded;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnImageEventListener']/method[@name='onImageLoaded' and count(parameter)=0]"
			[Register ("onImageLoaded", "()V", "GetOnImageLoadedHandler")]
			public virtual unsafe void OnImageLoaded ()
			{
				if (id_onImageLoaded == IntPtr.Zero)
					id_onImageLoaded = JNIEnv.GetMethodID (class_ref, "onImageLoaded", "()V");
				try {

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onImageLoaded);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onImageLoaded", "()V"));
				} finally {
				}
			}

			static Delegate cb_onPreviewLoadError_Ljava_lang_Exception_;
#pragma warning disable 0169
			static Delegate GetOnPreviewLoadError_Ljava_lang_Exception_Handler ()
			{
				if (cb_onPreviewLoadError_Ljava_lang_Exception_ == null)
					cb_onPreviewLoadError_Ljava_lang_Exception_ = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_OnPreviewLoadError_Ljava_lang_Exception_);
				return cb_onPreviewLoadError_Ljava_lang_Exception_;
			}

			static void n_OnPreviewLoadError_Ljava_lang_Exception_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnImageEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnImageEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Java.Lang.Exception p0 = global::Java.Lang.Object.GetObject<global::Java.Lang.Exception> (native_p0, JniHandleOwnership.DoNotTransfer);
				__this.OnPreviewLoadError (p0);
			}
#pragma warning restore 0169

			static IntPtr id_onPreviewLoadError_Ljava_lang_Exception_;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnImageEventListener']/method[@name='onPreviewLoadError' and count(parameter)=1 and parameter[1][@type='java.lang.Exception']]"
			[Register ("onPreviewLoadError", "(Ljava/lang/Exception;)V", "GetOnPreviewLoadError_Ljava_lang_Exception_Handler")]
			public virtual unsafe void OnPreviewLoadError (global::Java.Lang.Exception p0)
			{
				if (id_onPreviewLoadError_Ljava_lang_Exception_ == IntPtr.Zero)
					id_onPreviewLoadError_Ljava_lang_Exception_ = JNIEnv.GetMethodID (class_ref, "onPreviewLoadError", "(Ljava/lang/Exception;)V");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (p0);

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onPreviewLoadError_Ljava_lang_Exception_, __args);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onPreviewLoadError", "(Ljava/lang/Exception;)V"), __args);
				} finally {
				}
			}

			static Delegate cb_onPreviewReleased;
#pragma warning disable 0169
			static Delegate GetOnPreviewReleasedHandler ()
			{
				if (cb_onPreviewReleased == null)
					cb_onPreviewReleased = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnPreviewReleased);
				return cb_onPreviewReleased;
			}

			static void n_OnPreviewReleased (IntPtr jnienv, IntPtr native__this)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnImageEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnImageEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnPreviewReleased ();
			}
#pragma warning restore 0169

			static IntPtr id_onPreviewReleased;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnImageEventListener']/method[@name='onPreviewReleased' and count(parameter)=0]"
			[Register ("onPreviewReleased", "()V", "GetOnPreviewReleasedHandler")]
			public virtual unsafe void OnPreviewReleased ()
			{
				if (id_onPreviewReleased == IntPtr.Zero)
					id_onPreviewReleased = JNIEnv.GetMethodID (class_ref, "onPreviewReleased", "()V");
				try {

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onPreviewReleased);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onPreviewReleased", "()V"));
				} finally {
				}
			}

			static Delegate cb_onReady;
#pragma warning disable 0169
			static Delegate GetOnReadyHandler ()
			{
				if (cb_onReady == null)
					cb_onReady = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnReady);
				return cb_onReady;
			}

			static void n_OnReady (IntPtr jnienv, IntPtr native__this)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnImageEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnImageEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnReady ();
			}
#pragma warning restore 0169

			static IntPtr id_onReady;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnImageEventListener']/method[@name='onReady' and count(parameter)=0]"
			[Register ("onReady", "()V", "GetOnReadyHandler")]
			public virtual unsafe void OnReady ()
			{
				if (id_onReady == IntPtr.Zero)
					id_onReady = JNIEnv.GetMethodID (class_ref, "onReady", "()V");
				try {

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onReady);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onReady", "()V"));
				} finally {
				}
			}

			static Delegate cb_onTileLoadError_Ljava_lang_Exception_;
#pragma warning disable 0169
			static Delegate GetOnTileLoadError_Ljava_lang_Exception_Handler ()
			{
				if (cb_onTileLoadError_Ljava_lang_Exception_ == null)
					cb_onTileLoadError_Ljava_lang_Exception_ = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_OnTileLoadError_Ljava_lang_Exception_);
				return cb_onTileLoadError_Ljava_lang_Exception_;
			}

			static void n_OnTileLoadError_Ljava_lang_Exception_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnImageEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnImageEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Java.Lang.Exception p0 = global::Java.Lang.Object.GetObject<global::Java.Lang.Exception> (native_p0, JniHandleOwnership.DoNotTransfer);
				__this.OnTileLoadError (p0);
			}
#pragma warning restore 0169

			static IntPtr id_onTileLoadError_Ljava_lang_Exception_;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnImageEventListener']/method[@name='onTileLoadError' and count(parameter)=1 and parameter[1][@type='java.lang.Exception']]"
			[Register ("onTileLoadError", "(Ljava/lang/Exception;)V", "GetOnTileLoadError_Ljava_lang_Exception_Handler")]
			public virtual unsafe void OnTileLoadError (global::Java.Lang.Exception p0)
			{
				if (id_onTileLoadError_Ljava_lang_Exception_ == IntPtr.Zero)
					id_onTileLoadError_Ljava_lang_Exception_ = JNIEnv.GetMethodID (class_ref, "onTileLoadError", "(Ljava/lang/Exception;)V");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (p0);

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onTileLoadError_Ljava_lang_Exception_, __args);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onTileLoadError", "(Ljava/lang/Exception;)V"), __args);
				} finally {
				}
			}

		}

		// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnStateChangedListener']"
		[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$DefaultOnStateChangedListener", DoNotGenerateAcw=true)]
		public partial class DefaultOnStateChangedListener : global::Java.Lang.Object, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener {

			internal static new IntPtr java_class_handle;
			internal static new IntPtr class_ref {
				get {
					return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$DefaultOnStateChangedListener", ref java_class_handle);
				}
			}

			protected override IntPtr ThresholdClass {
				get { return class_ref; }
			}

			protected override global::System.Type ThresholdType {
				get { return typeof (DefaultOnStateChangedListener); }
			}

			protected DefaultOnStateChangedListener (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

			static IntPtr id_ctor;
			// Metadata.xml XPath constructor reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnStateChangedListener']/constructor[@name='SubsamplingScaleImageView.DefaultOnStateChangedListener' and count(parameter)=0]"
			[Register (".ctor", "()V", "")]
			public unsafe DefaultOnStateChangedListener ()
				: base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
			{
				if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
					return;

				try {
					if (((object) this).GetType () != typeof (DefaultOnStateChangedListener)) {
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

			static Delegate cb_onCenterChanged_Landroid_graphics_PointF_I;
#pragma warning disable 0169
			static Delegate GetOnCenterChanged_Landroid_graphics_PointF_IHandler ()
			{
				if (cb_onCenterChanged_Landroid_graphics_PointF_I == null)
					cb_onCenterChanged_Landroid_graphics_PointF_I = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr, int>) n_OnCenterChanged_Landroid_graphics_PointF_I);
				return cb_onCenterChanged_Landroid_graphics_PointF_I;
			}

			static void n_OnCenterChanged_Landroid_graphics_PointF_I (IntPtr jnienv, IntPtr native__this, IntPtr native_p0, int p1)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnStateChangedListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnStateChangedListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Android.Graphics.PointF p0 = global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (native_p0, JniHandleOwnership.DoNotTransfer);
				__this.OnCenterChanged (p0, p1);
			}
#pragma warning restore 0169

			static IntPtr id_onCenterChanged_Landroid_graphics_PointF_I;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnStateChangedListener']/method[@name='onCenterChanged' and count(parameter)=2 and parameter[1][@type='android.graphics.PointF'] and parameter[2][@type='int']]"
			[Register ("onCenterChanged", "(Landroid/graphics/PointF;I)V", "GetOnCenterChanged_Landroid_graphics_PointF_IHandler")]
			public virtual unsafe void OnCenterChanged (global::Android.Graphics.PointF p0, int p1)
			{
				if (id_onCenterChanged_Landroid_graphics_PointF_I == IntPtr.Zero)
					id_onCenterChanged_Landroid_graphics_PointF_I = JNIEnv.GetMethodID (class_ref, "onCenterChanged", "(Landroid/graphics/PointF;I)V");
				try {
					JValue* __args = stackalloc JValue [2];
					__args [0] = new JValue (p0);
					__args [1] = new JValue (p1);

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onCenterChanged_Landroid_graphics_PointF_I, __args);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onCenterChanged", "(Landroid/graphics/PointF;I)V"), __args);
				} finally {
				}
			}

			static Delegate cb_onScaleChanged_FI;
#pragma warning disable 0169
			static Delegate GetOnScaleChanged_FIHandler ()
			{
				if (cb_onScaleChanged_FI == null)
					cb_onScaleChanged_FI = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, float, int>) n_OnScaleChanged_FI);
				return cb_onScaleChanged_FI;
			}

			static void n_OnScaleChanged_FI (IntPtr jnienv, IntPtr native__this, float p0, int p1)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnStateChangedListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.DefaultOnStateChangedListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnScaleChanged (p0, p1);
			}
#pragma warning restore 0169

			static IntPtr id_onScaleChanged_FI;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.DefaultOnStateChangedListener']/method[@name='onScaleChanged' and count(parameter)=2 and parameter[1][@type='float'] and parameter[2][@type='int']]"
			[Register ("onScaleChanged", "(FI)V", "GetOnScaleChanged_FIHandler")]
			public virtual unsafe void OnScaleChanged (float p0, int p1)
			{
				if (id_onScaleChanged_FI == IntPtr.Zero)
					id_onScaleChanged_FI = JNIEnv.GetMethodID (class_ref, "onScaleChanged", "(FI)V");
				try {
					JValue* __args = stackalloc JValue [2];
					__args [0] = new JValue (p0);
					__args [1] = new JValue (p1);

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onScaleChanged_FI, __args);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onScaleChanged", "(FI)V"), __args);
				} finally {
				}
			}

		}

		// Metadata.xml XPath interface reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnAnimationEventListener']"
		[Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnAnimationEventListener", "", "Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnAnimationEventListenerInvoker")]
		public partial interface IOnAnimationEventListener : IJavaObject {

			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnAnimationEventListener']/method[@name='onComplete' and count(parameter)=0]"
			[Register ("onComplete", "()V", "GetOnCompleteHandler:Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnAnimationEventListenerInvoker, Subsampling-scale-image-view")]
			void OnComplete ();

			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnAnimationEventListener']/method[@name='onInterruptedByNewAnim' and count(parameter)=0]"
			[Register ("onInterruptedByNewAnim", "()V", "GetOnInterruptedByNewAnimHandler:Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnAnimationEventListenerInvoker, Subsampling-scale-image-view")]
			void OnInterruptedByNewAnim ();

			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnAnimationEventListener']/method[@name='onInterruptedByUser' and count(parameter)=0]"
			[Register ("onInterruptedByUser", "()V", "GetOnInterruptedByUserHandler:Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnAnimationEventListenerInvoker, Subsampling-scale-image-view")]
			void OnInterruptedByUser ();

		}

		[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnAnimationEventListener", DoNotGenerateAcw=true)]
		internal class IOnAnimationEventListenerInvoker : global::Java.Lang.Object, IOnAnimationEventListener {

			static IntPtr java_class_ref = JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnAnimationEventListener");

			protected override IntPtr ThresholdClass {
				get { return class_ref; }
			}

			protected override global::System.Type ThresholdType {
				get { return typeof (IOnAnimationEventListenerInvoker); }
			}

			IntPtr class_ref;

			public static IOnAnimationEventListener GetObject (IntPtr handle, JniHandleOwnership transfer)
			{
				return global::Java.Lang.Object.GetObject<IOnAnimationEventListener> (handle, transfer);
			}

			static IntPtr Validate (IntPtr handle)
			{
				if (!JNIEnv.IsInstanceOf (handle, java_class_ref))
					throw new InvalidCastException (string.Format ("Unable to convert instance of type '{0}' to type '{1}'.",
								JNIEnv.GetClassNameFromInstance (handle), "com.davemorrissey.labs.subscaleview.SubsamplingScaleImageView.OnAnimationEventListener"));
				return handle;
			}

			protected override void Dispose (bool disposing)
			{
				if (this.class_ref != IntPtr.Zero)
					JNIEnv.DeleteGlobalRef (this.class_ref);
				this.class_ref = IntPtr.Zero;
				base.Dispose (disposing);
			}

			public IOnAnimationEventListenerInvoker (IntPtr handle, JniHandleOwnership transfer) : base (Validate (handle), transfer)
			{
				IntPtr local_ref = JNIEnv.GetObjectClass (((global::Java.Lang.Object) this).Handle);
				this.class_ref = JNIEnv.NewGlobalRef (local_ref);
				JNIEnv.DeleteLocalRef (local_ref);
			}

			static Delegate cb_onComplete;
#pragma warning disable 0169
			static Delegate GetOnCompleteHandler ()
			{
				if (cb_onComplete == null)
					cb_onComplete = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnComplete);
				return cb_onComplete;
			}

			static void n_OnComplete (IntPtr jnienv, IntPtr native__this)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnAnimationEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnAnimationEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnComplete ();
			}
#pragma warning restore 0169

			IntPtr id_onComplete;
			public unsafe void OnComplete ()
			{
				if (id_onComplete == IntPtr.Zero)
					id_onComplete = JNIEnv.GetMethodID (class_ref, "onComplete", "()V");
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onComplete);
			}

			static Delegate cb_onInterruptedByNewAnim;
#pragma warning disable 0169
			static Delegate GetOnInterruptedByNewAnimHandler ()
			{
				if (cb_onInterruptedByNewAnim == null)
					cb_onInterruptedByNewAnim = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnInterruptedByNewAnim);
				return cb_onInterruptedByNewAnim;
			}

			static void n_OnInterruptedByNewAnim (IntPtr jnienv, IntPtr native__this)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnAnimationEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnAnimationEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnInterruptedByNewAnim ();
			}
#pragma warning restore 0169

			IntPtr id_onInterruptedByNewAnim;
			public unsafe void OnInterruptedByNewAnim ()
			{
				if (id_onInterruptedByNewAnim == IntPtr.Zero)
					id_onInterruptedByNewAnim = JNIEnv.GetMethodID (class_ref, "onInterruptedByNewAnim", "()V");
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onInterruptedByNewAnim);
			}

			static Delegate cb_onInterruptedByUser;
#pragma warning disable 0169
			static Delegate GetOnInterruptedByUserHandler ()
			{
				if (cb_onInterruptedByUser == null)
					cb_onInterruptedByUser = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnInterruptedByUser);
				return cb_onInterruptedByUser;
			}

			static void n_OnInterruptedByUser (IntPtr jnienv, IntPtr native__this)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnAnimationEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnAnimationEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnInterruptedByUser ();
			}
#pragma warning restore 0169

			IntPtr id_onInterruptedByUser;
			public unsafe void OnInterruptedByUser ()
			{
				if (id_onInterruptedByUser == IntPtr.Zero)
					id_onInterruptedByUser = JNIEnv.GetMethodID (class_ref, "onInterruptedByUser", "()V");
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onInterruptedByUser);
			}

		}

		[global::Android.Runtime.Register ("mono/com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView_OnAnimationEventListenerImplementor")]
		internal sealed partial class IOnAnimationEventListenerImplementor : global::Java.Lang.Object, IOnAnimationEventListener {

			object sender;

			public IOnAnimationEventListenerImplementor (object sender)
				: base (
					global::Android.Runtime.JNIEnv.StartCreateInstance ("mono/com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView_OnAnimationEventListenerImplementor", "()V"),
					JniHandleOwnership.TransferLocalRef)
			{
				global::Android.Runtime.JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, "()V");
				this.sender = sender;
			}

#pragma warning disable 0649
			public EventHandler OnCompleteHandler;
#pragma warning restore 0649

			public void OnComplete ()
			{
				var __h = OnCompleteHandler;
				if (__h != null)
					__h (sender, new EventArgs ());
			}
#pragma warning disable 0649
			public EventHandler OnInterruptedByNewAnimHandler;
#pragma warning restore 0649

			public void OnInterruptedByNewAnim ()
			{
				var __h = OnInterruptedByNewAnimHandler;
				if (__h != null)
					__h (sender, new EventArgs ());
			}
#pragma warning disable 0649
			public EventHandler OnInterruptedByUserHandler;
#pragma warning restore 0649

			public void OnInterruptedByUser ()
			{
				var __h = OnInterruptedByUserHandler;
				if (__h != null)
					__h (sender, new EventArgs ());
			}

			internal static bool __IsEmpty (IOnAnimationEventListenerImplementor value)
			{
				return value.OnCompleteHandler == null && value.OnInterruptedByNewAnimHandler == null && value.OnInterruptedByUserHandler == null;
			}
		}


		// Metadata.xml XPath interface reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnImageEventListener']"
		[Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnImageEventListener", "", "Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnImageEventListenerInvoker")]
		public partial interface IOnImageEventListener : IJavaObject {

			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnImageEventListener']/method[@name='onImageLoadError' and count(parameter)=1 and parameter[1][@type='java.lang.Exception']]"
			[Register ("onImageLoadError", "(Ljava/lang/Exception;)V", "GetOnImageLoadError_Ljava_lang_Exception_Handler:Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnImageEventListenerInvoker, Subsampling-scale-image-view")]
			void OnImageLoadError (global::Java.Lang.Exception p0);

			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnImageEventListener']/method[@name='onImageLoaded' and count(parameter)=0]"
			[Register ("onImageLoaded", "()V", "GetOnImageLoadedHandler:Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnImageEventListenerInvoker, Subsampling-scale-image-view")]
			void OnImageLoaded ();

			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnImageEventListener']/method[@name='onPreviewLoadError' and count(parameter)=1 and parameter[1][@type='java.lang.Exception']]"
			[Register ("onPreviewLoadError", "(Ljava/lang/Exception;)V", "GetOnPreviewLoadError_Ljava_lang_Exception_Handler:Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnImageEventListenerInvoker, Subsampling-scale-image-view")]
			void OnPreviewLoadError (global::Java.Lang.Exception p0);

			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnImageEventListener']/method[@name='onPreviewReleased' and count(parameter)=0]"
			[Register ("onPreviewReleased", "()V", "GetOnPreviewReleasedHandler:Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnImageEventListenerInvoker, Subsampling-scale-image-view")]
			void OnPreviewReleased ();

			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnImageEventListener']/method[@name='onReady' and count(parameter)=0]"
			[Register ("onReady", "()V", "GetOnReadyHandler:Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnImageEventListenerInvoker, Subsampling-scale-image-view")]
			void OnReady ();

			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnImageEventListener']/method[@name='onTileLoadError' and count(parameter)=1 and parameter[1][@type='java.lang.Exception']]"
			[Register ("onTileLoadError", "(Ljava/lang/Exception;)V", "GetOnTileLoadError_Ljava_lang_Exception_Handler:Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnImageEventListenerInvoker, Subsampling-scale-image-view")]
			void OnTileLoadError (global::Java.Lang.Exception p0);

		}

		[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnImageEventListener", DoNotGenerateAcw=true)]
		internal class IOnImageEventListenerInvoker : global::Java.Lang.Object, IOnImageEventListener {

			static IntPtr java_class_ref = JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnImageEventListener");

			protected override IntPtr ThresholdClass {
				get { return class_ref; }
			}

			protected override global::System.Type ThresholdType {
				get { return typeof (IOnImageEventListenerInvoker); }
			}

			IntPtr class_ref;

			public static IOnImageEventListener GetObject (IntPtr handle, JniHandleOwnership transfer)
			{
				return global::Java.Lang.Object.GetObject<IOnImageEventListener> (handle, transfer);
			}

			static IntPtr Validate (IntPtr handle)
			{
				if (!JNIEnv.IsInstanceOf (handle, java_class_ref))
					throw new InvalidCastException (string.Format ("Unable to convert instance of type '{0}' to type '{1}'.",
								JNIEnv.GetClassNameFromInstance (handle), "com.davemorrissey.labs.subscaleview.SubsamplingScaleImageView.OnImageEventListener"));
				return handle;
			}

			protected override void Dispose (bool disposing)
			{
				if (this.class_ref != IntPtr.Zero)
					JNIEnv.DeleteGlobalRef (this.class_ref);
				this.class_ref = IntPtr.Zero;
				base.Dispose (disposing);
			}

			public IOnImageEventListenerInvoker (IntPtr handle, JniHandleOwnership transfer) : base (Validate (handle), transfer)
			{
				IntPtr local_ref = JNIEnv.GetObjectClass (((global::Java.Lang.Object) this).Handle);
				this.class_ref = JNIEnv.NewGlobalRef (local_ref);
				JNIEnv.DeleteLocalRef (local_ref);
			}

			static Delegate cb_onImageLoadError_Ljava_lang_Exception_;
#pragma warning disable 0169
			static Delegate GetOnImageLoadError_Ljava_lang_Exception_Handler ()
			{
				if (cb_onImageLoadError_Ljava_lang_Exception_ == null)
					cb_onImageLoadError_Ljava_lang_Exception_ = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_OnImageLoadError_Ljava_lang_Exception_);
				return cb_onImageLoadError_Ljava_lang_Exception_;
			}

			static void n_OnImageLoadError_Ljava_lang_Exception_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Java.Lang.Exception p0 = global::Java.Lang.Object.GetObject<global::Java.Lang.Exception> (native_p0, JniHandleOwnership.DoNotTransfer);
				__this.OnImageLoadError (p0);
			}
#pragma warning restore 0169

			IntPtr id_onImageLoadError_Ljava_lang_Exception_;
			public unsafe void OnImageLoadError (global::Java.Lang.Exception p0)
			{
				if (id_onImageLoadError_Ljava_lang_Exception_ == IntPtr.Zero)
					id_onImageLoadError_Ljava_lang_Exception_ = JNIEnv.GetMethodID (class_ref, "onImageLoadError", "(Ljava/lang/Exception;)V");
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onImageLoadError_Ljava_lang_Exception_, __args);
			}

			static Delegate cb_onImageLoaded;
#pragma warning disable 0169
			static Delegate GetOnImageLoadedHandler ()
			{
				if (cb_onImageLoaded == null)
					cb_onImageLoaded = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnImageLoaded);
				return cb_onImageLoaded;
			}

			static void n_OnImageLoaded (IntPtr jnienv, IntPtr native__this)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnImageLoaded ();
			}
#pragma warning restore 0169

			IntPtr id_onImageLoaded;
			public unsafe void OnImageLoaded ()
			{
				if (id_onImageLoaded == IntPtr.Zero)
					id_onImageLoaded = JNIEnv.GetMethodID (class_ref, "onImageLoaded", "()V");
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onImageLoaded);
			}

			static Delegate cb_onPreviewLoadError_Ljava_lang_Exception_;
#pragma warning disable 0169
			static Delegate GetOnPreviewLoadError_Ljava_lang_Exception_Handler ()
			{
				if (cb_onPreviewLoadError_Ljava_lang_Exception_ == null)
					cb_onPreviewLoadError_Ljava_lang_Exception_ = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_OnPreviewLoadError_Ljava_lang_Exception_);
				return cb_onPreviewLoadError_Ljava_lang_Exception_;
			}

			static void n_OnPreviewLoadError_Ljava_lang_Exception_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Java.Lang.Exception p0 = global::Java.Lang.Object.GetObject<global::Java.Lang.Exception> (native_p0, JniHandleOwnership.DoNotTransfer);
				__this.OnPreviewLoadError (p0);
			}
#pragma warning restore 0169

			IntPtr id_onPreviewLoadError_Ljava_lang_Exception_;
			public unsafe void OnPreviewLoadError (global::Java.Lang.Exception p0)
			{
				if (id_onPreviewLoadError_Ljava_lang_Exception_ == IntPtr.Zero)
					id_onPreviewLoadError_Ljava_lang_Exception_ = JNIEnv.GetMethodID (class_ref, "onPreviewLoadError", "(Ljava/lang/Exception;)V");
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onPreviewLoadError_Ljava_lang_Exception_, __args);
			}

			static Delegate cb_onPreviewReleased;
#pragma warning disable 0169
			static Delegate GetOnPreviewReleasedHandler ()
			{
				if (cb_onPreviewReleased == null)
					cb_onPreviewReleased = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnPreviewReleased);
				return cb_onPreviewReleased;
			}

			static void n_OnPreviewReleased (IntPtr jnienv, IntPtr native__this)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnPreviewReleased ();
			}
#pragma warning restore 0169

			IntPtr id_onPreviewReleased;
			public unsafe void OnPreviewReleased ()
			{
				if (id_onPreviewReleased == IntPtr.Zero)
					id_onPreviewReleased = JNIEnv.GetMethodID (class_ref, "onPreviewReleased", "()V");
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onPreviewReleased);
			}

			static Delegate cb_onReady;
#pragma warning disable 0169
			static Delegate GetOnReadyHandler ()
			{
				if (cb_onReady == null)
					cb_onReady = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnReady);
				return cb_onReady;
			}

			static void n_OnReady (IntPtr jnienv, IntPtr native__this)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnReady ();
			}
#pragma warning restore 0169

			IntPtr id_onReady;
			public unsafe void OnReady ()
			{
				if (id_onReady == IntPtr.Zero)
					id_onReady = JNIEnv.GetMethodID (class_ref, "onReady", "()V");
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onReady);
			}

			static Delegate cb_onTileLoadError_Ljava_lang_Exception_;
#pragma warning disable 0169
			static Delegate GetOnTileLoadError_Ljava_lang_Exception_Handler ()
			{
				if (cb_onTileLoadError_Ljava_lang_Exception_ == null)
					cb_onTileLoadError_Ljava_lang_Exception_ = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_OnTileLoadError_Ljava_lang_Exception_);
				return cb_onTileLoadError_Ljava_lang_Exception_;
			}

			static void n_OnTileLoadError_Ljava_lang_Exception_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Java.Lang.Exception p0 = global::Java.Lang.Object.GetObject<global::Java.Lang.Exception> (native_p0, JniHandleOwnership.DoNotTransfer);
				__this.OnTileLoadError (p0);
			}
#pragma warning restore 0169

			IntPtr id_onTileLoadError_Ljava_lang_Exception_;
			public unsafe void OnTileLoadError (global::Java.Lang.Exception p0)
			{
				if (id_onTileLoadError_Ljava_lang_Exception_ == IntPtr.Zero)
					id_onTileLoadError_Ljava_lang_Exception_ = JNIEnv.GetMethodID (class_ref, "onTileLoadError", "(Ljava/lang/Exception;)V");
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onTileLoadError_Ljava_lang_Exception_, __args);
			}

		}

		public partial class ImageLoadErrorEventArgs : global::System.EventArgs {

			public ImageLoadErrorEventArgs (global::Java.Lang.Exception p0)
			{
				this.p0 = p0;
			}

			global::Java.Lang.Exception p0;
			public global::Java.Lang.Exception P0 {
				get { return p0; }
			}
		}

		public partial class PreviewLoadErrorEventArgs : global::System.EventArgs {

			public PreviewLoadErrorEventArgs (global::Java.Lang.Exception p0)
			{
				this.p0 = p0;
			}

			global::Java.Lang.Exception p0;
			public global::Java.Lang.Exception P0 {
				get { return p0; }
			}
		}

		public partial class TileLoadErrorEventArgs : global::System.EventArgs {

			public TileLoadErrorEventArgs (global::Java.Lang.Exception p0)
			{
				this.p0 = p0;
			}

			global::Java.Lang.Exception p0;
			public global::Java.Lang.Exception P0 {
				get { return p0; }
			}
		}

		[global::Android.Runtime.Register ("mono/com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView_OnImageEventListenerImplementor")]
		internal sealed partial class IOnImageEventListenerImplementor : global::Java.Lang.Object, IOnImageEventListener {

			object sender;

			public IOnImageEventListenerImplementor (object sender)
				: base (
					global::Android.Runtime.JNIEnv.StartCreateInstance ("mono/com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView_OnImageEventListenerImplementor", "()V"),
					JniHandleOwnership.TransferLocalRef)
			{
				global::Android.Runtime.JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, "()V");
				this.sender = sender;
			}

#pragma warning disable 0649
			public EventHandler<ImageLoadErrorEventArgs> OnImageLoadErrorHandler;
#pragma warning restore 0649

			public void OnImageLoadError (global::Java.Lang.Exception p0)
			{
				var __h = OnImageLoadErrorHandler;
				if (__h != null)
					__h (sender, new ImageLoadErrorEventArgs (p0));
			}
#pragma warning disable 0649
			public EventHandler OnImageLoadedHandler;
#pragma warning restore 0649

			public void OnImageLoaded ()
			{
				var __h = OnImageLoadedHandler;
				if (__h != null)
					__h (sender, new EventArgs ());
			}
#pragma warning disable 0649
			public EventHandler<PreviewLoadErrorEventArgs> OnPreviewLoadErrorHandler;
#pragma warning restore 0649

			public void OnPreviewLoadError (global::Java.Lang.Exception p0)
			{
				var __h = OnPreviewLoadErrorHandler;
				if (__h != null)
					__h (sender, new PreviewLoadErrorEventArgs (p0));
			}
#pragma warning disable 0649
			public EventHandler OnPreviewReleasedHandler;
#pragma warning restore 0649

			public void OnPreviewReleased ()
			{
				var __h = OnPreviewReleasedHandler;
				if (__h != null)
					__h (sender, new EventArgs ());
			}
#pragma warning disable 0649
			public EventHandler OnReadyHandler;
#pragma warning restore 0649

			public void OnReady ()
			{
				var __h = OnReadyHandler;
				if (__h != null)
					__h (sender, new EventArgs ());
			}
#pragma warning disable 0649
			public EventHandler<TileLoadErrorEventArgs> OnTileLoadErrorHandler;
#pragma warning restore 0649

			public void OnTileLoadError (global::Java.Lang.Exception p0)
			{
				var __h = OnTileLoadErrorHandler;
				if (__h != null)
					__h (sender, new TileLoadErrorEventArgs (p0));
			}

			internal static bool __IsEmpty (IOnImageEventListenerImplementor value)
			{
				return value.OnImageLoadErrorHandler == null && value.OnImageLoadedHandler == null && value.OnPreviewLoadErrorHandler == null && value.OnPreviewReleasedHandler == null && value.OnReadyHandler == null && value.OnTileLoadErrorHandler == null;
			}
		}


		// Metadata.xml XPath interface reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnStateChangedListener']"
		[Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnStateChangedListener", "", "Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnStateChangedListenerInvoker")]
		public partial interface IOnStateChangedListener : IJavaObject {

			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnStateChangedListener']/method[@name='onCenterChanged' and count(parameter)=2 and parameter[1][@type='android.graphics.PointF'] and parameter[2][@type='int']]"
			[Register ("onCenterChanged", "(Landroid/graphics/PointF;I)V", "GetOnCenterChanged_Landroid_graphics_PointF_IHandler:Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnStateChangedListenerInvoker, Subsampling-scale-image-view")]
			void OnCenterChanged (global::Android.Graphics.PointF p0, int p1);

			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/interface[@name='SubsamplingScaleImageView.OnStateChangedListener']/method[@name='onScaleChanged' and count(parameter)=2 and parameter[1][@type='float'] and parameter[2][@type='int']]"
			[Register ("onScaleChanged", "(FI)V", "GetOnScaleChanged_FIHandler:Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView/IOnStateChangedListenerInvoker, Subsampling-scale-image-view")]
			void OnScaleChanged (float p0, int p1);

		}

		[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnStateChangedListener", DoNotGenerateAcw=true)]
		internal class IOnStateChangedListenerInvoker : global::Java.Lang.Object, IOnStateChangedListener {

			static IntPtr java_class_ref = JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnStateChangedListener");

			protected override IntPtr ThresholdClass {
				get { return class_ref; }
			}

			protected override global::System.Type ThresholdType {
				get { return typeof (IOnStateChangedListenerInvoker); }
			}

			IntPtr class_ref;

			public static IOnStateChangedListener GetObject (IntPtr handle, JniHandleOwnership transfer)
			{
				return global::Java.Lang.Object.GetObject<IOnStateChangedListener> (handle, transfer);
			}

			static IntPtr Validate (IntPtr handle)
			{
				if (!JNIEnv.IsInstanceOf (handle, java_class_ref))
					throw new InvalidCastException (string.Format ("Unable to convert instance of type '{0}' to type '{1}'.",
								JNIEnv.GetClassNameFromInstance (handle), "com.davemorrissey.labs.subscaleview.SubsamplingScaleImageView.OnStateChangedListener"));
				return handle;
			}

			protected override void Dispose (bool disposing)
			{
				if (this.class_ref != IntPtr.Zero)
					JNIEnv.DeleteGlobalRef (this.class_ref);
				this.class_ref = IntPtr.Zero;
				base.Dispose (disposing);
			}

			public IOnStateChangedListenerInvoker (IntPtr handle, JniHandleOwnership transfer) : base (Validate (handle), transfer)
			{
				IntPtr local_ref = JNIEnv.GetObjectClass (((global::Java.Lang.Object) this).Handle);
				this.class_ref = JNIEnv.NewGlobalRef (local_ref);
				JNIEnv.DeleteLocalRef (local_ref);
			}

			static Delegate cb_onCenterChanged_Landroid_graphics_PointF_I;
#pragma warning disable 0169
			static Delegate GetOnCenterChanged_Landroid_graphics_PointF_IHandler ()
			{
				if (cb_onCenterChanged_Landroid_graphics_PointF_I == null)
					cb_onCenterChanged_Landroid_graphics_PointF_I = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr, int>) n_OnCenterChanged_Landroid_graphics_PointF_I);
				return cb_onCenterChanged_Landroid_graphics_PointF_I;
			}

			static void n_OnCenterChanged_Landroid_graphics_PointF_I (IntPtr jnienv, IntPtr native__this, IntPtr native_p0, int p1)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Android.Graphics.PointF p0 = global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (native_p0, JniHandleOwnership.DoNotTransfer);
				__this.OnCenterChanged (p0, p1);
			}
#pragma warning restore 0169

			IntPtr id_onCenterChanged_Landroid_graphics_PointF_I;
			public unsafe void OnCenterChanged (global::Android.Graphics.PointF p0, int p1)
			{
				if (id_onCenterChanged_Landroid_graphics_PointF_I == IntPtr.Zero)
					id_onCenterChanged_Landroid_graphics_PointF_I = JNIEnv.GetMethodID (class_ref, "onCenterChanged", "(Landroid/graphics/PointF;I)V");
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onCenterChanged_Landroid_graphics_PointF_I, __args);
			}

			static Delegate cb_onScaleChanged_FI;
#pragma warning disable 0169
			static Delegate GetOnScaleChanged_FIHandler ()
			{
				if (cb_onScaleChanged_FI == null)
					cb_onScaleChanged_FI = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, float, int>) n_OnScaleChanged_FI);
				return cb_onScaleChanged_FI;
			}

			static void n_OnScaleChanged_FI (IntPtr jnienv, IntPtr native__this, float p0, int p1)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				__this.OnScaleChanged (p0, p1);
			}
#pragma warning restore 0169

			IntPtr id_onScaleChanged_FI;
			public unsafe void OnScaleChanged (float p0, int p1)
			{
				if (id_onScaleChanged_FI == IntPtr.Zero)
					id_onScaleChanged_FI = JNIEnv.GetMethodID (class_ref, "onScaleChanged", "(FI)V");
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onScaleChanged_FI, __args);
			}

		}

		public partial class CenterChangedEventArgs : global::System.EventArgs {

			public CenterChangedEventArgs (global::Android.Graphics.PointF p0, int p1)
			{
				this.p0 = p0;
				this.p1 = p1;
			}

			global::Android.Graphics.PointF p0;
			public global::Android.Graphics.PointF P0 {
				get { return p0; }
			}

			int p1;
			public int P1 {
				get { return p1; }
			}
		}

		public partial class ScaleChangedEventArgs : global::System.EventArgs {

			public ScaleChangedEventArgs (float p0, int p1)
			{
				this.p0 = p0;
				this.p1 = p1;
			}

			float p0;
			public float P0 {
				get { return p0; }
			}

			int p1;
			public int P1 {
				get { return p1; }
			}
		}

		[global::Android.Runtime.Register ("mono/com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView_OnStateChangedListenerImplementor")]
		internal sealed partial class IOnStateChangedListenerImplementor : global::Java.Lang.Object, IOnStateChangedListener {

			object sender;

			public IOnStateChangedListenerImplementor (object sender)
				: base (
					global::Android.Runtime.JNIEnv.StartCreateInstance ("mono/com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView_OnStateChangedListenerImplementor", "()V"),
					JniHandleOwnership.TransferLocalRef)
			{
				global::Android.Runtime.JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, "()V");
				this.sender = sender;
			}

#pragma warning disable 0649
			public EventHandler<CenterChangedEventArgs> OnCenterChangedHandler;
#pragma warning restore 0649

			public void OnCenterChanged (global::Android.Graphics.PointF p0, int p1)
			{
				var __h = OnCenterChangedHandler;
				if (__h != null)
					__h (sender, new CenterChangedEventArgs (p0, p1));
			}
#pragma warning disable 0649
			public EventHandler<ScaleChangedEventArgs> OnScaleChangedHandler;
#pragma warning restore 0649

			public void OnScaleChanged (float p0, int p1)
			{
				var __h = OnScaleChangedHandler;
				if (__h != null)
					__h (sender, new ScaleChangedEventArgs (p0, p1));
			}

			internal static bool __IsEmpty (IOnStateChangedListenerImplementor value)
			{
				return value.OnCenterChangedHandler == null && value.OnScaleChangedHandler == null;
			}
		}


		// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.ScaleAndTranslate']"
		[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$ScaleAndTranslate", DoNotGenerateAcw=true)]
		public partial class ScaleAndTranslate : global::Java.Lang.Object {

			internal static new IntPtr java_class_handle;
			internal static new IntPtr class_ref {
				get {
					return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$ScaleAndTranslate", ref java_class_handle);
				}
			}

			protected override IntPtr ThresholdClass {
				get { return class_ref; }
			}

			protected override global::System.Type ThresholdType {
				get { return typeof (ScaleAndTranslate); }
			}

			protected ScaleAndTranslate (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

		}

		// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.Tile']"
		[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$Tile", DoNotGenerateAcw=true)]
		public partial class Tile : global::Java.Lang.Object {

			internal static new IntPtr java_class_handle;
			internal static new IntPtr class_ref {
				get {
					return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$Tile", ref java_class_handle);
				}
			}

			protected override IntPtr ThresholdClass {
				get { return class_ref; }
			}

			protected override global::System.Type ThresholdType {
				get { return typeof (Tile); }
			}

			protected Tile (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

		}

		// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.TileLoadTask']"
		[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$TileLoadTask", DoNotGenerateAcw=true)]
		public partial class TileLoadTask : global::Android.OS.AsyncTask {

			internal static new IntPtr java_class_handle;
			internal static new IntPtr class_ref {
				get {
					return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$TileLoadTask", ref java_class_handle);
				}
			}

			protected override IntPtr ThresholdClass {
				get { return class_ref; }
			}

			protected override global::System.Type ThresholdType {
				get { return typeof (TileLoadTask); }
			}

			protected TileLoadTask (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

			static Delegate cb_doInBackground_arrayLjava_lang_Object_;
#pragma warning disable 0169
			static Delegate GetDoInBackground_arrayLjava_lang_Object_Handler ()
			{
				if (cb_doInBackground_arrayLjava_lang_Object_ == null)
					cb_doInBackground_arrayLjava_lang_Object_ = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, IntPtr, IntPtr>) n_DoInBackground_arrayLjava_lang_Object_);
				return cb_doInBackground_arrayLjava_lang_Object_;
			}

			static IntPtr n_DoInBackground_arrayLjava_lang_Object_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.TileLoadTask __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.TileLoadTask> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Java.Lang.Object[] p0 = (global::Java.Lang.Object[]) JNIEnv.GetArray (native_p0, JniHandleOwnership.DoNotTransfer, typeof (global::Java.Lang.Object));
				IntPtr __ret = JNIEnv.ToLocalJniHandle (__this.DoInBackground (p0));
				if (p0 != null)
					JNIEnv.CopyArray (p0, native_p0);
				return __ret;
			}
#pragma warning restore 0169

			static IntPtr id_doInBackground_arrayLjava_lang_Object_;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.TileLoadTask']/method[@name='doInBackground' and count(parameter)=1 and parameter[1][@type='java.lang.Object[]']]"
			[Register ("doInBackground", "([Ljava/lang/Object;)Ljava/lang/Object;", "GetDoInBackground_arrayLjava_lang_Object_Handler")]
			protected override unsafe global::Java.Lang.Object DoInBackground (global::Java.Lang.Object[] p0)
			{
				if (id_doInBackground_arrayLjava_lang_Object_ == IntPtr.Zero)
					id_doInBackground_arrayLjava_lang_Object_ = JNIEnv.GetMethodID (class_ref, "doInBackground", "([Ljava/lang/Object;)Ljava/lang/Object;");
				IntPtr native_p0 = JNIEnv.NewArray (p0);
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (native_p0);

					global::Java.Lang.Object __ret;
					if (((object) this).GetType () == ThresholdType)
						__ret = global::Java.Lang.Object.GetObject<global::Java.Lang.Object> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_doInBackground_arrayLjava_lang_Object_, __args), JniHandleOwnership.TransferLocalRef);
					else
						__ret = global::Java.Lang.Object.GetObject<global::Java.Lang.Object> (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "doInBackground", "([Ljava/lang/Object;)Ljava/lang/Object;"), __args), JniHandleOwnership.TransferLocalRef);
					return __ret;
				} finally {
					if (p0 != null) {
						JNIEnv.CopyArray (native_p0, p0);
						JNIEnv.DeleteLocalRef (native_p0);
					}
				}
			}

			static Delegate cb_doInBackground_arrayLjava_lang_Void_;
#pragma warning disable 0169
			static Delegate GetDoInBackground_arrayLjava_lang_Void_Handler ()
			{
				if (cb_doInBackground_arrayLjava_lang_Void_ == null)
					cb_doInBackground_arrayLjava_lang_Void_ = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, IntPtr, IntPtr>) n_DoInBackground_arrayLjava_lang_Void_);
				return cb_doInBackground_arrayLjava_lang_Void_;
			}

			static IntPtr n_DoInBackground_arrayLjava_lang_Void_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.TileLoadTask __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.TileLoadTask> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Java.Lang.Void[] p0 = (global::Java.Lang.Void[]) JNIEnv.GetArray (native_p0, JniHandleOwnership.DoNotTransfer, typeof (global::Java.Lang.Void));
				IntPtr __ret = JNIEnv.ToLocalJniHandle (__this.DoInBackground (p0));
				if (p0 != null)
					JNIEnv.CopyArray (p0, native_p0);
				return __ret;
			}
#pragma warning restore 0169

			static IntPtr id_doInBackground_arrayLjava_lang_Void_;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.TileLoadTask']/method[@name='doInBackground' and count(parameter)=1 and parameter[1][@type='java.lang.Void...']]"
			[Register ("doInBackground", "([Ljava/lang/Void;)Landroid/graphics/Bitmap;", "GetDoInBackground_arrayLjava_lang_Void_Handler")]
			protected virtual unsafe global::Android.Graphics.Bitmap DoInBackground (params global:: Java.Lang.Void[] p0)
			{
				if (id_doInBackground_arrayLjava_lang_Void_ == IntPtr.Zero)
					id_doInBackground_arrayLjava_lang_Void_ = JNIEnv.GetMethodID (class_ref, "doInBackground", "([Ljava/lang/Void;)Landroid/graphics/Bitmap;");
				IntPtr native_p0 = JNIEnv.NewArray (p0);
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (native_p0);

					global::Android.Graphics.Bitmap __ret;
					if (((object) this).GetType () == ThresholdType)
						__ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.Bitmap> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_doInBackground_arrayLjava_lang_Void_, __args), JniHandleOwnership.TransferLocalRef);
					else
						__ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.Bitmap> (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "doInBackground", "([Ljava/lang/Void;)Landroid/graphics/Bitmap;"), __args), JniHandleOwnership.TransferLocalRef);
					return __ret;
				} finally {
					if (p0 != null) {
						JNIEnv.CopyArray (native_p0, p0);
						JNIEnv.DeleteLocalRef (native_p0);
					}
				}
			}

			static Delegate cb_onPostExecute_Landroid_graphics_Bitmap_;
#pragma warning disable 0169
			static Delegate GetOnPostExecute_Landroid_graphics_Bitmap_Handler ()
			{
				if (cb_onPostExecute_Landroid_graphics_Bitmap_ == null)
					cb_onPostExecute_Landroid_graphics_Bitmap_ = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_OnPostExecute_Landroid_graphics_Bitmap_);
				return cb_onPostExecute_Landroid_graphics_Bitmap_;
			}

			static void n_OnPostExecute_Landroid_graphics_Bitmap_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.TileLoadTask __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.TileLoadTask> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Android.Graphics.Bitmap p0 = global::Java.Lang.Object.GetObject<global::Android.Graphics.Bitmap> (native_p0, JniHandleOwnership.DoNotTransfer);
				__this.OnPostExecute (p0);
			}
#pragma warning restore 0169

			static IntPtr id_onPostExecute_Landroid_graphics_Bitmap_;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.TileLoadTask']/method[@name='onPostExecute' and count(parameter)=1 and parameter[1][@type='android.graphics.Bitmap']]"
			[Register ("onPostExecute", "(Landroid/graphics/Bitmap;)V", "GetOnPostExecute_Landroid_graphics_Bitmap_Handler")]
			protected virtual unsafe void OnPostExecute (global::Android.Graphics.Bitmap p0)
			{
				if (id_onPostExecute_Landroid_graphics_Bitmap_ == IntPtr.Zero)
					id_onPostExecute_Landroid_graphics_Bitmap_ = JNIEnv.GetMethodID (class_ref, "onPostExecute", "(Landroid/graphics/Bitmap;)V");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (p0);

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onPostExecute_Landroid_graphics_Bitmap_, __args);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onPostExecute", "(Landroid/graphics/Bitmap;)V"), __args);
				} finally {
				}
			}

		}

		// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.TilesInitTask']"
		[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$TilesInitTask", DoNotGenerateAcw=true)]
		public partial class TilesInitTask : global::Android.OS.AsyncTask {

			internal static new IntPtr java_class_handle;
			internal static new IntPtr class_ref {
				get {
					return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$TilesInitTask", ref java_class_handle);
				}
			}

			protected override IntPtr ThresholdClass {
				get { return class_ref; }
			}

			protected override global::System.Type ThresholdType {
				get { return typeof (TilesInitTask); }
			}

			protected TilesInitTask (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

			static Delegate cb_doInBackground_arrayLjava_lang_Object_;
#pragma warning disable 0169
			static Delegate GetDoInBackground_arrayLjava_lang_Object_Handler ()
			{
				if (cb_doInBackground_arrayLjava_lang_Object_ == null)
					cb_doInBackground_arrayLjava_lang_Object_ = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, IntPtr, IntPtr>) n_DoInBackground_arrayLjava_lang_Object_);
				return cb_doInBackground_arrayLjava_lang_Object_;
			}

			static IntPtr n_DoInBackground_arrayLjava_lang_Object_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.TilesInitTask __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.TilesInitTask> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Java.Lang.Object[] p0 = (global::Java.Lang.Object[]) JNIEnv.GetArray (native_p0, JniHandleOwnership.DoNotTransfer, typeof (global::Java.Lang.Object));
				IntPtr __ret = JNIEnv.ToLocalJniHandle (__this.DoInBackground (p0));
				if (p0 != null)
					JNIEnv.CopyArray (p0, native_p0);
				return __ret;
			}
#pragma warning restore 0169

			static IntPtr id_doInBackground_arrayLjava_lang_Object_;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.TilesInitTask']/method[@name='doInBackground' and count(parameter)=1 and parameter[1][@type='java.lang.Object[]']]"
			[Register ("doInBackground", "([Ljava/lang/Object;)Ljava/lang/Object;", "GetDoInBackground_arrayLjava_lang_Object_Handler")]
			protected override unsafe global::Java.Lang.Object DoInBackground (global::Java.Lang.Object[] p0)
			{
				if (id_doInBackground_arrayLjava_lang_Object_ == IntPtr.Zero)
					id_doInBackground_arrayLjava_lang_Object_ = JNIEnv.GetMethodID (class_ref, "doInBackground", "([Ljava/lang/Object;)Ljava/lang/Object;");
				IntPtr native_p0 = JNIEnv.NewArray (p0);
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (native_p0);

					global::Java.Lang.Object __ret;
					if (((object) this).GetType () == ThresholdType)
						__ret = global::Java.Lang.Object.GetObject<global::Java.Lang.Object> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_doInBackground_arrayLjava_lang_Object_, __args), JniHandleOwnership.TransferLocalRef);
					else
						__ret = global::Java.Lang.Object.GetObject<global::Java.Lang.Object> (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "doInBackground", "([Ljava/lang/Object;)Ljava/lang/Object;"), __args), JniHandleOwnership.TransferLocalRef);
					return __ret;
				} finally {
					if (p0 != null) {
						JNIEnv.CopyArray (native_p0, p0);
						JNIEnv.DeleteLocalRef (native_p0);
					}
				}
			}

			static Delegate cb_doInBackground_arrayLjava_lang_Void_;
#pragma warning disable 0169
			static Delegate GetDoInBackground_arrayLjava_lang_Void_Handler ()
			{
				if (cb_doInBackground_arrayLjava_lang_Void_ == null)
					cb_doInBackground_arrayLjava_lang_Void_ = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, IntPtr, IntPtr>) n_DoInBackground_arrayLjava_lang_Void_);
				return cb_doInBackground_arrayLjava_lang_Void_;
			}

			static IntPtr n_DoInBackground_arrayLjava_lang_Void_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.TilesInitTask __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.TilesInitTask> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				global::Java.Lang.Void[] p0 = (global::Java.Lang.Void[]) JNIEnv.GetArray (native_p0, JniHandleOwnership.DoNotTransfer, typeof (global::Java.Lang.Void));
				IntPtr __ret = JNIEnv.NewArray (__this.DoInBackground (p0));
				if (p0 != null)
					JNIEnv.CopyArray (p0, native_p0);
				return __ret;
			}
#pragma warning restore 0169

			static IntPtr id_doInBackground_arrayLjava_lang_Void_;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.TilesInitTask']/method[@name='doInBackground' and count(parameter)=1 and parameter[1][@type='java.lang.Void...']]"
			[Register ("doInBackground", "([Ljava/lang/Void;)[I", "GetDoInBackground_arrayLjava_lang_Void_Handler")]
			protected virtual unsafe int[] DoInBackground (params global:: Java.Lang.Void[] p0)
			{
				if (id_doInBackground_arrayLjava_lang_Void_ == IntPtr.Zero)
					id_doInBackground_arrayLjava_lang_Void_ = JNIEnv.GetMethodID (class_ref, "doInBackground", "([Ljava/lang/Void;)[I");
				IntPtr native_p0 = JNIEnv.NewArray (p0);
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (native_p0);

					int[] __ret;
					if (((object) this).GetType () == ThresholdType)
						__ret = (int[]) JNIEnv.GetArray (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_doInBackground_arrayLjava_lang_Void_, __args), JniHandleOwnership.TransferLocalRef, typeof (int));
					else
						__ret = (int[]) JNIEnv.GetArray (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "doInBackground", "([Ljava/lang/Void;)[I"), __args), JniHandleOwnership.TransferLocalRef, typeof (int));
					return __ret;
				} finally {
					if (p0 != null) {
						JNIEnv.CopyArray (native_p0, p0);
						JNIEnv.DeleteLocalRef (native_p0);
					}
				}
			}

			static Delegate cb_onPostExecute_arrayI;
#pragma warning disable 0169
			static Delegate GetOnPostExecute_arrayIHandler ()
			{
				if (cb_onPostExecute_arrayI == null)
					cb_onPostExecute_arrayI = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_OnPostExecute_arrayI);
				return cb_onPostExecute_arrayI;
			}

			static void n_OnPostExecute_arrayI (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
			{
				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.TilesInitTask __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.TilesInitTask> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
				int[] p0 = (int[]) JNIEnv.GetArray (native_p0, JniHandleOwnership.DoNotTransfer, typeof (int));
				__this.OnPostExecute (p0);
				if (p0 != null)
					JNIEnv.CopyArray (p0, native_p0);
			}
#pragma warning restore 0169

			static IntPtr id_onPostExecute_arrayI;
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView.TilesInitTask']/method[@name='onPostExecute' and count(parameter)=1 and parameter[1][@type='int[]']]"
			[Register ("onPostExecute", "([I)V", "GetOnPostExecute_arrayIHandler")]
			protected virtual unsafe void OnPostExecute (int[] p0)
			{
				if (id_onPostExecute_arrayI == IntPtr.Zero)
					id_onPostExecute_arrayI = JNIEnv.GetMethodID (class_ref, "onPostExecute", "([I)V");
				IntPtr native_p0 = JNIEnv.NewArray (p0);
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (native_p0);

					if (((object) this).GetType () == ThresholdType)
						JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onPostExecute_arrayI, __args);
					else
						JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onPostExecute", "([I)V"), __args);
				} finally {
					if (p0 != null) {
						JNIEnv.CopyArray (native_p0, p0);
						JNIEnv.DeleteLocalRef (native_p0);
					}
				}
			}

		}

		internal static new IntPtr java_class_handle;
		internal static new IntPtr class_ref {
			get {
				return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/SubsamplingScaleImageView", ref java_class_handle);
			}
		}

		protected override IntPtr ThresholdClass {
			get { return class_ref; }
		}

		protected override global::System.Type ThresholdType {
			get { return typeof (SubsamplingScaleImageView); }
		}

		protected SubsamplingScaleImageView (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

		static IntPtr id_ctor_Landroid_content_Context_Landroid_util_AttributeSet_;
		// Metadata.xml XPath constructor reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/constructor[@name='SubsamplingScaleImageView' and count(parameter)=2 and parameter[1][@type='android.content.Context'] and parameter[2][@type='android.util.AttributeSet']]"
		[Register (".ctor", "(Landroid/content/Context;Landroid/util/AttributeSet;)V", "")]
		public unsafe SubsamplingScaleImageView (global::Android.Content.Context p0, global::Android.Util.IAttributeSet p1)
			: base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
		{
			if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
				return;

			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				if (((object) this).GetType () != typeof (SubsamplingScaleImageView)) {
					SetHandle (
							global::Android.Runtime.JNIEnv.StartCreateInstance (((object) this).GetType (), "(Landroid/content/Context;Landroid/util/AttributeSet;)V", __args),
							JniHandleOwnership.TransferLocalRef);
					global::Android.Runtime.JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, "(Landroid/content/Context;Landroid/util/AttributeSet;)V", __args);
					return;
				}

				if (id_ctor_Landroid_content_Context_Landroid_util_AttributeSet_ == IntPtr.Zero)
					id_ctor_Landroid_content_Context_Landroid_util_AttributeSet_ = JNIEnv.GetMethodID (class_ref, "<init>", "(Landroid/content/Context;Landroid/util/AttributeSet;)V");
				SetHandle (
						global::Android.Runtime.JNIEnv.StartCreateInstance (class_ref, id_ctor_Landroid_content_Context_Landroid_util_AttributeSet_, __args),
						JniHandleOwnership.TransferLocalRef);
				JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, class_ref, id_ctor_Landroid_content_Context_Landroid_util_AttributeSet_, __args);
			} finally {
			}
		}

		static IntPtr id_ctor_Landroid_content_Context_;
		// Metadata.xml XPath constructor reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/constructor[@name='SubsamplingScaleImageView' and count(parameter)=1 and parameter[1][@type='android.content.Context']]"
		[Register (".ctor", "(Landroid/content/Context;)V", "")]
		public unsafe SubsamplingScaleImageView (global::Android.Content.Context p0)
			: base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
		{
			if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
				return;

			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				if (((object) this).GetType () != typeof (SubsamplingScaleImageView)) {
					SetHandle (
							global::Android.Runtime.JNIEnv.StartCreateInstance (((object) this).GetType (), "(Landroid/content/Context;)V", __args),
							JniHandleOwnership.TransferLocalRef);
					global::Android.Runtime.JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, "(Landroid/content/Context;)V", __args);
					return;
				}

				if (id_ctor_Landroid_content_Context_ == IntPtr.Zero)
					id_ctor_Landroid_content_Context_ = JNIEnv.GetMethodID (class_ref, "<init>", "(Landroid/content/Context;)V");
				SetHandle (
						global::Android.Runtime.JNIEnv.StartCreateInstance (class_ref, id_ctor_Landroid_content_Context_, __args),
						JniHandleOwnership.TransferLocalRef);
				JNIEnv.FinishCreateInstance (((global::Java.Lang.Object) this).Handle, class_ref, id_ctor_Landroid_content_Context_, __args);
			} finally {
			}
		}

		static IntPtr id_getAppliedOrientation;
		public unsafe int AppliedOrientation {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='getAppliedOrientation' and count(parameter)=0]"
			[Register ("getAppliedOrientation", "()I", "GetGetAppliedOrientationHandler")]
			get {
				if (id_getAppliedOrientation == IntPtr.Zero)
					id_getAppliedOrientation = JNIEnv.GetMethodID (class_ref, "getAppliedOrientation", "()I");
				try {
					return JNIEnv.CallIntMethod (((global::Java.Lang.Object) this).Handle, id_getAppliedOrientation);
				} finally {
				}
			}
		}

		static IntPtr id_getCenter;
		public unsafe global::Android.Graphics.PointF Center {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='getCenter' and count(parameter)=0]"
			[Register ("getCenter", "()Landroid/graphics/PointF;", "GetGetCenterHandler")]
			get {
				if (id_getCenter == IntPtr.Zero)
					id_getCenter = JNIEnv.GetMethodID (class_ref, "getCenter", "()Landroid/graphics/PointF;");
				try {
					return global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_getCenter), JniHandleOwnership.TransferLocalRef);
				} finally {
				}
			}
		}

		static Delegate cb_hasImage;
#pragma warning disable 0169
		static Delegate GetHasImageHandler ()
		{
			if (cb_hasImage == null)
				cb_hasImage = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, bool>) n_HasImage);
			return cb_hasImage;
		}

		static bool n_HasImage (IntPtr jnienv, IntPtr native__this)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			return __this.HasImage;
		}
#pragma warning restore 0169

		static IntPtr id_hasImage;
		public virtual unsafe bool HasImage {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='hasImage' and count(parameter)=0]"
			[Register ("hasImage", "()Z", "GetHasImageHandler")]
			get {
				if (id_hasImage == IntPtr.Zero)
					id_hasImage = JNIEnv.GetMethodID (class_ref, "hasImage", "()Z");
				try {

					if (((object) this).GetType () == ThresholdType)
						return JNIEnv.CallBooleanMethod (((global::Java.Lang.Object) this).Handle, id_hasImage);
					else
						return JNIEnv.CallNonvirtualBooleanMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "hasImage", "()Z"));
				} finally {
				}
			}
		}

		static IntPtr id_isImageLoaded;
		public unsafe bool IsImageLoaded {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='isImageLoaded' and count(parameter)=0]"
			[Register ("isImageLoaded", "()Z", "GetIsImageLoadedHandler")]
			get {
				if (id_isImageLoaded == IntPtr.Zero)
					id_isImageLoaded = JNIEnv.GetMethodID (class_ref, "isImageLoaded", "()Z");
				try {
					return JNIEnv.CallBooleanMethod (((global::Java.Lang.Object) this).Handle, id_isImageLoaded);
				} finally {
				}
			}
		}

		static IntPtr id_isReady;
		public unsafe bool IsReady {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='isReady' and count(parameter)=0]"
			[Register ("isReady", "()Z", "GetIsReadyHandler")]
			get {
				if (id_isReady == IntPtr.Zero)
					id_isReady = JNIEnv.GetMethodID (class_ref, "isReady", "()Z");
				try {
					return JNIEnv.CallBooleanMethod (((global::Java.Lang.Object) this).Handle, id_isReady);
				} finally {
				}
			}
		}

		static IntPtr id_getMaxScale;
		static IntPtr id_setMaxScale_F;
		public unsafe float MaxScale {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='getMaxScale' and count(parameter)=0]"
			[Register ("getMaxScale", "()F", "GetGetMaxScaleHandler")]
			get {
				if (id_getMaxScale == IntPtr.Zero)
					id_getMaxScale = JNIEnv.GetMethodID (class_ref, "getMaxScale", "()F");
				try {

					if (((object) this).GetType () == ThresholdType)
						return JNIEnv.CallFloatMethod (((global::Java.Lang.Object) this).Handle, id_getMaxScale);
					else
						return JNIEnv.CallNonvirtualFloatMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "getMaxScale", "()F"));
				} finally {
				}
			}
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setMaxScale' and count(parameter)=1 and parameter[1][@type='float']]"
			[Register ("setMaxScale", "(F)V", "GetSetMaxScale_FHandler")]
			set {
				if (id_setMaxScale_F == IntPtr.Zero)
					id_setMaxScale_F = JNIEnv.GetMethodID (class_ref, "setMaxScale", "(F)V");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (value);
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setMaxScale_F, __args);
				} finally {
				}
			}
		}

		static IntPtr id_getMinScale;
		static IntPtr id_setMinScale_F;
		public unsafe float MinScale {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='getMinScale' and count(parameter)=0]"
			[Register ("getMinScale", "()F", "GetGetMinScaleHandler")]
			get {
				if (id_getMinScale == IntPtr.Zero)
					id_getMinScale = JNIEnv.GetMethodID (class_ref, "getMinScale", "()F");
				try {
					return JNIEnv.CallFloatMethod (((global::Java.Lang.Object) this).Handle, id_getMinScale);
				} finally {
				}
			}
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setMinScale' and count(parameter)=1 and parameter[1][@type='float']]"
			[Register ("setMinScale", "(F)V", "GetSetMinScale_FHandler")]
			set {
				if (id_setMinScale_F == IntPtr.Zero)
					id_setMinScale_F = JNIEnv.GetMethodID (class_ref, "setMinScale", "(F)V");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (value);
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setMinScale_F, __args);
				} finally {
				}
			}
		}

		static IntPtr id_getOrientation;
		static IntPtr id_setOrientation_I;
		public unsafe int Orientation {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='getOrientation' and count(parameter)=0]"
			[Register ("getOrientation", "()I", "GetGetOrientationHandler")]
			get {
				if (id_getOrientation == IntPtr.Zero)
					id_getOrientation = JNIEnv.GetMethodID (class_ref, "getOrientation", "()I");
				try {
					return JNIEnv.CallIntMethod (((global::Java.Lang.Object) this).Handle, id_getOrientation);
				} finally {
				}
			}
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setOrientation' and count(parameter)=1 and parameter[1][@type='int']]"
			[Register ("setOrientation", "(I)V", "GetSetOrientation_IHandler")]
			set {
				if (id_setOrientation_I == IntPtr.Zero)
					id_setOrientation_I = JNIEnv.GetMethodID (class_ref, "setOrientation", "(I)V");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (value);
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setOrientation_I, __args);
				} finally {
				}
			}
		}

		static IntPtr id_isPanEnabled;
		static IntPtr id_setPanEnabled_Z;
		public unsafe bool PanEnabled {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='isPanEnabled' and count(parameter)=0]"
			[Register ("isPanEnabled", "()Z", "GetIsPanEnabledHandler")]
			get {
				if (id_isPanEnabled == IntPtr.Zero)
					id_isPanEnabled = JNIEnv.GetMethodID (class_ref, "isPanEnabled", "()Z");
				try {
					return JNIEnv.CallBooleanMethod (((global::Java.Lang.Object) this).Handle, id_isPanEnabled);
				} finally {
				}
			}
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setPanEnabled' and count(parameter)=1 and parameter[1][@type='boolean']]"
			[Register ("setPanEnabled", "(Z)V", "GetSetPanEnabled_ZHandler")]
			set {
				if (id_setPanEnabled_Z == IntPtr.Zero)
					id_setPanEnabled_Z = JNIEnv.GetMethodID (class_ref, "setPanEnabled", "(Z)V");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (value);
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setPanEnabled_Z, __args);
				} finally {
				}
			}
		}

		static IntPtr id_isQuickScaleEnabled;
		static IntPtr id_setQuickScaleEnabled_Z;
		public unsafe bool QuickScaleEnabled {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='isQuickScaleEnabled' and count(parameter)=0]"
			[Register ("isQuickScaleEnabled", "()Z", "GetIsQuickScaleEnabledHandler")]
			get {
				if (id_isQuickScaleEnabled == IntPtr.Zero)
					id_isQuickScaleEnabled = JNIEnv.GetMethodID (class_ref, "isQuickScaleEnabled", "()Z");
				try {
					return JNIEnv.CallBooleanMethod (((global::Java.Lang.Object) this).Handle, id_isQuickScaleEnabled);
				} finally {
				}
			}
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setQuickScaleEnabled' and count(parameter)=1 and parameter[1][@type='boolean']]"
			[Register ("setQuickScaleEnabled", "(Z)V", "GetSetQuickScaleEnabled_ZHandler")]
			set {
				if (id_setQuickScaleEnabled_Z == IntPtr.Zero)
					id_setQuickScaleEnabled_Z = JNIEnv.GetMethodID (class_ref, "setQuickScaleEnabled", "(Z)V");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (value);
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setQuickScaleEnabled_Z, __args);
				} finally {
				}
			}
		}

		static IntPtr id_getSHeight;
		public unsafe int SHeight {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='getSHeight' and count(parameter)=0]"
			[Register ("getSHeight", "()I", "GetGetSHeightHandler")]
			get {
				if (id_getSHeight == IntPtr.Zero)
					id_getSHeight = JNIEnv.GetMethodID (class_ref, "getSHeight", "()I");
				try {
					return JNIEnv.CallIntMethod (((global::Java.Lang.Object) this).Handle, id_getSHeight);
				} finally {
				}
			}
		}

		static IntPtr id_getSWidth;
		public unsafe int SWidth {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='getSWidth' and count(parameter)=0]"
			[Register ("getSWidth", "()I", "GetGetSWidthHandler")]
			get {
				if (id_getSWidth == IntPtr.Zero)
					id_getSWidth = JNIEnv.GetMethodID (class_ref, "getSWidth", "()I");
				try {
					return JNIEnv.CallIntMethod (((global::Java.Lang.Object) this).Handle, id_getSWidth);
				} finally {
				}
			}
		}

		static IntPtr id_getScale;
		public unsafe float Scale {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='getScale' and count(parameter)=0]"
			[Register ("getScale", "()F", "GetGetScaleHandler")]
			get {
				if (id_getScale == IntPtr.Zero)
					id_getScale = JNIEnv.GetMethodID (class_ref, "getScale", "()F");
				try {
					return JNIEnv.CallFloatMethod (((global::Java.Lang.Object) this).Handle, id_getScale);
				} finally {
				}
			}
		}

		static IntPtr id_getState;
		public unsafe global::Com.Davemorrissey.Labs.Subscaleview.ImageViewState State {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='getState' and count(parameter)=0]"
			[Register ("getState", "()Lcom/davemorrissey/labs/subscaleview/ImageViewState;", "GetGetStateHandler")]
			get {
				if (id_getState == IntPtr.Zero)
					id_getState = JNIEnv.GetMethodID (class_ref, "getState", "()Lcom/davemorrissey/labs/subscaleview/ImageViewState;");
				try {
					return global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageViewState> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_getState), JniHandleOwnership.TransferLocalRef);
				} finally {
				}
			}
		}

		static IntPtr id_isZoomEnabled;
		static IntPtr id_setZoomEnabled_Z;
		public unsafe bool ZoomEnabled {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='isZoomEnabled' and count(parameter)=0]"
			[Register ("isZoomEnabled", "()Z", "GetIsZoomEnabledHandler")]
			get {
				if (id_isZoomEnabled == IntPtr.Zero)
					id_isZoomEnabled = JNIEnv.GetMethodID (class_ref, "isZoomEnabled", "()Z");
				try {
					return JNIEnv.CallBooleanMethod (((global::Java.Lang.Object) this).Handle, id_isZoomEnabled);
				} finally {
				}
			}
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setZoomEnabled' and count(parameter)=1 and parameter[1][@type='boolean']]"
			[Register ("setZoomEnabled", "(Z)V", "GetSetZoomEnabled_ZHandler")]
			set {
				if (id_setZoomEnabled_Z == IntPtr.Zero)
					id_setZoomEnabled_Z = JNIEnv.GetMethodID (class_ref, "setZoomEnabled", "(Z)V");
				try {
					JValue* __args = stackalloc JValue [1];
					__args [0] = new JValue (value);
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setZoomEnabled_Z, __args);
				} finally {
				}
			}
		}

		static Delegate cb_animateCenter_Landroid_graphics_PointF_;
#pragma warning disable 0169
		static Delegate GetAnimateCenter_Landroid_graphics_PointF_Handler ()
		{
			if (cb_animateCenter_Landroid_graphics_PointF_ == null)
				cb_animateCenter_Landroid_graphics_PointF_ = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, IntPtr, IntPtr>) n_AnimateCenter_Landroid_graphics_PointF_);
			return cb_animateCenter_Landroid_graphics_PointF_;
		}

		static IntPtr n_AnimateCenter_Landroid_graphics_PointF_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			global::Android.Graphics.PointF p0 = global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (native_p0, JniHandleOwnership.DoNotTransfer);
			IntPtr __ret = JNIEnv.ToLocalJniHandle (__this.AnimateCenter (p0));
			return __ret;
		}
#pragma warning restore 0169

		static IntPtr id_animateCenter_Landroid_graphics_PointF_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='animateCenter' and count(parameter)=1 and parameter[1][@type='android.graphics.PointF']]"
		[Register ("animateCenter", "(Landroid/graphics/PointF;)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;", "GetAnimateCenter_Landroid_graphics_PointF_Handler")]
		public virtual unsafe global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder AnimateCenter (global::Android.Graphics.PointF p0)
		{
			if (id_animateCenter_Landroid_graphics_PointF_ == IntPtr.Zero)
				id_animateCenter_Landroid_graphics_PointF_ = JNIEnv.GetMethodID (class_ref, "animateCenter", "(Landroid/graphics/PointF;)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);

				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder __ret;
				if (((object) this).GetType () == ThresholdType)
					__ret = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_animateCenter_Landroid_graphics_PointF_, __args), JniHandleOwnership.TransferLocalRef);
				else
					__ret = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder> (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "animateCenter", "(Landroid/graphics/PointF;)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;"), __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

		static Delegate cb_animateScale_F;
#pragma warning disable 0169
		static Delegate GetAnimateScale_FHandler ()
		{
			if (cb_animateScale_F == null)
				cb_animateScale_F = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, float, IntPtr>) n_AnimateScale_F);
			return cb_animateScale_F;
		}

		static IntPtr n_AnimateScale_F (IntPtr jnienv, IntPtr native__this, float p0)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			return JNIEnv.ToLocalJniHandle (__this.AnimateScale (p0));
		}
#pragma warning restore 0169

		static IntPtr id_animateScale_F;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='animateScale' and count(parameter)=1 and parameter[1][@type='float']]"
		[Register ("animateScale", "(F)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;", "GetAnimateScale_FHandler")]
		public virtual unsafe global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder AnimateScale (float p0)
		{
			if (id_animateScale_F == IntPtr.Zero)
				id_animateScale_F = JNIEnv.GetMethodID (class_ref, "animateScale", "(F)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);

				if (((object) this).GetType () == ThresholdType)
					return global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_animateScale_F, __args), JniHandleOwnership.TransferLocalRef);
				else
					return global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder> (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "animateScale", "(F)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;"), __args), JniHandleOwnership.TransferLocalRef);
			} finally {
			}
		}

		static Delegate cb_animateScaleAndCenter_FLandroid_graphics_PointF_;
#pragma warning disable 0169
		static Delegate GetAnimateScaleAndCenter_FLandroid_graphics_PointF_Handler ()
		{
			if (cb_animateScaleAndCenter_FLandroid_graphics_PointF_ == null)
				cb_animateScaleAndCenter_FLandroid_graphics_PointF_ = JNINativeWrapper.CreateDelegate ((Func<IntPtr, IntPtr, float, IntPtr, IntPtr>) n_AnimateScaleAndCenter_FLandroid_graphics_PointF_);
			return cb_animateScaleAndCenter_FLandroid_graphics_PointF_;
		}

		static IntPtr n_AnimateScaleAndCenter_FLandroid_graphics_PointF_ (IntPtr jnienv, IntPtr native__this, float p0, IntPtr native_p1)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			global::Android.Graphics.PointF p1 = global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (native_p1, JniHandleOwnership.DoNotTransfer);
			IntPtr __ret = JNIEnv.ToLocalJniHandle (__this.AnimateScaleAndCenter (p0, p1));
			return __ret;
		}
#pragma warning restore 0169

		static IntPtr id_animateScaleAndCenter_FLandroid_graphics_PointF_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='animateScaleAndCenter' and count(parameter)=2 and parameter[1][@type='float'] and parameter[2][@type='android.graphics.PointF']]"
		[Register ("animateScaleAndCenter", "(FLandroid/graphics/PointF;)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;", "GetAnimateScaleAndCenter_FLandroid_graphics_PointF_Handler")]
		public virtual unsafe global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder AnimateScaleAndCenter (float p0, global::Android.Graphics.PointF p1)
		{
			if (id_animateScaleAndCenter_FLandroid_graphics_PointF_ == IntPtr.Zero)
				id_animateScaleAndCenter_FLandroid_graphics_PointF_ = JNIEnv.GetMethodID (class_ref, "animateScaleAndCenter", "(FLandroid/graphics/PointF;)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;");
			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);

				global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder __ret;
				if (((object) this).GetType () == ThresholdType)
					__ret = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_animateScaleAndCenter_FLandroid_graphics_PointF_, __args), JniHandleOwnership.TransferLocalRef);
				else
					__ret = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.AnimationBuilder> (JNIEnv.CallNonvirtualObjectMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "animateScaleAndCenter", "(FLandroid/graphics/PointF;)Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$AnimationBuilder;"), __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

		static Delegate cb_onImageLoaded;
#pragma warning disable 0169
		static Delegate GetOnImageLoadedHandler ()
		{
			if (cb_onImageLoaded == null)
				cb_onImageLoaded = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnImageLoaded);
			return cb_onImageLoaded;
		}

		static void n_OnImageLoaded (IntPtr jnienv, IntPtr native__this)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			__this.OnImageLoaded ();
		}
#pragma warning restore 0169

		static IntPtr id_onImageLoaded;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='onImageLoaded' and count(parameter)=0]"
		[Register ("onImageLoaded", "()V", "GetOnImageLoadedHandler")]
		protected virtual unsafe void OnImageLoaded ()
		{
			if (id_onImageLoaded == IntPtr.Zero)
				id_onImageLoaded = JNIEnv.GetMethodID (class_ref, "onImageLoaded", "()V");
			try {

				if (((object) this).GetType () == ThresholdType)
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onImageLoaded);
				else
					JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onImageLoaded", "()V"));
			} finally {
			}
		}

		static Delegate cb_onReady;
#pragma warning disable 0169
		static Delegate GetOnReadyHandler ()
		{
			if (cb_onReady == null)
				cb_onReady = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) n_OnReady);
			return cb_onReady;
		}

		static void n_OnReady (IntPtr jnienv, IntPtr native__this)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			__this.OnReady ();
		}
#pragma warning restore 0169

		static IntPtr id_onReady;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='onReady' and count(parameter)=0]"
		[Register ("onReady", "()V", "GetOnReadyHandler")]
		protected virtual unsafe void OnReady ()
		{
			if (id_onReady == IntPtr.Zero)
				id_onReady = JNIEnv.GetMethodID (class_ref, "onReady", "()V");
			try {

				if (((object) this).GetType () == ThresholdType)
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_onReady);
				else
					JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onReady", "()V"));
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
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			__this.Recycle ();
		}
#pragma warning restore 0169

		static IntPtr id_recycle;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='recycle' and count(parameter)=0]"
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

		static IntPtr id_resetScaleAndCenter;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='resetScaleAndCenter' and count(parameter)=0]"
		[Register ("resetScaleAndCenter", "()V", "")]
		public unsafe void ResetScaleAndCenter ()
		{
			if (id_resetScaleAndCenter == IntPtr.Zero)
				id_resetScaleAndCenter = JNIEnv.GetMethodID (class_ref, "resetScaleAndCenter", "()V");
			try {
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_resetScaleAndCenter);
			} finally {
			}
		}

		static IntPtr id_setBitmapDecoderClass_Ljava_lang_Class_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setBitmapDecoderClass' and count(parameter)=1 and parameter[1][@type='java.lang.Class&lt;? extends com.davemorrissey.labs.subscaleview.decoder.ImageDecoder&gt;']]"
		[Register ("setBitmapDecoderClass", "(Ljava/lang/Class;)V", "")]
		public unsafe void SetBitmapDecoderClass (global::Java.Lang.Class p0)
		{
			if (id_setBitmapDecoderClass_Ljava_lang_Class_ == IntPtr.Zero)
				id_setBitmapDecoderClass_Ljava_lang_Class_ = JNIEnv.GetMethodID (class_ref, "setBitmapDecoderClass", "(Ljava/lang/Class;)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setBitmapDecoderClass_Ljava_lang_Class_, __args);
			} finally {
			}
		}

		static IntPtr id_setBitmapDecoderFactory_Lcom_davemorrissey_labs_subscaleview_decoder_DecoderFactory_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setBitmapDecoderFactory' and count(parameter)=1 and parameter[1][@type='com.davemorrissey.labs.subscaleview.decoder.DecoderFactory&lt;? extends com.davemorrissey.labs.subscaleview.decoder.ImageDecoder&gt;']]"
		[Register ("setBitmapDecoderFactory", "(Lcom/davemorrissey/labs/subscaleview/decoder/DecoderFactory;)V", "")]
		public unsafe void SetBitmapDecoderFactory (global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IDecoderFactory p0)
		{
			if (id_setBitmapDecoderFactory_Lcom_davemorrissey_labs_subscaleview_decoder_DecoderFactory_ == IntPtr.Zero)
				id_setBitmapDecoderFactory_Lcom_davemorrissey_labs_subscaleview_decoder_DecoderFactory_ = JNIEnv.GetMethodID (class_ref, "setBitmapDecoderFactory", "(Lcom/davemorrissey/labs/subscaleview/decoder/DecoderFactory;)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setBitmapDecoderFactory_Lcom_davemorrissey_labs_subscaleview_decoder_DecoderFactory_, __args);
			} finally {
			}
		}

		static IntPtr id_setDebug_Z;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setDebug' and count(parameter)=1 and parameter[1][@type='boolean']]"
		[Register ("setDebug", "(Z)V", "")]
		public unsafe void SetDebug (bool p0)
		{
			if (id_setDebug_Z == IntPtr.Zero)
				id_setDebug_Z = JNIEnv.GetMethodID (class_ref, "setDebug", "(Z)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setDebug_Z, __args);
			} finally {
			}
		}

		static IntPtr id_setDoubleTapZoomDpi_I;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setDoubleTapZoomDpi' and count(parameter)=1 and parameter[1][@type='int']]"
		[Register ("setDoubleTapZoomDpi", "(I)V", "")]
		public unsafe void SetDoubleTapZoomDpi (int p0)
		{
			if (id_setDoubleTapZoomDpi_I == IntPtr.Zero)
				id_setDoubleTapZoomDpi_I = JNIEnv.GetMethodID (class_ref, "setDoubleTapZoomDpi", "(I)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setDoubleTapZoomDpi_I, __args);
			} finally {
			}
		}

		static IntPtr id_setDoubleTapZoomDuration_I;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setDoubleTapZoomDuration' and count(parameter)=1 and parameter[1][@type='int']]"
		[Register ("setDoubleTapZoomDuration", "(I)V", "")]
		public unsafe void SetDoubleTapZoomDuration (int p0)
		{
			if (id_setDoubleTapZoomDuration_I == IntPtr.Zero)
				id_setDoubleTapZoomDuration_I = JNIEnv.GetMethodID (class_ref, "setDoubleTapZoomDuration", "(I)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setDoubleTapZoomDuration_I, __args);
			} finally {
			}
		}

		static IntPtr id_setDoubleTapZoomScale_F;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setDoubleTapZoomScale' and count(parameter)=1 and parameter[1][@type='float']]"
		[Register ("setDoubleTapZoomScale", "(F)V", "")]
		public unsafe void SetDoubleTapZoomScale (float p0)
		{
			if (id_setDoubleTapZoomScale_F == IntPtr.Zero)
				id_setDoubleTapZoomScale_F = JNIEnv.GetMethodID (class_ref, "setDoubleTapZoomScale", "(F)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setDoubleTapZoomScale_F, __args);
			} finally {
			}
		}

		static IntPtr id_setDoubleTapZoomStyle_I;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setDoubleTapZoomStyle' and count(parameter)=1 and parameter[1][@type='int']]"
		[Register ("setDoubleTapZoomStyle", "(I)V", "")]
		public unsafe void SetDoubleTapZoomStyle (int p0)
		{
			if (id_setDoubleTapZoomStyle_I == IntPtr.Zero)
				id_setDoubleTapZoomStyle_I = JNIEnv.GetMethodID (class_ref, "setDoubleTapZoomStyle", "(I)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setDoubleTapZoomStyle_I, __args);
			} finally {
			}
		}

		static IntPtr id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setImage' and count(parameter)=1 and parameter[1][@type='com.davemorrissey.labs.subscaleview.ImageSource']]"
		[Register ("setImage", "(Lcom/davemorrissey/labs/subscaleview/ImageSource;)V", "")]
		public unsafe void SetImage (global::Com.Davemorrissey.Labs.Subscaleview.ImageSource p0)
		{
			if (id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_ == IntPtr.Zero)
				id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_ = JNIEnv.GetMethodID (class_ref, "setImage", "(Lcom/davemorrissey/labs/subscaleview/ImageSource;)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_, __args);
			} finally {
			}
		}

		static IntPtr id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageSource_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setImage' and count(parameter)=2 and parameter[1][@type='com.davemorrissey.labs.subscaleview.ImageSource'] and parameter[2][@type='com.davemorrissey.labs.subscaleview.ImageSource']]"
		[Register ("setImage", "(Lcom/davemorrissey/labs/subscaleview/ImageSource;Lcom/davemorrissey/labs/subscaleview/ImageSource;)V", "")]
		public unsafe void SetImage (global::Com.Davemorrissey.Labs.Subscaleview.ImageSource p0, global::Com.Davemorrissey.Labs.Subscaleview.ImageSource p1)
		{
			if (id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageSource_ == IntPtr.Zero)
				id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageSource_ = JNIEnv.GetMethodID (class_ref, "setImage", "(Lcom/davemorrissey/labs/subscaleview/ImageSource;Lcom/davemorrissey/labs/subscaleview/ImageSource;)V");
			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageSource_, __args);
			} finally {
			}
		}

		static IntPtr id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageViewState_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setImage' and count(parameter)=3 and parameter[1][@type='com.davemorrissey.labs.subscaleview.ImageSource'] and parameter[2][@type='com.davemorrissey.labs.subscaleview.ImageSource'] and parameter[3][@type='com.davemorrissey.labs.subscaleview.ImageViewState']]"
		[Register ("setImage", "(Lcom/davemorrissey/labs/subscaleview/ImageSource;Lcom/davemorrissey/labs/subscaleview/ImageSource;Lcom/davemorrissey/labs/subscaleview/ImageViewState;)V", "")]
		public unsafe void SetImage (global::Com.Davemorrissey.Labs.Subscaleview.ImageSource p0, global::Com.Davemorrissey.Labs.Subscaleview.ImageSource p1, global::Com.Davemorrissey.Labs.Subscaleview.ImageViewState p2)
		{
			if (id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageViewState_ == IntPtr.Zero)
				id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageViewState_ = JNIEnv.GetMethodID (class_ref, "setImage", "(Lcom/davemorrissey/labs/subscaleview/ImageSource;Lcom/davemorrissey/labs/subscaleview/ImageSource;Lcom/davemorrissey/labs/subscaleview/ImageViewState;)V");
			try {
				JValue* __args = stackalloc JValue [3];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				__args [2] = new JValue (p2);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageViewState_, __args);
			} finally {
			}
		}

		static IntPtr id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageViewState_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setImage' and count(parameter)=2 and parameter[1][@type='com.davemorrissey.labs.subscaleview.ImageSource'] and parameter[2][@type='com.davemorrissey.labs.subscaleview.ImageViewState']]"
		[Register ("setImage", "(Lcom/davemorrissey/labs/subscaleview/ImageSource;Lcom/davemorrissey/labs/subscaleview/ImageViewState;)V", "")]
		public unsafe void SetImage (global::Com.Davemorrissey.Labs.Subscaleview.ImageSource p0, global::Com.Davemorrissey.Labs.Subscaleview.ImageViewState p1)
		{
			if (id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageViewState_ == IntPtr.Zero)
				id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageViewState_ = JNIEnv.GetMethodID (class_ref, "setImage", "(Lcom/davemorrissey/labs/subscaleview/ImageSource;Lcom/davemorrissey/labs/subscaleview/ImageViewState;)V");
			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setImage_Lcom_davemorrissey_labs_subscaleview_ImageSource_Lcom_davemorrissey_labs_subscaleview_ImageViewState_, __args);
			} finally {
			}
		}

		static Delegate cb_setMaxTileSize_I;
#pragma warning disable 0169
		static Delegate GetSetMaxTileSize_IHandler ()
		{
			if (cb_setMaxTileSize_I == null)
				cb_setMaxTileSize_I = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, int>) n_SetMaxTileSize_I);
			return cb_setMaxTileSize_I;
		}

		static void n_SetMaxTileSize_I (IntPtr jnienv, IntPtr native__this, int p0)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			__this.SetMaxTileSize (p0);
		}
#pragma warning restore 0169

		static IntPtr id_setMaxTileSize_I;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setMaxTileSize' and count(parameter)=1 and parameter[1][@type='int']]"
		[Register ("setMaxTileSize", "(I)V", "GetSetMaxTileSize_IHandler")]
		public virtual unsafe void SetMaxTileSize (int p0)
		{
			if (id_setMaxTileSize_I == IntPtr.Zero)
				id_setMaxTileSize_I = JNIEnv.GetMethodID (class_ref, "setMaxTileSize", "(I)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);

				if (((object) this).GetType () == ThresholdType)
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setMaxTileSize_I, __args);
				else
					JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "setMaxTileSize", "(I)V"), __args);
			} finally {
			}
		}

		static Delegate cb_setMaxTileSize_II;
#pragma warning disable 0169
		static Delegate GetSetMaxTileSize_IIHandler ()
		{
			if (cb_setMaxTileSize_II == null)
				cb_setMaxTileSize_II = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, int, int>) n_SetMaxTileSize_II);
			return cb_setMaxTileSize_II;
		}

		static void n_SetMaxTileSize_II (IntPtr jnienv, IntPtr native__this, int p0, int p1)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			__this.SetMaxTileSize (p0, p1);
		}
#pragma warning restore 0169

		static IntPtr id_setMaxTileSize_II;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setMaxTileSize' and count(parameter)=2 and parameter[1][@type='int'] and parameter[2][@type='int']]"
		[Register ("setMaxTileSize", "(II)V", "GetSetMaxTileSize_IIHandler")]
		public virtual unsafe void SetMaxTileSize (int p0, int p1)
		{
			if (id_setMaxTileSize_II == IntPtr.Zero)
				id_setMaxTileSize_II = JNIEnv.GetMethodID (class_ref, "setMaxTileSize", "(II)V");
			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);

				if (((object) this).GetType () == ThresholdType)
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setMaxTileSize_II, __args);
				else
					JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "setMaxTileSize", "(II)V"), __args);
			} finally {
			}
		}

		static IntPtr id_setMaximumDpi_I;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setMaximumDpi' and count(parameter)=1 and parameter[1][@type='int']]"
		[Register ("setMaximumDpi", "(I)V", "")]
		public unsafe void SetMaximumDpi (int p0)
		{
			if (id_setMaximumDpi_I == IntPtr.Zero)
				id_setMaximumDpi_I = JNIEnv.GetMethodID (class_ref, "setMaximumDpi", "(I)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setMaximumDpi_I, __args);
			} finally {
			}
		}

		static IntPtr id_setMinimumDpi_I;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setMinimumDpi' and count(parameter)=1 and parameter[1][@type='int']]"
		[Register ("setMinimumDpi", "(I)V", "")]
		public unsafe void SetMinimumDpi (int p0)
		{
			if (id_setMinimumDpi_I == IntPtr.Zero)
				id_setMinimumDpi_I = JNIEnv.GetMethodID (class_ref, "setMinimumDpi", "(I)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setMinimumDpi_I, __args);
			} finally {
			}
		}

		static IntPtr id_setMinimumScaleType_I;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setMinimumScaleType' and count(parameter)=1 and parameter[1][@type='int']]"
		[Register ("setMinimumScaleType", "(I)V", "")]
		public unsafe void SetMinimumScaleType (int p0)
		{
			if (id_setMinimumScaleType_I == IntPtr.Zero)
				id_setMinimumScaleType_I = JNIEnv.GetMethodID (class_ref, "setMinimumScaleType", "(I)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setMinimumScaleType_I, __args);
			} finally {
			}
		}

		static Delegate cb_setMinimumTileDpi_I;
#pragma warning disable 0169
		static Delegate GetSetMinimumTileDpi_IHandler ()
		{
			if (cb_setMinimumTileDpi_I == null)
				cb_setMinimumTileDpi_I = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, int>) n_SetMinimumTileDpi_I);
			return cb_setMinimumTileDpi_I;
		}

		static void n_SetMinimumTileDpi_I (IntPtr jnienv, IntPtr native__this, int p0)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			__this.SetMinimumTileDpi (p0);
		}
#pragma warning restore 0169

		static IntPtr id_setMinimumTileDpi_I;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setMinimumTileDpi' and count(parameter)=1 and parameter[1][@type='int']]"
		[Register ("setMinimumTileDpi", "(I)V", "GetSetMinimumTileDpi_IHandler")]
		public virtual unsafe void SetMinimumTileDpi (int p0)
		{
			if (id_setMinimumTileDpi_I == IntPtr.Zero)
				id_setMinimumTileDpi_I = JNIEnv.GetMethodID (class_ref, "setMinimumTileDpi", "(I)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);

				if (((object) this).GetType () == ThresholdType)
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setMinimumTileDpi_I, __args);
				else
					JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "setMinimumTileDpi", "(I)V"), __args);
			} finally {
			}
		}

		static Delegate cb_setOnImageEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnImageEventListener_;
#pragma warning disable 0169
		static Delegate GetSetOnImageEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnImageEventListener_Handler ()
		{
			if (cb_setOnImageEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnImageEventListener_ == null)
				cb_setOnImageEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnImageEventListener_ = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_SetOnImageEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnImageEventListener_);
			return cb_setOnImageEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnImageEventListener_;
		}

		static void n_SetOnImageEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnImageEventListener_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener p0 = (global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener)global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener> (native_p0, JniHandleOwnership.DoNotTransfer);
			__this.SetOnImageEventListener (p0);
		}
#pragma warning restore 0169

		static IntPtr id_setOnImageEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnImageEventListener_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setOnImageEventListener' and count(parameter)=1 and parameter[1][@type='com.davemorrissey.labs.subscaleview.SubsamplingScaleImageView.OnImageEventListener']]"
		[Register ("setOnImageEventListener", "(Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnImageEventListener;)V", "GetSetOnImageEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnImageEventListener_Handler")]
		public virtual unsafe void SetOnImageEventListener (global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener p0)
		{
			if (id_setOnImageEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnImageEventListener_ == IntPtr.Zero)
				id_setOnImageEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnImageEventListener_ = JNIEnv.GetMethodID (class_ref, "setOnImageEventListener", "(Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnImageEventListener;)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);

				if (((object) this).GetType () == ThresholdType)
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setOnImageEventListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnImageEventListener_, __args);
				else
					JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "setOnImageEventListener", "(Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnImageEventListener;)V"), __args);
			} finally {
			}
		}

		static Delegate cb_setOnStateChangedListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnStateChangedListener_;
#pragma warning disable 0169
		static Delegate GetSetOnStateChangedListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnStateChangedListener_Handler ()
		{
			if (cb_setOnStateChangedListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnStateChangedListener_ == null)
				cb_setOnStateChangedListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnStateChangedListener_ = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_SetOnStateChangedListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnStateChangedListener_);
			return cb_setOnStateChangedListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnStateChangedListener_;
		}

		static void n_SetOnStateChangedListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnStateChangedListener_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener p0 = (global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener)global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener> (native_p0, JniHandleOwnership.DoNotTransfer);
			__this.SetOnStateChangedListener (p0);
		}
#pragma warning restore 0169

		static IntPtr id_setOnStateChangedListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnStateChangedListener_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setOnStateChangedListener' and count(parameter)=1 and parameter[1][@type='com.davemorrissey.labs.subscaleview.SubsamplingScaleImageView.OnStateChangedListener']]"
		[Register ("setOnStateChangedListener", "(Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnStateChangedListener;)V", "GetSetOnStateChangedListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnStateChangedListener_Handler")]
		public virtual unsafe void SetOnStateChangedListener (global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener p0)
		{
			if (id_setOnStateChangedListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnStateChangedListener_ == IntPtr.Zero)
				id_setOnStateChangedListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnStateChangedListener_ = JNIEnv.GetMethodID (class_ref, "setOnStateChangedListener", "(Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnStateChangedListener;)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);

				if (((object) this).GetType () == ThresholdType)
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setOnStateChangedListener_Lcom_davemorrissey_labs_subscaleview_SubsamplingScaleImageView_OnStateChangedListener_, __args);
				else
					JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "setOnStateChangedListener", "(Lcom/davemorrissey/labs/subscaleview/SubsamplingScaleImageView$OnStateChangedListener;)V"), __args);
			} finally {
			}
		}

		static IntPtr id_setPanLimit_I;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setPanLimit' and count(parameter)=1 and parameter[1][@type='int']]"
		[Register ("setPanLimit", "(I)V", "")]
		public unsafe void SetPanLimit (int p0)
		{
			if (id_setPanLimit_I == IntPtr.Zero)
				id_setPanLimit_I = JNIEnv.GetMethodID (class_ref, "setPanLimit", "(I)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setPanLimit_I, __args);
			} finally {
			}
		}

		static Delegate cb_setParallelLoadingEnabled_Z;
#pragma warning disable 0169
		static Delegate GetSetParallelLoadingEnabled_ZHandler ()
		{
			if (cb_setParallelLoadingEnabled_Z == null)
				cb_setParallelLoadingEnabled_Z = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, bool>) n_SetParallelLoadingEnabled_Z);
			return cb_setParallelLoadingEnabled_Z;
		}

		static void n_SetParallelLoadingEnabled_Z (IntPtr jnienv, IntPtr native__this, bool p0)
		{
			global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView __this = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			__this.SetParallelLoadingEnabled (p0);
		}
#pragma warning restore 0169

		static IntPtr id_setParallelLoadingEnabled_Z;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setParallelLoadingEnabled' and count(parameter)=1 and parameter[1][@type='boolean']]"
		[Register ("setParallelLoadingEnabled", "(Z)V", "GetSetParallelLoadingEnabled_ZHandler")]
		public virtual unsafe void SetParallelLoadingEnabled (bool p0)
		{
			if (id_setParallelLoadingEnabled_Z == IntPtr.Zero)
				id_setParallelLoadingEnabled_Z = JNIEnv.GetMethodID (class_ref, "setParallelLoadingEnabled", "(Z)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);

				if (((object) this).GetType () == ThresholdType)
					JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setParallelLoadingEnabled_Z, __args);
				else
					JNIEnv.CallNonvirtualVoidMethod (((global::Java.Lang.Object) this).Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "setParallelLoadingEnabled", "(Z)V"), __args);
			} finally {
			}
		}

		static IntPtr id_setRegionDecoderClass_Ljava_lang_Class_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setRegionDecoderClass' and count(parameter)=1 and parameter[1][@type='java.lang.Class&lt;? extends com.davemorrissey.labs.subscaleview.decoder.ImageRegionDecoder&gt;']]"
		[Register ("setRegionDecoderClass", "(Ljava/lang/Class;)V", "")]
		public unsafe void SetRegionDecoderClass (global::Java.Lang.Class p0)
		{
			if (id_setRegionDecoderClass_Ljava_lang_Class_ == IntPtr.Zero)
				id_setRegionDecoderClass_Ljava_lang_Class_ = JNIEnv.GetMethodID (class_ref, "setRegionDecoderClass", "(Ljava/lang/Class;)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setRegionDecoderClass_Ljava_lang_Class_, __args);
			} finally {
			}
		}

		static IntPtr id_setRegionDecoderFactory_Lcom_davemorrissey_labs_subscaleview_decoder_DecoderFactory_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setRegionDecoderFactory' and count(parameter)=1 and parameter[1][@type='com.davemorrissey.labs.subscaleview.decoder.DecoderFactory&lt;? extends com.davemorrissey.labs.subscaleview.decoder.ImageRegionDecoder&gt;']]"
		[Register ("setRegionDecoderFactory", "(Lcom/davemorrissey/labs/subscaleview/decoder/DecoderFactory;)V", "")]
		public unsafe void SetRegionDecoderFactory (global::Com.Davemorrissey.Labs.Subscaleview.Decoder.IDecoderFactory p0)
		{
			if (id_setRegionDecoderFactory_Lcom_davemorrissey_labs_subscaleview_decoder_DecoderFactory_ == IntPtr.Zero)
				id_setRegionDecoderFactory_Lcom_davemorrissey_labs_subscaleview_decoder_DecoderFactory_ = JNIEnv.GetMethodID (class_ref, "setRegionDecoderFactory", "(Lcom/davemorrissey/labs/subscaleview/decoder/DecoderFactory;)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setRegionDecoderFactory_Lcom_davemorrissey_labs_subscaleview_decoder_DecoderFactory_, __args);
			} finally {
			}
		}

		static IntPtr id_setScaleAndCenter_FLandroid_graphics_PointF_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setScaleAndCenter' and count(parameter)=2 and parameter[1][@type='float'] and parameter[2][@type='android.graphics.PointF']]"
		[Register ("setScaleAndCenter", "(FLandroid/graphics/PointF;)V", "")]
		public unsafe void SetScaleAndCenter (float p0, global::Android.Graphics.PointF p1)
		{
			if (id_setScaleAndCenter_FLandroid_graphics_PointF_ == IntPtr.Zero)
				id_setScaleAndCenter_FLandroid_graphics_PointF_ = JNIEnv.GetMethodID (class_ref, "setScaleAndCenter", "(FLandroid/graphics/PointF;)V");
			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setScaleAndCenter_FLandroid_graphics_PointF_, __args);
			} finally {
			}
		}

		static IntPtr id_setTileBackgroundColor_I;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='setTileBackgroundColor' and count(parameter)=1 and parameter[1][@type='int']]"
		[Register ("setTileBackgroundColor", "(I)V", "")]
		public unsafe void SetTileBackgroundColor (int p0)
		{
			if (id_setTileBackgroundColor_I == IntPtr.Zero)
				id_setTileBackgroundColor_I = JNIEnv.GetMethodID (class_ref, "setTileBackgroundColor", "(I)V");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				JNIEnv.CallVoidMethod (((global::Java.Lang.Object) this).Handle, id_setTileBackgroundColor_I, __args);
			} finally {
			}
		}

		static IntPtr id_sourceToViewCoord_Landroid_graphics_PointF_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='sourceToViewCoord' and count(parameter)=1 and parameter[1][@type='android.graphics.PointF']]"
		[Register ("sourceToViewCoord", "(Landroid/graphics/PointF;)Landroid/graphics/PointF;", "")]
		public unsafe global::Android.Graphics.PointF SourceToViewCoord (global::Android.Graphics.PointF p0)
		{
			if (id_sourceToViewCoord_Landroid_graphics_PointF_ == IntPtr.Zero)
				id_sourceToViewCoord_Landroid_graphics_PointF_ = JNIEnv.GetMethodID (class_ref, "sourceToViewCoord", "(Landroid/graphics/PointF;)Landroid/graphics/PointF;");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				global::Android.Graphics.PointF __ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_sourceToViewCoord_Landroid_graphics_PointF_, __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

		static IntPtr id_sourceToViewCoord_Landroid_graphics_PointF_Landroid_graphics_PointF_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='sourceToViewCoord' and count(parameter)=2 and parameter[1][@type='android.graphics.PointF'] and parameter[2][@type='android.graphics.PointF']]"
		[Register ("sourceToViewCoord", "(Landroid/graphics/PointF;Landroid/graphics/PointF;)Landroid/graphics/PointF;", "")]
		public unsafe global::Android.Graphics.PointF SourceToViewCoord (global::Android.Graphics.PointF p0, global::Android.Graphics.PointF p1)
		{
			if (id_sourceToViewCoord_Landroid_graphics_PointF_Landroid_graphics_PointF_ == IntPtr.Zero)
				id_sourceToViewCoord_Landroid_graphics_PointF_Landroid_graphics_PointF_ = JNIEnv.GetMethodID (class_ref, "sourceToViewCoord", "(Landroid/graphics/PointF;Landroid/graphics/PointF;)Landroid/graphics/PointF;");
			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				global::Android.Graphics.PointF __ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_sourceToViewCoord_Landroid_graphics_PointF_Landroid_graphics_PointF_, __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

		static IntPtr id_sourceToViewCoord_FF;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='sourceToViewCoord' and count(parameter)=2 and parameter[1][@type='float'] and parameter[2][@type='float']]"
		[Register ("sourceToViewCoord", "(FF)Landroid/graphics/PointF;", "")]
		public unsafe global::Android.Graphics.PointF SourceToViewCoord (float p0, float p1)
		{
			if (id_sourceToViewCoord_FF == IntPtr.Zero)
				id_sourceToViewCoord_FF = JNIEnv.GetMethodID (class_ref, "sourceToViewCoord", "(FF)Landroid/graphics/PointF;");
			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				return global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_sourceToViewCoord_FF, __args), JniHandleOwnership.TransferLocalRef);
			} finally {
			}
		}

		static IntPtr id_sourceToViewCoord_FFLandroid_graphics_PointF_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='sourceToViewCoord' and count(parameter)=3 and parameter[1][@type='float'] and parameter[2][@type='float'] and parameter[3][@type='android.graphics.PointF']]"
		[Register ("sourceToViewCoord", "(FFLandroid/graphics/PointF;)Landroid/graphics/PointF;", "")]
		public unsafe global::Android.Graphics.PointF SourceToViewCoord (float p0, float p1, global::Android.Graphics.PointF p2)
		{
			if (id_sourceToViewCoord_FFLandroid_graphics_PointF_ == IntPtr.Zero)
				id_sourceToViewCoord_FFLandroid_graphics_PointF_ = JNIEnv.GetMethodID (class_ref, "sourceToViewCoord", "(FFLandroid/graphics/PointF;)Landroid/graphics/PointF;");
			try {
				JValue* __args = stackalloc JValue [3];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				__args [2] = new JValue (p2);
				global::Android.Graphics.PointF __ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_sourceToViewCoord_FFLandroid_graphics_PointF_, __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

		static IntPtr id_viewToSourceCoord_Landroid_graphics_PointF_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='viewToSourceCoord' and count(parameter)=1 and parameter[1][@type='android.graphics.PointF']]"
		[Register ("viewToSourceCoord", "(Landroid/graphics/PointF;)Landroid/graphics/PointF;", "")]
		public unsafe global::Android.Graphics.PointF ViewToSourceCoord (global::Android.Graphics.PointF p0)
		{
			if (id_viewToSourceCoord_Landroid_graphics_PointF_ == IntPtr.Zero)
				id_viewToSourceCoord_Landroid_graphics_PointF_ = JNIEnv.GetMethodID (class_ref, "viewToSourceCoord", "(Landroid/graphics/PointF;)Landroid/graphics/PointF;");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				global::Android.Graphics.PointF __ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_viewToSourceCoord_Landroid_graphics_PointF_, __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

		static IntPtr id_viewToSourceCoord_Landroid_graphics_PointF_Landroid_graphics_PointF_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='viewToSourceCoord' and count(parameter)=2 and parameter[1][@type='android.graphics.PointF'] and parameter[2][@type='android.graphics.PointF']]"
		[Register ("viewToSourceCoord", "(Landroid/graphics/PointF;Landroid/graphics/PointF;)Landroid/graphics/PointF;", "")]
		public unsafe global::Android.Graphics.PointF ViewToSourceCoord (global::Android.Graphics.PointF p0, global::Android.Graphics.PointF p1)
		{
			if (id_viewToSourceCoord_Landroid_graphics_PointF_Landroid_graphics_PointF_ == IntPtr.Zero)
				id_viewToSourceCoord_Landroid_graphics_PointF_Landroid_graphics_PointF_ = JNIEnv.GetMethodID (class_ref, "viewToSourceCoord", "(Landroid/graphics/PointF;Landroid/graphics/PointF;)Landroid/graphics/PointF;");
			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				global::Android.Graphics.PointF __ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_viewToSourceCoord_Landroid_graphics_PointF_Landroid_graphics_PointF_, __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

		static IntPtr id_viewToSourceCoord_FF;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='viewToSourceCoord' and count(parameter)=2 and parameter[1][@type='float'] and parameter[2][@type='float']]"
		[Register ("viewToSourceCoord", "(FF)Landroid/graphics/PointF;", "")]
		public unsafe global::Android.Graphics.PointF ViewToSourceCoord (float p0, float p1)
		{
			if (id_viewToSourceCoord_FF == IntPtr.Zero)
				id_viewToSourceCoord_FF = JNIEnv.GetMethodID (class_ref, "viewToSourceCoord", "(FF)Landroid/graphics/PointF;");
			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				return global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_viewToSourceCoord_FF, __args), JniHandleOwnership.TransferLocalRef);
			} finally {
			}
		}

		static IntPtr id_viewToSourceCoord_FFLandroid_graphics_PointF_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='SubsamplingScaleImageView']/method[@name='viewToSourceCoord' and count(parameter)=3 and parameter[1][@type='float'] and parameter[2][@type='float'] and parameter[3][@type='android.graphics.PointF']]"
		[Register ("viewToSourceCoord", "(FFLandroid/graphics/PointF;)Landroid/graphics/PointF;", "")]
		public unsafe global::Android.Graphics.PointF ViewToSourceCoord (float p0, float p1, global::Android.Graphics.PointF p2)
		{
			if (id_viewToSourceCoord_FFLandroid_graphics_PointF_ == IntPtr.Zero)
				id_viewToSourceCoord_FFLandroid_graphics_PointF_ = JNIEnv.GetMethodID (class_ref, "viewToSourceCoord", "(FFLandroid/graphics/PointF;)Landroid/graphics/PointF;");
			try {
				JValue* __args = stackalloc JValue [3];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				__args [2] = new JValue (p2);
				global::Android.Graphics.PointF __ret = global::Java.Lang.Object.GetObject<global::Android.Graphics.PointF> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_viewToSourceCoord_FFLandroid_graphics_PointF_, __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

#region "Event implementation for Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener"
		public event EventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.ImageLoadErrorEventArgs> ImageLoadError {
			add {
				global::Java.Interop.EventHelper.AddEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor>(
						ref weak_implementor_SetOnImageEventListener,
						__CreateIOnImageEventListenerImplementor,
						SetOnImageEventListener,
						__h => __h.OnImageLoadErrorHandler += value);
			}
			remove {
				global::Java.Interop.EventHelper.RemoveEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor>(
						ref weak_implementor_SetOnImageEventListener,
						global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor.__IsEmpty,
						__v => SetOnImageEventListener (null),
						__h => __h.OnImageLoadErrorHandler -= value);
			}
		}

		public event EventHandler ImageLoaded {
			add {
				global::Java.Interop.EventHelper.AddEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor>(
						ref weak_implementor_SetOnImageEventListener,
						__CreateIOnImageEventListenerImplementor,
						SetOnImageEventListener,
						__h => __h.OnImageLoadedHandler += value);
			}
			remove {
				global::Java.Interop.EventHelper.RemoveEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor>(
						ref weak_implementor_SetOnImageEventListener,
						global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor.__IsEmpty,
						__v => SetOnImageEventListener (null),
						__h => __h.OnImageLoadedHandler -= value);
			}
		}

		public event EventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.PreviewLoadErrorEventArgs> PreviewLoadError {
			add {
				global::Java.Interop.EventHelper.AddEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor>(
						ref weak_implementor_SetOnImageEventListener,
						__CreateIOnImageEventListenerImplementor,
						SetOnImageEventListener,
						__h => __h.OnPreviewLoadErrorHandler += value);
			}
			remove {
				global::Java.Interop.EventHelper.RemoveEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor>(
						ref weak_implementor_SetOnImageEventListener,
						global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor.__IsEmpty,
						__v => SetOnImageEventListener (null),
						__h => __h.OnPreviewLoadErrorHandler -= value);
			}
		}

		public event EventHandler PreviewReleased {
			add {
				global::Java.Interop.EventHelper.AddEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor>(
						ref weak_implementor_SetOnImageEventListener,
						__CreateIOnImageEventListenerImplementor,
						SetOnImageEventListener,
						__h => __h.OnPreviewReleasedHandler += value);
			}
			remove {
				global::Java.Interop.EventHelper.RemoveEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor>(
						ref weak_implementor_SetOnImageEventListener,
						global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor.__IsEmpty,
						__v => SetOnImageEventListener (null),
						__h => __h.OnPreviewReleasedHandler -= value);
			}
		}

		public event EventHandler Ready {
			add {
				global::Java.Interop.EventHelper.AddEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor>(
						ref weak_implementor_SetOnImageEventListener,
						__CreateIOnImageEventListenerImplementor,
						SetOnImageEventListener,
						__h => __h.OnReadyHandler += value);
			}
			remove {
				global::Java.Interop.EventHelper.RemoveEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor>(
						ref weak_implementor_SetOnImageEventListener,
						global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor.__IsEmpty,
						__v => SetOnImageEventListener (null),
						__h => __h.OnReadyHandler -= value);
			}
		}

		public event EventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.TileLoadErrorEventArgs> TileLoadError {
			add {
				global::Java.Interop.EventHelper.AddEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor>(
						ref weak_implementor_SetOnImageEventListener,
						__CreateIOnImageEventListenerImplementor,
						SetOnImageEventListener,
						__h => __h.OnTileLoadErrorHandler += value);
			}
			remove {
				global::Java.Interop.EventHelper.RemoveEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor>(
						ref weak_implementor_SetOnImageEventListener,
						global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor.__IsEmpty,
						__v => SetOnImageEventListener (null),
						__h => __h.OnTileLoadErrorHandler -= value);
			}
		}

		WeakReference weak_implementor_SetOnImageEventListener;

		global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor __CreateIOnImageEventListenerImplementor ()
		{
			return new global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnImageEventListenerImplementor (this);
		}
#endregion
#region "Event implementation for Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener"
		public event EventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.CenterChangedEventArgs> CenterChanged {
			add {
				global::Java.Interop.EventHelper.AddEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListenerImplementor>(
						ref weak_implementor_SetOnStateChangedListener,
						__CreateIOnStateChangedListenerImplementor,
						SetOnStateChangedListener,
						__h => __h.OnCenterChangedHandler += value);
			}
			remove {
				global::Java.Interop.EventHelper.RemoveEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListenerImplementor>(
						ref weak_implementor_SetOnStateChangedListener,
						global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListenerImplementor.__IsEmpty,
						__v => SetOnStateChangedListener (null),
						__h => __h.OnCenterChangedHandler -= value);
			}
		}

		public event EventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.ScaleChangedEventArgs> ScaleChanged {
			add {
				global::Java.Interop.EventHelper.AddEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListenerImplementor>(
						ref weak_implementor_SetOnStateChangedListener,
						__CreateIOnStateChangedListenerImplementor,
						SetOnStateChangedListener,
						__h => __h.OnScaleChangedHandler += value);
			}
			remove {
				global::Java.Interop.EventHelper.RemoveEventHandler<global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListener, global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListenerImplementor>(
						ref weak_implementor_SetOnStateChangedListener,
						global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListenerImplementor.__IsEmpty,
						__v => SetOnStateChangedListener (null),
						__h => __h.OnScaleChangedHandler -= value);
			}
		}

		WeakReference weak_implementor_SetOnStateChangedListener;

		global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListenerImplementor __CreateIOnStateChangedListenerImplementor ()
		{
			return new global::Com.Davemorrissey.Labs.Subscaleview.SubsamplingScaleImageView.IOnStateChangedListenerImplementor (this);
		}
#endregion
	}
}
