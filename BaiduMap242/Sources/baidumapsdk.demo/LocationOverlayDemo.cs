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
using Com.Baidu.Location;
using Android.Content.PM;

namespace baidumapsdk.demo
{
    /**
     * 此demo用来展示如何结合定位SDK实现定位，并使用MyLocationOverlay绘制定位位置
     * 同时展示如何使用自定义图标绘制并点击时弹出泡泡
     *
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_location", ScreenOrientation = ScreenOrientation.Sensor)]
    public class LocationOverlayDemo : Activity
    {
        private enum E_BUTTON_TYPE
        {
            LOC,
            COMPASS,
            FOLLOW
        }

        private E_BUTTON_TYPE mCurBtnType;

        // 定位相关
        LocationClient mLocClient;
        LocationData locData = null;
        public MyLocationListenner myListener;// = new MyLocationListenner(this);

        //定位图层
        LocationOverlay myLocationOverlay = null;
        //弹出泡泡图层
        private PopupOverlay pop = null;//弹出泡泡图层，浏览节点时使用
        private TextView popupText = null;//泡泡view
        private View viewCache = null;

        //地图相关，使用继承MapView的MyLocationMapView目的是重写touch事件实现泡泡处理
        //如果不处理touch事件，则无需继承，直接使用MapView即可
        MyLocationMapView mMapView = null;	// 地图View
        private MapController mMapController = null;

        //UI相关
        Android.Widget.RadioGroup.IOnCheckedChangeListener radioButtonListener = null;
        Button requestLocButton = null;
        bool isRequest = false;//是否手动触发请求定位
        bool isFirstLoc = true;//是否首次定位

        class BtnClickListenerImpl : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            LocationOverlayDemo locationOverlayDemo;

            public BtnClickListenerImpl(LocationOverlayDemo locationOverlayDemo)
            {
                this.locationOverlayDemo = locationOverlayDemo;
            }

            public void OnClick(View v)
            {
                switch (locationOverlayDemo.mCurBtnType)
                {
                    case E_BUTTON_TYPE.LOC:
                        //手动定位请求
                        locationOverlayDemo.RequestLocClick();
                        break;
                    case E_BUTTON_TYPE.COMPASS:
                        locationOverlayDemo.myLocationOverlay.SetLocationMode(Com.Baidu.Mapapi.Map.MyLocationOverlay.LocationMode.Normal);
                        locationOverlayDemo.requestLocButton.Text = "定位";
                        locationOverlayDemo.mCurBtnType = E_BUTTON_TYPE.LOC;
                        break;
                    case E_BUTTON_TYPE.FOLLOW:
                        locationOverlayDemo.myLocationOverlay.SetLocationMode(Com.Baidu.Mapapi.Map.MyLocationOverlay.LocationMode.Compass);
                        locationOverlayDemo.requestLocButton.Text = "罗盘";
                        locationOverlayDemo.mCurBtnType = E_BUTTON_TYPE.COMPASS;
                        break;
                }
            }
        }

        class RadioButtonListenerImpl : Java.Lang.Object, Android.Widget.RadioGroup.IOnCheckedChangeListener
        {
            LocationOverlayDemo locationOverlayDemo;

            public RadioButtonListenerImpl(LocationOverlayDemo locationOverlayDemo)
            {
                this.locationOverlayDemo = locationOverlayDemo;
            }

            public void OnCheckedChanged(RadioGroup group, int checkedId)
            {
                if (checkedId == Resource.Id.defaulticon)
                {
                    //传入null则，恢复默认图标
                    locationOverlayDemo.ModifyLocationOverlayIcon(null);
                }
                if (checkedId == Resource.Id.customicon)
                {
                    //修改为自定义marker
                    locationOverlayDemo.ModifyLocationOverlayIcon(locationOverlayDemo.Resources.GetDrawable(Resource.Drawable.icon_geo));
                }
            }
        }

        class PopListenerImpl : Java.Lang.Object, IPopupClickListener
        {
            LocationOverlayDemo locationOverlayDemo;

            public PopListenerImpl(LocationOverlayDemo locationOverlayDemo)
            {
                this.locationOverlayDemo = locationOverlayDemo;
            }

            public void OnClickedPopup(int index)
            {
                Log.Verbose("click", "clickapoapo");
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
            SetContentView(Resource.Layout.activity_locationoverlay);
            ICharSequence titleLable = new String("定位功能");
            Title = titleLable.ToString();

            myListener = new MyLocationListenner(this);

            requestLocButton = FindViewById<Button>(Resource.Id.button1);
            mCurBtnType = E_BUTTON_TYPE.LOC;
            Android.Views.View.IOnClickListener btnClickListener = new BtnClickListenerImpl(this);

            requestLocButton.SetOnClickListener(btnClickListener);

            RadioGroup group = this.FindViewById<RadioGroup>(Resource.Id.radioGroup);
            radioButtonListener = new RadioButtonListenerImpl(this);
            group.SetOnCheckedChangeListener(radioButtonListener);

            //地图初始化
            mMapView = FindViewById<MyLocationMapView>(Resource.Id.bmapView);
            mMapController = mMapView.Controller;
            mMapView.Controller.SetZoom(14);
            mMapView.Controller.EnableClick(true);
            mMapView.SetBuiltInZoomControls(true);
            //创建 弹出泡泡图层
            CreatePaopao();

            //定位初始化
            mLocClient = new LocationClient(this);
            locData = new LocationData();
            mLocClient.RegisterLocationListener(myListener);
            LocationClientOption option = new LocationClientOption();
            option.OpenGps = true;//打开gps
            option.CoorType = "bd09ll";     //设置坐标类型
            option.ScanSpan = 1000;
            mLocClient.LocOption = option;
            mLocClient.Start();

            //定位图层初始化
            myLocationOverlay = new LocationOverlay(this, mMapView);
            //设置定位数据
            myLocationOverlay.SetData(locData);
            //添加定位图层
            mMapView.Overlays.Add(myLocationOverlay);
            myLocationOverlay.EnableCompass();
            //修改定位数据后刷新图层生效
            mMapView.Refresh();

        }
        /**
         * 手动触发一次定位请求
         */
        public void RequestLocClick()
        {
            isRequest = true;
            mLocClient.RequestLocation();
            Toast.MakeText(this, "正在定位……", ToastLength.Short).Show();
        }
        /**
         * 修改位置图标
         * @param marker
         */
        public void ModifyLocationOverlayIcon(Drawable marker)
        {
            //当传入marker为null时，使用默认图标绘制
            myLocationOverlay.SetMarker(marker);
            //修改图层，需要刷新MapView生效
            mMapView.Refresh();
        }
        /**
	     * 创建弹出泡泡图层
	     */
        public void CreatePaopao()
        {
            viewCache = LayoutInflater.Inflate(Resource.Layout.custom_text_view, null);
            popupText = viewCache.FindViewById<TextView>(Resource.Id.textcache);
            //泡泡点击响应回调
            IPopupClickListener popListener = new PopListenerImpl(this);
            pop = new PopupOverlay(mMapView, popListener);
            MyLocationMapView.pop = pop;
        }
        /**
         * 定位SDK监听函数
         */
        public class MyLocationListenner : Java.Lang.Object, IBDLocationListener
        {

            LocationOverlayDemo locationOverlayDemo;

            public MyLocationListenner(LocationOverlayDemo locationOverlayDemo)
            {
                this.locationOverlayDemo = locationOverlayDemo;
            }

            public void OnReceiveLocation(BDLocation location)
            {
                if (location == null)
                    return;

                locationOverlayDemo.locData.Latitude = location.Latitude;
                locationOverlayDemo.locData.Longitude = location.Longitude;
                //如果不显示定位精度圈，将accuracy赋值为0即可
                locationOverlayDemo.locData.Accuracy = location.Radius;
                // 此处可以设置 locData的方向信息, 如果定位 SDK 未返回方向信息，用户可以自己实现罗盘功能添加方向信息。
                locationOverlayDemo.locData.Direction = location.Derect;
                //更新定位数据
                locationOverlayDemo.myLocationOverlay.SetData(locationOverlayDemo.locData);
                //更新图层数据执行刷新后生效
                locationOverlayDemo.mMapView.Refresh();
                //是手动触发请求或首次定位时，移动到定位点
                if (locationOverlayDemo.isRequest || locationOverlayDemo.isFirstLoc)
                {
                    //移动地图到定位点
                    Log.Debug("LocationOverlay", "receive location, animate to it");
                    locationOverlayDemo.mMapController.AnimateTo(new GeoPoint((int)(locationOverlayDemo.locData.Latitude * 1e6), (int)(locationOverlayDemo.locData.Longitude * 1e6)));
                    locationOverlayDemo.isRequest = false;
                    locationOverlayDemo.myLocationOverlay.SetLocationMode(Com.Baidu.Mapapi.Map.MyLocationOverlay.LocationMode.Following);
                    locationOverlayDemo.requestLocButton.Text = "跟随";
                    locationOverlayDemo.mCurBtnType = E_BUTTON_TYPE.FOLLOW;
                }
                //首次定位完成
                locationOverlayDemo.isFirstLoc = false;
            }

            public void OnReceivePoi(BDLocation poiLocation)
            {
                if (poiLocation == null)
                {
                    return;
                }
            }
        }

        //继承MyLocationOverlay重写dispatchTap实现点击处理
        public class LocationOverlay : MyLocationOverlay
        {


            LocationOverlayDemo locationOverlayDemo;

            public LocationOverlay(LocationOverlayDemo locationOverlayDemo, MapView mapView)
                : base(mapView)
            {
                // TODO Auto-generated constructor stub
                this.locationOverlayDemo = locationOverlayDemo;
            }

            protected override bool DispatchTap()
            {
                // TODO Auto-generated method stub
                //处理点击事件,弹出泡泡
                locationOverlayDemo.popupText.SetBackgroundResource(Resource.Drawable.popup);
                locationOverlayDemo.popupText.Text = "我的位置";
                locationOverlayDemo.pop.ShowPopup(BMapUtil.GetBitmapFromView(locationOverlayDemo.popupText),
                        new GeoPoint((int)(locationOverlayDemo.locData.Latitude * 1e6), (int)(locationOverlayDemo.locData.Longitude * 1e6)),
                        8);
                return true;
            }

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

        protected override void OnDestroy()
        {
            //退出时销毁定位
            if (mLocClient != null)
                mLocClient.Stop();
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //        getMenuInflater().inflate(R.menu.activity_main, menu);
            return true;
        }
    }

    /**
     * 继承MapView重写onTouchEvent实现泡泡处理操作
     * @author hejin
     *
     */
    class MyLocationMapView : MapView
    {
        internal static PopupOverlay pop = null;//弹出泡泡图层，点击图标使用
        public MyLocationMapView(Context context)
            : base(context)
        {

            // TODO Auto-generated constructor stub
        }
        public MyLocationMapView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {

        }
        public MyLocationMapView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {

        }
        public override bool OnTouchEvent(MotionEvent eventX)
        {
            if (!base.OnTouchEvent(eventX))
            {
                //消隐泡泡
                if (pop != null && eventX.Action == MotionEventActions.Up)
                    pop.HidePop();
            }
            return true;
        }
    }
}