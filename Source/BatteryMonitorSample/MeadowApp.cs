using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace BatteryMonitorSample
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");


            return base.Initialize();
        }

        public override Task Run()
        {
            Resolver.Log.Info("Run...");

            return base.Run();
        }
    }
}