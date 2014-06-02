
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

namespace baidumapsdk.demo
{
    public class MyPoiOverlay : PoiOverlay
    {
        MKSearch mSearch;

        public MyPoiOverlay(Activity activity, MapView mapView, MKSearch search)
            : base(activity, mapView)
        {
            mSearch = search;
        }

        protected override bool OnTap(int i)
        {
            base.OnTap(i);
            MKPoiInfo info = GetPoi(i);
            if (info.HasCaterDetails)
            {
                mSearch.PoiDetailSearch(info.Uid);
            }
            return true;
        }
    }
}