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
    /// Interaction logic for MainHeader.xaml
    /// </summary>
    public partial class MainHeader : UserControl
    {
        public delegate void Changing(int index);
        public event Changing OnChange;
        public MainHeader()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int index = int.Parse((sender as TextBlock).Tag.ToString());
            OnChange?.Invoke(index);
        }
    }
}
