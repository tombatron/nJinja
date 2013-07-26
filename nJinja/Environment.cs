namespace nJinja
{
    using System;
    using nJinja.Exceptions;

    public class Environment
    {
        private Jinja2Wrapper _jinja2;
        private dynamic _jinja2_env;

        public Environment(string templateRoot)
        {
            _jinja2 = Jinja2Wrapper.GetInstance();
            _jinja2_env = _jinja2.GetEnvironment(templateRoot, null);
        }

        public string Render(string templateName, dynamic context)
        {
            return _jinja2.RenderTemplate(_jinja2_env, templateName, Utils.ConvertToJinjaContext(context));
        }

        public void AppendFilter(string filterName, Delegate customFilter)
        {
            _jinja2.AddCustomFilterToEnvironment(_jinja2_env, filterName, customFilter);
        }

        #region Static Business

        private static Environment _instance;
        public static Environment GetInstance()
        {
            if (_instance == null)
            {
                throw new EnvironmentException("You must bootstrap the Jinja2 environment first...");
            }

            return _instance;
        }

        public static void Bootstrap(string templatePath)
        {
            if (_instance == null)
            {
                _instance = new Environment(templatePath);
            }
        }

        #endregion
    }
}
