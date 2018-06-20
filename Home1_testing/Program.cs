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

        public Colonist() { }
        public Colonist(string name, bool gender, int age)
        {
            Name = name;
            Gender = gender;
            Age = age;
        }

        public override string ToString()
        {
            var genderString = Gender ? "male" : "female";
            return $"{Name} {Age} {genderString}";
        }
    }
    public class Miner : Colonist
    {
        public Miner() { }
        public Miner(string name, bool gender, int age)
        {
            Name = name;
            Gender = gender;
            Age = age;
        }
        public static double Coefficient { get; set; } = 1;
        public override string ToString()
        {
            return "Miner " + base.ToString();
        }
    }
    public class Builder : Colonist
    {
        public Builder() { }
        public Builder(string name, bool gender, int age)
        {
            Name = name;
            Gender = gender;
            Age = age;
        }
        public static double Coefficient { get; set; } = 0.8;
        public override string ToString()
        {
            return "Builder " + base.ToString();
        }
    }
    public class Cook : Colonist
    {
        public Cook() { }
        public Cook(string name, bool gender, int age)
        {
            Name = name;
            Gender = gender;
            Age = age;
        }
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
        public Colony()
        {
            for (int i = 0; i < quantityUnits; i++)
            {
                UnitType type = (UnitType)Program.rand.Next(0, 4);
                switch (type)
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
            if(unitsToAdd != null)
            {
                (units as List<Unit>).AddRange(unitsToAdd);
            }           
            return units.Count();
        }
        public int RemoteSetters(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (units.Count() < 1)
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
            if (count > 0)
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
            if (units == null)
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
        public int CalculatorPopulations(ICollection<Unit> units, int period, List<Config> configs)
        {
            ICollection<Unit> unitsTest = units;
            RobotCreator robotCreator = new RobotCreator();
            ColonistCreator colonistCreator = new ColonistCreator();

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
                for (int j = 0; j < quantityBabes; j++)
                {
                    if (configs[i] is ConfigRobot)
                    {
                        (unitsTest as List<Unit>).AddRange(robotCreator.FactoryMethod(configs[i]));
                    }
                    else if (configs[i] is ConfigColonist)
                    {
                        (unitsTest as List<Unit>).AddRange(colonistCreator.FactoryMethod(configs[i]));
                    }
                }
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
        public static string RandMaleName()
        {
            List<String> names = new List<string>
            {
                "Sasha", "Nick", "Dima", "Roma", "Ostap"
            };
            return names[Program.rand.Next(0, 5)];
        }
        public static string RandFemaleName()
        {
            List<String> names = new List<string>
            {
                "Ulya", "Vika", "Lina", "Zina", "Anya"
            };
            return names[Program.rand.Next(0, 5)];
        }
    }

    public abstract class Config { }

    public class ConfigRobot : Config
    {
        public int QuantityInstance { get; set; }
    }

    public class ConfigColonist : Config
    {
        public Type Type { get; set; }
        public int QuantityMale { get; set; }
        public int QuantityFemale { get; set; }
    }

    public abstract class Creator
    {
        public abstract ICollection<Unit> FactoryMethod(Config config);
    }

    public class RobotCreator : Creator
    {
        public override ICollection<Unit> FactoryMethod(Config config)
        {
            List<Unit> robots = new List<Unit>();
            int quantity = (config as ConfigRobot).QuantityInstance;
            for (int i = 0; i < quantity; i++)
            {
                robots.Add(new Robot());
            }
            return robots;
        }
    }

    public class ColonistCreator : Creator
    {
        public override ICollection<Unit> FactoryMethod(Config config)
        {
            List<Unit> colonists = new List<Unit>();
            var configColonist = (config as ConfigColonist);
            int quantityMale = configColonist.QuantityMale;
            int quantityFemale = configColonist.QuantityFemale;
            int totalQuantity = quantityMale + quantityFemale;

            for (int i = 0; i < totalQuantity; i++)
            {
                var type = configColonist.Type;
                object[] parameters;
                if (quantityMale > 0)
                {
                    parameters = new object[3] { Generator.RandMaleName(), true, Generator.RandAge() };
                    var colonistMale = (Unit)Activator.CreateInstance(type, parameters);
                    colonists.Add(colonistMale);
                    quantityMale--;
                    continue;
                }
                parameters = new object[3] { Generator.RandMaleName(), false, Generator.RandAge() };
                var colonistFemale = (Unit)Activator.CreateInstance(type, parameters);
                colonists.Add(colonistFemale);
            }
            return colonists;
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
                        Name = Generator.RandMaleName()
                    };
                case UnitType.Miner:
                    return new Miner
                    {
                        Name = Generator.RandMaleName(),
                        Gender = Generator.RandGender(),
                        Age = Generator.RandAge()
                    };
                case UnitType.Builder:
                    return new Builder
                    {
                        Gender = Generator.RandGender(),
                        Age = Generator.RandAge(),
                        Name = Generator.RandMaleName()
                    };
                case UnitType.Cook:
                    return new Cook
                    {
                        Gender = Generator.RandGender(),
                        Age = Generator.RandAge(),
                        Name = Generator.RandFemaleName()
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

        }
    }
}