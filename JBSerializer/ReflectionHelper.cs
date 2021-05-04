using System;
using System.Linq;
using System.Reflection;

namespace JBSerializer
{
    /// <summary>
    /// リフレクション処理を行うクラス
    /// </summary>
    internal static class ReflectionHelper
    {
        /// <summary>
        /// シリアライズに使用する型名を取得する
        /// </summary>
        /// <param name="type">使用する型</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        /// <returns><paramref name="type"/>を表す文字列</returns>
        public static string GetTypeName(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return $"{type.FullName}, {type.Assembly.FullName}";
        }
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
        /// <summary>
        /// 指定した属性を持っているかどうかを取得する
        /// </summary>
        /// <typeparam name="TAttribute">検索する属性</typeparam>
        /// <param name="type">属性を検索する型</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        /// <returns><paramref name="type"/>が<typeparamref name="TAttribute"/>を持っていたらtrue，それ以外でfalse</returns>
        public static bool HasAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetCustomAttribute<TAttribute>(true) != null;
        }
        /// <summary>
        /// 指定した属性を持っているかどうかを取得する
        /// </summary>
        /// <typeparam name="TAttribute">検索する属性</typeparam>
        /// <param name="method">属性を検索するメソッド</param>
        /// <exception cref="ArgumentNullException"><paramref name="method"/>がnull</exception>
        /// <returns><paramref name="method"/>が<typeparamref name="TAttribute"/>を持っていたらtrue，それ以外でfalse</returns>
        public static bool HasAttribute<TAttribute>(MethodInfo method) where TAttribute : Attribute
        {
            if (method == null) throw new ArgumentNullException(nameof(method), "引数がnullです");
            return method.GetCustomAttribute<TAttribute>() != null;
        }
        /// <summary>
        /// 指定した属性を持っているかどうかを取得する
        /// </summary>
        /// <typeparam name="TAttribute">検索するフィールド</typeparam>
        /// <param name="field">属性を検索するメソッド</param>
        /// <exception cref="ArgumentNullException"><paramref name="field"/>がnull</exception>
        /// <returns><paramref name="field"/>が<typeparamref name="TAttribute"/>を持っていたらtrue，それ以外でfalse</returns>
        public static bool HasAttribute<TAttribute>(FieldInfo field) where TAttribute : Attribute
        {
            if (field == null) throw new ArgumentNullException(nameof(field), "引数がnullです");
            return field.GetCustomAttribute<TAttribute>() != null;
        }
        /// <summary>
        /// 指定した名前を持つフィールドを取得する
        /// </summary>
        /// <param name="type">使用する型</param>
        /// <param name="fieldName">検索するフィールドの名前</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>または<paramref name="fieldName"/>がnull</exception>
        /// <returns><paramref name="type"/>の持つフィールド</returns>
        public static FieldInfo GetInstanceField(Type type, string fieldName)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }
        /// <summary>
        /// 全てのフィールドを取得する
        /// </summary>
        /// <param name="type">使用する型</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        /// <returns><paramref name="type"/>の持つ全てのフィールド</returns>
        public static FieldInfo[] GetInstanceFields(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }
        /// <summary>
        /// 全てのメソッドを取得する
        /// </summary>
        /// <param name="type">使用する型</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        /// <returns><paramref name="type"/>の持つ全てのメソッド</returns>
        public static MethodInfo[] GetInstanceMethods(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }
        /// <summary>
        /// 指定した名前を持つメソッドを取得する
        /// </summary>
        /// <param name="type">使用する型</param>
        /// <param name="name">検索するメソッドの名前</param>
        /// <param name="paramTypes">使用する引数の型一覧</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>または<paramref name="name"/>，<paramref name="type"/>がnull</exception>
        /// <returns><paramref name="type"/>の持つメソッド</returns>
        public static MethodInfo GetInstanceMethod(Type type, string name, params Type[] paramTypes)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            if (paramTypes == null) throw new ArgumentNullException(nameof(paramTypes), "引数がnullです");
            return type.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, paramTypes, null);
        }
        /// <summary>
        /// 無引数のコンストラクタを取得する
        /// </summary>
        /// <param name="type">使用する型</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        /// <returns><paramref name="type"/>の持つ無引数コンストラクタ</returns>
        public static ConstructorInfo GetEmptyConstructor(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
        }
        /// <summary>
        /// 指定した引数を持つコンストラクタを取得する
        /// </summary>
        /// <param name="type">使用する型</param>
        /// <param name="paramTypes">検索するコンストラクタの引数一覧</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>または<paramref name="type"/>，<paramref name="type"/>内の要素がnull</exception>
        /// <returns><paramref name="type"/>の持つコンストラクタ</returns>
        public static ConstructorInfo GetConstructor(Type type, params Type[] paramTypes)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, paramTypes, null);
        }
        /// <summary>
        /// 指定した型を継承/実装しているかどうかを取得する
        /// </summary>
        /// <typeparam name="TParent">継承/実装を検索する型</typeparam>
        /// <param name="type">使用する型</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        /// <returns><paramref name="type"/>が<typeparamref name="TParent"/>を継承/実装していたらtrue，それ以外でnull</returns>
        public static bool IsInherited<TParent>(Type type) where TParent : class
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            var checkType = typeof(TParent);
            if (checkType.IsInterface)
            {
                var interfaces = type.GetInterfaces();
                for (int i = 0; i < interfaces.Length; i++)
                    if (interfaces[i] == checkType)
                        return true;
                return false;
            }
            else return type.BaseType == checkType;
        }
    }
}
