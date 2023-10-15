using System.Net;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using fnxpe002.Model;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
namespace fnxpe002
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        public Function1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _configuration = configuration;
        }

        [Function("fnPedidoxpe001")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var cCon = _configuration.GetConnectionString("ServiceBusConnection");

            var body = await req.ReadAsStringAsync() ;

            var response = req.CreateResponse(HttpStatusCode.OK);

            var pedido = JsonConvert.DeserializeObject<Pedido>(body);

            await SendMessageAsync(pedido, cCon);

            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Pedido Recebido.");

            return response;
        }

        public async Task SendMessageAsync(Pedido pedido, string cCon)
        {

#if DEBUG
            cCon = "Endpoint=sb://svxpe001.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=1v+QMCF2qkyPJc4UeGXzh4GhuXX+0DHjV+ASbEywuLQ=";
            await using var client = new ServiceBusClient(cCon);
#else
            await using var client = new ServiceBusClient(cCon);
#endif

            ServiceBusSender sender = client.CreateSender("outputqueue");

            ServiceBusMessage message = new ServiceBusMessage(JsonConvert.SerializeObject(pedido));

            await sender.SendMessageAsync(message);

            Console.WriteLine($"Sent message: {pedido}");
        }
    }
}
