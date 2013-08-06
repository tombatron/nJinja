namespace nJinja
{
    using System.Collections.Generic;
    using nJinja.Exceptions;

    public class Environment
    {
        private Jinja2Wrapper _jinja2;
        private dynamic _jinja2_env;

        /// <summary>
        /// Leaving this constructor public in order to give consumers the option of managing
        /// the instance themselves. The recommended way to spin up an instance of 'Environment'
        /// is to invoke the Bootrap method and then the GetInstance method.
        /// </summary>
        /// <param name="templateRoot">The absolute path to your Jinja2 style templates.</param>
        public Environment(string templateRoot)
        {
            _jinja2 = Jinja2Wrapper.GetInstance();
            _jinja2_env = _jinja2.GetEnvironment(templateRoot, null);
        }

        /// <summary>
        /// Render a specified template using a given context.
        /// </summary>
        /// <param name="templateName">Relative path to the intended template. If the template exists on the template root, 
        /// you would simply provide the name of the template (include the file extension).
        /// </param>
        /// <param name="context">An anonymous type containing the template context.</param>
        /// <returns>Unicode string containing the results of the template rendering.</returns>
        public string Render(string templateName, dynamic context)
        {
            return _jinja2.RenderTemplate(_jinja2_env, templateName, Utils.ConvertToJinjaContext(context));
        }

        /// <summary>
        /// Add a custom template filter to the Jinja2 environment.
        /// </summary>
        /// <param name="filterName">The name of custom filter.</param>
        /// <param name="customFilter">A delegate that defines the logic of the custom filter.</param>
        public void AppendFilter(string filterName, dynamic customFilter)
        {
            _jinja2.AddCustomFilterToEnvironment(_jinja2_env, filterName, customFilter);
        }

        #region Static Business

        private static Environment _instance;

        /// <summary>
        /// This method allows access to the singleton instance of the nJinja Environment.
        /// </summary>
        /// <returns>Existing instance of the nJinja Environment.</returns>
        public static Environment GetInstance()
        {
            if (_instance == null)
            {
                throw new EnvironmentException("You must bootstrap the Jinja2 environment first...");
            }

            return _instance;
        }

        /// <summary>
        /// This method allows for destroying the current singleton instance of the nJinja Environment. 
        /// 
        /// If the instance is already null, then this method really doesn't have an affect now does it?
        /// </summary>
        public static void DestroyInstance()
        {
            _instance = null;
        }

        /// <summary>
        /// Spin up a singleton instance of the nJinja Environment.
        /// </summary>
        /// <param name="templatePath">Absolute path to the folder containing Jinja2 style templates.</param>
        public static void Bootstrap(string templatePath)
        {
            if (_instance == null)
            {
                _instance = new Environment(templatePath);
            }
        }

        /// <summary>
        /// Spin up a singleton instance of the nJinja Environment as provide some
        /// custom template filters. 
        /// </summary>
        /// <param name="templatePath">Absolute path to the folder containing Jinja2 style templates.</param>
        /// <param name="customFilters">A list of custom template filters.</param>
        public static void Bootstrap(string templatePath, IDictionary<string, dynamic> customFilters)
        {
            Bootstrap(templatePath);

            foreach (var cf in customFilters)
            {
                _instance.AppendFilter(cf.Key, cf.Value);
            }
        }

        #endregion
    }
}
