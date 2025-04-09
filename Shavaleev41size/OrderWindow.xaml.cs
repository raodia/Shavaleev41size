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

    
        public OrderWindow(List<OrderProduct> selectedOrderProducts, List<Product> selectedProducts, string FIO)
        {
            InitializeComponent();

            var currentPickups = Shavaleev41Entities.getContext().PickUpPoint.ToList();
            
            PickupCb.ItemsSource = currentPickups;
            PickupCb.DisplayMemberPath = "PickUpVisual";


            ClientTB.Text = FIO;
            //TBOrderID.Text = selectedOrderProducts.First().OrderID.ToString();

            foreach(Product p in selectedProducts)
            {
                p.PrCount = 0;
                foreach (Product q in selectedProducts)
                {
                    if (p == q)
                    {
                        p.PrCount++;
                    }

                }

                foreach (OrderProduct q in selectedOrderProducts)
                {
                    if (p.ProductArticleNumber == q.ProductArticleNumber) p.PrCount = q.ProductCount;
                }
            }
            ShoeLV.ItemsSource = selectedProducts.Distinct();

            this.selectedOrderProducts = selectedOrderProducts;
            this.selectedProducts = selectedProducts;
            OrderDP.Text = DateTime.Now.ToString();
            DeliveryDP.Text = DateTime.Parse(OrderDP.Text).AddDays(3).ToString();

            
 
        }

        private void minusOneBtn_Click(object sender, RoutedEventArgs e)
        {
            var butt = (sender as Button).DataContext as Product;

            //var index = ShoeLV.Items.IndexOf(item);
            butt.PrCount--;
            if (butt.PrCount < 1)
            {
                selectedProducts.Remove(butt);
                
            }
            ShoeLV.UpdateLayout();
            ShoeLV.ItemsSource = selectedProducts.Distinct();
            if (selectedProducts.Count == 0)
            {
                this.Close();
            }

        }

        private void plusOneBtn_Click(object sender, RoutedEventArgs e)
        {
            var butt = (sender as Button).DataContext as Product;
            //var index = ShoeLV.Items.IndexOf(item);
            butt.PrCount++;
            ShoeLV.UpdateLayout();
            ShoeLV.ItemsSource = selectedProducts.Distinct();


        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            currentOrder.OrderID++;
            currentOrder.OrderDate = DateTime.Now;
            Shavaleev41Entities.getContext().Order.Add(currentOrder);
            Shavaleev41Entities.getContext().SaveChanges();
            MessageBox.Show("Заказ добавлен");
            this.Close();

        }
    }
}
