using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    public class Convert2
    {
        public static int NumberFromString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            string onlyNumber = new string(input.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(onlyNumber))
                return 0;

            return int.Parse(onlyNumber);
        }
    }
}
