using System;
using System.Diagnostics;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Trinity.EmailGeneration
{
    #region IEmail interface
    public interface IEmail
    {
        string To { get; set; }
        string CC { get; set; }
        string BCC { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        void Generate ();
    }
    #endregion

    #region OutlookEmail
    public class OutlookEmail : IEmail
    {
        #region Members
        protected string _to;
        protected string _cc;
        protected string _bcc;
        protected string _subject;
        protected string _body;
        protected Outlook.Application _outlookApp;
        protected Outlook.MailItem _outlookMailItem;
        protected bool _outlookApplicationAndMailItemAreInitialized;
        protected bool _emailFieldsArePopulated;
        #endregion

        #region Generate
        public virtual void Generate ()
        {
            if (!_outlookApplicationAndMailItemAreInitialized) DefineOutlookApplicationAndMailItem ();
            if (!_emailFieldsArePopulated) PopulateEmailItemWithFields ();
            _outlookMailItem.Display ();
            releaseComInteropResources ();
        }
        #endregion

        #region DefineOutlookApplicationAndMailItem
        public virtual void DefineOutlookApplicationAndMailItem ()
        {
            defineOutlookApp ();
            initializeEMailItem ();
            _outlookApplicationAndMailItemAreInitialized = true;
        }
        #endregion

        #region defineOutlookApp
        protected virtual void defineOutlookApp ()
        {
            try
            {
                _outlookApp = (Outlook.Application)System.Runtime.InteropServices.Marshal.GetActiveObject ("Outlook.Application");
            }
            catch (EmailException)
            {
                throw;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                Debug.WriteLine ("defineOutlookApp() unable to grab existing Outlook application so attempting to create new instance.");
                try
                {
                    _outlookApp = new Outlook.Application ();
                }
                catch (Exception)
                {
                    string errorMsg = "defineOutlookApp() unable to either grab existing Outlook application or create new instance.";
                    Debug.WriteLine (errorMsg);
                    throw new EmailException (errorMsg);
                }
            }
        }
        #endregion

        #region initializeEMailItem
        protected virtual void initializeEMailItem ()
        {
            _outlookMailItem = _outlookApp.CreateItem (Outlook.OlItemType.olMailItem);
        }
        #endregion

        #region PopulateEmailItemWithFields
        public virtual void PopulateEmailItemWithFields ()
        {
            _outlookMailItem.To = _to;
            _outlookMailItem.CC = _cc;
            _outlookMailItem.BCC = _bcc;
            _outlookMailItem.Body = _body;
            _outlookMailItem.Subject = _subject;
            _emailFieldsArePopulated = true;
        }
        #endregion

        #region releaseComInteropResources
        protected void releaseComInteropResources ()
        {
            try
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject (_outlookApp);
            }
            catch { }   // Eat error
            finally
            {
                _outlookApp = null;
            }
        }
        #endregion

        #region Properties
        public virtual string To
        {
            get { return _to; }
            set { _to = value; }
        }

        public virtual string CC
        {
            get { return _cc; }
            set { _cc = value; }
        }

        public virtual string BCC
        {
            get { return _bcc; }
            set { _bcc = value; }
        }

        public virtual string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public virtual string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        #endregion
    }
    #endregion

    #region OutlookReply
    public class OutlookReply : OutlookEmail
    {
        #region Members
        protected bool _replyAll;
        protected string _htmlBodyOfEmailBeingRepliedTo;
        protected string _bodyTextOfEmailBeingRepliedTo;
        #endregion

        #region defineOutlookApp
        protected override void defineOutlookApp ()
        {
            try
            {
                Debug.WriteLine ("Called override method defineOutlookApp().");
                _outlookApp = (Outlook.Application)System.Runtime.InteropServices.Marshal.GetActiveObject ("Outlook.Application");
            }
            catch
            {
                string errorMsg = "Unable to get active Outlook object when OutlookEmailReply requires grabbing existing instance.";
                Debug.WriteLine (errorMsg);
                throw new Exception (errorMsg);
            }
        }
        #endregion

        #region initializeEMailItem
        protected override void initializeEMailItem ()
        {
            ensureOutlookAppHasOneAndOnlyOneEmailSelected ();

            if (_replyAll) _outlookMailItem = _outlookApp.ActiveExplorer ().Selection [1].ReplyAll;
            else _outlookMailItem = _outlookApp.ActiveExplorer ().Selection [1].Reply;

            grabPropertiesFromEmailBeingRepliedTo ();
        }
        #endregion

        #region ensureOutlookAppHasOneAndOnlyOneEmailSelected
        protected void ensureOutlookAppHasOneAndOnlyOneEmailSelected ()
        {
            int numberOfSelectedEmails = _outlookApp.ActiveExplorer ().Selection.Count;
            if (numberOfSelectedEmails == 1) return;
            else
            {
                string errorMsg;
                if (numberOfSelectedEmails == 0)
                {
                    errorMsg = "No emails selected in Outlook Active Explorer.";
                }
                else
                {
                    errorMsg = "More than 1 email selected in Outlook Active Explorer.";
                }
                Debug.WriteLine (errorMsg);
                throw new EmailException (errorMsg);
            }
        }
        #endregion

        #region grabPropertiesFromEmailBeingRepliedTo
        protected void grabPropertiesFromEmailBeingRepliedTo ()
        {
            _to = _outlookMailItem.To;
            _cc = _outlookMailItem.CC;
            _htmlBodyOfEmailBeingRepliedTo = _outlookMailItem.HTMLBody;
            _bodyTextOfEmailBeingRepliedTo = _outlookMailItem.Body;
            _subject = _outlookMailItem.Subject;
        }
        #endregion

        #region PopulateEmailItemWithFields
        public override void PopulateEmailItemWithFields ()
        {
            _outlookMailItem.To = _to;
            _outlookMailItem.CC = _cc;
            _outlookMailItem.BCC = _bcc;
            _outlookMailItem.HTMLBody = _body + _htmlBodyOfEmailBeingRepliedTo;
            _outlookMailItem.Subject = _subject;
            _emailFieldsArePopulated = true;
        }
        #endregion

        #region Properties
        public bool ReplyAll
        {
            set { _replyAll = value; }
        }

        /// <summary>
        /// Overrides the base class Body in order to return the body text of the email we are replying to.
        /// </summary>
        public override string Body
        {
            get
            {
                return _bodyTextOfEmailBeingRepliedTo;
            }
            set { base.Body = value; }
        }
        #endregion
    }
    #endregion
}
