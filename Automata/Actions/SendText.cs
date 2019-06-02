using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace Automata.Actions
{
	class SendText
	{
		public static void Text(string subject, dynamic body, bool mms)
		{
			MailMessage message = new MailMessage();
			if (!mms)
			{
				message.To.Add(Automata.Operations.GetSecret("smsto"));
			}
			else
			{
				message.To.Add(Automata.Operations.GetSecret("mmsto"));
			}
			message.From = new MailAddress(Automata.Operations.GetSecret("mailfrom"));
			message.Subject = subject;
			message.Body = body;
			SmtpClient smtp = new SmtpClient("smtp.gmail.com")
			{
				EnableSsl = true,
				Port = 587,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential(Automata.Operations.GetSecret("mailfrom"), Automata.Operations.GetSecret("apppw"))
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
