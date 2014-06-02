
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
    [Activity(Label = "@string/title_activity_panorama_geo_selector")]
    public class PanoramaGeoSelectorActivity : FragmentActivity
    {

        private MapView mMapView = null;
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
            SetContentView(Resource.Layout.activity_panorama_geo_selector);
            InitMap();
        }

        private void InitMap()
        {
            mMapView = (Android.Runtime.Extensions.JavaCast<SupportMapFragment>(SupportFragmentManager
                    .FindFragmentById(Resource.Id.map)).MapView);
            GeoPoint p = new GeoPoint((int)(39.945 * 1E6), (int)(116.404 * 1E6));
            mMapView.Controller.SetCenter(p);
            mMapView.Controller.SetZoom(13);
            mMapView.RegMapTouchListner(new IMKMapTouchListenerImpl(this));
        }

        class IMKMapTouchListenerImpl : Java.Lang.Object, IMKMapTouchListener
        {
            PanoramaGeoSelectorActivity panoramaGeoSelectorActivity;

            public IMKMapTouchListenerImpl(PanoramaGeoSelectorActivity panoramaGeoSelectorActivity)
            {
                this.panoramaGeoSelectorActivity = panoramaGeoSelectorActivity;
            }

            public void OnMapClick(GeoPoint point)
            {
                panoramaGeoSelectorActivity.UpdateUI(point);
            }

            public void OnMapDoubleClick(GeoPoint point)
            {

            }

            public void OnMapLongClick(GeoPoint point)
            {

            }
        }

        [Java.Interop.Export]
        public void StartPanorama(View v)
        {
            float lat = Float.ParseFloat((FindViewById<EditText>(Resource.Id.lat)).Text);
            float lon = Float.ParseFloat((FindViewById<EditText>(Resource.Id.lon)).Text);
            Intent intent = Intent;
            intent.SetClass(this,
                    typeof(PanoramaDemoActivityMain));
            intent.PutExtra("lon", (int)(lon * 1E6));
            intent.PutExtra("lat", (int)(lat * 1E6));
            StartActivity(intent);
        }

        private void UpdateUI(GeoPoint p)
        {
            (FindViewById<EditText>(Resource.Id.lat)).Text = String.Format("{0:3F}", p.LatitudeE6 * 1E-6);
            (FindViewById<EditText>(Resource.Id.lon)).Text = String.Format("{0:3F}", p.LongitudeE6 * 1E-6);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}