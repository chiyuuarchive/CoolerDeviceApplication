using System.ComponentModel;

namespace SmartCooler
{
    public interface IHeatSensor
    {
        event EventHandler<HeatSensorData> OnExceedingCriticalTemperature;
        event EventHandler<HeatSensorData> OnExceedingRegulatedTemperature;
        event EventHandler<HeatSensorData> OnSubceedRegulatedTemperature;

        void Run() { }
    }

    internal class HeatSensor : IHeatSensor
    {
        double criticalTemperature;
        double temperatureToRegulate;

        bool isCoolerOn;

        protected EventHandlerList sensorEvents = new EventHandlerList();

        // Keys to access the list of event delegates
        static readonly object exceedCriticalTempKey = new object();
        static readonly object exceedRegulatedTempKey = new object();
        static readonly object subceedRegulatedTempKey = new object();

        private double[] temperatureData;

        public HeatSensor(double criticalTemperature, double temperatureToRegulate)
        {
            this.criticalTemperature = criticalTemperature;
            this.temperatureToRegulate = temperatureToRegulate;
            isCoolerOn = false;

            SeedData();
        }
        public void Run()
        {
            Console.WriteLine($"Heat sensor is monitoring the temperature\n");
            MonitorTemperature();
        }

        private void SeedData()
        {
            temperatureData = new double[] { 16, 17, 18, 23, 60, 43, 23.2, 56.5, 45, 2, 0, 123, 76 };
        }

        private void MonitorTemperature()
        {
            foreach (double temperature in temperatureData)
            {
                Console.ResetColor();
                Console.WriteLine($"DateTime: {DateTime.Now}, Temperature: {temperature}");

                if (temperature >= criticalTemperature)
                {
                    // Alarm the fire brigade
                    HeatSensorData data = new HeatSensorData
                    {
                        Temperature = temperature,
                        CurrentDateTime = DateTime.Now,
                    };
                    OnCriticalTempExceeded(data);
                }
                else if (temperature >= temperatureToRegulate && !isCoolerOn)
                {
                    // Turn on the cooler
                    isCoolerOn = true;

                    HeatSensorData data = new HeatSensorData
                    {
                        Temperature = temperature,
                        CurrentDateTime = DateTime.Now
                    };
                    OnRegulatedTempExceeded(data);
                }
                else if (temperature < temperatureToRegulate && isCoolerOn)
                {
                    // Turn off the cooler once threshold is subceeded
                    isCoolerOn = false;

                    HeatSensorData data = new HeatSensorData
                    {
                        Temperature = temperature,
                        CurrentDateTime = DateTime.Now
                    };
                    OnRegulatedTempSubsceeded(data);
                }
                Thread.Sleep(1500);
            }
        }

        private void OnCriticalTempExceeded(HeatSensorData data)
        {
            EventHandler<HeatSensorData> handler = (EventHandler<HeatSensorData>)sensorEvents[exceedCriticalTempKey]!;
            if (handler != null) handler(this, data);
        }
        private void OnRegulatedTempExceeded(HeatSensorData data)
        {
            EventHandler<HeatSensorData> handler = (EventHandler<HeatSensorData>)sensorEvents[exceedRegulatedTempKey]!;
            if (handler != null) handler(this, data);
        }
        private void OnRegulatedTempSubsceeded(HeatSensorData data)
        {
            EventHandler<HeatSensorData> handler = (EventHandler<HeatSensorData>)sensorEvents[subceedRegulatedTempKey]!;
            if (handler != null) handler(this, data);
        }
        event EventHandler<HeatSensorData> IHeatSensor.OnExceedingCriticalTemperature
        {
            add
            {
                sensorEvents.AddHandler(exceedCriticalTempKey, value);
            }
            remove
            {
                sensorEvents.RemoveHandler(exceedCriticalTempKey, value);
            }
        }

        event EventHandler<HeatSensorData> IHeatSensor.OnExceedingRegulatedTemperature
        {
            add
            {
                sensorEvents.AddHandler(exceedRegulatedTempKey, value);
            }
            remove
            {
                sensorEvents.RemoveHandler(exceedRegulatedTempKey, value);
            }
        }

        event EventHandler<HeatSensorData> IHeatSensor.OnSubceedRegulatedTemperature
        {
            add
            {
                sensorEvents.AddHandler(subceedRegulatedTempKey, value);
            }
            remove
            {
                sensorEvents.RemoveHandler(subceedRegulatedTempKey, value);
            }
        }
    }
}
