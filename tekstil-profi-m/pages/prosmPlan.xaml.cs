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
using tekstil_profi_m.classes;
using tekstil_profi_m.models;

namespace tekstil_profi_m.pages
{
    /// <summary>
    /// Логика взаимодействия для prosmPlan.xaml
    /// </summary>
    public partial class prosmPlan : Window
    {
        private ObservableCollection<nackladn.nacklItem> nacklItems;
        private ObservableCollection<Plan> planCollection;
        public prosmPlan()
        {
            InitializeComponent();
            planCollection = new ObservableCollection<Plan>(dipEntitie.GetContext().Plan.ToList());
            BDPlan.ItemsSource = planCollection;


        }

        private void nazClick(object sender, RoutedEventArgs e)
        {
            nackladn nack = new nackladn();
            Visibility = Visibility.Hidden;
            nack.Show();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            var planDell = BDPlan.SelectedItems.Cast<Plan>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {planDell.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    IEnumerable<Plan> enumerable = dipEntitie.GetContext().Plan.RemoveRange((IEnumerable<Plan>)planDell);
                    dipEntitie.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    BDPlan.ItemsSource = dipEntitie.GetContext().Plan.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
