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

namespace Shavaleev41size
{
    public partial class feetPage : Page
    {
        int _newOrderID;
        User _currentUser;
        List<Product> _Tablelist;
        List<Product> _selectedProducts = new List<Product>();
        private Order _currentOrder = new Order();
        List<OrderProduct> _selectedOrderProducts = new List<OrderProduct>();

        private void Update()
        {
            var currentProd = Shavaleev41Entities.getContext().Product.ToList();
            maxCountProd.Text = (currentProd.Count).ToString();

            switch (CbDisc.SelectedIndex)
            {
                case 0:
                    currentProd = Shavaleev41Entities.getContext().Product.ToList();
                    break;
                case 1:
                    currentProd = currentProd.Where(a => (Convert.ToInt32(a.ProductDiscountAmount) >= 0 && Convert.ToInt32(a.ProductDiscountAmount) < 10)).ToList();
                    break;
                case 2:
                    currentProd = currentProd.Where(a => (Convert.ToInt32(a.ProductDiscountAmount) >= 10 && Convert.ToInt32(a.ProductDiscountAmount) < 15)).ToList();
                    break;
                case 3:
                    currentProd = currentProd.Where(a => (Convert.ToInt32(a.ProductDiscountAmount) >= 15)).ToList();
                    break;
                default:
                    currentProd = Shavaleev41Entities.getContext().Product.ToList();
                    break;
            }

            currentProd = currentProd.Where(a => a.ProductName.ToLower().Contains(search.Text.ToLower())).ToList();

            Listv.ItemsSource = currentProd.ToList();

            if (descending.IsChecked.Value)
            {
                Listv.ItemsSource = currentProd.OrderByDescending(a => a.ProductCost).ToList();
            }
            if (ascending.IsChecked.Value)
            {
                Listv.ItemsSource = currentProd.OrderBy(a => a.ProductCost);
            }

            countProd.Text = (currentProd.Count).ToString();
        }

            public feetPage(User currentUser)
        {
            this._currentUser = currentUser;
            InitializeComponent();

            Manager.OrderBtn = openWindow;

            if (currentUser != null)
            {
                fullname.Text = currentUser.UserSurname + " " + currentUser.UserName + " " + currentUser.UserPatronymic;


                switch (currentUser.UserRole)
                {
                    case 1:
                        role.Text = "Клиент";
                        break;
                    case 2:
                        role.Text = "Менеджер";
                        break;
                    default:
                        role.Text = "Администратор";
                        break;
                }
            }
            else
            {
                fullname.Text = "Гость";
                role.Text = "Гость";
            }
            var currentData = Shavaleev41Entities.getContext().Product.ToList();
            Listv.ItemsSource = currentData;

            CbDisc.SelectedIndex = 0;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());

        }

        private void CbDisc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();

        }

        private void ascending_Checked(object sender, RoutedEventArgs e)
        {
            Update();

        }

        private void descending_Checked(object sender, RoutedEventArgs e)
        {
            Update();

        }

        //public List<OrderProduct> selectedProducts =  
        //List<OrderProduct> selectedOrderProducts = new List<OrderProduct>();
        //List<Product> selectedProducts = new List<Product>();



        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //var currentProd = Shavaleev41Entities.getContext().Product.ToList();
            //var currentOrderProd = Shavaleev41Entities.getContext().OrderProduct.ToList();

            //if (openWindow.Visibility == Visibility.Hidden || selectedProducts.Count > 0)
            //{
            //    //if (selectedOrderProducts.Contains(currentOrderProd[Listv.SelectedIndex]) || selectedProducts.Contains(currentProd[Listv.SelectedIndex]))
            //    //{

            //    //}
            //    //selectedOrderProducts.Add(currentOrderProd[Listv.SelectedIndex]);
            //    selectedProducts.Add(currentProd[Listv.SelectedIndex]);
            //}
            //openWindow.Visibility = Visibility.Visible;



            if (Listv.SelectedIndex >= 0)
            {
                var prod = Listv.SelectedItem as Product;
                _selectedProducts.Add(prod);

                var newOrderProd = new OrderProduct();
                //newOrderProd.OrderID = _newOrderID;

                newOrderProd.ProductArticleNumber = prod.ProductArticleNumber;
                newOrderProd.ProductCount = 1;

                var selectedOP = _selectedOrderProducts.Where(p => Equals(p.ProductArticleNumber, prod.ProductArticleNumber));

                if (selectedOP.Count() == 0)
                {
                    _selectedOrderProducts.Add(newOrderProd);
                    _selectedProducts.Add(prod);
                }
                else
                {
                    foreach (OrderProduct p in _selectedOrderProducts)
                    {
                        if (p.ProductArticleNumber == prod.ProductArticleNumber)
                        {
                            p.ProductCount++;
                        }
                    }
                }

                openWindow.Visibility = Visibility.Visible;
                Listv.SelectedIndex = -1;
            }
        }

        //private void ShowOrderBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    //selectedProducts = selectedProducts.Distinct().ToList();
        //    //OrderWindow orderWindow = new OrderWindow(selectedOrderProducts, selectedProducts, fullname.Text);
        //    //orderWindow.ShowDialog();
        //}

        private void openWindow_Click(object sender, RoutedEventArgs e)
        {

            _selectedProducts = _selectedProducts.Distinct().ToList();

           foreach (var product in _selectedProducts)
            {
               var orderProduct = _selectedOrderProducts.FirstOrDefault(p =>
                    p.ProductArticleNumber == product.ProductArticleNumber);

                if (orderProduct != null)
                {
                    product.PrCount = orderProduct.ProductCount;
                }
                else
                {
                   product.PrCount = 1;
                }
            }

            OrderWindow orderWindow = new OrderWindow(_selectedOrderProducts, _selectedProducts, _currentUser);
            bool? result = orderWindow.ShowDialog();

            if (result == true)
            {
                _selectedProducts.Clear();
                _selectedOrderProducts.Clear();
                Listv.Items.Refresh();



                openWindow.Visibility = _selectedProducts.Any() ? Visibility.Visible : Visibility.Hidden;

        }

        //if (_selectedProducts.Count == 0)
        //{
        //    orderWindow.Visibility = Visibility.Hidden;
        //}
        //else
        //{
        //    orderWindow.Visibility = Visibility.Visible;
        //}
    }
    }
}