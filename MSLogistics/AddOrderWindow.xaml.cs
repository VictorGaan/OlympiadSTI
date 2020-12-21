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
using Newtonsoft.Json;

namespace MSLogistics
{
    /// <summary>
    /// Логика взаимодействия для AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        public static Blockchain.P2PServer Server = null;
        public static Blockchain.P2PClient Client = new Blockchain.P2PClient();
        public AddOrderWindow()
        {
            InitializeComponent();
            Server = new Blockchain.P2PServer();
            Server.Start();
            Load();
        }

        private void Load()
        {
            CmbFrom.ItemsSource = Helper.context.ClientPoints.Where(x => x.ClientId == Helper.client).Select(x => x.DeliveryPoint).ToList();
            CmbTo.ItemsSource = Helper.context.DeliveryPoints.ToList();
            CmbTypes.ItemsSource = Helper.context.TypeDeliveries.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //Client.Connect($"ws://127.0.0.1:8080/Blockchain");
            DeliveryPoint from = CmbFrom.SelectedItem as DeliveryPoint;
            DeliveryPoint to = CmbTo.SelectedItem as DeliveryPoint;
            TypeDelivery type = CmbTypes.SelectedItem as TypeDelivery;
            if (from==to)
            {
                MessageBox.Show("Место отправки и место назначение не могут совпадать!","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            int id = Settings.Default.Id;
            id++;
            Settings.Default.Id = id;
            Settings.Default.Save();
            Order order = new Order()
            {
                Id = id,
                Date = DateTime.Now,
                Description = TbDescription.Text,
                ClientId = Helper.client,
                IsStatusDelivery = null,
                IsStatusPaid = true,
                TypeDeliveryId = type.Id,
                From = from.Id,
                To = to.Id,
                TypeDelivery = type,
                FromDeliveryPoint = from,
                ToDeliveryPoint = to,
                IsActive = true,
            };
            Helper.Blockchain.AddTransaction(Helper.context.Clients.Find(Helper.client), Helper.context.OrganizationAccounts.FirstOrDefault(), order, order.TypeDelivery.Price);
            Blockchain.Helper.SaveBlockchain(Helper.Blockchain);
            Helper.context.SaveChanges();
            //Client.Broadcast(JsonConvert.SerializeObject(Helper.Blockchain));
            //Client.Close();
            Server.Close();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Server.Close();
            Client.Close();
        }
    }
}
