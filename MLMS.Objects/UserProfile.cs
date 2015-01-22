namespace MLMS.Objects
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines teh User Profile type.
    /// contains user registration and profile information.
    /// </summary>
    [Serializable]
    public class UserProfile
    {
        /// <summary>
        /// Gets or sets User Identifier
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets HostID
        /// </summary>
        public int HostID { get; set; }
        
        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets LastName.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets OriginalUserName.
        /// </summary>
        public string OriginalUserName { get; set; }

        /// <summary>
        /// Gets or sets Role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets AddressLine1.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets AddressLine2.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets City.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets State.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets Country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets Zip.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 
        /// the user wishes to receive promotional correspondence.
        /// </summary>
        public bool? ReceivePromoEmail { get; set; }

        /// <summary>
        /// Gets or sets ReceivePromoText.
        /// </summary>
        public bool? ReceivePromoText { get; set; }

        /// <summary>
        /// Gets or sets Phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets CellPhone.
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// Gets or sets AdditionalEmail.
        /// </summary>
        public string AdditionalEmail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 
        /// the user wishes to share their data with 
        /// our partner.
        /// </summary>
        public bool? ShareDataWithPartner { get; set; }

        /// <summary>
        /// Gets or sets Age.
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// Gets or sets Gender.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets Ethnicity.
        /// </summary>
        public string Ethnicity { get; set; }

        /// <summary>
        /// Gets or sets Experience.
        /// </summary>
        public string Experience { get; set; }

        /// <summary>
        /// Gets or sets PositionsHeld.
        /// </summary>
        public string PositionsHeld { get; set; }

        /// <summary>
        /// Gets or sets SiteReferral about CertifyU.
        /// </summary>
        public int? SiteReferral { get; set; }

        /// <summary>
        /// Gets or sets ModifiedBy.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets ModifiedDate.
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// Gets or sets a value indicating whether 
        /// the user is locked out, preventing login
        /// </summary>
        public bool IsLockedOut { get; set; }

        /// <summary>
        /// Gets or sets CreatedDate.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        public bool IsInRole(string role)
        {
            if (this.Role == role)
                    return true;

            return false;
        }

        public int CompareTo(UserProfile other) { return UserName.CompareTo(other.UserName); }

        /// <summary>
        /// Gets or sets culture code.
        /// </summary>
        public string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets User Profile Picture.
        /// </summary>
        public string ProfilePicture { get; set; }
    }
    public class UserProfileComparer
    {
        
        #region LastName
        private static IComparer<UserProfile> _compareByLastName = new _sortLastName(false);
        public static IComparer<UserProfile> CompareByLastName { get { return _compareByLastName; } }

        private static IComparer<UserProfile> _compareByLastNameDesc = new _sortLastName(true);
        public static IComparer<UserProfile> CompareByLastNameDesc { get { return _compareByLastNameDesc; } }

        private class _sortLastName : IComparer<UserProfile>
        {
            bool _reverse;
            public _sortLastName(bool reverse)
            {
                this._reverse = reverse;
            }

            #region IComparer<UserProfile> Members

            public int Compare(UserProfile x, UserProfile y)
            {
                if (_reverse) return y.LastName.CompareTo(x.LastName);
                else return x.LastName.CompareTo(y.LastName);
            }

            #endregion
        }
        #endregion
        #region FirstName
        private static IComparer<UserProfile> _compareByFirstName = new _sortFirstName(false);
        public static IComparer<UserProfile> CompareByFirstName { get { return _compareByFirstName; } }

        private static IComparer<UserProfile> _compareByFirstNameDesc = new _sortFirstName(true);
        public static IComparer<UserProfile> CompareByFirstNameDesc { get { return _compareByFirstNameDesc; } }

        private class _sortFirstName : IComparer<UserProfile>
        {
            bool _reverse;
            public _sortFirstName(bool reverse)
            {
                this._reverse = reverse;
            }

            #region IComparer<UserProfile> Members

            public int Compare(UserProfile x, UserProfile y)
            {
                if (_reverse) return y.FirstName.CompareTo(x.FirstName);
                else return x.FirstName.CompareTo(y.FirstName);
            }

            #endregion
        }
        #endregion
        #region UserName
        private static IComparer<UserProfile> _compareByUserName = new _sortUserName(false);
        public static IComparer<UserProfile> CompareByUserName { get { return _compareByUserName; } }

        private static IComparer<UserProfile> _compareByUserNameDesc = new _sortUserName(true);
        public static IComparer<UserProfile> CompareByUserNameDesc { get { return _compareByUserNameDesc; } }

        private class _sortUserName : IComparer<UserProfile>
        {
            bool _reverse;
            public _sortUserName(bool reverse)
            {
                this._reverse = reverse;
            }

            #region IComparer<UserProfile> Members

            public int Compare(UserProfile x, UserProfile y)
            {
                if (_reverse) return y.UserName.CompareTo(x.UserName);
                else return x.UserName.CompareTo(y.UserName);
            }

            #endregion
        }
        #endregion
        #region Role
        private static IComparer<UserProfile> _compareByRole = new _sortRole(false);
        public static IComparer<UserProfile> CompareByRole { get { return _compareByRole; } }

        private static IComparer<UserProfile> _compareByRoleDesc = new _sortRole(true);
        public static IComparer<UserProfile> CompareByRoleDesc { get { return _compareByRoleDesc; } }

        private class _sortRole : IComparer<UserProfile>
        {
            bool _reverse;
            public _sortRole(bool reverse)
            {
                this._reverse = reverse;
            }

            #region IComparer<UserProfile> Members

            public int Compare(UserProfile x, UserProfile y)
            {
                if (_reverse) return y.Role.CompareTo(x.Role);
                else return x.Role.CompareTo(y.Role);
            }

            #endregion
        }
        #endregion
    }

}
