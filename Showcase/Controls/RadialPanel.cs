using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Showcase {

    public class RadialPanel : Panel {
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            nameof(RadialPanel.Radius),
            typeof(double),
            typeof(RadialPanel),
            new PropertyMetadata(10.0d,
                new PropertyChangedCallback(OnAppearancePropertyChanged)));

        public double Radius {
            get { return (double)base.GetValue(RadiusProperty); }
            set { base.SetValue(RadiusProperty, value); }
        }

        static void OnAppearancePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs args) {
            var scales = o as RadialPanel;
            scales.InvalidateArrange();
        }

        protected override Size MeasureOverride(Size availableSize) {
            var anySize = new Size(double.PositiveInfinity, double.PositiveInfinity);
            foreach (UIElement item in base.Children) {
                item.Measure(anySize);
            }
            return new Size(0, 0);
        }

        protected override Size ArrangeOverride(Size finalSize) {
            int count = base.Children.Count;
            double step = Math.PI * 2 / count;
            double centerX = finalSize.Width * 0.5;
            double centerY = finalSize.Height * 0.5;
            double radius = this.Radius;
            double angle = 0.0;
            foreach (UIElement item in base.Children) {
                double width = item.DesiredSize.Width;
                double height = item.DesiredSize.Height;
                double cosAngle = Math.Cos(angle);
                double sinAngle = Math.Sin(angle);
                double x = centerX + cosAngle * radius;
                double y = centerY + sinAngle * radius - height * 0.5;
                // vertical and left exceptions
                if (Math.Abs(cosAngle) < 0.1) {
                    x -= width * 0.5;
                }
                else if (cosAngle < 0.0) {
                    x -= width;
                }
                item.Arrange(new Rect(x, y, width, height));
                angle -= step;
            }
            return finalSize;
        }
    }
}
