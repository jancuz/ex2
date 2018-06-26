using System;
using System.Collections.Generic;
using System.IO;

namespace ex2
{
    public class FileInOut
    {
        private string[] _obj;
        private int _position;

        // считывание из файла
        public FileInOut(string name)
        {
            var reader = new StreamReader(name + ".txt");
            var strings = reader.ReadToEnd();
            _obj = strings.Split('\n', ' ');

            reader.Close();
        }

        // получение кол-ва элементов в словарном квадрате
        public int NextCount()
        {
            int buf = Convert.ToInt32(_obj[_position++]);
            if (buf > 10 || buf < 2)
                return 0;
            else
                return buf;
        }

        // получение следующего слова квадрата
        public string Next(int n)
        {
            string buf = _obj[_position++];
            if (buf.Length > n)
                return buf.Substring(0, n);
            else
                return buf;
        }
    }

    class Program
    {
        // инициализация
        public static List<string> FileRead()
        {
            var input = new FileInOut("input");
            int n = input.NextCount();
            List<string> words = new List<string>();
            for (int i = 0; i < n * 2; i++)
                words.Add(input.Next(n));
            return words;
        }

        // поиск начал словарных квадратов
        public static void Find(List<string> rem)
        {
            int n = rem[0].Length;
            // полный перебор всех начальных слов квадратов
            for (int fi = 0; fi < rem.Count; fi++)
            {
                for (int si = 0; si < rem.Count; si++)
                {
                    if (fi == si)
                    {
                        continue;
                    }
                    string tmpFirstStr = rem[fi];                   // первое слово первого квадрата
                    string tmpSecondStr = rem[si];                  // первое слово второго квадрата
                    List<string> copyRem = new List<string>(rem);   // создание копии заданных слов
                    List<string> first = new List<string>();        // создание первого квадрата
                    List<string> second = new List<string>();       // создание второго квадрата
                    bool found = true;

                    copyRem.RemoveAt(Math.Max(si, fi));             // удаление первых слов из копии всех слов
                    copyRem.RemoveAt(Math.Min(si, fi));

                    first.Add(tmpFirstStr);                         // добавление первых слов в соответствующие квадраты
                    second.Add(tmpSecondStr);
                    string prefF = tmpFirstStr.Substring(1, 1);     // определение префикса первого слова первого квадрата
                    string prefS = tmpSecondStr.Substring(1, 1);    // определение префикса первого слова второго квадрата

                    // поиск следующих слов словарных квадратов
                    FindNextWord(prefF, prefS, ref first, ref second, ref copyRem, ref found, n);
                    if (found == true)
                        return;
                    else
                        continue;
                }
            }
        }

        // формирование словарных квадратов
        public static void FindNextWord(string tmpFirstStr, string tmpSecondStr, ref List<string> first, ref List<string> second, ref List<string> copyRem, ref bool found, int n)
        {
            // если все слова распределены
            // запись в файл словарных квадратов
            if (copyRem.Count == 0)
            {
                StreamWriter f = new StreamWriter("output.txt");
                foreach (var sf in first)
                    f.Write(sf + "\n");
                f.Write("\n");
                foreach (var ss in second)
                    f.Write(ss + "\n");
                f.Close();
            }
            else
            {
                int pos = first.Count;                  // длина подстроки ля сравнения оставшихся слов
                bool ok = false;
                for (int i = 0; i < copyRem.Count; i++) // для каждого слова в копии слов
                {
                    if (tmpFirstStr == copyRem[i].Substring(0, pos)) //префикс = подстроке заданной длины?
                    {
                        first.Add(copyRem[i]);          // добавление слова в первый квадрат 
                        copyRem.RemoveAt(i);            // удаление добавленного слова из копии слов
                        ok = true;
                        break;
                    }
                }
                if (ok == false)                        // если следующее слово для словарного квадрата  не найдено
                {
                    found = false;                      // такой словарный квадрат с заданным первым словом не существует
                    return;
                }
                for (int i = 0; i < copyRem.Count; i++) // для каждого слова в копии слов
                {
                    if (tmpSecondStr == copyRem[i].Substring(0, pos)) //префикс = подстроке заданной длины?
                    {
                        second.Add(copyRem[i]);         // добавление слова во второй квадрат 
                        copyRem.RemoveAt(i);            // удаление добавленного слова из копии слов
                        break;
                    }
                }
                if (first.Count == second.Count)        // если кол-во элементов в обоих квадратах равно
                {
                    string prefF = null;
                    string prefS = null;
                    if (first.Count == n)               // если квадраты полностью заполнены
                        FindNextWord(prefF, prefS, ref first, ref second, ref copyRem, ref found, n); // запись в файл
                    else
                    {
                        // определение нового префикса для первого и второго квадратов
                        for (int i = 0; i < first.Count; i++)
                            prefF = prefF + first[i].Substring(pos + 1, 1);
                        for (int i = 0; i < second.Count; i++)
                            prefS = prefS + second[i].Substring(pos + 1, 1);
                        // поиск следубщих слов квадратов
                        FindNextWord(prefF, prefS, ref first, ref second, ref copyRem, ref found, n);
                    }

                }
                else found = false;
            }
        }

        static void Main(string[] args)
        {
            var words = FileRead(); // инициализация
            Find(words);            // поиск
        }
    }
}
