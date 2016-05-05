using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OOP3CarProject
{
    /// <summary>
    /// Interaction logic for SetupWindow.xaml
    /// </summary>
    public partial class SetupWindow : Window
    {
        public CarType TypeOfCar { get; set; }
        public decimal StartingMoney { get; set; }
        public string  CarName { get; set; }

        public SetupWindow()
        {
            InitializeComponent();
            carTypeComboBox.ItemsSource = Enum.GetValues(typeof(CarType));
            carTypeComboBox.SelectedIndex = 0;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.StartingMoney = Decimal.Parse(this.startingMoneyTextBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Starting money must be a decimal value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.TypeOfCar = (CarType)carTypeComboBox.SelectedItem;
            this.CarName = this.carNameTextBox.Text;
            this.DialogResult = true;
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
