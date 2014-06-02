using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Widget;
using baidumapsdk.demo;
using Com.Baidu.Mapapi;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Platform.Comapi.Basestruct;
using Java.IO;
using Java.Lang;
using System.IO;

namespace baidumapsdk.demo
{
    /**
     * 演示地图缩放，旋转，视角控制
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_control", ScreenOrientation = ScreenOrientation.Sensor)]
    public class MapControlDemo : Activity
    {
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
        IMKMapViewListener mMapListener = null;

        /**
         * 用于截获屏坐标
         */
        IMKMapTouchListener mapTouchListener = null;

        /**
         * 当前地点击点
         */
        private GeoPoint currentPt = null;

        /**
         * 控制按钮
         */
        private Button zoomButton = null;
        private Button rotateButton = null;
        private Button overlookButton = null;
        private Button saveScreenButton = null;

        /**
         * 
         */
        private string touchType = null;

        /**
         * 用于显示地图状态的面板
         */
        private TextView mStateBar = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            /**
             * 使用地图sdk前需先初始化BMapManageResource.
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
            SetContentView(Resource.Layout.activity_mapcontrol);

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

            mStateBar = FindViewById<TextView>(Resource.Id.state);

            /**
             * 初始化地图事件监听
             */
            InitListener();

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

        private class MKMapTouchListenerImpl : Java.Lang.Object, IMKMapTouchListener
        {
            private MapControlDemo mapControlDemo;

            public MKMapTouchListenerImpl(MapControlDemo mapControlDemo)
            {
                this.mapControlDemo = mapControlDemo;
            }

            public void OnMapClick(GeoPoint point)
            {
                mapControlDemo.touchType = "单击";
                mapControlDemo.currentPt = point;
                mapControlDemo.UpdateMapState();
            }

            public void OnMapDoubleClick(GeoPoint point)
            {
                mapControlDemo.touchType = "双击";
                mapControlDemo.currentPt = point;
                mapControlDemo.UpdateMapState();
            }

            public void OnMapLongClick(GeoPoint point)
            {
                mapControlDemo.touchType = "长按";
                mapControlDemo.currentPt = point;
                mapControlDemo.UpdateMapState();
            }
        }

        private class MKMapViewListenerImpl : Java.Lang.Object, IMKMapViewListener
        {
            private MapControlDemo mapControlDemo;

            public MKMapViewListenerImpl(MapControlDemo mapControlDemo)
            {
                this.mapControlDemo = mapControlDemo;
            }

            public void OnMapMoveFinish()
            {
                /**
                 * 在此处理地图移动完成回调
                 * 缩放，平移等操作完成后，此回调被触发
                 */
                mapControlDemo.UpdateMapState();
            }

            public void OnClickMapPoi(MapPoi mapPoiInfo)
            {
                /**
                 * 在此处理底图poi点击事件
                 * 显示底图poi名称并移动至该点
                 * 设置过： mMapController.enableClick(true); 时，此回调才能被触发
                 * 
                 */
            }

            public void OnGetCurrentMap(Bitmap b)
            {
                /**
                 *  当调用过 mMapView.getCurrentMap()后，此回调会被触发
                 *  可在此保存截图至存储设备
                 */
                string filePath = "/mnt/sdcard/test.png";// File file = new File("/mnt/sdcard/test.png");
                System.IO.FileStream fileOutputStream;

                try
                {
                    fileOutputStream = new System.IO.FileStream(filePath, FileMode.Create);

                    if (b.Compress(Bitmap.CompressFormat.Png, 70, fileOutputStream))
                    {
                        fileOutputStream.Flush();
                        fileOutputStream.Close();
                    }

                    Toast.MakeText(mapControlDemo, "屏幕截图成功，图片存在: " + filePath.ToString(), ToastLength.Short).Show();
                }
                catch (System.IO.FileNotFoundException e)
                {
                    Log.Error("imknown", e.StackTrace);
                }
                catch (System.IO.IOException e)
                {
                    Log.Error("imknown", e.StackTrace);
                }
            }

            public void OnMapAnimationFinish()
            {
                /**
                 *  地图完成带动画的操作（如: animationTo()）后，此回调被触发
                 */
                mapControlDemo.UpdateMapState();
            }

            public void OnMapLoadFinish()
            {
                // TODO Auto-generated method stub
            }
        }

        private void InitListener()
        {
            /**
             * 设置地图点击事件监听 
             */
            mapTouchListener = new MKMapTouchListenerImpl(this);

            mMapView.RegMapTouchListner(mapTouchListener);

            /**
             * 设置地图事件监听
             */
            mMapListener = new MKMapViewListenerImpl(this);

            mMapView.RegMapViewListener(DemoApplication.getInstance().mBMapManager, mMapListener);

            /**
             * 设置按键监听
             */
            zoomButton = FindViewById<Button>(Resource.Id.zoombutton);
            rotateButton = FindViewById<Button>(Resource.Id.rotatebutton);
            overlookButton = FindViewById<Button>(Resource.Id.overlookbutton);
            saveScreenButton = FindViewById<Button>(Resource.Id.savescreen);

            Android.Views.View.IOnClickListener onClickListener = new OnClickListenerImpl(this);

            zoomButton.SetOnClickListener(onClickListener);
            rotateButton.SetOnClickListener(onClickListener);
            overlookButton.SetOnClickListener(onClickListener);
            saveScreenButton.SetOnClickListener(onClickListener);
        }

        private class OnClickListenerImpl : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            MapControlDemo mapControlDemo;

            public OnClickListenerImpl(MapControlDemo mapControlDemo)
            {
                this.mapControlDemo = mapControlDemo;
            }

            public void OnClick(Android.Views.View view)
            {
                if (view.Equals(mapControlDemo.zoomButton))
                {
                    mapControlDemo.PerfomZoom();
                }
                else if (view.Equals(mapControlDemo.rotateButton))
                {
                    mapControlDemo.PerfomRotate();
                }
                else if (view.Equals(mapControlDemo.overlookButton))
                {
                    mapControlDemo.PerfomOverlook();
                }
                else if (view.Equals(mapControlDemo.saveScreenButton))
                {
                    //截图，在MKMapViewListener中保存图片
                    bool x = mapControlDemo.mMapView.CurrentMap;
                    Toast.MakeText(mapControlDemo, "正在截取屏幕图片...", ToastLength.Short).Show();
                }

                mapControlDemo.UpdateMapState();
            }
        }

        /**
         * 处理缩放
         * sdk 缩放级别范围： [3.0,19.0]
         */
        private void PerfomZoom()
        {
            EditText t = FindViewById<EditText>(Resource.Id.zoomlevel);
            try
            {
                float zoomLevel = Float.ParseFloat(t.Text.ToString());
                mMapController.SetZoom(zoomLevel);
            }
            catch (NumberFormatException e)
            {
                Toast.MakeText(this, "请输入正确的缩放级别", ToastLength.Short).Show();
                e.PrintStackTrace();
            }
        }

        /**
         * 处理旋转 
         * 旋转角范围： -180 ~ 180 , 单位：度   逆时针旋转  
         */
        private void PerfomRotate()
        {
            EditText t = FindViewById<EditText>(Resource.Id.rotateangle);

            try
            {
                int rotateAngle = Integer.ParseInt(t.Text.ToString());
                mMapController.SetRotation(rotateAngle);
            }
            catch (NumberFormatException e)
            {
                Toast.MakeText(this, "请输入正确的旋转角度", ToastLength.Short).Show();
                e.PrintStackTrace();
            }
        }

        /**
         * 处理俯视
         * 俯角范围：  -45 ~ 0 , 单位： 度
         */
        private void PerfomOverlook()
        {
            EditText t = FindViewById<EditText>(Resource.Id.overlookangle);

            try
            {
                int overlookAngle = Integer.ParseInt(t.Text.ToString());
                mMapController.SetOverlooking(overlookAngle);
            }
            catch (NumberFormatException e)
            {
                Toast.MakeText(this, "请输入正确的俯角", ToastLength.Short).Show();
                e.PrintStackTrace();
            }
        }

        /**
         * 更新地图状态显示面板
         */
        private void UpdateMapState()
        {
            if (mStateBar == null)
            {
                return;
            }

            string state = "";

            if (currentPt == null)
            {
                state = "点击、长按、双击地图以获取经纬度和地图状态";
            }
            else
            {
                state = String.Format(touchType + ",当前经度 ： %f 当前纬度：%f", currentPt.LongitudeE6 * 1E-6, currentPt.LatitudeE6 * 1E-6);
            }

            state += "\n";
            state += String.Format("zoom level= %.1f    rotate angle= %d   overlaylook angle=  %d",
                mMapView.ZoomLevel,
                mMapView.MapRotation,
                mMapView.MapOverlooking
            );

            mStateBar.SetText(state, Android.Widget.TextView.BufferType.Normal);
        }

        protected override void OnPause()
        {
            /**
             *  MapView的生命周期与Activity同步，当activity挂起时需调用MapView.OnPause()
             */
            mMapView.OnPause();
            base.OnPause();
        }

        protected override void OnResume()
        {
            /**
             *  MapView的生命周期与Activity同步，当activity恢复时需调用MapView.OnResume()
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