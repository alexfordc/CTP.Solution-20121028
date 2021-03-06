﻿using System;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Threading;
using WrapperTest;
using System.Net;

namespace SendMail
{
    public static class Email
    {
        public static void SendMail(string subjectInfo = "Test sending e_mail",
            string bodyInfo = "Hello Eric, This is my first testing e_mail", bool isMailingEnabled = true,
            string attachedFiles = null)
        {
            try
            {
                //if (isMailingEnabled)
                //{
                //    const string senderServerIp = "smtp.163.com";

                //    string toMailAddress = "liuning.1982@qq.com";
                //    string fromMailAddress = "liu7788414@163.com";
                //    string mailUsername = "liu7788414";
                //    string mailPassword = "guang1982***"; //发送邮箱的密码（）
                //    string mailPort = "25";

                //    var email = new MyEmail(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo,
                //        mailUsername, mailPassword, mailPort, false, false);

                //    if (!string.IsNullOrEmpty(attachedFiles))
                //    {
                //        email.AddAttachments(attachedFiles);
                //    }

                //    email.Send();
                //}

            }
            catch (Exception ex)
            {
                Utils.WriteException(ex);
            }
        }

        private static string url = "http://utf8.sms.webchinese.cn/?";
        private static string strUid = "Uid=";
        private static string strKey = "&key=71504c7f46f1233b3776"; //这里*代表秘钥，由于从长有点麻烦，就不在窗口上输入了
        private static string strMob = "&smsMob=";
        private static string strContent = "&smsText=";
        private static DateTime dtLastTime = DateTime.Now - new TimeSpan(0, 1, 0);

        public static string SendMessage(bool isMailingEnabled = true, string userName = "liu7788414", string txtAttnNum = "15800377605", string txtContent = "触发交易信号")
        {
            if (isMailingEnabled)
            {
                //两条短信间隔要超过1分钟
                var dtNow = DateTime.Now;
                var tsTimeSpan = dtNow - dtLastTime;    
                dtLastTime = dtNow;

                if(tsTimeSpan <= new TimeSpan(0,1,0))
                {
                    Thread.Sleep(new TimeSpan(0,1,1) - tsTimeSpan);  //等待到达1分钟
                }

                if (userName.Trim() != "" && txtAttnNum.Trim() != "" && txtContent.Trim() != null)
                {
                    var fullUrl = url + strUid + userName + strKey + strMob + txtAttnNum + strContent + txtContent + " " + DateTime.Now.ToString("HH:mm:ss.fff");
                    //string Result = GetHtmlFromUrl(fullUrl);

                    //return Result;
                }
            }

            return "";
        }       

        public static string GetHtmlFromUrl(string url)
        {
            string strRet = null;
            if (url == null || url.Trim().ToString() == "")
            {
                return strRet;
            }
            string targeturl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.Default);
                strRet = ser.ReadToEnd();
            }
            catch (Exception ex)
            {
                strRet = null;
            }
            return strRet;
        }
    }

    public class MyEmail
    {
        private MailMessage mMailMessage; //主要处理发送邮件的内容（如：收发人地址、标题、主体、图片等等）
        private SmtpClient mSmtpClient; //主要处理用smtp方式发送此邮件的配置信息（如：邮件服务器、发送端口号、验证方式等等）
        private int mSenderPort; //发送邮件所用的端口号（htmp协议默认为25）
        private string mSenderServerHost; //发件箱的邮件服务器地址（IP形式或字符串形式均可）
        private string mSenderPassword; //发件箱的密码
        private string mSenderUsername; //发件箱的用户名（即@符号前面的字符串，例如：hello@163.com，用户名为：hello）
        private bool mEnableSsl; //是否对邮件内容进行socket层加密传输
        private bool mEnablePwdAuthentication; //是否对发件人邮箱进行密码验证

        ///<summary>
        /// 构造函数
        ///</summary>
        ///<param name="server">发件箱的邮件服务器地址</param>
        ///<param name="toMail">收件人地址（可以是多个收件人，程序中是以“;"进行区分的）</param>
        ///<param name="fromMail">发件人地址</param>
        ///<param name="subject">邮件标题</param>
        ///<param name="emailBody">邮件内容（可以以html格式进行设计）</param>
        ///<param name="username">发件箱的用户名（即@符号前面的字符串，例如：hello@163.com，用户名为：hello）</param>
        ///<param name="password">发件人邮箱密码</param>
        ///<param name="port">发送邮件所用的端口号（htmp协议默认为25）</param>
        ///<param name="sslEnable">true表示对邮件内容进行socket层加密传输，false表示不加密</param>
        ///<param name="pwdCheckEnable">true表示对发件人邮箱进行密码验证，false表示不对发件人邮箱进行密码验证</param>
        public MyEmail(string server, string toMail, string fromMail, string subject, string emailBody, string username,
            string password, string port, bool sslEnable, bool pwdCheckEnable)
        {
            try
            {
                mMailMessage = new MailMessage();
                mMailMessage.To.Add(toMail);
                mMailMessage.From = new MailAddress(fromMail);
                mMailMessage.Subject = subject;
                mMailMessage.Body = emailBody;
                mMailMessage.IsBodyHtml = true;
                mMailMessage.BodyEncoding = Encoding.UTF8;
                mMailMessage.Priority = MailPriority.Normal;
                mSenderServerHost = server;
                mSenderUsername = username;
                mSenderPassword = password;
                mSenderPort = Convert.ToInt32(port);
                mEnableSsl = sslEnable;
                mEnablePwdAuthentication = pwdCheckEnable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        ///<summary>
        /// 添加附件
        ///</summary>
        ///<param name="attachmentsPath">附件的路径集合，以分号分隔</param>
        public void AddAttachments(string attachmentsPath)
        {
            try
            {
                string[] path = attachmentsPath.Split(';'); //以什么符号分隔可以自定义
                for (int i = 0; i < path.Length; i++)
                {
                    var data = new Attachment(path[i], MediaTypeNames.Application.Octet);
                    var disposition = data.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(path[i]);
                    disposition.ModificationDate = File.GetLastWriteTime(path[i]);
                    disposition.ReadDate = File.GetLastAccessTime(path[i]);
                    mMailMessage.Attachments.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        ///<summary>
        /// 邮件的发送
        ///</summary>
        public void Send()
        {
            try
            {
                if (mMailMessage != null)
                {
                    mSmtpClient = new SmtpClient
                    {
                        Host = mSenderServerHost,
                        Port = mSenderPort,
                        UseDefaultCredentials = false,
                        EnableSsl = mEnableSsl
                    };

                    if (mEnablePwdAuthentication)
                    {
                        System.Net.NetworkCredential nc = new System.Net.NetworkCredential(mSenderUsername,
                            mSenderPassword);

                        mSmtpClient.Credentials = nc.GetCredential(mSmtpClient.Host, mSmtpClient.Port, "NTLM");
                    }
                    else
                    {
                        mSmtpClient.Credentials = new System.Net.NetworkCredential(mSenderUsername, mSenderPassword);
                    }
                    mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mSmtpClient.Send(mMailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}