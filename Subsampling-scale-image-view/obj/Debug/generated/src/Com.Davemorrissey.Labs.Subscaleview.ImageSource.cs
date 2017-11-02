using System;
using System.Collections.Generic;
using Android.Runtime;

namespace Com.Davemorrissey.Labs.Subscaleview {

	// Metadata.xml XPath class reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']"
	[global::Android.Runtime.Register ("com/davemorrissey/labs/subscaleview/ImageSource", DoNotGenerateAcw=true)]
	public sealed partial class ImageSource : global::Java.Lang.Object {

		internal static new IntPtr java_class_handle;
		internal static new IntPtr class_ref {
			get {
				return JNIEnv.FindClass ("com/davemorrissey/labs/subscaleview/ImageSource", ref java_class_handle);
			}
		}

		protected override IntPtr ThresholdClass {
			get { return class_ref; }
		}

		protected override global::System.Type ThresholdType {
			get { return typeof (ImageSource); }
		}

		internal ImageSource (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer) {}

		static IntPtr id_getBitmap;
		protected unsafe global::Android.Graphics.Bitmap Bitmap {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='getBitmap' and count(parameter)=0]"
			[Register ("getBitmap", "()Landroid/graphics/Bitmap;", "GetGetBitmapHandler")]
			get {
				if (id_getBitmap == IntPtr.Zero)
					id_getBitmap = JNIEnv.GetMethodID (class_ref, "getBitmap", "()Landroid/graphics/Bitmap;");
				try {
					return global::Java.Lang.Object.GetObject<global::Android.Graphics.Bitmap> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_getBitmap), JniHandleOwnership.TransferLocalRef);
				} finally {
				}
			}
		}

		static IntPtr id_isCached;
		protected unsafe bool IsCached {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='isCached' and count(parameter)=0]"
			[Register ("isCached", "()Z", "GetIsCachedHandler")]
			get {
				if (id_isCached == IntPtr.Zero)
					id_isCached = JNIEnv.GetMethodID (class_ref, "isCached", "()Z");
				try {
					return JNIEnv.CallBooleanMethod (((global::Java.Lang.Object) this).Handle, id_isCached);
				} finally {
				}
			}
		}

		static IntPtr id_getResource;
		protected unsafe global::Java.Lang.Integer Resource {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='getResource' and count(parameter)=0]"
			[Register ("getResource", "()Ljava/lang/Integer;", "GetGetResourceHandler")]
			get {
				if (id_getResource == IntPtr.Zero)
					id_getResource = JNIEnv.GetMethodID (class_ref, "getResource", "()Ljava/lang/Integer;");
				try {
					return global::Java.Lang.Object.GetObject<global::Java.Lang.Integer> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_getResource), JniHandleOwnership.TransferLocalRef);
				} finally {
				}
			}
		}

		static IntPtr id_getSHeight;
		protected unsafe int SHeight {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='getSHeight' and count(parameter)=0]"
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

		static IntPtr id_getSRegion;
		protected unsafe global::Android.Graphics.Rect SRegion {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='getSRegion' and count(parameter)=0]"
			[Register ("getSRegion", "()Landroid/graphics/Rect;", "GetGetSRegionHandler")]
			get {
				if (id_getSRegion == IntPtr.Zero)
					id_getSRegion = JNIEnv.GetMethodID (class_ref, "getSRegion", "()Landroid/graphics/Rect;");
				try {
					return global::Java.Lang.Object.GetObject<global::Android.Graphics.Rect> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_getSRegion), JniHandleOwnership.TransferLocalRef);
				} finally {
				}
			}
		}

		static IntPtr id_getSWidth;
		protected unsafe int SWidth {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='getSWidth' and count(parameter)=0]"
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

		static IntPtr id_getTile;
		protected unsafe bool Tile {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='getTile' and count(parameter)=0]"
			[Register ("getTile", "()Z", "GetGetTileHandler")]
			get {
				if (id_getTile == IntPtr.Zero)
					id_getTile = JNIEnv.GetMethodID (class_ref, "getTile", "()Z");
				try {
					return JNIEnv.CallBooleanMethod (((global::Java.Lang.Object) this).Handle, id_getTile);
				} finally {
				}
			}
		}

		static IntPtr id_getUri;
		protected unsafe global::Android.Net.Uri Uri {
			// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='getUri' and count(parameter)=0]"
			[Register ("getUri", "()Landroid/net/Uri;", "GetGetUriHandler")]
			get {
				if (id_getUri == IntPtr.Zero)
					id_getUri = JNIEnv.GetMethodID (class_ref, "getUri", "()Landroid/net/Uri;");
				try {
					return global::Java.Lang.Object.GetObject<global::Android.Net.Uri> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_getUri), JniHandleOwnership.TransferLocalRef);
				} finally {
				}
			}
		}

		static IntPtr id_asset_Ljava_lang_String_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='asset' and count(parameter)=1 and parameter[1][@type='java.lang.String']]"
		[Register ("asset", "(Ljava/lang/String;)Lcom/davemorrissey/labs/subscaleview/ImageSource;", "")]
		public static unsafe global::Com.Davemorrissey.Labs.Subscaleview.ImageSource Asset (string p0)
		{
			if (id_asset_Ljava_lang_String_ == IntPtr.Zero)
				id_asset_Ljava_lang_String_ = JNIEnv.GetStaticMethodID (class_ref, "asset", "(Ljava/lang/String;)Lcom/davemorrissey/labs/subscaleview/ImageSource;");
			IntPtr native_p0 = JNIEnv.NewString (p0);
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (native_p0);
				global::Com.Davemorrissey.Labs.Subscaleview.ImageSource __ret = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageSource> (JNIEnv.CallStaticObjectMethod  (class_ref, id_asset_Ljava_lang_String_, __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
				JNIEnv.DeleteLocalRef (native_p0);
			}
		}

		static IntPtr id_bitmap_Landroid_graphics_Bitmap_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='bitmap' and count(parameter)=1 and parameter[1][@type='android.graphics.Bitmap']]"
		[Register ("bitmap", "(Landroid/graphics/Bitmap;)Lcom/davemorrissey/labs/subscaleview/ImageSource;", "")]
		public static unsafe global::Com.Davemorrissey.Labs.Subscaleview.ImageSource InvokeBitmap (global::Android.Graphics.Bitmap p0)
		{
			if (id_bitmap_Landroid_graphics_Bitmap_ == IntPtr.Zero)
				id_bitmap_Landroid_graphics_Bitmap_ = JNIEnv.GetStaticMethodID (class_ref, "bitmap", "(Landroid/graphics/Bitmap;)Lcom/davemorrissey/labs/subscaleview/ImageSource;");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				global::Com.Davemorrissey.Labs.Subscaleview.ImageSource __ret = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageSource> (JNIEnv.CallStaticObjectMethod  (class_ref, id_bitmap_Landroid_graphics_Bitmap_, __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

		static IntPtr id_cachedBitmap_Landroid_graphics_Bitmap_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='cachedBitmap' and count(parameter)=1 and parameter[1][@type='android.graphics.Bitmap']]"
		[Register ("cachedBitmap", "(Landroid/graphics/Bitmap;)Lcom/davemorrissey/labs/subscaleview/ImageSource;", "")]
		public static unsafe global::Com.Davemorrissey.Labs.Subscaleview.ImageSource CachedBitmap (global::Android.Graphics.Bitmap p0)
		{
			if (id_cachedBitmap_Landroid_graphics_Bitmap_ == IntPtr.Zero)
				id_cachedBitmap_Landroid_graphics_Bitmap_ = JNIEnv.GetStaticMethodID (class_ref, "cachedBitmap", "(Landroid/graphics/Bitmap;)Lcom/davemorrissey/labs/subscaleview/ImageSource;");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				global::Com.Davemorrissey.Labs.Subscaleview.ImageSource __ret = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageSource> (JNIEnv.CallStaticObjectMethod  (class_ref, id_cachedBitmap_Landroid_graphics_Bitmap_, __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

		static IntPtr id_dimensions_II;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='dimensions' and count(parameter)=2 and parameter[1][@type='int'] and parameter[2][@type='int']]"
		[Register ("dimensions", "(II)Lcom/davemorrissey/labs/subscaleview/ImageSource;", "")]
		public unsafe global::Com.Davemorrissey.Labs.Subscaleview.ImageSource Dimensions (int p0, int p1)
		{
			if (id_dimensions_II == IntPtr.Zero)
				id_dimensions_II = JNIEnv.GetMethodID (class_ref, "dimensions", "(II)Lcom/davemorrissey/labs/subscaleview/ImageSource;");
			try {
				JValue* __args = stackalloc JValue [2];
				__args [0] = new JValue (p0);
				__args [1] = new JValue (p1);
				return global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageSource> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_dimensions_II, __args), JniHandleOwnership.TransferLocalRef);
			} finally {
			}
		}

		static IntPtr id_region_Landroid_graphics_Rect_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='region' and count(parameter)=1 and parameter[1][@type='android.graphics.Rect']]"
		[Register ("region", "(Landroid/graphics/Rect;)Lcom/davemorrissey/labs/subscaleview/ImageSource;", "")]
		public unsafe global::Com.Davemorrissey.Labs.Subscaleview.ImageSource Region (global::Android.Graphics.Rect p0)
		{
			if (id_region_Landroid_graphics_Rect_ == IntPtr.Zero)
				id_region_Landroid_graphics_Rect_ = JNIEnv.GetMethodID (class_ref, "region", "(Landroid/graphics/Rect;)Lcom/davemorrissey/labs/subscaleview/ImageSource;");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				global::Com.Davemorrissey.Labs.Subscaleview.ImageSource __ret = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageSource> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_region_Landroid_graphics_Rect_, __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

		static IntPtr id_resource_I;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='resource' and count(parameter)=1 and parameter[1][@type='int']]"
		[Register ("resource", "(I)Lcom/davemorrissey/labs/subscaleview/ImageSource;", "")]
		public static unsafe global::Com.Davemorrissey.Labs.Subscaleview.ImageSource InvokeResource (int p0)
		{
			if (id_resource_I == IntPtr.Zero)
				id_resource_I = JNIEnv.GetStaticMethodID (class_ref, "resource", "(I)Lcom/davemorrissey/labs/subscaleview/ImageSource;");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				return global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageSource> (JNIEnv.CallStaticObjectMethod  (class_ref, id_resource_I, __args), JniHandleOwnership.TransferLocalRef);
			} finally {
			}
		}

		static IntPtr id_tiling_Z;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='tiling' and count(parameter)=1 and parameter[1][@type='boolean']]"
		[Register ("tiling", "(Z)Lcom/davemorrissey/labs/subscaleview/ImageSource;", "")]
		public unsafe global::Com.Davemorrissey.Labs.Subscaleview.ImageSource Tiling (bool p0)
		{
			if (id_tiling_Z == IntPtr.Zero)
				id_tiling_Z = JNIEnv.GetMethodID (class_ref, "tiling", "(Z)Lcom/davemorrissey/labs/subscaleview/ImageSource;");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				return global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageSource> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_tiling_Z, __args), JniHandleOwnership.TransferLocalRef);
			} finally {
			}
		}

		static IntPtr id_tilingDisabled;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='tilingDisabled' and count(parameter)=0]"
		[Register ("tilingDisabled", "()Lcom/davemorrissey/labs/subscaleview/ImageSource;", "")]
		public unsafe global::Com.Davemorrissey.Labs.Subscaleview.ImageSource TilingDisabled ()
		{
			if (id_tilingDisabled == IntPtr.Zero)
				id_tilingDisabled = JNIEnv.GetMethodID (class_ref, "tilingDisabled", "()Lcom/davemorrissey/labs/subscaleview/ImageSource;");
			try {
				return global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageSource> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_tilingDisabled), JniHandleOwnership.TransferLocalRef);
			} finally {
			}
		}

		static IntPtr id_tilingEnabled;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='tilingEnabled' and count(parameter)=0]"
		[Register ("tilingEnabled", "()Lcom/davemorrissey/labs/subscaleview/ImageSource;", "")]
		public unsafe global::Com.Davemorrissey.Labs.Subscaleview.ImageSource TilingEnabled ()
		{
			if (id_tilingEnabled == IntPtr.Zero)
				id_tilingEnabled = JNIEnv.GetMethodID (class_ref, "tilingEnabled", "()Lcom/davemorrissey/labs/subscaleview/ImageSource;");
			try {
				return global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageSource> (JNIEnv.CallObjectMethod (((global::Java.Lang.Object) this).Handle, id_tilingEnabled), JniHandleOwnership.TransferLocalRef);
			} finally {
			}
		}

		static IntPtr id_uri_Landroid_net_Uri_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='uri' and count(parameter)=1 and parameter[1][@type='android.net.Uri']]"
		[Register ("uri", "(Landroid/net/Uri;)Lcom/davemorrissey/labs/subscaleview/ImageSource;", "")]
		public static unsafe global::Com.Davemorrissey.Labs.Subscaleview.ImageSource InvokeUri (global::Android.Net.Uri p0)
		{
			if (id_uri_Landroid_net_Uri_ == IntPtr.Zero)
				id_uri_Landroid_net_Uri_ = JNIEnv.GetStaticMethodID (class_ref, "uri", "(Landroid/net/Uri;)Lcom/davemorrissey/labs/subscaleview/ImageSource;");
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (p0);
				global::Com.Davemorrissey.Labs.Subscaleview.ImageSource __ret = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageSource> (JNIEnv.CallStaticObjectMethod  (class_ref, id_uri_Landroid_net_Uri_, __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
			}
		}

		static IntPtr id_uri_Ljava_lang_String_;
		// Metadata.xml XPath method reference: path="/api/package[@name='com.davemorrissey.labs.subscaleview']/class[@name='ImageSource']/method[@name='uri' and count(parameter)=1 and parameter[1][@type='java.lang.String']]"
		[Register ("uri", "(Ljava/lang/String;)Lcom/davemorrissey/labs/subscaleview/ImageSource;", "")]
		public static unsafe global::Com.Davemorrissey.Labs.Subscaleview.ImageSource InvokeUri (string p0)
		{
			if (id_uri_Ljava_lang_String_ == IntPtr.Zero)
				id_uri_Ljava_lang_String_ = JNIEnv.GetStaticMethodID (class_ref, "uri", "(Ljava/lang/String;)Lcom/davemorrissey/labs/subscaleview/ImageSource;");
			IntPtr native_p0 = JNIEnv.NewString (p0);
			try {
				JValue* __args = stackalloc JValue [1];
				__args [0] = new JValue (native_p0);
				global::Com.Davemorrissey.Labs.Subscaleview.ImageSource __ret = global::Java.Lang.Object.GetObject<global::Com.Davemorrissey.Labs.Subscaleview.ImageSource> (JNIEnv.CallStaticObjectMethod  (class_ref, id_uri_Ljava_lang_String_, __args), JniHandleOwnership.TransferLocalRef);
				return __ret;
			} finally {
				JNIEnv.DeleteLocalRef (native_p0);
			}
		}

	}
}
