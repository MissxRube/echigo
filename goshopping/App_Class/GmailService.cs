using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

public class GmailService
{
    public string MessageText { get; set; }
    public string SenderName { get; set; }
    public string SenderEmail { get; set; }
    public string AppPassword { get; set; }
    public string ReceiveEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }

    public GmailService()
    {
        MessageText = "";
        SenderName = "網站管理員";
        SenderEmail = "wanwan1029jj@gmail.com";
        AppPassword = "bqjklrqqumbutbix";
        ReceiveEmail = "";
        Subject = "";
        Body = "";
    }

    public void Send()
    {
        var fromEmail = new MailAddress(SenderEmail, SenderName);
        var toEmail = new MailAddress(ReceiveEmail);
        try
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, AppPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = Subject,
                Body = Body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        catch (Exception ex)
        {
            MessageText = ex.Message;
        }
    }
}
