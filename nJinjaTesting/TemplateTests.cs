namespace nJinjaTesting
{
    using nJinja;
    using NUnit.Framework;

    [TestFixture]
    public class TemplateTests
    {
        /// <summary>
        /// Verify that it's possible to spin up a Template object, pass it a
        /// template in the form of a string and the template context in the 
        /// form of an anonymous type. 
        /// </summary>
        [Test]
        public void CanRenderAdhocTemplateString()
        {
            var template = new Template("Hello {{ recipient }}!");

            var result = template.Render(new { recipient = "world" });

            Assert.That(result, Is.EqualTo("Hello world!"));
        }
    }
}
