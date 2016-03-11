using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Lab1
{
    class dfs
    {
        /// <summary>
        /// Класс,реализующий алгоритм поиска в глубину
        /// </summary>
        Graph graph;
        bool[] used;
        Stack<int> path;

        public dfs(Graph g)
        {
            this.graph = g;
            path = new Stack<int>();
            used = new bool[graph.n];
        }

        public void Trace(int p)//Выполнить поиск в глубину от указанной вершины с выводом результата по каждой операции
        {
            DFS(p);
        }

        private void DFS(int v)//Алгоритм поиска в глубину
        {
            used[v] = true;
            path.Push(v);//Сохраняем текущую
            System.Console.Write("Текущая вершина - {0},содержание стека:",v + 1);
            foreach (int i in path)
                System.Console.Write(i + 1 + " ");
            System.Console.WriteLine();

            foreach (int i in graph.verteses[v].adjances)
                if (used[i] == false)
                {
                    DFS(i);
                    path.Pop();//Извлекаем проверенные
                }

        }
    }
}
