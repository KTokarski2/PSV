namespace PSV.Services
{
    public interface ISmsService
    {
        public Task SendMessage(string phoneNumber, string templateName, params string[] args);

    }
}