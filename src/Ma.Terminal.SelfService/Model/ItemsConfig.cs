using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Ma.Terminal.SelfService.Model
{
    public class ItemsConfig
    {
        public int Card { get; set; }
        public int Lanyard { get; set; }
        public int Ink { get; set; }

        public static ItemsConfig Read()
        {
            return JsonSerializer.Deserialize<ItemsConfig>(File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}config.json"));
        }

        public void Save()
        {
            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}config.json", JsonSerializer.Serialize(this));
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
