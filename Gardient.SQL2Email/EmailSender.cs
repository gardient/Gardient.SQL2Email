using Gardient.SQL2Email.Helpers;
using Gardient.SQL2Email.Models;
using Serilog;
using System;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace Gardient.SQL2Email
{
    static class EmailSender
    {
        private static readonly SmtpClient smtpClient;
        private static readonly string bodyTemplate;
        private static readonly string subjectTemplate;
        private static readonly bool isHtmlBody;
        static EmailSender()
        {
            smtpClient = new SmtpClient();
            bodyTemplate = File.ReadAllText(ConfigHelper.BodyTemplatePath);
            isHtmlBody = Path.GetExtension(ConfigHelper.BodyTemplatePath) == "html";
            subjectTemplate = ConfigHelper.SubjectTemplate;
        }

        public static void SendEmails(QueryResult queryResult)
        {
            Log.Debug("sending mails to {RowCount} people", queryResult.RowCount);
            foreach (QueryResultRow row in queryResult.Rows)
            {
                string body = GetEmailBody(row);
                string subject = GetSubject(row);
                SendEmail(row.Email, body, subject);
            }
            Log.Debug("Done sending mails");
        }

        private static string GetSubject(QueryResultRow row)
        {
            return TemplateHelper.ReplaceTokens(subjectTemplate, row);
        }

        private static string GetEmailBody(QueryResultRow row)
        {
            return TemplateHelper.ReplaceTokens(bodyTemplate, row);
        }

        private static void SendEmail(string email, string body, string subject)
        {
            using (MailMessage emailMessage = new MailMessage())
            {
                emailMessage.To.Add(email);
                emailMessage.Subject = subject;
                emailMessage.BodyEncoding = Encoding.UTF8;
                emailMessage.IsBodyHtml = isHtmlBody;
                emailMessage.Body = body;
                emailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                smtpClient.Send(emailMessage);
            }
        }
    }
}
