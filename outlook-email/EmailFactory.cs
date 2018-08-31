using System;

namespace Trinity.EmailGeneration
{
    #region EmailFactory
    public static class EmailFactory
    {
        #region GetNewOutlookEmail
        public static IEmail GetNewOutlookEmail (string to, string cc, string bcc, string subject, string body)
        {
            OutlookEmail newEmail = new OutlookEmail ();
            newEmail.DefineOutlookApplicationAndMailItem ();
            if (to != null) newEmail.To = to;
            if (cc != null) newEmail.CC = cc;
            if (bcc != null) newEmail.BCC = bcc;
            if (subject != null) newEmail.Subject = subject;
            if (body != null) newEmail.Body = body;
            newEmail.PopulateEmailItemWithFields ();
            return newEmail;
        }
        #endregion

        #region GetOutlookEmailReply
        public static IEmail GetOutlookEmailReply (bool replyAll)
        {
            OutlookReply newReply = new OutlookReply ();
            newReply.ReplyAll = replyAll;
            newReply.DefineOutlookApplicationAndMailItem ();
            return newReply;
        }
        #endregion
    }
    #endregion
}
