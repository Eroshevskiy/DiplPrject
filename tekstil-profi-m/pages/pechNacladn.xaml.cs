using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using static tekstil_profi_m.pages.nackladn;

namespace tekstil_profi_m.pages
{
    /// <summary>
    /// Логика взаимодействия для pechNacladn.xaml
    /// </summary>
    public partial class pechNacladn : Window
    {
        private ObservableCollection<nackladn.nacklItem> nacklItems;
        




        public pechNacladn(ObservableCollection<nackladn.nacklItem> nacklItems)
        {
            InitializeComponent();
            foreach (var nacklItem in nacklItems)
            {
                nacklItem.OtvetCollection = new ObservableCollection<Otvetstvenie>(dboconnect.modeldb.Otvetstvenie);
            }
            this.nacklItems = nacklItems;
            OrderLV.ItemsSource = nacklItems;

        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    return;
                }
            }
        }


        private void ydalClick(object sender, RoutedEventArgs e)
        {
            if (OrderLV.SelectedItem != null)
            {
                nacklItem selectednacklItem = OrderLV.SelectedItem as nacklItem;
                if (selectednacklItem != null)
                {
                    nacklItems.Remove(selectednacklItem);
                    
                }
            }
        }

        

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(this);

            
            Image image = new Image();
            image.Source = renderTargetBitmap;

            
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                
                printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;

                
                printDialog.PrintVisual(image, "Print Page");

        
            }



        }
        

        private void SavePlan(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new dipEntitie())
                {
                    foreach (var nacklItem in nacklItems)
                    {
                        var order = new Plan
                        {

                            id_merch = nacklItem.MerchId,
                            id_otvetst = nacklItem.OtvetPoint?.id ?? 1,
                            material = nacklItem.MerchMaterial,
                            name = nacklItem.MerchName,
                            razmer = nacklItem.MerchRazmer,
                            color = nacklItem.MerchColor,
                            photo = nacklItem.PhotoPath,
                            count = nacklItem.count 

                        };

                        context.Plan.Add(order);

                       
                    }

                    context.SaveChanges();
                }
                MessageBox.Show("План сохранен успешно!", "Сохранение успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении плана в базу данных: {ex.Message}", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
    }
}
