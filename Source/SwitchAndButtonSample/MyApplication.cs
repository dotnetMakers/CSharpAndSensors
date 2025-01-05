using Meadow;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Switches;
using Meadow.Hardware;
using YoshiPi;

namespace SwitchAndButtonSample
{
    internal sealed class MyApplication : YoshiPiApp
    {
        private PushButton _button;
        private SpstSwitch _switch;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize");

            Resolver.Services.Add(
                new DisplayService(Hardware.Display, Hardware.Touchscreen)
                );

            _button = new PushButton(
                Hardware.Gpio.Pins.D01,
                Meadow.Hardware.ResistorMode.InternalPullUp);

            _switch = new SpstSwitch(
                Hardware.Gpio.Pins.D00,
                InterruptMode.EdgeBoth,
                ResistorMode.InternalPullDown);

            _button.Clicked += OnButtonClicked;
            _switch.Changed += OnSwitchChanged;

            return base.Initialize();
        }

        private void OnSwitchChanged(object? sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void OnButtonClicked(object? sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            var text = $"Button {(_button.State ? "pressed" : "released")}  "
                + $"Switch {(_switch.IsOn ? "on" : "off")}";

            Resolver.Services.Get<DisplayService>()
                .SetLabelText(text);
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
