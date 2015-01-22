using System;
namespace MLMS.Objects.Security
{
    [Serializable]
    public class AccessControlEntry
    {
        /// <summary>
        /// Gets or sets the Role Alias that is responsible for this permission
        /// </summary>
        public string RoleNameAlias { get; set; }

        /// <summary>
        /// Gets or sets the permission
        /// </summary>
        private Permission _permission = new Permission();
        public Permission Permission 
        {
            get { return _permission; }
            set { _permission = value; }
        }
    }
}
