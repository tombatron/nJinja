namespace nJinja
{
    public class Template
    {
        private string _templateFragment;
        private Jinja2Wrapper _jw;

        public Template(string templateFragment)
        {
            _templateFragment = templateFragment;
            _jw = Jinja2Wrapper.GetInstance();
        }

        public string Render(dynamic context)
        {
            var jinjaContext = Utils.ConvertToJinjaContext(context);

            return _jw.RenderTemplateString(_templateFragment, jinjaContext);
        }
    }
}
