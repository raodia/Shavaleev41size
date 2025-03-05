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
    /// <summary>
    /// Логика взаимодействия для AuthPage2.xaml
    /// </summary>
    /// 
    
    public partial class AuthPage2 : Page
    {
        public AuthPage2()
        {
            InitializeComponent();
        }

        private void asGuestButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new feetPage(null));
            inPassword.Text = "";
            inUsername.Text = "";
            captchaAns.Text = "";
            captcha.Visibility = Visibility.Hidden;
            captchaAns.Visibility = Visibility.Hidden;
            attention.Visibility = Visibility.Hidden;

        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = inUsername.Text;
            string password = inPassword.Text;
            if (login == "" || password == "" )
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }
            User user = Shavaleev41Entities.getContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
            string rightAns = captcha1.Text + captcha3.Text + captcha5.Text + captcha7.Text;

            if (user != null)
            {
                if ((captcha.Visibility == Visibility.Hidden || rightAns == captchaAns.Text))
                {
                    Random rand = new Random();


                    Manager.MainFrame.Navigate(new feetPage(user));
                    inPassword.Text = "";
                    inUsername.Text = "";
                    captchaAns.Text = "";
                    //string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    //loginButton.IsEnabled = true;
                    //captcha1.Text = (chars[rand.Next(0, chars.Length)]).ToString();
                    //captcha7.Text = (chars[rand.Next(0, chars.Length)]).ToString();
                    //captcha5.Text = (chars[rand.Next(0, chars.Length)]).ToString();
                    //captcha3.Text = (chars[rand.Next(0, chars.Length)]).ToString();

                    //captcha2.Text = ((char)rand.Next(0x0410, 0x44F)).ToString();
                    //captcha4.Text = ((char)rand.Next(0x0410, 0x44F)).ToString();
                    //captcha6.Text = ((char)rand.Next(0x0410, 0x44F)).ToString();

                    captcha.Visibility = Visibility.Hidden;
                    captchaAns.Visibility = Visibility.Hidden;
                    attention.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("Капча провалена: " + rightAns + "  !=  " + captchaAns.Text);

                }
            }
            else
            {
                Random rand = new Random();

                loginButton.IsEnabled = false;

                MessageBox.Show("Ввведены неверные данные");
                captcha.Visibility = Visibility.Visible;
                captchaAns.Visibility = Visibility.Visible;
                attention.Visibility = Visibility.Visible;

                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                captcha1.Text = (chars[rand.Next(0, chars.Length)]).ToString();
                captcha7.Text = (chars[rand.Next(0, chars.Length)]).ToString();
                captcha5.Text = (chars[rand.Next(0, chars.Length)]).ToString();
                captcha3.Text = (chars[rand.Next(0, chars.Length)]).ToString();

                captcha2.Text = ((char)rand.Next(0x0410, 0x44F)).ToString();
                captcha4.Text = ((char)rand.Next(0x0410, 0x44F)).ToString();
                captcha6.Text = ((char)rand.Next(0x0410, 0x44F)).ToString();

                await Task.Delay(10000);

                //rightAns = captcha1.Text + captcha3.Text + captcha5.Text + captcha7.Text;
                //DateTime NowTime = DateTime.Now;
                //DateTime NeedTime = DateTime.Now.AddSeconds(10);

                //while (NowTime < NeedTime)
                //{
                //    NowTime = DateTime.Now;
                //}

                loginButton.IsEnabled = true;

            }
        }

        // loginDEftn2018
        // gPq+a}

   
    }
}
