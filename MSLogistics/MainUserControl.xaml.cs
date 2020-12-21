using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSLogistics
{
    /// <summary>
    /// Логика взаимодействия для MainUserControl.xaml
    /// </summary>
    public partial class MainUserControl : UserControl
    {
        public MainUserControl()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var client = Helper.context.Clients.Include(x => x.Person)
                .Where(x => x.Person.Login == TbLogin.Text && x.Person.Password == PbPassword.Password)
                .FirstOrDefault();
            var employee = Helper.context.Employees.Include(x => x.Person)
                .Where(x => x.Person.Login == TbLogin.Text && x.Person.Password == PbPassword.Password)
                .FirstOrDefault();
            if (client==null&&employee==null)
            {
                MessageBox.Show("Данного пользователя нет в системе!","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            if (client!=null)
            {
                Settings.Default.ClientId = client.Id;
                Settings.Default.Save();
                Helper.MainFrame.Navigate(new ClientPage(client.Id));
            }
            if (employee!=null)
            {
                Settings.Default.EmployeeId = employee.Id;
                Settings.Default.Save();
                switch (employee.RoleId)
                {
                    case 1:
                        Helper.MainFrame.Navigate(new CourierPage(employee.Id));
                        break;
                    case 2:
                        Helper.MainFrame.Navigate(new ManagerPage());
                        break;
                    case 3:
                        goto case 2;
                }
            }
        }
    }
}
