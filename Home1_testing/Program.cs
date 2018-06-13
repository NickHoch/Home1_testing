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
    class Robot : Unit
    {
        public static double Coefficient { get; set; } = 3;
        public override string ToString()
        {
            return $"Robot {Name}";
        }
    }
    class Colonist : Unit
    {
        public bool Gender { get; set; }
        public int Age { get; set; }
        public override string ToString()
        {
            var genderString = Gender ? "male" : "female";
            return $"Miner {Name} {Age} {genderString}";
        }
    }
    class Miner : Colonist
    {
        public static double Coefficient { get; set; } = 1;
    }
    class Builder : Colonist
    {
        public static double Coefficient { get; set; } = 0.8;
    }
    class Cook : Colonist
    {
        public static double Coefficient { get; set; } = 0.5;
    }

    interface IExpandeable
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

    public abstract class Colony : IExpandeable, IReduceable
    {
        public ICollection<Unit> units = new List<Unit>();
        public Colony()
        {
            units = UnitFactory.GetUnits(10);
        }
        public int LocateSetters(ICollection<Unit> unitsToAdd)
        {
            units.ToList().AddRange(unitsToAdd);
            return units.Count();
        }
        public int RemoteSetters(int count)
        {
            for (int i = 0; i < count; i++)
            {
                units.ToList().RemoveAt(Program.rand.Next(0, units.Count()));
            }
            return units.Count();
        }
        public int RemoteSetters(int count, Type type)
        {
            for (int i = 0; i < units.Count() && count > 0; i++)
            {
                if (units.ToList()[i].GetType() == type)
                {
                    units.Remove(units.ToList()[i]);
                    count--;
                }
            }
            return units.Count();
        }
    }

    public class MarsColony : Colony, IProductionable
    {
        public double CalculateProd(ICollection<Unit> units, int period)
        {
            double quantityProd = 0d;
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
                unitsTest.Add(ColonistsFactory.GetColonist());
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
        public static ICollection<Unit> GetUnits(int quantityUnits)
        {
            ICollection<Unit> units = new List<Unit>();
            for (int i = 0; i < quantityUnits; i++)
            {
                UnitType type = (UnitType)Program.rand.Next(0, 4);
                switch (type)
                {
                    case UnitType.Robot:
                        units.Add(GetUnit(UnitType.Robot));
                        break;
                    case UnitType.Miner:
                        units.Add(GetUnit(UnitType.Miner));
                        break;
                    case UnitType.Builder:
                        units.Add(GetUnit(UnitType.Builder));
                        break;
                    case UnitType.Cook:
                        units.Add(GetUnit(UnitType.Cook));
                        break;
                    default:
                        break;
                }
            }
            return units;
        }
    }
    class ColonistsFactory
    {
        //public static ICollection<Unit> GetColonists(int quantityColonists)
        //{
        //    ICollection<Unit> colonists = new List<Unit>();
        //    for (int i = 0; i < quantityColonists; i++)
        //    {
        //        UnitType type = (UnitType)Program.rand.Next(1, 4);
        //        switch (type)
        //        {
        //            case UnitType.Miner:
        //                colonists.Add(UnitFactory.GetUnit(UnitType.Miner));
        //                break;
        //            case UnitType.Builder:
        //                colonists.Add(UnitFactory.GetUnit(UnitType.Builder));
        //                break;
        //            case UnitType.Cook:
        //                colonists.Add(UnitFactory.GetUnit(UnitType.Cook));
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    return colonists;
        //}
        public static Unit GetColonist()
        {
            UnitType type = (UnitType)Program.rand.Next(1, 4);
            switch (type)
            {
                case UnitType.Miner:
                    return UnitFactory.GetUnit(UnitType.Miner);
                case UnitType.Builder:
                    return UnitFactory.GetUnit(UnitType.Builder);
                case UnitType.Cook:
                    return UnitFactory.GetUnit(UnitType.Cook);
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
            foreach(var item in UnitFactory.GetUnits(10))
            {
                Console.WriteLine(item);
            }
        }
    }
}