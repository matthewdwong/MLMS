using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MLMS.Data;

namespace MLMS.Service
{
    public static class AdminService
    {
        #region Insert
        /// <summary>
        /// Inserts How did you hear 
        /// </summary>
        public static int InsertHowDidYouHear(string HowDidYouHear, string UserName)
        {
            return AdminDataAccess.InsertHowDidYouHear(HowDidYouHear, UserName);
        }

        /// <summary>
        /// Inserts Achieve options 
        /// </summary>
        public static int InsertAchieve(string Achieve, string UserName)
        {
            return AdminDataAccess.InsertAchieve(Achieve, UserName);
        }

        /// <summary>
        /// Inserts Objection
        /// </summary>
        public static int InsertObjection(string Objection, string UserName)
        {
            return AdminDataAccess.InsertObjection(Objection, UserName);
        }

        /// <summary>
        /// Inserts User Check List 
        /// </summary>
        public static int InsertUserChecklist(string ChecklistName, string UserName)
        {
            return AdminDataAccess.InsertUserChecklist(ChecklistName, UserName);
        }

        /// <summary>
        /// Inserts User Checklist Option
        /// </summary>
        public static int InsertUserChecklistOption(string OptionName, int UserChecklistID)
        {
            return AdminDataAccess.InsertUserChecklistOption(OptionName, UserChecklistID);
        }
        #endregion

        #region Get
        /// <summary>
        /// Get a list of UserCheckList 
        /// </summary>
        public static DataTable GetUserCheckList(string UserName)
        {
            return AdminDataAccess.GetUserCheckList(UserName);
        }

        #endregion

        #region Update
        /// <summary>
        /// Update How did you hear 
        /// </summary>
        public static void UpdateHowDidYouHear(string HowDidYouHear, int HowDidYouHearID)
        {
            AdminDataAccess.UpdateHowDidYouHear(HowDidYouHear, HowDidYouHearID);
        }

        /// <summary>
        /// Update Achieve option
        /// </summary>
        public static void UpdateAchieve(string Achieve, int AchieveID)
        {
            AdminDataAccess.UpdateAchieve(Achieve, AchieveID);
        }

        /// <summary>
        /// Update objection
        /// </summary>
        public static void UpdateObjection(string Objection, int ObjectionID)
        {
            AdminDataAccess.UpdateObjection(Objection, ObjectionID);
        }

        /// <summary>
        /// Update User Check List
        /// </summary>
        public static void UpdateUserChecklist(string ChecklistName, int UserChecklistID)
        {
            AdminDataAccess.UpdateUserChecklist(ChecklistName, UserChecklistID);
        }

        /// <summary>
        /// Update User Check List Option
        /// </summary>
        public static void UpdateUserChecklistOption(string OptionName, int UserChecklistOptionID)
        {
            AdminDataAccess.UpdateUserChecklistOption(OptionName, UserChecklistOptionID);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete How did you hear 
        /// </summary>
        public static void DeleteHowDidYouHear(int HowDidYouHearID)
        {
            AdminDataAccess.DeleteHowDidYouHear( HowDidYouHearID);
        }

        /// <summary>
        /// Delete Achieve option
        /// </summary>
        public static void DeleteAchieve(int AchieveID)
        {
            AdminDataAccess.DeleteAchieve(AchieveID);
        }

        /// <summary>
        /// Delete Objection
        /// </summary>
        public static void DeleteObjection(int ObjectionID)
        {
            AdminDataAccess.DeleteObjection(ObjectionID);
        }

        /// <summary>
        /// Delete User Check List
        /// </summary>
        public static void DeleteUserChecklist(int UserChecklistID)
        {
            AdminDataAccess.DeleteUserChecklist(UserChecklistID);
        }

        /// <summary>
        /// Delete User Check List Option
        /// </summary>
        public static void DeleteUserChecklistOption(int UserChecklistOptionID)
        {
            AdminDataAccess.DeleteUserChecklistOption(UserChecklistOptionID);
        }
        #endregion
    }
}
