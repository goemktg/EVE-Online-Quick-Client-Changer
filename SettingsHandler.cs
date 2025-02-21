using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EVE_Online_Quick_Client_Changer
{
    public class SettingsHandler
    {
        private static readonly string settingsPath = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");

        public static List<EveClientData> LoadSettings()
        {
            if (File.Exists(settingsPath))
            {
                var settingsJson = File.ReadAllText(settingsPath);

                // TODO: add error windows for invalid json
                return JsonSerializer.Deserialize<List<EveClientData>>(settingsJson) ?? [];
            }

            return [];
        }

        public static void SaveSettings(List<EveClientData> settings)
        {
            var settingsJson = JsonSerializer.Serialize(settings);
            File.WriteAllText(settingsPath, settingsJson);
        }
    }
}
