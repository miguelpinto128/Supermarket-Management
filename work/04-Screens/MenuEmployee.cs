using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using work.Data;
using work.Enums;

namespace work.Screens
{
    class MenuEmployee
    {

        #region Menu
        public static void Menu(Employee sessionUser)
        {
            int response = -1;
            Console.Clear();
            Header();
            EntityEmployees listEntityEmployees = new EntityEmployees();
            listEntityEmployees.List();

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
                        ListEmployees(listEntityEmployees);
                        break;
                    case 2:
                        CreateEmployee(listEntityEmployees, sessionUser);
                        break;
                    case 3:
                        Search(listEntityEmployees);
                        break;
                    case 4:
                        EditEmployee(listEntityEmployees, sessionUser);
                        break;
                    case 5:
                        ClearList(listEntityEmployees);
                        break;
                    case 6:
                        RemoveEmployee(listEntityEmployees);
                        break;
                    case 7:
                        EditProfil(listEntityEmployees, sessionUser);
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
            Console.WriteLine("--------------------------   Funcionários   ---------------------------");
            Console.WriteLine("#######################################################################");
            Console.WriteLine("#  1 - Ver lista                                                      #");
            Console.WriteLine("#  2 - Gravar Funcionários                                            #");
            Console.WriteLine("#  3 - Procurar                                                       #");
            Console.WriteLine("#  4 - Editar                                                         #");
            Console.WriteLine("#  5 - Limpar Lista                                                   #");
            Console.WriteLine("#  6 - Remover Funcionário                                            #");
            Console.WriteLine("#  7 - Editar Perfil                                                  #");
            Console.WriteLine("#  9 - Sair                                                           #");
            Console.WriteLine("#######################################################################");
        }

        private static void ListEmployees(EntityEmployees listEntityEmployees)
        {
            Console.WriteLine("\n#                          Ver lista                                  #\n");

            Tuple<int, List<string>> list = listEntityEmployees.GetData();

            ToolsTable.Print(list.Item1, list.Item2);
        }

        private static void CreateEmployee(EntityEmployees listEntityEmployees, Employee sessionUser)
        {
            Console.WriteLine("\n#                         Gravar Funcionários                         #\n");

            #region camppos
            Console.WriteLine("Insira o Primeiro Nome do Funcionário:");
            string firstName = Console.ReadLine();
            while (string.IsNullOrEmpty(firstName))
            {
                Console.WriteLine("#ERRO: Insira o Primeiro Nome do Funcionário:");
                firstName = Console.ReadLine();
            }

            Console.WriteLine("Insira o Segundo Nome do Funcionário:");
            string lastName = Console.ReadLine();
            while (string.IsNullOrEmpty(lastName))
            {
                Console.WriteLine("#ERRO: Insira o Segundo Nome do Funcionário:");
                lastName = Console.ReadLine();
            }

            Console.WriteLine("Insira o Email do Funcionário");
            string email = Console.ReadLine();

            while (string.IsNullOrEmpty(email) || !validateEmail(email) || listEntityEmployees.listEmployees.Any(x => x.email == email))
            {
                if (listEntityEmployees.listEmployees.Any(x => x.email == email)) { Console.WriteLine("#ERRO: Email já registado insira um novo:"); }
                else { Console.WriteLine("#ERRO (email invalido): Insira o Email do Funcionário:"); }
                email = Console.ReadLine();
            }


            Console.WriteLine("Insira a Palavra-Passe do Funcionário (min - 8 Carácters):");
            string passWord = Console.ReadLine();
            while (string.IsNullOrEmpty(passWord) || passWord.Length < 8)
            {
                Console.WriteLine("#ERRO: Insira a Palavra-Passe do Funcionário (min - 8 Carácters):");
                passWord = Console.ReadLine();
            }

            Console.WriteLine("Insira o Tipo do Funcionário (1) - Gerente | (2) - Caixa | (3) - Repositor:");
            int type_ = 0;
            Int32.TryParse(Console.ReadLine(), out type_);
            int type = type_;
            List<int> types = new List<int> { 1, 2, 3 };
            while (type == 0 || !types.Contains(type))
            {
                Console.WriteLine("#ERRO: Insira o Tipo do Funcionário (1) - Gerente | (2) - Caixa | (3) - Repositor:");
                Int32.TryParse(Console.ReadLine(), out type_);
                type = type_;
            }

            Console.WriteLine("Insira a Morada do Funcionário:");
            string address = Console.ReadLine();

            Console.WriteLine("Insira a Número de telefone do Funcionário:");
            string auxContact = Console.ReadLine();
            long contact_ = 0;
            long contact = 0;
            Int64.TryParse(auxContact, out contact_);
            contact = contact_;
            while ((!string.IsNullOrEmpty(auxContact) && (contact <= 0 || contact.ToString().Length != 9 || (!contact.ToString().StartsWith("91") && !contact.ToString().StartsWith("92") && !contact.ToString().StartsWith("93") && !contact.ToString().StartsWith("96")))))
            {
                Console.WriteLine("#ERRO: Insira a Número de telefone do Funcionário");
                auxContact = Console.ReadLine();
                Int64.TryParse(auxContact, out contact_);
                contact = contact_;
            }

            Console.WriteLine("Insira a Data de nascimento do Funcionário (dd/MM/yyyy)");
            string auxbirthDate = Console.ReadLine();
            DateTime birthDate_ = new DateTime();
            DateTime birthDate = new DateTime();
            DateTime.TryParse(auxbirthDate, out birthDate_);
            birthDate = birthDate_;
            int birthDateDif = Convert.ToInt32(DateTime.Now.Year) - Convert.ToInt32((int)birthDate.Year);
            while (auxbirthDate != "" && (birthDate == new DateTime() || birthDateDif < 18))
            {
                if (birthDateDif < 18) { Console.WriteLine("#ERRO: O Funcionário tem de ter +18 anos"); }
                Console.WriteLine("#ERRO: Insira a Data de nascimento do Funcionário (dd/MM/yyyy)");
                auxbirthDate = Console.ReadLine();
                DateTime.TryParse(auxbirthDate, out birthDate_);
                birthDate = birthDate_;
                birthDateDif = Convert.ToInt32(DateTime.Now.Year) - Convert.ToInt32((int)birthDate.Year);
            }


            Console.WriteLine("Insira a data de quando o Funcionário começou a trabalhar (dd/MM/yyyy)");
            string auxinitWork = Console.ReadLine();
            DateTime initWork_ = new DateTime();
            DateTime initWork = new DateTime();
            DateTime.TryParse(auxinitWork, out initWork_);
            initWork = initWork_;
            while (auxinitWork != "" && (initWork == new DateTime() || (initWork < birthDate)))
            {
                Console.WriteLine("#ERRO: Insira a data de quando o Funcionário começou a trabalhar (dd/MM/yyyy)");
                auxinitWork = Console.ReadLine();
                DateTime.TryParse(auxinitWork, out initWork_);
                initWork = initWork_;
            }

            Console.WriteLine("Insira o Salario do Funcionário");
            string auxsalary = Console.ReadLine();
            int salary_ = 0;
            int salary = 0;
            Int32.TryParse(auxsalary, out salary_);
            salary = salary_;
            while (!string.IsNullOrEmpty(auxsalary) && salary == 0)
            {
                Console.WriteLine("#ERRO: Insira o Salario do Funcionário");
                auxsalary = Console.ReadLine();
                Int32.TryParse(auxsalary, out salary_);
                salary = salary_;
            }
            #endregion

            Employee e = new Employee(email, (EnumTypeEmployee)type, firstName, lastName, address, contact, birthDate, initWork, salary, passWord, sessionUser.id);

            if (listEntityEmployees.AddEmployee(e))
            {
                Console.WriteLine($"# O Funcionário {e.ToString()} foi criado com sucesso");
            }
            else
            {
                Console.WriteLine($"#ERRO: houve erro ao criar o Funcionário");
            }

        }

        private static void EditEmployee(EntityEmployees listEntityEmployees, Employee sessionUser)
        {
            Console.WriteLine("\n#                            Editar                                   #\n");

            Console.WriteLine("Insira o email do Funcionario que pretende editar");
            string email = Console.ReadLine();
            while (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Insira o email do Funcionario que pretende editar");
                email = Console.ReadLine();
            }

            Employee employee = listEntityEmployees.FindEmployeesToEdit(email);

            if (employee == null)
            {
                Console.WriteLine("#ERRO: Funcionário não encontrado");
            }
            else
            {
                #region campos
                Console.WriteLine("Insira o Primeiro Nome do Funcionário:");
                employee.firstName = Console.ReadLine();
                while (string.IsNullOrEmpty(employee.firstName))
                {
                    Console.WriteLine("#ERRO: Insira o Primeiro Nome do Funcionário:");
                    employee.firstName = Console.ReadLine();
                }

                Console.WriteLine("Insira o Segundo Nome do Funcionário:");
                employee.lastName = Console.ReadLine();
                while (string.IsNullOrEmpty(employee.lastName))
                {
                    Console.WriteLine("#ERRO: Insira o Segundo Nome do Funcionário:");
                    employee.lastName = Console.ReadLine();
                }

                Console.WriteLine("Insira o Tipo do Funcionário (1) - Gerente | (2) - Caixa | (3) - Repositor:");
                int type_ = 0;
                Int32.TryParse(Console.ReadLine(), out type_);
                employee.type = (EnumTypeEmployee)type_;
                List<int> types = new List<int> { 1, 2, 3 };
                while (employee.type == 0 || !types.Contains((int)employee.type))
                {
                    Console.WriteLine("#ERRO: Insira o Tipo do Funcionário (1) - Gerente | (2) - Caixa | (3) - Repositor:");
                    Int32.TryParse(Console.ReadLine(), out type_);
                    employee.type = (EnumTypeEmployee)type_;
                }

                Console.WriteLine("Insira a Morada do Funcionário:");
                employee.address = Console.ReadLine();

                Console.WriteLine("Insira a Número de telefone do Funcionário:");
                string auxContact = Console.ReadLine();
                long contact_ = 0;
                Int64.TryParse(auxContact, out contact_);
                employee.contact = contact_;

                while ((!string.IsNullOrEmpty(auxContact) && (employee.contact <= 0 || employee.contact.ToString().Length != 9 || (!employee.contact.ToString().StartsWith("91") && !employee.contact.ToString().StartsWith("92") && !employee.contact.ToString().StartsWith("93") && !employee.contact.ToString().StartsWith("96")))))
                {
                    Console.WriteLine("#ERRO: Insira a Número de telefone do Funcionário");
                    auxContact = Console.ReadLine();
                    Int64.TryParse(auxContact, out contact_);
                    employee.contact = contact_;
                }


                Console.WriteLine("Insira o Salario do Funcionário");
                string auxsalary = Console.ReadLine();
                int salary_ = 0;
                Int32.TryParse(auxsalary, out salary_);
                employee.salary = salary_;

                while (!string.IsNullOrEmpty(auxsalary) && salary_ == 0)
                {
                    Console.WriteLine("#ERRO: Insira o Salario do Funcionário");
                    auxsalary = Console.ReadLine();
                    Int32.TryParse(auxsalary, out salary_);
                    employee.salary = salary_;
                }
                #endregion

                if (listEntityEmployees.EditEmployee(employee, sessionUser.id))
                {
                    Console.WriteLine($"# O Funcionário {employee.ToString()} foi editado com sucesso");
                }
                else
                {
                    Console.WriteLine($"#ERRO: houve erro ao editar o Funcionário");
                }
            }
        }

        public static void EditProfil(EntityEmployees listEntityEmployees, Employee sessionUser)
        {
            Console.WriteLine("\n#                          Editar Perfil                              #\n");

            Employee employee = sessionUser;

            if (employee == null)
            {
                Console.WriteLine("#ERRO: Funcionário não encontrado");
            }
            else
            {
                #region campos
                Console.WriteLine("Insira o seu Primeiro Nome:");
                employee.firstName = Console.ReadLine();
                while (string.IsNullOrEmpty(employee.firstName))
                {
                    Console.WriteLine("#ERRO: Insira o seu Primeiro Nome:");
                    employee.firstName = Console.ReadLine();
                }

                Console.WriteLine("Insira o seu Segundo Nome:");
                employee.lastName = Console.ReadLine();
                while (string.IsNullOrEmpty(employee.lastName))
                {
                    Console.WriteLine("#ERRO: Insira o seu Segundo Nome:");
                    employee.lastName = Console.ReadLine();
                }

                Console.WriteLine("Insira a sua Morada:");
                employee.address = Console.ReadLine();

                Console.WriteLine("Insira o seu Número de telefone:");
                string auxContact = Console.ReadLine();
                long contact_ = 0;
                Int64.TryParse(auxContact, out contact_);
                employee.contact = contact_;

                while ((!string.IsNullOrEmpty(auxContact) && (employee.contact <= 0 || employee.contact.ToString().Length < 9 || (!employee.contact.ToString().StartsWith("91") && !employee.contact.ToString().StartsWith("92") && !employee.contact.ToString().StartsWith("93") && !employee.contact.ToString().StartsWith("96")))))
                {
                    Console.WriteLine("#ERRO: Insira o seu Número de telefone:");
                    auxContact = Console.ReadLine();
                    Int64.TryParse(auxContact, out contact_);
                    employee.contact = contact_;
                }

                Console.WriteLine("Deseja alterar a palavra-passe? (1-Sim | 2-Não)");
                Console.WriteLine("Insira uma opção");
                int response = 0;
                int response_ = -1;
                Int32.TryParse(Console.ReadLine(), out response_);
                response = response_;
                while (response == 0 || response < 1 || response > 2)
                {
                    Console.WriteLine("#ERRO: OPÇÃO INVALIDA \n");
                    Console.WriteLine("Insira uma opção");
                    Int32.TryParse(Console.ReadLine(), out response_);
                    response = response_;
                }

                if (response == 1)
                {
                    Console.WriteLine("Insira a Palavra-Passe do Funcionário (min - 8 Carácters):");
                    string password = null;
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
                    while (string.IsNullOrEmpty(password) || password.Length < 8)
                    {
                        Console.WriteLine("#ERRO: Insira a Palavra-Passe do Funcionário (min - 8 Carácters):");
                        password = null;
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
                    }
                    string hash = listEntityEmployees.GetHash(employee.email, password);
                    if (!string.IsNullOrEmpty(hash))
                    {
                        employee.authHash = hash;
                    };
                }
                #endregion

                if (listEntityEmployees.EditEmployee(employee, sessionUser.id))
                {
                    Console.WriteLine($"\n# O seu Perfil foi editado com sucesso");
                }
                else
                {
                    Console.WriteLine($"\n#ERRO: houve erro ao editar o seu Perfil");
                }
            }
        }

        private static void RemoveEmployee(EntityEmployees listEntityEmployees)
        {
            Console.WriteLine("\n#                        Remover Funcionário                          #\n");

            Console.WriteLine("Insira o email do Funcionario que pretende editar");
            string email = Console.ReadLine();
            while (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Insira o email do Funcionario que pretende apagar");
                email = Console.ReadLine();
            }

            Employee employee = listEntityEmployees.FindEmployeesToEdit(email);

            if (employee == null)
            {
                Console.WriteLine("#ERRO: Funcionário não encontrado");
            }
            else
            {
                if (listEntityEmployees.removeFromEmployees(employee.id))
                {
                    Console.WriteLine($"# O Funcionário {string.Concat(employee.firstName, employee.lastName)} foi apagado com sucesso");
                }
                else
                {
                    Console.WriteLine($"#ERRO: houve erro ao apagar o Funcionário");
                }
            }
        }

        private static void ClearList(EntityEmployees listEntityEmployees)
        {
            Console.WriteLine("\n#                          Limpar Lista                              #\n");

            if (listEntityEmployees.CleanFile())
            {
                Console.WriteLine($"# A lista foi limpa com sucesso");
            }
            else
            {
                Console.WriteLine($"#ERRO: houve erro ao limpar a lista");
            }
        }

        private static void Search(EntityEmployees listEntityEmployees)
        {
            Console.WriteLine("\n#                           Procurar                                  #\n");

            Console.WriteLine("Primeiro Nome do Funcionário");
            string firstName = Console.ReadLine();


            Console.WriteLine("Segundo Nome do Funcionário");
            string lastName = Console.ReadLine();


            Console.WriteLine("Email do Funcionário");
            string email = Console.ReadLine();

            Tuple<int, List<string>> list = listEntityEmployees.GetData(firstName, lastName, email);

            ToolsTable.Print(list.Item1, list.Item2);
        }

        private static bool validateEmail(string email)
        {
            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                RegexOptions.CultureInvariant | RegexOptions.Singleline);
            bool isValidEmail = regex.IsMatch(email);

            return isValidEmail;
        }
        #endregion
    }
}
