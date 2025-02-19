using Meadow;
using Meadow.Foundation.Sensors.Color;
using YoshiPi;

namespace ColorSample;

internal sealed class MyApplication : YoshiPiApp
{
    private Tcs3472x _sensor;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize");

        _sensor = new Tcs3472x(Hardware.MikroBus.I2cBus);
        _sensor.Updated += OnSensorUpdated;
        _sensor.StartUpdating(TimeSpan.FromSeconds(1));

        Resolver.Services.Add(
            new DisplayService(Hardware.Display, Hardware.Touchscreen)
            );

        return base.Initialize();
    }

    private void OnSensorUpdated(object? sender, IChangeResult<Color> e)
    {
        var display = Resolver.Services.Get<DisplayService>();
        display.SetLabelText($"RGB: [{e.New.R},{e.New.G},{e.New.B}]");
        display.SetBackColor(e.New);
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
