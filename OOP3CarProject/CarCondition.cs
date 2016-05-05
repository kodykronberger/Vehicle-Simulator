using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP3CarProject
{
    public abstract class CarCondition
    {
        public abstract void Damage(Car context);
        public void Repair(Car car)
        {
            car.Condition = new NewCondition();
        }
    }

    public class NewCondition : CarCondition
    {
        public override void Damage(Car car)
        {
            car.Condition = new GoodCondition();
        }
        public override string ToString()
        {
            return "Factory New";
        }
    }

    public class GoodCondition : CarCondition
    {
        public override void Damage(Car car)
        {
            car.Condition = new WellCondition();
        }
        public override string ToString()
        {
            return "Great Condition";
        }
    }

    public class WellCondition : CarCondition
    {
        public override void Damage(Car car)
        {
            car.Condition = new BadCondition();
        }
        public override string ToString()
        {
            return "Well Condition";
        }
    }

    public class BadCondition : CarCondition
    {
        public override void Damage(Car car)
        {
            car.Condition = new BrokenCondition();
        }
        public override string ToString()
        {
            return "Bad Condition";
        }
    }

    public class BrokenCondition : CarCondition
    {
        public override void Damage(Car car)
        {
            car.Condition = new BrokenCondition();
        }
        public override string ToString()
        {
            return "Broken.";
        }
    }
}
