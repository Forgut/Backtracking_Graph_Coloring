using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backtracking_Graph_Coloring {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("How many vertices?");
            int n = Convert.ToInt32(Console.ReadLine());
            Graph g = new Graph(n);
            g.printMatrix();
            int min = g.ColorGraph();
            Console.Write(min + " colors is enough\n");
            g.printMatrix();
            Console.ReadLine();
        }
    }
}
