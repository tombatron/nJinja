namespace nJinja
{

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

        private static Environment _instance;
        public static Environment GetInstance()
        {
            // TODO: Throw exception here if the environment hasn't been bootstrapped.

            return _instance;
        }

        public static void Bootstrap(string templatePath)
        {
            if (_instance == null)
            {
                _instance = new Environment(templatePath);
            }
        }
    }
}
