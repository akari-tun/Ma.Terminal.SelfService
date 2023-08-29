using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.WebApi;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ma.Terminal.SelfService.Model
{
    public class ItemsConfig
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

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

            Task.Run(async () =>
            {
                try
                {
                    var machine = Ioc.Default.GetRequiredService<Machine>();
                    var requester = Ioc.Default.GetRequiredService<Requester>();

                    machine.Detail.CardCount = Card.ToString();
                    machine.Detail.InkCount = Ink.ToString();
                    machine.Detail.CardRopeCover = Lanyard.ToString();

                    await requester.SaveMachine(machine.MachineNo,
                                                machine.Detail.CardCount,
                                                machine.Detail.InkCount,
                                                machine.Detail.CardRopeCover);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    _logger.Error(ex.StackTrace);
                }

            });
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
