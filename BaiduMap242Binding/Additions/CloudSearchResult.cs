using Android.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Com.Baidu.Mapapi.Cloud
{
    public partial class CloudSearchResult : BaseSearchResult
    {
        static IntPtr poiList_jfieldId;

        // Metadata.xml XPath field reference: path="/api/package[@name='com.baidu.mapapi.cloud']/class[@name='CloudSearchResult']/field[@name='poiList']"
        [Register("poiList")]
        public global::System.Collections.Generic.IList<CloudPoiInfo> PoiList
        {
            get
            {
                if (poiList_jfieldId == IntPtr.Zero)
                    poiList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "poiList", "Ljava/util/List;");
                IntPtr __ret = JNIEnv.GetObjectField(Handle, poiList_jfieldId);
                return global::Android.Runtime.JavaList<CloudPoiInfo>.FromJniHandle(__ret, JniHandleOwnership.TransferLocalRef);
            }
            set
            {
                if (poiList_jfieldId == IntPtr.Zero)
                    poiList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "poiList", "Ljava/util/List;");
                IntPtr native_value = global::Android.Runtime.JavaList<CloudPoiInfo>.ToLocalJniHandle(value);
                JNIEnv.SetField(Handle, poiList_jfieldId, native_value);
                JNIEnv.DeleteLocalRef(native_value);
            }
        }

        internal static IntPtr java_class_handle_2;
        internal static IntPtr class_ref_2
        {
            get
            {
                return JNIEnv.FindClass("com/baidu/mapapi/cloud/CloudSearchResult", ref java_class_handle_2);
            }
        }
    }

    public abstract partial class BaseSearchResult : Java.Lang.Object
    {
    }

    public partial class CloudPoiInfo : Java.Lang.Object
    {
    }
}