using netbackendforeningsblog.Models;
using System.Net.Mail;

namespace netbackendforeningsblog.Service
{
    public class EmailSerivce
    {
        public static string EmailStr = @"Du er hermed inviteret til {Title}";

        public static void PrepareEmail(List<string> userEmails, string title)
        {
            try
            {
                foreach (var usrEmail in userEmails)
                {
                    var email = BuildEmailContent(usrEmail, title);
                    SendMail(email);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static Email? BuildEmailContent(string userEmail, string title)
        {
            string body = EmailStr;
            try
            {
                // replacing the required things  
                body = body.Replace("{Title}", title);

                Email email = new Email
                {
                    UserEmail = userEmail,
                    Body = body,
                    Title = title
                };

                return email;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        private async static void SendMail(Email email)
        {
            MailMessage mail = new MailMessage("foreningsblog@thomasblok.dk", email.UserEmail, email.Title, email.Body);
            mail.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("asmtp.simply.com", 587);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("foreningsblog@thomasblok.dk", "Password.123");
            await client.SendMailAsync(mail);
        }
    }
}
