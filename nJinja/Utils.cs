namespace nJinja
{
    using System.Collections.Generic;

    public static class Utils
    {
        public static Dictionary<string, dynamic> ConvertToJinjaContext(dynamic context)
        {
            var result = new Dictionary<string, dynamic>();

            if (context != null)
            {
                foreach (var prop in context.GetType().GetProperties())
                {
                    result.Add(prop.Name, prop.GetValue(context, null));
                }
            }

            return result;
        }
    }
}