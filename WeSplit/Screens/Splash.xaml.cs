using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WeSplit.Screens
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public class PlaceRandomImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Place place = value as Place;
            string path = "/Images/Members/empty.jpg";
            if (place == null || place.PlaceImages.Count == 0) return path;
            var imgs = place.PlaceImages;
            Random rng = new Random();
            var idx = rng.Next(place.PlaceImages.Count);

            return $"{AppDomain.CurrentDomain.BaseDirectory}Images/Places/{imgs.ElementAt(idx).file_name}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class Splash : Window, INotifyPropertyChanged
    {
        public Splash()
        {
            InitializeComponent();
            using (WeSplitDBEntities db = new WeSplitDBEntities())
            {
                Random rand = new Random();
                int toSkip = rand.Next(db.Places.Count());
                Place = db.Places.Include("PlaceImages").OrderBy(p => p.id).Skip(toSkip).Take(1).First();
            }
            DataContext = this;
        }
        private Timer t;
        private Place _place;
        public Place Place
        {
            get => _place; set
            {
                _place = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Place"));
            }
        }

        private MainWindow mainWindow;

        public event PropertyChangedEventHandler PropertyChanged;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            mainWindow = new MainWindow();
            t = new Timer(new TimerCallback(CloseSplash), null, new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 0, 0, -1));
        }

        private void CloseSplash(object info)
        {
            Dispatcher.Invoke(new Action(() => { CloseSplashMain(); } ));
        }

        private void CloseSplashMain()
        {
           mainWindow.Show();
           Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            t.Dispose();
            t = new Timer(new TimerCallback(CloseSplash), null, new TimeSpan(0, 0, 0, 0, 200), new TimeSpan(0, 0, 0, 0, -1));
            UserSettings.ShowSplash = false;
        }
    }
}
