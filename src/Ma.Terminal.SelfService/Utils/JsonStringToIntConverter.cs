using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ma.Terminal.SelfService.Utils
{
    public class JsonStringToIntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            int value = -1;

            if (reader.TokenType == JsonTokenType.Number)
            {
                value = reader.GetInt32();
            }
            else if ((reader.TokenType == JsonTokenType.String))
            {
                int.TryParse(reader.GetString(), out value);
            }

            return value;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
