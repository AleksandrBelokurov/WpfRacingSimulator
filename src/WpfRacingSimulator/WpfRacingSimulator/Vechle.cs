using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfRacingSimulator
{
    abstract class Vechle
    {
        int speed_;
        public int Speed
        {
            get => speed_;
            set
            {
                if ((value > 0) && (value < 180))
                {
                    speed_ = value;
                }
            }
        }
        double damageRand_;
        public double DamageRand
        {
            get => damageRand_;
            set
            {
                if ((value > 0) && (value < 1))
                {
                    damageRand_ = value;
                }
            }
        }
        int distance_;
        public int Distance
        {
            get => distance_;
            set
            {
                if ((value > 0) && (value < 1000))
                {
                    distance_ = value;
                }
            }
        }
        int damageCount_;
        int damageCountInit_ = 5;
        public void Run() 
        {
            Distance -= speed_;
        }
        public void Damage() 
        {
            var rand = new Random();
            if (damageCount_ > 0)
            {
                --damageCount_;
            } else if (rand.NextDouble() < damageRand_)
            {
                damageCount_ = damageCountInit_;
            }
        }
    }
    class Truck : Vechle
    {
        int weight_;
        public int Weight
        {
            get => weight_;
            set
            {
                if ((value >= 0) && (value < 5000))
                {
                    weight_ = value;
                }
            }
        }
    }
    class Car : Vechle
    {
        int passengers_;
        public int Passengers
        {
            get => passengers_;
            set
            {
                if ((value >= 0) && (value < 4))
                {
                    passengers_ = value;
                }
            }
        }
    }
    class Moto : Vechle
    {
        bool isSidecar_;
        public bool IsSidecar
        {
            get => isSidecar_;
            set
            {
                isSidecar_ = value;
            }
        }
    }
}
