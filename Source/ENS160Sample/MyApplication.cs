using Meadow;
using Meadow.Foundation.Sensors.Environmental;
using YoshiPi;

namespace ENS160Sample
{
    internal sealed class MyApplication : YoshiPiApp
    {
        private Ens160 _sensor;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize");

            _sensor = new Ens160(Hardware.Qwiic, Ens160.Addresses.Address_0x53);
            _sensor.EthanolConcentrationUpdated += OnEthanolConcentrationUpdated;
            _sensor.StartUpdating();

            Resolver.Services.Add(
                new DisplayService(Hardware.Display, Hardware.Touchscreen)
                );

            return base.Initialize();
        }

        private void OnEthanolConcentrationUpdated(object? sender, IChangeResult<Meadow.Units.Concentration> e)
        {
            var display = Resolver.Services.Get<DisplayService>();

            display?.SetLabelText($"{e.New.PartsPerThousand} ppt");
        }

        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }
    }
}
