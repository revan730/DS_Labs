using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Lab1
{
    class Program
    {
        const string menu_text = "1.Алгоритм Беллмана - Форда\n2.Алгоритм Флойда - Уоршелла\n3.Алгоритм Джонсона\n4.Гамильтоновы циклы и пути\n5.Перезапуск\n6.Выход";
        static Graph graph;
        static void Main(string[] args)
        {
            SetGraph();
            UserMenu();
        }

        static void SetGraph()//Ввод графа с файла
        {
            System.Console.WriteLine("Введите название файла с графом:");
            try
            {
                graph = new Graph(System.Console.ReadLine(), true);
                graph.ProcessGraph();
            } catch (Exception e)
            {
                System.Console.WriteLine("Исключение:"+ e.Message);
                Restart();
            }
        }

        static void Restart()//Перезапуск программы
        {
            SetGraph();
            UserMenu();
        }

        static void UserMenu()//Вывод меню
        {
            int i = 0;
            bool restart = false;
            while (!restart)
            {
                System.Console.Clear();
                System.Console.WriteLine(menu_text);
                if (Int32.TryParse(System.Console.ReadLine(), out i))
                    switch (i)
                    {
                        case 1:
                                PrintDist2();
                                PrintDistA();
                            break;
                        case 2:
                            graph.Bellman(1, 2);
                            if (!graph.NCyclic)
                            {
                                PrintDistanceMatrix();
                                PrintDist2();
                            }
                            else Console.WriteLine("Граф имеет отрицательные циклы");
                            break;
                        case 3:
                            Johnson j = new Johnson(graph);
                            try
                            {
                                PrintMatrix(j.DistMatr());
                                PrintDist2();
                                PrintDistA();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 4:
                            PrintGCycles();
                            break;
                        case 5:
                            restart = true;
                            Restart();
                            break;
                        case 6:
                            System.Environment.Exit(1);
                            break;
                        default:
                            System.Console.WriteLine("Try again");
                            UserMenu();
                            break;

                    }
                System.Console.ReadKey();
            }
        }

        static void PrintAdjacencyMatrix()//вывод матрицы смежности
        {
            System.Console.Clear();
            System.Console.WriteLine("Матрица смежности:");
            PrintMatrix(graph.AdjMatr);
        }

        static void PrintMatrix<T>(T[,] Mat)//Вывод матрицы
        {
            for (int i = 0; i < Mat.GetLength(0); i++)
            {
                for (int j = 0; j < Mat.GetLength(1); j++) System.Console.Write("{0,2} ",Mat[i,j]);
                System.Console.WriteLine();
            }
        }

        static void PrintIncidenceMatrix()//вывод матрицы инцидентности
        {
            System.Console.Clear();
            System.Console.WriteLine("Матрица инцидентности:");
            PrintMatrix(graph.IncMatr);
        }

        static void PrintDistanceMatrix()
        {
            System.Console.WriteLine("Матрица расстояний:");
            PrintMatrix(graph.DistMatr);
        }

        static void PrintReachMatrix()
        {
            System.Console.WriteLine("Матрица достижимости:");
            PrintMatrix(graph.ReachMatr);
        }

        static void PrintExcs()
        {
            System.Console.WriteLine("Ексцентриситеты:");
            for (int i = 0; i < graph.n; i++) System.Console.WriteLine("{0}-й вершины = {1}", i + 1, graph.Excs[i]);
        }

        static void PrintCenters()
        {
            System.Console.Write("Центр графа: ");
            for (int с = 0; с < graph.n; с++)
                if (graph.Excs[с] == graph.Radius) System.Console.Write("вершина {0} ", с + 1);
            System.Console.WriteLine();
        }

        static void PrintLayers()
        {
            for (int i =0;i < graph.n;i++)
            {
                System.Console.WriteLine("Ярусы вершины {0}:", i + 1);
                System.Console.WriteLine("Расстояние 0: Вершина {0}", i + 1);
                for (int j = 1;j <= graph.Excs[i];j++)
                {
                    System.Console.Write("Расстояние {0}: ",j);
                    for (int k = 0; k < graph.n; k++) if (graph.DistMatr[i, k] == j) System.Console.Write("Вершина {0} ", k + 1);
                    System.Console.WriteLine();
                    
                }
                System.Console.WriteLine();
            }
        }

        static void PrintVertesesPower()
        {
            System.Console.WriteLine("Степени вершин: ");
            for (int i = 0;i < graph.verteses.GetLength(0);i++)
            {
                System.Console.WriteLine("{0}-я вершина - {1},полувыхода - {2},полувхода - {3}",i + 1,graph.verteses[i].power,graph.verteses[i].opower,graph.verteses[i].ipower);
            }
        }

        static void PrintIsolated()
        {
            System.Console.WriteLine("Изолированные вершины: ");
            for (int i = 0; i < graph.n; i++)
                if (graph.verteses[i].power == 0) System.Console.Write("Вершина №{0} ", i + 1);
            System.Console.WriteLine();
        }

        static void PrintHanging()
        {
            System.Console.WriteLine("Висящие вершины: ");
            for (int i = 0; i < graph.n; i++)
                if (graph.verteses[i].power == 1) System.Console.Write("Вершина №{0} ", i + 1);
            System.Console.WriteLine();
        }

        static void PrintCycles()
        {
            if (graph.Cyclic)
            {
                System.Console.WriteLine("Простые циклы:");
                for (int i = 0; i < graph.catalogCycles.Count; i++) System.Console.WriteLine("{0}", graph.catalogCycles[i]);
            }
            else System.Console.WriteLine("Граф не имеет циклов");
        }

        static void PrintCoherency()
        {
            switch (graph.Coherency)
            {
                case 3:
                    System.Console.WriteLine("Граф сильно связный");
                    break;

                case 2:
                    System.Console.WriteLine("Граф односторонне связный");
                    break;
                case 1:
                    System.Console.WriteLine("Граф слабосвязный");
                    break;
                case 0:
                    System.Console.WriteLine("Граф не связный");
                    break;
            }
        }

        static void StartDFS()
        {
            int v;
            System.Console.Write("Введите вершину для начала поиска в глубину:");
            if (Int32.TryParse(System.Console.ReadLine(), out v))
            {
                dfs dfs = new dfs(graph);
                System.Console.WriteLine("Содержание стека на каждой итерации:");
                dfs.TrDFS(v - 1);
            }

        }

        static void StartBFS()
        {
            int v;
            System.Console.Write("Введите вершину для начала поиска в ширину:");
            if (Int32.TryParse(System.Console.ReadLine(), out v))
            {
                bfs bfs = new bfs(graph);
                System.Console.WriteLine("Содержание очереди на каждой итерации:");
                bfs.BFS(v - 1);
            }

        }

        static void PrintDist2()
        {
            graph.Bellman(1, 2);
            if (!graph.NCyclic)
            {
                System.Console.WriteLine("Введите номера двух вершин для вывода расстояния");
                int v1 = Int32.Parse(System.Console.ReadLine().ToString()) - 1;
                int v2 = Int32.Parse(System.Console.ReadLine().ToString()) - 1;
                System.Console.WriteLine("Расстояние между вершинами:{0}", graph.DistMatr[v1, v2]);
                if (graph.DistMatr[v1, v2] != 0) System.Console.WriteLine("Путь:" + graph.Bellman(v1, v2));
            }
            else Console.WriteLine("Граф имеет отрицательные циклы");
        }

        static void PrintDistA()
        {
            graph.Bellman(1,2);
            if (!graph.NCyclic)
            {
                System.Console.WriteLine("Введите номер вершины для вывода расстояний");
                int v = Int32.Parse(System.Console.ReadLine().ToString()) - 1;
                for (int i = 0; i < graph.n; i++)
                    if (graph.DistMatr[v, i] != 0 && v != i)
                        System.Console.WriteLine("К вершине {0} - {1}", i + 1, graph.DistMatr[v, i]);
                    else System.Console.WriteLine("Вершина {0} недостижима", i + 1);
            }
        }
        
        static void PrintEuCycles()
        {
            if (graph.hasECycle)
            {
                Console.WriteLine("Граф содержит эйлеровский цикл:{0}",graph.catalogCycles[0]);
            }
            else if (!graph.hasECycle && graph.hasEPaths)
            {
                Console.WriteLine("Граф полуэйлеровский (не содержит цикл,но содержит путь)");
                dfs d = new dfs(graph);
                d.FindEPaths();

            }
            else Console.WriteLine("Граф не эйлеровский");
        }
        
        static void PrintGCycles()
        {
            if (graph.hasGCycle || graph.Cyclic)
                Console.WriteLine("Граф содержит гамильтонов цикл:{0}",graph.catalogCycles[0]);
            else
            {
                dfs d = new dfs(graph);
                d.FindGPath();
            }
        }
    }
}
