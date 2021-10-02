using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using work.Data;
using work.Screens;

namespace work.Data
{
    class ToolsTable
    {
        static private int tableWidth;
        static private List<string> table;


        static public void Print(int tableWidth_, List<string> table_)
        {
            tableWidth = tableWidth_ * 2;
            table = table_;

            PrintLine();
            string head = table.FirstOrDefault();
            PrintRow(head.Split("|"));
            PrintLine();
            foreach (var item in table.Where(x => x != head))
            {
                PrintRow(item.Split("|"));
            }

            PrintLine();
        }

        static private void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static private void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static private string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
