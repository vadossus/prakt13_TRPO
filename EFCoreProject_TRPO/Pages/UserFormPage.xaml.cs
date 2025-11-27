using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EFCoreProject_TRPO.Pages
{
    public partial class UserFormPage : Page
    {
        private UsersService _service = new();
        public User CurrentUser { get; set; } = new();
        private bool isEdit = false;
        public string PageTitle => isEdit ? "Редактирование пользователя" : "Добавление пользователя";

        public UserFormPage()
        {
            InitializeComponent();
            CurrentUser.CreatedAt = System.DateTime.Now.Date;
            DataContext = this;
        }

        public UserFormPage(User editUser) : this()
        {
            CurrentUser = editUser;
            isEdit = true;
            DataContext = this;

            if (!string.IsNullOrEmpty(CurrentUser.Password))
            {
                txtPassword.Password = CurrentUser.Password;
            }
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CurrentUser.Password = txtPassword.Password;
            ValidatePassword();
        }

        private void ValidatePassword()
        {
            var errors = new List<string>();
            string password = txtPassword.Password;

            if (!isEdit)
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    errors.Add("Пароль обязателен для заполнения");
                }
                else
                {
                    if (password.Length < 8)
                        errors.Add("Пароль должен содержать минимум 8 символов");

                    if (!Regex.IsMatch(password, @"[0-9]"))
                        errors.Add("Пароль должен содержать цифры");

                    if (!Regex.IsMatch(password, @"[a-z]"))
                        errors.Add("Пароль должен содержать буквы в нижнем регистре");

                    if (!Regex.IsMatch(password, @"[A-Z]"))
                        errors.Add("Пароль должен содержать буквы в верхнем регистре");

                    if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
                        errors.Add("Пароль должен содержать специальные символы");
                }
            }

            PasswordErrorItems.ItemsSource = errors;
            PasswordErrorItems.Visibility = errors.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool IsFormValid()
        {
            bool isLoginValid = !System.Windows.Controls.Validation.GetHasError(txtLogin);
            bool isEmailValid = !System.Windows.Controls.Validation.GetHasError(txtEmail);
            bool isDateValid = !System.Windows.Controls.Validation.GetHasError(dpCreatedAt);

            bool isPasswordValid = isEdit || (PasswordErrorItems.Items.Count == 0 && !string.IsNullOrEmpty(txtPassword.Password));

            if (isLoginValid && !_service.IsLoginUnique(CurrentUser.Login, isEdit ? CurrentUser.Id : null))
            {
                MessageBox.Show("Логин уже существует", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (isEmailValid && !_service.IsEmailUnique(CurrentUser.Email, isEdit ? CurrentUser.Id : null))
            {
                MessageBox.Show("Email уже существует", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return isLoginValid && isEmailValid && isPasswordValid && isDateValid;
        }

        private void save(object sender, RoutedEventArgs e)
        {
            txtLogin.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            txtEmail.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            dpCreatedAt.GetBindingExpression(DatePicker.SelectedDateProperty)?.UpdateSource();

            ValidatePassword();

            if (!IsFormValid())
            {
                return;
            }

            try
            {
                if (!isEdit)
                {
                    CurrentUser.Password = txtPassword.Password;
                }
                else if (string.IsNullOrEmpty(CurrentUser.Password) && !string.IsNullOrEmpty(txtPassword.Password))
                {
                    CurrentUser.Password = txtPassword.Password;
                }

                if (isEdit)
                    _service.Update(CurrentUser);
                else
                    _service.Add(CurrentUser);

                NavigationService.GoBack();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}