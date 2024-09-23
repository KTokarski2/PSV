using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PSV.Services;

namespace PSV.Services;

public class SmsService : ISmsService
{
    private readonly HttpClient _httpClient;
    private readonly string _username;
    private readonly string _password;
    private readonly IConfiguration _configuration;

    public SmsService(IOptions<SmsServiceSettings> smsServiceSettings, IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _username = smsServiceSettings.Value.Username;
        _password = smsServiceSettings.Value.Password;
        _httpClient.BaseAddress = new Uri(smsServiceSettings.Value.BaseAddress);
        _configuration = configuration;
    }

    public async Task SendMessage(string phoneNumber, string templateName, params string[] args)
    {
        
        string messageTemplate = GetSmsTemplate(templateName);
        string message = string.Format(messageTemplate, args);
        
        var payload = new 
        {
            msisdn = phoneNumber,
            body = message
        };

        var json = JsonConvert.SerializeObject(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/sms/send");

        var authenticationString = $"{_username}:{_password}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
        request.Content = content;
        try {

            var response = await _httpClient.SendAsync(request);

        } catch (Exception ex)
        {
            ex.GetBaseException();
        }

    }

    private string GetSmsTemplate(string templateName)
    {
        return _configuration[$"SmsTemplates:{templateName}"];
    }
}