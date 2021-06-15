using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blendmode
{
    public struct Pixel
    {
        static public byte ToByte(double x)
        {
            if (x < 0)
                return 0;
            if (x > 255)
                return 255;
            return (byte)x;

        }

        public static Pixel None = new Pixel(0, 0, 0);
        public static Pixel Max = new Pixel(255, 255, 255);

        public static BigPixel operator +(Pixel a, Pixel b) => (BigPixel)a + (BigPixel)b;
        public static BigPixel operator -(Pixel a, Pixel b) => (BigPixel)a - (BigPixel)b;
        public static BigPixel operator *(Pixel a, Pixel b) => (BigPixel)a * (BigPixel)b;
        public static BigPixel operator /(Pixel a, Pixel b) => (BigPixel)a / (BigPixel)b ;

        public static BigPixel operator +(Pixel a, double b) => (BigPixel)a + b;
        public static BigPixel operator -(Pixel a, double b) => (BigPixel)a - b;
        public static BigPixel operator *(Pixel a, double b) => (BigPixel)a * b;
        public static BigPixel operator *(double a, Pixel b) => (BigPixel)b * a;
        public static BigPixel operator /(Pixel a, double b) => (BigPixel)a / b;

        public static bool operator >(Pixel a, Pixel b)
        {
            var av = a.R + a.G + a.B;
            var bv = b.R + b.G + b.B;

            return av > bv;
        }
        public static bool operator <(Pixel a, Pixel b) => b > a;

        public static bool operator >(Pixel a, double b)
        {
            // dodaję +1 do sumy wartości a potem odejmuje 1/765. Jest to zabezpieczenie gdyby suma wartości RGB wynosiła 0;
            var av = (a.R + a.G + a.B + 1) / 765.0 - 1 / 765.0;
            return av > b;
        }

        public static bool operator <(Pixel a, double b)
        {
            // dodaję +1 do sumy wartości a potem odejmuje 1/765. Jest to zabezpieczenie gdyby suma wartości RGB wynosiła 0;
            var av = (a.R + a.G + a.B +1) / 765.0 - 1 / 765.0;
            return av < b;
        }


        public static implicit operator BigPixel(Pixel p) => new BigPixel(p.R, p.G, p.B);

        public override string ToString()
        {
            return $"Pixel[R:{R} G:{G} B:{B}]";
        }

        public Pixel(byte R, byte G, byte B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }



    // ta struktura słóży do wykonywania działań arytmetycznych
    public struct BigPixel
    {
        public static BigPixel operator +(BigPixel a, BigPixel b)
            => new BigPixel( a.R + b.R, a.G + b.G, a.B + b.B);
        

        public static BigPixel operator -(BigPixel a, BigPixel b)
            => new BigPixel(a.R - b.R, a.G - b.G, a.B - b.B);
        

        public static BigPixel operator *(BigPixel a, BigPixel b)
        {
            // Każdy kolor musi zostać podzielony przez 255 ( ponieważ wzory są podane w zakresie od 0 do 1 a kolory w zakresie od 0 do 255 )
            var red = a.R * b.R / 255.0;
            var green = a.G * b.G / 255.0;
            var blue = a.B * b.B / 255.0;

            return new BigPixel(red, green, blue);
        }

        public static BigPixel operator /(BigPixel a, BigPixel b)
        {
            // Każdy kolor musi zostać przemnożony przez 255 ( ponieważ wzory są podane w zakresie od 0 do 1 a kolory w zakresie od 0 do 255 )
            var red = a.R / b.R * 255.0;
            var green = a.G / b.G * 255.0;
            var blue = a.B / b.B * 255.0;

            return new BigPixel(red, green, blue);
        }


        public static BigPixel operator +(BigPixel a, Pixel b) => a + (BigPixel)b;
        public static BigPixel operator -(BigPixel a, Pixel b) => a - (BigPixel)b;
        public static BigPixel operator -(Pixel a, BigPixel b) => (BigPixel)a - b;
        public static BigPixel operator *(BigPixel a, Pixel b) => a * (BigPixel)b;
        public static BigPixel operator /(BigPixel a, Pixel b) => a / (BigPixel)b;
        public static BigPixel operator /(Pixel a, BigPixel b) => (BigPixel)a / b;
        public static BigPixel operator +(BigPixel a, double b) => new BigPixel(a.R + b, a.G + b, a.B + b);
        public static BigPixel operator -(BigPixel a, double b) => new BigPixel(a.R - b, a.G - b, a.B - b);


        public static BigPixel operator *(BigPixel a, double b)
        {
            var max = Pixel.Max;
            var bb = new BigPixel(max.R * b, max.G * b, max.B * b);
            return a * bb;
        }
        public static BigPixel operator *(double a, BigPixel b) => b * a;


        public static BigPixel operator /(BigPixel a, double b)
        {
            var max = Pixel.Max;
            var bb = new BigPixel(max.R / b, max.G / b, max.B / b);
            return a / bb;
        }


        public static BigPixel ABS(BigPixel v)
        {
            if ((v.R + v.G + v.B) < 0)
                return v * -1.0;
            return v;
        }


        public static BigPixel SQRT(BigPixel v) 
            => new BigPixel((int)Math.Sqrt(v.R), (int)Math.Sqrt(v.G), (int)Math.Sqrt(v.B));


        public static implicit operator Pixel(BigPixel bp) 
            => new Pixel(Pixel.ToByte(bp.R), Pixel.ToByte(bp.G), Pixel.ToByte(bp.B));

        public BigPixel(double R, double G, double B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }
    }
}
