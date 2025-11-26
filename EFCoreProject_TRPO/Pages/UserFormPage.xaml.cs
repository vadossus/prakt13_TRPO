using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;

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
            CurrentUser.CreatedAt = System.DateTime.Now;
            DataContext = this;
            SetupPasswordBinding();
        }

        public UserFormPage(User editUser) : this()
        {
            CurrentUser = editUser;
            isEdit = true;
            DataContext = this;
        }

        private void SetupPasswordBinding()
        {
            txtPassword.PasswordChanged += (s, e) =>
            {
                CurrentUser.Password = txtPassword.Password;
            };

            if (isEdit && !string.IsNullOrEmpty(CurrentUser.Password))
            {
                txtPassword.Password = CurrentUser.Password;
            }
        }

        private bool IsFormValid()
        {
            bool isLoginValid = !System.Windows.Controls.Validation.GetHasError(txtLogin);
            bool isEmailValid = !System.Windows.Controls.Validation.GetHasError(txtEmail);
            bool isPasswordValid = isEdit || !System.Windows.Controls.Validation.GetHasError(txtPassword);
            bool isDateValid = !System.Windows.Controls.Validation.GetHasError(dpCreatedAt);

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

            if (!isEdit)
            {
                var bindingExpression = txtPassword.GetBindingExpression(PasswordBox.TagProperty);
                bindingExpression?.UpdateSource();
            }

            if (!IsFormValid())
            {
                return; 
            }

            try
            {
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