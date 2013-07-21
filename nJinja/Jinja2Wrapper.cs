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

        public dynamic GetEnvironment(string templateRoot, dynamic options)
        {
            var scope = getScriptScope();

            var pyFunc = scope.GetVariable<PythonFunction>("get_environment");
            var getEnvironment = _engine.Operations.ConvertTo<Func<string, dynamic, dynamic>>(pyFunc);

            return getEnvironment(templateRoot, options);
        }

        public string RenderTemplate(dynamic environment, string templateName, Dictionary<string, dynamic> context)
        {
            var scope = getScriptScope();

            var pyFunc = scope.GetVariable<PythonFunction>("render_template");
            var renderTemplateFunction = _engine.Operations.ConvertTo<Func<dynamic, string, Dictionary<string, dynamic>, string>>(pyFunc);

            return renderTemplateFunction(environment, templateName, context);
        }

        public string RenderTemplateString(string templateSource, Dictionary<string, dynamic> context)
        {
            var scope = getScriptScope();

            var pyFunc = scope.GetVariable<PythonFunction>("render_template_string");
            var renderTemplateStringFunction = _engine.Operations.ConvertTo<Func<string, Dictionary<string, dynamic>, string>>(pyFunc);

            return renderTemplateStringFunction(templateSource, context);
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