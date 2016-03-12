using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Lab1
{
    /// <summary>
    /// Класс,реализующий алгоритм поиска в ширину
    /// </summary>
    /// 

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

        public void Trace(int p)//Выполнить поиск в ширину от указанной вершины с выводом результата по каждой операции
        {
            BFS(p);
        }

        private void BFS(int v)//Алгоритм поиска в ширину
        {
            used[v] = true;
            q.Enqueue(v);//Вставляем вершину в очередь
            while (q.Count !=0)
            {
                k++;
                int t = q.Dequeue();//Достаем вершину из очереди (с удалением)
                if (q.Count != 0 || k == 1) System.Console.Write("Текущая вершина - {0},BFS-номер - {1},содержание очереди:",t + 1,k);
                for (int i = 0;i < graph.n;i++)
                {
                    if (graph.AdjMatr[t,i] == 1 && !used[i])
                    {
                        q.Enqueue(i);//Вставляем в очередь смежные,не посещенные вершины
                        used[i] = true;
                    }
                }
                foreach (int i in q) System.Console.Write(i + 1 + " ");//Вывод содержимого очереди
                System.Console.WriteLine();
            }
        }
    }
}
