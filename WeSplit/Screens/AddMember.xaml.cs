using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddMember.xaml
    /// </summary>
    public partial class AddMember : Window
    {
        private Member _member = null;

        public Member Member { get => _member; private set => _member = value; }

        public AddMember()
        {
            InitializeComponent();
        }

        public AddMember(Member member)
        {
            InitializeComponent();
            _member = member;
            nameTextbox.Value = member.name;
            phoneTextbox.Value = member.phone;
            if (member.avatar != null)
            {
                avatarImage.Source = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}Images/Members/{member.avatar}", UriKind.Absolute));
            }
        }

        private void submitButton_Click(object sender, MouseButtonEventArgs e)
        {
            // Xác minh dữ liệu
            if (nameTextbox.Value == "" || phoneTextbox.Value == "")
            {
                MessageBox.Show("Vui lòng điền cả Tên và SĐT!", "Cảnh báo");
                return;
            }

            // Tạo tên mới cho hình
            string newImagePath = null;
            string oldImagePath = null;
            string imageUri = avatarImage.Source.ToString();
            string newImageName = null;
            if (imageUri.StartsWith("file"))
            {
                oldImagePath = imageUri.Substring(8); // Bỏ qua file:///
                string imageName = oldImagePath.Substring(oldImagePath.LastIndexOf("/"));
                string extension = imageName.Substring(imageName.LastIndexOf("."));
                newImageName = Guid.NewGuid().ToString() + extension;
                string distFolder = $"{AppDomain.CurrentDomain.BaseDirectory}Images\\Members\\";
                if (!System.IO.Directory.Exists(distFolder))
                {
                    System.IO.Directory.CreateDirectory(distFolder);
                }
                newImagePath = $"{distFolder}{newImageName}";
            }

            // Chinh sua doi tuong
            if (_member == null) _member = new Member();
            _member.name = nameTextbox.Value;
            _member.phone = phoneTextbox.Value;
            _member.avatar = newImageName;
            
            // Copy hình vào thư mục
            if (newImageName != null)
                System.IO.File.Copy(oldImagePath, newImagePath);

            this.DialogResult = true;
            this.Close();
        }

        private void clearButton_Click(object sender, MouseButtonEventArgs e)
        {
            nameTextbox.Value = phoneTextbox.Value = "";
            avatarImage.Source = new BitmapImage(new Uri("/resources/add-image.png", UriKind.Relative));
        }

        private void avatarImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Hình ảnh (*.png;*.jpeg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                avatarImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }
}
