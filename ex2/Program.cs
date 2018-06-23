using System;
using System.Collections.Generic;
using System.IO;

namespace ex2
{
    public class FileInOut
    {
        private string[] _obj;
        private int _position;

        public FileInOut(string name)
        {
            var reader = new StreamReader(name + ".txt");
            var strings = reader.ReadToEnd();
            _obj = strings.Split('\n', ' ');

            reader.Close();
        }

        public int NextCount()
        {
            int buf = Convert.ToInt32(_obj[_position++]);
            if (buf > 10 || buf < 2)
                return 0;
            else
                return buf;
        }

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
        public static List<string> FileRead()
        {
            var input = new FileInOut("input");
            int n = input.NextCount();
            List<string> words = new List<string>();
            for (int i = 0; i < n * 2; i++)
                words.Add(input.Next(n));
            return words;
        }

        public static void Find(List<string> rem)
        {
            int n = rem[0].Length;
            for (int fi = 0; fi < rem.Count; fi++)
            {
                for (int si = 0; si < rem.Count; si++)
                {
                    if (fi == si)
                    {
                        continue;
                    }
                    string tmpFirstStr = rem[fi];
                    string tmpSecondStr = rem[si];
                    List<string> copyRem = new List<string>(rem);
                    List<string> first = new List<string>();
                    List<string> second = new List<string>();
                    bool found = true;

                    copyRem.RemoveAt(Math.Max(si, fi));
                    copyRem.RemoveAt(Math.Min(si, fi));

                    first.Add(tmpFirstStr);
                    second.Add(tmpSecondStr);
                    string prefF = tmpFirstStr.Substring(1, 1);
                    string prefS = tmpSecondStr.Substring(1, 1);


                    FindNextWord(prefF, prefS, ref first, ref second, ref copyRem, ref found, n);
                    if (found == true)
                        return;
                    else
                        continue;
                }
            }
        }

        public static void FindNextWord(string tmpFirstStr, string tmpSecondStr, ref List<string> first, ref List<string> second, ref List<string> copyRem, ref bool found, int n)
        {
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
                int pos = first.Count;
                bool ok = false;
                for (int i = 0; i < copyRem.Count; i++)
                {
                    if (tmpFirstStr == copyRem[i].Substring(0, pos))
                    {
                        first.Add(copyRem[i]);
                        copyRem.RemoveAt(i);
                        ok = true;
                        break;
                    }
                }
                if (ok == false)
                {
                    found = false;
                    return;
                }
                for (int i = 0; i < copyRem.Count; i++)
                {
                    if (tmpSecondStr == copyRem[i].Substring(0, pos))
                    {
                        second.Add(copyRem[i]);
                        copyRem.RemoveAt(i);
                        break;
                    }
                }
                if (first.Count == second.Count)
                {
                    string prefF = null;
                    string prefS = null;
                    if (first.Count == n)
                        FindNextWord(prefF, prefS, ref first, ref second, ref copyRem, ref found, n);
                    else
                    {
                        for (int i = 0; i < first.Count; i++)
                            prefF = prefF + first[i].Substring(pos + 1, 1);
                        for (int i = 0; i < second.Count; i++)
                            prefS = prefS + second[i].Substring(pos + 1, 1);
                        FindNextWord(prefF, prefS, ref first, ref second, ref copyRem, ref found, n);
                    }

                }
                else found = false;
            }
        }

        static void Main(string[] args)
        {
            var words = FileRead();
            Find(words);
        }
    }
}
