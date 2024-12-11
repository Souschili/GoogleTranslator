using Shared;

namespace SharedSource.Contract
{
    public interface ITranslateService
    {
        Task<IEnumerable<string>> TranslateAsync(TranslateRequest request);
        Task<ServiceInfoResponse> Information();
    }
}
