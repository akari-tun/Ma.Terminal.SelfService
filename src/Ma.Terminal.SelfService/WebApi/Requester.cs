using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.Utils;
using Ma.Terminal.SelfService.WebApi.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ma.Terminal.SelfService.WebApi
{
    public class Requester
    {
        const string MACHINE_DETAIL = "/yktInfo/openCard/machineDetail/{0}";
        const string OPEN_CARD_USER_DETAIL = "/yktInfo/openCard/openCardUserDetail";
        const string GET_OPEN_CARD_APDU = "/yktInfo/openCard/getOpenCardApdu";
        const string APDU_EXE_RESULT = "/yktInfo/openCard/apduExeResult";
        const string FINISH = "/yktInfo/openCard/finish";
        const string SAVE_MACHINE = "/yktInfo/openCard/saveMachine";

        private Machine _machine;
        private JsonSerializerOptions _options;

        public string LastMessage { get; set; }

        public Requester(Machine machine)
        {
            _machine = machine;
            _options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<MachineDetailEntity> GetMachineDetail()
        {
            var respone = await HttpUtility.HttpGetResponseAsync(string.Format($"{_machine.ApiUrl}{MACHINE_DETAIL}", _machine.MachineNo),
                10000,
                Encoding.UTF8);

            if (respone.IsSuccessStatusCode)
            {
                var content = await respone.Content.ReadAsStringAsync();

                Debug.WriteLine($"Response --> {content}");

                var entity = JsonSerializer.Deserialize<ApiRespone<MachineDetailEntity>>(content, _options);
                if (entity != null) return entity.Data;
            }

            return null;
        }

        public async Task<UserDetail> OpenCardUserDetail(string phoneNumber, string pickupCode)
        {
            var respone = await HttpUtility.HttpPostResponseAsync($"{_machine.ApiUrl}{OPEN_CARD_USER_DETAIL}",
                JsonSerializer.Serialize(new { PhoneNumber = phoneNumber, PickupCode = pickupCode, MachineNo = _machine.MachineNo }, _options),
                10000,
                Encoding.UTF8,
                "application/json");

            if (respone.IsSuccessStatusCode)
            {
                var content = await respone.Content.ReadAsStringAsync();

                Debug.WriteLine($"Response --> {content}");

                var entity = JsonSerializer.Deserialize<ApiRespone<UserDetail>>(content, _options);
                if (entity != null)
                {
                    LastMessage = entity.Msg;
                    return entity.Data;
                }
            }
            else
            {
                LastMessage = $"调用用户信息接口失败，HttpCode [{respone.StatusCode}]。";
            }

            return null;
        }

        public async Task<OpenCardApdu> OpenCardApdu(string orderId, string userId, string uid)
        {
            var respone = await HttpUtility.HttpPostResponseAsync($"{_machine.ApiUrl}{GET_OPEN_CARD_APDU}",
                JsonSerializer.Serialize(new { OrderId = orderId, UserId = userId, Uid = uid }, _options),
                10000,
                Encoding.UTF8,
                "application/json");

            if (respone.IsSuccessStatusCode)
            {
                var content = await respone.Content.ReadAsStringAsync();

                Debug.WriteLine($"Response --> {content}");

                var entity = JsonSerializer.Deserialize<ApiRespone<OpenCardApdu>>(content, _options);
                if (entity != null)
                {
                    LastMessage = entity.Msg;
                    return entity.Data;
                }
            }
            else
            {
                LastMessage = $"调用开卡指令获取接口失败，HttpCode [{respone.StatusCode}]。";
                Debug.WriteLine($"Response --> {LastMessage}");
            }

            return null;
        }

        public async Task<ApduExeResult> ApduExeResult(
            string apduIndex,
            string rapdu,
            int result,
            string userId,
            string uid)
        {
            var respone = await HttpUtility.HttpPostResponseAsync($"{_machine.ApiUrl}{APDU_EXE_RESULT}",
                JsonSerializer.Serialize(
                    new 
                    { 
                        ApduIndex = apduIndex,
                        Rapdu = rapdu,
                        Result = result,
                        UserId = userId,
                        Uid = uid
                    }, _options),
                10000,
                Encoding.UTF8,
                "application/json");

            if (respone.IsSuccessStatusCode)
            {
                var content = await respone.Content.ReadAsStringAsync();

                Debug.WriteLine($"Response --> {content}");

                var entity = JsonSerializer.Deserialize<ApiRespone<ApduExeResult>>(content, _options);
                if (entity != null)
                {
                    LastMessage = entity.Msg;
                    return entity.Data;
                }
            }
            else
            {
                LastMessage = $"调用开卡指令后续接口失败，HttpCode [{respone.StatusCode}]。";
                Debug.WriteLine($"Response --> {LastMessage}");
            }

            return null;
        }

        public async Task<bool> Finish(
            string orderId,
            string userId,
            string uid,
            string machineNo)
        {
            var respone = await HttpUtility.HttpPostResponseAsync($"{_machine.ApiUrl}{FINISH}",
                JsonSerializer.Serialize(
                    new
                    {
                        OrderId = orderId,
                        UserId = userId,
                        Uid = uid,
                        MachineNo = machineNo
                    }, _options),
                10000,
                Encoding.UTF8,
                "application/json");

            if (respone.IsSuccessStatusCode)
            {
                var content = await respone.Content.ReadAsStringAsync();

                Debug.WriteLine($"Response --> {content}");

                var entity = JsonSerializer.Deserialize<ApiRespone<string>>(content, _options);
                if (entity != null)
                {
                    LastMessage = entity.Msg;
                    return entity.Code == 0;
                }
            }
            else
            {
                LastMessage = $"调用制卡完成通知接口失败，HttpCode [{respone.StatusCode}]。";
                Debug.WriteLine($"Response --> {LastMessage}");
            }

            return false;
        }

        public async Task<string> SaveMachine(
            string machineNo,
            string cardCount,
            string inkCount,
            string cardRopeCover)
        {
            var respone = await HttpUtility.HttpPostResponseAsync($"{_machine.ApiUrl}{SAVE_MACHINE}",
                JsonSerializer.Serialize(
                    new
                    {
                        MachineNo = machineNo,
                        CardCount = cardCount,
                        InkCount = inkCount,
                        CardRopeCover = cardRopeCover
                    }, _options),
                10000,
                Encoding.UTF8,
                "application/json");

            if (respone.IsSuccessStatusCode)
            {
                var content = await respone.Content.ReadAsStringAsync();

                Debug.WriteLine($"Response --> {content}");

                var entity = JsonSerializer.Deserialize<ApiRespone<string>>(content, _options);
                if (entity != null)
                {
                    LastMessage = entity.Msg;
                    return entity.Data;
                }
            }
            else
            {
                LastMessage = $"调用制卡机信息同步接口失败，HttpCode [{respone.StatusCode}]。";
                Debug.WriteLine($"Response --> {LastMessage}");
            }

            return null;
        }
    }
}
