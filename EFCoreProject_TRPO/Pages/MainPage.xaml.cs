using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;

namespace EFCoreProject_TRPO.Pages
{
    public partial class MainPage : Page
    {
        public UsersService service { get; set; } = new();
        public User? selectedUser { get; set; } = null;

        public MainPage()
        {
            InitializeComponent();
            DataContext = this;
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            service.GetAll();

            DataContext = null;
            DataContext = this;
        }

        public void go_form(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserFormPage());
        }

        public void Edit(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Выберите элемент из списка!");
                return;
            }
            NavigationService.Navigate(new UserFormPage(selectedUser));
        }

        public void remove(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Выберите запись!");
                return;
            }

            if (MessageBox.Show("Вы действительно хотите удалить запись?", "Удалить?",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                service.Remove(selectedUser);
            }
        }
    }
}