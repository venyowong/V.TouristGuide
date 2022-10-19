using System.Reflection;

namespace V.TouristGuide.Server.Helpers
{
    public static class ReflectionHelper
    {
        private static readonly Dictionary<string, PropertyInfo> _properties = new Dictionary<string, PropertyInfo>();

        public static object GetValue(object obj, string property)
        {
            var type = obj.GetType();
            var key = type.FullName + property;
            PropertyInfo pro = null;
            if (!_properties.ContainsKey(key))
            {
                pro = type.GetProperties().FirstOrDefault(p => p.Name.ToLower() == property.ToLower());
                _properties.Add(key, pro);
            }
            else
            {
                pro = _properties[key];
            }
            if (pro == null)
            {
                return null;
            }

            return pro.GetValue(obj);
        }
    }
}
