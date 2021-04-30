using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JBSerializer
{
    /// <summary>
    /// 
    /// </summary>
    public class BinaricJsonSerializer
    {
        /// <summary>
        /// <see cref="ValueConverter"/>を与えるプロバイダを取得または設定する
        /// </summary>
        public ValueConverterProvider ConverterProvider { get; set; }
        public string Serialize<T>(T value)
        {
            var provider = ConverterProvider ?? new DefaultValueConverterProvider();
            var serializedValue = provider.GetConverter(typeof(T)).Convert(value, provider);
            var type = serializedValue?.GetType() ?? typeof(object);
            return JsonSerializer.Serialize(serializedValue, type);
        }
        public T Deserialize<T>(string json)
        {
            var provider = ConverterProvider ?? new DefaultValueConverterProvider();
            var obj = JsonSerializer.Deserialize(json, provider.GetConverter(typeof(T)).GetConvertedType(typeof(T)));
            return (T)provider.GetConverter(typeof(T)).ConvertBack(obj, provider);
        }
    }
}
