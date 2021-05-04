# JBSerializer

このライブラリは， `BinaryFormatter` のシリアライズ経路(`ISerialziable`，`IDeserializationCallBack`など)を経由してJson形式にシリアライズをするシリアライザを提供します。

Jsonシリアライズのカスタマイズをする際，あのけったいなJsonのWriterやReaderを扱わなければならないのはとても嫌， `BinaryFormatter` では `ISerializable` のような，ディクショナリ風の実装方法があるという事で，この経路を使いまわしてJsonにしようというのがこのライブラリを作った目的。

## 使い方

`BinaricJsonSerializer` クラスでシリアライズ/デシリアライズを行う。

### サンプル

#### シリアライズ
```cs
using System;
using JBSerializer;

class SampleClass
{
    void Test()
    {
        MyClass value = new MyClass();
        BinaricJsonSerializer serializer = new BinaricJsonSerializer();
        
        // シリアライズ
        string json = serializer.Serialize(value);
        // デシリアライズ
        MyClass obj = serializer.Deserialize<MyClass>(json);
    }
}

[Serializable]
class MyClass
{
    private int value = 3;
    public string Name = "Hoge";
}
```

出力
```json
{
  "IsNull": false,
  "TypeName": "Test.MyClass, Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
  "Fields": {
    "value": {
      "TypeName": "System.Int32, System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
      "Value": 3
    },
    "Name": {
      "TypeName": "System.String, System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
      "Value": "Hoge"
    }
  }
}

```