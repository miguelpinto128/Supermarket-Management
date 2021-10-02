using System;
using System.Collections.Generic;
using System.Text;
using work.Data;
using work.Screens;

namespace work.Screens
{
    class ScnLogin
    {
        public static void Login()
        {
            try
            {
                Console.Clear();

                string email = "", password = "";
                int response = -1;

                Employee sessionUser = null;

                while (response != 9)
                {
                    Console.WriteLine("------------------------------   Login   ------------------------------");
                    Console.WriteLine("Insira o seu Email");
                    email = Console.ReadLine();
                    Console.WriteLine("Insira a sua Palavra-Passe");
                    password = null;
                    ConsoleKey key;
                    do
                    {
                        var keyInfo = Console.ReadKey(intercept: true);
                        key = keyInfo.Key;

                        if (key == ConsoleKey.Backspace && password.Length > 0)
                        {
                            Console.Write("\b \b");
                            password = password[0..^1];
                        }
                        else if (!char.IsControl(keyInfo.KeyChar))
                        {
                            Console.Write("*");
                            password += keyInfo.KeyChar;
                        }
                    } while (key != ConsoleKey.Enter);

                    EntityEmployees login = new EntityEmployees();

                    sessionUser = login.MakeLogin(email, password);

                    if (sessionUser != null)
                    {
                        EntityEmployees list = new EntityEmployees();

                        sessionUser.lastDateLogin = DateTime.Now;

                        list.EditEmployee(sessionUser, 12);

                        response = 9;

                        if(sessionUser.type == Enums.EnumTypeEmployee.Gerente)
                        {
                            InitMenu.MenuGerente(sessionUser);
                        }
                        else if (sessionUser.type == Enums.EnumTypeEmployee.Caixa)
                        {
                            InitMenu.MenuCaixa_(sessionUser);
                        }
                        if (sessionUser.type == Enums.EnumTypeEmployee.Repositor)
                        {
                            InitMenu.MenuRepositor_(sessionUser);
                        }
                    }
                    else
                    {
                        Console.Clear();

                        Console.WriteLine("#ERRO: Email ou Palavra-Passe incorretos # \n");
                        Console.WriteLine("#######################################################################");
                        Console.WriteLine("#  1 - Tentar login novamente                                         #");
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

                        Console.Clear();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
