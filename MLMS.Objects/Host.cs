
namespace MLMS.Objects
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Host
    {
        #region Properties
        /// <summary>
        /// Gets or sets the host id
        /// </summary>
        public int HostID { get; set; }

        /// <summary>
        /// Gets or sets the host name
        /// </summary>
        public string HostName { get; set; }
        
        /// <summary>
        /// Gets or sets the application name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the theme
        /// </summary>
        public string ThemeName { get; set; }

        /// <summary>
        /// Gets or sets IsSecure (http/https)
        /// </summary>
        public bool IsSecure { get; set; }

        /// <summary>
        /// Gets or sets IsSecure (http/https)
        /// </summary>
        public int TemplateID { get; set; }

        /// <summary>
        /// Gets or sets the label text for ShareDataWithParter field
        /// </summary>
        public string ShareDataWithPartnerText { get; set; }

        /// <summary>
        /// Gets or sets the label text for ShareDataWithPartner help (reduced font size)
        /// </summary>
        public string ShareDataWithPartnerHelpText { get; set; }

        /// <summary>
        /// Gets or sets the label text for the AdditionalEmail field
        /// </summary>
        public string AdditionalEmailText { get; set; }

        /// <summary>
        /// Gets or sets the label text on the Login control
        /// </summary>
        public string LoginHelpText { get; set; }

        /// <summary>
        /// Gets or sets the label text to receive promo email
        /// </summary>
        public string ReceivePromoEmailText { get; set; }

        /// <summary>
        /// Gets or sets the site's shortname
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the Twitter Url
        /// </summary>
        public string TwitterUrl { get; set; }

        /// <summary>
        /// Gets or sets the Facebook Url
        /// </summary>
        public string FacebookUrl { get; set; }

        /// <summary>
        /// Gets or sets the LinkedIn url
        /// </summary>
        public string LinkedInUrl { get; set; }

        /// <summary>
        /// Gets or sets E-mail support address where Contact Us emails are sent
        /// </summary>
        public string EmailSupport { get; set; }

        /// <summary>
        /// Gets or sets the PayPal business account email
        /// </summary>
        public string PayPalAccount { get; set; }

        /// <summary>
        /// Gets or sets the PayPal auth token
        /// </summary>
        public string PayPalAuthorizationToken { get; set; }

        /// <summary>
        /// Gets or sets the company image for display on PayPal
        /// </summary>
        public string PayPalImage { get; set; }

        /// <summary>
        /// Gets or sets the text to display on the return button on PayPal
        /// </summary>
        public string PayPalReturnButtonText { get; set; }

        /// <summary>
        /// Gets or sets the logo image
        /// </summary>
        public string LogoImage { get; set; }

        /// <summary>
        /// Gets or sets the image on the homepage
        /// </summary>        
        public string HomepageImage { get; set; }
        
        /// <summary>
        /// Gets or sets DummyFromEmail for use in Password Recovery, Registration Welcome, and Certificate emails
        /// </summary>
        public string DummyFromEmail { get; set; }

        /// <summary>
        /// Gets or sets the logo image
        /// </summary>
        public string CertificateBackgroundImage { get; set; }

        /// <summary>
        /// Hides Register link on menu
        /// </summary>
        public bool HideRegistration { get; set; }

        /// <summary>
        /// username is an email address
        /// </summary>
        public bool UsernameIsEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the name of the label for the login text box 
        /// </summary>
        public string LabelUserNameText { get; set; }

        /// <summary>
        /// Gets or sets if alias is redirect or not
        /// </summary>
        public bool IsRedirect { get; set; }
        

        public List<RequiredField> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        public string Languages { get; set; }

        public string DefaultLanguage { get; set; }

        public string HostSignInControl { get; set; }

        /// <summary>
        /// Gets or sets the image for the save button on the courses
        /// </summary>        
        public string SaveCourseImage { get; set; }

        /// <summary>
        /// Gets or sets if the host uses chains(retailers)
        /// </summary>        
        public bool UseChainList { get; set; }
        #endregion

        #region Fields
        /// <summary>
        /// Gets or sets configurable fields list.
        /// </summary>
        private static List<RequiredField> _fields = new List<RequiredField>();

        #endregion
      

    }
}
