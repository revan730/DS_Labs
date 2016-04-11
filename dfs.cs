using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Lab1
{
    /// <summary>
    /// Depth-first search implementation
    /// </summary>
    class dfs
    {
        Graph graph;
        bool[] used;
        Stack<int> path;
        int k;//DFS index

        public dfs(Graph g)
        {
            this.graph = g;
            path = new Stack<int>();
            used = new bool[graph.n];
            k = 0;
        }

        /// <summary>
        /// DFS graph traversal method
        /// </summary>
        /// <param name="v">Start vertex</param>
        public void TrDFS(int v)
        {
            k++;
            used[v] = true;
            path.Push(v);
            System.Console.Write("Текущая вершина - {0},DFS-номер - {1},содержание стека:",v + 1,k);
            foreach (int i in path)
               System.Console.Write(i + 1 + " ");
             System.Console.WriteLine();

            foreach (int i in graph.verteses[v].adjances)
                if (used[i] == false)
                {
                    TrDFS(i);
                    path.Pop();
                }
        }

        private void TopDFS(int v)
        {
            used[v] = true;
            foreach (int i in graph.verteses[v].adjances)
                if (used[i] == false)
                    TopDFS(i);
            path.Push(v);
        }

        private void CmDFS(int v)
        {
            used[v] = true;
            path.Push(v);
            foreach (int i in graph.verteses[v].adjances)
                if (used[i] == false)
                {
                    CmDFS(i);
                }
        }


        /// <summary>
        /// Coherency components search
        /// </summary>
        public void FindComps()
        {
            for (int i = 0;i < graph.n;i++)
            {
                if (!used[i])
                {
                    k++;
                    path.Clear();
                    CmDFS(i);
                    System.Console.Write("Компонента №{0}:",k);
                    foreach (int c in path)
                        System.Console.Write(c + 1 + " ");
                    System.Console.WriteLine();
                }
            }
            System.Console.WriteLine("Количество компонент:{0}", k);
        }

        /// <summary>
        /// Performs topological sorting of graph vertices
        /// </summary>
        public void TopologicalSort()
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
        

        /// <summary>
        /// Eurelian path search
        /// </summary>
        public void FindEPaths()
        {
            for (int i = 0;i < graph.n;i++)
            {
                if (!used[i])
                {
                    path.Clear();
                    CmDFS(i);
                    System.Console.Write("Эйлеров путь: ");
                    foreach (int c in path.Reverse())
                        System.Console.Write(c + 1 + " ");
                    System.Console.WriteLine();
                }
            }
        }
        
        private bool checkGamiltonianH(int v)
        {
            k++;
            foreach (int c in graph.verteses[v].adjances)
            checkGamiltonianH(c);
            if (k == graph.n)
                return true;
            else
                return false;
        }
        
        private bool checkGamiltonian()
        {
            for (int i = 0; i < graph.n;i++)
                if (checkGamiltonianH(i))
                    return true;
            return false;
        }
        
        public void FindGPath()
        {
            if (checkGamiltonian())
                for (int i = 0;i < graph.n;i++)
                {
                    if (!used[i])
                    {
                        path.Clear();
                        CmDFS(i);
                        System.Console.Write("Гамильтонов путь: ");
                        foreach (int c in path.Reverse())
                            System.Console.Write(c + 1 + " ");
                        System.Console.WriteLine();
                    }
                }
            else
                Console.WriteLine("Граф не гамильтонов");
        }
    }
}
