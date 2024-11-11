using Meadow;
using Meadow.Foundation.Sensors.Motion;
using Meadow.Foundation.Spatial;
using Meadow.Units;
using YoshiPi;

namespace BNO055Sample;

internal sealed class MyApplication : YoshiPiApp
{
    private Bno055 _sensor;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize");

        Resolver.Services.Add(
            new DisplayService(Hardware.Display, Hardware.Touchscreen)
            );

        _sensor = new Bno055(Hardware.Qwiic);
        _sensor.Updated += OnSensorUpdated;
        _sensor.StartUpdating(TimeSpan.FromSeconds(1));

        return base.Initialize();
    }

    private void OnSensorUpdated(object? sender, IChangeResult<(
        Acceleration3D? Acceleration3D,
        AngularVelocity3D? AngularVelocity3D,
        MagneticField3D? MagneticField3D,
        Quaternion? QuaternionOrientation,
        Acceleration3D? LinearAcceleration,
        Acceleration3D? GravityVector,
        EulerAngles? EulerOrientation,
        Temperature? Temperature)> e)
    {
        // ShowTilt(e.New.GravityVector.Value);
        ShowHeading(e.New.MagneticField3D.Value);
    }

    private void ShowTilt(Acceleration3D gravityVector)
    {
        var text = $"({gravityVector.X},{gravityVector.Y},{gravityVector.Z})";
        var display = Resolver.Services.Get<DisplayService>();
        display.SetLabelText(text);
    }

    private void ShowHeading(MagneticField3D magneticField3D)
    {
        var text = GetHeading(magneticField3D).Compass16PointCardinalName.ToString();
        var display = Resolver.Services.Get<DisplayService>();
        display.SetLabelText(text);
    }

    private Azimuth GetHeading(MagneticField3D magneticField3D)
    {
        // Calculate the heading from the magnetometer data
        var heading = Math.Atan2(magneticField3D.Y.Gauss, magneticField3D.X.Gauss) * (180 / Math.PI);
        if (heading < 0)
        {
            heading += 360;
        }
        return new Azimuth(heading);
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
