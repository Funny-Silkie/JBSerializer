using System.Runtime.Serialization;
using System.Text.Json;

namespace JBSerializer
{
    /// <summary>
    /// バイナリシリアライズの経路をJSONシリアライズに使用できるシリアライザのクラス
    /// </summary>
    public class BinaricJsonSerializer
    {
        /// <summary>
        /// <see cref="ValueConverter"/>を与えるプロバイダを取得または設定する
        /// </summary>
        public ValueConverterProvider ConverterProvider { get; set; }
        /// <summary>
        /// シリアライズを行う
        /// </summary>
        /// <typeparam name="T">シリアライズする要素の型</typeparam>
        /// <param name="value">シリアライズする要素</param>
        /// <returns>シリアライズ結果のJSON</returns>
        public string Serialize<T>(T value)
        {
            var provider = ConverterProvider ?? new DefaultValueConverterProvider();
            var converter = provider.GetConverter(typeof(T));
            if (converter == null) throw new SerializationException("シリアライズするコンバータが見つかりませんでした");
            var serializedValue = converter.Convert(value, provider);
            var type = serializedValue?.GetType() ?? typeof(object);
            return JsonSerializer.Serialize(serializedValue, type, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
        }
        /// <summary>
        /// デシリアライズを行う
        /// </summary>
        /// <typeparam name="T">デシリアライズする要素の型</typeparam>
        /// <param name="json">デシリアライズするJSON</param>
        /// <returns>デシリアライズされた値</returns>
        public T Deserialize<T>(string json)
        {
            var provider = ConverterProvider ?? new DefaultValueConverterProvider();
            var converter = provider.GetConverter(typeof(T));
            if (converter == null) throw new SerializationException("シリアライズするコンバータが見つかりませんでした");
            var fixType = converter.GetConvertedType(typeof(T));
            var obj = JsonSerializer.Deserialize(json, fixType, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
            return (T)provider.GetConverter(typeof(T)).ConvertBack(obj, provider);
        }
    }
}
