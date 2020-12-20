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
    /// Interaction logic for MemberList.xaml
    /// </summary>

    public class MemberImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "/Images/Members/empty.jpg";
            }
            string name = value.ToString();
            return $"{AppDomain.CurrentDomain.BaseDirectory}Images/Members/{name}";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class MemberList : UserControl
    {
        public ObservableCollection<Member> Members { get; private set; } = new ObservableCollection<Member>();
        public MemberList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (WeSplitDBEntities db = new WeSplitDBEntities())
            {
                var list = db.Members.ToList();
                list.Reverse();
                list.ForEach(member => Members.Add(member));
            }
        }

        private void editButton_Click(object sender, MouseButtonEventArgs e)
        {
            int id = int.Parse((sender as EnhancedButton).Tag.ToString());
            var foundMember = Members.FirstOrDefault(member => member.id == id);
            if (foundMember != null)
            {
                var w = new AddMember(foundMember);
                if (w.ShowDialog() == true)
                {
                    var updatedMember = w.Member;
                    using (WeSplitDBEntities db = new WeSplitDBEntities())
                    {
                        db.Entry(updatedMember).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }

                    int index = Members.IndexOf(foundMember);
                    Members.RemoveAt(index);
                    Members.Insert(index, updatedMember);
                }
                
            }
        }

        private void deleteButton_Click(object sender, MouseButtonEventArgs e)
        {
            int id = int.Parse((sender as EnhancedButton).Tag.ToString());
            var result = MessageBox.Show("Bạn có chắc chắn muốn xoá thành viên này?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var foundMember = Members.FirstOrDefault(member => member.id == id);
                if (foundMember != null)
                {
                    using (WeSplitDBEntities db = new WeSplitDBEntities())
                    {
                        db.Entry(foundMember).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                    }
                    Members.Remove(foundMember);
                }
            }
        }

        private void createMemberButton_Click(object sender, MouseButtonEventArgs e)
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

                Members.Insert(0, newMember);
                scrollViewer.ScrollToHome();
            }
        }
    }
}
