using Kenny.Services.Email.Messages;

namespace Kenny.Services.Email.Repository.Interfaces
{
    public interface IEmailRepository
    {
        Task SendAndLogEmailAsync(UpdatePaymentResultMessage message);
    }
}
