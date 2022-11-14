using Azure.Messaging.ServiceBus;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapPost("/integration", async (ItemCheckinCheckoutRequestViewModel request) =>
{
    var connectionString = builder.Configuration["Servicebus:ConnectionStringCaseQueue"];   
    var client = new ServiceBusClient(connectionString);
    var sender = client.CreateSender("maintenance-checkin-checkout");
    var body = JsonSerializer.Serialize(request);
    var message = new ServiceBusMessage(body);
    await sender.SendMessageAsync(message);

});

app.Run();


public class ItemCheckinCheckoutRequestViewModel
{
    // AMRentDeviceCheckOrigin
    // Eproc = 1
    // App = 2
    // Telemetry = 3
    // SalesForce = 4
    public int CheckOrigin { get; set; }

    // CNPJCPFNum_BR
    public string Document { get; set; }
    public string IntegrationRefId { get; set; }
    public string RegistrationNumber { get; set; }

    // Checkin or Checkout date
    public DateTime WorkshopCheckDate { get; set; }

    // WorkshopCheckType
    // Checkin = 1
    // Checkout = 2
    public int WorkshopCheckType { get; set; }
    public bool WorkshopCustomerConfirmation { get; set; }
    public string WorkshopObservation { get; set; }
    public bool WorkshopServiceCompleted { get; set; }
    public string WorkshopServiceReason { get; set; }
}
