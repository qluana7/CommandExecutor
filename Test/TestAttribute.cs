using CommandExecutor.Attributes;
using CommandExecutor.Structures;

namespace Test
{
    public class TestAttribute : CommandCheckAttribute
    {
        public override bool Check(Command cmd)
        {
            return true;
        }
    }
}