using Meadow;
using Meadow.Foundation.Sensors.Switches;
using Meadow.Hardware;
using YoshiPi;

namespace FloatSwitchSample
{
    internal sealed class MyApplication : YoshiPiApp
    {
        private SpstSwitch sensor;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize");

            Resolver.Services.Add(
                new DisplayService(Hardware.Display, Hardware.Touchscreen)
                );

            sensor = new SpstSwitch(
                Hardware.Gpio.Pins.D00,
                InterruptMode.EdgeBoth,
                ResistorMode.InternalPullDown);

            sensor.Changed += OnSensorChanged;

            return base.Initialize();
        }

        private void OnSensorChanged(object? sender, EventArgs e)
        {
            var display = Resolver.Services.Get<DisplayService>();

            display?.SetLabelText(
                $"Tank is {(sensor.IsOn ? "NOT" : "")} full");
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
}
