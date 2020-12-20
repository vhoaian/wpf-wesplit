using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace WeSplit.Screens
{
    /// <summary>
    /// Interaction logic for AddPlace.xaml
    /// </summary>
    public partial class AddPlace : Window
    {
        private ObservableCollection<Province> _provinces = new ObservableCollection<Province>();
        private Place _place = null;
        public ObservableCollection<Province> Provinces { get => _provinces; set => _provinces = value; }
        public Place Place { get => _place; }

        public AddPlace()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public AddPlace(Place place)
        {
            InitializeComponent();
            this.DataContext = this;
            _place = place;
            
        }
        private void submitButton_Click(object sender, MouseButtonEventArgs e)
        {
            // Xac minh du lieu
            if (provinceCombo.SelectedIndex < 0) {
                MessageBox.Show("Vui lòng chọn tỉnh thành");
                return;
            }
            if (nameTextbox.Value.Length == 0)
            {
                MessageBox.Show("Vui lòng điền tên địa điểm");
                return;
            }
            if (descTextbox.Value.Length == 0)
            {
                MessageBox.Show("Vui lòng điền mô tả cho địa điểm");
                return;
            }
            
            List<PlaceImage> placeImages = new List<PlaceImage>();
            if (_place != null)
                placeImages = _place.PlaceImages.ToList();

            int beginIndex = 1;
            if (_place != null)
                beginIndex += _place.PlaceImages.Count;

            for (int i = beginIndex; i < imageList.Children.Count; i++)
            {
                // Tạo tên mới cho hình
                string newImagePath = null;
                string oldImagePath = null;
                string imageUri = (imageList.Children[i] as Image).Source.ToString();
                string newImageName = null;
                if (imageUri.StartsWith("file"))
                {
                    oldImagePath = imageUri.Substring(8); // Bỏ qua file:///
                    string imageName = oldImagePath.Substring(oldImagePath.LastIndexOf("/"));
                    string extension = imageName.Substring(imageName.LastIndexOf("."));
                    newImageName = Guid.NewGuid().ToString() + extension;
                    string distFolder = $"{AppDomain.CurrentDomain.BaseDirectory}Images\\Places\\";
                    if (!System.IO.Directory.Exists(distFolder))
                    {
                        System.IO.Directory.CreateDirectory(distFolder);
                    }
                    newImagePath = $"{distFolder}{newImageName}";

                    // Thêm vào list
                    var newPlaceImage = new PlaceImage()
                    {
                        file_name = newImageName
                    };
                    if (_place != null)
                        newPlaceImage.place_id = _place.id;

                    // Copy hình vào thư mục
                    if (newImageName != null)
                        System.IO.File.Copy(oldImagePath, newImagePath);

                    placeImages.Add(newPlaceImage);
                }
            }
            if (_place == null)
                _place = new Place();

            _place.name = nameTextbox.Value;
            _place.description = descTextbox.Value;
            _place.province_id = (provinceCombo.SelectedItem as Province).id;
            _place.Province = provinceCombo.SelectedItem as Province;
            _place.PlaceImages = placeImages;

            this.DialogResult = true;
            this.Close();
        }

        private void clearButton_Click(object sender, MouseButtonEventArgs e)
        {
            nameTextbox.Value = descTextbox.Value = "";
            provinceCombo.SelectedIndex = -1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (WeSplitDBEntities db = new WeSplitDBEntities())
            {
                db.Provinces.ToList().ForEach(province => _provinces.Add(province));
            }
            if (_place != null)
            {
                var sProvince = _provinces.FirstOrDefault(p => p.id == _place.province_id);
                provinceCombo.SelectedItem = sProvince;
                nameTextbox.Value = _place.name;
                descTextbox.Value = _place.description;
                foreach (var file in _place.PlaceImages)
                {
                    Image image = new Image()
                    {
                        Margin = new Thickness(3)
                    };
                    image.Source = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}Images\\Places\\{file.file_name}"));
                    imageList.Children.Add(image);
                }
            }

        }

        private void addImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Hình ảnh (*.png;*.jpeg)|*.png;*.jpeg;*.jpg";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    Image image = new Image()
                    {
                        Margin = new Thickness(3)
                    };
                    image.Source = new BitmapImage(new Uri(file));
                    imageList.Children.Add(image);
                }
            }
        }
    }
}
