using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace JBSerializer
{
    /// <summary>
    /// バイナリシリアライズの経路をJSONシリアライズに使用できるシリアライザのクラス
    /// </summary>
    public class BinaricJsonSerializer
    {
        private readonly JsonSerializerOptions options = new() { WriteIndented = true };
        /// <summary>
        /// ケツカンマを許すかどうかを取得または設定する
        /// </summary>
        public bool AllowTrailingCommas
        {
            get => options.AllowTrailingCommas;
            set => options.AllowTrailingCommas = value;
        }
        /// <summary>
        /// <see cref="ValueConverter"/>を与えるプロバイダを取得または設定する
        /// </summary>
        public ValueConverterProvider ConverterProvider { get; set; }
        /// <summary>
        /// 使用する<see cref="ValueConverter"/>を取得または設定する
        /// </summary>
        public IList<ValueConverter> Converters { get; set; } = new List<ValueConverter>();
        /// <summary>
        /// デフォルトのバッファーの大きさを取得または設定する
        /// </summary>
        /// <exception cref="ArgumentException">設定しようとした値が0以下</exception>
        public int DefaultBufferSize
        {
            get => options.DefaultBufferSize;
            set => options.DefaultBufferSize = value;
        }
        /// <summary>
        /// エンコーダを取得または設定する
        /// </summary>
        public JavaScriptEncoder Encoder
        {
            get => options.Encoder;
            set => options.Encoder = value;
        }
        /// <summary>
        /// JSONのネストの深さを取得または設定する
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">設定しようとした値が0未満</exception>
        public int MaxDepth
        {
            get => options.MaxDepth;
            set => options.MaxDepth = value;
        }
        /// <summary>
        /// プロパティ名をCase-Insensitiveにするかどうかを取得または設定する
        /// </summary>
        public bool PropertyNameCaseInsensitive
        {
            get => options.PropertyNameCaseInsensitive;
            set => options.PropertyNameCaseInsensitive = value;
        }
        /// <summary>
        /// プロパティの命名規約を取得または設定する
        /// </summary>
        public JsonNamingPolicy PropertyNamingPolicy
        {
            get => options.PropertyNamingPolicy;
            set => options.PropertyNamingPolicy = value;
        }
        /// <summary>
        /// デシリアライズ時に見つからなかったメンバーの復元をスキップするかどうかを取得または設定する
        /// </summary>
        public bool IgnoreNotFoundMember { get; set; }
        /// <summary>
        /// インデント付きで出力するかどうかを取得または設定する
        /// </summary>
        public bool WriteIndented
        {
            get => options.WriteIndented;
            set => options.WriteIndented = value;
        }
        /// <summary>
        /// <see cref="BinaricJsonSerializer"/>の新しいインスタンスを生成する
        /// </summary>
        public BinaricJsonSerializer() { }
        /// <summary>
        /// シリアライズを行う
        /// </summary>
        /// <typeparam name="T">シリアライズする要素の型</typeparam>
        /// <param name="value">シリアライズする要素</param>
        /// <exception cref="SerializationException">シリアライズに失敗した</exception>
        /// <returns>シリアライズ結果のJSON</returns>
        public string Serialize<T>(T value) => Serialize(typeof(T), value);
        /// <summary>
        /// シリアライズを行う
        /// </summary>
        /// <param name="type">シリアライズする要素の型</param>
        /// <param name="value">シリアライズする要素</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        /// <exception cref="SerializationException">シリアライズに失敗した</exception>
        /// <returns>シリアライズ結果のJSON</returns>
        public string Serialize(Type type, object value)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            var provider = GetOrDefaultProvider();
            var converter = provider.GetConverter(type);
            if (converter == null) throw new SerializationException("シリアライズするコンバータが見つかりませんでした");
            var serializedValue = converter.Convert(value, this);
            var jsonType = serializedValue?.GetType() ?? typeof(object);
            return JsonSerializer.Serialize(serializedValue, jsonType, options);
        }
        /// <summary>
        /// デシリアライズを行う
        /// </summary>
        /// <typeparam name="T">デシリアライズする要素の型</typeparam>
        /// <param name="json">デシリアライズするJSON</param>
        /// <exception cref="ArgumentNullException"><paramref name="json"/>がnull</exception>
        /// <exception cref="JsonException">Jsonの形式が無効</exception>
        /// <exception cref="SerializationException">デシリアライズに失敗した</exception>
        /// <returns>デシリアライズされた値</returns>
        public T Deserialize<T>(string json) => (T)Deserialize(json, typeof(T));
        /// <summary>
        /// デシリアライズを行う
        /// </summary>
        /// <param name="json">デシリアライズするJSON</param>
        /// <param name="type">デシリアライズする要素の型</param>
        /// <exception cref="ArgumentNullException"><paramref name="json"/>または<paramref name="type"/>がnull</exception>
        /// <exception cref="JsonException">Jsonの形式が無効</exception>
        /// <exception cref="SerializationException">デシリアライズに失敗した</exception>
        /// <returns>デシリアライズされた値</returns>
        public object Deserialize(string json, Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            var provider = GetOrDefaultProvider();
            var converter = provider.GetConverter(type);
            if (converter == null) throw new SerializationException("シリアライズするコンバータが見つかりませんでした");
            var fixType = converter.GetConvertedType(type);
            var obj = JsonSerializer.Deserialize(json, fixType, options);
            return converter.ConvertBack(obj, this);
        }
        /// <summary>
        /// <see cref="ValueConverterProvider"/>を取得する
        /// </summary>
        /// <returns><see cref="ConverterProvider"/>，nullだったら<see cref="DefaultValueConverterProvider"/></returns>
        internal ValueConverterProvider GetOrDefaultProvider() => ConverterProvider ?? new DefaultValueConverterProvider();
        /// <summary>
        /// 型に合った<see cref="ValueConverter"/>を取得する
        /// </summary>
        /// <param name="type">検索する<see cref="ValueConverter"/>の型</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        /// <returns><paramref name="type"/>に対応する<see cref="ValueConverter"/>のインスタンス</returns>
        internal ValueConverter GetConverter(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            if (Converters != null)
                for (int i = 0; i < Converters.Count; i++)
                    if (Converters[i]?.CanConvert(type) ?? false)
                        return Converters[i];
            return GetOrDefaultProvider()?.GetConverter(type);
        }
    }
}
