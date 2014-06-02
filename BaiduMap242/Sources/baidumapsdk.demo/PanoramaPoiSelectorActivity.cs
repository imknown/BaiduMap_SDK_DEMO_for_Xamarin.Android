
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
using Android.Support.V4.App;
using Android.Content.Res;
using Android.App;
using Com.Baidu.Mapapi.PanoramaX;
using Android.Graphics;
using Android.Runtime;

namespace baidumapsdk.demo
{
    [Activity(Label = "@string/title_activity_panorama_poi_selector")]
    public class PanoramaPoiSelectorActivity : FragmentActivity
    {
        MKSearch mSearch = null;
        MapView mMapView = null;

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
            /**
             * 由于MapView在setContentView()中初始化,所以它需要在BMapManager初始化之后
             */
            SetContentView(Resource.Layout.activity_panorama_poi_selector);
            InitMap();
            InitSearcher();
        }

        private void InitMap()
        {
            mMapView = (Android.Runtime.Extensions.JavaCast<SupportMapFragment>(SupportFragmentManager
                    .FindFragmentById(Resource.Id.map)).MapView);
            GeoPoint p = new GeoPoint((int)(39.945 * 1E6), (int)(116.404 * 1E6));
            mMapView.Controller.SetCenter(p);
            mMapView.Controller.SetZoom(13);
        }

        class IMKSearchListenerImpl : Java.Lang.Object, IMKSearchListener
        {
            PanoramaPoiSelectorActivity panoramaPoiSelectorActivity;
            public IMKSearchListenerImpl(PanoramaPoiSelectorActivity panoramaPoiSelectorActivity)
            {
                this.panoramaPoiSelectorActivity = panoramaPoiSelectorActivity;
            }

            public void OnGetPoiResult(MKPoiResult res, int type, int iError)
            {
                if (iError != 0)
                {
                    Toast.MakeText(panoramaPoiSelectorActivity,
                            "抱歉，未能找到结果", ToastLength.Short).Show();
                    return;
                }
                if (res.CurrentNumPois > 0)
                {
                    // 将poi结果显示到地图上
                    SelectPoiOverlay poiOverlay = new SelectPoiOverlay(panoramaPoiSelectorActivity, panoramaPoiSelectorActivity.mMapView);
                    poiOverlay.SetData(res.AllPoi);
                    panoramaPoiSelectorActivity.mMapView.Overlays.Clear();
                    panoramaPoiSelectorActivity.mMapView.Overlays.Add(poiOverlay);
                    panoramaPoiSelectorActivity.mMapView.Refresh();
                    //当ePoiType为2（公交线路）或4（地铁线路）时， poi坐标为空
                    foreach (MKPoiInfo info in res.AllPoi)
                    {
                        if (info.Pt != null)
                        {
                            panoramaPoiSelectorActivity.mMapView.Controller.AnimateTo(info.Pt);
                            break;
                        }
                    }
                }

            }


            public void OnGetTransitRouteResult(MKTransitRouteResult result, int iError)
            {
            }


            public void OnGetDrivingRouteResult(MKDrivingRouteResult result, int iError)
            {
            }


            public void OnGetWalkingRouteResult(MKWalkingRouteResult result, int iError)
            {
            }


            public void OnGetAddrResult(MKAddrInfo result, int iError)
            {
            }


            public void OnGetBusDetailResult(MKBusLineResult result, int iError)
            {
            }


            public void OnGetSuggestionResult(MKSuggestionResult result, int iError)
            {
            }


            public void OnGetPoiDetailSearchResult(int type, int iError)
            {
            }


            public void OnGetShareUrlResult(MKShareUrlResult result, int type, int error)
            {

            }
        }

        private void InitSearcher()
        {
            mSearch = new MKSearch();
            mSearch.Init(((DemoApplication)this.Application).mBMapManager, new IMKSearchListenerImpl(this));
        }

        [Java.Interop.Export]
        public void DoPoiSearch(View v)
        {
            mSearch.PoiSearchInCity((FindViewById<EditText>(Resource.Id.city)).Text,
                    (FindViewById<EditText>(Resource.Id.key)).Text);
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (mSearch != null)
            {
                mSearch.Destory();
                mSearch = null;
            }
        }

        private class SelectPoiOverlay : PoiOverlay
        {
            Activity activity;

            public SelectPoiOverlay(Activity activity, MapView mapView) :
                base(activity, mapView)
            {
                this.activity = activity;
            }


            protected override bool OnTap(int i)
            {
                base.OnTap(i);
                MKPoiInfo info = GetPoi(i);
                if (!info.IsPano)
                {
                    Toast.MakeText(activity,
                            "当前POI当不包含全景信息", ToastLength.Short).Show();
                }
                else
                {
                    Intent intent = new Intent();
                    intent.SetClass(activity,
                            typeof(PanoramaDemoActivityMain));
                    intent.PutExtra("uid", info.Uid);
                    activity.StartActivity(intent);
                }
                return true;
            }
        }
    }
}