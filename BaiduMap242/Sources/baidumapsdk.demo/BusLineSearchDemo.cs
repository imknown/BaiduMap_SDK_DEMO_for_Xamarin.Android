using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using baidumapsdk.demo;
using Com.Baidu.Mapapi;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Search;
using Com.Baidu.Platform.Comapi.Basestruct;
using Java.Lang;
using System.Collections;
using System.Collections.Generic;

namespace baidumapsdk.demo
{
    /**
     * 此demo用来展示如何进行公交线路详情检索，并使用RouteOverlay在地图上绘制
     * 同时展示如何浏览路线节点并弹出泡泡
     *
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_bus", ScreenOrientation = ScreenOrientation.Sensor)]
    public class BusLineSearchDemo : Activity
    {
        //UI相关
        Button mBtnSearch = null;
        Button mBtnNextLine = null;
        //浏览路线节点相关
        Button mBtnPre = null;//上一个节点
        Button mBtnNext = null;//下一个节点
        int nodeIndex = -2;//节点索引,供浏览节点时使用
        MKRoute route = null;//保存驾车/步行路线数据的变量，供浏览节点时使用
        private PopupOverlay pop = null;//弹出泡泡图层，浏览节点时使用
        private TextView popupText = null;//泡泡view
        private View viewCache = null;
        private List<string> busLineIDList = null;
        int busLineIndex = 0;

        //地图相关，使用继承MapView的MyBusLineMapView目的是重写touch事件实现泡泡处理
        //如果不处理touch事件，则无需继承，直接使用MapView即可
        MapView mMapView = null;	// 地图View	
        //搜索相关
        MKSearch mSearch = null;	// 搜索模块，也可去掉地图模块独立使用

        class MKMapTouchListenerImpl : Java.Lang.Object, IMKMapTouchListener
        {
            BusLineSearchDemo busLineSearchDemo;

            public MKMapTouchListenerImpl(BusLineSearchDemo busLineSearchDemo)
            {
                this.busLineSearchDemo = busLineSearchDemo;
            }

            public void OnMapClick(GeoPoint point)
            {
                //在此处理地图点击事件 
                //消隐pop
                if (busLineSearchDemo.pop != null)
                {
                    busLineSearchDemo.pop.HidePop();
                }
            }


            public void OnMapDoubleClick(GeoPoint point)
            {

            }


            public void OnMapLongClick(GeoPoint point)
            {

            }
        }

        class MKSearchListenerImpl : Java.Lang.Object, IMKSearchListener
        {
            private BusLineSearchDemo busLineSearchDemo;

            public MKSearchListenerImpl(BusLineSearchDemo busLineSearchDemo)
            {
                this.busLineSearchDemo = busLineSearchDemo;
            }

            public void OnGetPoiDetailSearchResult(int type, int error)
            {
            }

            public void OnGetPoiResult(MKPoiResult res, int type, int error)
            {
                // 错误号可参考MKEvent中的定义
                if (error != 0 || res == null)
                {
                    Toast.MakeText(busLineSearchDemo, "抱歉，未找到结果", ToastLength.Long).Show();
                    return;
                }

                // 找到公交路线poi node
                MKPoiInfo curPoi = null;
                int totalPoiNum = res.CurrentNumPois;
                //遍历所有poi，找到类型为公交线路的poi
                busLineSearchDemo.busLineIDList.Clear();
                for (int idx = 0; idx < totalPoiNum; idx++)
                {
                    if (2 == res.GetPoi(idx).EPoiType)
                    {
                        // poi类型，0：普通点，1：公交站，2：公交线路，3：地铁站，4：地铁线路
                        curPoi = res.GetPoi(idx);
                        //使用poi的uid发起公交详情检索
                        busLineSearchDemo.busLineIDList.Add(curPoi.Uid);
                        JavaSystem.Out.Println(curPoi.Uid);

                    }
                }
                busLineSearchDemo.SearchNextBusline();

                // 没有找到公交信息
                if (curPoi == null)
                {
                    Toast.MakeText(busLineSearchDemo, "抱歉，未找到结果", ToastLength.Long).Show();
                    return;
                }
                busLineSearchDemo.route = null;
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
            public void OnGetAddrResult(MKAddrInfo res, int error)
            {
            }
            /**
                * 获取公交路线结果，展示公交线路
                */
            public void OnGetBusDetailResult(MKBusLineResult result, int iError)
            {
                if (iError != 0 || result == null)
                {
                    Toast.MakeText(busLineSearchDemo, "抱歉，未找到结果", ToastLength.Long).Show();
                    return;
                }

                RouteOverlay routeOverlay = new RouteOverlay(busLineSearchDemo, busLineSearchDemo.mMapView);

                // 此处仅展示一个方案作为示例
                routeOverlay.SetData(result.BusRoute);

                //清除其他图层
                busLineSearchDemo.mMapView.Overlays.Clear();

                //添加路线图层
                busLineSearchDemo.mMapView.Overlays.Add(routeOverlay);

                //刷新地图使生效
                busLineSearchDemo.mMapView.Refresh();

                //移动地图到起点
                busLineSearchDemo.mMapView.Controller.AnimateTo(result.BusRoute.Start);

                //将路线数据保存给全局变量
                busLineSearchDemo.route = result.BusRoute;

                //重置路线节点索引，节点浏览时使用
                busLineSearchDemo.nodeIndex = -1;
                busLineSearchDemo.mBtnPre.Visibility = ViewStates.Visible;
                busLineSearchDemo.mBtnNext.Visibility = ViewStates.Visible;
                Toast.MakeText(busLineSearchDemo,
                                result.BusName,
                                ToastLength.Short).Show();
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

        class ClickListenerImpl : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            private BusLineSearchDemo busLineSearchDemo;

            public ClickListenerImpl(BusLineSearchDemo busLineSearchDemo)
            {
                this.busLineSearchDemo = busLineSearchDemo;
            }

            public void OnClick(View v)
            {
                busLineSearchDemo.SearchButtonProcess(v);
            }
        }

        class NextLineClickListener : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            private BusLineSearchDemo busLineSearchDemo;

            public NextLineClickListener(BusLineSearchDemo busLineSearchDemo)
            {
                this.busLineSearchDemo = busLineSearchDemo;
            }

            public void OnClick(View v)
            {
                busLineSearchDemo.SearchNextBusline();
            }
        }

        class NodeClickListener : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            private BusLineSearchDemo busLineSearchDemo;

            public NodeClickListener(BusLineSearchDemo busLineSearchDemo)
            {
                this.busLineSearchDemo = busLineSearchDemo;
            }

            public void OnClick(View v)
            {
                busLineSearchDemo.NodeClick(v);
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

            SetContentView(Resource.Layout.buslinesearch);

            ICharSequence titleLable = new String("公交线路查询功能");

            Title = titleLable.ToString();

            //地图初始化
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mMapView.Controller.EnableClick(true);
            mMapView.Controller.SetZoom(12);
            busLineIDList = new List<string>();

            //创建 弹出泡泡图层
            CreatePaopao();

            // 设定搜索按钮的响应
            mBtnSearch = FindViewById<Button>(Resource.Id.search);
            mBtnNextLine = FindViewById<Button>(Resource.Id.nextline);
            mBtnPre = FindViewById<Button>(Resource.Id.pre);
            mBtnNext = FindViewById<Button>(Resource.Id.next);
            mBtnPre.Visibility = ViewStates.Invisible;
            mBtnNext.Visibility = ViewStates.Invisible;

            //地图点击事件处理
            mMapView.RegMapTouchListner(new MKMapTouchListenerImpl(this));

            // 初始化搜索模块，注册事件监听
            mSearch = new MKSearch();
            mSearch.Init(app.mBMapManager, new MKSearchListenerImpl(this));

            Android.Views.View.IOnClickListener clickListener = new ClickListenerImpl(this);
            Android.Views.View.IOnClickListener nextLineClickListener = new NextLineClickListener(this);
            Android.Views.View.IOnClickListener nodeClickListener = new NodeClickListener(this);

            mBtnSearch.SetOnClickListener(clickListener);
            mBtnNextLine.SetOnClickListener(nextLineClickListener);
            mBtnPre.SetOnClickListener(nodeClickListener);
            mBtnNext.SetOnClickListener(nodeClickListener);

            //mBtnSearch.Click += (sender, e) =>
            //{
            //    SearchButtonProcess(mBtnSearch);
            //};


            //mBtnNextLine.Click += (sender, e) =>
            //{
            //    SearchNextBusline();
            //};

            //mBtnPre.Click += (sender, e) =>
            //{
            //    NodeClick(mBtnPre);
            //};

            //mBtnNext.Click += (sender, e) =>
            //{
            //    NodeClick(mBtnNext);
            //};
        }

        /**
         * 发起检索
         * @param v
         */
        void SearchButtonProcess(View v)
        {
            busLineIDList.Clear();
            busLineIndex = 0;
            mBtnPre.Visibility = ViewStates.Invisible;
            mBtnNext.Visibility = ViewStates.Invisible;
            if (mBtnSearch.Equals(v))
            {
                EditText editCity = FindViewById<EditText>(Resource.Id.city);
                EditText editSearchKey = FindViewById<EditText>(Resource.Id.searchkey);
                //发起poi检索，从得到所有poi中找到公交线路类型的poi，再使用该poi的uid进行公交详情搜索
                mSearch.PoiSearchInCity(editCity.Text.ToString(), editSearchKey.Text.ToString());
            }

        }

        void SearchNextBusline()
        {
            if (busLineIndex >= busLineIDList.Count)
            {
                busLineIndex = 0;
            }
            if (busLineIndex >= 0 && busLineIndex < busLineIDList.Count && busLineIDList.Count > 0)
            {
                mSearch.BusLineSearch((FindViewById<EditText>(Resource.Id.city)).Text.ToString(), busLineIDList[busLineIndex]);
                busLineIndex++;
            }

        }

        class PopListener : Java.Lang.Object, IPopupClickListener
        {
            private BusLineSearchDemo busLineSearchDemo;

            public PopListener(BusLineSearchDemo busLineSearchDemo)
            {
                this.busLineSearchDemo = busLineSearchDemo;
            }

            public void OnClickedPopup(int index)
            {
                Log.Verbose("click", "clickapoapo");
            }
        }

        /**
         * 创建弹出泡泡图层
         */
        public void CreatePaopao()
        {
            viewCache = LayoutInflater.Inflate(Resource.Layout.custom_text_view, null);
            popupText = viewCache.FindViewById<TextView>(Resource.Id.textcache);

            //泡泡点击响应回调
            IPopupClickListener popListener = new PopListener(this);

            pop = new PopupOverlay(mMapView, popListener);
        }

        /**
         * 节点浏览示例
         * @param v
         */
        public void NodeClick(View v)
        {

            if (nodeIndex < -1 || route == null || nodeIndex >= route.NumSteps)
                return;
            viewCache = LayoutInflater.Inflate(Resource.Layout.custom_text_view, null);
            popupText = viewCache.FindViewById<TextView>(Resource.Id.textcache);
            //上一个节点
            if (mBtnPre.Equals(v) && nodeIndex > 0)
            {
                //索引减
                nodeIndex--;
                //移动到指定索引的坐标
                mMapView.Controller.AnimateTo(route.GetStep(nodeIndex).Point);
                //弹出泡泡
                popupText.Text = route.GetStep(nodeIndex).Content;
                popupText.SetBackgroundResource(Resource.Drawable.popup);
                pop.ShowPopup(BMapUtil.GetBitmapFromView(popupText),
                        route.GetStep(nodeIndex).Point,
                        5);
            }
            //下一个节点
            if (mBtnNext.Equals(v) && nodeIndex < (route.NumSteps - 1))
            {
                //索引加
                nodeIndex++;
                //移动到指定索引的坐标
                mMapView.Controller.AnimateTo(route.GetStep(nodeIndex).Point);
                //弹出泡泡
                popupText.Text = route.GetStep(nodeIndex).Content;
                popupText.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.popup));
                pop.ShowPopup(BMapUtil.GetBitmapFromView(popupText),
                        route.GetStep(nodeIndex).Point,
                        5);
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
    }
}