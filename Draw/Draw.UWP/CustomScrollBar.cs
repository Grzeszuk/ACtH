using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Draw.UWP;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;


[assembly: ExportRenderer(typeof(ScrollView), typeof(CustomScrollBar))]

namespace Draw.UWP
{
    class CustomScrollBar : ScrollViewRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Control.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            Control.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }
    }
}
