using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace EFCoreProject_TRPO.Pages
{
    public partial class MainPage : Page
    {
        public UsersService service { get; set; } = new();
        public User? selectedUser { get; set; } = null;

        public ObservableCollection<Role> Roles { get; set; } = new();


        private int? _selectedRoleId = null;
        public int? SelectedRoleId
        {
            get => _selectedRoleId;
            set
            {
                _selectedRoleId = value;
                UpdateFUsers();
            }
        }

        public ObservableCollection<User> FUsers { get; set; } = new();

        public MainPage()
        {
            InitializeComponent();

            LoadRoles();

            DataContext = this;
            this.Loaded += MainPage_Loaded;
        }

        private void LoadRoles()
        {
            var roleService = new RoleService();
            Roles.Clear();

            Roles.Add(new Role { Id = 0, Title = "Все роли" });

            foreach (var role in roleService.Roles)
            {
                Roles.Add(role);
            }

            SelectedRoleId = 0;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            service.GetAll();
            UpdateFUsers();

            DataContext = null;
            DataContext = this;
        }

        private void UpdateFUsers()
        {
            FUsers.Clear();

            if (SelectedRoleId == null || SelectedRoleId == 0)
            {
                foreach (var user in service.Users)
                {
                    FUsers.Add(user);
                }
            }
            else
            {
                foreach (var user in service.Users.Where(u => u.RoleId == SelectedRoleId))
                {
                    FUsers.Add(user);
                }
            }
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
                UpdateFUsers(); 
            }
        }

        public void ClearFilter(object sender, RoutedEventArgs e)
        {
            SelectedRoleId = 0;
        }

        public void RefreshUsers()
        {
            service.GetAll();
            UpdateFUsers();
        }
    }
}