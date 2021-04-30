using System;
using System.Linq;
using System.Reflection;

namespace JBSerializer
{
    internal static class ReflectionHelper
    {
        /// <summary>
        /// 指定した型の持っている<see cref="Attribute"/>を列挙する
        /// </summary>
        /// <param name="type">属性を取得する型</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        /// <returns><paramref name="type"/>の持っている全属性</returns>
        public static Attribute[] GetAttributes(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetCustomAttributes().ToArray();
        }
        public static bool HasAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetCustomAttribute<TAttribute>() != null;
        }
        public static bool HasAttribute<TAttribute>(MethodInfo method) where TAttribute : Attribute
        {
            if (method == null) throw new ArgumentNullException(nameof(method), "引数がnullです");
            return method.GetCustomAttribute<TAttribute>() != null;
        }
        public static bool HasAttribute<TAttribute>(FieldInfo field) where TAttribute : Attribute
        {
            if (field == null) throw new ArgumentNullException(nameof(field), "引数がnullです");
            return field.GetCustomAttribute<TAttribute>() != null;
        }
        public static FieldInfo GetInstanceField(Type type, string fieldName)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }
        public static FieldInfo[] GetInstanceFields(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }
        public static MethodInfo[] GetInstanceMethods(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }
        public static MethodInfo GetInstanceMethod(Type type, string name, params Type[] paramTypes)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            if (paramTypes == null) throw new ArgumentNullException(nameof(paramTypes), "引数がnullです");
            return type.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, paramTypes, null);
        }
        public static ConstructorInfo GetEmptyConstructor(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
        }
        public static ConstructorInfo GetConstructor(Type type, params Type[] paramTypes)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, paramTypes, null);
        }
        public static bool IsInherited<TParent>(Type type) where TParent : class
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            var checkType = typeof(TParent);
            if (type.IsInterface)
            {
                var interfaces = type.GetInterfaces();
                for (int i = 0; i < interfaces.Length; i++)
                    if (interfaces[i] == checkType)
                        return true;
                return false;
            }
            else return type.BaseType == checkType;
        }
        public static object InvokeInstanceMethod(object value, string methodName, params (object obj, Type type)[] args)
        {
            if (value == null) throw new ArgumentNullException(nameof(value), "引数がnullです");
            var paramTypes = Array.ConvertAll(args, x => x.type);
            return GetInstanceMethod(value.GetType(), methodName, paramTypes)?.Invoke(value, Array.ConvertAll(args, x => x.obj));
        }
    }
}
