using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using WebApi.Accessors;
using WebApi.Adapters;
using WebApi.Adapters.Common;
using WebApi.Adapters.LabelAdapter;
using WebApi.Helpers;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;

    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignore omitted parameters on models to enable optional params (e.g. User update)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        x.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

    // configure strongly typed settings object
    services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));

    // configure DI for application services
    services.AddSingleton<DataContext>();
    services.AddScoped<ILabelService, LabelService>();
    services.AddScoped<ILabelAccessor, LabelAccessor>();
    services.AddScoped<ILabelAdapter, LabelAdapter>();
    services.AddScoped<IPagingAdapter, PagingAdapter>();
    services.AddScoped<IDbUtils, DbUtils>();
    services.AddScoped<ICommonUtils, CommonUtils>();
}

var app = builder.Build();

// ensure database and tables exist
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
}

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.MapControllers();
}

app.Run("http://localhost:4000");