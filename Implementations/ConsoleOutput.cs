using ConcurrentFileReading.Interfaces;

namespace ConcurrentFileReading.Implementations
{
    public class ConsoleOutput : IOutput
    {
        public void WriteLine(string str)
        {
            Console.WriteLine(str);
        }
    }
}
