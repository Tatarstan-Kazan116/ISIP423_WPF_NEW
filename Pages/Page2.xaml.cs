using Pr14;
using Pr14.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Linq;

namespace Pr14.Pages
{
    public partial class Page2 : Page
    {
        Client Us;

        public Page2()
        {
            InitializeComponent();
        }

        public Page2(Client user) : this()
        {
            InitializeComponent();
            Us = user;
            if (LoginEnter != null) LoginEnter.Text = user.Email;
            if (PasswordEnter != null) PasswordEnter.Password = user.Password;
        }

        
        public bool Auth(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            try
            {
                var cl = Core.Context.Client
                    .FirstOrDefault(c => c.Email == login && c.Password == password);

                if (cl != null)
                {
                    Us = cl;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page3());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string login = LoginEnter != null ? LoginEnter.Text : "";
            string password = PasswordEnter != null ? PasswordEnter.Password : "";

            if (Auth(login, password))
            {
                MessageBox.Show("Вы успешно авторизовались!");
                NavigationService.Navigate(new Page1(Us));
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Пользователь не найден в Базе данных! Желаете зарегистрироваться?",
                    "Ошибка входа",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                {
                    NavigationService.Navigate(new Page3());
                }
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1());
        }
    }
}