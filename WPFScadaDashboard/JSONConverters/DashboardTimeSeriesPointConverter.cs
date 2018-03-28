using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using WPFScadaDashboard.DashboardDataPointClasses;

namespace WPFScadaDashboard.JSONConverters
{
    public class DashboardTimeSeriesPointConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IDashboardTimeSeriesPoint);
        }
        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Use default serialization.");
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            List<IDashboardTimeSeriesPoint> fields = new List<IDashboardTimeSeriesPoint>();
            var jsonArray = JArray.Load(reader);
            foreach (var item in jsonArray)
            {
                var jsonObject = item as JObject;
                var dashboardTimeSeriesPoint = default(IDashboardTimeSeriesPoint);
                switch (jsonObject["TimeSeriesType_"].Value<string>())
                {
                    case DashboardScadaTimeSeriesPoint.timeSeriesType:
                        dashboardTimeSeriesPoint = new DashboardScadaTimeSeriesPoint();
                        break;
                }
                serializer.Populate(jsonObject.CreateReader(), dashboardTimeSeriesPoint);
                fields.Add(dashboardTimeSeriesPoint);
            }
            return fields;
        }
    }
}