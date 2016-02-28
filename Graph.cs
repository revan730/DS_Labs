using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Lab1
{
    /// <summary>
    ///Класс,описывающий ориентированный граф 
    /// </summary>
    /// 
    
    //TODO: Поиск кратчайшего пути работает только для ориентированного графа
    class Graph
    {
        public int n, m, HPower; // n - количество вершин,m- рёбер,HPower - степень графа (-1 если не однородный)
        public Edge[] edges;
        public Vertex[] verteses;
        public int[,] AdjMatr;//Матрица смежности
        public int[,] IncMatr;//Матрица инцидентности
        public int[,] DistMatr;
        public int[,] ReachMatr;
        public int[] Excs;
        //private byte[] cl;//Массив цветов вершин для поиска в глубину
        //public int[] p;//Массив предков для того же алгоритма
        //private int cycle_st = -1, cycle_end;
        public bool Homogen;//Однородность графа
        public bool Oriented;
        //public bool Cyclic;
        public int Radius;
        public int Diameter;

        public Graph(string file,bool oriented)
        {
            InputFromFile(file);
            HPower = -1;
            Homogen = true;
            Oriented = oriented;
        }

        public void ProcessGraph()//Полное решение всех полей графа
        {
            AdjMatr = FillAdjacencyMatrix();
            IncMatr = FillIncidenceMatrix();
            DistMatr = Dijkstra();
            ReachMatr = FloydWarshellR();
            if (!Oriented)
            {
                SymmetrizeMatrix(DistMatr);
                SymmetrizeMatrix(ReachMatr);
            }
            Excs = FillExcentricitiesArr();
            Radius = Excs.Where(x => x > 0).Min();
            Diameter = Excs.Max();
            CheckLoopedVerteses();
            GetVertesesPower();
            CheckHomogeneity();
            //cl = new byte[n];
            //p = Enumerable.Repeat(-1,n).ToArray();
            //FindCycles();
        }

        private int[,] FillAdjacencyMatrix()//Заполнение матрицы смежности
        {
            int[,] AdjMatr = new int[n, n];
            for (int i = 0; i < m; i++)
            {
                AdjMatr[edges[i].n1 - 1, edges[i].n2 - 1] = 1;
            }
            return AdjMatr;
        }

        private int[,] FillIncidenceMatrix()//Заполение матрицы инцидентности
        {
            int[,] IncMatr = new int[n, m];
            for (int i = 0; i < m; i++)
                IncMatr[edges[i].n1 - 1, i] = -1;
            for (int i = 0; i < m; i++)
                 IncMatr[edges[i].n2 - 1, i] = 1;
            for (int i = 0; i < m; i++)
                if (edges[i].n1 == edges[i].n2) IncMatr[edges[i].n1 - 1, i] = 2;
                return IncMatr;
        }

        private int[,] FillDistanceMatrix()
        {
            int[,] DistMatr = new int[n, n];

                return DistMatr;
        }

        private void SymmetrizeMatrix(int[,] Matrix)
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
                for (int j = i + 1; j < Matrix.GetLength(1); j++) Matrix[j, i] = Matrix[i, j];
        }

        private bool[,] ConvertToBool(int[,] Matrix)
        {
            int n = Matrix.GetLength(0);
            int m = Matrix.GetLength(1);
            bool[,] BMatr = new bool[n,m];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    BMatr[i, j] = Convert.ToBoolean(Matrix[i,j]);

            return BMatr;
        }

        private int[,] ConvertToInt(bool[,] Matrix)
        {
            int n = Matrix.GetLength(0);
            int m = Matrix.GetLength(1);
            int[,] IMatr = new int[n, m];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    IMatr[i, j] = Convert.ToInt32(Matrix[i,j]);

            return IMatr;
        }

        private int GetRowMaximum(int[,] Matr,int r)
        {
            int max = 0;
            for (int i = 0; i < Matr.GetLength(0); i++) if (Matr[r, i] > max) max = Matr[r, i];
            return max;
        }

        private int[] FillExcentricitiesArr()
        {
            int[] excs = new int[n];
            for (int i = 0; i < n; i++) excs[i] = GetRowMaximum(DistMatr,i);
            return excs;
        }

        private void InputFromFile(string path)//Ввод графа с файла
        {
            int c = 0;
            string line;
            StreamReader file = new StreamReader(path);
            line = file.ReadLine();
            n = Int32.Parse(line[0].ToString());
            m = Int32.Parse(line.Split(' ')[1].ToString());
            //System.Console.WriteLine("n={0} m={1}", gb.n, gb.m);
            edges = new Edge[m];
            verteses = new Vertex[n];
            InitializeVerteses();
            string[] rawlist = new string[m];
            while ((line = file.ReadLine()) != null)
            {
                rawlist[c] = line;
                c++;

            }
            for (c = 0; c < rawlist.GetLength(0); c++)
            {
                int n1,n2;
                n1 = Int32.Parse(rawlist[c][0].ToString());
                n2 = Int32.Parse(rawlist[c][2].ToString());
                verteses[n1 - 1].adjances.Add(n2 - 1); 
                edges[c] = new Edge(n1,n2);

            }
        }

        private void InitializeVerteses()//Инициализация массива вершин
        {
            for (int i = 0; i < verteses.GetLength(0); i++) verteses[i] = new Vertex();
        }

        private void GetVertesesPower()//Нахождение степеней вершин
        {
            for (int i = 0; i < n; i++)
            {
                int inside = 0, outside = 0;
                for (int j = 0; j < IncMatr.GetLength(1); j++)
                {
                    if (IncMatr[i, j] == -1) outside++;
                    else if (IncMatr[i, j] == 1) inside++;
                }
                verteses[i].ipower = inside + Convert.ToInt32(verteses[i].isLooped);
                verteses[i].opower = outside + Convert.ToInt32(verteses[i].isLooped);
                verteses[i].power = verteses[i].ipower + verteses[i].opower;
                if (verteses[i].power == 0) verteses[i].isIsolated = true;
                else if (verteses[i].power == 1) verteses[i].isHanging = true;
            }
        }

        private void CheckLoopedVerteses()
        {
            for (int i = 0; i < m; i++)
                if (edges[i].n1 == edges[i].n2) verteses[edges[i].n1 - 1].isLooped = true;
        }

        private void CheckHomogeneity()
        {
            for (int i = 1;i < verteses.GetLength(0);i++) if (verteses[i].power != verteses[i-1].power) Homogen = false;
            if (Homogen) HPower = verteses[0].power;
        }

        /*private void FindCycles()
        {
            for (int i = 0; i < n; i++) if (dfs(i)) break;
            if (cycle_st == -1) Cyclic = false;
            else Cyclic = true;
        }*/

        private int[,] FloydWarshell()//Алгоритм Флойда - Уоршелла для матрицы расстояний
        {
            int[,] d = new int[n,n];
            Array.Copy(AdjMatr,d,AdjMatr.Length);
            //Заполение матрицы максимальными значениями вместо нулей
            //Главную диагональ заполняет нулями,так как кратчайший путь от вершины до самой себя - 0
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if ((d[i, j] == 0) && (i != j)) d[i, j] = Int32.MaxValue;
                    else if (i == j) d[i, j] = 0;
                }
            //Сам алгоритм
            for (int k = 0; k < n; k++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (d[i,k] < Int32.MaxValue && d[k,j] < Int32.MaxValue)
                            if (d[i,k] + d[k,j] < d[i,j])
                                d[i,j] = d[i,k] + d[k,j];
            //Замена максимальных значений обратно на нули
            for (int i = 0; i < d.GetLength(0); i++)
                for (int j = 0; j < d.GetLength(1); j++) if (d[i, j] == Int32.MaxValue) d[i, j] = 0;
            return d;
        }

        private int[,] FloydWarshellR()//Тот же алгоритм,но для матрицы достижимости
        {
            bool[,] r = new bool[n, n];
            Array.Copy(ConvertToBool(AdjMatr), r, AdjMatr.Length);

            for (int k = 0; k < n; k++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        r[i, j] = r[i, j] || (r[i, k] && r[k, j]);

            return ConvertToInt(r);
        }

        private int[,] Dijkstra()//Алгоритм Дейкстры для матрицы расстояний,не пытайся понять код,магия)
        {
            int[,] d = new int[n,n];
            for (int k = 0;k< n;k++)// Итерация алгоритма для каждой вершины
            {
                int count, index = 0, i, u, m = k+1;
                int[] distance = new int[n];
                bool[] visited = new bool[n];
                for (i = 0; i < n; i++)
                {
                    distance[i]=Int32.MaxValue; 
                    visited[i]=false;
                }
                distance[k]=0;
                for (count=0; count < n-1; count++)
                {
                    int min=Int32.MaxValue;
                    for (i=0; i < n; i++)
                        if (!visited[i] && distance[i]<=min)
                        {
                            min=distance[i]; index=i;
                        }
                    u=index;
                    visited[u]=true;
                    for (i = 0; i < n; i++)
                    if (!visited[i] && AdjMatr[u,i] > 0 && distance[u]!=Int32.MaxValue &&
                    distance[u]+AdjMatr[u,i]<distance[i])
                    distance[i]=distance[u]+AdjMatr[u,i];

                    for (i = 0; i < n; i++) d[k, i] = distance[i];
                }
            }
            //Замена всех максимальных значений на 0
            for (int i = 0; i < d.GetLength(0); i++)
                for (int j = 0; j < d.GetLength(1); j++) if (d[i, j] == Int32.MaxValue) d[i, j] = 0;
            return d;
        }

        /*private bool dfs(int v)
        {
            cl[v] = 1;
            for (int i = 0; i < n; ++i)
            {
                int to = 0;
                if (i < verteses[v].adjances.Count) to = verteses[v].adjances[i];
                if (cl[to] == 0)
                {
                    p[to] = v;
                    if (dfs(to)) return true;
                }
                else if (cl[to] == 1)
                {
                    cycle_end = v;
                    cycle_st = to;
                    return true;
                }
            }
            cl[v] = 2;
            return false;
        }*/
    }
}
