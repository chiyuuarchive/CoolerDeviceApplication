using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCooler
{
    internal interface IEventSystem
    {
        void Run();
    }

    internal class EventSystem : IEventSystem
    {
        private ICooler cooler;
        private IHeatSensor sensor;
        private IDevice device;

        public EventSystem(ICooler cooler, IHeatSensor sensor, IDevice device)
        {
            this.cooler = cooler;
            this.sensor = sensor;
            this.device = device;
        }

        public void Run()
        {
            Console.WriteLine($"AC is running.\nRegulated Temperature: {device.TemperatureToRegulate}\nCritical Temperature: {device.CriticalTemperature}\n");
            BindDelegatesToSensorEvent();
            sensor.Run();
        }

        private void BindDelegatesToSensorEvent()
        {
            sensor.OnExceedingCriticalTemperature += HandleCriticalTempExceeded!;
            sensor.OnExceedingRegulatedTemperature += HandleRegulatedTempExceeded!;
            sensor.OnSubceedRegulatedTemperature += HandleRegulatedTempSubsceeded!;
        }

        // Delegates
        private void HandleCriticalTempExceeded(object sender, HeatSensorData data)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ambient temperature reached critical threshold ({data.Temperature} Celsius). Executing alarm protocol...\n");
            device.FireAlarm();
        }

        private void HandleRegulatedTempExceeded(object sender, HeatSensorData data)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Exceeded the temperature to regulate ({data.Temperature} Celsius). Executing cooling protocol...\n");
            cooler.Off();
        }

        private void HandleRegulatedTempSubsceeded(object sender, HeatSensorData data)
        {
            Console.WriteLine($"Subceeded temperature to regulated ({data.Temperature} Celsius)\n");
            cooler.On();
        }
    }
}
