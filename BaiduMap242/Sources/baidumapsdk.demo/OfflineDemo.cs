
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

namespace baidumapsdk.demo
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_offline", ScreenOrientation = ScreenOrientation.Sensor)]
    public class OfflineDemo : Activity, IMKOfflineMapListener
    {
        private MapView mMapView = null;
        private MKOfflineMap mOffline = null;
        private TextView cidView;
        private TextView stateView;
        private EditText cityNameView;
        private MapController mMapController = null;
        /**
         * 已下载的离线地图信息列表
         */
        private IList<MKOLUpdateElement> localMapList = null;
        private LocalMapAdapter lAdapter = null;

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
            SetContentView(Resource.Layout.activity_offline);
            mMapView = new MapView(this);
            mMapController = mMapView.Controller;

            mOffline = new MKOfflineMap();
            /**
             * 初始化离线地图模块,MapControler可从MapView.getController()获取
             */
            mOffline.Init(mMapController, this);
            InitView();

        }

        private void InitView()
        {

            cidView = FindViewById<TextView>(Resource.Id.cityid);
            cityNameView = FindViewById<EditText>(Resource.Id.city);
            stateView = FindViewById<TextView>(Resource.Id.state);

            ListView hotCityList = FindViewById<ListView>(Resource.Id.hotcitylist);
            IList<string> hotCities = new List<string>();
            //获取热闹城市列表
            IList<MKOLSearchRecord> records1 = mOffline.HotCityList;
            if (records1 != null)
            {
                foreach (MKOLSearchRecord r in records1)
                {
                    hotCities.Add(r.CityName + "(" + r.CityID + ")" + "   --" + this.FormatDataSize(r.Size));
                }
            }
            IListAdapter hAdapter = (IListAdapter)new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, hotCities);
            hotCityList.Adapter = hAdapter;

            ListView allCityList = FindViewById<ListView>(Resource.Id.allcitylist);
            //获取所有支持离线地图的城市
            IList<string> allCities = new List<string>();
            IList<MKOLSearchRecord> records2 = mOffline.OfflineCityList;
            if (records1 != null)
            {
                foreach (MKOLSearchRecord r in records2)
                {
                    allCities.Add(r.CityName + "(" + r.CityID + ")" + "   --" + this.FormatDataSize(r.Size));
                }
            }
            IListAdapter aAdapter = (IListAdapter)new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, allCities);
            allCityList.Adapter = aAdapter;

            LinearLayout cl = FindViewById<LinearLayout>(Resource.Id.citylist_layout);
            LinearLayout lm = FindViewById<LinearLayout>(Resource.Id.localmap_layout);
            lm.Visibility = ViewStates.Gone;
            cl.Visibility = ViewStates.Visible;

            //获取已下过的离线地图信息
            localMapList = mOffline.AllUpdateInfo;
            if (localMapList == null)
            {
                localMapList = new List<MKOLUpdateElement>();
            }

            ListView localMapListView = FindViewById<ListView>(Resource.Id.localmaplist);
            lAdapter = new LocalMapAdapter(this);
            localMapListView.Adapter = lAdapter;

        }
        /**
         * 切换至城市列表
         * @param view
         */
        [Java.Interop.Export]
        public void ClickCityListButton(View view)
        {
            LinearLayout cl = FindViewById<LinearLayout>(Resource.Id.citylist_layout);
            LinearLayout lm = FindViewById<LinearLayout>(Resource.Id.localmap_layout);
            lm.Visibility = ViewStates.Gone;
            cl.Visibility = ViewStates.Visible;

        }
        /**
         * 切换至下载管理列表
         * @param view
         */
        [Java.Interop.Export]
        public void ClickLocalMapListButton(View view)
        {
            LinearLayout cl = FindViewById<LinearLayout>(Resource.Id.citylist_layout);
            LinearLayout lm = FindViewById<LinearLayout>(Resource.Id.localmap_layout);
            lm.Visibility = ViewStates.Visible;
            cl.Visibility = ViewStates.Gone;
        }
        /**
         * 搜索离线需市
         * @param view
         */
        [Java.Interop.Export]
        public void Search(View view)
        {
            IList<MKOLSearchRecord> records = mOffline.SearchCity(cityNameView.Text);
            if (records == null || records.Count != 1)
                return;
            cidView.Text = String.ValueOf(records[0].CityID);
        }

        /**
         * 开始下载
         * @param view
         */
        [Java.Interop.Export]
        public void Start(View view)
        {
            int cityid = Integer.ParseInt(cidView.Text);
            mOffline.Start(cityid);
            ClickLocalMapListButton(null);
            Toast.MakeText(this, "开始下载离线地图. cityid: " + cityid, ToastLength.Short)
                      .Show();
        }
        /**
         * 暂停下载
         * @param view
         */
        [Java.Interop.Export]
        public void Stop(View view)
        {
            int cityid = Integer.ParseInt(cidView.Text);
            mOffline.Pause(cityid);
            Toast.MakeText(this, "暂停下载离线地图. cityid: " + cityid, ToastLength.Short)
                      .Show();
        }
        /**
         * 删除离线地图
         * @param view
         */
        [Java.Interop.Export]
        public void Remove(View view)
        {
            int cityid = Integer.ParseInt(cidView.Text);
            mOffline.Remove(cityid);
            Toast.MakeText(this, "删除离线地图. cityid: " + cityid, ToastLength.Short)
                      .Show();
        }
        /**
         * 从SD卡导入离线地图安装包
         * @param view
         */
        [Java.Interop.Export]
        public void ImportFromSDCard(View view)
        {
            int num = mOffline.Scan();
            string msg = "";
            if (num == 0)
            {
                msg = "没有导入离线包，这可能是离线包放置位置不正确，或离线包已经导入过";
            }
            else
            {
                msg = string.Format("成功导入 {0} 个离线包，可以在下载管理查看", num);
            }
            Toast.MakeText(this, msg, ToastLength.Short).Show();
        }
        /**
         * 更新状态显示 
         */
        public void UpdateView()
        {
            localMapList = mOffline.AllUpdateInfo;
            if (localMapList == null)
            {
                localMapList = new List<MKOLUpdateElement>();
            }
            lAdapter.NotifyDataSetChanged();
        }




        protected override void OnPause()
        {
            int cityid = Integer.ParseInt(cidView.Text);
            mOffline.Pause(cityid);
            mMapView.OnPause();
            base.OnPause();
        }


        protected override void OnResume()
        {
            mMapView.OnResume();
            base.OnResume();
        }


        public string FormatDataSize(int size)
        {
            string ret = "";
            if (size < (1024 * 1024))
            {
                ret = string.Format("{0}K", size / 1024);
            }
            else
            {
                ret = string.Format("{0:F1}M", size / (1024 * 1024.0));
            }
            return ret;
        }


        protected override void OnDestroy()
        {
            /**
             * 退出时，销毁离线地图模块
             */
            mOffline.Destroy();
            mMapView.Destroy();
            base.OnDestroy();
        }


        public void OnGetOfflineMapState(int type, int state)
        {
            switch (type)
            {
                case MKOfflineMap.TypeDownloadUpdate:
                    {
                        MKOLUpdateElement update = mOffline.GetUpdateInfo(state);
                        //处理下载进度更新提示
                        if (update != null)
                        {
                            stateView.Text = string.Format("{0} : {1}" + "%", update.CityName, update.Ratio);
                            UpdateView();
                        }
                    }
                    break;
                case MKOfflineMap.TypeNewOffline:
                    //有新离线地图安装
                    Log.Debug("OfflineDemo", string.Format("add offlinemap num:{0}", state));
                    break;
                case MKOfflineMap.TypeVerUpdate:
                    // 版本更新提示
                    //	MKOLUpdateElement e = mOffline.getUpdateInfo(state);

                    break;
            }

        }
        /**
         * 离线地图管理列表适配器
         */
        public class LocalMapAdapter : BaseAdapter
        {

            class IOnClickRemoveListenerImpl : Java.Lang.Object, Android.Views.View.IOnClickListener
            {
                private LocalMapAdapter localMapAdapter;
                private MKOLUpdateElement e;

                public IOnClickRemoveListenerImpl(LocalMapAdapter localMapAdapter, MKOLUpdateElement e)
                {
                    this.localMapAdapter = localMapAdapter;
                    this.e = e;
                }

                public void OnClick(View arg0)
                {
                    localMapAdapter.offlineDemo.mOffline.Remove(e.CityID);
                    localMapAdapter.offlineDemo.UpdateView();
                }
            }

            class IOnClickDisplayListenerImpl : Java.Lang.Object, Android.Views.View.IOnClickListener
            {
                private LocalMapAdapter localMapAdapter;
                private MKOLUpdateElement e;

                public IOnClickDisplayListenerImpl(LocalMapAdapter localMapAdapter, MKOLUpdateElement e)
                {
                    this.localMapAdapter = localMapAdapter;
                    this.e = e;
                }

                public void OnClick(View arg0)
                {
                    Intent intent = new Intent();
                    intent.PutExtra("x", e.GeoPt.LongitudeE6);
                    intent.PutExtra("y", e.GeoPt.LatitudeE6);
                    intent.SetClass(localMapAdapter.offlineDemo, typeof(BaseMapDemo));
                    localMapAdapter.offlineDemo.StartActivity(intent);
                }
            }

            private OfflineDemo offlineDemo;

            public LocalMapAdapter(OfflineDemo offlineDemo)
            {
                this.offlineDemo = offlineDemo;
            }

            public override int Count
            {
                get { return offlineDemo.localMapList.Count; }
            }

            public override Object GetItem(int index)
            {
                return offlineDemo.localMapList[index];
            }


            public override long GetItemId(int index)
            {
                return index;
            }


            public override View GetView(int index, View view, ViewGroup arg2)
            {
                MKOLUpdateElement e = (MKOLUpdateElement)GetItem(index);
                view = View.Inflate(offlineDemo, Resource.Layout.offline_localmap_list, null);
                InitViewItem(view, e);
                return view;
            }

            void InitViewItem(View view, MKOLUpdateElement e)
            {
                Button display = view.FindViewById<Button>(Resource.Id.display);
                Button remove = view.FindViewById<Button>(Resource.Id.remove);
                TextView title = view.FindViewById<TextView>(Resource.Id.title);
                TextView update = view.FindViewById<TextView>(Resource.Id.update);
                TextView ratio = view.FindViewById<TextView>(Resource.Id.ratio);

                ratio.Text = e.Ratio + "%";
                title.Text = e.CityName;
                if (e.Update)
                {
                    update.Text = "可更新";
                }
                else
                {
                    update.Text = "最新";
                }
                if (e.Ratio != 100)
                {
                    display.Enabled = false;
                }
                else
                {
                    display.Enabled = true;
                }




                remove.SetOnClickListener(new IOnClickRemoveListenerImpl(this, e));


                display.SetOnClickListener(new IOnClickDisplayListenerImpl(this, e));
            }




        }



    }
}