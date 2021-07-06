using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekz
{
    public class Ekz
    {
        //source - симплекс таблица без базисных переменных
        double[,] SimplexTable; //симплекс таблица
        int m, n;
        List<int> ListBasisP; //List c базисными переменными
        /// <summary>
        /// Работа с фиктивными и базисными переменными
        /// </summary>
        /// <param name="source"></param>
        public Ekz(double[,] source)
        {
            m = source.GetLength(0);
            n = source.GetLength(1);
            SimplexTable = new double[m, n + m - 1];
            ListBasisP = new List<int>();
            // Добавление фиктивных переменных
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < SimplexTable.GetLength(1); j++)
                {
                    if (j < n)
                        SimplexTable[i, j] = source[i, j];
                    else
                        SimplexTable[i, j] = 0;
                }
                //выставляем коэффициент 1 перед базисной переменной в строке, это для правильного выстраивания фиктивных переменных
                if ((n + i) < SimplexTable.GetLength(1))
                {
                    SimplexTable[i, n + i] = 1;
                    ListBasisP.Add(n + i);
                }
            }
            n = SimplexTable.GetLength(1);
        }
        /// <summary>
        /// Подсчет симплекс таблицы
        /// </summary>
        /// <param name="result">Массив для вывода результата</param>
        /// <returns></returns>
        public double[,] Calculate(double[] result)
        {
            int ResultCol; //столбцы результатов
            int ResultRow; //строки результатов
            while (!IsItEnd())
            {
                ResultCol = findResultCol();
                ResultRow = findResultRow(ResultCol);
                ListBasisP[ResultRow] = ResultCol;
                double[,] new_SimplexTable = new double[m, n];
                for (int j = 0; j < n; j++)
                    new_SimplexTable[ResultRow, j] = SimplexTable[ResultRow, j] / SimplexTable[ResultRow, ResultCol];
                for (int i = 0; i < m; i++)
                {
                    if (i == ResultRow)
                        continue;
                    for (int j = 0; j < n; j++)
                        new_SimplexTable[i, j] = SimplexTable[i, j] - SimplexTable[i, ResultCol] * new_SimplexTable[ResultRow, j];
                }
                SimplexTable = new_SimplexTable;
            }
            //заносим в result найденные значения X
            for (int i = 0; i < result.Length; i++)
            {
                int k = ListBasisP.IndexOf(i + 1);
                if (k != -1)
                    result[i] = SimplexTable[k, 0];
                else
                    result[i] = 0;
            }
            return SimplexTable;
        }
        /// <summary>
        /// остановка программы, если строка оценок меньше 0
        /// </summary>
        /// <returns></returns>
        private bool IsItEnd() 
        {
            bool flag = true;
            for (int j = 1; j < n; j++)
            {
                if (SimplexTable[m - 1, j] < 0)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        /// <summary>
        /// Нахождение разрешающей строки
        /// </summary>
        /// <param name="ResultCol">Результат нахождения расрешающей строки</param>
        /// <returns></returns>
        private int findResultRow(int ResultCol)
        {
            int ResultRow = 0;
            for (int i = 0; i < m - 1; i++)
                if (SimplexTable[i, ResultCol] > 0)
                {
                    ResultRow = i;
                    break;
                }
            for (int i = ResultRow + 1; i < m - 1; i++)
                if ((SimplexTable[i, ResultCol] > 0) && ((SimplexTable[i, 0] / SimplexTable[i, ResultCol]) < (SimplexTable[ResultRow, 0] / SimplexTable[ResultRow, ResultCol])))
                    ResultRow = i;
            Debug.WriteLine("Разрешающая строка: " + ResultRow);
            return ResultRow;
        }
        /// <summary>
        /// Нахождение разрешающего столбца
        /// </summary>
        /// <returns></returns>
        private int findResultCol()//Ищем разрешающую столбец
        {
            int ResultCol = 1;
            for (int j = 2; j < n; j++)
                if (SimplexTable[m - 1, j] < SimplexTable[m - 1, ResultCol])
                    ResultCol = j;
            Debug.WriteLine("Разрешающий столбец: "+ ResultCol);
            return ResultCol;
        }
        
    }
    public class vvodZnach
    {
        public double[,] mas;
        public double[] bufMass = { };
        public double[,] ResultTable;
        /// <summary>
        /// Метод ввода и вывода данных
        /// </summary>
        public void simplexBol()
        {
            double[] ms1 = { };
            string str1 = "";
            int raz1 = 0, d = 0;
            //Запись из csv в массив
            try
            {
                using (StreamReader sr = new StreamReader(@"Ввод.csv"))
                {
                    sr.ReadLine();
                    str1 = sr.ReadToEnd();
                    string[] st = str1.Split('\n');
                    raz1 = st.Length;
                    ms1 = Array.ConvertAll(st[0].Split(';'), double.Parse);
                    d = ms1.Length;
                    mas = new double[raz1, d];
                    for (int i = 0; i < raz1; i++)
                    {
                        ms1 = Array.ConvertAll(st[i].Split(';'), double.Parse);
                        for (int j = 0; j < d; j++)
                        {
                            mas[i, j] = ms1[j];

                        }
                    }

                    // Меняем первый и последний столбцы местами для того что бы удобно вводить в csv файл ограничения
                    for (int i = 0; i < raz1; i++)
                    {
                        for (int j = 0; j < d; j += d - 1)
                        {
                            double tmp = mas[i, j];
                            mas[i, j] = mas[i, d - 1];
                            mas[i, d - 1] = tmp;
                        }

                    }
                    // делаем строку оценок отрицательной для корректного вывода
                    for (int i = 0; i < raz1; i++)
                    {
                        for (int j = 0; j < d; j++)
                        {
                            if (i == raz1 - 1)
                            {
                                mas[i, j] = mas[i, j] * (-1);
                            }
                        }
                    }
                    Console.WriteLine("Входная матрица");                 
                    for (int i = 0; i < raz1; i++)
                    {
                        for (int j = 0; j < d; j++)
                        {
                            Console.Write($"{mas[i, j],5}");
                        }
                        Console.WriteLine();
                    }
                }

                //Объявляем массив размерностью в два раза больше, чем введенный массив для фиктивных переменных
                double[] result = new double[raz1 * 2];
                //Конструктор класса
                Ekz S = new Ekz(mas);
                //Основной метод программы
                ResultTable = S.Calculate(result);
                for (int i = 0; i < ResultTable.GetLength(0); i++)
                {
                    for (int j = 0; j < ResultTable.GetLength(1); j++)
                    {
                        if (i == raz1 - 1)
                        {
                            ResultTable[i, j] = ResultTable[i, j] * (-1);
                        }
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("Результат:");                
                for (int i = 0; i < ResultTable.GetLength(0); i++)
                {
                    for (int j = 0; j < ResultTable.GetLength(1); j++)
                        Console.Write($"{Math.Round(ResultTable[i, j]),5}" + ";");
                    Console.WriteLine("");
                }
                int ind1 = 1;
                for (int j = d - 2; j >= 0; j--)
                {
                    Console.WriteLine("X[{0}] = {1}", ind1, result[j]);
                    ind1++;
                }
                Console.WriteLine("F = " + (ResultTable[ResultTable.GetLength(0) - 1, 0] * -1));
                Console.WriteLine("F' = " + (ResultTable[ResultTable.GetLength(0) - 1, 0]));
                using (StreamWriter sw = new StreamWriter(@"Вывод.csv"))
                {
                    sw.WriteLine("reshenie:");
                    for (int i = 0; i < ResultTable.GetLength(0); i++)
                    {
                        for (int j = 0; j < ResultTable.GetLength(1); j++)
                            sw.Write($"{Math.Round(ResultTable[i, j]),5}" + ";");
                        sw.WriteLine();
                    }
                    ind1 = 1;
                    for (int j = d - 2; j >= 0; j--)
                    {
                        sw.WriteLine("X[{0}] = {1}", ind1, result[j]);
                        ind1++;
                    }
                    sw.WriteLine("F = " + (ResultTable[ResultTable.GetLength(0) - 1, 0] * -1));
                    sw.WriteLine("F' = " + (ResultTable[ResultTable.GetLength(0) - 1, 0]));
                }
            }
            catch
            {
                Console.WriteLine("Программа не может прочитать файл, проверте правильность введенных данных");
            }
        }
    }
    class Program
    {
        /// <summary>
        /// Главный метод программы
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new TextWriterTraceListener(File.CreateText("Промежуточные.txt")));
            Debug.AutoFlush = true;
            vvodZnach vz = new vvodZnach();
            vz.simplexBol();
            Console.ReadKey();
        }
    }
}
