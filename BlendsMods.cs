using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blendmode
{
    static class BlendsMods
    {
        // wszystkie wzory z książki + funkcja None która zwraca czarny pixel

        public static Pixel None() => Pixel.None;
        

        public static Pixel Additive(Pixel s1, Pixel s2) =>s1 + s2;


        public static Pixel Substractive(Pixel s1, Pixel s2) => s1 + s2 - Pixel.Max;
        

        public static Pixel Difference(Pixel s1, Pixel s2) =>  BigPixel.ABS(s1 - s2);
        

        public static Pixel Multiplay(Pixel s1, Pixel s2) =>  s1 * s2;
        

        public static Pixel Screen(Pixel s1, Pixel s2) 
            => Pixel.Max - (Pixel.Max - s1) * (Pixel.Max - s2);


        public static Pixel Negation(Pixel s1, Pixel s2)
            => Pixel.Max - BigPixel.ABS(Pixel.Max - s1 - s2);


        public static Pixel Darken(Pixel s1, Pixel s2) => s1 < s2 ? s1 : s2;


        public static Pixel Lighten(Pixel s1, Pixel s2) => s1 > s2 ? s1 : s2;



        public static Pixel Exclusion(Pixel s1, Pixel s2) => s1 + s2 - 2 * s1 * s2;


        public static Pixel Overlay(Pixel s1, Pixel s2) 
            => s1 < 0.5 ? 2 * s1 * s2 : Pixel.Max - 2 * (Pixel.Max - s1) * (Pixel.Max - s2);


        public static Pixel HardLight(Pixel s1, Pixel s2) 
            => s2 < 0.0 ? 2 * s1 * s2 : Pixel.Max - 2 * ((Pixel.Max - s1) * (Pixel.Max - s2));


        public static Pixel SoftLight(Pixel s1, Pixel s2)
            => s2< 0.5 ? 2 * s1* s2 + s1* s1* (Pixel.Max - 2 * s2) 
            : BigPixel.SQRT(s1) * (2 * s2 - Pixel.Max) + (2 * s1) * (Pixel.Max - s2);


        public static Pixel ColorDoge(Pixel s1, Pixel s2) => s1 / (Pixel.Max - s2);


        public static Pixel ColorBurn(Pixel s1, Pixel s2) => Pixel.Max - (Pixel.Max - s1) / s2;


        public static Pixel Reflect(Pixel s1, Pixel s2) => s1 * s1 / (Pixel.Max - s2);


        public static Pixel Transparency(Pixel s1, Pixel s2, byte alfa) => (255 - alfa) / 255.0 * s2 + alfa / 255.0 * s1;
    }
}
