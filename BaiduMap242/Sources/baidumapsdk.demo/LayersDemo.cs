using Android.App;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
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

namespace baidumapsdk.demo
{
    /**
     * 演示地图图层显示的控制方法
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_layers", ScreenOrientation = ScreenOrientation.Sensor)]
    public class LayersDemo : Activity
    {
        /**
	     *  MapView 是地图主控件
	     */
        private MapView mMapView = null;
        /**
         *  用MapController完成地图控制 
         */
        private MapController mMapController = null;

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
            SetContentView(Resource.Layout.activity_layers);

            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            /**
             * 获取地图控制器
             */
            mMapController = mMapView.Controller;
            /**
             *  设置地图是否响应点击事件  .
             */
            mMapController.EnableClick(true);
            /**
             * 设置地图缩放级别
             */
            mMapController.SetZoom(12);
            /**
             * 显示内置缩放控件
             */
            mMapView.SetBuiltInZoomControls(true);

            /**
             * 将地图移动至天安门
             * 使用百度经纬度坐标，可以通过http://api.map.baidu.com/lbsapi/getpoint/index.html查询地理坐标
             * 如果需要在百度地图上显示使用其他坐标系统的位置，请发邮件至mapapi@baidu.com申请坐标转换接口
             */
            double cLat = 39.945;
            double cLon = 116.404;
            GeoPoint p = new GeoPoint((int)(cLat * 1E6), (int)(cLon * 1E6));
            mMapController.SetCenter(p);
        }

        /**
         * 设置底图显示模式
         * @param view
         */
        [Java.Interop.Export]
        public void SetMapMode(View view)
        {
            bool checkedX = ((RadioButton)view).Checked;
            switch (view.Id)
            {
                case Resource.Id.normal:
                    if (checkedX)
                        mMapView.Satellite = false;
                    break;
                case Resource.Id.statellite:
                    if (checkedX)
                        mMapView.Satellite = true;
                    break;
            }
        }
        /**
         * 设置是否显示交通图
         * @param view
         */
        [Java.Interop.Export]
        public void SetTraffic(View view)
        {
            mMapView.Traffic = ((CheckBox)view).Checked;
        }

        protected override void OnPause()
        {
            /**
             *  MapView的生命周期与Activity同步，当activity挂起时需调用MapView.OnPause()
             */
            mMapView.OnPause();
            base.OnPause();
        }

        protected override void OnDestroy()
        {
            /**
             *  MapView的生命周期与Activity同步，当activity销毁时需调用MapView.Destroy()
             */
            mMapView.Destroy();
            base.OnDestroy();
        }

        protected override void OnResume()
        {
            /**
             *  MapView的生命周期与Activity同步，当activity恢复时需调用MapView.OnResume()
             */
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