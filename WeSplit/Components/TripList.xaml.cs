using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for TripList.xaml
    /// </summary>
    public partial class TripList : UserControl, INotifyPropertyChanged
    {

        public delegate void ClickOnTrip(int trip);
        public event ClickOnTrip OnTrip;
        public int PerPage
        {
            get { return (int)GetValue(PerPageProperty); }
            set { SetValue(PerPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PerPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PerPageProperty =
            DependencyProperty.Register("PerPage", typeof(int), typeof(TripList), new PropertyMetadata(4));


        public List<Trip> Trips
        {
            get { return (List<Trip>)GetValue(TripsProperty); }
            set { SetValue(TripsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Trips.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TripsProperty =
            DependencyProperty.Register("Trips", typeof(List<Trip>), typeof(TripList), new PropertyMetadata(new List<Trip>()));

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Trip> CurrentTrips { get; private set; } = new ObservableCollection<Trip>();

        public int CurrentPage { get; private set; } = 1;
        private int _totalPage = 1;
        public int TotalPage
        {
            get { return _totalPage; }
            set { _totalPage = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TotalPage")); }
        }

        public TripList()
        {
            InitializeComponent();
        }

        private void loadCurrentTrips()
        {
            CurrentTrips.Clear();
            for (int i = (CurrentPage - 1) * PerPage; i < CurrentPage * PerPage; i++)
            {
                if (i >= Trips.Count) break;
                CurrentTrips.Add(Trips[i]);
            }
        }

        private void Pagination_onSwitch(int destPage)
        {
            CurrentPage = destPage;
            loadCurrentTrips();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TotalPage = Trips.Count / PerPage + (Trips.Count % PerPage == 0 ? 0 : 1);
            loadCurrentTrips();
            this.DataContext = this;
        }

        private void TripItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnTrip?.Invoke((sender as TripItem).Id);
        }
    }
}
