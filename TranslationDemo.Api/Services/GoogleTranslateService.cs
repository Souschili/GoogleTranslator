using Microsoft.Extensions.Caching.Memory;
using Shared;
using SharedSource.Contract;
using System.Text.Json;


namespace TranslationDemo.Api.Services
{
    public class GoogleTranslateService : ITranslateService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<GoogleTranslateService> _logger;
        public GoogleTranslateService(IHttpClientFactory httpClientFactory, 
            ILogger<GoogleTranslateService> logger,IMemoryCache cache)
        {
            _httpClient = httpClientFactory.CreateClient("Google");
            _cache = cache;
            _logger = logger;
        }
        
       
        public Task<ServiceInfoResponse> Information()
        {
            var cachStat=_cache.GetCurrentStatistics();
            return Task.FromResult( new ServiceInfoResponse
            {
                CacheSize= cachStat?.CurrentEntryCount ?? 0,
                ExternalService = "Google Translate API",
                CacheType = "In-Memory Cache"
            });
        }

        public async Task<IEnumerable<string>> TranslateAsync(TranslateRequest request)
        {
            // Проверяем, есть ли чтото для перевода
            if (request.Texts == null || !request.Texts.Any())
            {
                _logger.LogWarning("No texts provided for translation.");
                return Enumerable.Empty<string>();
            }

            // Создаем ключ для кэша на основе текста и языков перевода
            string cacheKey = $"{request.From}-{request.To}-{string.Join(",", request.Texts)}";

            // Проверяем кэш, чтобы избежать повторных запросов
            if (_cache.TryGetValue(cacheKey, out string cachedTranslations))
            {
                _logger.LogInformation("Returning cached translation result.");
                return cachedTranslations.Split('|').AsEnumerable();
            }

            // тупо хреначим большую строку и отсылаем разом
            // чтобы не создавать 100500 запросов
            string message = String.Join("|", request.Texts);

            // можно оставить автоопределение языка и указывать только на какой переводить
            // Формируем URL запроса
            string url = $"translate_a/single?client=gtx&sl=auto&tl={request.To}&dt=t&q={Uri.EscapeDataString(message)}";
            try
            {
                // Отправляем GET-запрос
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Получаем ответ
                string responseBody = await response.Content.ReadAsStringAsync();

                // парсим ответ и выдираем перевод
                var jsonDoc = JsonDocument.Parse(responseBody);
                // тупо через индекс ,так как все переводы в одном месте,просто и понятно
                var translations = jsonDoc.RootElement[0][0][0].ToString();

                // Кэшируем результат
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                _cache.Set(cacheKey, translations, cacheEntryOptions);

                // разбиваем на строки
                return translations.Split("|").AsEnumerable();
                


            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.Message);
                return Enumerable.Empty<string>();
            }
        }
    }
}
