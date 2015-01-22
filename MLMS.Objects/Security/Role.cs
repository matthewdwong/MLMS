using System;
namespace MLMS.Objects.Security
{
    [Serializable]
    public class Role
    {

        #region Properties
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

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets Hierarchy Order
        /// </summary>
        public string HierarchyOrder { get; set; }



        #endregion
    }
}
