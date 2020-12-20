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
    /// Interaction logic for Summary.xaml
    /// </summary>
    public class SummaryEntry
    {
        public Member Member { get; set; }
        public string type { get; set; }
        public decimal money { get; set; }
    }
    public partial class Summary : Window
    {
        public Trip Trip { get; set; }
        public decimal Deposit { get; set; }
        public decimal Total { get; set; }
        public List<SummaryEntry> SummaryEntries { get; set; } = new List<SummaryEntry>();
        public Summary()
        {
            InitializeComponent();
        }
        public Summary(int trip)
        {
            InitializeComponent();
            using (WeSplitDBEntities db = new WeSplitDBEntities())
            {
                Trip = db.Trips.Include("Joinings.Member").Include("Expenses").FirstOrDefault(t => t.id == trip);
                var Expenses = Trip.Expenses.ToList();
                var Joinings = Trip.Joinings.ToList();

                Deposit = Joinings.Select(j => j.advance_deposit).Sum();
                Total = Expenses.Select(e => e.expense).Sum();
            }

            // Tinh toan
            decimal perMember = Total / Trip.Joinings.Count;

            foreach (var joining in Trip.Joinings)
            {
                if (joining.advance_deposit > perMember)
                {
                    SummaryEntries.Add(new SummaryEntry()
                    {
                        Member = joining.Member,
                        type = "TRẢ",
                        money = joining.advance_deposit - perMember
                    });
                } else
                {
                    SummaryEntries.Add(new SummaryEntry()
                    {
                        Member = joining.Member,
                        type = "THU",
                        money = perMember - joining.advance_deposit
                    });
                }
            }

            DataContext = this;
        }
    }
}
