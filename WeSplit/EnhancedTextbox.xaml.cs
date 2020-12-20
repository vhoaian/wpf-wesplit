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
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for EnhancedTextbox.xaml
    /// </summary>
    public partial class EnhancedTextbox : UserControl
    {

        public event TextChangedEventHandler TextChanged;
        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }



        public int Thickness
        {
            get { return (int)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }



        public int CornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }



        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }



        public bool IsMultiline
        {
            get { return (bool)GetValue(IsMultilineProperty); }
            set { SetValue(IsMultilineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMultiline.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMultilineProperty =
            DependencyProperty.Register("IsMultiline", typeof(bool), typeof(EnhancedTextbox), new PropertyMetadata(false));



        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(EnhancedTextbox), new PropertyMetadata(String.Empty));


        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(int), typeof(EnhancedTextbox), new PropertyMetadata(3));



        // Using a DependencyProperty as the backing store for Thickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(int), typeof(EnhancedTextbox), new PropertyMetadata(1));



        // Using a DependencyProperty as the backing store for PlaceHolder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.Register("PlaceHolder", typeof(string), typeof(EnhancedTextbox), new PropertyMetadata(String.Empty));

        public EnhancedTextbox()
        {
            InitializeComponent();
            this.DataContext = this;

            
        }


        private void contentText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (contentText.Text.Length > 0) placeHolderText.Visibility = Visibility.Hidden;
            else
                placeHolderText.Visibility = Visibility.Visible;
            Value = contentText.Text;
            TextChanged?.Invoke(this, e);
        }

        private void contentText_GotFocus(object sender, RoutedEventArgs e)
        {
            TimeSpan duration = TimeSpan.FromMilliseconds(400);
            DoubleAnimation animateOpacity = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = duration,
                AutoReverse = false
            };
            (border.Effect as DropShadowEffect).BeginAnimation(DropShadowEffect.OpacityProperty,
                                            animateOpacity);
        }

        private void contentText_LostFocus(object sender, RoutedEventArgs e)
        {
            TimeSpan duration = TimeSpan.FromMilliseconds(400);
            DoubleAnimation animateOpacity = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = duration,
                AutoReverse = false
            };
            (border.Effect as DropShadowEffect).BeginAnimation(DropShadowEffect.OpacityProperty,
                                            animateOpacity);
        }
    }
}
