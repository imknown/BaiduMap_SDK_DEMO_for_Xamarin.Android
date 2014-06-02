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
using Com.Baidu.Platform.Comapi.Basestruct;
using Java.IO;
using Java.Lang;
using System.IO;

namespace baidumapsdk.demo
{
    [Activity(Label = "@string/title_activity_panorama_demo")]
    public class PanoramaDemo : Activity
    {
        //通过全景ID打开全景是使用的默认ID，全景ID可以使用PanoramaService查询得到
        public static readonly string DEFAULT_PANORAMA_ID = "0100220000130817164838355J5";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_panorama_demo);
        }

        //通过poi uid 打开全景
        [Java.Interop.Export]
        public void StartPoiSelector(View v)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(PanoramaPoiSelectorActivity));
            StartActivity(intent);
        }

        //通过经纬度坐标开启全景
        [Java.Interop.Export]
        public void StartGeoSelector(View vS)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(PanoramaGeoSelectorActivity));
            StartActivity(intent);
        }
        //通过全景ID开启全景
        [Java.Interop.Export]
        public void StartIDSelector(View v)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(PanoramaDemoActivityMain));
            intent.PutExtra("pid", DEFAULT_PANORAMA_ID);
            StartActivity(intent);
        }
    }
}