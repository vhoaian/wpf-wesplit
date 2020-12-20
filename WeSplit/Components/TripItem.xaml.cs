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

namespace WeSplit.Components
{
    /// <summary>
    /// Interaction logic for TripItem.xaml
    /// </summary>
    public class ThumbnailImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "/Images/Members/empty.jpg";
            }
            using (WeSplitDBEntities db = new WeSplitDBEntities())
            {
                Trip trip = value as Trip;
                var foundedPlace = db.Places.Include("PlaceImages").FirstOrDefault(p => p.id == trip.place_id);
                return $"{AppDomain.CurrentDomain.BaseDirectory}Images/Places/{foundedPlace.PlaceImages.ElementAt(0).file_name}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (System.DateTime)value;
            return date.ToString("dd/MM/yyyy");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class TripItem : UserControl
    {


        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(TripItem), new PropertyMetadata(1));


        public TripItem()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (WeSplitDBEntities db = new WeSplitDBEntities())
            {
                var foundedTrip = db.Trips.FirstOrDefault(trip => trip.id == Id);
                this.DataContext = foundedTrip;
                if (foundedTrip.is_finished) isFinished.Visibility = Visibility.Visible;
            }

        }
    }
}
