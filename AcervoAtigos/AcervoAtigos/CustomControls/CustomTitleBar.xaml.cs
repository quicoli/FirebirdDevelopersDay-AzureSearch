using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace AcervoAtigos.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomTitleBar.xaml
    /// </summary>
    public partial class CustomTitleBar : UserControl
    {
        public CustomTitleBar()
        {
            InitializeComponent();
        }

        private void BtnMinimizar_OnClick(object sender, RoutedEventArgs e)
        {
            if (Owner != null)
            {
                Owner.WindowState = System.Windows.WindowState.Minimized;
            }
        }

        private void BtnMaximizar_OnClick(object sender, RoutedEventArgs e)
        {
            if (Owner != null)
            {
                Owner.WindowState = Owner.WindowState == System.Windows.WindowState.Maximized ? WindowState.Normal : System.Windows.WindowState.Maximized;

            }
        }

        [Bindable(true)]
        public string WindowCaption
        {
            get { return (string)GetValue(WindowCaptionProperty); }
            set { SetValue(WindowCaptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WindowCaption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowCaptionProperty =
            DependencyProperty.Register("WindowCaption", typeof(string), typeof(CustomTitleBar),
                new UIPropertyMetadata("Caption da janela"));


        [Bindable(true)]
        public bool MinimizeButtonEnabled
        {
            get { return (bool)GetValue(MinimizeButtonEnabledProperty); }
            set { SetValue(MinimizeButtonEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinimizeButtonEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimizeButtonEnabledProperty =
            DependencyProperty.Register("MinimizeButtonEnabled", typeof(bool), typeof(CustomTitleBar),
                new UIPropertyMetadata(false));

        [Bindable(true)]
        public bool MaximizeButtonEnabled
        {
            get { return (bool)GetValue(MaximizeButtonEnabledProperty); }
            set { SetValue(MaximizeButtonEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinimizeButtonEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximizeButtonEnabledProperty =
            DependencyProperty.Register("MaximizeButtonEnabled", typeof(bool), typeof(CustomTitleBar),
                new UIPropertyMetadata(false));

        [Bindable(true)]
        public Window Owner
        {
            get { return (Window)GetValue(OwnerProperty); }
            set
            {
                SetValue(OwnerProperty, value);
            }
        }


        // Using a DependencyProperty as the backing store for Owner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OwnerProperty =
            DependencyProperty.Register("Owner", typeof(Window), typeof(CustomTitleBar), new UIPropertyMetadata(null));



        [Bindable(true)]
        public bool IsOwnerFocused
        {
            get { return (bool)GetValue(IsOwnerFocusedProperty); }
            set
            {
                SetValue(IsOwnerFocusedProperty, value);
                if (Owner != null)
                {
                    Owner.Opacity = value ? 100 : 80;
                }
            }
        }

        // Using a DependencyProperty as the backing store for IsOwnerFocused.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOwnerFocusedProperty =
            DependencyProperty.Register("IsOwnerFocused", typeof(bool), typeof(CustomTitleBar), new UIPropertyMetadata(true));

        [Bindable(true)]
        public ICommand CmdFechar
        {
            get { return (ICommand)GetValue(CmdFecharProperty); }
            set
            {
                SetValue(CmdFecharProperty, value);
            }
        }

        public static readonly DependencyProperty CmdFecharProperty =
            DependencyProperty.Register("CmdFechar", typeof(ICommand), typeof(CustomTitleBar), new UIPropertyMetadata(null));


        private void BtnFechar_OnClick(object sender, RoutedEventArgs e)
        {
            if (Owner != null)
            {
                Owner.Close();
            }
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (Owner != null)
                    Owner.DragMove();
            }

        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Owner != null)
            {
                Owner.WindowState = Owner.WindowState == System.Windows.WindowState.Maximized ? WindowState.Normal : System.Windows.WindowState.Maximized;

            }
        }
    }
}
