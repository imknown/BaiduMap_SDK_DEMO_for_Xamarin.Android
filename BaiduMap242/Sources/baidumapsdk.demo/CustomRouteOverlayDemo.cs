using Android.App;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using baidumapsdk.demo;
using Com.Baidu.Mapapi;
using Com.Baidu.Mapapi.Cloud;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Search;
using Com.Baidu.Platform.Comapi.Basestruct;
using Java.Lang;
using System.Collections.Generic;

namespace baidumapsdk.demo
{
    /**
     * 此demo用来展示如何用自己的数据构造一条路线在地图上绘制出来
     *
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, ScreenOrientation = ScreenOrientation.Sensor)]
    public class CustomRouteOverlayDemo : Activity
    {
        //地图相关
        MapView mMapView = null;	// 地图View
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
            SetContentView(Resource.Layout.activity_customroute);
            ICharSequence titleLable = new String("路线规划功能——自设路线示例");
            Title = titleLable.ToString();
            //初始化地图
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mMapView.Controller.EnableClick(true);
            mMapView.Controller.SetZoom(13);

            /** 演示自定义路线使用方法	
             *  在北京地图上画一个北斗七星
             *  想知道某个点的百度经纬度坐标请点击：http://api.map.baidu.com/lbsapi/getpoint/index.html	
             */
            GeoPoint p1 = new GeoPoint((int)(39.9411 * 1E6), (int)(116.3714 * 1E6));
            GeoPoint p2 = new GeoPoint((int)(39.9498 * 1E6), (int)(116.3785 * 1E6));
            GeoPoint p3 = new GeoPoint((int)(39.9436 * 1E6), (int)(116.4029 * 1E6));
            GeoPoint p4 = new GeoPoint((int)(39.9329 * 1E6), (int)(116.4035 * 1E6));
            GeoPoint p5 = new GeoPoint((int)(39.9218 * 1E6), (int)(116.4115 * 1E6));
            GeoPoint p6 = new GeoPoint((int)(39.9144 * 1E6), (int)(116.4230 * 1E6));
            GeoPoint p7 = new GeoPoint((int)(39.9126 * 1E6), (int)(116.4387 * 1E6));
            //起点坐标
            GeoPoint start = p1;
            //终点坐标
            GeoPoint stop = p7;
            //第一站，站点坐标为p3,经过p1,p2
            GeoPoint[] step1 = new GeoPoint[3];
            step1[0] = p1;
            step1[1] = p2;
            step1[2] = p3;
            //第二站，站点坐标为p5,经过p4
            GeoPoint[] step2 = new GeoPoint[2];
            step2[0] = p4;
            step2[1] = p5;
            //第三站，站点坐标为p7,经过p6
            GeoPoint[] step3 = new GeoPoint[2];
            step3[0] = p6;
            step3[1] = p7;
            //站点数据保存在一个二维数据中
            GeoPoint[][] routeData = new GeoPoint[3][];
            routeData[0] = step1;
            routeData[1] = step2;
            routeData[2] = step3;
            //用站点数据构建一个MKRoute
            MKRoute route = new MKRoute();
            route.CustomizeRoute(start, stop, routeData);
            //将包含站点信息的MKRoute添加到RouteOverlay中
            RouteOverlay routeOverlay = new RouteOverlay(this, mMapView);
            routeOverlay.SetData(route);
            //向地图添加构造好的RouteOverlay
            mMapView.Overlays.Add(routeOverlay);
            //执行刷新使生效
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