using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Topwox.Core.Utility
{
    public class SmtpMailer
    {
        public static MailSetting Setting;

        public static void SendMail(string to, string subject, BodyBuilder builder)
        {
            try
            {

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(Setting.DisplayName, Setting.From));
                if (!string.IsNullOrEmpty(to))
                {
                    foreach (var item in to.Split(','))
                    {
                        message.To.Add(new MailboxAddress(item, item));
                    }
                    message.Subject = subject;
                    message.Body = builder.ToMessageBody();
                    using (var client = new SmtpClient())
                    {
                        if (!Setting.UseSSL)
                            client.ServerCertificateValidationCallback = (object sender2, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
                        client.Connect(Setting.Host, Setting.Port, false);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Authenticate(Setting.UserName, Setting.Password);
                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
                else
                {
                    throw new System.ArgumentNullException("收件地址不能为空");
                }
            }
            catch(Exception e)
            {
                throw new System.Net.Mail.SmtpException("邮件发送错误", e);
            }
        }
    }
}