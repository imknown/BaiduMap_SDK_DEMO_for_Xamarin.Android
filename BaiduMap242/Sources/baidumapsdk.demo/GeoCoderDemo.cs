using Android.App;
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

namespace baidumapsdk.demo
{
    /**
     * 此demo用来展示如何进行地理编码搜索（用地址检索坐标）、反地理编码搜索（用坐标检索地址）
     * 同时展示了如何使用ItemizedOverlay在地图上标注结果点
     *
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_geocode", ScreenOrientation = ScreenOrientation.Sensor)]
    public class GeoCoderDemo : Activity
    {
        //UI相关
        Button mBtnReverseGeoCode = null;	// 将坐标反编码为地址
        Button mBtnGeoCode = null;	// 将地址编码为坐标

        //地图相关
        MapView mMapView = null;	// 地图View
        //搜索相关
        MKSearch mSearch = null;	// 搜索模块，也可去掉地图模块独立使用

        class ClickListenerImpl : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            GeoCoderDemo geoCoderDemo;

            public ClickListenerImpl(GeoCoderDemo geoCoderDemo)
            {
                this.geoCoderDemo = geoCoderDemo;
            }

            public void OnClick(View v)
            {
                geoCoderDemo.SearchButtonProcess(v);
            }
        }

        class IMKSearchListenerImpl : Java.Lang.Object, IMKSearchListener
        {
            GeoCoderDemo geoCoderDemo;

            public IMKSearchListenerImpl(GeoCoderDemo geoCoderDemo)
            {
                this.geoCoderDemo = geoCoderDemo;
            }

            public void OnGetPoiDetailSearchResult(int type, int error)
            {
            }

            public void OnGetAddrResult(MKAddrInfo res, int error)
            {
                if (error != 0)
                {
                    string str = String.Format("错误号：%d", error);
                    Toast.MakeText(geoCoderDemo, str, ToastLength.Long).Show();
                    return;
                }
                //地图移动到该点
                geoCoderDemo.mMapView.Controller.AnimateTo(res.GeoPt);
                if (res.Type == MKAddrInfo.MkGeocode)
                {
                    //地理编码：通过地址检索坐标点
                    string strInfo = String.Format("纬度：%f 经度：%f", res.GeoPt.LatitudeE6 / 1e6, res.GeoPt.LongitudeE6 / 1e6);
                    Toast.MakeText(geoCoderDemo, strInfo, ToastLength.Long).Show();
                }
                if (res.Type == MKAddrInfo.MkReversegeocode)
                {
                    //反地理编码：通过坐标点检索详细地址及周边poi
                    string strInfo = res.StrAddr;
                    Toast.MakeText(geoCoderDemo, strInfo, ToastLength.Long).Show();
                }
                //生成ItemizedOverlay图层用来标注结果点
                ItemizedOverlay<OverlayItem> itemOverlay = new ItemizedOverlay<OverlayItem>(null, geoCoderDemo.mMapView);
                //生成Item
                OverlayItem item = new OverlayItem(res.GeoPt, "", null);
                //得到需要标在地图上的资源
                Drawable marker = geoCoderDemo.Resources.GetDrawable(Resource.Drawable.icon_markf);
                //为maker定义位置和边界
                marker.SetBounds(0, 0, marker.IntrinsicWidth, marker.IntrinsicHeight);
                //给item设置marker
                item.Marker = marker;
                //在图层上添加item
                itemOverlay.AddItem(item);

                //清除地图其他图层
                geoCoderDemo.mMapView.Overlays.Clear();
                //添加一个标注ItemizedOverlay图层
                geoCoderDemo.mMapView.Overlays.Add(itemOverlay);
                //执行刷新使生效
                geoCoderDemo.mMapView.Refresh();
            }

            public void OnGetPoiResult(MKPoiResult res, int type, int error)
            {
            }

            public void OnGetDrivingRouteResult(MKDrivingRouteResult res, int error)
            {
            }

            public void OnGetTransitRouteResult(MKTransitRouteResult res, int error)
            {
            }

            public void OnGetWalkingRouteResult(MKWalkingRouteResult res, int error)
            {
            }

            public void OnGetBusDetailResult(MKBusLineResult result, int iError)
            {
            }

            public void OnGetSuggestionResult(MKSuggestionResult res, int arg1)
            {
            }

            public void OnGetShareUrlResult(MKShareUrlResult result, int type,
                    int error)
            {
                // TODO Auto-generated method stub
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            /**
             * 使用地图sdk前需先初始化BMapManager.
             * BMapManager是全局的，可为多个MapView共用，它需要地图模块创建前创建，
             * 并在地图地图模块销毁后销毁，只要还有地图模块在使用，BMapManager就不应该销毁
             */
            DemoApplication app = (DemoApplication)this.Application;
            if (app.mBMapManager == null)
            {
                app.mBMapManager = new BMapManager(ApplicationContext);
                /**
                 * 如果BMapManager没有初始化则初始化BMapManager
                 */
                app.mBMapManager.Init(new DemoApplication.MyGeneralListener());
            }
            SetContentView(Resource.Layout.geocoder);
            ICharSequence titleLable = new String("地理编码功能");
            Title = titleLable.ToString();

            //地图初始化
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mMapView.Controller.EnableClick(true);
            mMapView.Controller.SetZoom(12);

            // 初始化搜索模块，注册事件监听
            mSearch = new MKSearch();
            mSearch.Init(app.mBMapManager, new IMKSearchListenerImpl(this));

            // 设定地理编码及反地理编码按钮的响应
            mBtnReverseGeoCode = FindViewById<Button>(Resource.Id.reversegeocode);
            mBtnGeoCode = FindViewById<Button>(Resource.Id.geocode);

            Android.Views.View.IOnClickListener clickListener = new ClickListenerImpl(this);

            mBtnReverseGeoCode.SetOnClickListener(clickListener);
            mBtnGeoCode.SetOnClickListener(clickListener);
        }

        /**
         * 发起搜索
         * @param v
         */
        void SearchButtonProcess(View v)
        {
            if (mBtnReverseGeoCode.Equals(v))
            {
                EditText lat = FindViewById<EditText>(Resource.Id.lat);
                EditText lon = FindViewById<EditText>(Resource.Id.lon);
                GeoPoint ptCenter = new GeoPoint((int)(float.Parse(lat.Text) * 1e6), (int)(float.Parse(lon.Text) * 1e6));
                //反Geo搜索
                mSearch.ReverseGeocode(ptCenter);
            }
            else if (mBtnGeoCode.Equals(v))
            {
                EditText editCity = FindViewById<EditText>(Resource.Id.city);
                EditText editGeoCodeKey = FindViewById<EditText>(Resource.Id.geocodekey);
                //Geo搜索
                mSearch.Geocode(editGeoCodeKey.Text, editCity.Text);
            }
        }

        protected override void OnDestroy()
        {
            mMapView.Destroy();
            mSearch.Destory();
            base.OnDestroy();
        }

        protected override void OnPause()
        {
            mMapView.OnPause();
            base.OnPause();
        }

        protected override void OnResume()
        {
            mMapView.OnResume();
            base.OnResume();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            mMapView.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            mMapView.OnRestoreInstanceState(savedInstanceState);
        }
    }
}