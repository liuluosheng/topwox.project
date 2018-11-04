﻿namespace Topwox.Core.Utility
{
    public class MailSetting
    {
        public int Port { get; set; }
        public string From { get; set; }
        public string DisplayName { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseSSL { get; set; }
    }
}