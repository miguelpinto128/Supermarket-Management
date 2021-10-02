using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using work.Data;
using work.Enums;

namespace work.Screens
{
    class MenuBill
    {
        public static void Menu(Employee sessionUser)
        {
            int response = -1;
            Console.Clear();
            Header();

            EntityBill listEntityBill = new EntityBill();
            listEntityBill.List();

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
                        billList(listEntityBill);
                        break;
                    case 2:
                        selectBill(listEntityBill);
                        break;
                    default:
                        break;
                }
            }
        }
        private static void Header()
        {
            Console.WriteLine("--------------------------   Venda   ---------------------------");
            Console.WriteLine("#######################################################################");
            Console.WriteLine("#  1 - Ver Lista de Faturas                                           #");
            Console.WriteLine("#  2 - Ver Fatura (PDF)                                               #");
            Console.WriteLine("#  9 - Sair                                                           #");
            Console.WriteLine("#######################################################################");
        }

        private static void billList(EntityBill listEntityBill)
        {
            Console.WriteLine("Deseja ordenar? (1-Sim) : (2-Nao)");
            int response_ = 0;
            int response = 0;
            Int32.TryParse(Console.ReadLine(), out response_);
            response = response_;
            while (response == 0)
            {
                Console.WriteLine("#ERRO: Deseja ordenar? (1-Sim) : (2-Nao)");
                Int32.TryParse(Console.ReadLine(), out response_);
                response = response_;
            }

            Tuple<int, List<string>> list = listEntityBill.GetData(response == 1 ? true : false );

            ToolsTable.Print(list.Item1, list.Item2);
        }

        private static void selectBill(EntityBill listEntityBill)
        {
            Console.WriteLine("Insira o numero da fatura:");
            long response_ = 0;
            long nmr = 0;
            Int64.TryParse(Console.ReadLine(), out response_);
            nmr = response_;
            if (nmr > 0)
            {
                listEntityBill.openPDF(nmr);
            }
        }
    }
}
