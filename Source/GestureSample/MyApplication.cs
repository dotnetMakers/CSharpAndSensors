using Meadow;
using Meadow.Foundation.Sensors.Motion;
using YoshiPi;

namespace GestureSample
{
    internal sealed class MyApplication : YoshiPiApp
    {
        private Apds9960 _sensor;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize");

            var interrupt = Hardware.MikroBus.Pins.INT.CreateDigitalInterruptPort(
                Meadow.Hardware.InterruptMode.EdgeFalling, Meadow.Hardware.ResistorMode.InternalPullUp);
            interrupt.Changed += OnInterrupt;

            _sensor = new Apds9960(Hardware.Qwiic, null);
            _sensor.EnableGestureSensor(true);

            Resolver.Services.Add(
                new DisplayService(Hardware.Display, Hardware.Touchscreen)
                );

            return base.Initialize();
        }

        private void OnInterrupt(object? sender, Meadow.Hardware.DigitalPortResult e)
        {
            var direction = _sensor.ReadGesture();

            if (direction != Apds9960.Direction.NONE)
            {
                var display = Resolver.Services.Get<DisplayService>();
                display.SetLabelText(direction.ToString());
            }
        }

        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }
    }
}
