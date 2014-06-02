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
     * 此demo用来展示如何在地图上用GraphicsOverlay添加点、线、多边形、圆
     * 同时展示如何在地图上用TextOverlay添加文字
     *
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_geometry", ScreenOrientation = ScreenOrientation.Sensor)]
    public class GeometryDemo : Activity
    {
        //地图相关
        MapView mMapView = null;

        //UI相关
        Button resetBtn = null;
        Button clearBtn = null;

        class ClearListenerImpl : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            GeometryDemo geometryDemo;
            public ClearListenerImpl(GeometryDemo geometryDemo)
            {
                this.geometryDemo = geometryDemo;
            }
            public void OnClick(View v)
            {
                geometryDemo.ClearClick();
            }
        }
        class RestListenerImpl : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            GeometryDemo geometryDemo;
            public RestListenerImpl(GeometryDemo geometryDemo)
            {
                this.geometryDemo = geometryDemo;
            }
            public void OnClick(View v)
            {
                geometryDemo.ResetClick();
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
            SetContentView(Resource.Layout.activity_geometry);
            ICharSequence titleLable = new String("自定义绘制功能");
            Title = titleLable.ToString();

            //初始化地图
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mMapView.Controller.SetZoom(12.5f);
            mMapView.Controller.EnableClick(true);

            //UI初始化
            clearBtn = FindViewById<Button>(Resource.Id.button1);
            resetBtn = FindViewById<Button>(Resource.Id.button2);

            Android.Views.View.IOnClickListener clearListener = new ClearListenerImpl(this);
            Android.Views.View.IOnClickListener restListener = new RestListenerImpl(this);

            clearBtn.SetOnClickListener(clearListener);
            resetBtn.SetOnClickListener(restListener);

            //界面加载时添加绘制图层
            AddCustomElementsDemo();
        }

        /**
         * 添加点、线、多边形、圆、文字
         */
        public void AddCustomElementsDemo()
        {
            GraphicsOverlay graphicsOverlay = new GraphicsOverlay(mMapView);
            mMapView.Overlays.Add(graphicsOverlay);
            //添加点
            graphicsOverlay.SetData(DrawPoint());
            //添加折线
            graphicsOverlay.SetData(DrawLine());
            //添加弧线
            graphicsOverlay.SetData(DrawArc());
            //添加多边形
            graphicsOverlay.SetData(DrawPolygon());
            //添加圆
            graphicsOverlay.SetData(DrawCircle());
            //绘制文字
            TextOverlay textOverlay = new TextOverlay(mMapView);
            mMapView.Overlays.Add(textOverlay);
            textOverlay.AddText(DrawText());
            //执行地图刷新使生效
            mMapView.Refresh();
        }

        public void ResetClick()
        {
            //添加绘制元素
            AddCustomElementsDemo();
        }

        public void ClearClick()
        {
            //清除所有图层
            mMapView.Overlays.Clear();
            mMapView.Refresh();
        }

        /**
         * 绘制折线，该折线状态随地图状态变化
         * @return 折线对象
         */
        public Graphic DrawLine()
        {
            double mLat = 39.97923;
            double mLon = 116.357428;

            int lat = (int)(mLat * 1E6);
            int lon = (int)(mLon * 1E6);
            GeoPoint pt1 = new GeoPoint(lat, lon);

            mLat = 39.94923;
            mLon = 116.397428;
            lat = (int)(mLat * 1E6);
            lon = (int)(mLon * 1E6);
            GeoPoint pt2 = new GeoPoint(lat, lon);
            mLat = 39.97923;
            mLon = 116.437428;
            lat = (int)(mLat * 1E6);
            lon = (int)(mLon * 1E6);
            GeoPoint pt3 = new GeoPoint(lat, lon);

            //构建线
            Geometry lineGeometry = new Geometry();
            //设定折线点坐标
            GeoPoint[] linePoints = new GeoPoint[3];
            linePoints[0] = pt1;
            linePoints[1] = pt2;
            linePoints[2] = pt3;
            lineGeometry.SetPolyLine(linePoints);
            //设定样式
            Symbol lineSymbol = new Symbol();
            Symbol.Color lineColor = new Com.Baidu.Mapapi.Map.Symbol.Color(lineSymbol);
            lineColor.Red = 255;
            lineColor.Green = 0;
            lineColor.Blue = 0;
            lineColor.Alpha = 255;
            lineSymbol.SetLineSymbol(lineColor, 10);
            //生成Graphic对象
            Graphic lineGraphic = new Graphic(lineGeometry, lineSymbol);
            return lineGraphic;
        }

        /**
         * 绘制弧线
         * 
         * @return 折线对象
         */
        public Graphic DrawArc()
        {
            double mLat = 39.97923;
            double mLon = 116.357428;

            int lat = (int)(mLat * 1E6);
            int lon = (int)(mLon * 1E6);
            GeoPoint pt1 = new GeoPoint(lat, lon);

            mLat = 39.94923;
            mLon = 116.397428;
            lat = (int)(mLat * 1E6);
            lon = (int)(mLon * 1E6);
            GeoPoint pt2 = new GeoPoint(lat, lon);
            mLat = 39.97923;
            mLon = 116.437428;
            lat = (int)(mLat * 1E6);
            lon = (int)(mLon * 1E6);
            GeoPoint pt3 = new GeoPoint(lat, lon);

            Geometry arcGeometry = new Geometry();

            arcGeometry.SetArc(pt1, pt3, pt2);
            // 设定样式
            Symbol arcSymbol = new Symbol();
            Symbol.Color arcColor = new Com.Baidu.Mapapi.Map.Symbol.Color(arcSymbol);
            arcColor.Red = 255;
            arcColor.Green = 0;
            arcColor.Blue = 225;
            arcColor.Alpha = 255;
            arcSymbol.SetLineSymbol(arcColor, 4);
            // 生成Graphic对象
            Graphic arcGraphic = new Graphic(arcGeometry, arcSymbol);
            return arcGraphic;
        }

        /**
         * 绘制多边形，该多边形随地图状态变化
         * @return 多边形对象
         */
        public Graphic DrawPolygon()
        {
            double mLat = 39.93923;
            double mLon = 116.357428;
            int lat = (int)(mLat * 1E6);
            int lon = (int)(mLon * 1E6);
            GeoPoint pt1 = new GeoPoint(lat, lon);
            mLat = 39.91923;
            mLon = 116.327428;
            lat = (int)(mLat * 1E6);
            lon = (int)(mLon * 1E6);
            GeoPoint pt2 = new GeoPoint(lat, lon);
            mLat = 39.89923;
            mLon = 116.347428;
            lat = (int)(mLat * 1E6);
            lon = (int)(mLon * 1E6);
            GeoPoint pt3 = new GeoPoint(lat, lon);
            mLat = 39.89923;
            mLon = 116.367428;
            lat = (int)(mLat * 1E6);
            lon = (int)(mLon * 1E6);
            GeoPoint pt4 = new GeoPoint(lat, lon);
            mLat = 39.91923;
            mLon = 116.387428;
            lat = (int)(mLat * 1E6);
            lon = (int)(mLon * 1E6);
            GeoPoint pt5 = new GeoPoint(lat, lon);

            //构建多边形
            Geometry polygonGeometry = new Geometry();
            //设置多边形坐标
            GeoPoint[] polygonPoints = new GeoPoint[5];
            polygonPoints[0] = pt1;
            polygonPoints[1] = pt2;
            polygonPoints[2] = pt3;
            polygonPoints[3] = pt4;
            polygonPoints[4] = pt5;
            polygonGeometry.SetPolygon(polygonPoints);
            //设置多边形样式
            Symbol polygonSymbol = new Symbol();
            Symbol.Color polygonColor = new Com.Baidu.Mapapi.Map.Symbol.Color(polygonSymbol);
            polygonColor.Red = 0;
            polygonColor.Green = 0;
            polygonColor.Blue = 255;
            polygonColor.Alpha = 126;
            polygonSymbol.SetSurface(polygonColor, 1, 5);
            //生成Graphic对象
            Graphic polygonGraphic = new Graphic(polygonGeometry, polygonSymbol);
            return polygonGraphic;
        }

        /**
         * 绘制单点，该点状态不随地图状态变化而变化
         * @return 点对象
         */
        public Graphic DrawPoint()
        {
            double mLat = 39.98923;
            double mLon = 116.397428;
            int lat = (int)(mLat * 1E6);
            int lon = (int)(mLon * 1E6);
            GeoPoint pt1 = new GeoPoint(lat, lon);

            //构建点
            Geometry pointGeometry = new Geometry();
            //设置坐标
            pointGeometry.SetPoint(pt1, 10);
            //设定样式
            Symbol pointSymbol = new Symbol();
            Symbol.Color pointColor = new Com.Baidu.Mapapi.Map.Symbol.Color(pointSymbol);
            pointColor.Red = 0;
            pointColor.Green = 126;
            pointColor.Blue = 255;
            pointColor.Alpha = 255;
            pointSymbol.SetPointSymbol(pointColor);
            //生成Graphic对象
            Graphic pointGraphic = new Graphic(pointGeometry, pointSymbol);
            return pointGraphic;
        }

        /**
         * 绘制圆，该圆随地图状态变化
         * @return 圆对象
         */
        public Graphic DrawCircle()
        {
            double mLat = 39.90923;
            double mLon = 116.447428;
            int lat = (int)(mLat * 1E6);
            int lon = (int)(mLon * 1E6);
            GeoPoint pt1 = new GeoPoint(lat, lon);

            //构建圆
            Geometry circleGeometry = new Geometry();

            //设置圆中心点坐标和半径
            circleGeometry.SetCircle(pt1, 2500);
            //设置样式
            Symbol circleSymbol = new Symbol();
            Symbol.Color circleColor = new Com.Baidu.Mapapi.Map.Symbol.Color(circleSymbol);
            circleColor.Red = 0;
            circleColor.Green = 255;
            circleColor.Blue = 0;
            circleColor.Alpha = 126;
            circleSymbol.SetSurface(circleColor, 1, 3, new Com.Baidu.Mapapi.Map.Symbol.Stroke(3, new Com.Baidu.Mapapi.Map.Symbol.Color(circleSymbol, Android.Graphics.Color.ParseColor("#FFFF0000").ToArgb())));
            //生成Graphic对象
            Graphic circleGraphic = new Graphic(circleGeometry, circleSymbol);
            return circleGraphic;
        }

        /**
         * 绘制文字，该文字随地图变化有透视效果
         * @return 文字对象
         */
        public TextItem DrawText()
        {
            double mLat = 39.86923;
            double mLon = 116.397428;
            int lat = (int)(mLat * 1E6);
            int lon = (int)(mLon * 1E6);
            //构建文字
            TextItem item = new TextItem();
            //设置文字位置
            item.Pt = new GeoPoint(lat, lon);
            //设置文件内容
            item.Text = "百度地图SDK";
            //设文字大小
            item.FontSize = 40;
            Symbol symbol = new Symbol();
            Symbol.Color bgColor = new Com.Baidu.Mapapi.Map.Symbol.Color(symbol);
            //设置文字背景色
            bgColor.Red = 0;
            bgColor.Blue = 0;
            bgColor.Green = 255;
            bgColor.Alpha = 50;

            Symbol.Color fontColor = new Com.Baidu.Mapapi.Map.Symbol.Color(symbol);
            //设置文字着色
            fontColor.Alpha = 255;
            fontColor.Red = 0;
            fontColor.Green = 0;
            fontColor.Blue = 255;
            //设置对齐方式
            item.Align = TextItem.AlignCenter;
            //设置文字颜色和背景颜色
            item.FontColor = fontColor;
            item.BgColor = bgColor;
            return item;
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