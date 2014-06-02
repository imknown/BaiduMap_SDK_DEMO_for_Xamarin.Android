using Android.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Com.Baidu.Platform.Comapi.Basestruct
{
    public partial class GeoPoint : Java.Lang.Object { };
}

namespace Com.Baidu.Platform.Comapi.Map.Base
{
    public abstract partial class SDKLayerImageBase : SDKLayerBase
    {
        //public SDKLayerImageBase()  { }

        protected SDKLayerImageBase(IntPtr javaReference, JniHandleOwnership transfer, bool imknown) : base(javaReference, transfer, imknown) { }
    }

    public abstract partial class SDKLayerBase : Com.Baidu.Mapapi.Map.Overlay
    {
        //public SDKLayerBase() { }

        protected SDKLayerBase(IntPtr javaReference, JniHandleOwnership transfer, bool imknown) : base(javaReference, transfer, imknown) { }
    }

    public abstract partial class Overlay : Java.Lang.Object
    {
        protected Overlay(IntPtr javaReference, JniHandleOwnership transfer, bool imknown) : base(javaReference, transfer) { }
    }

    public partial class SDKLayerDataModelImageBase : SDKLayerDataModelBase
    {
    }

    public partial class SDKLayerDataModelBase : Java.Lang.Object
    {
    }
}

namespace Com.Baidu.Mapapi.Map
{
    public abstract partial class Overlay : Com.Baidu.Platform.Comapi.Map.Base.Overlay
    {
        protected Overlay(IntPtr javaReference, JniHandleOwnership transfer, bool imknown) : base(javaReference, transfer, imknown) { }
    }

    public partial class MapView : global::Android.Views.ViewGroup
    {
    }

    public partial class OverlayItem : Com.Baidu.Platform.Comapi.Map.Base.SDKLayerDataModelImageBase
    {
    }

    // Metadata.xml XPath class reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']"
    [global::Android.Runtime.Register("com/baidu/mapapi/map/ItemizedOverlay", DoNotGenerateAcw = true)]
    public partial class ItemizedOverlay<T> : Com.Baidu.Platform.Comapi.Map.Base.SDKLayerImageBase, Java.Util.IComparator
    where T : Com.Baidu.Mapapi.Map.OverlayItem
    {
        internal static new IntPtr java_class_handle;
        internal static new IntPtr class_ref
        {
            get
            {
                return JNIEnv.FindClass("com/baidu/mapapi/map/ItemizedOverlay", ref java_class_handle);
            }
        }

        protected override IntPtr ThresholdClass
        {
            get { return class_ref; }
        }

        protected override global::System.Type ThresholdType
        {
            get { return typeof(ItemizedOverlay<T>); }
        }

        protected ItemizedOverlay(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer, true) { }

        static IntPtr id_ctor_Landroid_graphics_drawable_Drawable_Lcom_baidu_mapapi_map_MapView_;
        // Metadata.xml XPath constructor reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/constructor[@name='ItemizedOverlay' and count(parameter)=2 and parameter[1][@type='android.graphics.drawable.Drawable'] and parameter[2][@type='com.baidu.mapapi.map.MapView']]"
        [Register(".ctor", "(Landroid/graphics/drawable/Drawable;Lcom/baidu/mapapi/map/MapView;)V", "")]
        public ItemizedOverlay(global::Android.Graphics.Drawables.Drawable p0, global::Com.Baidu.Mapapi.Map.MapView p1)
            : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer, true)
        {
            if (Handle != IntPtr.Zero)
                return;

            if (GetType() != typeof(ItemizedOverlay<T>))
            {
                SetHandle(
                        global::Android.Runtime.JNIEnv.StartCreateInstance(GetType(), "(Landroid/graphics/drawable/Drawable;Lcom/baidu/mapapi/map/MapView;)V", new JValue(p0), new JValue(p1)),
                        JniHandleOwnership.TransferLocalRef);
                global::Android.Runtime.JNIEnv.FinishCreateInstance(Handle, "(Landroid/graphics/drawable/Drawable;Lcom/baidu/mapapi/map/MapView;)V", new JValue(p0), new JValue(p1));
                return;
            }

            if (id_ctor_Landroid_graphics_drawable_Drawable_Lcom_baidu_mapapi_map_MapView_ == IntPtr.Zero)
                id_ctor_Landroid_graphics_drawable_Drawable_Lcom_baidu_mapapi_map_MapView_ = JNIEnv.GetMethodID(class_ref, "<init>", "(Landroid/graphics/drawable/Drawable;Lcom/baidu/mapapi/map/MapView;)V");
            SetHandle(
                    global::Android.Runtime.JNIEnv.StartCreateInstance(class_ref, id_ctor_Landroid_graphics_drawable_Drawable_Lcom_baidu_mapapi_map_MapView_, new JValue(p0), new JValue(p1)),
                    JniHandleOwnership.TransferLocalRef);
            JNIEnv.FinishCreateInstance(Handle, class_ref, id_ctor_Landroid_graphics_drawable_Drawable_Lcom_baidu_mapapi_map_MapView_, new JValue(p0), new JValue(p1));
        }

        static Delegate cb_getAllItem;
#pragma warning disable 0169
        static Delegate GetGetAllItemHandler()
        {
            if (cb_getAllItem == null)
                cb_getAllItem = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr>)n_GetAllItem);
            return cb_getAllItem;
        }

        static IntPtr n_GetAllItem(IntPtr jnienv, IntPtr native__this)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            return global::Android.Runtime.JavaList<global::Com.Baidu.Mapapi.Map.OverlayItem>.ToLocalJniHandle(__this.AllItem);
        }
#pragma warning restore 0169

        static IntPtr id_getAllItem;
        public virtual global::System.Collections.Generic.IList<global::Com.Baidu.Mapapi.Map.OverlayItem> AllItem
        {
            // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='getAllItem' and count(parameter)=0]"
            [Register("getAllItem", "()Ljava/util/ArrayList;", "GetGetAllItemHandler")]
            get
            {
                if (id_getAllItem == IntPtr.Zero)
                    id_getAllItem = JNIEnv.GetMethodID(class_ref, "getAllItem", "()Ljava/util/ArrayList;");

                if (GetType() == ThresholdType)
                    return global::Android.Runtime.JavaList<global::Com.Baidu.Mapapi.Map.OverlayItem>.FromJniHandle(JNIEnv.CallObjectMethod(Handle, id_getAllItem), JniHandleOwnership.TransferLocalRef);
                else
                    return global::Android.Runtime.JavaList<global::Com.Baidu.Mapapi.Map.OverlayItem>.FromJniHandle(JNIEnv.CallNonvirtualObjectMethod(Handle, ThresholdClass, id_getAllItem), JniHandleOwnership.TransferLocalRef);
            }
        }

        static Delegate cb_getCenter;
#pragma warning disable 0169
        static Delegate GetGetCenterHandler()
        {
            if (cb_getCenter == null)
                cb_getCenter = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr>)n_GetCenter);
            return cb_getCenter;
        }

        static IntPtr n_GetCenter(IntPtr jnienv, IntPtr native__this)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            return JNIEnv.ToLocalJniHandle(__this.Center);
        }
#pragma warning restore 0169

        static IntPtr id_getCenter;
        public virtual global::Com.Baidu.Platform.Comapi.Basestruct.GeoPoint Center
        {
            // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='getCenter' and count(parameter)=0]"
            [Register("getCenter", "()Lcom/baidu/platform/comapi/basestruct/GeoPoint;", "GetGetCenterHandler")]
            get
            {
                if (id_getCenter == IntPtr.Zero)
                    id_getCenter = JNIEnv.GetMethodID(class_ref, "getCenter", "()Lcom/baidu/platform/comapi/basestruct/GeoPoint;");

                if (GetType() == ThresholdType)
                    return global::Java.Lang.Object.GetObject<global::Com.Baidu.Platform.Comapi.Basestruct.GeoPoint>(JNIEnv.CallObjectMethod(Handle, id_getCenter), JniHandleOwnership.TransferLocalRef);
                else
                    return global::Java.Lang.Object.GetObject<global::Com.Baidu.Platform.Comapi.Basestruct.GeoPoint>(JNIEnv.CallNonvirtualObjectMethod(Handle, ThresholdClass, id_getCenter), JniHandleOwnership.TransferLocalRef);
            }
        }

        static Delegate cb_getLatSpanE6;
#pragma warning disable 0169
        static Delegate GetGetLatSpanE6Handler()
        {
            if (cb_getLatSpanE6 == null)
                cb_getLatSpanE6 = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, int>)n_GetLatSpanE6);
            return cb_getLatSpanE6;
        }

        static int n_GetLatSpanE6(IntPtr jnienv, IntPtr native__this)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            return __this.LatSpanE6;
        }
#pragma warning restore 0169

        static IntPtr id_getLatSpanE6;
        public virtual int LatSpanE6
        {
            // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='getLatSpanE6' and count(parameter)=0]"
            [Register("getLatSpanE6", "()I", "GetGetLatSpanE6Handler")]
            get
            {
                if (id_getLatSpanE6 == IntPtr.Zero)
                    id_getLatSpanE6 = JNIEnv.GetMethodID(class_ref, "getLatSpanE6", "()I");

                if (GetType() == ThresholdType)
                    return JNIEnv.CallIntMethod(Handle, id_getLatSpanE6);
                else
                    return JNIEnv.CallNonvirtualIntMethod(Handle, ThresholdClass, id_getLatSpanE6);
            }
        }

        static Delegate cb_getLonSpanE6;
#pragma warning disable 0169
        static Delegate GetGetLonSpanE6Handler()
        {
            if (cb_getLonSpanE6 == null)
                cb_getLonSpanE6 = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, int>)n_GetLonSpanE6);
            return cb_getLonSpanE6;
        }

        static int n_GetLonSpanE6(IntPtr jnienv, IntPtr native__this)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            return __this.LonSpanE6;
        }
#pragma warning restore 0169

        static IntPtr id_getLonSpanE6;
        public virtual int LonSpanE6
        {
            // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='getLonSpanE6' and count(parameter)=0]"
            [Register("getLonSpanE6", "()I", "GetGetLonSpanE6Handler")]
            get
            {
                if (id_getLonSpanE6 == IntPtr.Zero)
                    id_getLonSpanE6 = JNIEnv.GetMethodID(class_ref, "getLonSpanE6", "()I");

                if (GetType() == ThresholdType)
                    return JNIEnv.CallIntMethod(Handle, id_getLonSpanE6);
                else
                    return JNIEnv.CallNonvirtualIntMethod(Handle, ThresholdClass, id_getLonSpanE6);
            }
        }

        static Delegate cb_addItem_Lcom_baidu_mapapi_map_OverlayItem_;
#pragma warning disable 0169
        static Delegate GetAddItem_Lcom_baidu_mapapi_map_OverlayItem_Handler()
        {
            if (cb_addItem_Lcom_baidu_mapapi_map_OverlayItem_ == null)
                cb_addItem_Lcom_baidu_mapapi_map_OverlayItem_ = JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, IntPtr>)n_AddItem_Lcom_baidu_mapapi_map_OverlayItem_);
            return cb_addItem_Lcom_baidu_mapapi_map_OverlayItem_;
        }

        static void n_AddItem_Lcom_baidu_mapapi_map_OverlayItem_(IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            global::Com.Baidu.Mapapi.Map.OverlayItem p0 = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.OverlayItem>(native_p0, JniHandleOwnership.DoNotTransfer);
            __this.AddItem(p0);
        }
#pragma warning restore 0169

        static IntPtr id_addItem_Lcom_baidu_mapapi_map_OverlayItem_;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='addItem' and count(parameter)=1 and parameter[1][@type='com.baidu.mapapi.map.OverlayItem']]"
        [Register("addItem", "(Lcom/baidu/mapapi/map/OverlayItem;)V", "GetAddItem_Lcom_baidu_mapapi_map_OverlayItem_Handler")]
        public virtual void AddItem(global::Com.Baidu.Mapapi.Map.OverlayItem p0)
        {
            if (id_addItem_Lcom_baidu_mapapi_map_OverlayItem_ == IntPtr.Zero)
                id_addItem_Lcom_baidu_mapapi_map_OverlayItem_ = JNIEnv.GetMethodID(class_ref, "addItem", "(Lcom/baidu/mapapi/map/OverlayItem;)V");

            if (GetType() == ThresholdType)
                JNIEnv.CallVoidMethod(Handle, id_addItem_Lcom_baidu_mapapi_map_OverlayItem_, new JValue(p0));
            else
                JNIEnv.CallNonvirtualVoidMethod(Handle, ThresholdClass, id_addItem_Lcom_baidu_mapapi_map_OverlayItem_, new JValue(p0));
        }

        static Delegate cb_addItem_Ljava_util_List_;
#pragma warning disable 0169
        static Delegate GetAddItem_Ljava_util_List_Handler()
        {
            if (cb_addItem_Ljava_util_List_ == null)
                cb_addItem_Ljava_util_List_ = JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, IntPtr>)n_AddItem_Ljava_util_List_);
            return cb_addItem_Ljava_util_List_;
        }

        static void n_AddItem_Ljava_util_List_(IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            System.Collections.Generic.IList<Com.Baidu.Mapapi.Map.OverlayItem> p0 = global::Android.Runtime.JavaList<global::Com.Baidu.Mapapi.Map.OverlayItem>.FromJniHandle(native_p0, JniHandleOwnership.DoNotTransfer);
            __this.AddItem(p0);
        }
#pragma warning restore 0169

        static IntPtr id_addItem_Ljava_util_List_;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='addItem' and count(parameter)=1 and parameter[1][@type='java.util.List']]"
        [Register("addItem", "(Ljava/util/List;)V", "GetAddItem_Ljava_util_List_Handler")]
        public virtual void AddItem(global::System.Collections.Generic.IList<global::Com.Baidu.Mapapi.Map.OverlayItem> p0)
        {
            if (id_addItem_Ljava_util_List_ == IntPtr.Zero)
                id_addItem_Ljava_util_List_ = JNIEnv.GetMethodID(class_ref, "addItem", "(Ljava/util/List;)V");
            IntPtr native_p0 = global::Android.Runtime.JavaList<global::Com.Baidu.Mapapi.Map.OverlayItem>.ToLocalJniHandle(p0);

            if (GetType() == ThresholdType)
                JNIEnv.CallVoidMethod(Handle, id_addItem_Ljava_util_List_, new JValue(native_p0));
            else
                JNIEnv.CallNonvirtualVoidMethod(Handle, ThresholdClass, id_addItem_Ljava_util_List_, new JValue(native_p0));
            JNIEnv.DeleteLocalRef(native_p0);
        }

        static IntPtr id_boundCenter_Lcom_baidu_mapapi_map_OverlayItem_;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='boundCenter' and count(parameter)=1 and parameter[1][@type='com.baidu.mapapi.map.OverlayItem']]"
        [Register("boundCenter", "(Lcom/baidu/mapapi/map/OverlayItem;)V", "")]
        protected static void BoundCenter(global::Com.Baidu.Mapapi.Map.OverlayItem p0)
        {
            if (id_boundCenter_Lcom_baidu_mapapi_map_OverlayItem_ == IntPtr.Zero)
                id_boundCenter_Lcom_baidu_mapapi_map_OverlayItem_ = JNIEnv.GetStaticMethodID(class_ref, "boundCenter", "(Lcom/baidu/mapapi/map/OverlayItem;)V");
            JNIEnv.CallStaticVoidMethod(class_ref, id_boundCenter_Lcom_baidu_mapapi_map_OverlayItem_, new JValue(p0));
        }

        static IntPtr id_boundCenterBottom_Lcom_baidu_mapapi_map_OverlayItem_;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='boundCenterBottom' and count(parameter)=1 and parameter[1][@type='com.baidu.mapapi.map.OverlayItem']]"
        [Register("boundCenterBottom", "(Lcom/baidu/mapapi/map/OverlayItem;)V", "")]
        protected static void BoundCenterBottom(global::Com.Baidu.Mapapi.Map.OverlayItem p0)
        {
            if (id_boundCenterBottom_Lcom_baidu_mapapi_map_OverlayItem_ == IntPtr.Zero)
                id_boundCenterBottom_Lcom_baidu_mapapi_map_OverlayItem_ = JNIEnv.GetStaticMethodID(class_ref, "boundCenterBottom", "(Lcom/baidu/mapapi/map/OverlayItem;)V");
            JNIEnv.CallStaticVoidMethod(class_ref, id_boundCenterBottom_Lcom_baidu_mapapi_map_OverlayItem_, new JValue(p0));
        }

        static Delegate cb_compare_Ljava_lang_Integer_Ljava_lang_Integer_;
#pragma warning disable 0169
        static Delegate GetCompare_Ljava_lang_Integer_Ljava_lang_Integer_Handler()
        {
            if (cb_compare_Ljava_lang_Integer_Ljava_lang_Integer_ == null)
                cb_compare_Ljava_lang_Integer_Ljava_lang_Integer_ = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr, IntPtr, int>)n_Compare_Ljava_lang_Integer_Ljava_lang_Integer_);
            return cb_compare_Ljava_lang_Integer_Ljava_lang_Integer_;
        }

        static int n_Compare_Ljava_lang_Integer_Ljava_lang_Integer_(IntPtr jnienv, IntPtr native__this, IntPtr native_p0, IntPtr native_p1)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            global::Java.Lang.Integer p0 = global::Java.Lang.Object.GetObject<global::Java.Lang.Integer>(native_p0, JniHandleOwnership.DoNotTransfer);
            global::Java.Lang.Integer p1 = global::Java.Lang.Object.GetObject<global::Java.Lang.Integer>(native_p1, JniHandleOwnership.DoNotTransfer);
            int __ret = __this.Compare(p0, p1);
            return __ret;
        }
#pragma warning restore 0169

        static IntPtr id_compare_Ljava_lang_Integer_Ljava_lang_Integer_;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='compare' and count(parameter)=2 and parameter[1][@type='java.lang.Integer'] and parameter[2][@type='java.lang.Integer']]"
        [Register("compare", "(Ljava/lang/Integer;Ljava/lang/Integer;)I", "GetCompare_Ljava_lang_Integer_Ljava_lang_Integer_Handler")]
        public virtual int Compare(global::Java.Lang.Integer p0, global::Java.Lang.Integer p1)
        {
            if (id_compare_Ljava_lang_Integer_Ljava_lang_Integer_ == IntPtr.Zero)
                id_compare_Ljava_lang_Integer_Ljava_lang_Integer_ = JNIEnv.GetMethodID(class_ref, "compare", "(Ljava/lang/Integer;Ljava/lang/Integer;)I");

            int __ret;
            if (GetType() == ThresholdType)
                __ret = JNIEnv.CallIntMethod(Handle, id_compare_Ljava_lang_Integer_Ljava_lang_Integer_, new JValue(p0), new JValue(p1));
            else
                __ret = JNIEnv.CallNonvirtualIntMethod(Handle, ThresholdClass, id_compare_Ljava_lang_Integer_Ljava_lang_Integer_, new JValue(p0), new JValue(p1));
            return __ret;
        }

        static Delegate cb_createItem_I;
#pragma warning disable 0169
        static Delegate GetCreateItem_IHandler()
        {
            if (cb_createItem_I == null)
                cb_createItem_I = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, int, IntPtr>)n_CreateItem_I);
            return cb_createItem_I;
        }

        static IntPtr n_CreateItem_I(IntPtr jnienv, IntPtr native__this, int p0)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            return JNIEnv.ToLocalJniHandle(__this.CreateItem(p0));
        }
#pragma warning restore 0169

        static IntPtr id_createItem_I;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='createItem' and count(parameter)=1 and parameter[1][@type='int']]"
        [Register("createItem", "(I)Lcom/baidu/mapapi/map/OverlayItem;", "GetCreateItem_IHandler")]
        protected virtual global::Java.Lang.Object CreateItem(int p0)
        {
            if (id_createItem_I == IntPtr.Zero)
                id_createItem_I = JNIEnv.GetMethodID(class_ref, "createItem", "(I)Lcom/baidu/mapapi/map/OverlayItem;");

            if (GetType() == ThresholdType)
                return (Java.Lang.Object)global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(JNIEnv.CallObjectMethod(Handle, id_createItem_I, new JValue(p0)), JniHandleOwnership.TransferLocalRef);
            else
                return (Java.Lang.Object)global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(JNIEnv.CallNonvirtualObjectMethod(Handle, ThresholdClass, id_createItem_I, new JValue(p0)), JniHandleOwnership.TransferLocalRef);
        }

        static Delegate cb_getIndexToDraw_I;
#pragma warning disable 0169
        static Delegate GetGetIndexToDraw_IHandler()
        {
            if (cb_getIndexToDraw_I == null)
                cb_getIndexToDraw_I = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, int, int>)n_GetIndexToDraw_I);
            return cb_getIndexToDraw_I;
        }

        static int n_GetIndexToDraw_I(IntPtr jnienv, IntPtr native__this, int p0)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            return __this.GetIndexToDraw(p0);
        }
#pragma warning restore 0169

        static IntPtr id_getIndexToDraw_I;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='getIndexToDraw' and count(parameter)=1 and parameter[1][@type='int']]"
        [Register("getIndexToDraw", "(I)I", "GetGetIndexToDraw_IHandler")]
        protected virtual int GetIndexToDraw(int p0)
        {
            if (id_getIndexToDraw_I == IntPtr.Zero)
                id_getIndexToDraw_I = JNIEnv.GetMethodID(class_ref, "getIndexToDraw", "(I)I");

            if (GetType() == ThresholdType)
                return JNIEnv.CallIntMethod(Handle, id_getIndexToDraw_I, new JValue(p0));
            else
                return JNIEnv.CallNonvirtualIntMethod(Handle, ThresholdClass, id_getIndexToDraw_I, new JValue(p0));
        }

        static IntPtr id_getItem_I;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='getItem' and count(parameter)=1 and parameter[1][@type='int']]"
        [Register("getItem", "(I)Lcom/baidu/mapapi/map/OverlayItem;", "")]
        public global::Com.Baidu.Mapapi.Map.OverlayItem GetItem(int p0)
        {
            if (id_getItem_I == IntPtr.Zero)
                id_getItem_I = JNIEnv.GetMethodID(class_ref, "getItem", "(I)Lcom/baidu/mapapi/map/OverlayItem;");
            return global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.OverlayItem>(JNIEnv.CallObjectMethod(Handle, id_getItem_I, new JValue(p0)), JniHandleOwnership.TransferLocalRef);
        }

        static Delegate cb_hitTest_Lcom_baidu_mapapi_map_OverlayItem_Landroid_graphics_drawable_Drawable_II;
#pragma warning disable 0169
        static Delegate GetHitTest_Lcom_baidu_mapapi_map_OverlayItem_Landroid_graphics_drawable_Drawable_IIHandler()
        {
            if (cb_hitTest_Lcom_baidu_mapapi_map_OverlayItem_Landroid_graphics_drawable_Drawable_II == null)
                cb_hitTest_Lcom_baidu_mapapi_map_OverlayItem_Landroid_graphics_drawable_Drawable_II = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr, IntPtr, int, int, bool>)n_HitTest_Lcom_baidu_mapapi_map_OverlayItem_Landroid_graphics_drawable_Drawable_II);
            return cb_hitTest_Lcom_baidu_mapapi_map_OverlayItem_Landroid_graphics_drawable_Drawable_II;
        }

        static bool n_HitTest_Lcom_baidu_mapapi_map_OverlayItem_Landroid_graphics_drawable_Drawable_II(IntPtr jnienv, IntPtr native__this, IntPtr native_p0, IntPtr native_p1, int p2, int p3)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            global::Com.Baidu.Mapapi.Map.OverlayItem p0 = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.OverlayItem>(native_p0, JniHandleOwnership.DoNotTransfer);
            global::Android.Graphics.Drawables.Drawable p1 = global::Java.Lang.Object.GetObject<global::Android.Graphics.Drawables.Drawable>(native_p1, JniHandleOwnership.DoNotTransfer);
            bool __ret = __this.HitTest(p0, p1, p2, p3);
            return __ret;
        }
#pragma warning restore 0169

        static IntPtr id_hitTest_Lcom_baidu_mapapi_map_OverlayItem_Landroid_graphics_drawable_Drawable_II;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='hitTest' and count(parameter)=4 and parameter[1][@type='com.baidu.mapapi.map.OverlayItem'] and parameter[2][@type='android.graphics.drawable.Drawable'] and parameter[3][@type='int'] and parameter[4][@type='int']]"
        [Register("hitTest", "(Lcom/baidu/mapapi/map/OverlayItem;Landroid/graphics/drawable/Drawable;II)Z", "GetHitTest_Lcom_baidu_mapapi_map_OverlayItem_Landroid_graphics_drawable_Drawable_IIHandler")]
        protected virtual bool HitTest(global::Com.Baidu.Mapapi.Map.OverlayItem p0, global::Android.Graphics.Drawables.Drawable p1, int p2, int p3)
        {
            if (id_hitTest_Lcom_baidu_mapapi_map_OverlayItem_Landroid_graphics_drawable_Drawable_II == IntPtr.Zero)
                id_hitTest_Lcom_baidu_mapapi_map_OverlayItem_Landroid_graphics_drawable_Drawable_II = JNIEnv.GetMethodID(class_ref, "hitTest", "(Lcom/baidu/mapapi/map/OverlayItem;Landroid/graphics/drawable/Drawable;II)Z");

            bool __ret;
            if (GetType() == ThresholdType)
                __ret = JNIEnv.CallBooleanMethod(Handle, id_hitTest_Lcom_baidu_mapapi_map_OverlayItem_Landroid_graphics_drawable_Drawable_II, new JValue(p0), new JValue(p1), new JValue(p2), new JValue(p3));
            else
                __ret = JNIEnv.CallNonvirtualBooleanMethod(Handle, ThresholdClass, id_hitTest_Lcom_baidu_mapapi_map_OverlayItem_Landroid_graphics_drawable_Drawable_II, new JValue(p0), new JValue(p1), new JValue(p2), new JValue(p3));
            return __ret;
        }

        static Delegate cb_onTap_Lcom_baidu_platform_comapi_basestruct_GeoPoint_Lcom_baidu_mapapi_map_MapView_;
#pragma warning disable 0169
        static Delegate GetOnTap_Lcom_baidu_platform_comapi_basestruct_GeoPoint_Lcom_baidu_mapapi_map_MapView_Handler()
        {
            if (cb_onTap_Lcom_baidu_platform_comapi_basestruct_GeoPoint_Lcom_baidu_mapapi_map_MapView_ == null)
                cb_onTap_Lcom_baidu_platform_comapi_basestruct_GeoPoint_Lcom_baidu_mapapi_map_MapView_ = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr, IntPtr, bool>)n_OnTap_Lcom_baidu_platform_comapi_basestruct_GeoPoint_Lcom_baidu_mapapi_map_MapView_);
            return cb_onTap_Lcom_baidu_platform_comapi_basestruct_GeoPoint_Lcom_baidu_mapapi_map_MapView_;
        }

        static bool n_OnTap_Lcom_baidu_platform_comapi_basestruct_GeoPoint_Lcom_baidu_mapapi_map_MapView_(IntPtr jnienv, IntPtr native__this, IntPtr native_p0, IntPtr native_p1)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            global::Com.Baidu.Platform.Comapi.Basestruct.GeoPoint p0 = global::Java.Lang.Object.GetObject<global::Com.Baidu.Platform.Comapi.Basestruct.GeoPoint>(native_p0, JniHandleOwnership.DoNotTransfer);
            global::Com.Baidu.Mapapi.Map.MapView p1 = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.MapView>(native_p1, JniHandleOwnership.DoNotTransfer);
            bool __ret = __this.OnTap(p0, p1);
            return __ret;
        }
#pragma warning restore 0169

        static IntPtr id_onTap_Lcom_baidu_platform_comapi_basestruct_GeoPoint_Lcom_baidu_mapapi_map_MapView_;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='onTap' and count(parameter)=2 and parameter[1][@type='com.baidu.platform.comapi.basestruct.GeoPoint'] and parameter[2][@type='com.baidu.mapapi.map.MapView']]"
        [Register("onTap", "(Lcom/baidu/platform/comapi/basestruct/GeoPoint;Lcom/baidu/mapapi/map/MapView;)Z", "GetOnTap_Lcom_baidu_platform_comapi_basestruct_GeoPoint_Lcom_baidu_mapapi_map_MapView_Handler")]
        public virtual bool OnTap(global::Com.Baidu.Platform.Comapi.Basestruct.GeoPoint p0, global::Com.Baidu.Mapapi.Map.MapView p1)
        {
            if (id_onTap_Lcom_baidu_platform_comapi_basestruct_GeoPoint_Lcom_baidu_mapapi_map_MapView_ == IntPtr.Zero)
                id_onTap_Lcom_baidu_platform_comapi_basestruct_GeoPoint_Lcom_baidu_mapapi_map_MapView_ = JNIEnv.GetMethodID(class_ref, "onTap", "(Lcom/baidu/platform/comapi/basestruct/GeoPoint;Lcom/baidu/mapapi/map/MapView;)Z");

            bool __ret;
            if (GetType() == ThresholdType)
                __ret = JNIEnv.CallBooleanMethod(Handle, id_onTap_Lcom_baidu_platform_comapi_basestruct_GeoPoint_Lcom_baidu_mapapi_map_MapView_, new JValue(p0), new JValue(p1));
            else
                __ret = JNIEnv.CallNonvirtualBooleanMethod(Handle, ThresholdClass, id_onTap_Lcom_baidu_platform_comapi_basestruct_GeoPoint_Lcom_baidu_mapapi_map_MapView_, new JValue(p0), new JValue(p1));
            return __ret;
        }

        static Delegate cb_onTap_I;
#pragma warning disable 0169
        static Delegate GetOnTap_IHandler()
        {
            if (cb_onTap_I == null)
                cb_onTap_I = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, int, bool>)n_OnTap_I);
            return cb_onTap_I;
        }

        static bool n_OnTap_I(IntPtr jnienv, IntPtr native__this, int p0)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            return __this.OnTap(p0);
        }
#pragma warning restore 0169

        static IntPtr id_onTap_I;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='onTap' and count(parameter)=1 and parameter[1][@type='int']]"
        [Register("onTap", "(I)Z", "GetOnTap_IHandler")]
        protected virtual bool OnTap(int p0)
        {
            if (id_onTap_I == IntPtr.Zero)
                id_onTap_I = JNIEnv.GetMethodID(class_ref, "onTap", "(I)Z");

            if (GetType() == ThresholdType)
                return JNIEnv.CallBooleanMethod(Handle, id_onTap_I, new JValue(p0));
            else
                return JNIEnv.CallNonvirtualBooleanMethod(Handle, ThresholdClass, id_onTap_I, new JValue(p0));
        }

        static Delegate cb_removeAll;
#pragma warning disable 0169
        static Delegate GetRemoveAllHandler()
        {
            if (cb_removeAll == null)
                cb_removeAll = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, bool>)n_RemoveAll);
            return cb_removeAll;
        }

        static bool n_RemoveAll(IntPtr jnienv, IntPtr native__this)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            return __this.RemoveAll();
        }
#pragma warning restore 0169

        static IntPtr id_removeAll;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='removeAll' and count(parameter)=0]"
        [Register("removeAll", "()Z", "GetRemoveAllHandler")]
        public virtual bool RemoveAll()
        {
            if (id_removeAll == IntPtr.Zero)
                id_removeAll = JNIEnv.GetMethodID(class_ref, "removeAll", "()Z");

            if (GetType() == ThresholdType)
                return JNIEnv.CallBooleanMethod(Handle, id_removeAll);
            else
                return JNIEnv.CallNonvirtualBooleanMethod(Handle, ThresholdClass, id_removeAll);
        }

        static Delegate cb_removeItem_Lcom_baidu_mapapi_map_OverlayItem_;
#pragma warning disable 0169
        static Delegate GetRemoveItem_Lcom_baidu_mapapi_map_OverlayItem_Handler()
        {
            if (cb_removeItem_Lcom_baidu_mapapi_map_OverlayItem_ == null)
                cb_removeItem_Lcom_baidu_mapapi_map_OverlayItem_ = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr, bool>)n_RemoveItem_Lcom_baidu_mapapi_map_OverlayItem_);
            return cb_removeItem_Lcom_baidu_mapapi_map_OverlayItem_;
        }

        static bool n_RemoveItem_Lcom_baidu_mapapi_map_OverlayItem_(IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            global::Com.Baidu.Mapapi.Map.OverlayItem p0 = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.OverlayItem>(native_p0, JniHandleOwnership.DoNotTransfer);
            bool __ret = __this.RemoveItem(p0);
            return __ret;
        }
#pragma warning restore 0169

        static IntPtr id_removeItem_Lcom_baidu_mapapi_map_OverlayItem_;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='removeItem' and count(parameter)=1 and parameter[1][@type='com.baidu.mapapi.map.OverlayItem']]"
        [Register("removeItem", "(Lcom/baidu/mapapi/map/OverlayItem;)Z", "GetRemoveItem_Lcom_baidu_mapapi_map_OverlayItem_Handler")]
        public virtual bool RemoveItem(global::Com.Baidu.Mapapi.Map.OverlayItem p0)
        {
            if (id_removeItem_Lcom_baidu_mapapi_map_OverlayItem_ == IntPtr.Zero)
                id_removeItem_Lcom_baidu_mapapi_map_OverlayItem_ = JNIEnv.GetMethodID(class_ref, "removeItem", "(Lcom/baidu/mapapi/map/OverlayItem;)Z");

            bool __ret;
            if (GetType() == ThresholdType)
                __ret = JNIEnv.CallBooleanMethod(Handle, id_removeItem_Lcom_baidu_mapapi_map_OverlayItem_, new JValue(p0));
            else
                __ret = JNIEnv.CallNonvirtualBooleanMethod(Handle, ThresholdClass, id_removeItem_Lcom_baidu_mapapi_map_OverlayItem_, new JValue(p0));
            return __ret;
        }

        static Delegate cb_size;
#pragma warning disable 0169
        static Delegate GetSizeHandler()
        {
            if (cb_size == null)
                cb_size = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, int>)n_Size);
            return cb_size;
        }

        static int n_Size(IntPtr jnienv, IntPtr native__this)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            return __this.Size();
        }
#pragma warning restore 0169

        static IntPtr id_size;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='size' and count(parameter)=0]"
        [Register("size", "()I", "GetSizeHandler")]
        public virtual int Size()
        {
            if (id_size == IntPtr.Zero)
                id_size = JNIEnv.GetMethodID(class_ref, "size", "()I");

            if (GetType() == ThresholdType)
                return JNIEnv.CallIntMethod(Handle, id_size);
            else
                return JNIEnv.CallNonvirtualIntMethod(Handle, ThresholdClass, id_size);
        }

        static Delegate cb_updateItem_Lcom_baidu_mapapi_map_OverlayItem_;
#pragma warning disable 0169
        static Delegate GetUpdateItem_Lcom_baidu_mapapi_map_OverlayItem_Handler()
        {
            if (cb_updateItem_Lcom_baidu_mapapi_map_OverlayItem_ == null)
                cb_updateItem_Lcom_baidu_mapapi_map_OverlayItem_ = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr, bool>)n_UpdateItem_Lcom_baidu_mapapi_map_OverlayItem_);
            return cb_updateItem_Lcom_baidu_mapapi_map_OverlayItem_;
        }

        static bool n_UpdateItem_Lcom_baidu_mapapi_map_OverlayItem_(IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            global::Com.Baidu.Mapapi.Map.OverlayItem p0 = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.OverlayItem>(native_p0, JniHandleOwnership.DoNotTransfer);
            bool __ret = __this.UpdateItem(p0);
            return __ret;
        }
#pragma warning restore 0169

        static IntPtr id_updateItem_Lcom_baidu_mapapi_map_OverlayItem_;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='updateItem' and count(parameter)=1 and parameter[1][@type='com.baidu.mapapi.map.OverlayItem']]"
        [Register("updateItem", "(Lcom/baidu/mapapi/map/OverlayItem;)Z", "GetUpdateItem_Lcom_baidu_mapapi_map_OverlayItem_Handler")]
        public virtual bool UpdateItem(global::Com.Baidu.Mapapi.Map.OverlayItem p0)
        {
            if (id_updateItem_Lcom_baidu_mapapi_map_OverlayItem_ == IntPtr.Zero)
                id_updateItem_Lcom_baidu_mapapi_map_OverlayItem_ = JNIEnv.GetMethodID(class_ref, "updateItem", "(Lcom/baidu/mapapi/map/OverlayItem;)Z");

            bool __ret;
            if (GetType() == ThresholdType)
                __ret = JNIEnv.CallBooleanMethod(Handle, id_updateItem_Lcom_baidu_mapapi_map_OverlayItem_, new JValue(p0));
            else
                __ret = JNIEnv.CallNonvirtualBooleanMethod(Handle, ThresholdClass, id_updateItem_Lcom_baidu_mapapi_map_OverlayItem_, new JValue(p0));
            return __ret;
        }

        static Delegate cb_Compare_Ljava_lang_Object_Ljava_lang_Object_;
#pragma warning disable 0169
        static Delegate GetCompare_Ljava_lang_Object_Ljava_lang_Object_Handler()
        {
            if (cb_Compare_Ljava_lang_Object_Ljava_lang_Object_ == null)
                cb_Compare_Ljava_lang_Object_Ljava_lang_Object_ = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr, IntPtr, int>)n_Compare_Ljava_lang_Object_Ljava_lang_Object_);
            return cb_Compare_Ljava_lang_Object_Ljava_lang_Object_;
        }

        static int n_Compare_Ljava_lang_Object_Ljava_lang_Object_(IntPtr jnienv, IntPtr native__this, IntPtr native_p0, IntPtr native_p1)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            global::Java.Lang.Object p0 = global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(native_p0, JniHandleOwnership.DoNotTransfer);
            global::Java.Lang.Object p1 = global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(native_p1, JniHandleOwnership.DoNotTransfer);
            int __ret = __this.Compare(p0, p1);
            return __ret;
        }
#pragma warning restore 0169

        static IntPtr id_Compare_Ljava_lang_Object_Ljava_lang_Object_;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='Compare' and count(parameter)=2 and parameter[1][@type='java.lang.Object'] and parameter[2][@type='java.lang.Object']]"
        [Register("Compare", "(Ljava/lang/Object;Ljava/lang/Object;)I", "GetCompare_Ljava_lang_Object_Ljava_lang_Object_Handler")]
        public virtual int Compare(global::Java.Lang.Object p0, global::Java.Lang.Object p1)
        {
            if (id_Compare_Ljava_lang_Object_Ljava_lang_Object_ == IntPtr.Zero)
                id_Compare_Ljava_lang_Object_Ljava_lang_Object_ = JNIEnv.GetMethodID(class_ref, "Compare", "(Ljava/lang/Object;Ljava/lang/Object;)I");

            int __ret;
            if (GetType() == ThresholdType)
                __ret = JNIEnv.CallIntMethod(Handle, id_Compare_Ljava_lang_Object_Ljava_lang_Object_, new JValue(p0), new JValue(p1));
            else
                __ret = JNIEnv.CallNonvirtualIntMethod(Handle, ThresholdClass, id_Compare_Ljava_lang_Object_Ljava_lang_Object_, new JValue(p0), new JValue(p1));
            return __ret;
        }

        static Delegate cb_Equals_Ljava_lang_Object_;
#pragma warning disable 0169
        static Delegate GetEquals_Ljava_lang_Object_Handler()
        {
            if (cb_Equals_Ljava_lang_Object_ == null)
                cb_Equals_Ljava_lang_Object_ = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr, bool>)n_Equals_Ljava_lang_Object_);
            return cb_Equals_Ljava_lang_Object_;
        }

        static bool n_Equals_Ljava_lang_Object_(IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
        {
            global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T> __this = global::Java.Lang.Object.GetObject<global::Com.Baidu.Mapapi.Map.ItemizedOverlay<T>>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            global::Java.Lang.Object p0 = global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(native_p0, JniHandleOwnership.DoNotTransfer);
            bool __ret = __this.Equals(p0);
            return __ret;
        }
#pragma warning restore 0169

        static IntPtr id_Equals_Ljava_lang_Object_;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.baidu.mapapi.map']/class[@name='ItemizedOverlay']/method[@name='Equals' and count(parameter)=1 and parameter[1][@type='java.lang.Object']]"
        [Register("Equals", "(Ljava/lang/Object;)Z", "GetEquals_Ljava_lang_Object_Handler")]
        public override bool Equals(global::Java.Lang.Object p0)
        {
            if (id_Equals_Ljava_lang_Object_ == IntPtr.Zero)
                id_Equals_Ljava_lang_Object_ = JNIEnv.GetMethodID(class_ref, "Equals", "(Ljava/lang/Object;)Z");

            bool __ret;
            if (GetType() == ThresholdType)
                __ret = JNIEnv.CallBooleanMethod(Handle, id_Equals_Ljava_lang_Object_, new JValue(p0));
            else
                __ret = JNIEnv.CallNonvirtualBooleanMethod(Handle, ThresholdClass, id_Equals_Ljava_lang_Object_, new JValue(p0));
            return __ret;
        }
    }
}