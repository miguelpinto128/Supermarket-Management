using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using work.Data;
using work.Enums;

namespace work.Data
{
    [Serializable]
    class Stock 
    {
        #region Properties
        public int Quantity { get; set; }
        #endregion

        #region  Constructor
        public Stock(int quantity)
        {
            this.Quantity = quantity;
        }

        #endregion

        #region Methods
        #endregion


    }
}
