using System.Net;
using System.Net.Mail;

namespace Company.G05.PL.Helper
{
    public static class EmailSittings
    {
        public static bool SendEmail(Email email)
        {
            // Mail Server : Gmail
            // SMTP : Simple Mail Transfer Protocol

            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("sohailat441@gmail.com", "kqdkorelthtinadt"); // Sender
                client.Send("sohailat441@gmail.com", email.To, email.Subject, email.Body);
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
