using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic
{
    class Program
    {
        static void Main(string[] args)
        {
            Number number = new Number(3, -9);
            Console.WriteLine(number.ToString());
            Console.Read();
        }
        
        
    }
}
