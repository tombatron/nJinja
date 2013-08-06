namespace nJinjaTesting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using nJinja.Exceptions;
    using NUnit.Framework;

    [TestFixture]
    public class EnvironmentTests
    {
        nJinja.Environment _env;
        string _templatePath;

        [TestFixtureSetUp]
        public void Setup()
        {
            _templatePath = Path.Combine(System.Environment.CurrentDirectory, "Templates");
            _env = new nJinja.Environment(_templatePath);
        }

        [TearDown]
        public void TestTearDown()
        {
            nJinja.Environment.DestroyInstance();
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

        /// <summary>
        /// Can we bootstrap the nJinja environment with just a template path?
        /// </summary>
        [Test]
        public void CanBootstrapEnvironment()
        {
            nJinja.Environment.Bootstrap(_templatePath);

            var environment = nJinja.Environment.GetInstance();

            Assert.That(environment, Is.Not.Null);
        }

        /// <summary>
        /// Can we bootstrap the nJinja environment with a template path and 
        /// custom filters?
        /// </summary>
        [Test]
        public void CanBootstrapEnvironmentWithCustomTemplateFilters()
        {
            Func<string, string> redactedFilter = (x) =>
            {
                return new string('*', x.Length);
            };

            var customFilters = new Dictionary<string, dynamic>();
            customFilters.Add("redacted", redactedFilter);

            nJinja.Environment.Bootstrap(_templatePath, customFilters);

            var environment = nJinja.Environment.GetInstance();

            var context = new { testPayLoad = "Hello World" };

            var result = environment.Render("basicCustomFilter.html", context);

            // Ensure that the custom filter was applied...
            Assert.That(result, Contains.Substring("**********"));
        }

        /// <summary>
        /// Can we render an adhoc template using the preconfigured nJinja
        /// environment?
        /// </summary>
        [Test]
        public void CanRenderAdhocTemplateUsingEnvironment()
        {
            var template = "Hello {{ recipient}}!";

            var result = _env.RenderAdhocTemplate(template, new { recipient = "world" });

            Assert.That(result, Is.EqualTo("Hello World!"));
        }
    }
}
