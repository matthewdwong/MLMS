using System;
namespace MLMS.Objects.Security
{
    [Serializable]
    public class PermissionRole
    {
        #region Properties
        /// <summary>
        /// Gets or sets the identity
        /// </summary>
        public int PermissionRoleID { get; set; }

        /// <summary>
        /// Gets or sets the role id
        /// </summary>
        public int RoleID { get; set; }

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
        public bool IsActive { get; set; }


        public Role Role { get; set; }
        public Permission Permission { get; set; }
        #endregion
    }
}
