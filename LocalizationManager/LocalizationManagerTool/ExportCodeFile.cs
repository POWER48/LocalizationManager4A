using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LocalizationManagerTool
{    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            ButtonExport.IsEnabled = true;
        }

        private void Button_Export(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            
        }
    }

}
