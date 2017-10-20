using System.Reflection;

namespace csMACnz.SeaOrDew.Tests.TestHandlers
{
    public class Stats
    {
        public static readonly Assembly TestHandlersAssembly = typeof(TestHandlers.SetA.CustomTestCommand).GetTypeInfo().Assembly;

        public const int OtherTestsHandlersCount = 6;
        public const int SetACommandCount = 3;
        public const int SetAQueryCount = 0;
        public const int SetBCommandCount = 2;
        public const int SetBQueryCount = 0;
        public const int SetACount = SetACommandCount + SetAQueryCount;
        public const int SetBCount = SetBCommandCount + SetBQueryCount;
        public const int AssemblyCommandHandlerCount = SetACommandCount + SetBCommandCount;
        public const int AssemblyQueryHandlerCount = SetAQueryCount + SetBQueryCount;
        public const int AssemblyHandlerCount = SetACount + SetBCount;
    }
}