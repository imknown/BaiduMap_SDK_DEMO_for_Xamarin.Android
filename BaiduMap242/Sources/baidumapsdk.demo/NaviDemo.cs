
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Content;
using Android.Views;
using Android.Widget;
using baidumapsdk.demo;
using Com.Baidu.Mapapi;
using Com.Baidu.Mapapi.Cloud;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Search;
using Com.Baidu.Platform.Comapi.Basestruct;
using Java.Lang;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Content.Res;
using Android.Support.V4.App;
using Android.App;
using Com.Baidu.Mapapi.Navi;

namespace baidumapsdk.demo
{
    /**
     * 在一个Activity中展示多个地图
     */
    [Activity(Label = "@string/demo_name_navi")]
    public class NaviDemo : Activity
    {
        //天安门坐标
        double mLat1 = 39.915291;
        double mLon1 = 116.403857;
        //百度大厦坐标
        double mLat2 = 40.056858;
        double mLon2 = 116.308194;

        class IOnClickPositiveButtonListenerImpl : Java.Lang.Object, IDialogInterfaceOnClickListener
        {

            NaviDemo naviDemo;

            public IOnClickPositiveButtonListenerImpl(NaviDemo naviDemo) { this.naviDemo = naviDemo; }
            public void OnClick(IDialogInterface dialog, int which)
            {
                dialog.Dismiss();
                BaiduMapNavigation.GetLatestBaiduMapApp(naviDemo);
            }
        }

        class IOnClickNegativeButtonListenerImpl : Java.Lang.Object, IDialogInterfaceOnClickListener
        {

            NaviDemo naviDemo;

            public IOnClickNegativeButtonListenerImpl(NaviDemo naviDemo) { this.naviDemo = naviDemo; }
            public void OnClick(IDialogInterface dialog, int which)
            {
                dialog.Dismiss();

            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_navi_demo);
            TextView text = (TextView)FindViewById(Resource.Id.navi_info);
            text.Text = String.Format("起点:(%f,%f)\n终点:(%f,%f)", mLat1, mLon1, mLat2, mLon2);
        }

        /**
    * 开始导航		
    * @param view
    */
        [Java.Interop.Export]
        public void StartNavi(View view)
        {
            int lat = (int)(mLat1 * 1E6);
            int lon = (int)(mLon1 * 1E6);
            GeoPoint pt1 = new GeoPoint(lat, lon);
            lat = (int)(mLat2 * 1E6);
            lon = (int)(mLon2 * 1E6);
            GeoPoint pt2 = new GeoPoint(lat, lon);
            // 构建 导航参数
            NaviPara para = new NaviPara();
            para.StartPoint = pt1;
            para.StartName = "从这里开始";
            para.EndPoint = pt2;
            para.EndName = "到这里结束";

            try
            {
                BaiduMapNavigation.OpenBaiduMapNavi(para, this);
            }
            catch (BaiduMapAppNotSupportNaviException e)
            {
                e.PrintStackTrace();

                // 居然不走原版的这个, 可能是IKVM的bug, 也可能是我能力不够, 哎, 求大神指路
                // 用下面的 RuntimeException 是不对的, 虽然照顾到了
                // Com.Baidu.Mapapi.Navi.BaiduMapAppNotSupportNaviException
                // 但是 OpenBaiduMapNavi 方法还可能会抛一个
                // Com.Baidu.Mapapi.Navi.IllegalNaviArgumentException
                // 这样就没法区分了
            }
            catch (RuntimeException e)
            {
                e.PrintStackTrace();
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetMessage("您尚未安装百度地图app或app版本过低，点击确认安装？");
                builder.SetTitle("提示");
                builder.SetPositiveButton("确认", new IOnClickPositiveButtonListenerImpl(this));

                builder.SetNegativeButton("取消", new IOnClickNegativeButtonListenerImpl(this));

                builder.Create().Show();
            }
        }

        [Java.Interop.Export]
        public void StartWebNavi(View view)
        {
            int lat = (int)(mLat1 * 1E6);
            int lon = (int)(mLon1 * 1E6);
            GeoPoint pt1 = new GeoPoint(lat, lon);
            lat = (int)(mLat2 * 1E6);
            lon = (int)(mLon2 * 1E6);
            GeoPoint pt2 = new GeoPoint(lat, lon);
            // 构建 导航参数
            NaviPara para = new NaviPara();
            para.StartPoint = pt1;
            para.EndPoint = pt2;
            BaiduMapNavigation.OpenWebBaiduMapNavi(para, this);
        }
    }
}