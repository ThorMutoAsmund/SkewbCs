using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkewbCs
{
    class Program
    {








        static void Main(string[] args)
        {
            var s = new SkewbSolver();

            s.Dist();
            var ov2 = s.LayDist();
            var idx = 0;
            Console.WriteLine();
            Console.WriteLine("Result");
            foreach (var kk in ov2)
            {
                Console.WriteLine($"{idx++} {ov2.Length}");
            }
            Console.ReadLine();
        }
    }
}
