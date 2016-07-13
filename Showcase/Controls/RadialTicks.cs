using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Showcase {

    public class RadialTicks : FrameworkElement {

        #region dps

        public static readonly DependencyProperty BrushProperty = DependencyProperty.Register(
            nameof(RadialTicks.Brush),
            typeof(Brush),
            typeof(RadialTicks),
            new PropertyMetadata(new SolidColorBrush(Colors.Gray),
                new PropertyChangedCallback(OnAppearancePropertyChanged)));

        public Brush Brush {
            get { return (Brush)base.GetValue(BrushProperty); }
            set { base.SetValue(BrushProperty, value); }
        }

        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(
            nameof(RadialTicks.Thickness),
            typeof(double),
            typeof(RadialTicks),
            new PropertyMetadata(1.0d,
                new PropertyChangedCallback(OnAppearancePropertyChanged)));

        public double Thickness {
            get { return (double)base.GetValue(ThicknessProperty); }
            set { base.SetValue(ThicknessProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            nameof(RadialTicks.Radius),
            typeof(double),
            typeof(RadialTicks),
            new PropertyMetadata(10.0d,
                new PropertyChangedCallback(OnAppearancePropertyChanged)));

        public double Radius {
            get { return (double)base.GetValue(RadiusProperty); }
            set { base.SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty TickLengthProperty = DependencyProperty.Register(
            nameof(RadialTicks.TickLength),
            typeof(double),
            typeof(RadialTicks),
            new PropertyMetadata(8.0d,
                new PropertyChangedCallback(OnAppearancePropertyChanged)));

        public double TickLength {
            get { return (double)base.GetValue(TickLengthProperty); }
            set { base.SetValue(TickLengthProperty, value); }
        }

        public static readonly DependencyProperty TicksCountProperty = DependencyProperty.Register(
            nameof(RadialTicks.TicksCount),
            typeof(int),
            typeof(RadialTicks),
            new PropertyMetadata(20,
                new PropertyChangedCallback(OnAppearancePropertyChanged)));

        public int TicksCount {
            get { return (int)base.GetValue(TicksCountProperty); }
            set { base.SetValue(TicksCountProperty, value); }
        }

        #endregion

        protected override void OnRender(DrawingContext drawingContext) {
            this.Render(drawingContext);
        }

        void Render(DrawingContext dc) {
            int ticksCount = this.TicksCount;
            double step = Math.PI * 2 / ticksCount;
            double radius = this.Radius;
            double innerRadius = radius - this.TickLength;
            double centerX = this.ActualWidth * 0.5;
            double centerY = this.ActualHeight * 0.5;
            var pen = new Pen(this.Brush, this.Thickness);
            for (int i = 0; i < ticksCount; i++) {
                double angle = step * i;
                double cosAngle = Math.Cos(angle);
                double sinAngle = Math.Sin(angle);
                Point startPoint = new Point(centerX + radius * cosAngle, centerY + radius * sinAngle);
                Point endPoint = new Point(centerX + innerRadius * cosAngle, centerY + innerRadius * sinAngle);
                dc.DrawLine(pen, startPoint, endPoint);
            }
        }

        static void OnAppearancePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs args) {
            var scales = o as RadialTicks;
            scales.InvalidateVisual();
        }
    }
}
