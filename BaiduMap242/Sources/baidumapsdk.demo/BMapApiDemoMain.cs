using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using baidumapsdk.demo;
using Com.Baidu.Mapapi;
using System;
using Object = Java.Lang.Object;

namespace baidumapsdk.demo
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, Label = "BaiduMapSDKDemo", MainLauncher = true, ScreenOrientation = ScreenOrientation.Sensor)]
    public class BMapApiDemoMain : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);

            ListView mListView = FindViewById<ListView>(Resource.Id.listView);
            // 添加ListItem，设置事件响应
            mListView.Adapter = new DemoListAdapter(this);
            mListView.OnItemClickListener = new OnItemClickListenerImpl(this);
        }

        class OnItemClickListenerImpl : Java.Lang.Object, Android.Widget.AdapterView.IOnItemClickListener
        {
            BMapApiDemoMain bMapApiDemoMain;

            public OnItemClickListenerImpl(BMapApiDemoMain bMapApiDemoMain)
            {
                this.bMapApiDemoMain = bMapApiDemoMain;
            }

            public void OnItemClick(AdapterView parent, View view, int index, long id)
            {
                bMapApiDemoMain.OnListItemClick(index);
            }
        }

        void OnListItemClick(int index)
        {
            Intent intent = null;
            intent = new Intent(this, demos[index].demoClass.GetType());
            this.StartActivity(intent);
        }

        private static readonly DemoInfo<Activity>[] demos =
        {
			new DemoInfo<Activity>(Resource.String.demo_title_basemap,Resource.String.demo_desc_basemap, new BaseMapDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_map_fragment,Resource.String.demo_desc_map_fragment, new MapFragmentDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_mutimap, Resource.String.demo_desc_mutimap, new MutiMapViewDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_layers, Resource.String.demo_desc_layers, new LayersDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_control, Resource.String.demo_desc_control, new MapControlDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_ui, Resource.String.demo_desc_ui, new UISettingDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_location, Resource.String.demo_desc_location, new LocationOverlayDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_geometry, Resource.String.demo_desc_geometry,new GeometryDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_overlay, Resource.String.demo_desc_overlay,new OverlayDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_poi, Resource.String.demo_desc_poi,new PoiSearchDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_geocode, Resource.String.demo_desc_geocode,new GeoCoderDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_route, Resource.String.demo_desc_route,new RoutePlanDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_bus, Resource.String.demo_desc_bus,new BusLineSearchDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_offline, Resource.String.demo_desc_offline,new OfflineDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_panorama,Resource.String.demo_desc_panorama, new PanoramaDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_navi, Resource.String.demo_desc_navi,new NaviDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_cloud, Resource.String.demo_desc_cloud,new CloudSearchDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_share, Resource.String.demo_desc_share,new ShareDemo())
	    };

        protected override void OnResume()
        {
            DemoApplication app = (DemoApplication)this.Application;
            TextView text = FindViewById<TextView>(Resource.Id.text_Info);
            if (!app.m_bKeyRight)
            {
                text.SetText(Resource.String.key_error);
                text.SetTextColor(Color.Red);
            }
            else
            {
                text.SetTextColor(Color.Yellow);
                text.Text = "欢迎使用百度地图Android SDK v" + VersionInfo.ApiVersion;
            }
            base.OnResume();
        }

        // 建议在APP整体退出之前调用MapApi的destroy()函数，不要在每个activity的OnDestroy中调用，
        // 避免MapApi重复创建初始化，提高效率
        protected override void OnDestroy()
        {
            DemoApplication app = (DemoApplication)this.Application;
            if (app.mBMapManager != null)
            {
                app.mBMapManager.Destroy();
                app.mBMapManager = null;
            }
            base.OnDestroy();
            System.Environment.Exit(0);
        }

        private class DemoListAdapter : BaseAdapter
        {
            BMapApiDemoMain bMapApiDemoMain;

            public DemoListAdapter(BMapApiDemoMain bMapApiDemoMain)
                : base()
            {
                this.bMapApiDemoMain = bMapApiDemoMain;
            }

            public override View GetView(int index, View convertView, ViewGroup parent)
            {
                convertView = View.Inflate(bMapApiDemoMain, Resource.Layout.demo_info_item, null);
                TextView title = convertView.FindViewById<TextView>(Resource.Id.title);
                TextView desc = convertView.FindViewById<TextView>(Resource.Id.desc);
                if (demos[index].demoClass.GetType() == typeof(NaviDemo)
                            || demos[index].demoClass.GetType() == typeof(CloudSearchDemo)
                            || demos[index].demoClass.GetType() == typeof(ShareDemo)
                            || demos[index].demoClass.GetType() == typeof(PanoramaDemo)
                    )
                {
                    title.SetTextColor(Color.Yellow);
                    desc.SetTextColor(Color.Yellow);
                }
                title.SetText(demos[index].title);
                desc.SetText(demos[index].desc);
                return convertView;
            }

            public override int Count
            {
                get { return demos.Length; }
            }

            public override Object GetItem(int index)
            {
                return demos[index];
            }

            public override long GetItemId(int id)
            {
                return id;
            }
        }

        private class DemoInfo<T> : Object where T : Activity// Java.Lang.Object
        {
            public readonly int title;
            public readonly int desc;
            public readonly T demoClass;

            public DemoInfo(int title, int desc, T demoClass)
            {
                this.title = title;
                this.desc = desc;
                this.demoClass = demoClass;
            }
        }
    }
}