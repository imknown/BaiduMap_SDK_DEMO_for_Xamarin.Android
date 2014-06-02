using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using baidumapsdk.demo;
using Com.Baidu.Mapapi;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Platform.Comapi.Basestruct;

namespace baidumapsdk.demo
{
    /**
     * 演示MapView的基本用法
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_basemap", ScreenOrientation = ScreenOrientation.Sensor)]
    public class BaseMapDemo : Activity
    {
        // readonly static string TAG = "MainActivity";

        /**
         *  MapView 是地图主控件
         */
        private MapView mMapView = null;

        /**
         *  用MapController完成地图控制 
         */
        private MapController mMapController = null;

        /**
         *  MKMapViewListener 用于处理地图事件回调
         */
        MKMapViewListenerImpl mMapListener = null;

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
            SetContentView(Resource.Layout.activity_main);

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
             * 将地图移动至指定点
             * 使用百度经纬度坐标，可以通过http://api.map.baidu.com/lbsapi/getpoint/index.html查询地理坐标
             * 如果需要在百度地图上显示使用其他坐标系统的位置，请发邮件至mapapi@baidu.com申请坐标转换接口
             */
            GeoPoint p;
            double cLat = 39.945;
            double cLon = 116.404;

            var intent = Intent;

            if (intent.HasExtra("x") && intent.HasExtra("y"))
            {
                //当用intent参数时，设置中心点为指定点
                Bundle b = intent.Extras;

                p = new GeoPoint(b.GetInt("y"), b.GetInt("x"));
            }
            else
            {
                //设置中心点为天安门
                p = new GeoPoint((int)(cLat * 1E6), (int)(cLon * 1E6));
            }

            mMapController.SetCenter(p);

            /**
             *  MapView的生命周期与Activity同步，当activity挂起时需调用MapView.onPause()
             */
            mMapListener = new MKMapViewListenerImpl(this);

            mMapView.RegMapViewListener(DemoApplication.getInstance().mBMapManager, mMapListener);
        }

        private class MKMapViewListenerImpl : Java.Lang.Object, IMKMapViewListener
        {
            private BaseMapDemo baseMapDemo;

            public MKMapViewListenerImpl(BaseMapDemo baseMapDemo)
            {
                this.baseMapDemo = baseMapDemo;
            }

            public void OnMapMoveFinish()
            {
                /**
                 * 在此处理地图移动完成回调
                 * 缩放，平移等操作完成后，此回调被触发
                 */
            }

            public void OnClickMapPoi(MapPoi mapPoiInfo)
            {
                /**
                 * 在此处理底图poi点击事件
                 * 显示底图poi名称并移动至该点
                 * 设置过： mMapController.enableClick(true); 时，此回调才能被触发
                 * 
                 */
                string title = "";

                if (mapPoiInfo != null)
                {
                    title = mapPoiInfo.StrText;
                    Toast.MakeText(baseMapDemo, title, ToastLength.Short).Show();
                    baseMapDemo.mMapController.AnimateTo(mapPoiInfo.GeoPt);
                }
            }

            public void OnGetCurrentMap(Bitmap b)
            {
                /**
                 *  当调用过 mMapView.getCurrentMap()后，此回调会被触发
                 *  可在此保存截图至存储设备
                 */
            }

            public void OnMapAnimationFinish()
            {
                /**
                 *  地图完成带动画的操作（如: animationTo()）后，此回调被触发
                 */
            }

            /**
             * 在此处理地图载完成事件 
             */
            public void OnMapLoadFinish()
            {
                Toast.MakeText(baseMapDemo, "地图加载完成", ToastLength.Short).Show();
            }
        }

        protected override void OnPause()
        {
            /**
             *  MapView的生命周期与Activity同步，当activity挂起时需调用MapView.onPause()
             */
            mMapView.OnPause();
            base.OnPause();
        }

        protected override void OnResume()
        {
            /**
             *  MapView的生命周期与Activity同步，当activity恢复时需调用MapView.onResume()
             */
            mMapView.OnResume();
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            /**
             *  MapView的生命周期与Activity同步，当activity销毁时需调用MapView.destroy()
             */
            mMapView.Destroy();
            base.OnDestroy();
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