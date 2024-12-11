using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Shared;
using SharedSource.Contract;

namespace ClientConsoleApp.Services
{
    internal class GoogleTranslator : ITranslateService
    {
        private readonly TranslationService.TranslationServiceClient _grpcTranslationClient;

        public GoogleTranslator(TranslationService.TranslationServiceClient grpcServiceClient)
        {
            _grpcTranslationClient = grpcServiceClient;
        }
        public async Task<ServiceInfoResponse> Information()
        {
            try
            {
                var info= await _grpcTranslationClient.GetServiceInfoAsync(new Google.Protobuf.WellKnownTypes.Empty());
                return info;
            }
            catch (Exception ex) {

                Console.WriteLine($"Абонент не абонент по причине {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<string>> TranslateAsync(TranslateRequest request)
        {
            try
            {
                TranslateResponse response = await _grpcTranslationClient.TranslateAsync(request);
                return response.Translations.ToList();
            }
            catch (RpcException ex)
            {

                Console.WriteLine($"Ошибка при выполнении перевода: {ex.Message}");
                return Enumerable.Empty<string>(); // Возвращаем пустой 
            }
        }
    }
}
