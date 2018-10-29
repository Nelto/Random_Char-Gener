using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace LB_1
{
    class Generation
    {
        private StringBuilder res = new StringBuilder();
        private int numOfChance = 0;
        private List<int> pointsInLine = new List<int>();
        private double[] chances;
        private char[] chars;
        private int numOfChars;

        public Generation(String fileInput, char[] chars, int numOfChars)
        {
            this.chars = chars;
            this.numOfChars = numOfChars;
            StringBuilder build = new StringBuilder(" ");
            using (StreamReader sr = new StreamReader(fileInput, System.Text.Encoding.Default))
            {
                build.Append(sr.ReadToEnd());
            }
            build.Append(" ");
            build.Replace(",", ".");
            ObtChance(build);
            build.Clear();
            PointCounting();
            if (!isLinear() && !isSquare()) throw new Exception("Не соблюдено условие(Матрица не является квадратной или линейной)");
            if (chars.Length != numOfChance) throw new Exception("Кол-во вероятностей и генерируемых символов не равны ");
            Pars();
            if (!SumEquelOne()) throw new Exception("Сумма вероятностей не равна 1");
        }

        private void ObtChance(StringBuilder build)
        {
            int i = 0;

            for (; i < build.Length - 1; i++)
            {
                if (build[i] == '\n') res.Append('\n');
                if (build[i] == '0' || build[i] == '1')
                {
                    if ((build[i - 1] < '.' || build[i - 1] > '9')
                        && (build[i + 1] < '.' || build[i + 1] > '9')) res.Append(" ").Append(build[i]).Append(".0");
                }
                if (build[i] == '.' && build[i + 1] >= '0' && build[i + 1] <= '9')
                {
                    res.Append(" 0.");
                    i++;
                    while (i < build.Length && build[i] >= ('0') && build[i] <= ('9'))
                    {
                        res.Append(build[i]);
                        i++;
                    }
                }
            }
            res.Append(" ");
        }

        private void PointCounting()
        {
            int count = 0;
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i] == '.') count++;
                if (res[i] == '\n' && count > 0)
                {
                    pointsInLine.Add(count);
                    numOfChance += count;
                    count = 0;
                }
            }
            numOfChance += count;
        }

        private bool isLinear()
        {
            if (pointsInLine.Count == 1) return true;
            for (int i = 0; i < pointsInLine.Count; i++) if (pointsInLine[i] != 1) return false;
            return true;
        }

        private bool isSquare()
        {
            for (int i = 0; i < pointsInLine.Count; i++)
                if (pointsInLine[i] * pointsInLine[i] != numOfChance) return false;
            return true;
        }

        private void Pars()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            int k = 0;
            string chance = "";
            chances = new double[numOfChance];
            for (int i = 1; i < res.Length; i++)
            {
                chance += res[i];
                if (res[i] == ' ')
                {
                    chances[k] = Convert.ToDouble(chance);
                    chance = "";
                    k++;
                }
            }
        }

        private bool SumEquelOne()
        {
            double count = 0.0;
            for (int i = 0; i < chances.Length; i++) count += chances[i];
            if (count == 1.0) return true;
            else return false;
        }

        private int Gener(double ran)
        {
            double right = 0;
            double left = 0;
            for (int i = 0; i < numOfChance; i++)
            {
                right += chances[i];
                if (ran > left && ran <= right) return i;
                left = right;
            }
            return -1;
        }

        public void PrintChances()
        {
            for (int i = 0; i < chances.Length; i++)
                Console.Write(chances[i] + " ");
        }

        public void WriteChar(string fileOutput)
        {
            using (StreamWriter sw = new StreamWriter(fileOutput))
            {
                Random rand = new Random();
                int numChar = -1;
                for (int i = 0; i < numOfChars; i++)
                {
                    numChar = Gener(rand.NextDouble());
                    if (numChar != -1)
                        sw.Write(chars[numChar]);
                }
            }
        }

    }
}
