using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MLMS.Objects;

namespace MLMS.Services
{
    /// <summary>
    /// Summary description for UserService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class UserService : System.Web.Services.WebService
    {

        //[WebMethod]
        //public List<Picture> GetFeed(string FeedType)
        //{
        //    List<Picture> pic = new List<Picture>();
        //    pic[0].PictureDesc = "Test";
        //    pic[0].PictureID = 123;
        //    pic[0].PictureLocation = "SD";
        //    pic[0].PictureRecipe = "Yum";
            
        //    return pic;
        //}
    }
}
