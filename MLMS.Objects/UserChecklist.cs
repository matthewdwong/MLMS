using System;
using System.Collections.Generic;

namespace MLMS.Objects
{
    public class UserChecklist
    {
        public int UserChecklistID { get; set; }
        public string ChecklistName { get; set; }

        public List<UserChecklistOption> UserChecklistOption
        {
            get { return _userChecklistOption; }
            set { _userChecklistOption = value; }
        }

        private static List<UserChecklistOption> _userChecklistOption = new List<UserChecklistOption>();
    }
}
