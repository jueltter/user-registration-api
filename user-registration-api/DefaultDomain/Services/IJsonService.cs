using LanguageExt;

namespace user_registration_api.DefaultDomain.Services;

public interface IJsonService
{
    string? Stringify<T>(T? obj);
    
    T? Parse<T>(string? json);
    
    Try<string?>? StringifyTry<T>(T? obj);
    
    Try<T?> ParseTry<T>(string? json);
}