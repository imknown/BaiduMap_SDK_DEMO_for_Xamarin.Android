using Android.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Com.Baidu.Mapapi.Search
{
    public partial class MKRouteAddrResult : Java.Lang.Object
    {
        //public ArrayList<MKPoiInfo> mStartPoiList;
        static IntPtr mStartPoiList_jfieldId;

        // Metadata.xml XPath field reference: path="/api/package[@name='com.baidu.mapapi.search']/class[@name='MKRouteAddrResult']/field[@name='mStartPoiList']"
        [Register("mStartPoiList")]
        public global::System.Collections.Generic.IList<MKPoiInfo> MStartPoiList
        {
            get
            {
                if (mStartPoiList_jfieldId == IntPtr.Zero)
                    mStartPoiList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "mStartPoiList", "Ljava/util/ArrayList;");
                IntPtr __ret = JNIEnv.GetObjectField(Handle, mStartPoiList_jfieldId);
                return global::Android.Runtime.JavaList<MKPoiInfo>.FromJniHandle(__ret, JniHandleOwnership.TransferLocalRef);
            }
            set
            {
                if (mStartPoiList_jfieldId == IntPtr.Zero)
                    mStartPoiList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "mStartPoiList", "Ljava/util/ArrayList;");
                IntPtr native_value = global::Android.Runtime.JavaList<MKPoiInfo>.ToLocalJniHandle(value);
                JNIEnv.SetField(Handle, mStartPoiList_jfieldId, native_value);
                JNIEnv.DeleteLocalRef(native_value);
            }
        }




        //public ArrayList<MKPoiInfo> mEndPoiList;
        static IntPtr mEndPoiList_jfieldId;

        // Metadata.xml XPath field reference: path="/api/package[@name='com.baidu.mapapi.search']/class[@name='MKRouteAddrResult']/field[@name='mEndPoiList']"
        [Register("mEndPoiList")]
        public global::System.Collections.Generic.IList<MKPoiInfo> MEndPoiList
        {
            get
            {
                if (mEndPoiList_jfieldId == IntPtr.Zero)
                    mEndPoiList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "mEndPoiList", "Ljava/util/ArrayList;");
                IntPtr __ret = JNIEnv.GetObjectField(Handle, mEndPoiList_jfieldId);
                return global::Android.Runtime.JavaList<MKPoiInfo>.FromJniHandle(__ret, JniHandleOwnership.TransferLocalRef);
            }
            set
            {
                if (mEndPoiList_jfieldId == IntPtr.Zero)
                    mEndPoiList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "mEndPoiList", "Ljava/util/ArrayList;");
                IntPtr native_value = global::Android.Runtime.JavaList<MKPoiInfo>.ToLocalJniHandle(value);
                JNIEnv.SetField(Handle, mEndPoiList_jfieldId, native_value);
                JNIEnv.DeleteLocalRef(native_value);
            }
        }




        //public ArrayList<ArrayList<MKPoiInfo>> mWpPoiList;
        static IntPtr mWpPoiList_jfieldId;

        // Metadata.xml XPath field reference: path="/api/package[@name='com.baidu.mapapi.search']/class[@name='MKRouteAddrResult']/field[@name='mWpPoiList']"
        [Register("mWpPoiList")]
        public global::System.Collections.Generic.IList<global::System.Collections.Generic.IList<MKPoiInfo>> MWpPoiList
        {
            get
            {
                if (mWpPoiList_jfieldId == IntPtr.Zero)
                    mWpPoiList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "mWpPoiList", "Ljava/util/ArrayList;");
                IntPtr __ret = JNIEnv.GetObjectField(Handle, mWpPoiList_jfieldId);
                return global::Android.Runtime.JavaList<global::System.Collections.Generic.IList<MKPoiInfo>>.FromJniHandle(__ret, JniHandleOwnership.TransferLocalRef);
            }
            set
            {
                if (mWpPoiList_jfieldId == IntPtr.Zero)
                    mWpPoiList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "mWpPoiList", "Ljava/util/ArrayList;");
                IntPtr native_value = global::Android.Runtime.JavaList<global::System.Collections.Generic.IList<MKPoiInfo>>.ToLocalJniHandle(value);
                JNIEnv.SetField(Handle, mWpPoiList_jfieldId, native_value);
                JNIEnv.DeleteLocalRef(native_value);
            }
        }




        //public ArrayList<MKCityListInfo> mStartCityList;
        static IntPtr mStartCityList_jfieldId;

        // Metadata.xml XPath field reference: path="/api/package[@name='com.baidu.mapapi.search']/class[@name='MKRouteAddrResult']/field[@name='mStartCityList']"
        [Register("mStartCityList")]
        public global::System.Collections.Generic.IList<MKCityListInfo> MStartCityList
        {
            get
            {
                if (mStartCityList_jfieldId == IntPtr.Zero)
                    mStartCityList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "mStartCityList", "Ljava/util/ArrayList;");
                IntPtr __ret = JNIEnv.GetObjectField(Handle, mStartCityList_jfieldId);
                return global::Android.Runtime.JavaList<MKCityListInfo>.FromJniHandle(__ret, JniHandleOwnership.TransferLocalRef);
            }
            set
            {
                if (mStartCityList_jfieldId == IntPtr.Zero)
                    mStartCityList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "mStartCityList", "Ljava/util/ArrayList;");
                IntPtr native_value = global::Android.Runtime.JavaList<MKCityListInfo>.ToLocalJniHandle(value);
                JNIEnv.SetField(Handle, mStartCityList_jfieldId, native_value);
                JNIEnv.DeleteLocalRef(native_value);
            }
        }




        //public ArrayList<MKCityListInfo> mEndCityList;   
        static IntPtr mEndCityList_jfieldId;

        // Metadata.xml XPath field reference: path="/api/package[@name='com.baidu.mapapi.search']/class[@name='MKRouteAddrResult']/field[@name='mEndCityList']"
        [Register("mEndCityList")]
        public global::System.Collections.Generic.IList<MKCityListInfo> MEndCityList
        {
            get
            {
                if (mEndCityList_jfieldId == IntPtr.Zero)
                    mEndCityList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "mEndCityList", "Ljava/util/ArrayList;");
                IntPtr __ret = JNIEnv.GetObjectField(Handle, mEndCityList_jfieldId);
                return global::Android.Runtime.JavaList<MKCityListInfo>.FromJniHandle(__ret, JniHandleOwnership.TransferLocalRef);
            }
            set
            {
                if (mEndCityList_jfieldId == IntPtr.Zero)
                    mEndCityList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "mEndCityList", "Ljava/util/ArrayList;");
                IntPtr native_value = global::Android.Runtime.JavaList<MKCityListInfo>.ToLocalJniHandle(value);
                JNIEnv.SetField(Handle, mEndCityList_jfieldId, native_value);
                JNIEnv.DeleteLocalRef(native_value);
            }
        }




        //public ArrayList<ArrayList<MKCityListInfo>> mWpCityList;
        static IntPtr mWpCityList_jfieldId;

        // Metadata.xml XPath field reference: path="/api/package[@name='com.baidu.mapapi.search']/class[@name='MKRouteAddrResult']/field[@name='mWpCityList']"
        [Register("mWpCityList")]
        public global::System.Collections.Generic.IList<global::System.Collections.Generic.IList<MKCityListInfo>> MWpCityList
        {
            get
            {
                if (mWpCityList_jfieldId == IntPtr.Zero)
                    mWpCityList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "mWpCityList", "Ljava/util/ArrayList;");
                IntPtr __ret = JNIEnv.GetObjectField(Handle, mWpCityList_jfieldId);
                return global::Android.Runtime.JavaList<global::System.Collections.Generic.IList<MKCityListInfo>>.FromJniHandle(__ret, JniHandleOwnership.TransferLocalRef);
            }
            set
            {
                if (mWpCityList_jfieldId == IntPtr.Zero)
                    mWpCityList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "mWpCityList", "Ljava/util/ArrayList;");
                IntPtr native_value = global::Android.Runtime.JavaList<global::System.Collections.Generic.IList<MKCityListInfo>>.ToLocalJniHandle(value);
                JNIEnv.SetField(Handle, mWpCityList_jfieldId, native_value);
                JNIEnv.DeleteLocalRef(native_value);
            }
        }




        internal static IntPtr java_class_handle_2;
        internal static IntPtr class_ref_2
        {
            get
            {
                return JNIEnv.FindClass("com/baidu/mapapi/search/MKRouteAddrResult", ref java_class_handle_2);
            }
        }
    }

    public partial class MKPoiInfo : Java.Lang.Object
    {
    }

    public partial class MKCityListInfo : Java.Lang.Object
    {
    }
}