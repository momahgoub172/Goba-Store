using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace Goba_Store.Services;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Exexute(email, subject, htmlMessage);
    }

    public async Task Exexute(string email, string subject, string htmlMessage)
    {
        /*
         //TODO:Adding key and secret to appsetting
        MailjetClient client = new MailjetClient("89d99fca58b88f88181f6a08dbb7f341","8a2365e2e35f57c75955444901e7ff17");
         var request = new MailjetRequest
             {
                 Resource = Send.Resource,
             }
             .Property(Send.Messages, new JArray {
                 new JObject {
                     {"From", new JObject {
                         {"Email", "momahgoub172@gmail.com"},
                         {"Name", "Goba"}
                     }},
                     {"To", new JArray {
                         new JObject {
                             {"Email", email}
                         }
                     }},
                     {"Subject", subject},
                     {"HTMLPart", htmlMessage}
                 }
             });
 
         await client.PostAsync(request);
     }*/
        Console.WriteLine(htmlMessage);
    }
}
