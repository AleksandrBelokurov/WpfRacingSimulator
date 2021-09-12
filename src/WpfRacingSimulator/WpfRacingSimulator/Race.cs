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
        List<Vechle> vechles;
        MainWindow mainPage_;
        private int distance_ = 2000;
        private int droveDistance_;
        List<int> odometrs_;
        private StringBuilder score_;
        private int place_;
        public Race(MainWindow mainPage)
        {
            mainPage_ = mainPage;
            aTimer_ = new System.Timers.Timer(100); 
            aTimer_.Elapsed += OnTimedEvent;
            odometrs_ = new List<int>();
            score_ = new StringBuilder("");

            vechles = new List<Vechle> 
            { 
                new Truck() { VehicleType = "Truck", Speed = 50, DamageRandom = 0.1, Weight = 1000 },
                new Moto() { VehicleType = "Moto", Speed = 110, DamageRandom = 0.03, IsSidecar = true },
                new Car() { VehicleType = "Car", Speed = 90, DamageRandom = 0.05, Passengers = 2 },
                new Moto() { VehicleType = "Moto", Speed = 120, DamageRandom = 0.03, IsSidecar = false },
                new Car() { VehicleType = "Car", Speed = 90, DamageRandom = 0.05, Passengers = 1 },
                new Truck() { VehicleType = "Truck", Speed = 40, DamageRandom = 0.1, Weight = 2500 }
            };
        }
        public void Start()
        {
            droveDistance_ = 0;
            place_ = 0;
            score_.Clear();
            SetProgressMaximum(mainPage_, distance_);
            foreach (var vechle in vechles)
            {
                vechle.Startup();
            }
            aTimer_.Start();
        }
        private void OnTimedEvent(Object ?source, ElapsedEventArgs e)
        {
            odometrs_.Clear();
            StringBuilder sb = new StringBuilder("");
            foreach (var vechle in vechles)
            {
                vechle.Run();
            }
            foreach (var vechle in vechles)
            {
                VechleInfo info = vechle.GetInfo();
                odometrs_.Add(info.Odometr);
                sb.Append(info.VehicleType);
                sb.Append("\tSpeed: " + info.Speed.ToString());
                sb.Append("\tWheel puncture probability: " + info.DamageRandom.ToString());
                sb.Append("\t" + info.AdditionalInfo);
                int distance = info.Odometr;
                if (distance > distance_) 
                {
                    distance = distance_;
                }
                sb.Append("\tOdometer reading: " + distance.ToString());
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
                        score_.Append("\tOdometer reading: " + distance_.ToString());
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
            foreach (var vechle in vechles)
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
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                double sum = 0;
                int count = 0;

                JsonElement root = document.RootElement;
                JsonElement studentsElement = root.GetProperty("Students");

                count = studentsElement.GetArrayLength();

                foreach (JsonElement student in studentsElement.EnumerateArray())
                {
                    if (student.TryGetProperty("Grade", out JsonElement gradeElement))
                    {
                        sum += gradeElement.GetDouble();
                    }
                    else
                    {
                        sum += 70;
                    }
                }
            }
            StringBuilder sb = new StringBuilder("");
            foreach (var vechle in vechles)
            {
                VechleInfo info = vechle.GetInfo();
                sb.Append(info.VehicleType);
                sb.Append("\tSpeed: " + info.Speed.ToString());
                sb.Append("\tWheel punct: " + info.DamageRandom.ToString());
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
