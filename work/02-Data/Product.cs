using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using work.Data;
using work.Enums;

namespace work.Data
{
    [Serializable]
    class Product : Log 
    {
        #region Properties
        public long Id { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public EnumTypeProduct Type { get; set; }
        public EnumProductAvailability Availability { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Price { get; set; }

        public Stock Stock { get; set; }
        #endregion

        #region  Constructor
 
        public Product(string productName, string productReference, EnumTypeProduct productType, EnumProductAvailability productAvailability, DateTime expiryDate, decimal price, long userId, Stock Stock) : base(userId)
        {
            this.Id = generateProductId();
            this.Name = productName;
            this.Reference = productReference;
            this.Type = productType;
            this.Availability = productAvailability;
            this.ExpiryDate = expiryDate;
            this.Price = price;
            this.Stock = Stock;
        }


      
        public Product(long id, string productName, string productReference, EnumTypeProduct productType, EnumProductAvailability productAvailability, DateTime expiryDate, decimal price, DateTime Added, long AddedBy, DateTime Updated, long UpdatedBy, Stock Stock): base( Added, AddedBy, Updated, UpdatedBy)
        {
            this.Id = id;
            this.Name = productName;
            this.Reference = productReference;
            this.Type = productType;
            this.Availability = productAvailability;
            this.ExpiryDate = expiryDate;
            this.Price = price;
            this.Stock = Stock;
        }

        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{Name} {Reference}";
        }
        private long generateProductId()
        {
            return DateTime.Now.Ticks;
        }

        #endregion


    }
}
