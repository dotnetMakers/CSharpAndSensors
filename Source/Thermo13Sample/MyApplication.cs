using Meadow;
using Meadow.Foundation.mikroBUS;
using YoshiPi;

namespace Thermo13Sample
{
    internal sealed class MyApplication : YoshiPiApp
    {
        private CThermo13 _sensor;
        private DisplayService _display;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize");

            _display = new DisplayService(Hardware.Display, Hardware.Touchscreen);
            _sensor = new CThermo13(Hardware.MikroBus.I2cBus);
            _sensor.Updated += OnSensorUpdated;
            _sensor.StartUpdating(TimeSpan.FromSeconds(2));

            return base.Initialize();
        }

        private void OnSensorUpdated(object? sender, IChangeResult<Meadow.Units.Temperature> e)
        {
            _display.SetLabelText($"{e.New.Celsius:N1}C");
        }

        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }
    }
}
