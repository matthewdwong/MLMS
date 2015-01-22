using System;
using System.Collections.Generic;

namespace MLMS.Objects
{
    [Serializable]
    public class Member
    {
        #region Fields
        private CalendarEvent _calendarEvent = new CalendarEvent();
        #endregion

        public int MemberID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public bool Adult { get; set; }
        public string PGFirstName { get; set; }
        public string PGLastName { get; set; }
        public string PGFullName { get { return PGFirstName + " " + PGLastName; } }
        public DateTime? DOB { get; set; }
        public string PrimaryNumber { get; set; }
        public string SecondaryNumber { get; set; }
        public int? PreferredCallBackID { get; set; }
        public string PreferredCallBack { get; set; }
        public string EmailAddress { get; set; }
        public int? HowDidYouHearID { get; set; }
        public string Notes { get; set; }
        public int MemberTypeID { get; set; }
        public int? Quality { get; set; }

        public List<Achieve> Achieve
        {
            get { return _achieve; }
            set { _achieve = value; }
        }

        private static List<Achieve> _achieve = new List<Achieve>();

        public List<UserChecklist> UserChecklist
        {
            get { return _userChecklist; }
            set { _userChecklist = value; }
        }

        private static List<UserChecklist> _userChecklist = new List<UserChecklist>();

        public CalendarEvent CalendarEvent
        {
            get { return _calendarEvent; }
            set { _calendarEvent = value; }
        }

        public class MemberComparer
        {
            #region LastName
            private static IComparer<Member> _compareByLastName = new _sortLastName(false);
            public static IComparer<Member> CompareByLastName { get { return _compareByLastName; } }

            private static IComparer<Member> _compareByLastNameDesc = new _sortLastName(true);
            public static IComparer<Member> CompareByLastNameDesc { get { return _compareByLastNameDesc; } }

            private class _sortLastName : IComparer<Member>
            {
                bool _reverse;
                public _sortLastName(bool reverse)
                {
                    this._reverse = reverse;
                }

                #region IComparer<Member> Members

                public int Compare(Member x, Member y)
                {
                    if (_reverse) return y.LastName.CompareTo(x.LastName);
                    else return x.LastName.CompareTo(y.LastName);
                }

                #endregion
            }
            #endregion
            #region FirstName
            private static IComparer<Member> _compareByFirstName = new _sortFirstName(false);
            public static IComparer<Member> CompareByFirstName { get { return _compareByFirstName; } }

            private static IComparer<Member> _compareByFirstNameDesc = new _sortFirstName(true);
            public static IComparer<Member> CompareByFirstNameDesc { get { return _compareByFirstNameDesc; } }

            private class _sortFirstName : IComparer<Member>
            {
                bool _reverse;
                public _sortFirstName(bool reverse)
                {
                    this._reverse = reverse;
                }

                #region IComparer<Member> Members

                public int Compare(Member x, Member y)
                {
                    if (_reverse) return y.FirstName.CompareTo(x.FirstName);
                    else return x.FirstName.CompareTo(y.FirstName);
                }

                #endregion
            }
            #endregion
        }
    }
}
