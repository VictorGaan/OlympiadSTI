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
using System.Windows.Threading;
using Database;
using Microsoft.EntityFrameworkCore;

namespace MSLogistics
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public List<Order> Orders = new List<Order>();
        public int Id { get; set; }
        public ClientPage(int id)
        {
            InitializeComponent();
            Id = id;
            Helper.client = Id;
            Price();
            Load();
        }
        void Price()
        {
            var client = Helper.context.Clients.Include(x => x.Person).FirstOrDefault(x => x.Id == Id);
            TbPrice.Text = $"Клиент: {client.Person.LastName} {client.Person.FirstName} {client.Person.MiddleName}. Кошелёк: {client.Wallet} рублей";
        }
        private void Load()
        {
            try
            {
                Orders.Clear();
                Blockchain.Helper.LoadBlockchain(Helper.Blockchain);
                if (Helper.Blockchain.Chain == null)
                {
                    Helper.Blockchain = new Blockchain.Blockchain(Helper.context.OrganizationAccounts.FirstOrDefault().PublicKey);
                }
                else
                {
                    if (Helper.Blockchain.Chain.Count > 1)
                    {
                        for (int i = 1; i < Helper.Blockchain.Chain.Count; i++)
                        {
                            Orders.Add(Helper.Blockchain.StringToObject(Helper.Blockchain.Chain[i].Data));
                        }
                    }
                    Orders = Orders.OrderBy(x => x.Id).ToList();
                    var cloneOrders = new List<Order>();
                    for (int i = 0; i < Orders.Count; i++)
                    {
                        if (i + 1 >= Orders.Count)
                        {
                            break;
                        }

                        if (Orders[i].Id == Orders[i + 1].Id)
                        {
                            if (Orders[i].IsActive && !Orders[i + 1].IsActive)
                            {
                                cloneOrders.Add(Orders[i]);
                                cloneOrders.Add(Orders[i]);
                            }
                            if (i + 1 >= Orders.Count)
                            {
                                break;
                            }
                            if (Orders[i].IsStatusDelivery == null && Orders[i + 1].IsStatusDelivery == false)
                            {
                                cloneOrders.Add(Orders[i]);
                            }
                            if (i + 1 >= Orders.Count)
                            {
                                break;
                            }
                            if (Orders[i].IsStatusDelivery == false && Orders[i + 1].IsStatusDelivery == true)
                            {
                                cloneOrders.Add(Orders[i]);
                            }
                        }
                    }

                    foreach (var x in Orders.ToArray())
                    {
                        foreach (var y in cloneOrders.ToArray())
                        {
                            if (x.Id == y.Id && x.IsStatusDelivery == y.IsStatusDelivery)
                            {
                                Orders.Remove(x);
                            }
                        }
                    }
                    Orders = Orders.Where(x => x.ClientId == Id && x.IsActive).ToList();
                    OrdersGrid.ItemsSource = Orders;
                    OrdersGrid.Items.Refresh();
                }
            }
            catch (Exception)
            {

                OrdersGrid.ItemsSource = new List<Order>();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            new AddOrderWindow().ShowDialog();
            Load();
            Price();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Order order = (sender as Button).DataContext as Order;
            if (order.IsStatusDelivery != null)
            {
                MessageBox.Show("Отменить можно только, те заказы, которые имеют статус доставки: Не доставлено!", "Информация", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (order.IsStatusDelivery == null)
            {
                for (int i = 1; i < Helper.Blockchain.Chain.Count; i++)
                {
                    if (Helper.Blockchain.Chain[i].Data == Helper.Blockchain.ObjectToString(order))
                    {
                        order.IsActive = false;
                        Helper.Blockchain.AddReturnTransaction(Helper.context.OrganizationAccounts.FirstOrDefault(), Helper.context.Clients.Find(Helper.client), order, order.TypeDelivery.Price);
                        break;
                    }
                }
                Blockchain.Helper.SaveBlockchain(Helper.Blockchain);
                Helper.context.SaveChanges();
                Load();
                Price();
            }

        }
    }
}
