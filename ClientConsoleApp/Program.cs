using ClientConsoleApp.Services;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using SharedSource.Contract;

namespace ClientConsoleApp
{
    internal class Program
    {
        public static ITranslateService _translationService;
        static void Main(string[] args)
        {
            ConfigApp();
            if (_translationService == null)
            {
                Console.WriteLine("Unable to find translation service");
                return;
            }

            // основной цикл
            while (true)
            {

                ShowMenu();
                if (!Int32.TryParse(Console.ReadLine(), out int input) || input < 1 || input > 3)
                {
                    Console.WriteLine("Неверный ввод. Пожалуйста, выберите 1, 2 или 3.");
                    continue;
                }
                if (input == 3) break;

                InvokeMenu(input);

            }

        }


        private static void InvokeMenu(int input)
        {
            switch (input)
            {
                case 1:
                    Translate();
                    break;
                case 2:
                    Information();
                    break;
                default:
                    Console.WriteLine("Wrong input");
                    break;
            };

        }

        private static void Information()
        {
           var stat= _translationService.Information().GetAwaiter().GetResult();
            if (stat == null) return;
            Console.WriteLine($"External Service: {stat.ExternalService}");
            Console.WriteLine($"Cache Type: {stat.CacheType}");
            Console.WriteLine($"Cache Size: {stat.CacheSize}");
        }

        private static void Translate()
        {
            // пока так по идее тут надо юзера заставить вписывать
            TranslateRequest request = new TranslateRequest
            {
                To = "en",
                From = "ru"
            };
            Console.Clear();
            List<string> list = new List<string>();
            Console.WriteLine("Input string(s) for translate");
            Console.WriteLine("Input -1 to finish");
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "-1")
                    break;
                if (string.IsNullOrEmpty(input))
                    continue;
                list.Add(input);
            }

            request.Texts.AddRange(list);

            var translateResult = _translationService.TranslateAsync(request).GetAwaiter().GetResult();

            // показываем перевод
            foreach (var line in translateResult)
            {
                Console.WriteLine(line);
            }
            Console.ReadLine();
        }
        private static void ShowMenu()
        {
            Console.WriteLine("1. Начать перевод");
            Console.WriteLine("2. Получить инеформацию о сервере");
            Console.WriteLine("3. Завершить работу");

        }

        private static void ConfigApp()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ITranslateService, GoogleTranslator>()
                .AddSingleton<TranslationService.TranslationServiceClient>(cfg =>
                {
                    var chanel = GrpcChannel.ForAddress("https://localhost:7202");
                    return new TranslationService.TranslationServiceClient(chanel);
                })
                .BuildServiceProvider();
            _translationService = serviceProvider.GetRequiredService<ITranslateService>();
        }
    }
}
