using Android.App;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using baidumapsdk.demo;
using Com.Baidu.Mapapi;
using Com.Baidu.Mapapi.Cloud;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Platform.Comapi.Basestruct;
using Java.Lang;
using System.Collections.Generic;

namespace baidumapsdk.demo
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_cloud", ScreenOrientation = ScreenOrientation.Sensor)]
    public class CloudSearchActivity : Activity, ICloudListener
    {
        MapView mMapView;
        protected override void OnCreate(Bundle icicle)
        {
            base.OnCreate(icicle);
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
            SetContentView(Resource.Layout.lbssearch);
            CloudManager.Instance.Init(this);

            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mMapView.Controller.EnableClick(true);
            mMapView.Controller.SetZoom(12);
            mMapView.DoubleClickZooming = true;

            FindViewById(Resource.Id.regionSearch).Click += (sender, e) =>
            {
                LocalSearchInfo info = new LocalSearchInfo();
                info.Ak = "B266f735e43ab207ec152deff44fec8b";
                info.GeoTableId = 31869;
                info.Tags = "";
                info.Q = "天安门";
                info.Region = "北京市";
                CloudManager.Instance.LocalSearch(info);
            };

            FindViewById(Resource.Id.nearbySearch).Click += (sender, e) =>
            {
                NearbySearchInfo info = new NearbySearchInfo();
                info.Ak = "D9ace96891048231e8777291cda45ca0";
                info.GeoTableId = 32038;
                info.Filter = "time:20130801,20130810";
                info.Location = "116.403689,39.914957";
                info.Radius = 30000;
                CloudManager.Instance.NearbySearch(info);
            };

            FindViewById(Resource.Id.boundsSearch).Click += (sender, e) =>
            {
                BoundSearchInfo info = new BoundSearchInfo();
                info.Ak = "B266f735e43ab207ec152deff44fec8b";
                info.GeoTableId = 31869;
                info.Q = "天安门";
                info.Bound = "116.401663,39.913961;116.406529,39.917396";
                CloudManager.Instance.BoundSearch(info);
            };

            FindViewById(Resource.Id.detailsSearch).Click += (sender, e) =>
            {
                DetailSearchInfo info = new DetailSearchInfo();
                info.Ak = "B266f735e43ab207ec152deff44fec8b";
                info.GeoTableId = 31869;
                info.Uid = 18622266;
                CloudManager.Instance.DetailSearch(info);
            };
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

        public void OnGetDetailSearchResult(DetailSearchResult result, int error)
        {
            if (result != null)
            {
                if (result.PoiInfo != null)
                {
                    Toast.MakeText(this, result.PoiInfo.Title, ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "status:" + result.Status, ToastLength.Short).Show();
                }
            }
        }

        public void OnGetSearchResult(CloudSearchResult result, int error)
        {
            if (result != null && result.PoiList != null && result.PoiList.Count > 0)
            {
                CloudOverlay poiOverlay = new CloudOverlay(this, mMapView);
                poiOverlay.SetData(result.PoiList);
                mMapView.Overlays.Clear();
                mMapView.Overlays.Add(poiOverlay);
                mMapView.Refresh();
                mMapView.Controller.AnimateTo(new GeoPoint((int)((result.PoiList)[0].Latitude * 1e6), (int)((result.PoiList)[0].Longitude * 1e6)));
            }
        }
    }
    class CloudOverlay : ItemizedOverlay
    {

        IList<CloudPoiInfo> mLbsPoints;
        Activity mContext;

        public CloudOverlay(Activity context, MapView mMapView)
            : base(null, mMapView)
        {

            mContext = context;
        }

        public void SetData(IList<CloudPoiInfo> lbsPoints)
        {
            if (lbsPoints != null)
            {
                mLbsPoints = lbsPoints;
            }
            foreach (CloudPoiInfo rec in mLbsPoints)
            {
                GeoPoint pt = new GeoPoint((int)(rec.Latitude * 1e6), (int)(rec.Longitude * 1e6));
                OverlayItem item = new OverlayItem(pt, rec.Title, rec.Address);
                Drawable marker1 = this.mContext.Resources.GetDrawable(Resource.Drawable.icon_marka);
                item.Marker = marker1;
                AddItem(item);
            }
        }

        protected override Object Clone() // throws CloneNotSupportedException
        {
            // TODO Auto-generated method stub
            return base.Clone();
        }

        protected override bool OnTap(int arg0)
        {
            CloudPoiInfo item = mLbsPoints[arg0];
            Toast.MakeText(mContext, item.Title, ToastLength.Long).Show();
            return base.OnTap(arg0);
        }
    }
}