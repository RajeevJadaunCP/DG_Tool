using System;using CardPrintingApplication;
using System.Net.Mail;
using System.Net;
using System.Windows.Forms;
using System.Text;
using System.Xml.Linq;

namespace DG_Tool.HelperClass
{
    internal class EmailService
    {
        public bool EmailTo(string sendTo, string otp)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("noreply@domain.com");
                message.To.Add(new MailAddress("noreply@domain.com"));
                message.Subject = "Test";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = otp;
                smtp.Port = 587;
                //smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.Host = "smtp.mailtrap.io"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("1423cd4fab0176", "6b728cf1031f1d");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        ////Code to send otp to mobile
        //public bool SMSTo(string sendTo, string otp)
        //{
        //    try
        //    {
        //        string MainUrl = "SMSAPIURL"; //Here need to give SMS API URL
        //        string UserName = "username"; //Here need to give username
        //        string Password = "Password"; //Here need to give Password
        //        string SenderId = "SenderId";
        //        string strMobileno = sendTo;
        //        string URL = "";
        //        URL = MainUrl + "username=" + UserName + "&msg_token=" + Password + "&sender_id=" + SenderId + "&message=" + HttpUtility.UrlEncode(otp).Trim() + "&mobile=" + strMobileno.Trim() + "";
        //        string strResponce = GetResponse(URL);
        //        bool msg = false;
        //        if (strResponce.Equals("Fail"))
        //        {
        //            msg = false;
        //        }
        //        else
        //        {
        //            msg = true;
        //        }
        //        return msg;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        //// End SMS Sending function
        //// Get Response function
        //public static string GetResponse(string smsURL)
        //{
        //    try
        //    {
        //        WebClient objWebClient = new WebClient();
        //        StreamReader reader = new StreamReader(objWebClient.OpenRead(smsURL));
        //        string ResultHTML = reader.ReadToEnd();
        //        return ResultHTML;
        //    }
        //    catch (Exception)
        //    {
        //        return "Fail";
        //    }
        //}

        public bool SendMailOTP(string sendto,string otp,string username)
        {
            try
            {
                MailMessage Msg = new MailMessage("copal@colorplast.in", sendto);
                // Sender e-mail address.
                Msg.Subject = "Forget Password";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Hi " + username + ",\n\n");
                sb.AppendLine("Your one time password is :- " + otp +".\n\n\n\n");
                sb.AppendLine("Email From:");
                sb.AppendLine("Data Gen Tool");
                Msg.Body = sb.ToString();


               // Msg.Body = "Hi " + username + ",\r\n\r\nYour one time password is :- " + otp + ".\r\n\r\n\r\nMail From:-\r\nData Gen Tool";
               // Msg.Body = "Hi this is Mail Testing For Copal";

               // Msg.IsBodyHtml = true;

                // your remote SMTP server IP.
                SmtpClient client = new SmtpClient("smtp.office365.com", 587);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("copal@colorplast.in", "Tarun#100");
                client.Host = "smtp.office365.com";
                client.EnableSsl = true;
                client.TargetName = "STARTTLS/smtp.office365.com";
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                client.Port = 587;
                client.Send(Msg);

                return true;
            }
            catch  //(Exception e)
            {
               // MessageBox.Show(e.Message);
                return false;
            }
            
        }
    }
}
