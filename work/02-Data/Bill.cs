using System;
using System.Collections.Generic;
using System.Text;
using work.Data;

namespace work.Data
{
    [Serializable]
    class Bill
    {
        public long id { get; set; }
        public string employeeName { get; set; }
        public List<ProductSell> products { get; set; }
        public string clientName { get; set; }

        public Bill(long id, string employeeName, List<ProductSell> products, string clientName)
        {
            this.id = id;
            this.employeeName = employeeName;
            this.products = products;
            this.clientName = clientName;

        }
        public Bill(string employeeName, List<ProductSell> products, string clientName)
        {
            this.id = generateBillId();
            this.employeeName = employeeName;
            this.products = products;
            this.clientName = clientName;
        }
        private long generateBillId()
        {
            return DateTime.Now.Ticks;
        }
    }
}
