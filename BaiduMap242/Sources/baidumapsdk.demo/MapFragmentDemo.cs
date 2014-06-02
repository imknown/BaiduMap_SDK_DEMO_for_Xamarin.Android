
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

namespace baidumapsdk.demo
{
    [Android.App.Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_map_fragment", ScreenOrientation = ScreenOrientation.Sensor)]
    public class MapFragmentDemo : FragmentActivity
    {
        private static readonly string LTAG = typeof(MapFragmentDemo).Name;
        SupportMapFragment map;

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

            Log.Debug(LTAG, "OnCreate");
            SetContentView(Resource.Layout.activity_fragment);
            map = SupportMapFragment.NewInstance();
            FragmentManager manager = SupportFragmentManager;
            manager.BeginTransaction().Add(Resource.Id.map, map, "map_fragment").Commit();
        }


        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            Log.Debug(LTAG, "onRestoreInstanceState");
        }


        protected override void OnRestart()
        {
            base.OnRestart();
            Log.Debug(LTAG, "onRestart");
        }


        protected override void OnStart()
        {
            base.OnStart();
            Log.Debug(LTAG, "onStart");
        }


        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug(LTAG, "onResume");
            MapController controller = map.MapView.Controller;
            controller.SetCenter(new GeoPoint((int)(39.945 * 1E6), (int)(116.404 * 1E6)));
            controller.SetZoom(13);
        }


        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            Log.Debug(LTAG, "onSaveInstanceState");
        }


        protected override void OnPause()
        {
            base.OnPause();
            Log.Debug(LTAG, "onPause");
        }


        protected override void OnStop()
        {
            base.OnStop();
            Log.Debug(LTAG, "onStop");
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug(LTAG, "onDestory");
        }


        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            Log.Debug(LTAG, "onConfigurationChanged");
        }
    }
}