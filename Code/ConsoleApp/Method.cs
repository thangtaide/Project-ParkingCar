using System;
namespace ConsoleApp
{
    class method
    {
        public string ChuanHoa(string s)
        {
            s = s.Trim().ToLower();
            while (s.Contains("  "))
            {
                s.Replace("  ", " ");
            }
            string[] s1 = s.Split(" ");
            s = "";
            foreach (string item in s1)
            {
                s += item.Substring(0, 1).ToUpper() + item.Substring(1) + " ";
            }
            return s.TrimEnd();
        }
    }
}