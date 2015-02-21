using NUnit.Framework;

namespace DeadRinger.Test
{
    class ProgramTest
    {
        [Test]
        public void ShouldRunSuccessfully()
        {
            Program.Main(string.Empty);
        }
    }
}
