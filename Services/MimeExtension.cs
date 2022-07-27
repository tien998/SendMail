using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;

namespace MailTesting1.Services
{
    public static class MimeExtension
    {
        public static InternetAddressList Append<MailboxAddress>(this InternetAddressList internetAddressesList, MimeKit.MailboxAddress internetAddress)
        {
            internetAddressesList.Add(internetAddress);
            return internetAddressesList;
        }
    }
}