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


        private void CalculateTotalAndDiscount()
        {
            decimal total = 0;
            decimal discount = 0;

            foreach (var orderProduct in selectedOrderProducts)
            {
                var product1 = selectedProducts.FirstOrDefault(p => p.ProductArticleNumber == orderProduct.ProductArticleNumber);
                if (product1 == null) continue;

                decimal price = product1.ProductCost;
                decimal discountPercent = 0;
                if (product1.ProductDiscountAmount != null)
                discountPercent = product1.ProductDiscountAmount;

         
                total += orderProduct.ProductCount * price;

    
                discount += orderProduct.ProductCount * price * (discountPercent / 100);
            }

            decimal discountedTotal = total - discount;

            var disc = discount.ToString().Split(',');
            var discTotal = discountedTotal.ToString().Split(',');
            var discRes = "00";
            if (Convert.ToInt32(disc[1]) > 0)
            {
                discRes = disc[1];
            }

            var discTRes = "00";
            if (Convert.ToInt32(discTotal[1]) > 0)
            {
                discTRes = discTotal[1];
            }


            discountTB.Text = "Общая сумма: " + total.ToString() + "Р, скидка: " + disc[0] + "," + discRes + "Р, сумма со скидкой: " + discTotal[0] + ","+ discTRes.ToString() + " Р.";
        }

        public OrderWindow(List<OrderProduct> selectedOrderProducts, List<Product> selectedProducts, User currentUser)
        {
            InitializeComponent();



            var currentPickups = Shavaleev41Entities.getContext().PickUpPoint.ToList();
            
            PickupCb.ItemsSource = currentPickups;
            PickupCb.DisplayMemberPath = "PickUpVisual";

            PickupCb.SelectedIndex = 0;

            if (currentUser != null)
            {
                ClientTB.Text = currentUser.UserSurname + " " + currentUser.UserName + " " + currentUser.UserPatronymic;
                currentOrder.OrderClientID = currentUser.UserID;
            }
            else
            {
                ClientTB.Text = "Гость";
                currentOrder.OrderClientID = null;
            }

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

            int currentID = selectedOrderProducts.First().OrderID; 
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
            //        product.PrCount = 1;
            //    }
            //}

            //ShoeLV.ItemsSource = selectedProducts;

            //this.selectedOrderProducts = selectedOrderProducts;
            //this.selectedProducts = selectedProducts;
            OrderDP.Text = DateTime.Now.ToString();

            DeliveryDP.Text = deliveryDate.ToString();
            setDeliveryDate();



            CalculateTotalAndDiscount();


        }

        public void setDeliveryDate()
        {
            bool Status = false;

            foreach (var p in selectedProducts)
            {
                if (p.ProductQuantityInStock < (p.PrCount + 3))
                {
                    Status = true;
                }
            }

            deliveryDate = OrderDP.SelectedDate.Value;
            deliveryDate = Status ? deliveryDate.AddDays(6) : deliveryDate.AddDays(3);
            currentOrder.OrderDeliveryDate = deliveryDate;
            DeliveryDP.Text = deliveryDate.ToString();
            
        }

        /*
         * 
         * var prod = (sender as Button).DataContext as Product;
            var selectedOP = selectedOrderProducts.FirstOrDefault(p => p.ProductArticleNumber == prod.ProductArticleNumber);

            if (selectedOP != null)
            {
                if (selectedOP.ProductCount > 1)
                {
                    selectedOP.ProductCount--;
                    prod.Quantity = selectedOP.ProductCount; // Синхронизируем Quantity
                    SetDeliveryDate();
                    CalculateTotalAndDiscount();
                    ProductListView.Items.Refresh();
                }
                else
                {
                    // Удаляем OrderProduct из списка
                    selectedOrderProducts.Remove(selectedOP);

                    // Находим Product в selectedProducts по артикулу (чтобы избежать проблем с ссылками)
                    var productToRemove = selectedProducts.FirstOrDefault(p => p.ProductArticleNumber == prod.ProductArticleNumber);
                    if (productToRemove != null)
                    {
                        selectedProducts.Remove(productToRemove);
                    }

                    // Обновляем интерфейс
                    ProductListView.Items.Refresh();
                    // Перепривязываем данные, чтобы обновить интерфейс
                    ProductListView.ItemsSource = null;
                    ProductListView.ItemsSource = selectedProducts;
                    SetDeliveryDate();
                    CalculateTotalAndDiscount();
                    ProductListView.Items.Refresh();

                    if (selectedProducts.Count == 0)
                    {
                        Manager.OrderBtn.Visibility = Visibility.Hidden;
                        this.Close();
                    }
                }
         * 
         */

        private void minusOneBtn_Click(object sender, RoutedEventArgs e)
        {
            var butt = (sender as Button).DataContext as Product;
            var selectedOP = selectedOrderProducts.FirstOrDefault(p => p.ProductArticleNumber == butt.ProductArticleNumber);

            if (selectedOP != null)
            {
                if (selectedOP.ProductCount > 1)
                {
                    selectedOP.ProductCount--;
                    butt.PrCount = selectedOP.ProductCount; // Синхронизируем Quantity
                    setDeliveryDate();
                    CalculateTotalAndDiscount();
                   ShoeLV.Items.Refresh();
                }
                else
                {
                    // Удаляем OrderProduct из списка
                    selectedOrderProducts.Remove(selectedOP);

                    // Находим Product в selectedProducts по артикулу (чтобы избежать проблем с ссылками)
                    var productToRemove = selectedProducts.FirstOrDefault(p => p.ProductArticleNumber == butt.ProductArticleNumber);
                    if (productToRemove != null)
                    {
                        selectedProducts.Remove(productToRemove);
                    }

                    // Обновляем интерфейс
                    ShoeLV.Items.Refresh();
                    // Перепривязываем данные, чтобы обновить интерфейс
                    ShoeLV.ItemsSource = null;
                    ShoeLV.ItemsSource = selectedProducts;
                    setDeliveryDate();
                    CalculateTotalAndDiscount();
                    ShoeLV.Items.Refresh();

                    if (selectedProducts.Count == 0)
                    {
                        Manager.OrderBtn.Visibility = Visibility.Hidden;
                        this.Close();
                    }
                }
            }
        


            //    var butt = (sender as Button).DataContext as Product;

            ////var index = ShoeLV.Items.IndexOf(item);
            //butt.PrCount--;
            //currentOrderProduct.ProductCount--;
            
            //if (butt.PrCount < 1)
            //{
            //    selectedProducts.Remove(butt);
                
            //}
            //ShoeLV.UpdateLayout();
            //ShoeLV.ItemsSource = selectedProducts.Distinct();

            //if (selectedProducts.Count == 0)
            //{
            //    Manager.OrderBtn.Visibility = Visibility.Hidden;
            //    ShoeLV.ItemsSource = new List<Product>();
            //    this.Close();
                
            //}
            //CalculateTotalAndDiscount();


        }

        private void plusOneBtn_Click(object sender, RoutedEventArgs e)
        {
            var butt = (sender as Button).DataContext as Product;
            var selectedOP = selectedOrderProducts.FirstOrDefault(p => p.ProductArticleNumber == butt.ProductArticleNumber);

            if (selectedOP != null)
            {
                selectedOP.ProductCount++;
                butt.PrCount = selectedOP.ProductCount;
                setDeliveryDate();
                CalculateTotalAndDiscount();
                ShoeLV.Items.Refresh();
            }


            ////var index = ShoeLV.Items.IndexOf(item);
            //butt.PrCount++;
            //ShoeLV.ItemsSource = selectedProducts.Distinct();
            //CalculateTotalAndDiscount();


        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {

            currentOrder.OrderDate = DateTime.Now;
            currentOrder.OrderPickupPoint = PickupCb.SelectedIndex + 1;

            bool Status = false;

            foreach (var p in selectedProducts)
            {
                if (p.ProductQuantityInStock < (p.PrCount + 3))
                {
                    Status = true;
                }
            }

            deliveryDate = OrderDP.SelectedDate.Value;
            deliveryDate = Status ? deliveryDate.AddDays(6) : deliveryDate.AddDays(3);
            currentOrder.OrderDeliveryDate = deliveryDate;

            currentOrder.OrderCode = Shavaleev41Entities.getContext().Order.OrderByDescending(p => p.OrderCode).First().OrderCode + 1;
            currentOrder.OrderStatus = "Новый";

            foreach (var op in selectedOrderProducts)
            {
                op.OrderID = currentOrder.OrderID;
                Shavaleev41Entities.getContext().OrderProduct.Add(op);
            }
            
            Shavaleev41Entities.getContext().Order.Add(currentOrder);
            
            Shavaleev41Entities.getContext().SaveChanges();
            MessageBox.Show("Заказ добавлен");
            this.DialogResult = true;

            Manager.OrderBtn.Visibility = Visibility.Hidden;
            this.Close();

        }


        DateTime deliveryDate;

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