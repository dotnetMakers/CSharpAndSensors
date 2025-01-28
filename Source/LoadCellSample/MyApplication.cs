using Meadow;
using Meadow.Foundation.Sensors.LoadCell;
using YoshiPi;

namespace LoadCellSample
{
    internal sealed class MyApplication : YoshiPiApp
    {
        private Nau7802 _sensor;
        private int _calibrationStep = 0;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize");

            Resolver.Services.Add(
                new DisplayService(Hardware.Display, Hardware.Touchscreen)
                );

            CalibrateStateMachine();

            _sensor = new Nau7802(Hardware.MikroBus.I2cBus);
            Hardware.Button1.Clicked += OnButton1Clicked;
            return base.Initialize();
        }

        private void CalibrateStateMachine()
        {
            var display = Resolver.Services.Get<DisplayService>();

            while (true)
            {
                switch (_calibrationStep)
                {
                    case 0:
                        display?.SetLabelText("click when at 0");
                        break;
                    case 1:
                        _sensor.Tare();
                        _calibrationStep++;
                        break;
                    case 2:
                        display?.SetLabelText("click to calibrate");
                        break;
                    case 3:
                        display?.SetLabelText("calibrating...");
                        Thread.Sleep(500);
                        var calibrationFactor = _sensor.CalculateCalibrationFactor();
                        _sensor.SetCalibrationFactor(calibrationFactor,
                            new Meadow.Units.Mass(1640, Meadow.Units.Mass.UnitType.Grams));
                        _calibrationStep++;
                        break;
                    case 4:
                        display?.SetLabelText("remove weight");
                        break;
                    case 5:
                        _sensor.Tare();
                        _sensor.Updated += OnSensorUpdated;
                        _sensor.StartUpdating();
                        return;
                }

                Thread.Sleep(250);
            }
        }

        private void OnSensorUpdated(object? sender, IChangeResult<Meadow.Units.Mass> e)
        {
            var display = Resolver.Services.Get<DisplayService>();
            display?.SetLabelText($"{e.New.Grams:N0} g");
        }

        private void OnButton1Clicked(object? sender, EventArgs e)
        {
            _calibrationStep++;
        }

        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }
    }
}
