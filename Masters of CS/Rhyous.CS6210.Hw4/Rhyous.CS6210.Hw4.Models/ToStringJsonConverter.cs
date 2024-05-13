using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Rhyous.CS6210.Hw4.Models
{
    class ToStringJsonConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var conv = TypeDescriptor.GetConverter(objectType);
            if (conv.CanConvertFrom(typeof(string)))
                return conv.ConvertTo(reader.Value, objectType);
            return null;
        }
    }
}
