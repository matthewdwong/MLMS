using System;
namespace MLMS.Objects.Security
{   
    [Serializable]
    public class PermissionUser
    {
        #region Properties
        /// <summary>
        /// Gets or sets the identity
        /// </summary>
        public int PermissionUserID { get; set; }
        
        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public int UserID { get; set; }
        
        /// <summary>
        /// Gets or sets the host id
        /// </summary>
        public int HostID { get; set; }

        /// <summary>
        /// Gets or sets the permission id
        /// </summary>
        public int PermissionID { get; set; }

        /// <summary>
        /// Gets or sets IsActive
        /// </summary>
        public string IsActive { get; set; }

        #endregion
    }
}
