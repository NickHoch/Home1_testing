using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home1_testing
{
    enum UnitType
    {
        Robot,
        Miner,
        Builder,
        Cook
    }
    public class Unit
    {
        public string Name { get; set; }
    }
    public class Robot : Unit
    {
        public static double Coefficient { get; set; } = 3;
        public override string ToString()
        {
            return $"Robot {Name}";
        }
    }
    public class Colonist : Unit
    {
        public bool Gender { get; set; }
        public int Age { get; set; }
        public override string ToString()
        {
            var genderString = Gender ? "male" : "female";
            return $"{Name} {Age} {genderString}";
        }
    }
    public class Miner : Colonist
    {
        public static double Coefficient { get; set; } = 1;
        public override string ToString()
        {
            return "Miner " + base.ToString();
        }
    }
    public class Builder : Colonist
    {
        public static double Coefficient { get; set; } = 0.8;
        public override string ToString()
        {
            return "Builder " + base.ToString();
        }
    }
    public class Cook : Colonist
    {
        public static double Coefficient { get; set; } = 0.5;
        public override string ToString()
        {
            return "Cook " + base.ToString();
        }
    }

    interface IExpandable
    {
        int LocateSetters(ICollection<Unit> units);
    }
    interface IReduceable
    {
        int RemoteSetters(int count);
        int RemoteSetters(int count, Type type);
    }
    interface IProductionable
    {
        double CalculateProd(ICollection<Unit> units, int period);
    }

    public abstract class Colony : IExpandable, IReduceable
    {
        private const int quantityUnits = 10;
        public ICollection<Unit> units = new List<Unit>();
        private UnitType[] typeArr = new UnitType[quantityUnits]
        {
            UnitType.Builder,
            UnitType.Miner,
            UnitType.Cook,
            UnitType.Miner,
            UnitType.Robot,
            UnitType.Cook,
            UnitType.Builder,
            UnitType.Robot,
            UnitType.Miner,
            UnitType.Cook
        };
        public Colony()
        {
            for (int i = 0; i < quantityUnits; i++)
            {                
                //UnitType type = (UnitType)Program.rand.Next(0, 4);
                switch (typeArr[i])
                {
                    case UnitType.Robot:
                        units.Add(UnitFactory.GetUnit(UnitType.Robot));
                        break;
                    case UnitType.Miner:
                        units.Add(UnitFactory.GetUnit(UnitType.Miner));
                        break;
                    case UnitType.Builder:
                        units.Add(UnitFactory.GetUnit(UnitType.Builder));
                        break;
                    case UnitType.Cook:
                        units.Add(UnitFactory.GetUnit(UnitType.Cook));
                        break;
                    default:
                        break;
                }
            }
        }
        public int LocateSetters(ICollection<Unit> unitsToAdd)
        {
            (units as List<Unit>).AddRange(unitsToAdd);
            return units.Count();
        }
        public int RemoteSetters(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if(units.Count() < 1)
                {
                    return -1;
                }
                int index = Program.rand.Next(0, units.Count());
                (units as List<Unit>).RemoveAt(index);
            }
            return units.Count();
        }
        public int RemoteSetters(int count, Type type)
        {
            for (int i = 0; i < units.Count() && count > 0; i++)
            {
                if ((units as List<Unit>)[i].GetType() == type)
                {
                    units.Remove((units as List<Unit>)[i]);
                    count--;
                }
            }
            if(count > 0)
            {
                return -1;
            }
            return units.Count();
        }
    }

    public class MarsColony : Colony, IProductionable
    {
        public double CalculateProd(ICollection<Unit> units, int period)
        {
            double quantityProd = 0d;
            if(units == null)
            {
                return -1;
            }
            foreach (var item in units)
            {
                if (item is Robot)
                {
                    quantityProd += period * Robot.Coefficient;
                }
                else if (item is Miner)
                {
                    quantityProd += period * Miner.Coefficient;
                }
                else if (item is Builder)
                {
                    quantityProd += period * Builder.Coefficient;
                }
                else if (item is Cook)
                {
                    quantityProd += period * Cook.Coefficient;
                }
            }
            return quantityProd;
        }
        public ICollection<Unit> GetColonySetters()
        {
            return units;
        }
        public int CalculatorPopulations(ICollection<Unit> units, int period)
        {
            ICollection<Unit> unitsTest = units;
            for (int i = 0; i < period; i++)
            {
                int men = unitsTest.OfType<Colonist>().Count(x => x.Gender == true);
                int women = unitsTest.OfType<Colonist>().Count(x => x.Gender == false);
                int quantityBabes = 0;
                if (men == 0 || women == 0)
                {
                    return unitsTest.Count();
                }
                if (men >= women)
                {
                    quantityBabes = women;
                }
                else if (men < women)
                {
                    quantityBabes = men;
                }
                UnitType type = (UnitType)Program.rand.Next(1, 4);
                unitsTest.Add(UnitFactory.GetUnit(type));
            }
            return unitsTest.Count();
        }
    }

    class Generator
    {
        public static bool RandGender()
        {
            return Convert.ToBoolean(Program.rand.Next(0, 2));
        }
        public static int RandAge()
        {
            return Program.rand.Next(18, 30);
        }
        public static string RandName()
        {
            List<String> names = new List<string>
            {
                "Sasha", "Ulya", "Vika", "Nick", "Dima", "Roma", "Lina", "Ostap", "Taras", "Anya"
            };
            return names[Program.rand.Next(0, 10)];
        }
    }

    class UnitFactory
    {
        public static Unit GetUnit(UnitType type)
        {
            switch (type)
            {
                case UnitType.Robot:
                    return new Robot
                    {
                        Name = Generator.RandName()
                    };
                case UnitType.Miner:
                    return new Miner
                    {
                        Gender = Generator.RandGender(),
                        Age = Generator.RandAge(),
                        Name = Generator.RandName()
                    };
                case UnitType.Builder:
                    return new Builder
                    {
                        Gender = Generator.RandGender(),
                        Age = Generator.RandAge(),
                        Name = Generator.RandName()
                    };
                case UnitType.Cook:
                    return new Cook
                    {
                        Gender = Generator.RandGender(),
                        Age = Generator.RandAge(),
                        Name = Generator.RandName()
                    };
                default:
                    return null;
            }
        }
    }
    class Program
    {
        public static Random rand = new Random();
        static void Main(string[] args)
        {
            //ICollection<Unit> units = new List<Unit>();
            //for (int i = 0; i < 10; i++)
            //{
            //    UnitType type = (UnitType)rand.Next(0, 4);
            //    switch (type)
            //    {
            //        case UnitType.Robot:
            //            units.Add(UnitFactory.GetUnit(UnitType.Robot));
            //            break;
            //        case UnitType.Miner:
            //            units.Add(UnitFactory.GetUnit(UnitType.Miner));
            //            break;
            //        case UnitType.Builder:
            //            units.Add(UnitFactory.GetUnit(UnitType.Builder));
            //            break;
            //        case UnitType.Cook:
            //            units.Add(UnitFactory.GetUnit(UnitType.Cook));
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //foreach (var item in units)
            //{
            //    Console.WriteLine(item);
            //}
        }
    }
}