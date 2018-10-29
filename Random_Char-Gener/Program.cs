using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB_1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0].Equals("?")) Console.WriteLine("Введите имя входного файла, имя выходного файла, первый символ для генерации, кол-во символов для диапозона и кол-во генерируемых символов\nПример:LB_1 Input.txt Output.txt A 5 1000 ");
            else
            {
                char[] chars = new char[Convert.ToInt32(args[3])];
                char firstChar = Convert.ToChar(args[2]);
                for (int i = 0; i < chars.Length; i++)
                {
                    chars[i] = Convert.ToChar(firstChar + i);
                }
                try
                {
                    Generation generation = new Generation(args[0], chars, Convert.ToInt32(args[4]));
                    generation.PrintChances();
                    generation.WriteChar(args[1]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
