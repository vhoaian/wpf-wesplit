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
using WeSplit.Screens;

namespace WeSplit.Components
{
    /// <summary>
    /// Interaction logic for TripManagement.xaml
    /// </summary>
    public partial class TripManagement : UserControl, INotifyPropertyChanged
    {

        public delegate void OnSave(int id);
        public event OnSave onSave;
        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        private Trip _trip;

        private List<Place> places = null;
        public ObservableCollection<Member> Members { get; private set; } = new ObservableCollection<Member>();
        public ObservableCollection<Joining> Joinings { get; private set; } = new ObservableCollection<Joining>();
        public ObservableCollection<RestStop> RestStops { get; private set; } = new ObservableCollection<RestStop>();
        public Trip Trip { get => _trip; set => _trip = value; }
        public List<Place> Places { get => places; set
            {
                places = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Places"));
            }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(TripManagement), new PropertyMetadata(1));

        public event PropertyChangedEventHandler PropertyChanged;

        public TripManagement()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Id > 0)
            {
                using (WeSplitDBEntities db = new WeSplitDBEntities())
                {
                    Trip = db.Trips.Include("Joinings.Member").Include("Place").Include("RestStops").FirstOrDefault(trip => trip.id == Id);
                    this.DataContext = Trip;
                    var _members = db.Members.ToList();
                    foreach (var _m in _members)
                    {
                        if (Trip.Joinings.FirstOrDefault(j => j.member_id == _m.id) == null)
                        {
                            Members.Add(_m);
                        }
                    }
                    Trip.Joinings.ToList().ForEach(j => Joinings.Add(j));
                    Trip.RestStops.ToList().ForEach(j => RestStops.Add(j));
                    Places = db.Places.ToList();
                    Place sPlace = Places.Find(p => p.id == Trip.place_id);
                    placeCombo.SelectedItem = sPlace;
                    nameTextbox.Value = Trip.name;
                    descTextbox.Value = Trip.description;
                    departureDate.SelectedDate = Trip.departure_date;
                    returnDate.SelectedDate = Trip.return_date;
                }
            } else
            {
                using (WeSplitDBEntities db = new WeSplitDBEntities())
                {
                    Trip = new Trip();
                    this.DataContext = Trip;
                    db.Members.ToList().ForEach(_m => Members.Add(_m));
                    Trip.Joinings = new List<Joining>();
                    Trip.RestStops = new List<RestStop>();
                    Places = db.Places.ToList();
                }
            }
        }

        private void addmemButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (memberCombo.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn thành viên");
                return;
            }
            if (deposit.Value.Length == 0)
            {
                MessageBox.Show("Vui lòng điền số tiền ứng ban đầu");
                return;
            }
            int value = 0;
            try
            {
                value = int.Parse(deposit.Value);
            } catch (Exception)
            {
                MessageBox.Show("Số tiền không hợp lệ");
                return;
            }

            Joining joining = new Joining()
            {
                member_id = (memberCombo.SelectedItem as Member).id,
                advance_deposit = int.Parse(deposit.Value)
            };
            if (Id > 0) joining.trip_id = Id;

            joining.Member = memberCombo.SelectedItem as Member;
            joining.member_id = (memberCombo.SelectedItem as Member).id;
            Joinings.Add(joining);
            //Trip.Joinings.Add(joining);
            
            Members.RemoveAt(memberCombo.SelectedIndex);
            memberCombo.SelectedIndex = -1;
            deposit.Value = "";
            memberList.SelectedIndex = Joinings.Count - 1;
            memberList.ScrollIntoView(memberList.SelectedItem);

        }

        private void EnhancedButton_Click(object sender, MouseButtonEventArgs e)
        {
            var w = new AddMember();
            if (w.ShowDialog() == true)
            {
                var newMember = w.Member;
                using (WeSplitDBEntities db = new WeSplitDBEntities())
                {
                    newMember = db.Members.Add(newMember);
                    db.SaveChanges();
                }

                Members.Add(newMember);
                memberCombo.SelectedIndex = Members.Count - 1;

            }
        }

        private void addRestButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (restName.Value.Length == 0)
            {
                MessageBox.Show("Vui lòng điền tên điểm dừng chân");
                return;
            }
            var newRest = new RestStop()
            {
                name = restName.Value
            };
            if (Id > 0) newRest.trip_id = Id;
            RestStops.Add(newRest);
            //Trip.RestStops.Add(newRest);
            restName.Value = "";
            placeList.SelectedIndex = Places.Count - 1;
            placeList.ScrollIntoView(placeList.SelectedItem);
        }

        private void saveAllButton_Click(object sender, MouseButtonEventArgs e)
        {
            Trip updatedTrip;
            if (Id > 0)
            {
                using (WeSplitDBEntities db = new WeSplitDBEntities())
                {
                    for (int i = Trip.Joinings.Count; i < Joinings.Count; i++)
                    {
                        Joinings[i].Member = null;
                        db.Joinings.Add(Joinings[i]);
                    }
                    for (int i = Trip.RestStops.Count; i < RestStops.Count; i++)
                    {
                        db.RestStops.Add(RestStops[i]);
                    }
                    updatedTrip = new Trip()
                    {
                        id = Id,
                        name = nameTextbox.Value,
                        departure_date = (DateTime)departureDate.SelectedDate,
                        return_date = (DateTime)returnDate.SelectedDate,
                        description = descTextbox.Value,
                        place_id = (placeCombo.SelectedItem as Place).id,
                        is_finished = Trip.is_finished
                    };
                    db.Entry(updatedTrip).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }
            } else
            {
                for (int i = 0; i < Joinings.Count; i++)
                {
                    Joinings[i].Member = null;
                }
                using (WeSplitDBEntities db = new WeSplitDBEntities())
                {
                    updatedTrip = new Trip()
                    {
                        name = nameTextbox.Value,
                        departure_date = (DateTime)departureDate.SelectedDate,
                        return_date = (DateTime)returnDate.SelectedDate,
                        place_id = (placeCombo.SelectedItem as Place).id,
                        description = descTextbox.Value,
                        Joinings = Joinings,
                        RestStops = RestStops
                    };
                    db.Trips.Add(updatedTrip);
                    db.SaveChanges();
                }
            }
            onSave?.Invoke(updatedTrip.id);
        }
    }
}
