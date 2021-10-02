using NLog.Internal;
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
    class EntityEmployees
    {
        #region Properties
        public List<Employee> listEmployees;
        public string Path = Directory.GetCurrentDirectory() + ConfigurationManager.AppSettings["EmployeesPath"];

        #endregion

        #region  Constructor
        public EntityEmployees()
        {
            this.listEmployees = new List<Employee>();
        }
        #endregion

        #region Methods
        /// <summary>method <c>GetData</c> Este método é responsavel por returnar uma lista de string e tambem por fazer um simples pesquisa.</summary>
        public Tuple<int, List<string>> GetData(string firstName = "", string lastName = "", string email = "")
        {
            List();
            List<string> list = new List<string>();
            list.Add("EMAIL|TIPO DE FUNCIONARIO|PRIMEIRO NOME|ULTIMO NOME|CONTACTO|MORADA|DATA DE NASCIMENTO");
            int size = 0;

            try
            {
                if (listEmployees.Any())
                {
                    List<Employee> newList = this.listEmployees.ToList();
                  
                    newList = newList.Where(x =>
                         (!string.IsNullOrEmpty(firstName) ? x.firstName.ToLower() == firstName.ToLower() : x.firstName.ToLower() != "") &&
                         (!string.IsNullOrEmpty(lastName) ? x.lastName.ToLower() == lastName.ToLower() : x.lastName.ToLower() != "") &&
                         (!string.IsNullOrEmpty(email) ? x.email.ToLower() == email.ToLower() : x.email.ToLower() != "")
                    ).ToList();

                    if (newList.Any())
                    {
                        foreach (var item in newList)
                        {
                            string row = $"{item.email}|{item.type}|{item.firstName}|{item.lastName}|{(item.contact > 0 ? item.contact.ToString() : string.Empty)}|{item.address}|{(item.birthDate != new DateTime() ? item.birthDate.ToString("dd/MM/yyyy") : string.Empty)}";
                            list.Add(row);
                            if (row.Length > size)
                            {
                                size = row.Length;
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
            catch (Exception ex)
            {
                string row = "|||Nenhum resultado encontrado (0)|||";
                list.Add(row);
                size = row.Length;

            }

            return new Tuple<int, List<string>>(size, list);
        }

        /// <summary>method <c>MakeLogin</c> Este método é responsavel realizar o login.</summary>
        public Employee MakeLogin(string Email, string Password)
        {
            try
            {
                return FindEmployeesToLogin(Email, Password);
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>method <c>FindEmployeesToEdit</c> Este método é responsavel procurar um empregado pelo seu email.</summary>
        public Employee FindEmployeesToEdit(string email)
        {
            if (!this.listEmployees.Any())
            {
                List();
            }

            Employee u = this.listEmployees.FirstOrDefault(x => x.email.ToLower() == email.ToLower());

            if (u != null)
            {
                return u;
            }

            return null;
        }

        /// <summary>method <c>FindEmployeesToLogin</c> Este método é responsavel procurar um empregado pelo seu email e password.</summary>
        public Employee FindEmployeesToLogin(string Email, string Password)
        {
            if (!this.listEmployees.Any())
            {
                List();
            }

            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
            {
                string hash = GetHash(Email.ToLower(), Password);
                Employee u = this.listEmployees.FirstOrDefault(x => x.authHash == hash);

                if (u != null)
                {
                    return u;
                }
            }
            return null;
        }

        /// <summary>method <c>GetHash</c> Este método é responsavel por gerar uma hash.</summary>
        public string GetHash(string login, string passsWord)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(string.Concat(login, passsWord)));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
 
        public bool EditEmployee(Employee employeeToEdit, long userId)
        {
            try
            {
                List();

                employeeToEdit.Updated = DateTime.Now;
                employeeToEdit.UpdatedBy = userId;
                if (employeeToEdit != null)
                {
                    if (removeFromEmployees(employeeToEdit.id))
                    {
                        this.listEmployees.Add(employeeToEdit);
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

        public bool AddEmployee(Employee e)
        {
            try
            {
                if (listEmployees.Any(x => x.id == e.id))
                {
                    return false;
                }
                else
                {
                    listEmployees.Add(e);

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

        public bool removeFromEmployees(long Id)
        {
            try
            {
                Employee contactoToRemove = this.listEmployees.Where(x => x.id == Id).FirstOrDefault();

                if (contactoToRemove != null)
                {
                    this.listEmployees.Remove(contactoToRemove);
                    CleanFile();

                    SaveToTxt();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool CleanFile()
        {
            try
            {
                using (TextWriter tw = new StreamWriter(Path))
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

        public void List()
        {
            try
            {
                listEmployees.Clear();
                if (File.Exists(Path))
                {
                    FileStream fileStream = File.OpenRead(Path);
                    BinaryFormatter f = new BinaryFormatter();


                    List<Employee> g = f.Deserialize(fileStream) as List<Employee>;
                    listEmployees = g;

                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void SaveToTxt()
        {

            if (File.Exists(Path))
            {

                FileStream fileStream = File.Create(Path);
                BinaryFormatter f = new BinaryFormatter();

                f.Serialize(fileStream, listEmployees);
                fileStream.Close();
            }
        }
        #endregion

    }
}
