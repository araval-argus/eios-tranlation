namespace eios_tranlation.businesslogic.Features.Language
{
    public interface IGoogleTranslateTextCommandAuthorization
    {
        Task<bool> Authorize(GoogleTranslateTextCommand request);
    }
}