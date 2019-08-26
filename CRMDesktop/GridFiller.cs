using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CRMDesktop
{
    public static class GridFiller
    {
        public static void rapidFill(string[] strings, Grid g)
        {
            int i = 0;
            Brush c = ClientData.getGridColor();
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Pixel) });
            foreach (string s in strings)
            {
                Rectangle b = new Rectangle() { Fill = c, Margin = ClientData.GridMargin };
                Label l = new Label() { Content = s, Foreground = ClientData.textColor };
                g.Children.Add(b);
                Grid.SetColumn(b, i);
                Grid.SetRow(b, g.RowDefinitions.Count - 1);
                g.Children.Add(l);
                Grid.SetColumn(l, i);
                Grid.SetRow(l, g.RowDefinitions.Count - 1);
                i++;
            }
        }
        public static void rapidFillColorized(string[] strings, Grid g, bool CC)
        {
            int i = 0;
            Brush c = ClientData.getGridColorCC(CC);
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25, GridUnitType.Pixel) });
            foreach (string s in strings)
            {
                Rectangle b = new Rectangle() { Fill = c, Margin = ClientData.GridMargin };
                Label l = new Label() { Content = s, Foreground = ClientData.textColor };
                g.Children.Add(b);
                Grid.SetColumn(b, i);
                Grid.SetRow(b, g.RowDefinitions.Count - 1);
                g.Children.Add(l);
                Grid.SetColumn(l, i);
                Grid.SetRow(l, g.RowDefinitions.Count - 1);
                i++;
            }
        }
        public static void rapidAppendData(string[] values, int[] IDs, Grid g)
        {
            int count = 1;
            Brush c = ClientData.getGridColor();
            int column = g.ColumnDefinitions.Count;
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Pixel) });
            Rectangle b1 = new Rectangle() { Fill = c, Margin = ClientData.GridMargin };
            Label l = new Label() { Content = "Audited", Foreground = ClientData.textColor };

            g.Children.Add(b1);
            Grid.SetColumn(b1, column);
            Grid.SetRow(b1, 0);
            g.Children.Add(l);
            Grid.SetColumn(l, column);
            Grid.SetRow(l, 0);
            foreach (string i in values)
            {
                Rectangle b = new Rectangle() { Fill = c, Margin = ClientData.GridMargin };
                Label d = new Label() { Content = values[count - 1], Foreground = ClientData.textColor };
                g.Children.Add(b);
                Grid.SetColumn(b, column);
                Grid.SetRow(b, count);
                g.Children.Add(d);
                Grid.SetColumn(d, column);
                Grid.SetRow(d, count);
                count++;
            }
        }
        public static void rapidFillSpaced(string[] strings, Grid g, int[] Spacing)
        {
            int i = 0;
            int r = 0;
            Brush c = ClientData.getGridColor();
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Pixel) });
            foreach (string s in strings)
            {
                Rectangle b = new Rectangle() { Fill = c, Margin = ClientData.GridMargin };
                Label l = new Label() { Content = s, Foreground = ClientData.textColor };
                g.Children.Add(b);
                Grid.SetColumn(b, i);
                Grid.SetRow(b, g.RowDefinitions.Count - 1);
                g.Children.Add(l);
                Grid.SetColumn(l, i);
                Grid.SetRow(l, g.RowDefinitions.Count - 1);
                Grid.SetColumnSpan(b, Spacing[r]);
                Grid.SetColumnSpan(l, Spacing[r]);
                i += Spacing[r];
                r++;
            }
        }
        public static void rapidFillSpacedRowHeightLocked(string[] strings, Grid g, int[] Spacing, int[] rowHeigtWidth)
        {
            int i = 0;
            int r = 0;
            Brush c = ClientData.getGridColor();
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(rowHeigtWidth[0], GridUnitType.Pixel) });
            foreach (string s in strings)
            {
                Rectangle b = new Rectangle() { Fill = c, Margin = ClientData.GridMargin };
                Label l = new Label() { Content = s, Foreground = ClientData.textColor };
                g.Children.Add(b);
                Grid.SetColumn(b, i);
                Grid.SetRow(b, g.RowDefinitions.Count - 1);
                g.Children.Add(l);
                Grid.SetColumn(l, i);
                Grid.SetRow(l, g.RowDefinitions.Count - 1);
                Grid.SetColumnSpan(b, Spacing[r]);
                Grid.SetColumnSpan(l, Spacing[r]);
                i += Spacing[r];
                r++;
            }
        }
        public static void rapidFillSpacedPremadeObjects(List<UIElement> Objects, Grid g, int[] Spacing, bool[] boxoff)
        {
            int i = 0;
            int r = 0;
            Brush c = ClientData.getGridColor();
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            foreach (UIElement s in Objects)
            {
                if (boxoff[r])
                {
                    Rectangle b = new Rectangle() { Fill = c, Margin = ClientData.GridMargin };
                    g.Children.Add(b);
                    Grid.SetColumn(b, i);
                    Grid.SetRow(b, g.RowDefinitions.Count - 1);
                    Grid.SetColumnSpan(b, Spacing[r]);
                }
                g.Children.Add(s);
                Grid.SetColumn(s, i);
                Grid.SetRow(s, g.RowDefinitions.Count - 1);
                Grid.SetColumnSpan(s, Spacing[r]);
                i += Spacing[r];
                r++;
            }
        }
        public static void rapidFillPremadeObjects(List<UIElement> Objects, Grid g, bool[] boxoff)
        {
            int i = 0;
            Brush c = ClientData.getGridColor();
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25, GridUnitType.Pixel) });
            foreach (UIElement s in Objects)
            {
                if (boxoff[i])
                {
                    Rectangle b = new Rectangle() { Fill = c, Margin = ClientData.GridMargin };
                    g.Children.Add(b);
                    Grid.SetColumn(b, i);
                    Grid.SetRow(b, g.RowDefinitions.Count - 1);
                }
                g.Children.Add(s);
                Grid.SetColumn(s, i);
                Grid.SetRow(s, g.RowDefinitions.Count - 1);
                i++;
            }
        }
        public static void rapidVertFillPremadeObjects(List<UIElement> Objects, Grid g, bool[] boxoff)
        {
            int i = 0;
            Brush c = ClientData.getGridColor();
            foreach (UIElement s in Objects)
            {
                g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25, GridUnitType.Pixel) });
                if (boxoff[i])
                {
                    Rectangle b = new Rectangle() { Fill = c, Margin = ClientData.GridMargin };
                    g.Children.Add(b);
                    Grid.SetColumn(b, 0);
                    Grid.SetRow(b, g.RowDefinitions.Count - 1);
                }
                g.Children.Add(s);
                Grid.SetColumn(s, 0);
                Grid.SetRow(s, g.RowDefinitions.Count - 1);
                i++;
            }
        }
        public static void rapidFillPremadeObjectsStandardHeight(List<UIElement> Objects, Grid g, bool[] boxoff, int Height)
        {
            int i = 0;
            Brush c = ClientData.getGridColor();
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(Height, GridUnitType.Pixel) });
            foreach (UIElement s in Objects)
            {
                if (boxoff[i])
                {
                    Rectangle b = new Rectangle() { Fill = c, Margin = ClientData.GridMargin };
                    g.Children.Add(b);
                    Grid.SetColumn(b, i);
                    Grid.SetRow(b, g.RowDefinitions.Count - 1);
                }
                g.Children.Add(s);
                Grid.SetColumn(s, i);
                Grid.SetRow(s, g.RowDefinitions.Count - 1);
                i++;
            }
        }
        public static void PurgeGrid(Grid g)
        {
            var children = g.Children.OfType<UIElement>().ToArray();
            foreach (UIElement child in children)
            {
                g.Children.Remove(child);
            }
            while (g.RowDefinitions.Count > 1)
            {
                g.RowDefinitions.RemoveAt(0);
            }
        }
        public static void PurgeAllGrid(Grid g)
        {
            var children = g.Children.OfType<UIElement>().ToArray();
            foreach (UIElement child in children)
            {
                g.Children.Remove(child);
            }
            while (g.RowDefinitions.Count > 0)
            {
                g.RowDefinitions.RemoveAt(0);
            }
        }
        public static void PurgeHeader(Grid g)
        {
            var children = g.Children.OfType<UIElement>().ToArray();
            foreach (UIElement child in children)
            {
                if (Grid.GetRow(child) == 1)
                {
                    g.Children.Remove(child);
                }
            }
            while (g.RowDefinitions.Count > 1)
            {
                g.RowDefinitions.RemoveAt(0);
            }
        }
    }
}
