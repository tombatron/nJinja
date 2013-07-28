namespace nJinja
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using IronPython.Hosting;
    using IronPython.Runtime;
    using Microsoft.Scripting.Hosting;

    /// <summary>
    /// This class serves as the glue between nJinja and the Jinja2 template engine.
    /// </summary>
    public class Jinja2Wrapper
    {

        #region Load Jinja2 Stuff

        private Thread _scopeInitializationThread;
        private static ScriptEngine _engine;
        private static ScriptSource _source;

        private ScriptScope _scope;

        private Jinja2Wrapper()
        {
            InitializeHosting();

            _scope = _engine.CreateScope();

            _scopeInitializationThread = new Thread(
                () =>
                {
                    InitalizeScope();
                });

            _scopeInitializationThread.Start();
        }

        private void InitializeHosting()
        {
            if (_engine == null)
            {
                var engineOptions = new Dictionary<string, object>();

                _engine = Python.CreateEngine(engineOptions);

                var assembly = Assembly.GetExecutingAssembly();
                var embeddedWrapperScript = assembly.GetManifestResourceStream("nJinja.jinja2_wrapper.py");

                _source = _engine.CreateScriptSource(new BasicStreamContentProvider(embeddedWrapperScript), "jinja2_wrapper.py");
            }
        }

        private void InitalizeScope()
        {
            _source.Execute(_scope);
        }

        private ScriptScope getScriptScope()
        {
            _scopeInitializationThread.Join();
            return _scope;
        }

        #endregion

        #region Wrapped Jinja2 Methods

        /// <summary>
        /// Wrapper method that will spin up a Jinja2 environment using the Python Jinja2 library.
        /// </summary>
        /// <param name="templateRoot">Absolute path to Jinja2 style templates.</param>
        /// <param name="options">Currently this parameter is unused.</param>
        /// <returns>Jinja2 Environment object.</returns>
        public dynamic GetEnvironment(string templateRoot, dynamic options)
        {
            var scope = getScriptScope();

            var pyFunc = scope.GetVariable<PythonFunction>("get_environment");
            var getEnvironment = _engine.Operations.ConvertTo<Func<string, dynamic, dynamic>>(pyFunc);

            return getEnvironment(templateRoot, options);
        }

        /// <summary>
        /// Wrapper method that takes an already initialized Jinja2 environment and adds a custom
        /// filter to it. 
        /// </summary>
        /// <param name="environment">Initialed Jinja2 Environment object.</param>
        /// <param name="filterName">Name of the filter that you're adding.</param>
        /// <param name="customFilter">A delegate instance that represents the method that powers your custom filter.</param>
        public void AddCustomFilterToEnvironment(dynamic environment, string filterName, dynamic customFilter)
        {
            var scope = getScriptScope();

            var pyFunc = scope.GetVariable<PythonFunction>("add_custom_filter");
            var addCustomFilterFunction = _engine.Operations.ConvertTo<Action<dynamic, dynamic, dynamic>>(pyFunc);

            addCustomFilterFunction(environment, filterName, customFilter);
        }

        /// <summary>
        /// Wrapper method that takes an already initialized Jinja2 environment and renders an
        /// existing template.
        /// </summary>
        /// <param name="environment">Initialized Jinja2 Environment object.</param>
        /// <param name="templateName">Relative path to the Jinja2 style template that you want to render.</param>
        /// <param name="context">The context that you expect the Jinja2 template engine to apply to your template.</param>
        /// <returns>A fully rendered template.</returns>
        public string RenderTemplate(dynamic environment, string templateName, Dictionary<string, dynamic> context)
        {
            var scope = getScriptScope();

            var pyFunc = scope.GetVariable<PythonFunction>("render_template");
            var renderTemplateFunction = _engine.Operations.ConvertTo<Func<dynamic, string, Dictionary<string, dynamic>, string>>(pyFunc);

            return renderTemplateFunction(environment, templateName, context);
        }

        /// <summary>
        /// Wrapper method that accepts ad-hoc Jinja2 style template source and renders it using the
        /// provided context. 
        /// </summary>
        /// <param name="templateSource">A unicode string that defines the template.</param>
        /// <param name="context">The context that you expect the Jinja2 template engine to apply to your template.</param>
        /// <returns>A fully rendered template.</returns>
        public string RenderTemplateString(string templateSource, Dictionary<string, dynamic> context)
        {
            var scope = getScriptScope();

            var pyFunc = scope.GetVariable<PythonFunction>("render_template_string");
            var renderTemplateStringFunction = _engine.Operations.ConvertTo<Func<string, Dictionary<string, dynamic>, string>>(pyFunc);

            return renderTemplateStringFunction(templateSource, context);
        }

        /// <summary>
        /// Wrapper method that accepts ad-hoc Jinja2 style template source and renders it using an already
        /// boostrapped Jinja2 environment and the provided context. 
        /// </summary>
        /// <param name="environment">Initialized Jinja2 Environment object.</param>
        /// <param name="templateSource">A unicode string that defines the template.</param>
        /// <param name="context">The context that you expect the Jinja2 template engine to apply to your template.</param>
        /// <returns>A fully rendered template.</returns>
        public string RenderTemplateString(dynamic environment, string templateSource, Dictionary<string, dynamic> context)
        {
            // It'd be nice to be able to render an adhoc template using a preconfigured environment...
            throw new NotImplementedException();
        }


        #endregion

        #region Class Factory Thingy

        private static Jinja2Wrapper _instance;
        public static Jinja2Wrapper GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Jinja2Wrapper();
            }

            return _instance;
        }

        #endregion
    }
}