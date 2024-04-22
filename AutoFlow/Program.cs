using AutoFlow.DAL;
using AutoFlow.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IDbSimulator, DbSimulator>()
    .AddSingleton<IUiService, UiService>()
    .AddTransient<IContactService, ContactService>()
    .AddTransient<IValidationService, ValidationService>()
    .AddTransient<IFileService, FileService>()
    .AddLogging(builder =>
    {
        builder.AddConsole();
    })
    .BuildServiceProvider();

var uiService = serviceProvider.GetRequiredService<IUiService>();
uiService.Run();
