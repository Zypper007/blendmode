using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace blendmode
{
    public class ImagePixels
    {
        readonly public int Width, Height;
        readonly private int Stride, BPS;
        readonly private double DpiY, DpiX;
        readonly private byte[] Bits;
        readonly public PixelFormat Format;
        readonly public BitmapPalette Palette;

        public ImagePixels(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;

            DpiX = 96;
            DpiY = 96;

            Format = PixelFormats.Bgr24;
            Palette = null;

            Stride = (this.Width * Format.BitsPerPixel + 7) / 8;
            BPS = Format.BitsPerPixel / 8;

            Bits = new byte[Stride * Height];
            ForEach(() => Pixel.None);
            
        }

        // Konstruktor z BitmapySource
        public ImagePixels(BitmapSource imgSrc)
        {
            Width = imgSrc.PixelWidth;
            Height = imgSrc.PixelHeight;
            DpiY = imgSrc.DpiY;
            DpiX = imgSrc.DpiX;
            Format = imgSrc.Format;
            Palette = imgSrc.Palette;
            Stride = (Width * Format.BitsPerPixel + 7) / 8;
            BPS = Format.BitsPerPixel / 8;
            Bits = new byte[Stride * Height];
            imgSrc.CopyPixels(Bits, Stride, 0);
        }

           
        // Konstruktor Kopiujący
        public ImagePixels(ImagePixels source)
        {
            Width = source.Width;
            Height = source.Height;
            DpiX = source.DpiX;
            DpiY = source.DpiY;
            BPS = source.BPS;
            Stride = source.Stride;
            Format = source.Format;
            Palette = source.Palette;
            Bits = new byte[Stride * Height];
            source.Bits.CopyTo(Bits, 0);
        }
       

        // Konwersja do BitmapSource
        public BitmapSource ToBitmapSource()
        {
            var img = BitmapSource.Create(Width, Height, DpiX, DpiY, Format, Palette, Bits, Stride);
            img.Freeze();

            return img;
        }

        // słóży do zerowania tablicy pixeli
        public void ForEach(Func<Pixel> func)
        {
            for (long y = 0; y < Height; y++)
                for (long x = 0; x < Width; x++)
                    SetPixel(x, y, func());
        }

        // pobiera pixel do funkcji i zapisuje go w tym samym miejscu
        public void ForEach(Func<Pixel, Pixel> func)
        {
            for (long y = 0; y < Height; y++)
                for (long x = 0; x < Width; x++)
                    SetPixel(x, y, func(GetPixel(x, y)));
        }

        // udostępnia pozycje i zapisuje pixel
        public void ForEach(Func<long, long, Pixel> func)
        {
            for (long y = 0; y < Height; y++)
                for (long x = 0; x < Width; x++)
                    SetPixel(x, y, func(x, y));
        }

        private long CalcualteStep(long x, long y)
        {
            // tutaj powinnien być switch z wyborem formatu ale ja się ograniczyłem tylko do ARGB
            // ile jest bajtów na pixel
            /*
                W każdym wierszu jest Width pixeli a kazdy pixel to BPS bitów więc w każdym wierszu może być y*Width*BPS bajtów;
                Do tego trzeba dodać przesunięcie o x ale każdy pixel to BPS bajtów więc dodać x*BPS
             */


            return y * Width * BPS + x * BPS;
        }


        public Pixel GetPixel(long x, long y)
        {
            var step = CalcualteStep(x, y);

            return new Pixel(Bits[step + 2], Bits[step + 1], Bits[step]);
        }

        public void SetPixel(long x, long y, Pixel color)
        {
            var step = CalcualteStep(x, y);

            Bits[step] = color.B;
            Bits[step + 1] = color.G;
            Bits[step + 2] = color.R;
        }
    }
}
