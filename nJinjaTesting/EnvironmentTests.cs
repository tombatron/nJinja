namespace nJinjaTesting
{
    using System;
    using System.IO;
    using nJinja.Exceptions;
    using NUnit.Framework;

    [TestFixture]
    public class EnvironmentTests
    {
        nJinja.Environment _env;

        [TestFixtureSetUp]
        public void Setup()
        {
            var templatePath = Path.Combine(System.Environment.CurrentDirectory, "Templates");
            _env = new nJinja.Environment(templatePath);
        }

        /// <summary>
        /// The template used for the test here is a simple template that
        /// inherits from a base template. 
        /// </summary>
        [Test]
        public void CanRenderATestTemplate()
        {
            var result = _env.Render("testChildPage.html", new { });

            Assert.That(result, Contains.Substring("Base Template"));
            Assert.That(result, Contains.Substring("Child Template"));
        }

        /// <summary>
        /// Add a simple custom filter to the Environment and then use it.
        /// 
        /// The kicker here is that we are using C# to construct custom
        /// Jinja2 filters...
        /// </summary>
        [Test]
        public void CanAddCustomFilter()
        {
            Func<string, string> redactedFilter = (x) =>
            {
                return new string('*', x.Length);
            };

            _env.AppendFilter("redacted", redactedFilter);

            var context = new { testPayLoad = "Hello World" };

            var result = _env.Render("basicCustomFilter.html", context);

            Assert.That(result, Contains.Substring("**********"));
        }

        /// <summary>
        /// In the event that someone tries to get an instance of 'Environment'
        /// before the environment has been bootstrapped, we'll throw an
        /// appropriate exception.
        /// </summary>
        [Test]
        public void ExceptionIfEnvironmentNotBootstrappedBeforeGettingInstance()
        {
            Assert.Throws<EnvironmentException>(() =>
            {
                nJinja.Environment.GetInstance();
            });
        }
    }
}
