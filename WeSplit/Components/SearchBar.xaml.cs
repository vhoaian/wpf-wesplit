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
    /// Interaction logic for SearchBar.xaml
    /// </summary>
    public partial class SearchBar : UserControl, INotifyPropertyChanged
    {
        public delegate void OnFound(int id);
        public event OnFound onFound;
        
        public SearchBar()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void EnhancedButton_Click(object sender, MouseButtonEventArgs e)
        {
            using (WeSplitDBEntities db = new WeSplitDBEntities())
            {
                var query = db.Trips.AsQueryable();

                if (tripCombo.SelectedIndex == 1)
                    query = query.Where(t => t.is_finished == true);
                else if (tripCombo.SelectedIndex == 2)
                    query = query.Where(t => t.is_finished == false);

                if (keyword.Value.Length > 0)
                {
                    if (is_name.IsChecked == true)
                    {
                        query = query.Where(t => t.name.Contains(keyword.Value.ToLower()));
                    }
                    else if (is_member.IsChecked == true)
                    {
                        var members = from member in db.Members
                                      where member.name.Contains(keyword.Value.ToLower())
                                      select member;
                        var joinings = from member in members
                                       join joining in db.Joinings
                                       on member.id equals joining.member_id
                                       select joining;
                        var trips = from t in query
                                    join joining in joinings
                                    on t.id equals joining.trip_id
                                    select t;
                        query = trips.GroupBy(x => x.id)
                                     .Select(g => g.FirstOrDefault());
                    }
                    else
                    {

                        query = from place in db.Places
                                where place.name.Contains(keyword.Value.ToLower())
                                join trip in query
                                on place.id equals trip.place_id
                                select trip;

                    }
                }

                var _trips = query.OrderByDescending(t => t.id).ToList();
                if (_trips.Count > 0)
                {
                    var _tripList = new TripList()
                    {
                        Trips = _trips,
                        PerPage = UserSettings.PerPage
                    };
                    _tripList.OnTrip += _tripList_OnTrip;
                    tripList.Content = _tripList;

                }

            }
        }

        private void _tripList_OnTrip(int trip)
        {
            onFound?.Invoke(trip);
        }
    }
}
