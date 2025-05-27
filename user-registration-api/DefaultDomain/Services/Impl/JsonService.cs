using System.Text.Json;
using LanguageExt;
using static LanguageExt.Prelude;

namespace user_registration_api.DefaultDomain.Services.Impl;

public class JsonService : IJsonService
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };
    
    private readonly JsonSerializerOptions _jsonDeserializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    public string? Stringify<T>(T? obj)
    {
        if (obj == null)
        {
            return null;
        }

        try
        {
            return JsonSerializer.Serialize(obj, _jsonSerializerOptions);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to serialize object to JSON", ex);
        }
    }

    public T? Parse<T>(string? json)
    {
        if (json == null)
        {
            throw new ArgumentNullException(nameof(json));
        }

        return JsonSerializer.Deserialize<T>(json, _jsonDeserializerOptions);
    }

    public Try<string?> StringifyTry<T>(T? obj)
    {
        return obj == null ? Try<string?>(() => null) 
            : Try<string?>(() => JsonSerializer.Serialize(obj, _jsonSerializerOptions));
    }

    public Try<T?> ParseTry<T>(string? json)
    {
        return json == null ? Try<T?>( () => default)
            : Try(() => JsonSerializer.Deserialize<T>(json, _jsonDeserializerOptions));
    }
}