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
        public static bool HasAttribute<TAttribute>(MethodInfo type) where TAttribute : Attribute
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetCustomAttribute<TAttribute>() != null;
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
            var paramTypes = args.ConvertAll(x => x.type);
            return GetInstanceMethod(value.GetType(), methodName, paramTypes).Invoke(value, args.ConvertAll(x => x.obj));
        }
    }
}
