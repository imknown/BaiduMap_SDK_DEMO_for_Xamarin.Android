
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
using Android.Graphics;

namespace baidumapsdk.demo
{
    /**
     * 演示覆盖物的用法
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_overlay", ScreenOrientation = ScreenOrientation.Sensor)]
    public class OverlayDemo : Activity
    {
        /**
	     *  MapView 是地图主控件
	     */
        private MapView mMapView = null;
        private Button mClearBtn;
        private Button mResetBtn;
        /**
         *  用MapController完成地图控制 
         */
        private MapController mMapController = null;
        private MyOverlay mOverlay = null;
        private PopupOverlay pop = null;
        private List<OverlayItem> mItems = null;
        private TextView popupText = null;
        private View viewCache = null;
        private View popupInfo = null;
        private View popupLeft = null;
        private View popupRight = null;
        private Button button = null;
        private MapView.LayoutParams layoutParam = null;
        private OverlayItem mCurItem = null;
        /**
         * overlay 位置坐标
         */
        double mLon1 = 116.400244;
        double mLat1 = 39.963175;
        double mLon2 = 116.369199;
        double mLat2 = 39.942821;
        double mLon3 = 116.425541;
        double mLat3 = 39.939723;
        double mLon4 = 116.401394;
        double mLat4 = 39.906965;

        // ground overlay
        private GroundOverlay mGroundOverlay;
        private Ground mGround;
        private double mLon5 = 116.380338;
        private double mLat5 = 39.92235;
        private double mLon6 = 116.414977;
        private double mLat6 = 39.947246;

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
            SetContentView(Resource.Layout.activity_overlay);
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mClearBtn = FindViewById<Button>(Resource.Id.clear);
            mResetBtn = FindViewById<Button>(Resource.Id.reset);
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
            mMapController.SetZoom(14);
            /**
             * 显示内置缩放控件
             */
            mMapView.SetBuiltInZoomControls(true);

            InitOverlay();

            /**
             * 设定地图中心点
             */
            GeoPoint p = new GeoPoint((int)(39.933859 * 1E6), (int)(116.400191 * 1E6));
            mMapController.SetCenter(p);
        }


        public void InitOverlay()
        {
            /**
             * 创建自定义overlay
             */
            mOverlay = new MyOverlay(this, Resources.GetDrawable(Resource.Drawable.icon_marka), mMapView);
            /**
             * 准备overlay 数据
             */
            GeoPoint p1 = new GeoPoint((int)(mLat1 * 1E6), (int)(mLon1 * 1E6));
            OverlayItem item1 = new OverlayItem(p1, "覆盖物1", "");
            /**
             * 设置overlay图标，如不设置，则使用创建ItemizedOverlay时的默认图标.
             */
            item1.Marker = Resources.GetDrawable(Resource.Drawable.icon_marka);

            GeoPoint p2 = new GeoPoint((int)(mLat2 * 1E6), (int)(mLon2 * 1E6));
            OverlayItem item2 = new OverlayItem(p2, "覆盖物2", "");
            item2.Marker = Resources.GetDrawable(Resource.Drawable.icon_markb);

            GeoPoint p3 = new GeoPoint((int)(mLat3 * 1E6), (int)(mLon3 * 1E6));
            OverlayItem item3 = new OverlayItem(p3, "覆盖物3", "");
            item3.Marker = Resources.GetDrawable(Resource.Drawable.icon_markc);

            GeoPoint p4 = new GeoPoint((int)(mLat4 * 1E6), (int)(mLon4 * 1E6));
            OverlayItem item4 = new OverlayItem(p4, "覆盖物4", "");
            item4.Marker = Resources.GetDrawable(Resource.Drawable.icon_gcoding);
            /**
             * 将item 添加到overlay中
             * 注意： 同一个itme只能add一次
             */
            mOverlay.AddItem(item1);
            mOverlay.AddItem(item2);
            mOverlay.AddItem(item3);
            mOverlay.AddItem(item4);
            /**
             * 保存所有item，以便overlay在reset后重新添加
             */
            mItems = new List<OverlayItem>();
            mItems.AddRange(mOverlay.AllItem);

            // 初始化 ground 图层
            mGroundOverlay = new GroundOverlay(mMapView);
            GeoPoint leftBottom = new GeoPoint((int)(mLat5 * 1E6),
                    (int)(mLon5 * 1E6));
            GeoPoint rightTop = new GeoPoint((int)(mLat6 * 1E6),
                    (int)(mLon6 * 1E6));
            Drawable d = Resources.GetDrawable(Resource.Drawable.ground_overlay);
            Bitmap bitmap = ((BitmapDrawable)d).Bitmap;
            mGround = new Ground(bitmap, leftBottom, rightTop);

            /**
             * 将overlay 添加至MapView中
             */
            mMapView.Overlays.Add(mOverlay);
            mMapView.Overlays.Add(mGroundOverlay);
            mGroundOverlay.AddGround(mGround);
            /**
             * 刷新地图
             */
            mMapView.Refresh();
            mResetBtn.Enabled = false;
            mClearBtn.Enabled = true;
            /**
             * 向地图添加自定义View.
             */
            viewCache = LayoutInflater.Inflate(Resource.Layout.custom_text_view, null);
            popupInfo = viewCache.FindViewById<View>(Resource.Id.popinfo);
            popupLeft = viewCache.FindViewById<View>(Resource.Id.popleft);
            popupRight = viewCache.FindViewById<View>(Resource.Id.popright);
            popupText = viewCache.FindViewById<TextView>(Resource.Id.textcache);

            button = new Button(this);
            button.SetBackgroundResource(Resource.Drawable.popup);

            /**
             * 创建一个popupoverlay
             */
            IPopupClickListener popListener = new IPopupClickListenerImpl(this);
            pop = new PopupOverlay(mMapView, popListener);
        }

        class IPopupClickListenerImpl : Java.Lang.Object, IPopupClickListener
        {

            OverlayDemo overlayDemo;
            public IPopupClickListenerImpl(OverlayDemo overlayDemo)
            {
                this.overlayDemo = overlayDemo;
            }

            public void OnClickedPopup(int index)
            {
                if (index == 0)
                {
                    //更新item位置
                    overlayDemo.pop.HidePop();
                    GeoPoint p = new GeoPoint(overlayDemo.mCurItem.Point.LatitudeE6 + 5000,
                            overlayDemo.mCurItem.Point.LongitudeE6 + 5000);
                    overlayDemo.mCurItem.SetGeoPoint(p);
                    overlayDemo.mOverlay.UpdateItem(overlayDemo.mCurItem);
                    overlayDemo.mMapView.Refresh();
                }
                else if (index == 2)
                {
                    //更新图标
                    overlayDemo.mCurItem.Marker = overlayDemo.Resources.GetDrawable(Resource.Drawable.nav_turn_via_1);
                    overlayDemo.mOverlay.UpdateItem(overlayDemo.mCurItem);
                    overlayDemo.mMapView.Refresh();
                }
            }
        }

        /**
         * 清除所有Overlay
         * @param view
         */
        [Java.Interop.Export]
        public void ClearOverlay(View view)
        {
            mOverlay.RemoveAll();
            mGroundOverlay.RemoveGround(mGround);
            if (pop != null)
            {
                pop.HidePop();
            }
            mMapView.RemoveView(button);
            mMapView.Refresh();
            mResetBtn.Enabled = true;
            mClearBtn.Enabled = false;
        }
        /**
         * 重新添加Overlay
         * @param view
         */
        [Java.Interop.Export]
        public void ResetOverlay(View view)
        {
            //重新add overlay
            mOverlay.AddItem(mItems);
            mGroundOverlay.AddGround(mGround);
            mMapView.Refresh();
            mResetBtn.Enabled = false;
            mClearBtn.Enabled = true;
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

        public class MyOverlay : ItemizedOverlay
        {
            OverlayDemo overlayDemo;
            public MyOverlay(OverlayDemo overlayDemo, Drawable defaultMarker, MapView mapView) :
                base(defaultMarker, mapView)
            {
                this.overlayDemo = overlayDemo;
            }

            protected override bool OnTap(int index)
            {
                OverlayItem item = GetItem(index);
                overlayDemo.mCurItem = item;
                if (index == 3)
                {
                    overlayDemo.button.Text = "这是一个系统控件";
                    GeoPoint pt = new GeoPoint((int)(overlayDemo.mLat4 * 1E6),
                            (int)(overlayDemo.mLon4 * 1E6));
                    // 弹出自定义View
                    overlayDemo.pop.ShowPopup(overlayDemo.button, pt, 32);
                }
                else
                {
                    overlayDemo.popupText.Text = GetItem(index).Title;
                    Bitmap[] bitMaps ={
				    BMapUtil.GetBitmapFromView(overlayDemo.popupLeft), 		
				    BMapUtil.GetBitmapFromView(overlayDemo.popupInfo), 		
				    BMapUtil.GetBitmapFromView(overlayDemo.popupRight) 		
			    };
                    overlayDemo.pop.ShowPopup(bitMaps, item.Point, 32);
                }
                return true;
            }


            public override bool OnTap(GeoPoint pt, MapView mMapView)
            {
                if (overlayDemo.pop != null)
                {
                    overlayDemo.pop.HidePop();
                    overlayDemo.mMapView.RemoveView(overlayDemo.button);
                }
                return false;
            }
        }
    }
}