using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VectorDrawForms.Views
{
    /// <summary>
    /// Interaction logic for TransparentButton.xaml
    /// </summary>
    public partial class TransparentButton : UserControl
    {
        #region Constructor
        public TransparentButton()
        {
            InitializeComponent();
        }
        #endregion

        #region Dependency Properties
        public int ButtonHeight
        {
            get { return (int)GetValue(ButtonHeightProperty); }
            set { SetValue(ButtonHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonHeightProperty =
            DependencyProperty.Register("ButtonHeight", typeof(int), typeof(TransparentButton), new PropertyMetadata(30));

        public int ButtonWidth
        {
            get { return (int)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonWidthProperty =
            DependencyProperty.Register("ButtonWidth", typeof(int), typeof(TransparentButton), new PropertyMetadata(90));

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(TransparentButton), new PropertyMetadata(""));

        public SolidColorBrush ForegroundColor
        {
            get { return (SolidColorBrush)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ForegroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor", typeof(SolidColorBrush), typeof(TransparentButton), new PropertyMetadata(Brushes.White));

        public ICommand ButtonCommand
        {
            get
            {
                return (ICommand)GetValue(ButtonCommandProperty);
            }
            set { SetValue(ButtonCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register("ButtonCommand", typeof(ICommand), typeof(TransparentButton), new PropertyMetadata(null));
        #endregion

        #region Methods
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonCommand != null && ButtonCommand.CanExecute(null))
                ButtonCommand.Execute(null);
        }
        #endregion

    }
}
