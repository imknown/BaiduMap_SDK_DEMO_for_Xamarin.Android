using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi;
using Com.Baidu.Mapapi.Map;

namespace baidumapsdk.demo
{
    [Application]
    public class DemoApplication : Application
    {
        private static DemoApplication mInstance = null;
        public bool m_bKeyRight = true;
        internal BMapManager mBMapManager = null;

        public DemoApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            mInstance = this;

            initEngineManager(this);
        }

        public void initEngineManager(Context context)
        {
            if (mBMapManager == null)
            {
                mBMapManager = new BMapManager(context);
            }

            if (!mBMapManager.Init(new MyGeneralListener()))
            {
                Toast.MakeText(DemoApplication.getInstance().ApplicationContext, "BMapManager 初始化错误!", ToastLength.Short).Show();
            }
        }

        public static DemoApplication getInstance()
        {
            return mInstance;
        }

        // 常用事件监听，用来处理通常的网络错误，授权验证错误等
        public class MyGeneralListener : Java.Lang.Object, IMKGeneralListener
        {
            public void OnGetNetworkState(int iError)
            {
                if (iError == MKEvent.ErrorNetworkConnect)
                {
                    Toast.MakeText(DemoApplication.getInstance().ApplicationContext, "您的网络出错啦！", ToastLength.Short).Show();
                }
                else if (iError == MKEvent.ErrorNetworkData)
                {
                    Toast.MakeText(DemoApplication.getInstance().ApplicationContext, "输入正确的检索条件！", ToastLength.Short).Show();
                }
                // ...
            }

            public void OnGetPermissionState(int iError)
            {
                //非零值表示key验证未通过
                if (iError != 0)
                {
                    //授权Key错误：
                    Toast.MakeText(DemoApplication.getInstance().ApplicationContext, "请在 DemoApplication.java文件输入正确的授权Key,并检查您的网络连接是否正常！error: " + iError, ToastLength.Short).Show();
                    DemoApplication.getInstance().m_bKeyRight = false;
                }
                else
                {
                    DemoApplication.getInstance().m_bKeyRight = true;
                    Toast.MakeText(DemoApplication.getInstance().ApplicationContext, "key认证成功", ToastLength.Short).Show();
                }
            }
        }
    }
}