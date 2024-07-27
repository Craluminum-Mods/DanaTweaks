using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace DanaTweaks;

/// <summary>
/// Wrote by chat gpt
/// </summary>
public class StringArrayEnumConverter<TEnum> : JsonConverter where TEnum : struct, Enum
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        if (value is not TEnum[] enumArray)
        {
            throw new JsonSerializationException($"Invalid type. Expected {typeof(TEnum[])}.");
        }

        string[] stringValues = enumArray.Select(enumValue => enumValue.ToString()).ToArray();
        serializer.Serialize(writer, stringValues);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        if (objectType != typeof(TEnum[]))
        {
            throw new JsonSerializationException($"Invalid object type: {objectType}. Expected {typeof(TEnum[])}.");
        }

        string[] stringValues = serializer.Deserialize<string[]>(reader);
        if (stringValues == null)
        {
            return null;
        }

        List<TEnum> enumValues = new List<TEnum>();
        foreach (string stringValue in stringValues)
        {
            if (Enum.TryParse(stringValue, out TEnum enumValue))
            {
                enumValues.Add(enumValue);
            }
            else
            {
                throw new JsonSerializationException($"Invalid enum value: {stringValue}");
            }
        }

        return enumValues.ToArray();
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(TEnum[]);
    }
}
