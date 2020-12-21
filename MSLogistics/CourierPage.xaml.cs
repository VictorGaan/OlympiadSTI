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
using Database;
using Microsoft.EntityFrameworkCore;

namespace MSLogistics
{
    /// <summary>
    /// Логика взаимодействия для CourierPage.xaml
    /// </summary>
    public partial class CourierPage : Page
    {
        List<Order> Orders = new List<Order>();
        List<Order> Delivery = new List<Order>();
        int Id { get; set; }
        public CourierPage(int id)
        {
            InitializeComponent();
            Id = id;
            Helper.employee = Id;
            Blockchain.Helper.LoadBlockchain(Helper.Blockchain);
            Load();
            LoadDelivery();
        }

        private void LoadDelivery()
        {
            try
            {
                Delivery.Clear();
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
                            Delivery.Add(Helper.Blockchain.StringToObject(Helper.Blockchain.Chain[i].Data));
                        }
                    }
                    Delivery = Delivery.OrderBy(x => x.Id).ToList();
                    var cloneOrders = new List<Order>();
                    for (int i = 0; i < Delivery.Count; i++)
                    {
                        if (i + 1 >= Delivery.Count)
                        {
                            break;
                        }

                        if (Delivery[i].Id == Delivery[i + 1].Id)
                        {
                            if (Delivery[i].IsStatusDelivery == null && Delivery[i + 1].IsStatusDelivery == false)
                            {
                                cloneOrders.Add(Delivery[i]);
                            }
                            if (i + 1 >= Delivery.Count)
                            {
                                break;
                            }
                            if (Delivery[i].IsStatusDelivery == false && Delivery[i + 1].IsStatusDelivery == true)
                            {
                                cloneOrders.Add(Delivery[i]);
                            }
                        }
                    }
                    foreach (var x in Delivery.ToArray())
                    {
                        foreach (var y in cloneOrders.ToArray())
                        {
                            if (x.Id == y.Id && x.IsStatusDelivery == y.IsStatusDelivery)
                            {
                                Delivery.Remove(x);
                            }
                        }
                    }
                    Delivery = Delivery.Where(x => x.IsStatusDelivery == false && x.IsActive).ToList();
                    DeliveryGrid.ItemsSource = Delivery;
                    DeliveryGrid.Items.Refresh();
                }
            }
            catch (Exception)
            {

                DeliveryGrid.ItemsSource = new List<Delivery>();
            }
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
                    Orders = Orders.Where(x => x.IsActive).ToList();
                    OrdersGrid.ItemsSource = Orders;
                    OrdersGrid.Items.Refresh();
                }
            }
            catch (Exception)
            {

                OrdersGrid.ItemsSource = new List<Order>();
            }
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            Order order = OrdersGrid.SelectedItem as Order;
            if (order == null)
            {
                MessageBox.Show("Выберите заказ из таблицы заказов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (order.IsStatusDelivery == true || order.IsStatusDelivery == false)
            {
                MessageBox.Show("Данный заказ нельзя выбрать, для курьера!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Delivery delivery = new Delivery()
            {
                OrderId = order.Id,
                EmployeeId = Id,
            };
            Helper.context.Deliveries.Add(delivery);
            Helper.context.SaveChanges();
            for (int i = 1; i < Helper.Blockchain.Chain.Count; i++)
            {
                if (Helper.Blockchain.Chain[i].Data == Helper.Blockchain.ObjectToString(order))
                {
                    order.IsStatusDelivery = false;
                    var transaction = Helper.Blockchain.Chain[i].Transactions.FirstOrDefault();
                    var client = Helper.context.Clients.SingleOrDefault(x => x.PublicKey == transaction.Sender);
                    Helper.Blockchain.AddTransaction(client, Helper.context.OrganizationAccounts.FirstOrDefault(), order, order.TypeDelivery.Price);
                    break;
                }
            }
            Blockchain.Helper.SaveBlockchain(Helper.Blockchain);
            Load();
            LoadDelivery();
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            TabItem ti = MyTabControl.SelectedItem as TabItem;
            string message = "";
            if (ti.Header.ToString() == "Заказы")
            {
                Order order = OrdersGrid.SelectedItem as Order;
                if (order != null)
                {
                    DeliveryPoint from = Helper.context.DeliveryPoints.Find(order.From);
                    DeliveryPoint to = Helper.context.DeliveryPoints.Find(order.To);
                    Client client = Helper.context.Clients.Include(x => x.Person).FirstOrDefault(x => x.Id == order.ClientId);
                    message += $"Клиент: {client.Person.LastName} {client.Person.FirstName} {client.Person.MiddleName}\n" +
                               $"Откуда: {from.Name} {from.Address} {from.Phone}\n" +
                               $"Куда: {to.Name} {to.Address} {to.Phone}\n" +
                               $"Тип доставки: {order.TypeDelivery.Name}\n" +
                               $"Цена доставки(оплачена): {order.TypeDelivery.Price} рублей\n" +
                               $"Описание: {order.Description}";
                    MessageBox.Show(message, "Дополнительная информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            TabItem ti = MyTabControl.SelectedItem as TabItem;
            if (ti.Header.ToString() == "Доставки")
            {
                Order order = DeliveryGrid.SelectedItem as Order;
                if (order == null)
                {
                    return;
                }
                for (int i = 1; i < Helper.Blockchain.Chain.Count; i++)
                {
                    if (Helper.Blockchain.Chain[i].Data == Helper.Blockchain.ObjectToString(order))
                    {
                        order.IsStatusDelivery = true;
                        var transaction = Helper.Blockchain.Chain[i].Transactions.FirstOrDefault();
                        var client = Helper.context.Clients.SingleOrDefault(x => x.PublicKey == transaction.Sender);
                        Helper.Blockchain.AddTransaction(client, Helper.context.OrganizationAccounts.FirstOrDefault(), order, order.TypeDelivery.Price);
                        break;
                    }
                }
                Blockchain.Helper.SaveBlockchain(Helper.Blockchain);
                Load();
                LoadDelivery();
            }
        }
    }
}
