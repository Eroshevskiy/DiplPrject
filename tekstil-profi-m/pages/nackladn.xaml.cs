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
using tekstil_profi_m.models;

namespace tekstil_profi_m.pages
{
    /// <summary>
    /// Логика взаимодействия для nackladn.xaml
    /// </summary>
    public partial class nackladn : Window
    {
        private ObservableCollection<nacklItem> nacklItems = new ObservableCollection<nacklItem>();
        private ObservableCollection<Merch> merchCollection;
        private int _currentPage = 1;
        private int _countInPage = 3;

        public nackladn()
        {
            InitializeComponent();

            lv.ItemsSource = models.dipEntitie.GetContext().Merch.ToList();
            merchCollection = new ObservableCollection<Merch>(dipEntitie.GetContext().Merch.ToList());
            UpdateMerchCollection();
        }

        

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                UpdateMerchCollection();
            }
        }


        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            int totalItems = merchCollection.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / _countInPage);

            if (_currentPage < totalPages)
            {
                _currentPage++;
                UpdateMerchCollection();
            }
        }

        private void UpdateNacklViewButtonVisibility()
        {
            if (nacklItems.Any())
            {
                ShowNacklButton.Visibility = Visibility.Visible;
            }
            else
            {
                ShowNacklButton.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowNacklButton_Click(object sender, RoutedEventArgs e)
        {
           pechNacladn orderViewWindow = new pechNacladn(nacklItems);
            orderViewWindow.ShowDialog();

        }

        private void AddToNackl_Click(object sender, RoutedEventArgs e)
        {
            if (lv.SelectedItem != null)
            {
                Merch selectedMerch = lv.SelectedItem as Merch;

                nacklItem nacklItem = new nacklItem
                {
                    MerchId = selectedMerch.id,
                    MerchName = selectedMerch.name,
                    MerchMaterial = selectedMerch.material,
                    MerchRazmer = selectedMerch.razmer,
                    MerchColor = selectedMerch.color,
                    PhotoPath = selectedMerch.photo,
                };

                nacklItems.Add(nacklItem);
                UpdateNacklViewButtonVisibility();
            }
        }



        private void UpdateMerchCollection()
        {
            int startIndex = (_currentPage - 1) * _countInPage;
            var itemsForPage = merchCollection.Skip(startIndex).Take(_countInPage).ToList();
            lv.ItemsSource = itemsForPage;
        }


        public class nacklItem
        {
       
            public string name { get; set; }
            public string material { get; set; }
            public string razmer { get; set; }
            public string color { get; set; }
            public int MerchId { get; set; }
            public string MerchName { get; set; }
            public string MerchRazmer { get; set; }
            public string MerchColor { get; set; }
            public int Quantity { get; set; }
            public string MerchMaterial { get; set; }
            public string PhotoPath { get; set; }
            public string EticsPath { get; set; }
            public ObservableCollection<Otvetstvenie> OtvetCollection { get; set; }
            public Otvetstvenie OtvetPoint { get; set; }
            public string count { get; set; }
        }

        private void userClick(object sender, RoutedEventArgs e)
        {
            admin adm = new admin();
            Visibility = Visibility.Hidden;
            adm.Show();
        }

        private void MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        private void checkPlan(object sender, RoutedEventArgs e)
        {
            prosmPlan prosm = new prosmPlan();
            Visibility = Visibility.Hidden;
            prosm.Show();
        }
    }
}
