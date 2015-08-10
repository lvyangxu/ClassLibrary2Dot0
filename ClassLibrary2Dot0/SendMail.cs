using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace ClassLibrary2Dot0
{
    public class SendMail
    {
        /// <summary>
        /// 一次发送邮件的请求
        /// </summary>
        /// <param name="smtpserver">smtp服务器</param>
        /// <param name="mailuser">邮箱帐号</param>
        /// <param name="mailpass">邮箱密码</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="listReceiver">收件人列表</param>
        /// <param name="encode">编码格式</param>
        /// <returns></returns>
        public string sendMail(string smtpserver,string mailuser,string mailpass,string title,string content,List<string> listReceiver,string encode) {
            try
            {
                System.Net.Mail.SmtpClient client = new SmtpClient(smtpserver);
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(mailuser, mailpass);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                string strReceiver = string.Empty;
                for (int i = 0; i < listReceiver.Count; i++) {
                    strReceiver = strReceiver + listReceiver[i];
                    if (i != listReceiver.Count - 1) {
                        strReceiver = strReceiver + ",";
                    }
                }

                System.Net.Mail.MailMessage message = new MailMessage(mailuser, strReceiver, title, content);
                message.Sender = new MailAddress(mailuser);
                message.To.Add(listReceiver[0]);
                message.BodyEncoding = System.Text.Encoding.GetEncoding(encode);
                message.IsBodyHtml = true;
                client.Send(message);
                return "succ";
            }
            catch (Exception e) {
                return e.Message;
            }
        }

        /// <summary>
        /// 如果发送邮件失败,则重试一次
        /// </summary>
        /// <param name="smtpserver">smtp服务器</param>
        /// <param name="mailuser">邮箱帐号</param>
        /// <param name="mailpass">邮箱密码</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="listReceiver">收件人列表</param>
        /// <param name="encode">编码格式</param>
        /// <returns></returns>
        public string doSendMail(string smtpserver, string mailuser, string mailpass, string title, string content, List<string> listReceiver, string encode)
        {
            string sendResult = sendMail(smtpserver, mailuser, mailpass, title, content, listReceiver, encode);
            if (sendResult != "succ") {
                sendResult = sendMail(smtpserver, mailuser, mailpass, title, content, listReceiver, encode);
            }
            return sendResult;
        }
    }
}
