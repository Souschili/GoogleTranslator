
using SharedSource.Contract;
using System.Net.Http.Headers;
using TranslationDemo.Api.DTO;
using TranslationDemo.Api.Services;

namespace TranslationDemo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            // Регистрация HttpClient с настройками
            builder.Services.AddHttpClient<GoogleTranslateService>("Google", client =>
            {
                client.BaseAddress = new Uri("https://translate.googleapis.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });


            builder.Services.AddScoped<ITranslateService, GoogleTranslateService>();


            builder.Services.AddMemoryCache(options =>
            {
                options.TrackStatistics = true;

            });
            builder.Services.AddGrpc();
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

            app.MapGrpcService<GrpcGoogleTranslatorService>();
            app.MapControllers();
            // можно и так но меня бесит минимал апи
            //app.MapPost("/api/google-translate", async (TranslateRequestDto requestDto, ITranslateService translateService) =>
            //{
            //    try
            //    {
            //        var result = await translateService.TranslateAsync(requestDto.MapToProtoRequest());
            //        return Results.Ok(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return Results.BadRequest(ex.Message);
            //    }
            //});
            app.Run();
        }
    }
}
