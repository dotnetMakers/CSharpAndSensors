using Meadow;
using Meadow.Foundation.Sensors.Rotary;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors.Rotary;
using YoshiPi;

namespace RotaryEncoderSample
{
    internal sealed class MyApplication : YoshiPiApp
    {
        private RotaryEncoderWithButton _encoder;
        private DisplayService _displayService;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize");

            _displayService = new DisplayService(Hardware.Display, Hardware.Touchscreen);

            _encoder = new RotaryEncoderWithButton(
                Hardware.Gpio.Pins.D01,
                Hardware.Gpio.Pins.D00,
                Hardware.Gpio.Pins.D02,
                ResistorMode.InternalPullUp);

            _encoder.Clicked += OnEncoderClicked;
            _encoder.Rotated += OnEncoderRotated;

            return base.Initialize();
        }

        private void OnEncoderRotated(object? sender, RotaryChangeResult e)
        {
            _displayService.SetLabelText(e.New.ToString());
        }

        private void OnEncoderClicked(object? sender, EventArgs e)
        {
            _displayService.SetLabelText("Click");
        }

        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }
    }
}
