namespace nJinjaTesting
{
    using nJinja;
    using NUnit.Framework;

    [TestFixture]
    public class TemplateTests
    {
        [Test]
        public void CanRenderAdhocTemplateString()
        {
            var template = new Template("Hello {{ recipient }}!");

            var result = template.Render(new { recipient = "world" });

            Assert.That(result, Is.EqualTo("Hello world!"));
        }
    }
}
