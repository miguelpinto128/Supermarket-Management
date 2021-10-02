using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using work.Enums;
using work.Data;

namespace work.Data
{
    [Serializable]
    class Employee : DataEmployee
    {
        #region Properties
        public long id { get; set; }
        public decimal salary { get; set; }
        public string email { get; set; }
        public string authHash { get; set; }
        public DateTime lastDateLogin { get; set; }
        public EnumTypeEmployee type { get; set; }

        #endregion

        #region  Constructor

        public Employee(string email, EnumTypeEmployee type, string firstName, string lastName, string address, long contact, DateTime birthDate, DateTime initWork, decimal salary, string passWord, long userId) : base( firstName,  lastName,  address,  contact,  birthDate,  initWork,  userId)
        {

            this.id = generateId();
            this.email = email;
            this.authHash = GetHash(passWord);
            this.lastDateLogin = lastDateLogin;
            this.type = type;
            this.firstName = firstName;
            this.lastName = lastName;
            this.address = address;
            this.contact = contact;
            this.birthDate = birthDate;
            this.initWork = initWork;
            this.salary = salary;
        }

        public Employee(long id, string email, string authHash, DateTime lastDateLogin, EnumTypeEmployee type, string firstName, string lastName, string address, long contact, DateTime birthDate, DateTime initWork, decimal salary, DateTime Added, long AddedBy, DateTime Updated, long UpdatedBy) : base( firstName,  lastName,  address,  contact,  birthDate,  initWork,  Added,  AddedBy,  Updated,  UpdatedBy)
        {
            this.id = id;
            this.email = email;
            this.authHash = authHash;
            this.lastDateLogin = lastDateLogin;
            this.type = type;
            this.firstName = firstName;
            this.lastName = lastName;
            this.address = address;
            this.contact = contact;
            this.birthDate = birthDate;
            this.initWork = initWork;
            this.salary = salary;
        }

        #endregion

        #region Methods
        public override string ToString() {
            return $"{firstName} {lastName}";
        }

        private string GetHash(string passsWord)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(string.Concat(this.email, passsWord)));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        private long generateId()
        {
            return DateTime.Now.Ticks;
        }

        #endregion

    }


}
