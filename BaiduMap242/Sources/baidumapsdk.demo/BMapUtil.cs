using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using baidumapsdk.demo;
using Com.Baidu.Mapapi;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Platform.Comapi.Basestruct;

namespace baidumapsdk.demo
{
    public class BMapUtil
    {
        /**
	     * ´Óview µÃµ½Í¼Æ¬
	     * @param view
	     * @return
	     */
        public static Bitmap GetBitmapFromView(View view)
        {
            view.DestroyDrawingCache();
            view.Measure(View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
                View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
            view.Layout(0, 0, view.MeasuredWidth, view.MeasuredHeight);
            view.DrawingCacheEnabled = true;
            Bitmap bitmap = view.GetDrawingCache(true);
            return bitmap;
        }
    }
}