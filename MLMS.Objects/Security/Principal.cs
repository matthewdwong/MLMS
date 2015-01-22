namespace MLMS.Objects.Security
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;


    [Serializable]
    public class Principal : IPrincipal
    {
        #region IPrincipal Members
        private IIdentity _identity;
        //private List<Role> _roles;
        private List<AccessControlEntry> _accessControlList = new List<AccessControlEntry>();

        #endregion

        #region Constructor
        public Principal(IIdentity identity) 
        {
          _identity = identity;          
        }

        #endregion
        
        #region Properties
        
        public IIdentity Identity
        {
            get { return _identity; }
        }
       
        public List<AccessControlEntry> AccessControlList
        {
            get { return _accessControlList; }
            set { _accessControlList = value; }
        }
        #endregion

        #region Methods

        public bool IsInRole(string roleAlias)
        {
            foreach (AccessControlEntry access in _accessControlList)
                if (access.RoleNameAlias == roleAlias)
                    return true;

            return false;
        }
        public bool HasPermission(string permissionName)
        {
            foreach (AccessControlEntry access in _accessControlList)
                if (access.Permission.PermissionName == permissionName)
                        return true;

            return false;
        }

        #endregion

    }
}
