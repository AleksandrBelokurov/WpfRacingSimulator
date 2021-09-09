using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfRacingSimulator
{
    class Race
    {
        List<Vechle> vechles;
        public Race()
        {
            vechles = new List<Vechle> { 
                new Truck() { Speed = 50, DamageRand = 0.1, Weight = 1000 }
                , new Car() { Speed = 90, DamageRand = 0.1, Passengers = 2 }
                , new Moto() { Speed = 120, DamageRand = 0.1, IsSidecar = false }
            };
        }
    }
}
