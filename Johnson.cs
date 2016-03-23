using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Lab1
{
    /// <summary>
    /// Class implementing Johnson's algorithm
    /// </summary>
    class Johnson
    {
        Graph g;
        bool hasNCycle;
        public Johnson(Graph g)
        {
            this.g = g;
        }

        /// <summary>
        /// Build Distance matrix by Johnson's algorithm
        /// </summary>
        /// <returns>Two-dimensional array of distances between vertices</returns>
        public int[,] DistMatr()
        {
            var singlesp = Bellman(0);
            if (!hasNCycle)
            {
                for (int i = 0; i < g.n; i++)
                {
                    for (int j = 0; j < g.edges.Count(); j++)
                        g.edges[j].w = g.edges[j].w + singlesp[g.edges[j].n1] - singlesp[g.edges[j].n2];//Reweighting graph
                    return Dijkstra();
                }
            }
            else throw new Exception("Граф имеет отрицательные циклы");
            return null;
        }

        private int[] Bellman(int s)
        {
            int[] d = new int[g.n];
            int[] p = new int[g.n];
            for (int i = 0; i < g.n; i++)
            {//Заполнение массива максимальными значениями
                d[i] = Int32.MaxValue;
                p[i] = -1;
            }
            d[s] = 0;
                for (int j = 0; j < g.m; j++)
                    if (d[g.edges[j].n1] < Int32.MaxValue)
                        if (d[g.edges[j].n2] > d[g.edges[j].n1] + g.edges[j].w)
                        {
                            d[g.edges[j].n2] = d[g.edges[j].n1] + g.edges[j].w;
                            p[g.edges[j].n2] = g.edges[j].n1;
                        }
            for (int j = 0; j < g.m; j++)
                if (d[g.edges[j].n1] < Int32.MaxValue)
                    if (d[g.edges[j].n2] > d[g.edges[j].n1] + g.edges[j].w)
                        hasNCycle = true;

            return d;
        }

        private int[,] Dijkstra()
        {
            int[,] d = new int[g.n, g.n];
            for (int k = 0; k < g.n; k++)// Итерация алгоритма для каждой вершины
            {
                int count, index = 0, i, u, m = k + 1;
                int[] distance = new int[g.n];
                bool[] visited = new bool[g.n];
                for (i = 0; i < g.n; i++)
                {
                    distance[i] = Int32.MaxValue;
                    visited[i] = false;
                }
                distance[k] = 0;
                for (count = 0; count < g.n - 1; count++)
                {
                    int min = Int32.MaxValue;
                    for (i = 0; i < g.n; i++)
                        if (!visited[i] && distance[i] <= min)
                        {
                            min = distance[i]; index = i;
                        }
                    u = index;
                    visited[u] = true;
                    for (i = 0; i < g.n; i++)
                        if (!visited[i] && g.AdjMatr[u, i] != 0 && distance[u] != Int32.MaxValue &&
                        distance[u] + g.AdjMatr[u, i] < distance[i])
                            distance[i] = distance[u] + g.AdjMatr[u, i];

                    for (i = 0; i < g.n; i++) d[k, i] = distance[i];
                }
            }
            //Замена всех максимальных значений на 0
            for (int i = 0; i < d.GetLength(0); i++)
                for (int j = 0; j < d.GetLength(1); j++) if (d[i, j] == Int32.MaxValue) d[i, j] = 0;
            return d;
        }
    }
}