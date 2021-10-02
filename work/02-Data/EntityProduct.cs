using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using work.Enums;

namespace work.Data
{
    [Serializable]
    class EntityProduct
    {
        #region Properties
        public List<Product> listProduct;
        public string StockPath = Directory.GetCurrentDirectory() + ConfigurationManager.AppSettings["StockPath"];
        #endregion

        #region  Constructor
        public EntityProduct()
        {
            this.listProduct = new List<Product>();
        }
        #endregion

        #region Methods
        public Tuple<int, List<string>> GetData(bool? isCaixa = false, string ProductName = "", string ProductReference = "")
        {
            List();
            List<string> list = new List<string>();
            list.Add("NOME PRODUTO|REFERENCIA|TIPO|DISPONIBILIDADE|QUANTIDADE|VALIDADE|PREÇO");
            int size = 0;
            var lsitCat = Enum.GetValues(typeof(EnumTypeProduct)).OfType<EnumTypeProduct>().ToList();
            try
            {
                List<Product> newList = this.listProduct.ToList();

                if (listProduct.Any())
                {
                    newList = newList.Where(x =>
                        (!string.IsNullOrEmpty(ProductName) ? x.Name.ToLower() == ProductName.ToLower() : x.Name.ToLower() != "") &&
                        (!string.IsNullOrEmpty(ProductReference) ? x.Reference.ToLower() == ProductReference.ToLower() : x.Reference.ToLower() != "") &&
                        (isCaixa == true ? x.Availability == EnumProductAvailability.Disponivel && x.Stock.Quantity > 0 : x.Id > 0)
                    ).ToList();

                    if (newList.Any())
                    {
                        foreach (EnumTypeProduct enumAux in lsitCat)
                        {
                            list.Add($"||||||");
                            list.Add($"|||{enumAux}|||");
                            list.Add($"||||||");
                            foreach (var item in newList.Where(x => x.Type == enumAux && (isCaixa == true ? x.Availability == EnumProductAvailability.Disponivel && x.Stock.Quantity > 0 : x.Id > 0)))
                            {
                                List<EnumTypeProduct> types = new List<EnumTypeProduct>();
                                string row = $"{item.Name}|{item.Reference}|{item.Type}|{item.Availability}|{item.Stock.Quantity}|{(item.ExpiryDate != new DateTime() ? item.ExpiryDate.ToString("dd/MM/yyyy") : string.Empty)}|{item.Price }";
                                list.Add(row);
                                if (row.Length > size)
                                {
                                    size = item.Reference.ToString().Length + row.Length;
                                }
                            }
                        }
                    }
                    else
                    {
                        string row = "|||Nenhum resultado encontrado (0)|||";
                        list.Add(row);
                        size = (row.Length * 2) + 10;
                    }

                }
                else
                {
                    string row = "|||Nenhum resultado encontrado (0)|||";
                    list.Add(row);
                    size = (row.Length * 2) + 10;
                }
            }
            catch (Exception)
            {
                string row = "|||Nenhum resultado encontrado (0)|||";
                list.Add(row);
                size = row.Length;
            }

            return new Tuple<int, List<string>>(size, list);

        }

        public bool ClearList()
        {
            try
            {
                this.listProduct.Clear();
                using (TextWriter tw = new StreamWriter(StockPath))
                {
                    tw.WriteLine("");
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void List()
        {
            try
            {
                listProduct.Clear();
                if (File.Exists(StockPath))
                {
                    using (FileStream fileStream = File.OpenRead(StockPath))
                    {
                        BinaryFormatter f = new BinaryFormatter();

                        List<Product> g = f.Deserialize(fileStream) as List<Product>;
                        listProduct = g;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public Product FindProductsToEdit(string ProductReference)
        {
            if (!this.listProduct.Any())
            {
                List();
            }

            Product u = this.listProduct.FirstOrDefault(x => x.Reference.ToLower() == ProductReference.ToLower());

            if (u != null)
            {
                return u;
            }

            return null;
        }

        public Product FindProduct(string ProductName)
        {

            foreach (Product c in this.listProduct)
            {
                if (c.Name.ToLower().Equals(ProductName.ToLower()))
                {
                    return c;
                }
            }

            return null;
        }

        public bool RemoveProduct(string ProductReference)
        {
            Product productToRemove = FindProductsToEdit(ProductReference);

            if (productToRemove != null)
            {
                this.listProduct.Remove(productToRemove);
                CleanFile();
                foreach (var item in this.listProduct)
                {
                    SaveToTxt();
                }
                return true;
            }

            return false;
        }

        public bool EditProduct(Product productToEdit, long UserId)
        {

            try
            {
                if (productToEdit != null)
                {
                    if (RemoveFromProduct(productToEdit.Id))
                    {
                        productToEdit.Updated = DateTime.Now;
                        productToEdit.UpdatedBy = UserId;

                        this.listProduct.Add(productToEdit);
                        SaveToTxt();
                        List();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddProduct(Product p)
        {
            try
            {
                if (listProduct.Any(x => x.Id == p.Id))
                {
                    return false;
                }
                else
                {
                    listProduct.Add(p);
                    SaveToTxt();
                    List();
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        public bool RemoveFromProduct(long ProductId)
        {
            Product productToRemove = this.listProduct.Where(x => x.Id == ProductId).FirstOrDefault();

            if (productToRemove != null)
            {
                this.listProduct.Remove(productToRemove);
                CleanFile();
                SaveToTxt();
                return true;
            }

            return false;
        }

        public bool CleanFile()
        {
            try
            {
                using (TextWriter tw = new StreamWriter(StockPath))
                {
                    tw.Write("");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SaveToTxt()
        {

            if (File.Exists(StockPath))
            {
                using (FileStream fileStream = File.Create(StockPath))
                {
                    BinaryFormatter f = new BinaryFormatter();
                    f.Serialize(fileStream, listProduct);
                }
            }
        }

        #endregion
    }
}
