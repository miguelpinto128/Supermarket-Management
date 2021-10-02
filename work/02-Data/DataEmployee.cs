using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using work.Enums;
using work.Data;

namespace work.Data
{
    [Serializable]
    class DataEmployee : Log
    {
        #region Properties
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public long contact { get; set; }
        public DateTime birthDate { get; set; }
        public DateTime initWork { get; set; }


        #endregion

        #region  Constructor
        public DataEmployee( string firstName, string lastName, string address, long contact, DateTime birthDate, DateTime initWork,long userId) : base(userId)
        { 
            this.firstName = firstName;
            this.lastName = lastName;
            this.address = address;
            this.contact = contact;
            this.birthDate = birthDate;
            this.initWork = initWork;
        }

        public DataEmployee(string firstName, string lastName, string address, long contact, DateTime birthDate, DateTime initWork,DateTime Added, long AddedBy, DateTime Updated, long UpdatedBy) : base(Added, AddedBy, Updated, UpdatedBy)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.address = address;
            this.contact = contact;
            this.birthDate = birthDate;
            this.initWork = initWork;
        }
        #endregion
    }
}
