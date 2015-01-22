using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MLMS.Objects;
using MLMS.Service;

namespace MLMS.User
{
    /// <summary>
    /// Summary description for UserWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class UserWebService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string SaveLead(string FName, string LName, string Adult, string PGFName, string PGLName, string DOB, string PrimNumb, string SecNumb, string PrefCallBack, string CallBackDate, string EmailAdd, string IntroMeetDate, string HowDidYouHear, string WhatToAchieve, string Notes)
        {
            if (FName.Trim().Length > 0 || PrimNumb.Trim().Length > 0 || CallBackDate.Trim().Length > 0)
            {
                
                return "Success";
            }
            else return "Failed";
        }
        [WebMethod(EnableSession = true)]
        public string GetCalendarEvents()
        {
            return "Success";
        }
    }
}

