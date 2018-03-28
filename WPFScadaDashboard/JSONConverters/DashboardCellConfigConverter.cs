using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using WPFScadaDashboard.DashboardConfigClasses;

namespace WPFScadaDashboard.JSONConverters
{
    public class DashboardCellConfigConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IDashboardCellConfig);
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
            List<IDashboardCellConfig> fields = new List<IDashboardCellConfig>();
            var jsonArray = JArray.Load(reader);
            foreach (var item in jsonArray)
            {
                var jsonObject = item as JObject;
                var dashboardCellConfig = default(IDashboardCellConfig);
                switch (jsonObject["VizType_"].Value<string>())
                {
                    case LinePlotCellConfig.cellType:
                        dashboardCellConfig = new LinePlotCellConfig();
                        break;
                }
                serializer.Populate(jsonObject.CreateReader(), dashboardCellConfig);
                fields.Add(dashboardCellConfig);
            }            
            return fields;
        }
    }
}