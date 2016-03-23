using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Lab1
{
    /// <summary>
    /// Breadth-first search implementation
    /// </summary>
    class bfs
    {
        Graph graph;
        bool[] used;
        Queue<int> q;
        int k;

        public bfs(Graph g)
        {
            this.graph = g;
            used = new bool[graph.n];
            q = new Queue<int>();
            k = 0;
        }

        /// <summary>
        /// BFS graph traversal method
        /// </summary>
        /// <param name="v">Start vertex</param>
        public void BFS(int v)
        {
            used[v] = true;
            q.Enqueue(v);
            while (q.Count !=0)
            {
                k++;
                int t = q.Dequeue();
                if (q.Count != 0 || k == 1) System.Console.Write("Текущая вершина - {0},BFS-номер - {1},содержание очереди:",t + 1,k);
                for (int i = 0;i < graph.n;i++)
                {
                    if (graph.AdjMatr[t,i] != 0 && !used[i])
                    {
                        q.Enqueue(i);//Enqueuing unused,adjacent vertices
                        used[i] = true;
                    }
                }
                foreach (int i in q) System.Console.Write(i + 1 + " ");
                System.Console.WriteLine();
            }
        }
    }
}
