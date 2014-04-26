using log4net;
using Moq;
using NUnit.Framework;
using System;

namespace DeadRinger.Test
{
    class ApplicationRunnerTest
    {
        [Test]
        public void ShouldInitializeOptions()
        {
            var subject = new ApplicationRunner();

            subject.Execute(new string[1] {"bad args"});

            Assert.IsNull(subject.Options.Path);
            Assert.IsNull(subject.Options.Arguments);
            Assert.That(subject.Options.Delay, Is.EqualTo(0));
        }

        [Test]
        public void ShouldParseArguments()
        {
            var subject = new ApplicationRunner();

            subject.Execute(new string[3] { "-pfoo.exe", "-d10", "-ablah" });

            Assert.That(subject.Options.Path, Is.EqualTo("foo.exe"));
            Assert.That(subject.Options.Arguments, Is.EqualTo("blah"));
            Assert.That(subject.Options.Delay, Is.EqualTo(10));
        }

        [Test]
        public void ShouldLogErrorIfApplicationDoesNotExist()
        {
            var subject = new ApplicationRunner();
            var mockLog = new Mock<ILog>();
            subject.Log = mockLog.Object;
            var path = "/foo/bar.exe";
            mockLog.Setup(x => x.Error(It.IsAny<string>()));
            var message = String.Format("Cannot run {0}. It either does not exist or is inaccessible. Exiting...", path); ;

            subject.Execute(new string[1] { "-p" + path });

            mockLog.Verify(x => x.Error(It.Is<string>(y => y == message)), Times.Once);
        }

        [Test]
        public void ShouldLaunchProcessAndLogOutput()
        {
            var subject = new ApplicationRunner();
            var mockLog = new Mock<ILog>();
            subject.Log = mockLog.Object;
            mockLog.Setup(x => x.Info(It.IsAny<string>()));
            var path = @"C:\Windows\System32\cscript.exe";
            var arguments = @"foo.vbs";            
            var expected = "Windows Script Host";

            subject.Execute(new string[2] { "-p" + path, "-a" + arguments });

            mockLog.Verify(x => x.Info(It.Is<string>(y => y.Contains(expected))), Times.Once);
        }
    }
}
