using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using work.Data;
using work.Enums;

namespace work.Screens
{
    class MenuRepositor
    {
        #region Menu
        public static void Menu(Employee sessionUser)
        {
            int response = -1;

            Console.Clear();
            Header();
            EntityProduct listEntityProduct = new EntityProduct();
            listEntityProduct.List();

            while (response != 9)
            {
                Console.WriteLine("Insira uma opção");
                int response_ = -1;
                Int32.TryParse(Console.ReadLine(), out response_);
                response = response_;
                while (response == 0)
                {
                    Console.WriteLine("#ERRO: OPÇÃO INVALIDA \n");
                    Console.WriteLine("Insira uma opção");
                    Int32.TryParse(Console.ReadLine(), out response_);
                    response = response_;
                }

                Console.Clear();
                Header();
                switch (response)
                {
                    case 1:
                        ListProduct(listEntityProduct);
                        break;
                    case 2:
                        CreateProduct(listEntityProduct, sessionUser);
                        break;
                    case 3:
                        Search(listEntityProduct);
                        break;
                    case 4:
                        ListProduct(listEntityProduct);
                        EditProduct(listEntityProduct, sessionUser);
                        break;
                    case 5:
                        ClearList(listEntityProduct);
                        break;
                    case 6:
                        RemoveProduct(listEntityProduct);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region Functions
        private static void Header()
        {
            Console.WriteLine("--------------------------   Repositor   ------------------------------");
            Console.WriteLine("#######################################################################");
            Console.WriteLine("#  1 - Ver Stock                                                      #");
            Console.WriteLine("#  2 - Adicionar Produto                                              #");
            Console.WriteLine("#  3 - Procurar Produto                                               #");
            Console.WriteLine("#  4 - Editar Produtor                                                #");
            Console.WriteLine("#  5 - Limpar Stock                                                   #");
            Console.WriteLine("#  6 - Remover Produto                                                #");
            Console.WriteLine("#  9 - Sair                                                           #");
            Console.WriteLine("#######################################################################");
        }

        public static void ListProduct(EntityProduct listEntityProduct, bool? isCaixa = false)
        {
            Console.WriteLine("\n#                          Ver Stock                                #");

            Tuple<int, List<string>> list = listEntityProduct.GetData(isCaixa);

            ToolsTable.Print(list.Item1, list.Item2);

        }

        private static void CreateProduct(EntityProduct listEntityProduct, Employee sessionUser)
        {
            Console.WriteLine("\n#                 Adicionar Produto                                 #");

            Console.WriteLine("Insira o Nome do Produto:");
            string ProductName = Console.ReadLine();
            while (string.IsNullOrEmpty(ProductName))
            {
                Console.WriteLine("#ERRO: Insira o Nome do Produto:");
                ProductName = Console.ReadLine();
            }

            Console.WriteLine("Insira Referencia do Produto (min - 10 Carácters):");
            string ProductReference = Console.ReadLine();
            while (string.IsNullOrEmpty(ProductReference) || ProductReference.Length < 10 || listEntityProduct.listProduct.Any(x => x.Reference == ProductReference))
            {
                if (listEntityProduct.listProduct.Any(x => x.Reference == ProductReference))
                {
                    Console.WriteLine("#ERRO: Referencia inserida já existe:");
                }
                Console.WriteLine("#ERRO: Insira Referencia do Produto (min - 10 Carácters):");
                ProductReference = Console.ReadLine();
            }

            Console.WriteLine("Insira o Tipo de Produto (1) - Congelados | (2) - Prateleira | (3) - Enlatados:");
            int ProductType_ = 0;
            Int32.TryParse(Console.ReadLine(), out ProductType_);
            int ProductType = ProductType_;
            List<int> ProductTypes = new List<int> { 1, 2, 3 };
            while (ProductType == 0 || !ProductTypes.Contains(ProductType))
            {
                Console.WriteLine("#ERRO: Insira o Tipo de Produto (1) - Congelados | (2) - Prateleira | (3) - Enlatados :");
                Int32.TryParse(Console.ReadLine(), out ProductType_);
                ProductType = ProductType_;
            }

            Console.WriteLine("Está Disponivel ? (1) - Disponivel | (2) - Indisponivel:");
            int ProductAvailability_ = 0;
            Int32.TryParse(Console.ReadLine(), out ProductAvailability_);
            int ProductAvailability = ProductAvailability_;
            List<int> Availabilities = new List<int> { 1, 2 };
            while (ProductAvailability == 0 || !Availabilities.Contains(ProductAvailability))
            {
                Console.WriteLine("#ERRO: Está Disponivel ? (1) - Disponivel | (2) - Indisponivel:");
                Int32.TryParse(Console.ReadLine(), out ProductAvailability_);
                ProductAvailability = ProductAvailability_;
            }


            Console.WriteLine("Insira quantidade existente do produto:");
            string auxQuantity = Console.ReadLine();
            int Quantity_ = 0;
            int Quantity = 0;
            Int32.TryParse(auxQuantity, out Quantity_);
            Quantity = Quantity_;
            while (string.IsNullOrEmpty(auxQuantity) || Quantity == 0)
            {
                Console.WriteLine("#ERRO: Insira quantidade existente do produto");
                auxQuantity = Console.ReadLine();
                Int32.TryParse(auxQuantity, out Quantity_);
                Quantity = Quantity_;
            }


            Console.WriteLine("Insira a Data de Validade:");
            string auxExpiryDate = Console.ReadLine();
            DateTime ExpiryDate_ = new DateTime();
            DateTime ExpiryDate = new DateTime();
            DateTime.TryParse(auxExpiryDate, out ExpiryDate_);
            ExpiryDate = ExpiryDate_;
            while ((auxExpiryDate != "" && (ExpiryDate == new DateTime() || (ExpiryDate <= DateTime.Now)) ))
            {
                Console.WriteLine("#ERRO: Insira a Data de Validade (dd/MM/yyyy):");
                auxExpiryDate = Console.ReadLine();
                DateTime.TryParse(auxExpiryDate, out ExpiryDate_);
                ExpiryDate = ExpiryDate_;
            }

            Console.WriteLine("Insira o preço do produto:");
            string auxPrice = Console.ReadLine();
            decimal Price_ = 0;
            decimal Price = 0;
            Decimal.TryParse(auxPrice, out Price_);
            Price = Price_;
            while (string.IsNullOrEmpty(auxPrice) || Price == 0)
            {
                Console.WriteLine("#ERRO: Insira o preço do produto:");
                auxPrice = Console.ReadLine();
                Decimal.TryParse(auxPrice, out Price_);
                Price = Price_;
            }

            Stock stock = new Stock(Quantity);
            Product p = new Product(ProductName, ProductReference, (EnumTypeProduct)ProductType, (EnumProductAvailability)ProductAvailability, ExpiryDate, Price, sessionUser.id, stock);

            if (listEntityProduct.AddProduct(p))
            {
                Console.WriteLine($"# O Produto {p.ToString()} foi criado com sucesso");
            }
            else
            {
                Console.WriteLine($"#ERRO: houve erro ao criar o Produto");
            }
        }

        private static void EditProduct(EntityProduct listEntityProduct, Employee sessionUser)
        {
            Console.WriteLine("\n#                   Editar Produtor                                 #");

            Console.WriteLine("Insira a referencia do produto que pretende editar:");
            string reference = Console.ReadLine();
            while (string.IsNullOrEmpty(reference))
            {
                Console.WriteLine("Insira a referencia do produto que pretende editar:");
                reference = Console.ReadLine();
            }

            Product Product = listEntityProduct.FindProductsToEdit(reference);

            if (Product == null)
            {
                Console.WriteLine("#ERRO: Prodtuto não encontrado");
            }
            else
            {
                #region campos

                Console.WriteLine("Insira o Nome do Produto:");
                Product.Name = Console.ReadLine();
                while (string.IsNullOrEmpty(Product.Name))
                {
                    Console.WriteLine("#ERRO: Insira o Nome do Produto:");
                    Product.Name = Console.ReadLine();
                }

                Console.WriteLine("Insira Referencia do Produto (min - 10 Carácters):");
                Product.Reference = Console.ReadLine();
                while (string.IsNullOrEmpty(Product.Reference) || Product.Reference.Length < 10 || listEntityProduct.listProduct.Any(x => x.Reference == Product.Reference && x.Id != Product.Id))
                {
                    if (listEntityProduct.listProduct.Any(x => x.Reference == Product.Reference))
                    {
                        Console.WriteLine("#ERRO: Referencia inserida já existe:");
                    }
                    Console.WriteLine("#ERRO: Insira Referencia do Produto (min - 10 Carácters):");
                    Product.Reference = Console.ReadLine();
                }

                Console.WriteLine("Insira o Tipo de Produto (1) - Congelados | (2) - Prateleira | (3) - Enlatados:");
                int ProductType_ = 0;
                Int32.TryParse(Console.ReadLine(), out ProductType_);
                Product.Type = (EnumTypeProduct)ProductType_;
                List<int> ProductTypes = new List<int> { 1, 2, 3 };
                while (Product.Type == 0 || !ProductTypes.Contains((int)Product.Type))
                {
                    Console.WriteLine("#ERRO: Insira o Tipo de Produto (1) - Congelados | (2) - Prateleira | (3) - Enlatados :");
                    Int32.TryParse(Console.ReadLine(), out ProductType_);
                    Product.Type = (EnumTypeProduct)ProductType_;
                }

                Console.WriteLine("Está Disponivel ? (1) - Disponivel | (2) - Indisponivel:");
                int ProductAvailability_ = 0;
                Int32.TryParse(Console.ReadLine(), out ProductAvailability_);
                Product.Availability = (EnumProductAvailability)ProductAvailability_;
                List<int> Availabilities = new List<int> { 1, 2 };
                while (Product.Availability == 0 || !Availabilities.Contains((int)Product.Availability))
                {
                    Console.WriteLine("#ERRO: Está Disponivel ? (1) - Disponivel | (2) - Indisponivel:");
                    Int32.TryParse(Console.ReadLine(), out ProductAvailability_);
                    Product.Availability = (EnumProductAvailability)ProductAvailability_;
                }


                Console.WriteLine("Insira quantidade existente do produto:");
                string auxQuantity = Console.ReadLine();
                int Quantity_ = 0;
                Int32.TryParse(auxQuantity, out Quantity_);
                Product.Stock.Quantity = Quantity_;
                while (string.IsNullOrEmpty(auxQuantity) || Product.Stock.Quantity == 0)
                {
                    Console.WriteLine("#ERRO: Insira quantidade existente do produto");
                    auxQuantity = Console.ReadLine();
                    Int32.TryParse(auxQuantity, out Quantity_);
                    Product.Stock.Quantity = Quantity_;
                }


                Console.WriteLine("Insira a Data de Validade:");
                string auxExpiryDate = Console.ReadLine();
                DateTime ExpiryDate_ = new DateTime();
                DateTime.TryParse(auxExpiryDate, out ExpiryDate_);
                while (auxExpiryDate != "" && Product.ExpiryDate == new DateTime())
                {
                    Console.WriteLine("#ERRO: Insira a Data de Validade (dd/MM/yyyy):");
                    auxExpiryDate = Console.ReadLine();
                    DateTime.TryParse(auxExpiryDate, out ExpiryDate_);
                    Product.ExpiryDate = ExpiryDate_;
                }
                if (string.IsNullOrEmpty(auxExpiryDate))
                {
                    Product.ExpiryDate = new DateTime();
                }
                Console.WriteLine("Insira o preço do produto:");
                string auxPrice = Console.ReadLine();
                decimal Price_ = 0;
                Decimal.TryParse(auxPrice, out Price_);
                Product.Price = Price_;
                while (string.IsNullOrEmpty(auxPrice) || Product.Price == 0)
                {
                    Console.WriteLine("#ERRO: Insira o preço do produto:");
                    auxPrice = Console.ReadLine();
                    Decimal.TryParse(auxPrice, out Price_);
                    Product.Price = Price_;
                }
                #endregion

                if (listEntityProduct.EditProduct(Product, sessionUser.id))
                {
                    Console.WriteLine($"# O Produto {Product.ToString()} foi editado com sucesso");
                }
                else
                {
                    Console.WriteLine($"#ERRO: houve erro ao editar o Produto");
                }
            }
        }

        public static void Search(EntityProduct listEntityProduct, bool? isCaixa = false)
        {
            Console.WriteLine("\n#                  Procurar Produto                                 #");

            Console.WriteLine("Nome do Produto:");
            string ProductName = Console.ReadLine();


            Console.WriteLine("Referencia do Produto:");
            string ProductReference = Console.ReadLine();


            Tuple<int, List<string>> list = listEntityProduct.GetData(isCaixa, ProductName, ProductReference);

            ToolsTable.Print(list.Item1, list.Item2);
        }

        private static void ClearList(EntityProduct listEntityProduct)
        {
            Console.WriteLine("\n#                    Limpar Stock                                   #");

            if (listEntityProduct.CleanFile())
            {
                Console.WriteLine($"# A lista foi limpa com sucesso");
            }
            else
            {
                Console.WriteLine($"#ERRO: houve erro ao limpar a lista");
            }
        }

        private static void RemoveProduct(EntityProduct listEntityProduct)
        {
            Console.WriteLine("\n#                   Remover Produto                                 #");

            Console.WriteLine("Insira a referencia do produto que pretende editar:");
            string reference = Console.ReadLine();
            while (string.IsNullOrEmpty(reference))
            {
                Console.WriteLine("Insira a referencia do produto que pretende editar:");
                reference = Console.ReadLine();
            }

            Product Product = listEntityProduct.FindProductsToEdit(reference);

            if (Product == null)
            {
                Console.WriteLine("#ERRO: Prodtuto não encontrado");
            }
            else
            {
                if (listEntityProduct.RemoveFromProduct(Product.Id))
                {
                    Console.WriteLine($"# O Produto com a referencia {Product.Reference} foi apagado com sucesso");
                }
                else
                {
                    Console.WriteLine($"#ERRO: houve erro ao apagar o Produto");
                }
            }
        }
        #endregion
    }
}