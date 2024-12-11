namespace TranslationDemo.Api.DTO
{
    public record class TranslateRequestDto(string From, string To, IEnumerable<string> Texts);
   
}
