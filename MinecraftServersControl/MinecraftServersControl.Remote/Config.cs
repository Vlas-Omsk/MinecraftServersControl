using PinkJson2;
using PinkJson2.Serializers;
using System;
using System.Collections.Generic;
using System.IO;

namespace MinecraftServersControl.Remote
{
    public sealed class Config
    {
        public Guid Id { get; private set; }
        public string Url { get; private set; }
        public int ReconnectDelaySeconds { get; private set; }
        public IEnumerable<ServerInfo> Servers { get; private set; }

        private Config()
        {
        }

        public static Config Load(string path)
        {
            using var streamReader = new StreamReader(path);
            var json = Json.Parse(streamReader);

            return json.Deserialize<Config>(new ObjectSerializerOptions()
            {
                IgnoreMissingProperties = false
            });
        }

        public static void Save(string path, Config config)
        {
            File.WriteAllText(path, config.Serialize().ToString());
        }
    }
}
