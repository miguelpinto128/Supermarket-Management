using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using work.Data;
using work.Enums;

namespace work.Screens
{
    class MenuCaixa
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
                int response_ = 0;
                Int32.TryParse(Console.ReadLine(), out response_);
                response = response_;
                while (response == 0)
                {
                    Console.WriteLine("#ERRO: Insira uma opção");
                    Int32.TryParse(Console.ReadLine(), out response_);
                    response = response_;
                }

                Console.Clear();
                Header();
                switch (response)
                {
                    case 1:
                        newSell(listEntityProduct, sessionUser);
                        break;
                    case 2:
                        MenuRepositor.ListProduct(listEntityProduct,true);
                        break;
                    case 3:
                        MenuRepositor.Search(listEntityProduct, true);
                        break;
                    case 4:
                        Troco();
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
            Console.WriteLine("--------------------------   Venda   ---------------------------");
            Console.WriteLine("#######################################################################");
            Console.WriteLine("#  1 - Nova Venda                                                     #");
            Console.WriteLine("#  2 - Ver Produtos                                                   #");
            Console.WriteLine("#  3 - Procurar Produto                                               #");
            Console.WriteLine("#  4 - Troco                                                          #");
            Console.WriteLine("#  9 - Sair                                                           #");
            Console.WriteLine("#######################################################################");
        }


        private static void newSell(EntityProduct listEntityProduct, Employee sessionUser)
        {
            Console.WriteLine("\n#                            Nova Venda                             #\n");

            MenuRepositor.ListProduct(listEntityProduct, true);
            Console.WriteLine("\n");

            List<ProductSell> Listsell = new List<ProductSell>();

            int response_ = 0;
            int response = 0;
            decimal finalPrice = 0;
            do
            {
                int quant_ = 0;
                int quant = 0;
                Console.WriteLine("Degite a referencia do produto:");
                string productId = Console.ReadLine();
                if (listEntityProduct.listProduct.Any(X => X.Reference == productId))
                {
                    Product Product = listEntityProduct.listProduct.FirstOrDefault(X => X.Reference == productId);
                    Console.WriteLine("Insira a quantidade:");
                    Int32.TryParse(Console.ReadLine(), out quant_);
                    quant = quant_;
                    while (quant == 0 || (Product.Stock.Quantity - quant) < 0)
                    {
                        if (Product.Stock.Quantity - quant <= 0) { Console.WriteLine("#ERRO: Quantidade inexistente:"); }
                        Console.WriteLine("#ERRO: Insira a quantidade:");
                        Int32.TryParse(Console.ReadLine(), out quant_);
                        quant = quant_;
                    }

                    ProductSell sell = new ProductSell(Product, quant, quant * Product.Price);

                    Listsell.Add(sell);

                    finalPrice = finalPrice + (Product.Price * quant);
                }
                else
                {
                    Console.WriteLine("#ERRO: Produto não encontrado");
                }
                Console.WriteLine("Insira uma opção: (1 - Adicionar produto) | (9 : Concluir)");
                Int32.TryParse(Console.ReadLine(), out response_);
                response = response_;
                while (response == 0)
                {
                    Console.WriteLine("#ERRO: Insira uma opção: (1 - Adicionar produto) | (9 : Concluir)");
                    Int32.TryParse(Console.ReadLine(), out response_);
                    response = response_;
                }

            } while (response != 9 || response == 1);

            if (Listsell.Any())
            {
                List<string> listAux = new List<string>();
                listAux.Add("|Nome do Produto|Referencia|Quantidade|Preço/Quantidade");
                int len = 0;
                int i = 0;
                foreach (ProductSell item in Listsell)
                {
                    string row = $"#{++i}|{item.Product.Name}|{item.Product.Reference}|{item.Quantity}|{item.Price}";
                    listAux.Add(row);
                    if (len < row.Length) len = row.Length * 2;
                }
                listAux.Add($"Total a pagar||||{finalPrice}");
                ToolsTable.Print(len, listAux);

                int responseAux_ = 0;
                Console.WriteLine("Deseja finalizar a compra? (1 - SIM) | (2 - NÃO)");
                Int32.TryParse(Console.ReadLine(), out responseAux_);
                int responseAux = responseAux_;
                while (responseAux == 0 || responseAux < 1 || responseAux > 2)
                {
                    Console.WriteLine("#ERRO: Deseja finalizar a compra? (1 - SIM) | (2 - NÃO)");
                    Int32.TryParse(Console.ReadLine(), out responseAux_);
                    responseAux = responseAux_;
                }
                if (responseAux == 1)
                {
                    try
                    {
                        foreach (ProductSell sell in Listsell)
                        {
                            sell.Product.Stock.Quantity = sell.Product.Stock.Quantity - sell.Quantity;
                            if (sell.Product.Stock.Quantity <= 0)
                            {
                                sell.Product.Availability = EnumProductAvailability.Indisponivel;
                            }
                            sell.Product.Availability = sell.Product.Stock.Quantity <= 0 ? EnumProductAvailability.Indisponivel : sell.Product.Availability;
                            listEntityProduct.EditProduct(sell.Product, sessionUser.id);
                        }
                        EntityBill bill = new EntityBill();
                        Console.WriteLine("Insira o nome do cliente: ");
                        var clientName = Console.ReadLine();
                        bill.AddBill(sessionUser, Listsell, clientName);
                     

                        Console.WriteLine(" \n # Compra efetuada com sucesso \n");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("#ERRO: Ao realizar a compra \n");
                    }
                }
                else
                {
                    Console.WriteLine("#ERRO: Compra cancelada \n");
                }
            }
            else
            {
                Console.WriteLine("#ERRO: Não ha produtos na lista de compras \n");
            }
        }

        private static decimal Troco()
        {
            Console.WriteLine("\n#                              Troco                                #\n");

            decimal nmr1_ = 0;
            Console.WriteLine("Insira o montante recebido:");
            string aux = Console.ReadLine();
            decimal.TryParse(aux, out nmr1_);
            decimal nmr1 = nmr1_;
            while (nmr1 == 0 && !string.IsNullOrEmpty(aux))
            {
                Console.WriteLine("#ERRO: Insira o montante recebido:");
                decimal.TryParse(Console.ReadLine(), out nmr1_);
                nmr1 = nmr1_;
            }

            decimal nmr2_ = 0;
            Console.WriteLine("Insira o valor da compra:");
            string aux2 = Console.ReadLine();
            decimal.TryParse(aux2, out nmr2_);
            decimal nmr2 = nmr2_;
            while (nmr1 == 0 && !string.IsNullOrEmpty(aux))
            {
                Console.WriteLine("#ERRO: Insira o valor da compra:");
                decimal.TryParse(Console.ReadLine(), out nmr2_);
                nmr2 = nmr2_;
            }

            if (nmr1 - nmr2 < 0  )
            {
                Console.WriteLine($"Estão a faltar {(nmr1 - nmr2) * -1 } euros! \n");
            }
            else
            {
                Console.WriteLine($"Troco de {(nmr1 - nmr2)} euros!! \n");
            }
            return 0;
        }
        #endregion
    }
}
