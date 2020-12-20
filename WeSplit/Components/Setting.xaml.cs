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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeSplit.Components
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : UserControl
    {
        public Setting()
        {
            InitializeComponent();
            showSplash.IsChecked = UserSettings.ShowSplash;
            perPage.Value = UserSettings.PerPage.ToString();
        }

        private void saveButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (perPage.Value.Length == 0)
            {
                MessageBox.Show("Hiển thị mỗi trang không được bỏ trống");
                return;
            }
            try
            {
                int value = int.Parse(perPage.Value);
            } catch (Exception)
            {
                MessageBox.Show("Hiển thị mỗi trang không hợp lệ");
                return;
            }
            if (int.Parse(perPage.Value) <= 0)
            {
                MessageBox.Show("Hiển thị mỗi trang không được là số âm");
                return;
            }
            UserSettings.ShowSplash = (bool)showSplash.IsChecked;
            UserSettings.PerPage = int.Parse(perPage.Value);

            MessageBox.Show("Cài đặt lưu!");
        }
    }
}
