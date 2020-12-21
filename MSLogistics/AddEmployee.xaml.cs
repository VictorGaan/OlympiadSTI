using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Database;

namespace MSLogistics
{
    /// <summary>
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        public AddEmployee()
        {
            InitializeComponent();
            CmbRoles.ItemsSource = Helper.context.Roles.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbLastName.Text)
                || string.IsNullOrWhiteSpace(TbFirstName.Text)
                || string.IsNullOrWhiteSpace(TbLogin.Text)
                || string.IsNullOrWhiteSpace(TbPassword.Text)
                || string.IsNullOrWhiteSpace(TbPhone.Text)
                || string.IsNullOrWhiteSpace(TbEmail.Text))
            {
                MessageBox.Show("Пустые поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!new EmailAddressAttribute().IsValid(TbEmail.Text))
            {
                MessageBox.Show("Почта, имеет неправильный формат!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!new PhoneAttribute().IsValid(TbPhone.Text))
            {
                MessageBox.Show("Номер телефона, имеет неправильный формат!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Person person = new Person()
            {
                Email = TbEmail.Text,
                LastName = TbLastName.Text,
                FirstName = TbFirstName.Text,
                Login = TbLogin.Text,
                Password = TbPassword.Text,
                MiddleName = TbMiddleName.Text,
                Phone = TbPhone.Text,
            };
            Helper.context.People.Add(person);
            Helper.context.SaveChanges();
            Role role = CmbRoles.SelectedItem as Role;
            Employee employee = new Employee()
            {
                PersonId = person.Id,
                RoleId = role.Id,
                OrganizationAccountId=2,
            };
            Helper.context.Employees.Add(employee);
            Helper.context.SaveChanges();
            Close();
        }
    }
}
