using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WMC.Service.REPL
{
    class Program
    {
        private const string Prompt = "WMC> ";
        private const string ExitCommand = "/quit";
        private const string FileCommand = "/file";
        private static Evaluator _evaluator;
        static StringBuilder sb = new StringBuilder();
        static void Main(string[] args)
        {
            string command = "";
            do
            {
                Console.Write(Prompt);
                command = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(command))
                {
                    continue;
                }
                command = command.Trim();
                if (command == ExitCommand)
                {
                    break;
                }
                if (command.StartsWith(FileCommand))
                {
                    string[] fileExport = command.Split(' ');
                    if (fileExport.Length != 2)
                    {
                        Console.WriteLine("!> Usage /file filename.cs");
                    }

                    string fileName = fileExport[1];
                    using (StreamWriter streamWriter = new StreamWriter(fileName))
                    {
                        _evaluator = new Evaluator(sb.ToString());
                        streamWriter.Write(_evaluator.Interpret());
                    }


                    continue;
                }

                try
                {
                    sb.Append(command).Append("\n");

                    string text = sb.ToString();
                    _evaluator = new Evaluator(text);
                    Console.Write("->\n" + _evaluator.Interpret() + "\n");
                }
                catch (Exception e)
                {
                    Console.Write("!> " + e.Message + "\n");
                    sb.Clear();
                }


            } while (command != ExitCommand);

        }
    }
}
