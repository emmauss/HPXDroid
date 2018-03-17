using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HappyPandaXDroid.Helpers
{
    public class StringHelpers
    {
        public static string PascalSplit(string input)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in input)
            {
                if (Char.IsUpper(c))
                {
                    sb.Append(" ");
                }
                sb.Append(c);
            }

            if (input != null && input.Length > 0 && Char.IsUpper(input[0]))
            {
                sb.Remove(0, 1);
            }

            String result = sb.ToString();
            return result;
        }
    }
}