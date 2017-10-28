using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsShell.Dialogs
{
    public static class StringExt
    {
        public static string RemoveFromEnd(this string s, string suffix)
        {
            if (s.EndsWith(suffix))
            {
                return s.Substring(0, s.Length - suffix.Length);
            }
            else
            {
                return s;
            }
        }
    }
}
