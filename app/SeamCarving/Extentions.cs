using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving
{
    public static class Extentions
    {
        public static void Push<T>(this List<T> list, T el)
        {
            if (list.Capacity == list.Count)
            {
                list.RemoveAt(list.Count - 1);
            }

            list.Insert(0, el);
        }

        public static T Pop<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default(T);

            T el = list[0];
            list.RemoveAt(0);
            return el;
        }
        public static List<bool> ToBoolList(this string s)
        {
            return s.Select(a => a == '0' ? false : true).ToList();
        }

        public static BitArray ToBitArray(this string s)
        {
            List<bool> list = s.Select(a => a == '0' ? false : true).ToList();          
            return new BitArray(list.ToArray());
        }

        public static List<bool> ToList(this BitArray array)
        {
            List<bool> list = new List<bool>();
            for(int i = 0; i< array.Count; i++)
            {
                list.Add(array[i]);
            }
            return list;
        }

        public static string ToStr(this List<bool> list)
        {
           return String.Concat(list.Select(a => a ? '1' : '0'));
        }

        public static string ToStr(this bool b)
        {
            return b ? "1" : "0";
        }
    }
}
