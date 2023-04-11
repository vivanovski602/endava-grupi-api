using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class FirstTestFunction
    {
        public string ReturnsSomethingIfZero(int number)
        {
            if(number == 0)
            {
                return "SOMETHING";
            }
            else
            {
                return "FALSE";
            }
        }

    }
}
