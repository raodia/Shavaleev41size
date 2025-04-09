using Shavaleev41size;
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

    
        public OrderWindow(List<OrderProduct> selectedOrderProducts, List<Product> selectedProducts, User currentUser)
        {
            InitializeComponent();

            var currentPickups = Shavaleev41Entities.getContext().PickUpPoint.ToList();
            
            PickupCb.ItemsSource = currentPickups;
            PickupCb.DisplayMemberPath = "PickUpVisual";

            if (currentUser != null)
            {
                ClientTB.Text = currentUser.UserSurname + " " + currentUser.UserName + " " + currentUser.UserPatronymic;
            }
            else ClientTB.Text = "Гость";
            currentOrder.OrderClient = currentUser;

            //TBOrderID.Text = selectedOrderProducts.First().OrderID.ToString();

            ///*int currentID = selectedOrderProducts.First().OrderID;
            //currentOrder.OrderID = currentID;
            //TBOrderID.Text = currentID.ToString();
            //*/
            //foreach (Product p in selectedProducts)
            //{
            //    p.PrCount = 0;
            //    foreach (Product q in selectedProducts)
            //    {
            //        if (p == q)
            //        {
            //            p.PrCount++;
            //        }

            //    }

            //    foreach (OrderProduct q in selectedOrderProducts)
            //    {
            //        if (p.ProductArticleNumber == q.ProductArticleNumber) p.PrCount = q.ProductCount;
            //    }
            //}
            //ShoeLV.ItemsSource = selectedProducts.Distinct();

            //this.selectedOrderProducts = selectedOrderProducts;
            //this.selectedProducts = selectedProducts;

            int currentID = selectedOrderProducts.First().OrderID; //определение номера текущего заказа
            currentOrder.OrderID = currentID;
            TBOrderID.Text = currentID.ToString();

            var sqlCommand = "SELECT IDENT_CURRENT('Order')";
            var nextID = Shavaleev41Entities.getContext().Database.SqlQuery<decimal>(sqlCommand).FirstOrDefault() + 1;
            TBOrderID.Text = nextID.ToString();


            foreach (Product product in selectedProducts)
            {
                var orderProduct = selectedOrderProducts.FirstOrDefault(op => op.ProductArticleNumber == product.ProductArticleNumber);
                if (orderProduct != null)
                {
                    product.PrCount = orderProduct.ProductCount;
                }
                else
                {
                    product.PrCount = 1; // Если не найден, устанавливаем 1 (опционально)
                }
            }

            ShoeLV.ItemsSource = selectedProducts;

            this.selectedOrderProducts = selectedOrderProducts;
            this.selectedProducts = selectedProducts;
            OrderDP.Text = DateTime.Now.ToString();

            //foreach (Product product in selectedProducts)
            //{
            //    var orderProduct = selectedOrderProducts.FirstOrDefault(op => op.ProductArticleNumber == product.ProductArticleNumber);
            //    if (orderProduct != null)
            //    {
            //        product.PrCount = orderProduct.ProductCount;
            //    }
            //    else
            //    {
            //        product.PrCount = 1; // Если не найден, устанавливаем 1 (опционально)
            //    }
            //}

            //ShoeLV.ItemsSource = selectedProducts;

            //this.selectedOrderProducts = selectedOrderProducts;
            //this.selectedProducts = selectedProducts;
            OrderDP.Text = DateTime.Now.ToString();

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
            currentOrder.PickUpPoint = PickupCb.SelectedItem as PickUpPoint;

            bool Status = false;

            foreach (var p in selectedProducts)
            {
                if (p.ProductQuantityInStock < (p.PrCount + 3))
                {
                    Status = true;
                }
            }

            DateTime deliveryDate = OrderDP.SelectedDate.Value;
            deliveryDate = Status ? deliveryDate.AddDays(6) : deliveryDate.AddDays(3);
            currentOrder.OrderDeliveryDate = deliveryDate;

            Shavaleev41Entities.getContext().Order.Add(currentOrder);
            Shavaleev41Entities.getContext().SaveChanges();
            MessageBox.Show("Заказ добавлен");
            this.Close();

        }

       
        

    }



}




/*
public OrderWindow(List<OrderProduct> selectedOrderProducts, List<Product> selectedProducts, User user)
{
    InitializeComponent();

    
    int nextOrderID = GetNextOrderID();
    TBOrderID.Text = nextOrderID.ToString();


    foreach (Product product in selectedProducts)
    {
        var orderProduct = selectedOrderProducts.FirstOrDefault(op => op.ProductArticleNumber == product.ProductArticleNumber);
        if (orderProduct != null)
        {
            product.Quantity = orderProduct.ProductCount;
        }
        else
        {
            product.Quantity = 1; // Если не найден, устанавливаем 1 (опционально)
        }
    }

    ProductListView.ItemsSource = selectedProducts;

    this.selectedOrderProducts = selectedOrderProducts;
    this.selectedProducts = selectedProducts;
    OrderDP.Text = DateTime.Now.ToString();
}

private void BtnPlus_Click(object sender, RoutedEventArgs e)
{
    var prod = (sender as Button).DataContext as Product;
    var selectedOP = selectedOrderProducts.FirstOrDefault(p => p.ProductArticleNumber == prod.ProductArticleNumber);

    if (selectedOP != null)
    {
        selectedOP.ProductCount++;
        prod.Quantity = selectedOP.ProductCount;
        SetDeliveryDate();
        CalculateTotalAndDiscount();
        ProductListView.Items.Refresh();
    }
*/