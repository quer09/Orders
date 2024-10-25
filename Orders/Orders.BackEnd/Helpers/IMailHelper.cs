using Orders.Shared.Responses;

namespace Orders.BackEnd.Helpers
{
    public interface IMailHelper
    {
        ActionResponse<string> SendMail(string toName, string toEmail, string subject, string body);
    }
}
