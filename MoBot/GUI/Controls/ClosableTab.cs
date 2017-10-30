using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MoBot.GUI.Controls
{
    internal class ClosableTab : TabItem
    {
        private readonly CloseableHeader header = new CloseableHeader();
        public ClosableTab()
        {
            Header = header;
            header.ButtonClose.MouseEnter += ButtonCloseOnMouseEnter;
            header.ButtonClose.MouseLeave += ButtonCloseOnMouseLeave;
            header.ButtonClose.Click += ButtonCloseOnClick;
            header.LabelTabTitle.SizeChanged += LabelTabTitleOnSizeChanged;
        }

        private void LabelTabTitleOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            header.ButtonClose.Margin = new Thickness(header.LabelTabTitle.ActualWidth + 5,3,4,0);
        }

        private void ButtonCloseOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            ((TabControl)Parent).Items.Remove(this);
        }

        private void ButtonCloseOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            header.ButtonClose.Foreground = Brushes.Black;
        }

        private void ButtonCloseOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            header.ButtonClose.Foreground = Brushes.Red;
        }

        public string Title
        {
            set => header.LabelTabTitle.Content = value;
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            header.LabelTabTitle.Visibility = Visibility.Visible;
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            header.ButtonClose.Visibility = Visibility.Hidden;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            header.ButtonClose.Visibility = Visibility.Visible;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!IsSelected)
                header.ButtonClose.Visibility = Visibility.Hidden;
        }
    }
}
