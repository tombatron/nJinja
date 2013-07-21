namespace nJinja
{
    using System.Web.Mvc;

    public class nJinjaView : IView
    {
        private string _templateName;

        public nJinjaView(string templateName)
        {
            _templateName = templateName;
        }

        public void Render(ViewContext viewContext, System.IO.TextWriter writer)
        {
            var jinjaEnv = Environment.GetInstance();

            // TODO: Need to provide WAAAY better handling for the ViewContext here.
            var result = jinjaEnv.Render(_templateName, viewContext.ViewData.Model);

            writer.Write(result);
        }
    }
}
