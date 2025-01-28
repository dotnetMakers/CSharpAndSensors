using Meadow;
using Meadow.Foundation.Sensors.Distance;
using YoshiPi;

namespace UltrasonicSample;

internal sealed class MyApplication : YoshiPiApp
{
    private A02yyuw _sensor;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize");

        Resolver.Services.Add(
            new DisplayService(Hardware.Display, Hardware.Touchscreen)
            );

        _sensor = new A02yyuw(Hardware.MikroBus.CreateSerialPort(9600));
        _sensor.Updated += OnSensorUpdated;
        _sensor.StartUpdating(TimeSpan.FromMilliseconds(500));

        return base.Initialize();
    }

    private void OnSensorUpdated(object? sender, IChangeResult<Meadow.Units.Length> e)
    {
        var display = Resolver.Services.Get<DisplayService>();

        display.SetLabelText($"Distance: {e.New.Centimeters:N1}");
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run");

        var display = Resolver.Services.Get<DisplayService>();

        display?.SetLabelText("Hello YoshiPi!");

        return base.Run();
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
