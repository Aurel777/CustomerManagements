namespace CustomerManagement.Model.Extensions
{
    using System;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;
    using System.Windows;
    using System.Drawing;
    using System.Windows.Threading;

    public static class ExtensionMethods
    {
        static readonly Action EmptyDelegate = delegate () { };


        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }

        public static BitmapSource  ToImageSource(this Bitmap bitmap)
        {
            BitmapSource bitmapSource = null;
            if (bitmap == null)
                return null;

            var handle = bitmap.GetHbitmap();
            try
            {
                bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                NativeMethods.NativeMethods.DeleteObject(handle);
            }
            return bitmapSource;
        }
    }
}
