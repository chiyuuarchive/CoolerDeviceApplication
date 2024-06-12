using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCooler
{
    internal interface IDevice
    {
        double CriticalTemperature { get; }
        double TemperatureToRegulate { get; }
        void Run();
        void FireAlarm();
    }

    public class Device : IDevice
    {
        private const double criticalTemperature = 50;
        private const double temperatureToRegulate = 21;
        double IDevice.CriticalTemperature => criticalTemperature;
        double IDevice.TemperatureToRegulate => temperatureToRegulate;
        public void Run()
        {
            ICooler cooler = new Cooler();
            IHeatSensor sensor = new HeatSensor(criticalTemperature, temperatureToRegulate);
            IEventSystem system = new EventSystem(cooler, sensor, this);

            system.Run();
        }
        public void FireAlarm()
        {
            Console.WriteLine("Notifying the fire brigade about ensuing fire\n");
            ShutDownDevice();
        }

        private void ShutDownDevice()
        {
            Console.WriteLine("Prompting fire extinguisher system...");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
}
