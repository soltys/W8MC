using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMC.Service.REPL
{
    class Program
    {
        private const string Prompt = "WMC> ";
        private const string ExitCommand = "/quit";
        private const string FileCommand = "/file";
        private const string HelpCommand = "/help";
        private const string ClearCommand = "/clear";
        private const string ClipboardCommand = "/clipboard";
        private static Evaluator _evaluator;
        static StringBuilder sb = new StringBuilder();
        [STAThread]
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
                if (command.StartsWith("/"))
                {
                    if (processSpecialCommand(command)) break;
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

        private static bool processSpecialCommand(string command)
        {
            if (command == ExitCommand)
            {
                return true;
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
            }
            if (command == ClipboardCommand)
            {
                _evaluator = new Evaluator(sb.ToString());
                var text =_evaluator.Interpret();
                Clipboard.SetText(text);
            }
            if (command == ClearCommand)
            {
                sb.Clear();
            }
            if (command == HelpCommand)
            {
                printHelp();
            }
            return false;
        }

        private static void printHelp()
        {
            Console.WriteLine("?> ns Namespace.Name - Set namespace for class [optional]");
            Console.WriteLine("?> c ClassName - Create internal class [class must be after namespace]");
            Console.WriteLine("?> pc ClassName - Create public class [class must be after namespace]");
            Console.WriteLine("?> i PropertyName - Create int property [after declaring class]");
            Console.WriteLine("?> s PropertyName - Create string property [after declaring class]");
            Console.WriteLine("?> d PropertyName - Create double property [after declaring class]");
            Console.WriteLine("?> f PropertyName - Create float property [after declaring class]");
            Console.WriteLine("?> dt PropertyName - Create DateTime property [after declaring class]");
            Console.WriteLine("?> ts PropertyName - Create TimeSpan property [after declaring class]");
            Console.WriteLine("?>");
            Console.WriteLine("?> /help - this message");
            Console.WriteLine("?> /clear - clears buffer");
            Console.WriteLine("?> /clipboard - output to clipboard");
            Console.WriteLine("?> /file filename.cs - output to file");
            Console.WriteLine("?> /quit - Quit program");
        }
    }
}
