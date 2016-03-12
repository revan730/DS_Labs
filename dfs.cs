using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Lab1
{
    /// <summary>
    /// Класс,реализующий алгоритм поиска в глубину и связанные с ним операции
    /// </summary>
    /// 

    class dfs
    {
        Graph graph;
        bool[] used;
        Stack<int> path;
        int k;//DFS-номер вершины

        public dfs(Graph g)
        {
            this.graph = g;
            path = new Stack<int>();
            used = new bool[graph.n];
            k = 0;
        }

        public void Trace(int p)//Обход графа
        {
            DFS(p,true);
        }

        private void DFS(int v,bool r)//Алгоритм поиска в глубину,если r - режим вывода протокола обхода
        {
            k++;
            used[v] = true;
            path.Push(v);//Сохраняем текущую
            if (r) System.Console.Write("Текущая вершина - {0},DFS-номер - {1},содержание стека:",v + 1,k);
            foreach (int i in path)
               if (r) System.Console.Write(i + 1 + " ");//Вывод содержимого стека
             if (r) System.Console.WriteLine();

            foreach (int i in graph.verteses[v].adjances)
                if (used[i] == false)
                {
                    DFS(i,r);
                     if (r) path.Pop();//Извлекаем проверенные
                }

        }

        public void TopologicalSort()//Топологическая сортировка графа
        {
            if (!graph.Cyclic)
            {
                for (int i = 0; i < graph.n; i++)
                    if (!used[i])
                        DFS(i, false);
                Stack<int> sorted = new Stack<int>();
                while (path.Count != 0)//Инверсия стека для вывода топологической сортировки
                    sorted.Push(path.Pop());
                System.Console.Write("Содержимое стека топологической сортировки: ");
                foreach (int i in sorted)
                    System.Console.Write(i + 1 + " ");
            }
            else System.Console.WriteLine("Граф цикличный,сортировка невозможна");
        }
    }
}
