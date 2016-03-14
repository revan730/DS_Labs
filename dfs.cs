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

        public void TrDFS(int v)//Алгоритм поиска в глубину для обхода графа
        {
            k++;
            used[v] = true;
            path.Push(v);//Сохраняем текущую
            System.Console.Write("Текущая вершина - {0},DFS-номер - {1},содержание стека:",v + 1,k);
            foreach (int i in path)
               System.Console.Write(i + 1 + " ");//Вывод содержимого стека
             System.Console.WriteLine();

            foreach (int i in graph.verteses[v].adjances)
                if (used[i] == false)
                {
                    TrDFS(i);
                    path.Pop();//Извлекаем проверенные
                }
        }

        private void TopDFS(int v)//Алгоритм поиска в глубину для топологической сортировки
        {
            used[v] = true;
            foreach (int i in graph.verteses[v].adjances)
                if (used[i] == false)
                    TopDFS(i);
            path.Push(v);
        }

        private void CmDFS(int v)//Алгоритм поиска в глубину для поиска компонентов связности
        {
            used[v] = true;
            path.Push(v);
            foreach (int i in graph.verteses[v].adjances)
                if (used[i] == false)
                {
                    CmDFS(i);
                }
        }

        public void FindComps()//Поиск компонентов связности
        {
            for (int i = 0;i < graph.n;i++)
            {
                if (!used[i])
                {
                    path.Clear();
                    CmDFS(i);
                    System.Console.Write("Компонента:");
                    foreach (int c in path)
                        System.Console.Write(c + 1 + " ");
                    System.Console.WriteLine();
                }
            }
        }

        public void TopologicalSort()//Топологическая сортировка графа
        {
            int c = 1;
            if (!graph.Cyclic)
            {
                for (int i = 0; i < graph.n; i++)
                    if (!used[i])
                        TopDFS(i);
                System.Console.WriteLine("Содержимое стека топологической сортировки:\nНомер сортировки Вершина");
                foreach (int i in path)
                {
                    System.Console.WriteLine("{0} {1}",c,i + 1);
                    c++;
                }
            }
            else System.Console.WriteLine("Граф цикличный,сортировка невозможна");
        }
    }
}
