using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace Goba_Store.Services;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _config;
    public MailJetSettings _mailJetSettings { get; set; }

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Exexute(email, subject, htmlMessage);
    }

    public async Task Exexute(string email, string subject, string htmlMessage)
    {
        /*_mailJetSettings = _config.GetSection("MailJet").Get<MailJetSettings>();
        
        MailjetClient client = new MailjetClient(_mailJetSettings.ApiKey,_mailJetSettings.ApiSecret);
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
