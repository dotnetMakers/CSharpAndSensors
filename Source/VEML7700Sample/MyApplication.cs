using Meadow;
using Meadow.Foundation.Sensors.Light;
using YoshiPi;

namespace VEML7700Sample;

internal sealed class MyApplication : YoshiPiApp
{
    private Veml7700 _sensor;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize");

        Resolver.Services.Add(
            new DisplayService(Hardware.Display, Hardware.Touchscreen)
            );

        _sensor = new Veml7700(Hardware.MikroBus.I2cBus);
        _sensor.Updated += OnSensorUpdated;
        _sensor.StartUpdating(TimeSpan.FromSeconds(1));

        return base.Initialize();
    }

    private void OnSensorUpdated(object? sender, IChangeResult<Meadow.Units.Illuminance> e)
    {
        var displayService = Resolver.Services.Get<DisplayService>();

        var text = $"{e.New.Lux:N0}";
        displayService?.SetLabelText(text);
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
