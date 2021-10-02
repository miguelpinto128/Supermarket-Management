using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Markdig.Renderers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using work.Enums;

namespace work.Data
{
    [Serializable]
    class EntityBill
    {
        #region Properties
        public List<Bill> listBill;
        public string BillPath = Directory.GetCurrentDirectory() + ConfigurationManager.AppSettings["BillPath"];

        #endregion

        #region  Constructor
        public EntityBill()
        {
            this.listBill = new List<Bill>();
        }
        #endregion

        #region Methods
        public Tuple<int, List<string>> GetData(bool order)
        {
            List();
            List<string> list = new List<string>();
            list.Add("nmr FATURA|NOME CLIENTE|PRODUTO|REFERENCIA|QUANTIDADE|PRECO|NOME FUNCIONARIO");
            int size = 0;

            try
            {
                if (listBill.Any())
                {
                    var listAux = order == true ? listBill.OrderByDescending(x=>x.products.Sum(y=>y.Price)).ToList() : listBill.ToList();
                    foreach (var item in listAux)
                    {
                        string row = "||||||";
                        list.Add(row);
                        row = $"{item.id}|{item.clientName}|||||{item.employeeName}";
                        list.Add(row);
                        foreach (var p in item.products)
                        {
                            row = $"||{p.Product.Name}|{p.Product.Reference}|{p.Quantity}|{p.Price}|";
                            list.Add(row);
                        }
                        size = item.id.ToString().Length * 4;
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

        public void createPDF(Bill bill)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<header class='clearfix'>");
            sb.Append("<h1>IPCA SHOT 0,50 CENT</h1>");
            sb.Append("<div id='company' class='clearfix'>");
            sb.Append("<div>Migas & Bruno, Lda</div>");
            sb.Append("<div>455 Famas,<br /> RC 85004, Tuga</div>");
            sb.Append("<div>(602) 519-0450</div>");
            sb.Append("<div><a href='mailto:ipca@example.com'>ipca@geral.com</a></div>");
            sb.Append("</div>");
            sb.Append("<div id='project'>");
            sb.Append("<div><span>PROJECT</span> Shop c#</div>");
            sb.Append("<div><br><div>");
            sb.Append("<div><br><div>");
            sb.Append($"<div><span>CLIENTE</span> {bill.clientName} </div>");
            sb.Append("<div><br><div>");
            sb.Append($"<div><span>DATA</span> {DateTime.Now.ToString("dd/MM/yyyy")} </div>");
            sb.Append("</div>");
            sb.Append("</header>");
            sb.Append("<main>");
            sb.Append("<div><br><div>");
            sb.Append("<div><br><div>");
            sb.Append("<div><br><div>");
            sb.Append("<table>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th class='service'>#</th>");
            sb.Append("<th class='desc'>PRODUTO</th>");
            sb.Append("<th>REFERENCIA</th>");
            sb.Append("<th>QUANTIDADE</th>");
            sb.Append("<th>PREÇO</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 0;
            foreach (var item in bill.products)
            {
                ++i;
                sb.Append("<tr>");
                sb.Append($"<td class='service'>#{i}</td>");

                sb.Append($"<td class='service'>{item.Product.Name}</td>");

                sb.Append($"<td class='service'>{item.Product.Reference}</td>");
                sb.Append($"<td class='desc'>{item.Quantity}</td>");
                sb.Append($"<td class='unit'>{item.Price} €</td>");
                sb.Append("</tr>");
            }


            sb.Append("<tr>");
            sb.Append("<td colspan='4' class='grand total'>TOTAL</td>");
            sb.Append($"<td class='grand total'>{ bill.products.Select(x => x.Price).Sum()} €</td>");
            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");

            StringReader sr = new StringReader(sb.ToString());

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                htmlparser.Parse(sr);
                pdfDoc.Close();

                byte[] bytes = memoryStream.ToArray();
                File.WriteAllBytes($"bill_{bill.id}.PDF", bytes);

                memoryStream.Close();
            }
        }

        public void openPDF(long nmr)
        {
            string path = Directory.GetCurrentDirectory() + $"\\bill_{nmr}.PDF";
            if (!File.Exists(path))
            {
                Console.WriteLine("Não existe o ficheiro");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Black;
            Process cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();

            cmd.StandardInput.WriteLine($"explorer.exe bill_{nmr}.PDF");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
            Console.ForegroundColor = ConsoleColor.White;
        }

        public bool AddBill(Employee employee, List<ProductSell> products, string clientName)
        {
            try
            {
                List();
                Bill b = new Bill(employee.firstName, products, clientName);

                if (listBill.Any(x => x.id == b.id))
                {
                    return false;
                }
                else
                {
                    listBill.Add(b);
                    SaveToTxt();
                    createPDF(b);
                    List();
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        public void List()
        {
            try
            {
                listBill.Clear();
                if (File.Exists(BillPath))
                {
                    using (FileStream fileStream = File.OpenRead(BillPath))
                    {
                        BinaryFormatter f = new BinaryFormatter();


                        List<Bill> g = f.Deserialize(fileStream) as List<Bill>;
                        listBill = g;

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void SaveToTxt()
        {
            using (FileStream fileStream = File.Create(BillPath))
            {
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize(fileStream, listBill);
            }
        }

        #endregion
    }
}
