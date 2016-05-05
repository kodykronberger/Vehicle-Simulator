using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP3CarProject
{
    class Game
    {
        public decimal Money { get; set; }
        public Car Car { get; set; }

        public Game(Car car, decimal money)
        {
            this.Money = money;
            this.Car = car;
        }
    }
}
