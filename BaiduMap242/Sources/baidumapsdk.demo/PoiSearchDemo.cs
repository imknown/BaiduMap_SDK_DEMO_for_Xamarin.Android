
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
using Android.Text;

namespace baidumapsdk.demo
{
    /**
     * 演示poi搜索功能 
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_poi", ScreenOrientation = ScreenOrientation.Sensor)]
    public class PoiSearchDemo : Activity
    {
        private MapView mMapView = null;
        private MKSearch mSearch = null;   // 搜索模块，也可去掉地图模块独立使用
        /**
         * 搜索关键字输入窗口
         */
        private AutoCompleteTextView keyWorldsView = null;
        private ArrayAdapter<string> sugAdapter = null;
        private int load_Index;

        class IMKSearchListenerImpl : Java.Lang.Object, IMKSearchListener
        {
            PoiSearchDemo poiSearchDemo;

            public IMKSearchListenerImpl(PoiSearchDemo poiSearchDemo)
            {
                this.poiSearchDemo = poiSearchDemo;
            }

            //在此处理详情页结果            
            public void OnGetPoiDetailSearchResult(int type, int error)
            {
                if (error != 0)
                {
                    Toast.MakeText(poiSearchDemo, "抱歉，未找到结果", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(poiSearchDemo, "成功，查看详情页面", ToastLength.Short).Show();
                }
            }

            /**
             * 在此处理poi搜索结果
             */
            public void OnGetPoiResult(MKPoiResult res, int type, int error)
            {
                // 错误号可参考MKEvent中的定义
                if (error != 0 || res == null)
                {
                    Toast.MakeText(poiSearchDemo, "抱歉，未找到结果", ToastLength.Long).Show();
                    return;
                }
                // 将地图移动到第一个POI中心点
                if (res.CurrentNumPois > 0)
                {
                    // 将poi结果显示到地图上
                    MyPoiOverlay poiOverlay = new MyPoiOverlay(poiSearchDemo, poiSearchDemo.mMapView, poiSearchDemo.mSearch);
                    poiOverlay.SetData(res.AllPoi);
                    poiSearchDemo.mMapView.Overlays.Clear();
                    poiSearchDemo.mMapView.Overlays.Add(poiOverlay);
                    poiSearchDemo.mMapView.Refresh();
                    //当ePoiType为2（公交线路）或4（地铁线路）时， poi坐标为空
                    foreach (MKPoiInfo info in res.AllPoi)
                    {
                        if (info.Pt != null)
                        {
                            poiSearchDemo.mMapView.Controller.AnimateTo(info.Pt);
                            break;
                        }
                    }
                }
                else if (res.CityListNum > 0)
                {
                    //当输入关键字在本市没有找到，但在其他城市找到时，返回包含该关键字信息的城市列表
                    string strInfo = "在";
                    for (int i = 0; i < res.CityListNum; i++)
                    {
                        strInfo += res.GetCityListInfo(i).City;
                        strInfo += ",";
                    }
                    strInfo += "找到结果";
                    Toast.MakeText(poiSearchDemo, strInfo, ToastLength.Long).Show();
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
            public void OnGetAddrResult(MKAddrInfo res, int error)
            {
            }
            public void OnGetBusDetailResult(MKBusLineResult result, int iError)
            {
            }
            /**
             * 更新建议列表
             */
            public void OnGetSuggestionResult(MKSuggestionResult res, int arg1)
            {
                if (res == null || res.AllSuggestions == null)
                {
                    return;
                }
                poiSearchDemo.sugAdapter.Clear();
                foreach (MKSuggestionInfo info in res.AllSuggestions)
                {
                    if (info.Key != null)
                        poiSearchDemo.sugAdapter.Add(info.Key);
                }
                poiSearchDemo.sugAdapter.NotifyDataSetChanged();

            }

            public void OnGetShareUrlResult(MKShareUrlResult result, int type,
                    int error)
            {
                // TODO Auto-generated method stub

            }
        }

        class ITextWatcherImpl : Java.Lang.Object, ITextWatcher
        {
            PoiSearchDemo poiSearchDemo;

            public ITextWatcherImpl(PoiSearchDemo poiSearchDemo)
            {
                this.poiSearchDemo = poiSearchDemo;
            }

            public void AfterTextChanged(IEditable arg0)
            {
            }

            public void BeforeTextChanged(ICharSequence arg0, int arg1,
                    int arg2, int arg3)
            {
            }

            public void OnTextChanged(ICharSequence cs, int arg1, int arg2,
                    int arg3)
            {
                if (cs.Length() <= 0)
                {
                    return;
                }
                string city = (poiSearchDemo.FindViewById<EditText>(Resource.Id.city)).Text;
                /**
                 * 使用建议搜索服务获取建议列表，结果在OnSuggestionResult()中更新
                 */
                poiSearchDemo.mSearch.SuggestionSearch(cs.ToString(), city);
            }
        }

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

            SetContentView(Resource.Layout.activity_poisearch);
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mMapView.Controller.EnableClick(true);
            mMapView.Controller.SetZoom(12);

            // 初始化搜索模块，注册搜索事件监听
            mSearch = new MKSearch();
            mSearch.Init(app.mBMapManager, new IMKSearchListenerImpl(this));

            keyWorldsView = FindViewById<AutoCompleteTextView>(Resource.Id.searchkey);
            sugAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleDropDownItem1Line);
            keyWorldsView.Adapter = sugAdapter;

            /**
             * 当输入关键字变化时，动态更新建议列表
             */

            keyWorldsView.AddTextChangedListener(new ITextWatcherImpl(this));
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

        private void initMapView()
        {
            mMapView.LongClickable = true;
            mMapView.Controller.SetZoom(14);
            mMapView.Controller.EnableClick(true);
            mMapView.SetBuiltInZoomControls(true);
        }

        /**
         * 影响搜索按钮点击事件
         * @param v
         */
        [Java.Interop.Export]
        public void SearchButtonProcess(View v)
        {
            EditText editCity = FindViewById<EditText>(Resource.Id.city);
            EditText editSearchKey = FindViewById<EditText>(Resource.Id.searchkey);
            mSearch.PoiSearchInCity(editCity.Text,
                    editSearchKey.Text);
        }

        [Java.Interop.Export]
        public void GoToNextPage(View v)
        {
            //搜索下一组poi
            int flag = mSearch.GoToPoiPage(++load_Index);
            if (flag != 0)
            {
                Toast.MakeText(this, "先搜索开始，然后再搜索下一组数据", ToastLength.Short).Show();
            }
        }
    }
}