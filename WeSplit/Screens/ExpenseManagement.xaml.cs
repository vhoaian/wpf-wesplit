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
    /// Interaction logic for ExpenseManagement.xaml
    /// </summary>
    public partial class ExpenseManagement : Window
    {
        public ExpenseManagement()
        {
            InitializeComponent();
        }
        public Trip Trip { get; set; }
        public ObservableCollection<Expense> Expenses { get; private set; } = new ObservableCollection<Expense>();

        public ExpenseManagement(int id)
        {
            InitializeComponent();
            using (WeSplitDBEntities db = new WeSplitDBEntities())
            {
                Trip = db.Trips.FirstOrDefault(t => t.id == id);
                Trip.Expenses.ToList().ForEach(t => Expenses.Add(t));
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void createButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (moneyTextbox.Value.Length == 0)
            {
                MessageBox.Show("Vui lòng điền số tiền chi tiêu");
                return;
            }
            int value = 0;
            try
            {
                value = int.Parse(moneyTextbox.Value);
            }
            catch (Exception)
            {
                MessageBox.Show("Số tiền không hợp lệ");
                return;
            }
            if (descTextbox.Value.Length == 0)
            {
                MessageBox.Show("Vui lòng điền lý do chi tiêu");
                return;
            }

            using (WeSplitDBEntities db = new WeSplitDBEntities())
            {
                Expense expense = new Expense()
                {
                    trip_id = Trip.id,
                    expense = value,
                    description = descTextbox.Value
                };
                db.Expenses.Add(expense);
                db.SaveChanges();
                Expenses.Add(expense);
            }
            moneyTextbox.Value = descTextbox.Value = "";
        }

        private void deleteButton_Click(object sender, MouseButtonEventArgs e)
        {
            int id = int.Parse((sender as EnhancedButton).Tag.ToString());
            var result = MessageBox.Show("Bạn có chắc muốn xoá chi tiêu này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (WeSplitDBEntities db = new WeSplitDBEntities())
                {
                    Expense expense = db.Expenses.FirstOrDefault(ex => ex.id == id);
                    db.Expenses.Remove(expense);
                    db.SaveChanges();
                    Expenses.Remove(Expenses.FirstOrDefault(ex => ex.id == id));
                }
            }
            
        }

        private void summaryButton_Click(object sender, MouseButtonEventArgs e)
        {
            var w = new Summary(Trip.id);
            w.ShowDialog();
        }
    }
}
