using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Caching.Memory;
using Shared;
using SharedSource.Contract;

namespace TranslationDemo.Api.Services
{
    public class GrpcGoogleTranslatorService : TranslationService.TranslationServiceBase
    {
        private readonly ITranslateService _translateService;
        private readonly ILogger<GrpcGoogleTranslatorService> _logger;
        public GrpcGoogleTranslatorService(ITranslateService translateService,
            ILogger<GrpcGoogleTranslatorService> logger)
        {
            _translateService = translateService;
            _logger = logger;
        }

        public override async Task<ServiceInfoResponse> GetServiceInfo(Empty request, ServerCallContext context)
        {
            return await _translateService.Information();
        }

        public override async Task<TranslateResponse> Translate(TranslateRequest request,
            ServerCallContext context)
        {
            //var response = new TranslateResponse();

            //response.Translations.AddRange(new List<string>{"salam","from","server" ,"!"});
            //return Task.FromResult(response);
            try
            {
                var translateResult = await _translateService.TranslateAsync(request);
                TranslateResponse response = new TranslateResponse();
                response.Translations.AddRange(translateResult);
                return response;
            }
            catch (HttpRequestException ex)
            {
                // Логируем ошибку
                _logger.LogError(ex, "Ошибка при попытке перевода.");
                throw new RpcException(new Status(StatusCode.Internal, "Сбой сервиса гугл переводов.Не фартануло"));
            }
            catch (Exception ex)
            {
                // Логируем общие ошибки
                _logger.LogError(ex, "Неожиданная ошибка.");
                throw new RpcException(new Status(StatusCode.Unknown, "Факир был пьян и фокус не удался."));
            }
        }
    }
}
