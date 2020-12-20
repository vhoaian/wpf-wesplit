using System;
using System.Collections.Generic;
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
using WeSplit.Components;
using WeSplit.Screens;

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Control _userControl;
        public Control UserControl { get => _userControl; set {
                _userControl = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserControl"));
            }
        }

        SearchBar _searchBar = new SearchBar();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            MainHeader_OnChange(0);
            _searchBar.onFound += onTripClick;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        

        private void MainHeader_OnChange(int index)
        {
            switch (index)
            {
                case 1:
                    
                    UserControl = _searchBar;
                    break;
                case 2:
                    UserControl = new PlaceList();
                    break;
                case 3:
                    UserControl = new MemberList();
                    break;
                case 4:
                    var tripMgt = new TripManagement()
                    {
                        Id = -1
                    };
                    tripMgt.onSave += onTripClick;
                    UserControl = tripMgt;
                    break;
                case 5:
                    UserControl = new Setting();
                    break;
                default:
                    using (WeSplitDBEntities db = new WeSplitDBEntities())
                    {
                        var tripList = new TripList()
                        {
                            Trips = db.Trips.OrderByDescending(t => t.id).ToList(),
                            PerPage = UserSettings.PerPage,
                            
                        };
                        tripList.OnTrip += onTripClick;
                        UserControl = tripList;
                    }
                    break;
            }
        }

        private void onTripClick(int trip)
        {
            var tripDetail = new TripDetail() {
                Id = trip
            };
            tripDetail.onManagement += TripDetail_onManagement;
            UserControl = tripDetail;
        }

        private void TripDetail_onManagement(int trip)
        {
            var tripMgt = new TripManagement()
            {
                Id = trip
            };
            tripMgt.onSave += onTripClick;
            UserControl = tripMgt;
        }
    }
}
