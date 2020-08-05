using System.Net;
using System.Net.Mail;

namespace IndianBank_ChatBOT.Utils
{
    public class EmailUtility
    {
        public static void SendMail(EmailDetails emailDetails)
        {
            var fromAddress = new MailAddress(emailDetails.From, "Integra");
            var toAddress = new MailAddress(emailDetails.To, emailDetails.ToName);

            const string fromPassword = "bng@9343416401";
            string subject = emailDetails.Subject;
            string body = emailDetails.Body;
            //  string body = "<html><body><table>< tr > < td > Firstname </ td > < td > Shrati </ td > </ tr > < tr > < td > Lastname </ td > < td > Joshi </ td ></ td ></ tr > < tr > < td > Age </ td > < td > 23 </ td ></ tr ></table></body></html>";

            var smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}
