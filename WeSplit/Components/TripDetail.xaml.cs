using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for TripDetail.xaml
    /// </summary>

    public class TripImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "/Images/Members/empty.jpg";
            }
            TripImage img = value as TripImage;
            return $"{AppDomain.CurrentDomain.BaseDirectory}Images/Trips/{img.file_name}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    
    public partial class TripDetail : UserControl, INotifyPropertyChanged
    {
        public delegate void OnManagement(int trip);
        public event OnManagement onManagement;

        public Func<ChartPoint, string> PointLabel { get; set; }


        public ObservableCollection<TripImage> TripImages { get; private set; } = new ObservableCollection<TripImage>();

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(TripDetail), new PropertyMetadata(1));

        public event PropertyChangedEventHandler PropertyChanged;

        private Trip _trip;

        public Trip Trip1 { get => _trip; set
            {
                _trip = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Trip1"));
            }
        }

        public TripDetail()
        {
            InitializeComponent();
            PointLabel = chartPoint =>
                string.Format("{0:P1}", chartPoint.Participation);
            DataContext = this;
        }

        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (WeSplitDBEntities db = new WeSplitDBEntities())
            {
                Trip1 = db.Trips.Include("TripImages").Include("Expenses").Include("Place.PlaceImages").Include("RestStops").Include("Joinings.Member").FirstOrDefault(t => t.id == Id);
            }
            avatarImg.Source = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}Images\\Places\\{_trip.Place.PlaceImages.ElementAt(0).file_name}"));

            Trip1.TripImages.ToList().ForEach(i => TripImages.Add(i));

            // Tao bieu do
            piece1.Series = new SeriesCollection();

            foreach (var expense in Trip1.Expenses)
            {
                piece1.Series.Add(new PieSeries
                {
                    Title = expense.description.Substring(0, expense.description.Length > 20 ? 20: expense.description.Length) + ": " + expense.expense.ToString("F0"),
                    Values = new ChartValues<decimal> { expense.expense },
                    DataLabels = true,
                    LabelPoint = PointLabel
                });
            }


            piece2.Series = new SeriesCollection();

            foreach (var joining in Trip1.Joinings)
            {
                piece2.Series.Add(new PieSeries
                {
                    Title = joining.Member.name + " ứng " + joining.advance_deposit.ToString("F0"),
                    Values = new ChartValues<decimal> { joining.advance_deposit },
                    DataLabels = true,
                    LabelPoint = PointLabel
                });
            }

            if (Trip1.is_finished)
                finish.Visibility = Visibility.Collapsed;

        }

        private void management_Click(object sender, MouseButtonEventArgs e)
        {
            onManagement?.Invoke(Id);
        }

        private void expense_Click(object sender, MouseButtonEventArgs e)
        {
            ExpenseManagement expenseManagement = new ExpenseManagement(Id);
            expenseManagement.ShowDialog();
            using (WeSplitDBEntities db = new WeSplitDBEntities())
            {
                Trip1.Expenses = db.Expenses.Where(ex => ex.trip_id == Id).ToList();
            }
            piece1.Series = new SeriesCollection();

            foreach (var expense in Trip1.Expenses)
            {
                piece1.Series.Add(new PieSeries
                {
                    Title = expense.description.Substring(0, expense.description.Length > 20 ? 20 : expense.description.Length),
                    Values = new ChartValues<decimal> { expense.expense },
                    DataLabels = true,
                    LabelPoint = PointLabel
                });
            }
        }

        private void finish_Click(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Bạn có muốn kết thúc chuyến đi ở đây?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (WeSplitDBEntities db = new WeSplitDBEntities())
                {
                    Trip1.is_finished = true;
                    db.Entry(Trip1).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                finish.Visibility = Visibility.Collapsed;
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Hình ảnh (*.png;*.jpeg)|*.png;*.jpeg;*.jpg";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                List<TripImage> newTripImages = new List<TripImage>();

                foreach (var file in openFileDialog.FileNames)
                {
                    // Tạo tên mới cho hình
                    string newImagePath = null;
                    string imageUri = file;
                    string newImageName = null;
                    string imageName = imageUri.Substring(imageUri.LastIndexOf("\\"));
                    string extension = imageName.Substring(imageName.LastIndexOf("."));
                    newImageName = Guid.NewGuid().ToString() + extension;
                    string distFolder = $"{AppDomain.CurrentDomain.BaseDirectory}Images\\Trips\\";
                    if (!System.IO.Directory.Exists(distFolder))
                    {
                        System.IO.Directory.CreateDirectory(distFolder);
                    }
                    newImagePath = $"{distFolder}{newImageName}";

                    // Thêm vào list
                    var newTripImage = new TripImage()
                    {
                        file_name = newImageName,
                        trip_id = Id
                    };
                    // Copy hình vào thư mục
                    if (newImageName != null)
                        System.IO.File.Copy(imageUri, newImagePath);

                    newTripImages.Add(newTripImage);
                }
                using (WeSplitDBEntities db = new WeSplitDBEntities())
                {
                    foreach (var tI in newTripImages)
                    {
                        db.TripImages.Add(tI);
                        TripImages.Add(tI);
                    }
                    db.SaveChanges();
                }
            }
            

            
        }

        private void PieChart_DataClick(object sender, ChartPoint chartPoint)
        {
            var chart = (PieChart)chartPoint.ChartView;

            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartPoint.SeriesView;
            selectedSeries.PushOut = 5;
        }
    }
}
