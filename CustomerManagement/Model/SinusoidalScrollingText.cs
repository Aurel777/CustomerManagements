namespace CustomerManagement.Model
{
    #region Using Statements

    using Extensions;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.Windows.Media;
    using Point = System.Drawing.Point;
    using Size = System.Drawing.Size;
    using Brush = System.Drawing.Brush;
    using Color = System.Drawing.Color;
    using Pen = System.Drawing.Pen;
    using PixelFormat = System.Drawing.Imaging.PixelFormat;

    #endregion

    public sealed unsafe class SinusoidalScrollingText : IDisposable
    {
        #region Fields

        byte* bitmapPointer;
        bool disposed;

        readonly int width;
        readonly int height;
        int drawingWidth;
        int drawingHeight;
        string text;
        readonly Rectangle destinationRectangle;
        SizeF size;

        Bitmap workingBitmap;
        Bitmap stringBitmap;
        BitmapData bitmapData;
        Font font;
        readonly Graphics graphics;
        Color textColor;
        Color backGroundColor;
        readonly Brush backGroundBrush;
        readonly Brush textBrush;

        #endregion Fields

        #region Properties

        public int X { get; set; }
        public string Text { set => text = value; }
        
        public ImageSource WindowBitmap { get; set; }


        public Color TextColor { set => textColor = value; }
        public Color BackGroundColor { set => backGroundColor = value; }
        public Font Font { set => font = value; }
    
        #endregion Properties

        #region Constructor

        public SinusoidalScrollingText(Graphics graphics, string text, Font font, Size size, Color backGroundColor, Color textColor)
        {
            width = size.Width;
            height = size.Height;
            destinationRectangle = new Rectangle(Point.Empty, size);
            this.font = font;
            this.textColor = textColor;
            this.backGroundColor = backGroundColor;
            var txtPen = new Pen(this.textColor);
            textBrush = txtPen.Brush;
            txtPen.Dispose();
            var backGroundPen = new Pen(this.backGroundColor);
            backGroundBrush = backGroundPen.Brush;
            backGroundPen.Dispose();
            this.graphics = graphics;
            this.text = text;
            InitializeBitmap();
        }

        ~SinusoidalScrollingText()
        {
            Dispose();
        }

        #endregion Constructor

        #region Methods

        public void Dispose()
        {
            if (disposed)
                return;

            stringBitmap?.Dispose();
            workingBitmap?.Dispose();
            textBrush?.Dispose();
            backGroundBrush?.Dispose();
            font?.Dispose();
            graphics?.Dispose();
            GC.SuppressFinalize(this);
            disposed = true;
        }

        void InitializeBitmap()
        {
            size = graphics.MeasureString(text, font);
            drawingWidth = (int)size.Width;
            drawingHeight = (int)size.Height;
            stringBitmap = new Bitmap(drawingWidth, drawingHeight);
            var stringGraphics = Graphics.FromImage(stringBitmap);
            stringGraphics.FillRectangle(backGroundBrush, new Rectangle(Point.Empty, stringBitmap.Size));
            stringGraphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            stringGraphics.DrawString(text, font, textBrush, PointF.Empty);
            stringGraphics.Flush();
            bitmapData = stringBitmap.LockBits(new Rectangle(Point.Empty, stringBitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            bitmapPointer = (byte*)bitmapData.Scan0.ToPointer();
            UpdateBitmap();
        }

        public void UpdateBitmap()
        {
            var destinationX = 0 - X;
            workingBitmap = new Bitmap(width, height);
            var destinationGraphics = Graphics.FromImage(workingBitmap);
            destinationGraphics.FillRectangle(System.Drawing.Brushes.Transparent, destinationRectangle);
            destinationGraphics.Flush();
            var destinationData = workingBitmap.LockBits(destinationRectangle, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            var destinationPointer = (byte*)destinationData.Scan0.ToPointer();

            if (destinationX + stringBitmap.Width <= 0)
                X = -width;

            for (var i = 0; i <= drawingWidth; i++)
            {
                if (destinationX + i <= 0 || destinationX + i >= width)
                    continue;

                var destinationY = 10 + 32 + (int)(32 * Math.Sin((double)(destinationX + i) / 50));

                // horizontal skyline effect FFS !
                for (var j = 0; j < drawingHeight; j += 2)
                {
                    if (destinationY + j <= 0 || destinationY + j >= height)
                        continue;
                    if (i >= drawingWidth)
                        continue;

                    var sourcePointerValue = (i + drawingWidth * j) << 2;
                    var destinationPointerVal = (destinationX + i + width * (destinationY + j)) << 2;
                    destinationPointer[destinationPointerVal + 0] = bitmapPointer[sourcePointerValue + 0];
                    destinationPointer[destinationPointerVal + 1] = bitmapPointer[sourcePointerValue + 1];
                    destinationPointer[destinationPointerVal + 2] = bitmapPointer[sourcePointerValue + 2];
                    destinationPointer[destinationPointerVal + 3] = bitmapPointer[sourcePointerValue + 3];
                }
            }
            
            workingBitmap.UnlockBits(destinationData);
            //workingBitmap.MakeTransparent(Color.White);
            WindowBitmap = workingBitmap.ToImageSource();
        }

        #endregion Methods
    }
}