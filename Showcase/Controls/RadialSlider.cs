using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Showcase.Converters;

namespace Showcase {
    [TemplatePart(Name = "PART_ticks", Type = typeof(RadialTicks))]
    [TemplatePart(Name = "PART_labels", Type = typeof(RadialPanel))]
    [TemplatePart(Name = "PART_outer", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_inner", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_arrow", Type =typeof(FrameworkElement))]
    public class RadialSlider : Control {
        static RadialSlider() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadialSlider), new FrameworkPropertyMetadata(typeof(RadialSlider)));
        }

        RadialTicks ticks;
        RadialPanel labels;
        FrameworkElement outer;
        FrameworkElement inner;
        FrameworkElement arrow;
        TranslateTransform arrowTranslation;
        RotateTransform arrowRotation;

        #region dps

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            nameof(RadialSlider.Angle),
            typeof(double),
            typeof(RadialSlider),
            new PropertyMetadata(0.0d,
                new PropertyChangedCallback(OnAnglePropertyChanged)));

        public double Angle {
            get { return (double)base.GetValue(AngleProperty); }
            set { base.SetValue(AngleProperty, value); }
        }

        public static readonly DependencyProperty TicksRadiusProperty = DependencyProperty.Register(
            nameof(RadialSlider.TicksRadius),
            typeof(double),
            typeof(RadialSlider),
            new PropertyMetadata(0.77,
                new PropertyChangedCallback(OnAppearancePropertyChanged)));

        public double TicksRadius {
            get { return (double)base.GetValue(TicksRadiusProperty); }
            set { base.SetValue(TicksRadiusProperty, value); }
        }

        public static readonly DependencyProperty LabelsRadiusProperty = DependencyProperty.Register(
            nameof(RadialSlider.LabelsRadius),
            typeof(double),
            typeof(RadialSlider),
            new PropertyMetadata(0.86,
                new PropertyChangedCallback(OnAppearancePropertyChanged)));

        public double LabelsRadius {
            get { return (double)base.GetValue(LabelsRadiusProperty); }
            set { base.SetValue(LabelsRadiusProperty, value); }
        }

        public static readonly DependencyProperty OuterRadiusProperty = DependencyProperty.Register(
            nameof(RadialSlider.OuterRadius),
            typeof(double),
            typeof(RadialSlider),
            new PropertyMetadata(0.61,
                new PropertyChangedCallback(OnAppearancePropertyChanged)));

        public double OuterRadius {
            get { return (double)base.GetValue(OuterRadiusProperty); }
            set { base.SetValue(OuterRadiusProperty, value); }
        }

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
            nameof(RadialSlider.InnerRadius),
            typeof(double),
            typeof(RadialSlider),
            new PropertyMetadata(0.43,
                new PropertyChangedCallback(OnAppearancePropertyChanged)));

        public double InnerRadius {
            get { return (double)base.GetValue(InnerRadiusProperty); }
            set { base.SetValue(InnerRadiusProperty, value); }
        }

        public static readonly DependencyProperty TicksBrushProperty = DependencyProperty.Register(
            nameof(RadialSlider.TicksBrush),
            typeof(Brush),
            typeof(RadialSlider),
            new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public Brush TicksBrush {
            get { return (Brush)base.GetValue(TicksBrushProperty); }
            set { base.SetValue(TicksBrushProperty, value); }
        }

        public static readonly DependencyProperty LabelsBrushProperty = DependencyProperty.Register(
            nameof(RadialSlider.LabelsBrush),
            typeof(Brush),
            typeof(RadialSlider),
            new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public Brush LabelsBrush {
            get { return (Brush)base.GetValue(LabelsBrushProperty); }
            set { base.SetValue(LabelsBrushProperty, value); }
        }

        public static readonly DependencyProperty TickLengthProperty = DependencyProperty.Register(
            nameof(RadialSlider.TickLength),
            typeof(double),
            typeof(RadialSlider),
            new PropertyMetadata(6.0d));

        public double TickLength {
            get { return (double)base.GetValue(TickLengthProperty); }
            set { base.SetValue(TickLengthProperty, value); }
        }


        public static readonly DependencyProperty OuterBrushProperty = DependencyProperty.Register(
            nameof(RadialSlider.OuterBrush),
            typeof(Brush),
            typeof(RadialSlider),
            new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public Brush OuterBrush {
            get { return (Brush)base.GetValue(OuterBrushProperty); }
            set { base.SetValue(OuterBrushProperty, value); }
        }

        public static readonly DependencyProperty InnerBrushProperty = DependencyProperty.Register(
            nameof(RadialSlider.InnerBrush),
            typeof(Brush),
            typeof(RadialSlider),
            new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public Brush InnerBrush {
            get { return (Brush)base.GetValue(InnerBrushProperty); }
            set { base.SetValue(InnerBrushProperty, value); }
        }

        public static readonly DependencyProperty ArrowBrushProperty = DependencyProperty.Register(
            nameof(RadialSlider.ArrowBrush),
            typeof(Brush),
            typeof(RadialSlider),
            new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public Brush ArrowBrush {
            get { return (Brush)base.GetValue(ArrowBrushProperty); }
            set { base.SetValue(ArrowBrushProperty, value); }
        }

        public static readonly DependencyProperty AngleFontSizeProperty = DependencyProperty.Register(
            nameof(RadialSlider.AngleFontSize),
            typeof(double),
            typeof(RadialSlider),
            new PropertyMetadata(20.0));

        public double AngleFontSize {
            get { return (double)base.GetValue(AngleFontSizeProperty); }
            set { base.SetValue(AngleFontSizeProperty, value); }
        }

        #endregion

        #region handlers

        static void OnAppearancePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs args) {
            var scales = o as RadialTicks;
            scales.InvalidateVisual();
        }

        static void OnAnglePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs args) {
            var scales = o as RadialSlider;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            this.UpdateAngle(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed ||
                e.RightButton == MouseButtonState.Pressed) {
                this.UpdateAngle(e);
            }
        }

        #endregion

        protected override Size MeasureOverride(Size constraint) {
            double width = 0.0;
            if (!constraint.IsEmpty) {
                width = Math.Min(
                    double.IsInfinity(constraint.Width) ? constraint.Height : constraint.Width,
                    double.IsInfinity(constraint.Height) ? constraint.Width : constraint.Height);

                this.AdjustControlParts(new Size(width, width));
            }
            return base.MeasureOverride(new Size(width, width));
        }

        protected override Size ArrangeOverride(Size arrangeBounds) {
            double width = Math.Min(arrangeBounds.Width, arrangeBounds.Height);
            return base.ArrangeOverride(new Size(width, width));
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
            this.ticks = (RadialTicks)base.Template.FindName("PART_ticks", this);
            this.labels = (RadialPanel)base.Template.FindName("PART_labels", this);
            this.outer = (FrameworkElement)base.Template.FindName("PART_outer", this);
            this.inner = (FrameworkElement)base.Template.FindName("PART_inner", this);
            this.arrow = (FrameworkElement)base.Template.FindName("PART_arrow", this);
            this.arrowTranslation = new TranslateTransform();
            this.arrowRotation = new RotateTransform();
            var angleBinding = new Binding {
                Source = this,
                Path = new PropertyPath(RadialSlider.AngleProperty),
                Mode = BindingMode.OneWay,
                Converter = new DoubleNegationConverter()
            };
            BindingOperations.SetBinding(
                this.arrowRotation,
                RotateTransform.AngleProperty,
                angleBinding);
            var group = new TransformGroup();
            group.Children.Add(arrowTranslation);
            group.Children.Add(arrowRotation);
            this.arrow.RenderTransform = group;
        }


        void AdjustControlParts(Size size) {
            Debug.WriteLine("AdjustControlParts: " + size);
            double fullRadius = Math.Min(size.Width, size.Height) * 0.5;
            this.ticks.Radius = this.TicksRadius * fullRadius;
            this.labels.Radius = this.LabelsRadius * fullRadius;
            this.outer.Width = this.OuterRadius * fullRadius * 2.0;
            this.outer.Height = this.OuterRadius * fullRadius * 2.0;
            this.inner.Width = this.InnerRadius * fullRadius * 2.0;
            this.inner.Height = this.InnerRadius * fullRadius * 2.0;
            this.arrowTranslation.X = fullRadius + this.InnerRadius * fullRadius;
            this.arrowTranslation.Y = fullRadius;
            this.arrowRotation.CenterX = fullRadius;
            this.arrowRotation.CenterY = fullRadius;
        }

        void UpdateAngle(MouseEventArgs e) {
            Point position = e.GetPosition(this);
            double x = position.X - this.ActualWidth * 0.5;
            double y = -position.Y + this.ActualHeight * 0.5;
            double angle = Math.Atan2(y, x);
            this.Angle = angle / Math.PI * 180;
        }
    }
}
