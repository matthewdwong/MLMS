using System;

namespace MLMS.Objects.Security
{
    [Serializable]
    public class UserRole
    {
        #region Properties
        /// <summary>
        /// Gets or sets the RoleUserId
        /// </summary>
        public int UserRoleID { get; set; }
        
        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public int UserID { get; set; }
        
        /// <summary>
        /// Gets or sets the host id
        /// </summary>
        public int HostID { get; set; }
        
        /// <summary>
        /// Gets or sets the role id
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// Gets or sets the role name
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the role name alias
        /// </summary>
        public string RoleNameAlias { get; set; }

       
        #endregion
    }
}
