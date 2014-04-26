using NUnit.Framework;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace DeadRinger.Test
{
    public class OptionsTest
    {
        OptionAttribute GetOptionAttribute(string propertyName)
        {
            return typeof(Options).GetProperty(propertyName).GetCustomAttributes(false).First() as OptionAttribute;
        }

        [Test]
        public void ShouldConfigurePathOption()
        {
            var option = GetOptionAttribute("Path");

            Assert.That(option.ShortName, Is.EqualTo('p'));
            Assert.That(option.LongName, Is.EqualTo("path"));
            Assert.That(option.Required, Is.True);
            Assert.That(option.HelpText, Is.EqualTo("Path of application to run."));
        }

        [Test]
        public void ShouldConfigureArgumentsOption()
        {
            var option = GetOptionAttribute("Arguments");

            Assert.That(option.ShortName, Is.EqualTo('a'));
            Assert.That(option.LongName, Is.EqualTo("arguments"));
            Assert.That(option.HelpText, Is.EqualTo("Optional arguments for the application."));
        }

        [Test]
        public void ShouldConfigureDelayOption()
        {
            var option = GetOptionAttribute("Delay");

            Assert.That(option.ShortName, Is.EqualTo('d'));
            Assert.That(option.LongName, Is.EqualTo("delay"));
            Assert.That(option.DefaultValue, Is.EqualTo(0));
            Assert.That(option.HelpText, Is.EqualTo("Maximum amount of time (milliseconds) to delay application's execution."));
        }

        [Test]
        public void ShouldProvideUsage()
        {
            var subject = new Options();

            var usage = subject.GetUsage();

            Assert.That(usage.Length, Is.GreaterThan(0));
            Assert.That(usage, Is.StringContaining("path"));
            Assert.That(usage, Is.StringContaining("arguments"));
            Assert.That(usage, Is.StringContaining("delay"));
        }
    }
}
