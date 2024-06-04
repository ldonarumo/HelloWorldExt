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

namespace HelloWorldExt
{
    /// <summary>
    /// Interaction logic for CustomTitleControl.xaml
    /// </summary>
    public partial class CustomTitleControl : UserControl
    {
        public CustomTitleControl()
        {
            InitializeComponent();
        }

        public void SetTitle(string title)
        {
            CustomTitleTextBlock.Text = title;
        }
    }
}
