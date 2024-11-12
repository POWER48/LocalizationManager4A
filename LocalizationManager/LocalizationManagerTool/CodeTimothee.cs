using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LocalizationManagerTool
{
    public partial class MainWindow : Window
    {
        private void New(object sender, RoutedEventArgs e)
        {
            dataGrid.Columns.Clear();

            DataColumn textColumn = new DataColumn();
            textColumn.ColumnName = "id";
            dataTable.Columns.Add(textColumn);

            DataColumn textColumn2 = new DataColumn();
            textColumn2.ColumnName = "en";
            dataTable.Columns.Add(textColumn2);

            DataColumn textColumn3 = new DataColumn();
            textColumn3.ColumnName = "fr";
            dataTable.Columns.Add(textColumn3);

            DataColumn textColumn4 = new DataColumn();
            textColumn4.ColumnName = "jp";
            dataTable.Columns.Add(textColumn4);

            dataGrid.ItemsSource = dataTable.DefaultView;

        }

        private void Add_Column(object sender, RoutedEventArgs e)
        {
            DataColumn textColumn = new DataColumn();
            textColumn.ColumnName = "Placeholder";
            dataTable.Columns.Add(textColumn);
        }
    }

}
