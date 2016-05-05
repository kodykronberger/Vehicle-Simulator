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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOP3CarProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;

        public MainWindow()
        {
            InitializeComponent();
            if(this.SetupGame())
            {
                this.SetupObservers();
                this.CreateCarImage();
                this.RefreshUI();
                this.HandleCarConditionChanged();
            }
        }

        private void CreateCarImage()
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            switch (game.Car.CarType.ToString())
            {
                case "SportsCar":
                    logo.UriSource = new Uri(@"Images\porsche-cayman-photo-640546-s-original.jpg", UriKind.Relative);
                    break;
                case "MuscleCar":
                    logo.UriSource = new Uri(@"Images\638103-black-muscle-car.jpg", UriKind.Relative);
                    break;
                case "Coupe":
                    logo.UriSource = new Uri(@"Images\2014-hyundai-elantra-coupe-2-door-angular-front-exterior-view_100465906_m.jpg", UriKind.Relative);
                    break;
                case "Sedan":
                    logo.UriSource = new Uri(@"Images\honda-accord-photo-640604-s-original.jpg", UriKind.Relative);
                    break;
                case "Junker":
                    logo.UriSource = new Uri(@"Images\junkcar.jpg", UriKind.Relative);
                    break;
            }
            logo.EndInit();
            this.carImage.Source = logo;
        }

        private bool SetupGame()
        {
            // Calls a setup window, to initialize the game and allow user input for game variables.
            SetupWindow setupWindow = new SetupWindow();
            bool? result = setupWindow.ShowDialog();
            if (result == true)
            {
                Car newCar = CarFactory.CreateCar(setupWindow.TypeOfCar, setupWindow.CarName);
                this.game = new Game(newCar, setupWindow.StartingMoney);
                return true;
            }
            else
            {
                this.Close();
                return false;
            }
        }

        private void SetupObservers()
        {
            this.game.Car.FuelHasChanged = this.RefreshUI;
            this.game.Car.CarHasStarted = this.HandleCarStarting;
            this.game.Car.CarConditionChanged = this.HandleCarConditionChanged;
        }

        private void HandleCarConditionChanged()
        {
            Dispatcher.Invoke(new Action(() => {
                if (this.game.Car.Condition.GetType() == typeof(NewCondition))
                {
                    this.carConditionLabel.Foreground = Brushes.Green;
                }
                else if (this.game.Car.Condition.GetType() == typeof(GoodCondition))
                {
                    this.carConditionLabel.Foreground = Brushes.DarkGreen;
                }
                else if (this.game.Car.Condition.GetType() == typeof(WellCondition))
                {
                    this.carConditionLabel.Foreground = Brushes.LightBlue;
                }
                else if (this.game.Car.Condition.GetType() == typeof(BadCondition))
                {
                    this.carConditionLabel.Foreground = Brushes.Red;
                }
                else if (this.game.Car.Condition.GetType() == typeof(BrokenCondition))
                {
                    this.carConditionLabel.Foreground = Brushes.DarkGray;
                }
                this.carConditionLabel.Content = this.game.Car.Condition.ToString();
            }));
        }

        private void HandleCarStarting()
        {
            Dispatcher.Invoke(new Action(() => {
                this.startCarButton.Content = this.game.Car.IsStarted ? "Stop Car" : "Start Car";
                this.carIndicatorRect.Fill = this.game.Car.IsStarted ? Brushes.Green : Brushes.Red;
                this.driveCarButton.IsEnabled = this.game.Car.Condition.GetType() != typeof(BrokenCondition) ? this.game.Car.IsStarted : false;
                this.repairCarButton.IsEnabled = !this.game.Car.IsStarted;
                this.fuelButton.IsEnabled = !this.game.Car.IsStarted;
            }));
        }

        private void RefreshUI()
        {
            Dispatcher.Invoke(new Action(() => {
                this.moneyLabel.Content = "$" + game.Money.ToString();
                this.fuelLabel.Content = game.Car.Fuel.ToString();
                this.carNameLabel.Content = game.Car.Name.ToString();
            }));
        }

        private void startCar_Click(object sender, RoutedEventArgs e)
        {
            if (this.game.Car.Condition.GetType() == typeof(BrokenCondition))
            {
                MessageBox.Show("Car is broken and cannot be started.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(this.game.Car.Fuel == 0)
            {
                MessageBox.Show("Car is out of fuel and cannot be started.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.game.Car.ToggleCarOnOff();
            }
        }

        private void driveCarButton_Click(object sender, RoutedEventArgs e)
        {
            int miles = this.game.Car.DriveCar();
            MessageBox.Show("Drove " + miles + " miles.\n\nConsumed " + miles * 2 + "units of fuel.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            if (this.game.Car.DamageRandomly())
            {
                MessageBox.Show("Car was damaged while travelling.\n\nRepair car early to avoid big costs.", "Car was damaged", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void fuelButton_Click(object sender, RoutedEventArgs e)
        {
            decimal costOfFuel = (this.game.Car.MAXFUEL - this.game.Car.Fuel) * 0.35m;

            if (costOfFuel > this.game.Money)
            {
                MessageBox.Show("Not enough money.\n\nCost to refill " + (this.game.Car.MAXFUEL - this.game.Car.Fuel) + " units of fuel: $" + costOfFuel + "\nYour money: $" + this.game.Money, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to fuel up?\n\nCost to fill " + (this.game.Car.MAXFUEL - this.game.Car.Fuel) + " units of fuel: $" + costOfFuel, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    this.game.Car.RefuelCar();
                    this.game.Money -= costOfFuel;
                    this.RefreshUI();
                    MessageBox.Show("Car refueled!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            
        }

        private void repairCarButton_Click(object sender, RoutedEventArgs e)
        {
            decimal costOfRepair = (this.game.Car.Condition.GetType() == typeof(NewCondition)) ? 0.00m
                : (this.game.Car.Condition.GetType() == typeof(GoodCondition)) ? 10.00m
                : (this.game.Car.Condition.GetType() == typeof(WellCondition)) ? 18.00m
                : (this.game.Car.Condition.GetType() == typeof(BadCondition)) ? 30.00m
                : (this.game.Car.Condition.GetType() == typeof(BrokenCondition)) ? 200.00m : 1000.00m;


            if (costOfRepair > this.game.Money)
            {
                MessageBox.Show("Not enough money.\n\nCost to repair car: $" + costOfRepair + "\nYour money: $" + this.game.Money, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to repair your car?\n\nCost to repair car: " + costOfRepair, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    this.game.Car.RepairCar();
                    this.game.Money -= costOfRepair;
                    this.RefreshUI();
                    MessageBox.Show("Car repaired!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
