using Npgsql;
using Serilog;
using Serilog.Events;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data;
using V.TouristGuide.Server.Daos;
using V.TouristGuide.Server.Helpers;
using V.TouristGuide.Server.Middlewares;
using V.TouristGuide.Server.Services;
using V.User.Extensions;

DapperHelper.MakeDapperMapping("V.TouristGuide.Server.Entities");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
 {
     config.MinimumLevel.Information()
         .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
         .MinimumLevel.Override("System", LogEventLevel.Warning)
         .ReadFrom.Configuration(context.Configuration)
         .Enrich.FromLogContext();
 });

// Add services to the container.
builder.Services.AddTransient<IDbConnection>(_ =>
{
    var conn = new NpgsqlConnection(builder.Configuration["Postgres:ConnectionString"]);
    conn.Open();
    return conn;
});
builder.Services.AddTransient(_ =>
{
    var db = new QueryFactory(new NpgsqlConnection(builder.Configuration["Postgres:ConnectionString"]), new PostgresCompiler());
    db.Logger = sql =>
    {
        Log.Information(sql.Sql);
    };
    return db;
});
builder.Services.AddTransient<PlaceDao>()
    .AddTransient<PlaceService>()
    .AddTransient<OperationDao>()
    .AddTransient<OperationService>()
    .AddJwt(builder.Configuration["Jwt:Secret"])
    .AddUserModule(c =>
    {
        c.ServiceCode = "TouristGuide";
        c.ServiceName = "ÂÃÓÎÖ¸ÄÏ";
        c.NeedVerificationForSignUp = true;
        c.AccountMode = 1;
        c.TencentSmsSecretId = builder.Configuration["Tencent:Sms:SecretId"];
        c.TencentSmsSecretKey = builder.Configuration["Tencent:Sms:SecretKey"];
        c.TencentSmsAppId = builder.Configuration["Tencent:Sms:AppId"];
        c.TencentSmsSignName = builder.Configuration["Tencent:Sms:SignName"];
        c.TencentSmsTemplateId = builder.Configuration["Tencent:Sms:TemplateId"];
    }, new UserModuleCallback(builder.Configuration))
    .AddCors(o => o.AddPolicy("Default", builder =>
    {
        builder.WithOrigins("http://localhost", "https://vbranch.cn", "http://localhost:8080", "http://192.168.1.4:8080", "http://192.168.1.4")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    }));

builder.Services.AddControllers();
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

app.UseCors("Default")
    .UseMiddleware<SupportOptionsMiddleware>()
    .UseMiddleware<LogMiddleware>()
    .UseUserModule(true)
    .UseStaticFiles();

app.MapControllers();

app.Run();
