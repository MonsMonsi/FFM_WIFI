using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FFM_WIFI.Models.DataJson
{
    public class Config
    {
        public string ConnectionString { get; set; }
        public string ApiKeyName { get; set; }
        public string ApiKeyValue { get; set; }

        public static Config GetConfig()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Config.json");

            return JsonSerializer.Deserialize<Config>(File.ReadAllText(path));
        }
    }
}
