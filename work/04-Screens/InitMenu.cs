using System;
using System.Collections.Generic;
using System.Text;
using work.Data;
using work.Screens;

namespace work.Screens
{
    class InitMenu
    {
        public static void initMenu()
        {
            int response = -1;

            while (response != 9)
            {
                Console.WriteLine("#######################################################################");
                Console.WriteLine("#  1 - Login                                                          #");
                Console.WriteLine("#  9 - Sair                                                           #");
                Console.WriteLine("#######################################################################");

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

                switch (response)
                {
                    case 1:
                        ScnLogin.Login();
                        break;
                    default:
                        break;
                }
            }
        }
        public static void MenuGerente(Employee sessionUser)
        {
            int response = -1;
       
            while (response != 9)
            {
                Console.Clear();

                Console.WriteLine("------------------------------   Menu   -------------------------------");
                
                Console.WriteLine("#######################################################################");
                Console.WriteLine("#  1 - Funcionarios                                                   #");
                Console.WriteLine("#  2 - Venda                                                          #");
                Console.WriteLine("#  3 - Fatura                                                         #");
                Console.WriteLine("#  9 - Sair                                                           #");
                Console.WriteLine("#######################################################################");

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

                switch (response)
                {
                    case 1:
                        MenuEmployee.Menu(sessionUser);
                        break;
                    case 2:
                        MenuCaixa.Menu(sessionUser);
                        break;
                    case 3:
                        MenuBill.Menu(sessionUser);
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }
        public static void MenuCaixa_(Employee sessionUser)
        {
            int response = -1;

            while (response != 9)
            {
                Console.Clear();

                Console.WriteLine("------------------------------   Menu   -------------------------------");

                Console.WriteLine("#######################################################################");
                Console.WriteLine("#  1 - Venda                                                          #");
                Console.WriteLine("#  2 - Fatura                                                         #");
                Console.WriteLine("#  3 - Editar Perfil                                                  #");
                Console.WriteLine("#  9 - Sair                                                           #");
                Console.WriteLine("#######################################################################");

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
                switch (response)
                {
                    case 1:
                        MenuCaixa.Menu(sessionUser);
                        break;
                    case 2:
                        MenuBill.Menu(sessionUser);
                        break;
                    case 3:
                        EntityEmployees listEntityEmployees = new EntityEmployees();
                        MenuEmployee.EditProfil(listEntityEmployees, sessionUser);
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }
        public static void MenuRepositor_(Employee sessionUser)
        {
            int response = -1;

            while (response != 9)
            {
                Console.Clear();
                Console.WriteLine("------------------------------   Menu   -------------------------------");

                Console.WriteLine("#######################################################################");
                Console.WriteLine("#  1 - Stock                                                          #");
                Console.WriteLine("#  2 - Editar Perfil                                                  #");
                Console.WriteLine("#  9 - Sair                                                           #");
                Console.WriteLine("#######################################################################");

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
                switch (response)
                {
                    case 1:
                        MenuRepositor.Menu(sessionUser);
                        Console.Clear();

                        break;
                    case 2:
                        EntityEmployees listEntityEmployees = new EntityEmployees();
                        MenuEmployee.EditProfil(listEntityEmployees, sessionUser);
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }

    }
}
