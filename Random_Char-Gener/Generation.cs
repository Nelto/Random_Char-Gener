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
        private double[,] chances;
        private char[] chars;
        private int numOfChars;
        private bool islinear = true;

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
            if (islinear && chars.Length != numOfChance) throw new Exception("Кол-во вероятностей и генерируемых символов не равны 1");
            if(!islinear && pointsInLine[0]!=chars.Length) throw new Exception("Кол-во вероятностей и генерируемых символов не равны 2");
            Pars();
            if (!SumEquelOneLinear() && islinear ) throw new Exception("Сумма вероятностей не равна 1");
            int row = SumEquelOneSquare();
                if (row != -1) throw new Exception("Сумма вероятностей не равна 1 в "+ row +" строке");
        }

        private void ObtChance(StringBuilder build)
        {
            int i = 0;

            for (; i < build.Length - 1; i++)
            {
                if (build[i] == '\n') res.Append(" \n");
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
            DeleteNullRow();
        }

        private void PointCounting()
        {
            int count = 0;
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i] == '.') count++;
                if (res[i] == '\n')
                {
                    pointsInLine.Add(count);
                    numOfChance += count;
                    count = 0;
                }
            }
            if (count > 0) pointsInLine.Add(count);
            numOfChance += count;
        }

        private bool isLinear()
        {
            if (pointsInLine.Count == 1)
            {
                return true;
            }
            for (int i = 0; i < pointsInLine.Count; i++)
                if (pointsInLine[i] != 1)
                {
                    islinear = false;
                    return false;
                }
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
            int j = 0;
            string chance = "";
            chances = new double[pointsInLine[0], pointsInLine[0]];
            for (int i = 0; i < res.Length; i++)
            {
               
                chance += res[i];
                if (res[i] == ' ' && chance.Length > 1)
                {
                    chances[k, j] = Convert.ToDouble(chance);
                    chance = "";
                    j++;
                }
                if (res[i] == '\n')
                {
                    j = 0;
                    k++;
                    chance = "";
                }
            }
        }

         private void DeleteNullRow()
        {
            for (int i = 0; i < res.Length - 2; i++)
            {
                if (res[i] == '\n' && res[i + 2] == '\n') res.Remove(i, 1);
            }
        }

        private bool SumEquelOneLinear()
        {
            double count = 0.0;
            foreach (double c in chances) count += c;
            if (count == 1.0) return true;
            else return false;
        }

        private int SumEquelOneSquare()
        {
            double count = 0.0;
            for (int i = 0; i < pointsInLine.Count; i++)
            {
                count = 0.0;
                for (int j = 0; j < pointsInLine[i]; j++)
                {
                    count += chances[i, j];
                }
                if (count != 1.0) return i;
            }
            return -1;
        }

        private char GenerLinear(double ran)
        {
            double right = 0;
            int i = -1;
            foreach (double c in chances)
            {
                i++;
                right += c;
                if (ran <= right) return chars[i];
            }
            return ' ';
        }

        public void PrintChances()
        {
                Console.Write(res);
        }

        public void WriteChar(string fileOutput)
        {
            using (StreamWriter sw = new StreamWriter(fileOutput))
            {
                Random rand = new Random();
                if(islinear)
                for (int i = 0; i < numOfChars; i++)
                {
                        sw.Write(GenerLinear(rand.NextDouble()));
                }
            }
        }

    }
}
