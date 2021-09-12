using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Timers;

namespace WpfRacingSimulator
{
    class Race
    {
        private System.Timers.Timer aTimer_;
        int timeout_ = 100;
        List<Vechle> vechles_;
        MainWindow mainPage_;
        private int distance_;
        private int droveDistance_;
        List<int> odometrs_;
        private StringBuilder score_;
        private int place_;
        public Race(MainWindow mainPage)
        {
            mainPage_ = mainPage;
            aTimer_ = new System.Timers.Timer(timeout_);
            aTimer_.Elapsed += OnTimedEvent;
            odometrs_ = new List<int>();
            score_ = new StringBuilder("");
            vechles_ = new List<Vechle>();
        }
        public void Start()
        {
            droveDistance_ = 0;
            place_ = 0;
            score_.Clear();
            SetProgressMaximum(mainPage_, distance_);
            foreach (var vechle in vechles_)
            {
                vechle.Startup();
            }
            aTimer_.Start();
        }
        private void OnTimedEvent(Object ?source, ElapsedEventArgs e)
        {
            odometrs_.Clear();
            StringBuilder sb = new StringBuilder("");
            foreach (var vechle in vechles_)
            {
                vechle.Run();
            }
            foreach (var vechle in vechles_)
            {
                VechleInfo info = vechle.GetInfo();
                odometrs_.Add(info.Odometr);
                sb.Append(info.VehicleType);
                sb.Append("\tSpeed: " + info.Speed.ToString());
                sb.Append("\tWheel: " + info.DamageRandom.ToString());
                sb.Append("\t" + info.AdditionalInfo);
                int distance = info.Odometr;
                if (distance > distance_) 
                {
                    distance = distance_;
                }
                sb.Append("\tOdometer: " + distance.ToString());
                sb.Append("\t" + info.IsDamaged);
                sb.Append("\n");
                if (info.Odometr >= distance_) 
                {
                    if (vechle.isEanbleRun)
                    {
                        vechle.Shutdown();
                        ++place_;
                        score_.Append(place_.ToString() + " place\t");
                        score_.Append(info.VehicleType);
                        score_.Append("\tSpeed: " + info.Speed.ToString());
                        score_.Append("\tWheel: " + info.DamageRandom.ToString());
                        score_.Append("\t" + info.AdditionalInfo);
                        score_.Append("\tOdometer: " + distance_.ToString());
                        score_.Append("\n");
                    }
                }
            }
            DisplayVechleInfo(mainPage_, sb.ToString());
            droveDistance_ = odometrs_.Min();
            SetLeftToFinish(mainPage_, (distance_ - droveDistance_).ToString());
            SetProgressValue(mainPage_, droveDistance_);
            if (droveDistance_ >= distance_)
            {
                Finish(mainPage_);
                DisplayRaceInfo(mainPage_, "The race is finished. To race again press \"Start\" button.");
            }
        }
        
        public void Finish(MainWindow mainPage)
        {
            aTimer_.Stop();
            foreach (var vechle in vechles_)
            {
                vechle.Shutdown();
            }
            if (mainPage != null)
            {
                DisplayVechleInfo(mainPage, score_.ToString());
                mainPage.Finish();
            }
        }
        public void ParseConfig(String jsonString) 
        {
            distance_ = 0;
            vechles_.Clear();
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;
                if (root.TryGetProperty("Distance", out JsonElement distanceElement))
                {
                    distance_ = distanceElement.GetInt32();
                }
                if (root.TryGetProperty("Timeout", out JsonElement timeoutElement))
                {
                    timeout_ = timeoutElement.GetInt32();
                    aTimer_.Interval= timeout_;
                }
                
                int count = 0;
                int speed = 0;
                double damageRandom = 0.0;
                int weight = 0;
                int passengers = 0;
                bool isSidecar = false;
                JsonElement vechlesElement = root.GetProperty("Vechles");
                count = vechlesElement.GetArrayLength();
                string ?vehicleType = "";
                foreach (JsonElement vechle in vechlesElement.EnumerateArray())
                {
                    if (vechle.TryGetProperty("VehicleType", out JsonElement vehicleTypeElement))
                    {
                        vehicleType = vehicleTypeElement.GetString();
                    }
                    if (vechle.TryGetProperty("Speed", out JsonElement speedElement))
                    {
                        speed = speedElement.GetInt32();
                    }
                    if (vechle.TryGetProperty("DamageRandom", out JsonElement damageRandomElement))
                    {
                        damageRandom = damageRandomElement.GetDouble();
                    }
                    if (!string.IsNullOrEmpty(vehicleType) && vehicleType.Equals("Truck"))
                    {
                        if (vechle.TryGetProperty("Weight", out JsonElement weightElement))
                        {
                            weight = weightElement.GetInt32();
                            vechles_.Add(new Truck() 
                            { 
                                VehicleType = "Truck", 
                                Speed = speed, 
                                DamageRandom = damageRandom,
                                Weight = weight
                            });
                        }
                    }
                    if (!string.IsNullOrEmpty(vehicleType) && vehicleType.Equals("Car"))
                    {
                        if (vechle.TryGetProperty("Passengers", out JsonElement passengersElement))
                        {
                            weight = passengersElement.GetInt32();
                            vechles_.Add(new Car()
                            {
                                VehicleType = "Car",
                                Speed = speed,
                                DamageRandom = damageRandom,
                                Passengers = passengers
                            });
                        }
                    }
                    if (!string.IsNullOrEmpty(vehicleType) && vehicleType.Equals("Moto"))
                    {
                        if (vechle.TryGetProperty("IsSidecar", out JsonElement isSidecarElement))
                        {
                            isSidecar = isSidecarElement.GetBoolean();
                            vechles_.Add(new Moto()
                            {
                                VehicleType = "Moto",
                                Speed = speed,
                                DamageRandom = damageRandom,
                                IsSidecar = isSidecar
                            });
                        }
                    }
                }
            }
            StringBuilder sb = new StringBuilder("");
            foreach (var vechle in vechles_)
            {
                VechleInfo info = vechle.GetInfo();
                sb.Append(info.VehicleType);
                sb.Append("\tSpeed: " + info.Speed.ToString());
                sb.Append("\tWheel: " + info.DamageRandom.ToString());
                sb.Append("\t" + info.AdditionalInfo);
                sb.Append("\n");
            }
            DisplayVechleInfo(mainPage_, sb.ToString());
        }
        public void SetLeftToFinish(MainWindow mainPage, string message)
        {
            if (mainPage != null)
            {
                mainPage.LeftToFinish = message;
            }
        }
        public void SetProgressMaximum(MainWindow mainPage, double value)
        {
            if (mainPage != null)
            {
                mainPage.ProgressMaximum = value;
            }
        }
        public void SetProgressValue(MainWindow mainPage, double value)
        {
            if (mainPage != null)
            {
                mainPage.ProgressValue = value;
            }
        }
        public void DisplayRaceInfo(MainWindow mainPage, string message)
        {
            if (mainPage != null)
            {
                mainPage.InfoString = message;
            }
        }
        public void DisplayVechleInfo(MainWindow mainPage, String message)
        {
            if (mainPage != null)
            {
                mainPage.InfoBoard = message;
            }
        }
    }
}
