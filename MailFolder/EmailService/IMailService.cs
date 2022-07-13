using System.Threading.Tasks;
using RealtyWebApp.MailFolder.MailEntities;

namespace RealtyWebApp.MailFolder.EmailService
{
    public interface IMailService
    {
        Task WelcomeMail(WelcomeMessage message);
    }
}