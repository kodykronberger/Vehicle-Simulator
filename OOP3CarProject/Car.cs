using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OOP3CarProject
{
    public class Car
    {
        private Timer fuelTimer = new Timer(100);
        public Action FuelHasChanged;
        public Action CarHasStarted;
        public Action CarConditionChanged;

        public string Name { get; set; }
        public CarType CarType { get; set; }
        public int MAXFUEL { get; set; }
        public int Fuel { get; set; }
        public bool IsStarted { get; set; }
        public CarCondition Condition { get; set; }

        public Car(string name, CarType carType, int maxFuel, CarCondition condition)
        {
            this.Name = name;
            this.CarType = carType;
            this.Fuel = maxFuel;
            this.MAXFUEL = maxFuel;
            this.IsStarted = false;
            this.fuelTimer = new Timer(2000);
            this.fuelTimer.Elapsed += this.DepleteFuel;
            this.Condition = condition;
        }

        public void ToggleCarOnOff()
        {
            this.IsStarted = this.IsStarted ? false : true;
            if (this.IsStarted)
            {
                this.fuelTimer.Start();
            }
            else
            {
                this.fuelTimer.Stop();
            }
            if (this.CarHasStarted != null)
            {
                this.CarHasStarted();
            }
        }

        public void RefuelCar()
        {
            this.Fuel = this.MAXFUEL;
            if (this.FuelHasChanged != null)
            {
                this.FuelHasChanged();
            }
        }

        public void RepairCar()
        {
            this.Condition.Repair(this);
            if (this.CarConditionChanged != null)
            {
                this.CarConditionChanged();
            }
        }

        public void DamageCar()
        {
            this.Condition.Damage(this);
            if (this.CarConditionChanged != null)
            {
                this.CarConditionChanged();
            }
        }

        public int DriveCar()
        {
            int miles = new Random(DateTime.Now.Second).Next(20, 101);
            if (miles * 2 > this.Fuel)
            {
                int distance = this.Fuel;
                this.Fuel = 0;
                return distance;
            }
            else
            {
                this.Fuel -= miles * 2;
                return miles;
            }
        }

        public bool DamageRandomly()
        {
            if (new Random(DateTime.Now.Second).Next(1,6) == 5)
            {
                this.DamageCar();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DepleteFuel(object obj, ElapsedEventArgs e)
        {
            if (this.Fuel <= 0)
            {
                this.ToggleCarOnOff();
            }
            else
            {
                this.Fuel -= 1;
            }

            if (this.FuelHasChanged != null)
            {
                this.FuelHasChanged();
            }
        }

        public override string ToString()
        {
            return Name.ToString() + " | " + CarType.ToString();
        }
    }

    public static class CarFactory
    {
        public static Car CreateCar(CarType typeOfCar, string name)
        {
            int maxFuel = typeOfCar == CarType.SportsCar ? 1000
                : typeOfCar == CarType.Coupe ? 300
                : typeOfCar == CarType.Junker ? 5
                : typeOfCar == CarType.MuscleCar ? 100
                : typeOfCar == CarType.Sedan ? 350 : 0;

            CarCondition condition = null;

            switch(typeOfCar)
            {
                case CarType.Coupe:
                    condition = new GoodCondition();
                    break;
                case CarType.Junker:
                    condition = new BadCondition();
                    break;
                case CarType.MuscleCar:
                    condition = new NewCondition();
                    break;
                case CarType.Sedan:
                    condition = new GoodCondition();
                    break;
                case CarType.SportsCar:
                    condition = new NewCondition();
                    break;
                default:
                    condition = new NewCondition();
                    break;
            }

            return new Car(name, typeOfCar, maxFuel, condition);
        }
    }
}
