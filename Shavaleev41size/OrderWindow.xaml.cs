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
using System.Windows.Shapes;

namespace Shavaleev41size
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        List<OrderProduct> selectedOrderProducts = new List<OrderProduct>();
        List<Product> selectedProducts = new List<Product>();
        private Order currentOrder = new Order();
        private OrderProduct currentOrderProduct = new OrderProduct();

        public void SetDeliveryDate()
        {

        }
        public OrderWindow(List<OrderProduct> selectedOrderProducts, List<Product> selectedProducts, string FIO)
        {
            InitializeComponent();
            //this.selectedOrderProducts = selectedOrderProducts;
            //this.selectedProducts = selectedProducts;
            //this.currentOrder = currentOrder;
            //this.currentOrderProduct = currentOrderProduct;
            var currentPickups = Shavaleev41Entities.getContext().PickUpPoint.ToList();
            PickupCb.ItemsSource = currentPickups;

            ClientTB.Text = FIO;
            TBOrderID.Text = selectedOrderProducts.First().OrderID.ToString();

            ShoeLV.ItemsSource = selectedProducts;
            foreach(Product p in selectedProducts)
            {
                p.ProductQuantityInStock = 1;
                foreach(OrderProduct q in selectedOrderProducts)
                {
                    if (p.ProductArticleNumber == q.ProductArticleNumber) p.ProductQuantityInStock = q.ProductCount;
                }
            }

            this.selectedOrderProducts = selectedOrderProducts;
            this.selectedProducts = selectedProducts;
            OrderDP.Text = DateTime.Now.ToString();
            SetDeliveryDate();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            if (ShoeLV.SelectedIndex == 0)
            {
                var prod = ShoeLV.SelectedItem as Product;
                selectedProducts.Add(prod);

                var newOrderProd = new OrderProduct();
                newOrderProd.OrderID = 0;

                newOrderProd.ProductArticleNumber = prod.ProductArticleNumber;
                newOrderProd.ProductCount = 1;

                var selectedOP = selectedOrderProducts.Where(p => Equals(p.ProductArticleNumber, prod.ProductArticleNumber));

                if (selectedOP.Count() == 0)
                {
                    selectedOrderProducts.Add(newOrderProd);
                }
                else
                {
                    foreach (OrderProduct p in selectedOrderProducts)
                    {
                        if (p.ProductArticleNumber == prod.ProductArticleNumber)
                        {
                            p.ProductCount++;
                        }
                    }
                }

                OrderBtn.Visibility = Visibility.Visible;
                ShoeLV.SelectedIndex = -1;
            }
        }
    }
}
