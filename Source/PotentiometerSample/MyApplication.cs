using Meadow;
using Meadow.Foundation.Sensors;
using Meadow.Hardware;
using Meadow.Units;
using YoshiPi;

namespace PotentiometerSample
{
    internal sealed class MyApplication : YoshiPiApp
    {
        private Potentiometer potentiometer;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize");

            Resolver.Services.Add(
                new DisplayService(Hardware.Display, Hardware.Touchscreen)
                );

            potentiometer = new Potentiometer(Hardware.Adc.Pins.A00, 10_000.Ohms());
            potentiometer.Changed += OnPotentiometerChanged;
            return base.Initialize();
        }

        private void OnPotentiometerChanged(object? sender, IChangeResult<Resistance> e)
        {
            Resolver.Services.Get<DisplayService>()
                .SetLabelText($"{potentiometer.GetCurrentPosition()} ({e.New.Ohms:N0} ohms)");
        }

        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }
    }
}
