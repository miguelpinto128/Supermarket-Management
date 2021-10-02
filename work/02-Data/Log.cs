using System;
using System.Collections.Generic;
using System.Text;

namespace work.Data
{
    [Serializable]
    class Log
    {
        #region Properties
        public DateTime Added { get; set; }
        public long AddedBy { get; set; }
        public DateTime Updated { get; set; }
        public long UpdatedBy { get; set; }
        #endregion

        public Log(long userId)
        {
            this.Added = DateTime.Now;
            this.AddedBy = userId;
        }
        public Log(DateTime Added, long AddedBy, DateTime Updated, long UpdatedBy)
        {
            this.Added = Added;
            this.AddedBy = AddedBy;
            this.Updated = Updated;
            this.UpdatedBy = UpdatedBy;
        }
    }
}
