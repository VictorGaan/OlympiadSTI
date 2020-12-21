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
    /// Логика взаимодействия для ManagerPage.xaml
    /// </summary>
    public partial class ManagerPage : Page
    {
        List<Order> Orders = new List<Order>();
        public ManagerPage()
        {
            InitializeComponent();
            Blockchain.Helper.LoadBlockchain(Helper.Blockchain);
            CmbEmployees.ItemsSource = Helper.context.Employees.Include(x => x.Person).Where(x => x.RoleId == 1).ToList();
            PointsGrid.ItemsSource = Helper.context.DeliveryPoints.ToList();
            ClientsGrid.ItemsSource = Helper.context.Clients.Include(x => x.Person).ToList();
            EmployeesGrid.ItemsSource = Helper.context.Employees.Include(x => x.Person).Include(x=>x.Role).ToList();
            Load();
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
                    Orders = Orders.Where(x =>x.IsActive).ToList();
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
            TabItem ti = MyTabControl.SelectedItem as TabItem;
            if (ti.Header.ToString() == "Заказы")
            {
                Order order = OrdersGrid.SelectedItem as Order;
                Employee employee = CmbEmployees.SelectedItem as Employee;
                if (order == null)
                {
                    return;
                }
                if (order.IsStatusDelivery==null)
                {
                    Delivery delivery = new Delivery()
                    {
                        OrderId = order.Id,
                        EmployeeId = employee.Id,
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
                }
                else
                {
                    MessageBox.Show("Выбранный заказ, нельзя отправить в работу!","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            new AddPoint().ShowDialog();
            PointsGrid.ItemsSource = Helper.context.DeliveryPoints.ToList();
            PointsGrid.Items.Refresh();
        }

        private void BtnAddClient_Click(object sender, RoutedEventArgs e)
        {
            new AddClient().ShowDialog();
            ClientsGrid.ItemsSource = Helper.context.Clients.Include(x => x.Person).ToList();
            ClientsGrid.Items.Refresh();
        }

        private void BtnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            new AddEmployee().ShowDialog();
            EmployeesGrid.ItemsSource = Helper.context.Employees.Include(x => x.Person).Include(x => x.Role).ToList();
            EmployeesGrid.Items.Refresh();
        }
    }
}
