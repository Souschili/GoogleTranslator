using Shared;

namespace TranslationDemo.Api.DTO
{
    public static class DtoMapExtention
    {
        public static TranslateRequest MapToProtoRequest(this TranslateRequestDto dto)
        {
            TranslateRequest translateRequest = new TranslateRequest
            {
                From = dto.From,
                To = dto.To
            };

            translateRequest.Texts.AddRange(dto.Texts);

            return translateRequest;
        }
    }
}
