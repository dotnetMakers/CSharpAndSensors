using Meadow;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Units;
using YoshiPi;

namespace ThermistorSample
{
    internal sealed class MyApplication : YoshiPiApp
    {
        private Thermistor _sensor;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize");

            Resolver.Services.Add(
                new DisplayService(Hardware.Display, Hardware.Touchscreen)
                );

            _sensor = new Thermistor(
                Hardware.MikroBus.Pins.AN.CreateAnalogInputPort(1),
                referenceResistor: 10_000.Ohms(),
                ThermistorType.NTC,
                ThermistorPlacement.HighSide,
                3.3.Volts());

            return base.Initialize();
        }

        public override async Task Run()
        {
            while (true)
            {
                var temp = await _sensor.Read();
                Resolver.Services.Get<DisplayService>()?.SetLabelText($"{temp.Fahrenheit:N1}F");
                await Task.Delay(1000);
            }
        }

        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }
    }
}
