using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Логика взаимодействия для AddPoint.xaml
    /// </summary>
    public partial class AddPoint : Window
    {
        public AddPoint()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbName.Text)||string.IsNullOrWhiteSpace(TbAddress.Text)||string.IsNullOrWhiteSpace(TbPhone.Text))
            {
                MessageBox.Show("Пустые поля!","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            if (!new PhoneAttribute().IsValid(TbPhone.Text))
            {
                MessageBox.Show("Номер телефона, имеет неправильный формат!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DeliveryPoint point = new DeliveryPoint()
            {
                Address = TbAddress.Text,
                Name = TbName.Text,
                Phone = TbPhone.Text
            };
            Helper.context.DeliveryPoints.Add(point);
            Helper.context.SaveChanges();
            Close();
        }
    }
}
