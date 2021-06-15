using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace blendmode
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // getery i setery ustawiają opis obrazka
        private ImagePixels _pix1, _pix2, _pix3;
        private ImagePixels pix1
        {
            get => _pix1;
            set
            {
                _pix1 = value;
                img1Description.Text = $"{value.Width} x {value.Height} px";
            }
        }

        private ImagePixels pix2
        {
            get => _pix2;
            set
            {
                _pix2 = value;
                img2Description.Text = $"{value.Width} x {value.Height} px";
            }
        }

        private ImagePixels pix3
        {
            get => _pix3;
            set
            {
                _pix3 = value;
                img3Description.Text = $"{value.Width} x {value.Height} px";
            }
        }

        private string dir, mode;



        public MainWindow()
        {
            InitializeComponent();  
        }


        // otwarcie plików
        private void open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();
            openDialog.InitialDirectory = (dir == string.Empty) ? Directory.GetCurrentDirectory() : dir;
            openDialog.Filter = "obrazki|*.jpg";
            openDialog.CheckFileExists = true;
            openDialog.Multiselect = false;

            if (openDialog.ShowDialog() == true)
            {
                dir = openDialog.FileName;
                var img = new BitmapImage(new Uri(dir));
                var pix = new ImagePixels(img);

                if ((sender as Button).Name == "open1")
                {
                    img1.Source = img;
                    pix1 = pix;
                }
                else
                {
                    img3.Source = img;
                    pix3 = pix;
                }

                if (pix1 != null && pix3 != null)
                {
                    if (combo.IsEnabled == false) combo.IsEnabled = true;

                    if (combo.SelectedItem != null)
                        img2.Source = (pix2 = Blend(pix1, pix3, GetBlendFn(mode) )).ToBitmapSource();
                }
            }
        }

        private void AlfaSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if(mode == "Transparency")
                img2.Source = (pix2 = Blend(pix1, pix3, GetBlendFn(mode))).ToBitmapSource();
        }

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mode = (e.AddedItems[0] as UIElement).Uid;
            pix2 = Blend(pix1, pix3, GetBlendFn(mode));
            img2.Source = pix2.ToBitmapSource();
        }

        // funkcja blend miksuje obraz na podstawie dwoch obrazow i funkcji blendujacej i zwraca nowy obraz
        private ImagePixels Blend (ImagePixels source, ImagePixels mask, Func<Pixel, Pixel, Pixel> blendFn)
        {
            var w = (source.Width < mask.Width) ? source.Width : mask.Width;
            var h = (source.Height < mask.Height) ? source.Height : mask.Height;

            var result = new ImagePixels(w, h);

            result.ForEach((x, y) => {
                var sourcePixel = source.GetPixel(x, y);
                var maskPixel = mask.GetPixel(x, y);
                return blendFn(sourcePixel, maskPixel);
            });

            return result;
        }


        // ta funkcja pobiera funkcję z statycznej klasy BlemdsModes. Nie chciało mi sie używać nudnego switcha
        private Func<Pixel, Pixel, Pixel> GetBlendFn(string fnName)
        {
            var fn = typeof(BlendsMods).GetMethod(fnName);

            if(fn != null && fn.IsPublic && fn.ReturnType == typeof(Pixel))
            {
                var args = fn.GetParameters();

                if (args.Length == 2 && args[0].ParameterType == typeof(Pixel) && args[1].ParameterType == typeof(Pixel))
                    return (Pixel s1, Pixel s2) => (Pixel)fn.Invoke(null, new object[] { s1, s2 });

                if (args.Length == 0)
                    return (Pixel s1, Pixel s2) => (Pixel)fn.Invoke(null, new object[] { } ); ;

                if (args.Length == 3 && args[0].ParameterType == typeof(Pixel) && args[1].ParameterType == typeof(Pixel) && args[2].ParameterType == typeof(byte))
                    return (Pixel s1, Pixel s2) => (Pixel)fn.Invoke(null, new object[] { s1, s2, (byte)AlfaSlider.Value });
            }

            return (Pixel s1, Pixel s2) => Pixel.Max;
        }
    }
}
