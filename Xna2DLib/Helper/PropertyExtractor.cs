using System;
using System.Reflection;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Helper
{
    public static class PropertyExtractor
    {
        public static void ExtractAccessors<T>(this object obj, string propertyPath, out GetAccessorDelegate<T> getAccessor, out SetAccessorDelegate<T> setAccessor)
        {
            var properties = propertyPath.Split('.');
            var instance = obj;
            var type = obj.GetType();

            MethodInfo getMethod = null;
            MethodInfo setMethod = null;

            for (var index = 0; index < properties.Length; index++)
            {
                var propertyName = properties[index];
                var property = type.GetProperty(propertyName);
                if (property == null)
                    throw new ArgumentException(string.Format("{0} has no {1}, property", type.FullName, propertyName),
                        "propertyPath");
                if (index != properties.Length - 1 && !property.CanRead)
                    throw new MemberAccessException(string.Format("{0} have no get accessor", propertyName));

                getMethod = property.GetGetMethod(true);
                if (getMethod == null) continue;

                type = getMethod.ReturnType;

                if (index != properties.Length - 1)
                {
                    if (type.IsValueType)
                        throw new ArgumentException(
                            string.Format("{0} is type of {1} which is Value Type", propertyName, type.FullName),
                            "propertyPath");

                    instance = getMethod.Invoke(instance, null);
                }
                else
                {
                    setMethod = property.GetSetMethod(true);
                }
            }

            if (getMethod == null)
                getAccessor = null;
            else
                getAccessor = (GetAccessorDelegate<T>)Delegate.CreateDelegate(typeof(GetAccessorDelegate<T>), instance, getMethod);

            if (setMethod == null)
                setAccessor = null;
            else
                setAccessor = (SetAccessorDelegate<T>)Delegate.CreateDelegate(typeof(SetAccessorDelegate<T>), instance, setMethod);
        }

        public static Type GetPropertyType(this object obj, string propertyPath)
        {
            var properties = propertyPath.Split('.');
            var type = obj.GetType();

            foreach (var propertyName in properties)
            {
                var property = type.GetProperty(propertyName);
                if (property == null)
                    throw new ArgumentException(string.Format("{0} has no {1}, property", type.FullName, propertyName),
                        "propertyPath");

                var getMethod = property.GetGetMethod(true);
                if (getMethod == null)
                    throw new MemberAccessException(string.Format("{0} have no get accessor", propertyName));

                type = getMethod.ReturnType;
            }

            return type;
        }
    }
}
