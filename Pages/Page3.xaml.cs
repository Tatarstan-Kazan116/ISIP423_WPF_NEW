using Microsoft.Win32;
using Pr14;
using Pr14.Pages;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Pr14.Pages
{
    public partial class Page3 : Page
    {
        public bool EmPass { get; set; }
        public bool passwordPass { get; set; }
        public bool FioPass { get; set; }
        public bool PhonePass { get; set; }
        public bool AgePass { get; set; }
        Client user = new Client();

        public Page3()
        {
            InitializeComponent();
            Loaded += Page3_Loaded;
            if (Registr != null) Registr.IsEnabled = false;
        }

        
        public bool Register(string email, string password, string fio, string age, string phone)
        {
            System.Diagnostics.Debug.WriteLine($"\n=== МЕТОД REGISTER ===");
            System.Diagnostics.Debug.WriteLine($"Email: {email}");

            // Валидация
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                System.Diagnostics.Debug.WriteLine("❌ Провал: email");
                return false;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length <= 5)
            {
                System.Diagnostics.Debug.WriteLine("❌ Провал: пароль");
                return false;
            }

            if (string.IsNullOrWhiteSpace(fio))
            {
                System.Diagnostics.Debug.WriteLine("❌ Провал: fio");
                return false;
            }

            if (!int.TryParse(age, out int ageValue) || ageValue <= 0)
            {
                System.Diagnostics.Debug.WriteLine("❌ Провал: возраст");
                return false;
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                System.Diagnostics.Debug.WriteLine("❌ Провал: телефон пустой");
                return false;
            }

            // Проверка телефона
            string cleanPhone = phone.Trim();
            bool isPhoneValid = (cleanPhone.StartsWith("+") && cleanPhone.Length == 12) ||
                                (!cleanPhone.StartsWith("+") && cleanPhone.Length == 11);
            if (!isPhoneValid)
            {
                System.Diagnostics.Debug.WriteLine("❌ Провал: телефон невалиден");
                return false;
            }

            System.Diagnostics.Debug.WriteLine("✅ Все проверки пройдены");

            try
            {
                var existingUser = Core.Context.Client.FirstOrDefault(u => u.Email == email);
                System.Diagnostics.Debug.WriteLine($"Существующий пользователь: {existingUser != null}");

                if (existingUser != null)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Пользователь уже существует");
                    return false;
                }

                var newUser = new Client
                {
                    Email = email,
                    Password = password,
                    Fio = fio,
                    Age = ageValue,
                    PhoneNum = phone
                };

                System.Diagnostics.Debug.WriteLine("Добавляем пользователя...");
                Core.Context.Client.Add(newUser);

                System.Diagnostics.Debug.WriteLine("Сохраняем...");
                Core.Context.SaveChanges();

                System.Diagnostics.Debug.WriteLine("✅ Успешно!");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"\n❌❌ ИСКЛЮЧЕНИЕ ❌❌");
                System.Diagnostics.Debug.WriteLine($"Тип: {ex.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"Сообщение: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"\nInner exception:");
                    System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);
                }

                return false;
            }
        }

        private void CheckAllFieldsFilled()
        {
            bool isAllFilled = !string.IsNullOrWhiteSpace(LoginEnter?.Text) &&
                               !string.IsNullOrWhiteSpace(PasswordEnter?.Password) &&
                               !string.IsNullOrWhiteSpace(FIOEnter?.Text) &&
                               user.Age > 0 &&
                               !string.IsNullOrWhiteSpace(PhoneEnter?.Text);

            if (Registr != null)
                Registr.IsEnabled = isAllFilled;
        }

        private void Page3_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Registr_Click(object sender, RoutedEventArgs e)
        {
            string email = LoginEnter != null ? LoginEnter.Text : "";
            string password = PasswordEnter != null ? PasswordEnter.Password : "";
            string fio = FIOEnter != null ? FIOEnter.Text : "";
            string phone = PhoneEnter != null ? PhoneEnter.Text : "";
            string age = AgeEnter != null ? AgeEnter.Text : "0";

            if (!int.TryParse(age, out int ageValue))
            {
                MessageBox.Show("Возраст должен быть числом!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Register(email, password, fio, age, phone))
            {
                MessageBox.Show("Пользователь успешно зарегистрирован!", "Успешная регистрация");
                NavigationService.Navigate(new Page1(user));
            }
            else
            {
                MessageBox.Show("Ошибка! Регистрация не удалась, есть ошибки в заполнении полей.", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CheckFields(string email, string pass, string FIO, int age, string phoneNum)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                MessageBox.Show("Почта должна быть указана и содержать '@'!", "Некорректный ввод",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(pass) || pass.Length <= 5)
            {
                MessageBox.Show("Пароль должен содержать больше 5 символов и не быть пустым!", "Некорректный ввод",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(FIO))
            {
                MessageBox.Show("Поле ФИО должно быть заполнено!", "Некорректный ввод",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (age <= 0)
            {
                MessageBox.Show("Возраст должен быть положительным числом!", "Некорректный ввод",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(phoneNum))
            {
                MessageBox.Show("Номер телефона не может быть пустым!", "Некорректный ввод",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string cleanPhone = phoneNum.Trim();
            bool isPhoneValid = (cleanPhone.StartsWith("+") && cleanPhone.Length == 12) ||
                                (!cleanPhone.StartsWith("+") && cleanPhone.Length == 11);

            if (!isPhoneValid)
            {
                MessageBox.Show("Номер телефона должен содержать 11 цифр или 12 с '+' в начале!", "Некорректный ввод",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            user.Email = email;
            user.Password = pass;
            user.Fio = FIO;
            user.Age = age;
            user.PhoneNum = phoneNum;

            return true;
        }

        private void LoginEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoginEnter != null && !string.IsNullOrEmpty(LoginEnter.Text))
            {
                if (LoginEnter.Text.Contains("@"))
                {
                    user.Email = LoginEnter.Text;
                    EmPass = true;
                }
                else
                {
                    MessageBox.Show("Проверьте правильность введенной почты.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                    EmPass = false;
                }
            }
            else if (LoginEnter != null)
            {
                MessageBox.Show("Ошибка! Почта должна быть указана и содержать '@'!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CheckAllFieldsFilled();
            EmPass = false;
        }

        private void PasswordEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            string pasEnt = PasswordEnter != null ? PasswordEnter.Password : "";
            if (!string.IsNullOrEmpty(pasEnt))
            {
                if (pasEnt.Length > 5)
                {
                    user.Password = pasEnt;
                    passwordPass = true;
                }
                else
                {
                    MessageBox.Show("Пароль должен содержать больше 5 символов!");
                    CheckAllFieldsFilled();
                    passwordPass = false;
                }
            }
            else
            {
                MessageBox.Show("Ошибка! пароль не может быть пустым", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            CheckAllFieldsFilled();
            passwordPass = false;
        }

        private void FIOEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FIOEnter != null && !string.IsNullOrEmpty(FIOEnter.Text))
            {
                user.Fio = FIOEnter.Text;
                FioPass = true;
            }
            else
            {
                MessageBox.Show("Ошибка! Поле ФИО должно быть заполнено!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            CheckAllFieldsFilled();
            FioPass = false;
        }

        private void AgeEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AgeEnter != null && !string.IsNullOrEmpty(AgeEnter.Text))
            {
                if (int.TryParse(AgeEnter.Text, out int usAge))
                {
                    if (usAge > 0)
                    {
                        user.Age = usAge;
                        AgePass = true;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка! Возраст не может быть отрицательным!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
                        AgePass = false;
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка! Возраст - это число! Введите в поле числовое значение.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                    AgePass = false;
                }
            }
            else
            {
                MessageBox.Show("Ошибка! Поле не может быть пустым!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            CheckAllFieldsFilled();
            AgePass = false;
        }

        private void PhoneEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PhoneEnter != null && !string.IsNullOrEmpty(PhoneEnter.Text))
            {
                if (PhoneEnter.Text.Contains("+"))
                {
                    if (PhoneEnter.Text.Length == 12)
                    {
                        user.PhoneNum = PhoneEnter.Text;
                        PhonePass = true;
                    }
                    else
                    {
                        MessageBox.Show("Проверьте правильность введенного номера.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                        PhonePass = false;
                    }
                }
                else if (PhoneEnter.Text.Length == 11)
                {
                    user.PhoneNum = PhoneEnter.Text;
                    PhonePass = true;
                }
                else
                {
                    MessageBox.Show("Проверьте правильность введенного номера телефона.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
                    PhonePass = false;
                }
            }
            else
            {
                MessageBox.Show("Ошибка! Номер телефона не может быть пустым!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CheckAllFieldsFilled();
            PhonePass = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2(user));
        }
    }
}