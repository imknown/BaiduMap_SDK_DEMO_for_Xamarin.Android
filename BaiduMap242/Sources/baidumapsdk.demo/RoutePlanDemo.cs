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
using Android.Content.PM;

namespace baidumapsdk.demo
{
    /**
     * 此demo用来展示如何进行驾车、步行、公交路线搜索并在地图使用RouteOverlay、TransitOverlay绘制
     * 同时展示如何进行节点浏览并弹出泡泡
     *
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_route", ScreenOrientation = ScreenOrientation.Sensor)]
    public class RoutePlanDemo : Activity
    {

        //UI相关
        Button mBtnDrive = null;	// 驾车搜索
        Button mBtnTransit = null;	// 公交搜索
        Button mBtnWalk = null;	// 步行搜索
        Button mBtnCusRoute = null; //自定义路线
        Button mBtnCusIcon = null; //自定义起终点图标

        //浏览路线节点相关
        Button mBtnPre = null;//上一个节点
        Button mBtnNext = null;//下一个节点
        int nodeIndex = -2;//节点索引,供浏览节点时使用
        MKRoute route = null;//保存驾车/步行路线数据的变量，供浏览节点时使用
        TransitOverlay transitOverlay = null;//保存公交路线图层数据的变量，供浏览节点时使用
        RouteOverlay routeOverlay = null;
        bool useDefaultIcon = false;
        int searchType = -1;//记录搜索的类型，区分驾车/步行和公交
        private PopupOverlay pop = null;//弹出泡泡图层，浏览节点时使用
        private TextView popupText = null;//泡泡view
        private View viewCache = null;

        //地图相关，使用继承MapView的MyRouteMapView目的是重写touch事件实现泡泡处理
        //如果不处理touch事件，则无需继承，直接使用MapView即可
        MapView mMapView = null;	// 地图View
        //搜索相关
        MKSearch mSearch = null;	// 搜索模块，也可去掉地图模块独立使用


        class ClickListenerImpl : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            private RoutePlanDemo routePlanDemo;

            public ClickListenerImpl(RoutePlanDemo routePlanDemo)
            {
                this.routePlanDemo = routePlanDemo;
            }

            public void OnClick(View v)
            {
                //发起搜索
                routePlanDemo.SearchButtonProcess(v);
            }
        }

        class NodeClickListenerImpl : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            private RoutePlanDemo routePlanDemo;

            public NodeClickListenerImpl(RoutePlanDemo routePlanDemo)
            {
                this.routePlanDemo = routePlanDemo;
            }

            public void OnClick(View v)
            {
                //浏览路线节点
                routePlanDemo.NodeClick(v);
            }
        }

        class CustomClickListenerImpl : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            private RoutePlanDemo routePlanDemo;

            public CustomClickListenerImpl(RoutePlanDemo routePlanDemo)
            {
                this.routePlanDemo = routePlanDemo;
            }

            public void OnClick(View v)
            {
                //自设路线绘制示例
                routePlanDemo.IntentToActivity();
            }
        }


        class ChangeRouteIconListenerImpl : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            private RoutePlanDemo routePlanDemo;

            public ChangeRouteIconListenerImpl(RoutePlanDemo routePlanDemo)
            {
                this.routePlanDemo = routePlanDemo;
            }

            public void OnClick(View v)
            {
                //浏览路线节点
                routePlanDemo.ChangeRouteIcon();
            }
        }

        class IMKMapTouchListenerImpl : Java.Lang.Object, IMKMapTouchListener
        {


            private RoutePlanDemo routePlanDemo;

            public IMKMapTouchListenerImpl(RoutePlanDemo routePlanDemo)
            {
                this.routePlanDemo = routePlanDemo;
            }


            public void OnMapClick(GeoPoint point)
            {
                //在此处理地图点击事件 
                //消隐pop
                if (routePlanDemo.pop != null)
                {
                    routePlanDemo.pop.HidePop();
                }
            }

            public void OnMapDoubleClick(GeoPoint p0)
            {
            }

            public void OnMapLongClick(GeoPoint p0)
            {
            }
        }
        class IMKSearchListenerImpl : Java.Lang.Object, IMKSearchListener
        {


            private RoutePlanDemo routePlanDemo;

            public IMKSearchListenerImpl(RoutePlanDemo routePlanDemo)
            {
                this.routePlanDemo = routePlanDemo;
            }




            public void OnGetDrivingRouteResult(MKDrivingRouteResult res,
                                int error)
            {

                //起点或终点有歧义，需要选择具体的城市列表或地址列表
                if (error == MKEvent.ErrorRouteAddr)
                {
                    //遍历所有地址
                    //IList<MKPoiInfo> stPois = res.AddrResult.MStartPoiList;
                    //IList<MKPoiInfo> enPois = res.AddrResult.MEndPoiList;
                    //IList<MKCityListInfo> stCities = res.AddrResult.MStartCityList;
                    //IList<MKCityListInfo> enCities = res.AddrResult.MEndCityList;
                    return;
                }
                // 错误号可参考MKEvent中的定义
                if (error != 0 || res == null)
                {
                    Toast.MakeText(routePlanDemo, "抱歉，未找到结果", ToastLength.Short).Show();
                    return;
                }

                routePlanDemo.searchType = 0;
                routePlanDemo.routeOverlay = new RouteOverlay(routePlanDemo, routePlanDemo.mMapView);
                // 此处仅展示一个方案作为示例
                routePlanDemo.routeOverlay.SetData(res.GetPlan(0).GetRoute(0));
                //清除其他图层
                routePlanDemo.mMapView.Overlays.Clear();
                //添加路线图层
                routePlanDemo.mMapView.Overlays.Add(routePlanDemo.routeOverlay);
                //执行刷新使生效
                routePlanDemo.mMapView.Refresh();
                // 使用zoomToSpan()绽放地图，使路线能完全显示在地图上
                routePlanDemo.mMapView.Controller.ZoomToSpan(routePlanDemo.routeOverlay.LatSpanE6, routePlanDemo.routeOverlay.LonSpanE6);
                //移动地图到起点
                routePlanDemo.mMapView.Controller.AnimateTo(res.Start.Pt);
                //将路线数据保存给全局变量
                routePlanDemo.route = res.GetPlan(0).GetRoute(0);
                //重置路线节点索引，节点浏览时使用
                routePlanDemo.nodeIndex = -1;
                routePlanDemo.mBtnPre.Visibility = ViewStates.Visible;
                routePlanDemo.mBtnNext.Visibility = ViewStates.Visible;

            }

            public void OnGetTransitRouteResult(MKTransitRouteResult res,
                                int error)
            {
                //起点或终点有歧义，需要选择具体的城市列表或地址列表
                if (error == MKEvent.ErrorRouteAddr)
                {
                    //遍历所有地址
                    //IList<MKPoiInfo> stPois = res.AddrResult.MStartPoiList;
                    //IList<MKPoiInfo> enPois = res.AddrResult.MEndPoiList;
                    //IList<MKCityListInfo> stCities = res.AddrResult.MStartCityList;
                    //IList<MKCityListInfo> enCities = res.AddrResult.MEndCityList;
                    return;
                }
                if (error != 0 || res == null)
                {
                    Toast.MakeText(routePlanDemo, "抱歉，未找到结果", ToastLength.Short).Show();
                    return;
                }

                routePlanDemo.searchType = 1;
                routePlanDemo.transitOverlay = new TransitOverlay(routePlanDemo, routePlanDemo.mMapView);
                // 此处仅展示一个方案作为示例
                routePlanDemo.transitOverlay.SetData(res.GetPlan(0));
                //清除其他图层
                routePlanDemo.mMapView.Overlays.Clear();
                //添加路线图层
                routePlanDemo.mMapView.Overlays.Add(routePlanDemo.transitOverlay);
                //执行刷新使生效
                routePlanDemo.mMapView.Refresh();
                // 使用zoomToSpan()绽放地图，使路线能完全显示在地图上
                routePlanDemo.mMapView.Controller.ZoomToSpan(routePlanDemo.transitOverlay.LatSpanE6, routePlanDemo.transitOverlay.LonSpanE6);
                //移动地图到起点
                routePlanDemo.mMapView.Controller.AnimateTo(res.Start.Pt);
                //重置路线节点索引，节点浏览时使用
                routePlanDemo.nodeIndex = 0;
                routePlanDemo.mBtnPre.Visibility = ViewStates.Visible;
                routePlanDemo.mBtnNext.Visibility = ViewStates.Visible;
            }

            public void OnGetWalkingRouteResult(MKWalkingRouteResult res,
                                int error)
            {
                //起点或终点有歧义，需要选择具体的城市列表或地址列表
                if (error == MKEvent.ErrorRouteAddr)
                {
                    //遍历所有地址
                    //IList<MKPoiInfo> stPois = res.AddrResult.MStartPoiList;
                    //IList<MKPoiInfo> enPois = res.AddrResult.MEndPoiList;
                    //IList<MKCityListInfo> stCities = res.AddrResult.MStartCityList;
                    //IList<MKCityListInfo> enCities = res.AddrResult.MEndCityList;
                    return;
                }
                if (error != 0 || res == null)
                {
                    Toast.MakeText(routePlanDemo, "抱歉，未找到结果", ToastLength.Short).Show();
                    return;
                }

                routePlanDemo.searchType = 2;
                routePlanDemo.routeOverlay = new RouteOverlay(routePlanDemo, routePlanDemo.mMapView);
                // 此处仅展示一个方案作为示例
                routePlanDemo.routeOverlay.SetData(res.GetPlan(0).GetRoute(0));
                //清除其他图层
                routePlanDemo.mMapView.Overlays.Clear();
                //添加路线图层
                routePlanDemo.mMapView.Overlays.Add(routePlanDemo.routeOverlay);
                //执行刷新使生效
                routePlanDemo.mMapView.Refresh();
                // 使用zoomToSpan()绽放地图，使路线能完全显示在地图上
                routePlanDemo.mMapView.Controller.ZoomToSpan(routePlanDemo.routeOverlay.LatSpanE6, routePlanDemo.routeOverlay.LonSpanE6);
                //移动地图到起点
                routePlanDemo.mMapView.Controller.AnimateTo(res.Start.Pt);
                //将路线数据保存给全局变量
                routePlanDemo.route = res.GetPlan(0).GetRoute(0);
                //重置路线节点索引，节点浏览时使用
                routePlanDemo.nodeIndex = -1;
                routePlanDemo.mBtnPre.Visibility = ViewStates.Visible;
                routePlanDemo.mBtnNext.Visibility = ViewStates.Visible;
            }

            public void OnGetAddrResult(MKAddrInfo res, int error)
            {
            }
            public void OnGetPoiResult(MKPoiResult res, int arg1, int arg2)
            {
            }
            public void OnGetBusDetailResult(MKBusLineResult result, int iError)
            {
            }

            public void OnGetSuggestionResult(MKSuggestionResult res, int arg1)
            {
            }

            public void OnGetPoiDetailSearchResult(int type, int iError)
            {
                // TODO Auto-generated method stub
            }

            public void OnGetShareUrlResult(MKShareUrlResult result, int type,
                    int error)
            {
                // TODO Auto-generated method stub

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
            SetContentView(Resource.Layout.routeplan);
            ICharSequence titleLable = new String("路线规划功能");
            Title = titleLable.ToString();
            //初始化地图
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mMapView.SetBuiltInZoomControls(false);
            mMapView.Controller.SetZoom(12);
            mMapView.Controller.EnableClick(true);

            //初始化按键
            mBtnDrive = FindViewById<Button>(Resource.Id.drive);
            mBtnTransit = FindViewById<Button>(Resource.Id.transit);
            mBtnWalk = FindViewById<Button>(Resource.Id.walk);
            mBtnPre = FindViewById<Button>(Resource.Id.pre);
            mBtnNext = FindViewById<Button>(Resource.Id.next);
            mBtnCusRoute = FindViewById<Button>(Resource.Id.custombutton);
            mBtnCusIcon = FindViewById<Button>(Resource.Id.customicon);
            mBtnPre.Visibility = ViewStates.Invisible;
            mBtnNext.Visibility = ViewStates.Invisible;

            //按键点击事件
            Android.Views.View.IOnClickListener clickListener = new ClickListenerImpl(this);
            Android.Views.View.IOnClickListener nodeClickListener = new NodeClickListenerImpl(this);
            Android.Views.View.IOnClickListener customClickListener = new CustomClickListenerImpl(this);
            Android.Views.View.IOnClickListener changeRouteIconListener = new ChangeRouteIconListenerImpl(this);

            mBtnDrive.SetOnClickListener(clickListener);
            mBtnTransit.SetOnClickListener(clickListener);
            mBtnWalk.SetOnClickListener(clickListener);
            mBtnPre.SetOnClickListener(nodeClickListener);
            mBtnNext.SetOnClickListener(nodeClickListener);
            mBtnCusRoute.SetOnClickListener(customClickListener);
            mBtnCusIcon.SetOnClickListener(changeRouteIconListener);
            //创建 弹出泡泡图层
            CreatePaopao();

            //地图点击事件处理
            mMapView.RegMapTouchListner(new IMKMapTouchListenerImpl(this));
            // 初始化搜索模块，注册事件监听
            mSearch = new MKSearch();
            mSearch.Init(app.mBMapManager, new IMKSearchListenerImpl(this));
        }
        /**
         * 发起路线规划搜索示例
         * @param v
         */
        void SearchButtonProcess(View v)
        {
            //重置浏览节点的路线数据
            route = null;
            routeOverlay = null;
            transitOverlay = null;
            mBtnPre.Visibility = ViewStates.Invisible;
            mBtnNext.Visibility = ViewStates.Invisible;
            // 处理搜索按钮响应
            EditText editSt = FindViewById<EditText>(Resource.Id.start);
            EditText editEn = FindViewById<EditText>(Resource.Id.end);

            // 对起点终点的name进行赋值，也可以直接对坐标赋值，赋值坐标则将根据坐标进行搜索
            MKPlanNode stNode = new MKPlanNode();
            stNode.Name = editSt.Text;
            MKPlanNode enNode = new MKPlanNode();
            enNode.Name = editEn.Text;

            // 实际使用中请对起点终点城市进行正确的设定
            if (mBtnDrive.Equals(v))
            {
                mSearch.DrivingSearch("北京", stNode, "北京", enNode);
            }
            else if (mBtnTransit.Equals(v))
            {
                mSearch.TransitSearch("北京", stNode, enNode);
            }
            else if (mBtnWalk.Equals(v))
            {
                mSearch.WalkingSearch("北京", stNode, "北京", enNode);
            }
        }
        /**
         * 节点浏览示例
         * @param v
         */
        public void NodeClick(View v)
        {
            viewCache = LayoutInflater.Inflate(Resource.Layout.custom_text_view, null);
            popupText = viewCache.FindViewById<TextView>(Resource.Id.textcache);
            if (searchType == 0 || searchType == 2)
            {
                //驾车、步行使用的数据结构相同，因此类型为驾车或步行，节点浏览方法相同
                if (nodeIndex < -1 || route == null || nodeIndex >= route.NumSteps)
                    return;

                //上一个节点
                if (mBtnPre.Equals(v) && nodeIndex > 0)
                {
                    //索引减
                    nodeIndex--;
                    //移动到指定索引的坐标
                    mMapView.Controller.AnimateTo(route.GetStep(nodeIndex).Point);
                    //弹出泡泡
                    popupText.SetBackgroundResource(Resource.Drawable.popup);
                    popupText.Text = route.GetStep(nodeIndex).Content;
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
                    popupText.SetBackgroundResource(Resource.Drawable.popup);
                    popupText.Text = route.GetStep(nodeIndex).Content;
                    pop.ShowPopup(BMapUtil.GetBitmapFromView(popupText),
                            route.GetStep(nodeIndex).Point,
                            5);
                }
            }
            if (searchType == 1)
            {
                //公交换乘使用的数据结构与其他不同，因此单独处理节点浏览
                if (nodeIndex < -1 || transitOverlay == null || nodeIndex >= transitOverlay.AllItem.Count)
                    return;

                //上一个节点
                if (mBtnPre.Equals(v) && nodeIndex > 1)
                {
                    //索引减
                    nodeIndex--;
                    //移动到指定索引的坐标
                    mMapView.Controller.AnimateTo(transitOverlay.GetItem(nodeIndex).Point);
                    //弹出泡泡
                    popupText.SetBackgroundResource(Resource.Drawable.popup);
                    popupText.Text = transitOverlay.GetItem(nodeIndex).Title;
                    pop.ShowPopup(BMapUtil.GetBitmapFromView(popupText),
                            transitOverlay.GetItem(nodeIndex).Point,
                            5);
                }
                //下一个节点
                if (mBtnNext.Equals(v) && nodeIndex < (transitOverlay.AllItem.Count - 2))
                {
                    //索引加
                    nodeIndex++;
                    //移动到指定索引的坐标
                    mMapView.Controller.AnimateTo(transitOverlay.GetItem(nodeIndex).Point);
                    //弹出泡泡
                    popupText.SetBackgroundResource(Resource.Drawable.popup);
                    popupText.Text = transitOverlay.GetItem(nodeIndex).Title;
                    pop.ShowPopup(BMapUtil.GetBitmapFromView(popupText),
                            transitOverlay.GetItem(nodeIndex).Point,
                            5);
                }
            }

        }

        class IPopupClickListenerImpl : Java.Lang.Object, IPopupClickListener
        {
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

            ////泡泡点击响应回调
            IPopupClickListener popListener = new IPopupClickListenerImpl();
            pop = new PopupOverlay(mMapView, popListener);
        }
        /**
         * 跳转自设路线Activity
         */
        public void IntentToActivity()
        {
            //跳转到自设路线演示demo
            Intent intent = new Intent();
            intent.SetClass(this, typeof(CustomRouteOverlayDemo));
            StartActivity(intent);
        }

        /**
         * 切换路线图标，刷新地图使其生效
         * 注意： 起终点图标使用中心对齐.
         */
        protected void ChangeRouteIcon()
        {
            Button btn = FindViewById<Button>(Resource.Id.customicon);
            if (routeOverlay == null && transitOverlay == null)
            {
                return;
            }
            if (useDefaultIcon)
            {
                if (routeOverlay != null)
                {
                    routeOverlay.StMarker = null;
                    routeOverlay.EnMarker = null;
                }
                if (transitOverlay != null)
                {
                    transitOverlay.StMarker = null;
                    transitOverlay.EnMarker = null;
                }
                btn.Text = "自定义起终点图标";
                Toast.MakeText(this,
                               "将使用系统起终点图标",
                               ToastLength.Short).Show();
            }
            else
            {
                if (routeOverlay != null)
                {
                    routeOverlay.StMarker = Resources.GetDrawable(Resource.Drawable.icon_st);
                    routeOverlay.EnMarker = Resources.GetDrawable(Resource.Drawable.icon_en);
                }
                if (transitOverlay != null)
                {
                    transitOverlay.StMarker = Resources.GetDrawable(Resource.Drawable.icon_st);
                    transitOverlay.EnMarker = Resources.GetDrawable(Resource.Drawable.icon_en);
                }
                btn.Text = "系统起终点图标";
                Toast.MakeText(this,
                               "将使用自定义起终点图标",
                               ToastLength.Short).Show();
            }
            useDefaultIcon = !useDefaultIcon;
            mMapView.Refresh();

        }

        protected override void OnDestroy()
        {
            mMapView.Destroy();
            base.OnDestroy();
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