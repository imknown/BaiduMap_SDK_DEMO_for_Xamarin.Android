using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using baidumapsdk.demo;
using Com.Baidu.Mapapi;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Search;
using Com.Baidu.Platform.Comapi.Basestruct;

namespace baidumapsdk.demo
{
    /**
     * 演示poi搜索功能 
     */
    [Activity(Label = "@string/demo_name_share")]
    public class ShareDemoActivity : Activity
    {
        private MapView mMapView = null;
        private MKSearch mSearch = null;   // 搜索模块，也可去掉地图模块独立使用
        //保存搜索结果地址
        private string currentAddr = null;
        //搜索城市 
        private string mCity = "北京";
        //搜索关键字
        private string searchKey = "餐馆";
        //反地理编译点坐标
        private GeoPoint mPoint = new GeoPoint((int)(40.056878 * 1E6), (int)(116.308141 * 1E6));

        class IMKSearchListenerImpl : Java.Lang.Object, IMKSearchListener
        {
            ShareDemoActivity shareDemoActivity;
            public IMKSearchListenerImpl(ShareDemoActivity shareDemoActivity)
            {
                this.shareDemoActivity = shareDemoActivity;
            }

            public void OnGetPoiDetailSearchResult(int type, int error)
            {
            }
            /**
             * 在此处理poi搜索结果 , 用poioverlay 显示
             */
            public void OnGetPoiResult(MKPoiResult res, int type, int error)
            {
                // 错误号可参考MKEvent中的定义
                if (error != 0 || res == null)
                {
                    Toast.MakeText(shareDemoActivity, "抱歉，未找到结果", ToastLength.Long).Show();
                    return;
                }
                // 将地图移动到第一个POI中心点
                if (res.CurrentNumPois > 0)
                {
                    // 将poi结果显示到地图上
                    PoiShareOverlay poiOverlay = new PoiShareOverlay(shareDemoActivity, shareDemoActivity.mMapView);
                    poiOverlay.SetData(res.AllPoi);
                    shareDemoActivity.mMapView.Overlays.Clear();
                    shareDemoActivity.mMapView.Overlays.Add(poiOverlay);
                    shareDemoActivity.mMapView.Refresh();
                    //当ePoiType为2（公交线路）或4（地铁线路）时， poi坐标为空
                    foreach (MKPoiInfo info in res.AllPoi)
                    {
                        if (info.Pt != null)
                        {
                            shareDemoActivity.mMapView.Controller.AnimateTo(info.Pt);
                            break;
                        }
                    }
                }
            }
            public void OnGetDrivingRouteResult(MKDrivingRouteResult res,
                    int error)
            {
            }
            public void OnGetTransitRouteResult(MKTransitRouteResult res,
                    int error)
            {
            }
            public void OnGetWalkingRouteResult(MKWalkingRouteResult res,
                    int error)
            {
            }
            /**
             * 在此处理反地理编结果
             */
            public void OnGetAddrResult(MKAddrInfo res, int error)
            {
                // 错误号可参考MKEvent中的定义
                if (error != 0 || res == null)
                {
                    Toast.MakeText(shareDemoActivity, "抱歉，未找到结果", ToastLength.Long).Show();
                    return;
                }
                AddrShareOverlay addrOverlay = new AddrShareOverlay(shareDemoActivity, shareDemoActivity.Resources.GetDrawable(Resource.Drawable.icon_marka), shareDemoActivity.mMapView, res);
                shareDemoActivity.mMapView.Overlays.Clear();
                shareDemoActivity.mMapView.Overlays.Add(addrOverlay);
                shareDemoActivity.mMapView.Refresh();

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
                //分享短串结果
                Intent it = new Intent(Intent.ActionSend);
                it.PutExtra(Intent.ExtraText, "您的朋友通过百度地图SDK与您分享一个位置: " +
                               shareDemoActivity.currentAddr +
                               " -- " + result.Url);
                it.SetType("text/plain");
                shareDemoActivity.StartActivity(Intent.CreateChooser(it, "将短串分享到"));

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
            SetContentView(Resource.Layout.activity_share_demo_activity);
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mMapView.Controller.EnableClick(true);
            mMapView.Controller.SetZoom(12);

            // 初始化搜索模块，注册搜索事件监听
            mSearch = new MKSearch();
            mSearch.Init(app.mBMapManager, new IMKSearchListenerImpl(this)
            {


            });
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
            mMapView.Destroy();
            mSearch.Destory();
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

        private void InitMapView()
        {
            mMapView.LongClickable = true;
            mMapView.Controller.SetZoom(14);
            mMapView.Controller.EnableClick(true);
            mMapView.SetBuiltInZoomControls(true);
        }

        [Java.Interop.Export]
        public void SharePoi(View view)
        {
            //发起poi搜索
            mSearch.PoiSearchInCity(mCity, searchKey);
            Toast.MakeText(this,
                    "在" + mCity + "搜索 " + searchKey,
                    ToastLength.Short).Show();
        }

        [Java.Interop.Export]
        public void ShareAddr(View view)
        {
            //发起反地理编码请求
            mSearch.ReverseGeocode(mPoint);
            Toast.MakeText(this,
                    string.Format("搜索位置： %f，%f", (mPoint.LatitudeE6 * 1E-6), (mPoint.LongitudeE6 * 1E-6)),
                    ToastLength.Short).Show();
        }

        /**
         * 使用PoiOverlay 展示poi点，在poi被点击时发起短串请求.
         *
         */
        private class PoiShareOverlay : PoiOverlay
        {
            ShareDemoActivity shareDemoActivity;
            public PoiShareOverlay(Activity activity, MapView mapView) :
                base(activity, mapView)
            {
                this.shareDemoActivity = (ShareDemoActivity)activity;
            }


            protected override bool OnTap(int i)
            {
                MKPoiInfo info = GetPoi(i);
                shareDemoActivity.currentAddr = info.Address;
                shareDemoActivity.mSearch.PoiDetailShareURLSearch(info.Uid);
                return true;
            }
        }
        /**
         * 使用ItemizevOvelray展示反地理编码点位置，当该点被点击时发起短串请求.
         *
         */
        private class AddrShareOverlay : ItemizedOverlay
        {

            ShareDemoActivity shareDemoActivity;


            private MKAddrInfo addrInfo;

            public AddrShareOverlay(ShareDemoActivity shareDemoActivity, Drawable defaultMarker, MapView mapView, MKAddrInfo addrInfo) :
                base(defaultMarker, mapView)
            {
                this.shareDemoActivity = shareDemoActivity;
                this.addrInfo = addrInfo;
                AddItem(new OverlayItem(addrInfo.GeoPt, addrInfo.StrAddr, addrInfo.StrAddr));
            }

            protected override bool OnTap(int index)
            {
                shareDemoActivity.currentAddr = addrInfo.StrAddr;
                shareDemoActivity.mSearch.PoiRGCShareURLSearch(addrInfo.GeoPt, "分享地址", addrInfo.StrAddr);
                return true;
            }

        }
    }
}