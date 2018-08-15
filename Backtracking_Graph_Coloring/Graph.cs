using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Backtracking_Graph_Coloring {
    class Graph {
        public bool[,] matrix { get; }
        public int[] colors { get; set; }


        public int[][] outputcolors;


        public Graph(int size) {
            matrix = new bool[size, size];
            colors = new int[size];

            outputcolors = new int[size][];
            for (int i = 0; i < size; i++) {
                outputcolors[i] = new int[size];
            }

            Random rand = new Random();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (rand.Next(2) > 0)
                        matrix[i, j] = matrix[j, i] = true;
            for (int i = 0; i < size; i++) {
                colors[i] = -1;
                matrix[i, i] = false;
            }

        }

        public void printMatrix() {
            int size = matrix.GetLength(0);
#if false //prints out matrix
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                    if (matrix[i, j])
                        Console.Write("1 ");
                    else
                        Console.Write("0 ");
                Console.WriteLine();
            }
#endif
            Console.WriteLine();
            foreach (int i in colors) {
                Console.Write(i + " ");
            }
            Console.WriteLine();

        }

        public bool isSafe(int ID, int checkingColor, int[] colors) {
            int size = matrix.GetLength(0);
            for (int i = 0; i < size; i++)
                if (matrix[ID, i] && checkingColor == colors[i])
                    return false;
            return true;
        }

        private bool graphColoring(int ID, int maxColor, int[] inputColors) {
            int[] colors = (int[])inputColors.Clone();
            if (ID == matrix.GetLength(0)) {
                this.outputcolors[maxColor - 1] = colors;
                return true;
            }
            for (int newColor = 0; newColor < maxColor; newColor++) {
                if (isSafe(ID, newColor, colors)) {
                    colors[ID] = newColor;

                    if (graphColoring(ID + 1, maxColor, colors))
                        return true;
                    colors[ID] = -1;
                }
            }
            return false;
        }
        private void oversee(Thread[] tab) {
            while (true) {
                for (int i = 0; i < tab.GetLength(0); i++) {
                    if (!tab[i].IsAlive) {
                        for (int j = i; j < tab.GetLength(0); j++) {
                            tab[j].Abort();
                        }
                    }
                }
                bool allFinished = true;
                foreach (Thread thread in tab) {
                    if (thread.IsAlive) {
                        allFinished = false;
                        break;
                    }
                }
                if (allFinished)
                    break;
            }
        }
        private void checkForMaxColor(int maxColor, bool[] finished, int[] colors) {
            if (graphColoring(0, maxColor, colors)) {
                Console.WriteLine(maxColor + " is enough to color this graph");
                finished[maxColor - 1] = true;
            }
        }
        private void showStatus(Thread[] threads) {
            while (true) {
                bool end = true;
                for (int i = 0; i < threads.GetLength(0); i++) {
                    Console.Write(i + " thread is ");
                    if (threads[i].IsAlive) {
                        Console.WriteLine("running");
                        end = false;
                    }
                    else {
                        Console.WriteLine("finished");
                    }
                }
                if (end)
                    break;
                System.Threading.Thread.Sleep(100);
                Console.Clear();
            }
        }
        public int ColorGraph() {
            bool[] finished = new bool[matrix.GetLength(0)];
            Thread[] tab = new Thread[matrix.GetLength(0)];
            for (int maxColor = matrix.GetLength(0); maxColor > 0; maxColor--) {
                int[] tempColors = new int[matrix.GetLength(0)];
                for (int i = 0; i < matrix.GetLength(0); i++) {
                    tempColors[i] = -1;
                }
                tab[maxColor - 1] = new Thread(() => checkForMaxColor(maxColor, finished, tempColors));
                tab[maxColor - 1].Start();
                System.Threading.Thread.Sleep(10);
            }
            Thread status = new Thread(() => showStatus(tab));
            status.Start();
            Thread overseer = new Thread(() => oversee(tab));
            overseer.Start();
            status.Join();
            for (int i = 0; i < matrix.GetLength(0); i++) {
                if (tab[i].IsAlive)
                    tab[i].Join();
            }
            int min = matrix.GetLength(0);
            for (int i = 0; i < matrix.GetLength(0); i++) {
                if (finished[i]) {
                    min = i;
                    break;
                }
            }
            this.colors = this.outputcolors[min];
            Console.WriteLine("\n");
            return min;
        }
    }
}
