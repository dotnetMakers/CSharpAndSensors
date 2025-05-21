using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Temperature;
using System.Threading.Tasks;

namespace ThermocoupleSample;

public class MeadowApp : App<F7FeatherV2>
{
    private Max6675 _sensor;

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        _sensor = new Max6675(Device.CreateSpiBus(), Device.Pins.D00);

        while (true)
        {
            var temp = await _sensor.Read();
            Resolver.Log.Info($"Temp: {temp.Fahrenheit:N1}");
            await Task.Delay(1000);
        }
    }

}