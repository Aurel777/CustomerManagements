using System.IO;
using System.Windows.Media.Imaging;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Input;
using CustomerManagement.Model;
using CustomerManagement.ViewModel;

namespace CustomerManagement.View
{
    public partial class AboutWindow : Window
    {
        #region Fields

        string scrollingText;
        SinusoidalScrollingText sinusoidalScrollingText;
        DispatcherTimer dispatcherTimerScroll;
        DispatcherTimer dispatcherTimerRender;
        readonly AboutViewModel viewModel;

        #endregion Fields

        public AboutWindow()
        {
            InitializeComponent();

            viewModel = new AboutViewModel();
            DataContext = viewModel;
            Initialize();
        }

        
        void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                Close();
        }

        void Worker_DoWork(object sender, DoWorkEventArgs e) => sinusoidalScrollingText?.UpdateBitmap();

        void DispatcherTimerScroll_Tick(object sender, EventArgs e) => sinusoidalScrollingText.X += 3;

        void DispatcherTimerRender_Tick(object sender, EventArgs e)
        {
            sinusoidalScrollingText?.UpdateBitmap();
            viewModel.Image = sinusoidalScrollingText.WindowBitmap;
        }
        
        void Initialize()
        {
            scrollingText = @"Baby girl, all my girls, all my girls Sean da Paul sey, Well woman the way the time cold I wanna be keepin' you warm I got the right temperature to shelter you from the storm Oh lord, gal I got the right tactics to turn you on, and girl I, Wanna be the Papa, You can be the Mom, oh oh!";

            using (var font = new Font(System.Drawing.FontFamily.GenericSansSerif, 32F, System.Drawing.FontStyle.Regular))
            {
                var graphics = Graphics.FromHwnd(new System.Windows.Interop.WindowInteropHelper(this).Handle);
                sinusoidalScrollingText = new SinusoidalScrollingText(graphics, scrollingText, font, new System.Drawing.Size((int)Width, (int)Height), Color.Transparent, Color.RoyalBlue);
            }

            dispatcherTimerScroll = new DispatcherTimer {  Interval = TimeSpan.FromMilliseconds(15) };
            dispatcherTimerScroll.Tick += DispatcherTimerScroll_Tick;
            dispatcherTimerScroll.Start();
            dispatcherTimerRender = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
            dispatcherTimerRender.Tick += DispatcherTimerRender_Tick;
            dispatcherTimerRender.Start();
        }
    }
}
