using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfRacingSimulator
{
    struct VechleInfo 
    {
        public string VehicleType;
        public int Speed;
        public double DamageRandom;
        public int Odometr;
        public string IsDamaged;
        public string AdditionalInfo;
    }
    interface IVechle 
    {
        public void Startup();
        public void Shutdown();
        public void Run();
        public VechleInfo GetInfo();
    }
    abstract class Vechle : IVechle, INotifyPropertyChanged
    {
        private string vehicleType_ = "";
        private int speed_ = 20;
        private double damageRandom_;
        private int odometr_ = 0;
        private bool isEanbleRun_ = false;
        private int damageCount_;
        private int damageCountInit_ = 5;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string VehicleType
        {
            get => vehicleType_;
            set
            {
                vehicleType_ = value;
                OnPropertyChanged("VehicleType");
            }
        }
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
        
        public double DamageRandom
        {
            get => damageRandom_;
            set
            {
                if ((value > 0) && (value < 1))
                {
                    damageRandom_ = value;
                }
            }
        }
        public bool isEanbleRun
        {
            get => isEanbleRun_;
            
        }
        public void Startup() 
        {
            odometr_ = 0;
            isEanbleRun_ = true;
        }
        public void Shutdown() 
        {
            isEanbleRun_ = false;
        }
        public void Run() 
        {
            if (isEanbleRun_ && !isDamage())
            {
                odometr_ += speed_;
            }
        }
        public bool isDamage() 
        {
            bool result = false;
            if (--damageCount_ > 0) 
            {
                result = true;
            } 
            else 
            {
                var rand = new Random();
                if (rand.NextDouble() < damageRandom_)
                {
                    damageCount_ = damageCountInit_;
                    result = true;
                }
            }
            return result;
        }
        public virtual VechleInfo GetInfo()
        {
            string damage;
            if (damageCount_ > 0)
            {
                damage = "Wheel damaged";
            }
            else 
            {
                damage = "";
            }
                
            return new VechleInfo()
            {
                VehicleType = vehicleType_,
                DamageRandom = damageRandom_,
                Odometr = odometr_,
                IsDamaged = damage,
                Speed = speed_
            };
        }
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
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
        public override VechleInfo GetInfo()
        {
            VechleInfo info = base.GetInfo();
            string infoStr = "Weight: " + weight_.ToString();

            info.AdditionalInfo = infoStr;
            return info;
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
        public override VechleInfo GetInfo()
        {
            VechleInfo info = base.GetInfo();
            string infoStr = "Passengers: " + passengers_.ToString();

            info.AdditionalInfo = infoStr;
            return info;
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
        public override VechleInfo GetInfo()
        {
            VechleInfo info = base.GetInfo();
            string infoStr;
            if (isSidecar_)
            {
                infoStr = "Sidercar preset";
            } 
            else {
                infoStr = "\t";
            }
            info.AdditionalInfo = infoStr;
            return info;
        }
    }
}
