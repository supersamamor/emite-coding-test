using System.Reflection;
namespace Emite.Common.Utility.Helpers
{
    public static class ConstantHelper
    {
        public static string? GetPropertyNameByValue(Type type, string? value)
        {
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(null)?.ToString() == value)
                {
                    return field.Name;
                }
            }
            return value;
        }
        public static List<string?> GetConstantValues<T>()
        {
            return typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .Select(f => f.GetRawConstantValue()?.ToString())
                .ToList();
        }
    }
}
