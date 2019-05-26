using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace Automata.Actions
{
    class SendMail
    {
		public static void Mail(string subject, dynamic body)
		{
			MailMessage message = new MailMessage();
			message.To.Add(Automata.DBOps.GetSecret("mailto"));
			message.From = new MailAddress(Automata.DBOps.GetSecret("mailfrom"));
			message.Subject = subject;
			message.Body = body;
			SmtpClient smtp = new SmtpClient("smtp.gmail.com")
			{
				EnableSsl = true,
				Port = 587,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential(Automata.DBOps.GetSecret("mailfrom"), Automata.DBOps.GetSecret("apppw"))
			};
			try
			{
				smtp.Send(message);
				Console.WriteLine("Success");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
    }
}
