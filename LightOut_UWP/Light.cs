using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;

namespace LightOut_UWP
{
    public class Light : Button
    {
        public bool lightOn { get; set; }
        public Color onColor = Colors.Aqua;
        public Color offColor = Colors.Black;
        public bool ClickEnabled { get; set; } = true;

        public int[] position = new int[2]; // [row, col]

        public Light(bool lightOn)
        {
            //DefaultStyleKey = typeof(Light);
            this.lightOn = lightOn;

            // Styles
            BorderBrush = new SolidColorBrush(Colors.White);
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            this.Style = Application.Current.Resources["LightStyle2"] as Style;

            this.Translation += new System.Numerics.Vector3(0, 0, 16);


            // Events
            Click += LightOnClick;
            //this.PointerEntered += OnPointerEntered;

            if (lightOn)
            {
                Background = new SolidColorBrush(onColor);
            }
            else
            {
                Background = new SolidColorBrush(offColor);
            }

        }

        void LightOnClick(Object sender, RoutedEventArgs e)
        {
            if (ClickEnabled)
            {
                ToggleLight();
            }  
        }

        public void ToggleLight() // if on turen off, vice versa
        {
            if (lightOn)
            {
                this.Background = new SolidColorBrush(offColor);
                lightOn = false;
            }
            else
            {
                this.Background = new SolidColorBrush(onColor);
                lightOn = true;
            }
        }
    }
}
