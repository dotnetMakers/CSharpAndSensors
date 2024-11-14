using Meadow.Modbus;

namespace ModbusSample
{
    internal class Program
    {
        private static bool _doInputTest;

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello Modbus");

            var client = new ModbusTcpClient("192.168.0.9");
            await client.Connect();

            while (true)
            {
                if (_doInputTest)
                {
                    var value = await client.ReadHoldingRegisters(1, 0, 1);

                    Console.WriteLine(value[0]);

                    await Task.Delay(1000);
                }
                else
                {
                    Console.Write("Enter a rate: ");
                    var val = Console.ReadLine();
                    if (ushort.TryParse(val, out ushort rate))
                    {
                        await client.WriteHoldingRegister(1, 0, rate);
                    }
                }
            }
        }
    }
}
