using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;

namespace WpfRacingSimulator
{
    class Race
    {
        private static System.Timers.Timer aTimer_;
        List<Vechle> vechles;
        public Race()
        {
            // Create a timer with a two second interval.
            aTimer_ = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            aTimer_.Elapsed += OnTimedEvent;
            aTimer_.AutoReset = true;
            aTimer_.Enabled = true;

            vechles = new List<Vechle> { 
                new Truck() { Speed = 50, DamageRand = 0.1, Weight = 1000 }
                , new Car() { Speed = 90, DamageRand = 0.1, Passengers = 2 }
                , new Moto() { Speed = 120, DamageRand = 0.1, IsSidecar = false }
            };
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }
        public void Start() 
        {
            aTimer_.Start();
        }
        public void Finish()
        {
            aTimer_.Stop();
        }
        public void ParseCinfig(String jsonString) 
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
        }
    }
}
