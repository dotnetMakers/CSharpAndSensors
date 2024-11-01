using Meadow;
using Meadow.Foundation.Sensors.Atmospheric;
using YoshiPi;

namespace AHT10Sample;
internal sealed class MyApplication : YoshiPiApp
{
    private Aht10 _sensor;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize");

        _sensor = new Aht10(Hardware.MikroBus.I2cBus);

        Resolver.Services.Add(
            new DisplayService(Hardware.Display, Hardware.Touchscreen)
            );

        _sensor.Updated += OnSensorUpdated;
        _sensor.StartUpdating(TimeSpan.FromSeconds(3));

        return base.Initialize();
    }

    private void OnSensorUpdated(object? sender, IChangeResult<(Meadow.Units.RelativeHumidity? Humidity, Meadow.Units.Temperature? Temperature)> e)
    {
        var display = Resolver.Services.Get<DisplayService>();

        var text = $"{e.New.Temperature.Value.Fahrenheit:N1}F  {e.New.Humidity.Value:N0}%";

        display?.SetLabelText(text);
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
