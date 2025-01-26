using Meadow;
using Meadow.Devices;
using Meadow.Peripherals.Sensors.Flow;
using System.Threading.Tasks;

namespace FlowSensorSample;

public class MeadowApp : ProjectLabCoreComputeApp
{
    private YfB1 _sensor;

    public override Task Initialize()
    {
        _sensor = new YfB1(Hardware.MikroBus1.Pins.PWM);

        return base.Initialize();
    }

    public override async Task Run()
    {
        while (true)
        {
            var flow = await _sensor.Read();

            Resolver.Log.Info($"Flow: {flow.GallonsPerMinute:N2} gpm");
            await Task.Delay(500);
        }
    }
}