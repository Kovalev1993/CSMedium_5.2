using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMedium_5._2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<IMovable> wave = new List<IMovable>() {
                new UnitBuilder().WithMagicalArmor().Flying().Create(),
                new Squad(new List<Unit>(){
                    new UnitBuilder().WithMagicalArmor().Flying().Create(),
                    new UnitBuilder().WithPhysicalArmor().MovingOnPath().Create()
                }),
                new UnitBuilder().WithPhysicalArmor().Create()
            };

            Spawner spawner = new Spawner(wave, 2);

            while(true)
            {
                if(spawner.CanSpawn())
                    spawner.Spawn();
            }
        }
    }

    class Spawner
    {
        public int UnitsDelayInSeconds { get; private set; }

        private List<IMovable> _wave;
        private DateTime _lastSpawnTime;

        public Spawner(List<IMovable> wave, int unitsDelayInSeconds)
        {
            _wave = wave;
            UnitsDelayInSeconds = unitsDelayInSeconds;
            _lastSpawnTime = DateTime.Now.AddDays(-1);
        }

        public void Spawn()
        {
            _wave.ElementAt(0).Instantiate();
            _wave.RemoveAt(0);
            _lastSpawnTime = DateTime.Now;
        }

        public bool CanSpawn()
        {
            return (_wave.Count > 0) && (DateTime.Now - _lastSpawnTime).TotalSeconds >= UnitsDelayInSeconds;
        }
    }

    class UnitBuilder
    {
        public Unit CreatingObject { get; private set; }

        public UnitBuilder()
        {
            CreatingObject = new Unit();
        }

        public ArmoredUnitBuilder WithPhysicalArmor()
        {
            CreatingObject.Components.Add(new Health(new PhysicalArmor()));
            return new ArmoredUnitBuilder(CreatingObject);
        }

        public ArmoredUnitBuilder WithMagicalArmor()
        {
            CreatingObject.Components.Add(new Health(new MagicArmor()));
            return new ArmoredUnitBuilder(CreatingObject);
        }

        public virtual Unit Create()
        {
            return CreatingObject;
        }
    }

    class ArmoredUnitBuilder
    {
        public Unit CreatingObject { get; private set; }

        public ArmoredUnitBuilder(Unit creatingObject)
        {
            CreatingObject = creatingObject;
        }

        public FinalUnitBuilder MovingOnPath()
        {
            CreatingObject.Components.Add(new PathMovement());
            return new FinalUnitBuilder(CreatingObject);
        }

        public FinalUnitBuilder Flying()
        {
            CreatingObject.Components.Add(new FlyMovement());
            return new FinalUnitBuilder(CreatingObject);
        }

        public Unit Create()
        {
            return CreatingObject;
        }
    }

    class FinalUnitBuilder
    {
        public Unit CreatingObject { get; private set; }
        public IArmor Armor { get; private set; }
        public Movement Movement { get; private set; }

        public FinalUnitBuilder(Unit creatingObject)
        {
            CreatingObject = creatingObject;
        }

        public Unit Create()
        {
            return CreatingObject;
        }
    }

    interface IMovable
    {
        void Instantiate();
        void Move();
    }

    class Squad : IMovable
    {
        private List<Unit> _units;

        public Squad(List<Unit> units)
        {
            _units = units;
        }

        public void Move()
        {
            foreach(var unit in _units)
            {
                unit.Move();
            }
        }

        public void Instantiate()
        {
            foreach(var unit in _units)
            {
                unit.Instantiate();
            }
            Console.WriteLine("Squad was instantiated");
        }
    }

    class Unit : IMovable
    {
        public List<IComponent> Components;

        public Unit()
        {
            Components = new List<IComponent>();
        }

        public void Move() { }

        public void Instantiate()
        {
            Console.WriteLine("Unit was instantiated");
        }
    }

    interface IComponent
    {
        void Start();
        void Update();
    }

    class Health : IComponent
    {
        public IArmor Armor { get; private set; }

        public Health(IArmor armor)
        {
            Armor = armor;
        }

        public void Start() { }

        public void Update() { }
    }

    interface IArmor
    {
        int ProcessDamage(int damage);
    }

    class MagicArmor : IArmor
    {
        public int ProcessDamage(int damage)
        {
            return (int)(damage / 3);
        }
    }

    class PhysicalArmor : IArmor
    {
        public int ProcessDamage(int damage)
        {
            return (int)(damage / 2);
        }
    }

    abstract class Movement : IComponent
    {
        public void Start() { }

        public void Update() { }

        public abstract void Move();
    }

    class PathMovement : Movement
    {
        public override void Move() { }
    }

    class FlyMovement : Movement
    {
        public override void Move() { }
    }
}
