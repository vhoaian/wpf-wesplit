using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WeSplit.Screens;

namespace WeSplit.Components
{
    /// <summary>
    /// Interaction logic for PlaceList.xaml
    /// </summary>
    public class PlaceImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            Place place = value as Place;
            List<PlaceImage> placeImages = place.PlaceImages.ToList();

            if (placeImages.Count == 0)
            {
                return "/Images/Members/empty.jpg";
            }
            string name = placeImages[0].file_name;
            return $"{AppDomain.CurrentDomain.BaseDirectory}Images/Places/{name}";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class PlaceList : UserControl
    {
        public ObservableCollection<Place> Places { get; private set; } = new ObservableCollection<Place>();

        public PlaceList()
        {
            InitializeComponent();
        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (WeSplitDBEntities db = new WeSplitDBEntities())
            {
                var list = db.Places.Include("PlaceImages").Include("Province").ToList();
                list.Reverse();
                list.ForEach(place => Places.Add(place));
            }
        }

        private void editButton_Click(object sender, MouseButtonEventArgs e)
        {
            int id = int.Parse((sender as EnhancedButton).Tag.ToString());
            var foundPlace = Places.FirstOrDefault(place => place.id == id);
            int nCurrentImages = foundPlace.PlaceImages.Count;
            if (foundPlace != null)
            {
                var w = new AddPlace(foundPlace);
                if (w.ShowDialog() == true)
                {
                    var updatedPlace = w.Place;
                    using (WeSplitDBEntities db = new WeSplitDBEntities())
                    {
                        db.Entry(updatedPlace).State = System.Data.Entity.EntityState.Modified;
                        for (int i = nCurrentImages; i < updatedPlace.PlaceImages.Count; i++)
                        {
                            db.PlaceImages.Add(updatedPlace.PlaceImages.ElementAt(i));
                        }
                        db.SaveChanges();
                    }

                    int index = Places.IndexOf(foundPlace);
                    Places.RemoveAt(index);
                    Places.Insert(index, updatedPlace);
                }
            }
        }

        private void deleteButton_Click(object sender, MouseButtonEventArgs e)
        {
            int id = int.Parse((sender as EnhancedButton).Tag.ToString());
            var result = MessageBox.Show("Bạn có chắc chắn muốn xoá địa điểm này?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var foundPlace = Places.FirstOrDefault(member => member.id == id);
                if (foundPlace != null)
                {
                    using (WeSplitDBEntities db = new WeSplitDBEntities())
                    {
                        while (foundPlace.PlaceImages.Count != 0)
                            db.Entry(foundPlace.PlaceImages.ElementAt(0)).State = System.Data.Entity.EntityState.Deleted;
                        db.Entry(foundPlace).State = System.Data.Entity.EntityState.Deleted;
                        try
                        {
                            db.SaveChanges();
                        } catch (Exception ex)
                        {
                            MessageBox.Show("Bạn không thể xoá địa điểm này do có tồn tại chuyến đi thuộc về nó.", "Không thành công",
                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    Places.Remove(foundPlace);
                }
            }
        }

        private void createPlaceButton_Click(object sender, MouseButtonEventArgs e)
        {
            var w = new AddPlace();
            if (w.ShowDialog() == true)
            {
                var newPlace = w.Place;
                using (WeSplitDBEntities db = new WeSplitDBEntities())
                {
                    newPlace = db.Places.Add(newPlace);
                    db.SaveChanges();
                }

                Places.Insert(0, newPlace);
                scrollViewer.ScrollToHome();
            }
        }
    }
}
