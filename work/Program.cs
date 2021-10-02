using System;
using System.Collections.Generic;
using work.Data;
using work.Enums;
using work.Screens;

namespace work
{
    class Program
    {
        public Employee SessionUser;
        static void Main(string[] args)
        {
            Console.WriteLine("------------------------------ Bem Vindo ------------------------------");

            InitMenu.initMenu();
            //EntityEmployees listEntityEmployees = new EntityEmployees();
            //CreateEmployee(listEntityEmployees);

        }
        private static void CreateEmployee(EntityEmployees listEntityEmployees)
        {
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

            //while (string.IsNullOrEmpty(email) || !validateEmail(email) || listEntityEmployees.listEmployees.Any(x => x.email == email))
            //{
            //    if (listEntityEmployees.listEmployees.Any(x => x.email == email)) { Console.WriteLine("#ERRO: Email já registado insira um novo:"); }
            //    else { Console.WriteLine("#ERRO (email invalido): Insira o Email do Funcionário:"); }
            //    email = Console.ReadLine();
            //}


            Console.WriteLine("Insira a Palavra-Passe do Funcionário (min - 8 Carácters):");
            string passWord = Console.ReadLine();
            //while (string.IsNullOrEmpty(passWord) || passWord.Length < 8)
            //{
            //    Console.WriteLine("#ERRO: Insira a Palavra-Passe do Funcionário (min - 8 Carácters):");
            //    passWord = Console.ReadLine();
            //}

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
            while ((!string.IsNullOrEmpty(auxContact) && contact <= 0))
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

            Employee e = new Employee(email, (EnumTypeEmployee)type, firstName, lastName, address, contact, birthDate, initWork, salary, passWord, 0);

            if (listEntityEmployees.AddEmployee(e))
            {
                Console.WriteLine($"# O Funcionário {string.Concat(firstName, " ", lastName)} foi criado com sucesso");
            }
            else
            {
                Console.WriteLine($"#ERRO: houve erro ao criar o Funcionário");
            }

        }
    }
}
