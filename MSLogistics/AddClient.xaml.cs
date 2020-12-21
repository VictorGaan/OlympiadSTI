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
using System.Windows.Shapes;
using Database;

namespace MSLogistics
{
    /// <summary>
    /// Логика взаимодействия для AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        public AddClient()
        {
            InitializeComponent();
            CmbPoints.ItemsSource = Helper.context.DeliveryPoints.ToList();
        }
        int i = 0;
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
            DeliveryPoint point = CmbPoints.SelectedItem as DeliveryPoint;
           
            if (i==0)
            {
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
                Chilkat.Rsa rsa = new Chilkat.Rsa();
                rsa.GenerateKey(1024);
                string publicKey = rsa.ExportPublicKey();
                string privateKey = rsa.ExportPrivateKey();
                Client client = new Client()
                {
                    PersonId = person.Id,
                    Wallet = Convert.ToDecimal(TbWallet.Text),
                    PublicKey = publicKey,
                    PrivateKey = privateKey,
                };
                Helper.context.Clients.Add(client);
                Helper.context.SaveChanges();
                ClientPoint clientPoint = new ClientPoint()
                {
                    ClientId = client.Id,
                    DeliveryPointId = point.Id,
                };
                Helper.context.ClientPoints.Add(clientPoint);
                Helper.context.SaveChanges();
                var message = MessageBox.Show("Добавить еще одну точку забора, данному клиенту?","Точка забора",MessageBoxButton.YesNo,MessageBoxImage.Question);
                if (message==MessageBoxResult.Yes)
                {
                    i++;
                }
                else
                {
                    i = 0;
                    Close();
                }
            }
            else if (i>0)
            {
                var lastClient = Helper.context.Clients.OrderByDescending(x => x.Id).FirstOrDefault();
                ClientPoint clientPoint = new ClientPoint()
                {
                    ClientId = lastClient.Id,
                    DeliveryPointId = point.Id,
                };
                Helper.context.ClientPoints.Add(clientPoint);
                Helper.context.SaveChanges();
                var message = MessageBox.Show("Добавить еще одну точку забора, данному клиенту?", "Точка забора", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (message == MessageBoxResult.Yes)
                {
                    i++;
                }
                else
                {
                    i = 0;
                    Close();
                }
            }
        }
    }
}
