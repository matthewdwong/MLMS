using System;
namespace MLMS.Objects.Security
{
    [Serializable]
    public class Permission
    {
        #region Properties
        /// <summary>
        /// Gets or sets the permission id
        /// </summary>
        public int PermissionID { get; set; }

        /// <summary>
        /// Gets or sets the permission name
        /// </summary>
        public string PermissionName { get; set; }
        
        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets IsActive
        /// </summary>
        public bool IsActive { get; set; }

              
        #endregion

    }
}
