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

            subject.Execute("bad args");

            Assert.IsNull(subject.Options.Path);
            Assert.IsNull(subject.Options.Arguments);
            Assert.That(subject.Options.Delay, Is.EqualTo(0));
        }

        [Test]
        public void ShouldParseArguments()
        {
            var subject = new ApplicationRunner();

            subject.Execute("-pfoo.exe", "-d10", "-ablah");

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

            subject.Execute("-p" + path);

            mockLog.Verify(x => x.Error(It.Is<string>(y => y == message)), Times.Once);
        }

        [Test]
        public void ShouldLaunchProcessAndLogOutput()
        {
            var isRunningOnMono = Type.GetType("Mono.Runtime") != null;
            var subject = new ApplicationRunner();
            var mockLog = new Mock<ILog>();
            subject.Log = mockLog.Object;
            mockLog.Setup(x => x.Info(It.IsAny<string>()));
            var path = isRunningOnMono ? "/bin/ls" : @"C:\Windows\System32\cscript.exe";
            var arguments = isRunningOnMono ? "-l" : @"foo.vbs";            
            var expected = isRunningOnMono ? "DeadRinger.Test.dll" : "Windows Script Host";

            subject.Execute("-p" + path, "-a" + arguments);

            mockLog.Verify(x => x.Info(It.Is<string>(y => y.Contains(expected))), Times.Once);
        }
    }
}
