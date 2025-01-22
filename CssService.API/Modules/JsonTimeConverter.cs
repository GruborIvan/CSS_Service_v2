using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CssService.API.Modules
{
    public class JsonTimeConverter : JsonConverter<DateTime>
    {
        private readonly string _format;

        public JsonTimeConverter(string format)
        {
            _format = format;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return DateTime.ParseExact(value, _format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }
}
