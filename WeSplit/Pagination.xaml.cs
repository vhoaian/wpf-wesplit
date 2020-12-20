using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for Pagination.xaml
    /// </summary>

    public class PreviousPageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class NextPageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class Pagination : UserControl
    {
        const int NUM_OF_PAGE = 5;

        class PageItem : IComparable<PageItem>
        {
            public int Page { get; set; }
            public string Type { get; set; }

            public PageItem()
            {
                Type = "Success";
            }

            public int CompareTo(PageItem obj)
            {
                return Page - obj.Page;
            }
        }

        public Pagination()
        {
            InitializeComponent();
        }



        private void getPages()
        {
            if (Current == 1) goPrevious.Visibility = Visibility.Collapsed;
            else goPrevious.Visibility = Visibility.Visible;
            if (Current == Total) goNext.Visibility = Visibility.Collapsed;
            else goNext.Visibility = Visibility.Visible;

            SortedSet<PageItem> pagesSet = new SortedSet<PageItem>() { new PageItem() { Page = Current, Type = "Secondary" } };
            for (int delta = 1; delta < Total; delta++)
            {
                if (pagesSet.Count >= NUM_OF_PAGE) break;
                if (Current + delta <= Total)
                    pagesSet.Add(new PageItem() { Page = Current + delta });
                if (pagesSet.Count >= NUM_OF_PAGE) break;
                if (Current - delta > 0)
                    pagesSet.Add(new PageItem() { Page = Current - delta });
            }

            if (pagesSet.Min.Page > 2)
            {
                leftSpan.Visibility = Visibility.Visible;
                firstPage.Visibility = Visibility.Visible;
            }
            else
            {
                if (pagesSet.Min.Page == 1)
                    firstPage.Visibility = Visibility.Collapsed;
                else
                    firstPage.Visibility = Visibility.Visible;

                leftSpan.Visibility = Visibility.Collapsed;
            }

            if (pagesSet.Max.Page < Total - 1)
            {
                rightSpan.Visibility = Visibility.Visible;
                lastPage.Visibility = Visibility.Visible;
            }
            else
            {
                if (pagesSet.Max.Page == Total)
                    lastPage.Visibility = Visibility.Collapsed;
                else
                    lastPage.Visibility = Visibility.Visible;
                rightSpan.Visibility = Visibility.Collapsed;
            }

            pageList.ItemsSource = pagesSet;
        }

        public delegate void PageSwitching(int destPage);
        public event PageSwitching onSwitch;

        public int Current
        {
            get { return (int)GetValue(CurrentProperty); }
            set { SetValue(CurrentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Current.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentProperty =
            DependencyProperty.Register("Current", typeof(int), typeof(Pagination), new PropertyMetadata(0));


        public int Total
        {
            get { return (int)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Total.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(int), typeof(Pagination), new PropertyMetadata(0));

        private void EnhancedButton_Click(object sender, MouseButtonEventArgs e)
        {
            EnhancedButton btn = sender as EnhancedButton;
            int destPage = int.Parse(btn.Tag.ToString());
            Current = destPage;
            getPages();
            onSwitch?.Invoke(destPage);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            getPages();
        }
    }
}
