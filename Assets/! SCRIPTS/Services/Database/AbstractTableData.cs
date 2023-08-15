using System.Reflection;

namespace Services.Database
{
    public abstract class AbstractTableData
    {
        public T Copy<T>() where T : AbstractTableData, new()
        {
            var type = typeof(T);
            var result = new T();
            var flags = BindingFlags.DeclaredOnly
                | BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.NonPublic;

            var fields = type.GetFields(flags);
            foreach (var field in fields)
            {
                var value = field.GetValue(this);
                field.SetValue(result, value);
            }

            return result;
        }
    }
}
