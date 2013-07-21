namespace nJinjaTesting
{
    using System.IO;
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
    }
}
