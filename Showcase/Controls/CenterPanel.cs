using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Showcase {
    public class CenterPanel : Panel {
        public static readonly DependencyProperty CentralProperty =
            DependencyProperty.RegisterAttached(
                "Central",
                typeof(bool), 
                typeof(CenterPanel),
                new PropertyMetadata(false));
        
        public static void SetCentral(UIElement element, bool value) {
            element.SetValue(CentralProperty, value);
        }

        public static bool GetCentral(UIElement element) {
            return (bool)element.GetValue(CentralProperty);
        }

        protected override Size MeasureOverride(Size availableSize) {
            double totalWidth = 0.0;
            double maxHeight = 0.0;
            foreach (UIElement item in base.Children) {
                item.Measure(availableSize);
                totalWidth += item.DesiredSize.Width;
                if (item.DesiredSize.Height > maxHeight) {
                    maxHeight = item.DesiredSize.Height;
                }
            }
            return new Size(totalWidth, maxHeight);
        }

        protected override Size ArrangeOverride(Size finalSize) {
            if (base.Children.Count > 0) {
                // find central element
                var children = base.Children.Cast<UIElement>().ToList();
                var central = children.Single(e => GetCentral(e));
                int index = children.IndexOf(central);
                // place central element
                double offset = finalSize.Width * 0.5 - central.DesiredSize.Width * 0.5;
                central.Arrange(new Rect(
                    offset,
                    0,
                    central.DesiredSize.Width,
                    finalSize.Height));
                // left neighbors
                for (int i = index - 1; i > -1; i--) {
                    var item = children[i];
                    offset -= item.DesiredSize.Width;
                    item.Arrange(new Rect(
                        offset,
                        0,
                        item.DesiredSize.Width,
                        finalSize.Height));
                }
                // right neighbors
                offset = finalSize.Width * 0.5 + central.DesiredSize.Width * 0.5;
                for (int i = index + 1; i < children.Count; i++) {
                    var item = children[i];
                    item.Arrange(new Rect(
                        offset,
                        0,
                        item.DesiredSize.Width,
                        finalSize.Height));
                    offset += item.DesiredSize.Width;
                }
            }
            return finalSize;
        }
    }
}
