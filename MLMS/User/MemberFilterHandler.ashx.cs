using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using MLMS.Objects;
using MLMS.Service;

namespace MLMS.User
{
    /// <summary>
    /// Summary description for MemberFilterHandler
    /// </summary>
    public class MemberFilterHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            List<Member> Members = new List<Member>();

            // String array to store returned list
            string searchVal = context.Request.QueryString["q"];
            Members = FindMembers(searchVal);  //filter!
            Members = Sort(Members);  //sort!

            StringBuilder sb = new StringBuilder();
            foreach (Member Member in Members)
            {
                sb.AppendFormat("{0}{1}|{2}", (sb.Length > 0 ? "\n" : ""),Member.LastName + ", " + Member.FirstName, Member.MemberID);
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(sb.ToString());
        }

        private List<Member> FindMembers(string searchText)
        {
            List<Member> Members = MemberService.GetMembersByUser(HttpContext.Current.User.Identity.Name, Convert.ToInt32(MemberType.Member));

            return Members.FindAll(delegate(Member up)
            {
                return up.FirstName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                  up.LastName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
            });
        }

        /// <summary>
        /// Sort the list
        /// </summary>        
        private List<Member> Sort(List<Member> Members)
        {
            IEnumerable<Member> sorted = Members.OrderBy(l => l.LastName).ThenBy(l => l.FirstName);
            return sorted.ToList<Member>();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}