using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Turkai.Data;
using Turkai.Data.RabbitMq;
using Turkai.Extension;
using Turkai.Service.BackgroundServices;
using Turkai.Service.Interfaces;
using Turkai.Service.Services;
using Nest;
using Turkai.Model.ExtensionModel;
using Turkai.Model.ExtensionModel.ElasticSearch;
using StackExchange.Redis;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
var configuration = builder.Configuration;
var services = builder.Services;

services.AddHttpClient();


builder.Services.AddDbContextPool<TurkaiDbContext>(config =>
{
    config.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
    config.EnableSensitiveDataLogging();

});

services.AddSingleton<TurkaiDbContext>();
services.AddSingleton<IRabbitMqRepo, RabbitMqRepo>();
services.AddSingleton<IDummpyDataService, DummpyDataService>();
services.AddSingleton<IRabbitMQService, RabbitMQService>();


#region  RabbitMQ Configuration
RabbitMqConfigModel rabbitMQConfig = configuration.GetSection("RabbitMQConfig").Get<RabbitMqConfigModel>();

var connectionFactory = new ConnectionFactory
{
    HostName = rabbitMQConfig.HostName,
    UserName = rabbitMQConfig.UserName,
    Password = rabbitMQConfig.Password,
    VirtualHost = rabbitMQConfig.VirtualHost
};
services.AddSingleton<IConnection>(sp =>
{
    return connectionFactory.CreateConnection();
});

services.AddHostedService<GetDataWorkerService>(); 
services.AddHostedService<ReadRabbitMqWorker>();



#endregion


services.AddAutoMapper(typeof(AutoMapperProfile));

#region elastic search 

ElasticSearchConfigModel ElasticConfig = configuration.GetSection("ElasticSearchConfig").Get<ElasticSearchConfigModel>();

var settings = new ConnectionSettings(new Uri($"{ElasticConfig.Url}"))
           .BasicAuthentication(ElasticConfig.UserName, ElasticConfig.Password);
var elasticClient = new ElasticClient(settings);

services.AddSingleton<IElasticClient>(elasticClient);
services.AddTransient<IElasicSearchService, ElasticSearchService>();
#endregion


services.AddTransient<IProductService, ProductService>();

#region Redis

var multiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
services.AddSingleton<IConnectionMultiplexer>(multiplexer);
services.AddSingleton<IRedisService, RedisService>(); ;


#endregion


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
