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

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for EnhancedButton.xaml
    /// </summary>
    
    public enum EnhancedButtonType
    {
        None,
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info,
        Light,
        Dark
    }

    public partial class EnhancedButton : UserControl
    {

        public event MouseButtonEventHandler Click;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public EnhancedButtonType Type
        {
            get { return (EnhancedButtonType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }



        public int CornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }



        public Thickness MyPadding
        {
            get { return (Thickness)GetValue(MyPaddingProperty); }
            set { SetValue(MyPaddingProperty, value); }
        }


        public SolidColorBrush MyBackground
        {
            get { return (SolidColorBrush)GetValue(MyBackgroundProperty); }
            set { SetValue(MyBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyBackgroundProperty =
            DependencyProperty.Register("MyBackground", typeof(SolidColorBrush), typeof(EnhancedButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 123, 255))));


        // Using a DependencyProperty as the backing store for MyPadding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPaddingProperty =
            DependencyProperty.Register("MyPadding", typeof(Thickness), typeof(EnhancedButton), new PropertyMetadata(new Thickness(0)));



        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(int), typeof(EnhancedButton), new PropertyMetadata(4));



        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(EnhancedButtonType), typeof(EnhancedButton), new PropertyMetadata(EnhancedButtonType.None));


        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(EnhancedButton), new PropertyMetadata(String.Empty));

        public EnhancedButton()
        {
            InitializeComponent();
        }

        private void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Click?.Invoke(this, e);
        }

        private void enhancedButton_Loaded(object sender, RoutedEventArgs e)
        {
            switch (Type)
            {
                case EnhancedButtonType.Primary:
                    border.Background = new SolidColorBrush(Color.FromRgb(0, 123, 255));
                    break;
                case EnhancedButtonType.Secondary:
                    border.Background = new SolidColorBrush(Color.FromRgb(108, 117, 125));
                    break;
                case EnhancedButtonType.Success:
                    border.Background = new SolidColorBrush(Color.FromRgb(40, 167, 69));
                    break;
                case EnhancedButtonType.Danger:
                    border.Background = new SolidColorBrush(Color.FromRgb(220, 53, 69));
                    break;
                case EnhancedButtonType.Warning:
                    border.Background = new SolidColorBrush(Color.FromRgb(255, 193, 7));
                    button.Foreground = new SolidColorBrush(Color.FromRgb(52, 58, 64));
                    break;
                case EnhancedButtonType.Info:
                    border.Background = new SolidColorBrush(Color.FromRgb(23, 162, 184));
                    break;
                case EnhancedButtonType.Light:
                    border.Background = new SolidColorBrush(Color.FromRgb(248, 249, 250));
                    button.Foreground = new SolidColorBrush(Color.FromRgb(52, 58, 64));
                    break;
                case EnhancedButtonType.Dark:
                    border.Background = new SolidColorBrush(Color.FromRgb(52, 58, 64));
                    break;
            }
        }
    }
}
