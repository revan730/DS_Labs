using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Lab1
{
    /// <summary>
    ///Класс,описывающий ориентированный,взвешенный граф 
    /// </summary>
    /// 
    
    //TODO: Поиск кратчайшего пути работает только для ориентированного графа,рефакторинг
    //Определение связности для графов с отрицательным весом работает не правильно (впрочем где еще кроме 3 лабы оно используется? :) )
    public class Graph
    {
        public int n, m, HPower; // n - количество вершин,m - рёбер,HPower - степень графа (-1 если не однородный)
        public Edge[] edges;
        public Vertex[] verteses;
        public int[,] AdjMatr;//Матрица смежности
        public int[,] IncMatr;//Матрица инцидентности
        public int[,] DistMatr;
        public int[,] ReachMatr;
        public int[] Excs;
        public bool Homogen;//Однородность графа
        public bool Oriented;
        public bool Cyclic;
        public int Radius;
        public int Diameter;
        public int Coherency;//0 - не связный,1 - слабосвязный,2 - односторонне связный,3 - сильно связный
        public List<string> catalogCycles = new List<string>();//Список с циклами графа в виде строки,например "1-2-1"

        public Graph(string file,bool oriented)
        {
            InputFromFile(file);
            HPower = -1;
            Homogen = true;
            Coherency = 3;
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
            Radius = Excs.Min();
            Diameter = Excs.Max();
            CheckLoopedVerteses();
            GetVertesesPower();
            CheckHomogeneity();
            FindCycles();
            CheckCoherence();
        }

        private int[,] FillAdjacencyMatrix()//Заполнение матрицы смежности
        {
            int[,] AdjMatr = new int[n, n];
            for (int i = 0; i < m; i++)
            {
                AdjMatr[edges[i].n1, edges[i].n2] = edges[i].w;
            }
            return AdjMatr;
        }

        private int[,] FillIncidenceMatrix()//Заполение матрицы инцидентности
        {
            int[,] IncMatr = new int[n, m];
            for (int i = 0; i < m; i++)
                IncMatr[edges[i].n1, i] = -1;
            for (int i = 0; i < m; i++)
                 IncMatr[edges[i].n2, i] = 1;
            for (int i = 0; i < m; i++)
                if (edges[i].n1 == edges[i].n2) IncMatr[edges[i].n1, i] = 2;
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

        private T[,] TransposeMatrix<T>(T[,] Matrix)
        {
            int n = Matrix.GetLength(0);
            int m = Matrix.GetLength(1);
            T[,] TMatr = new T[n,m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    TMatr[i, j] = Matrix[j, i];
            return TMatr;
        }

        private int[,] MultiplyMatrix(int[,] m1,int[,] m2)
        {
            int n = m1.GetLength(0);
            int m = m2.GetLength(1);
            int[,] resultMatrix = new int[n,m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j< m; j++)
                {
                    resultMatrix[i,j] = 0;
                    for (int k = 0; k < m; k++)
                    {
                        resultMatrix[i, j] += m1[i, k] * m2[k, j];
                    }
                }
             }
            return resultMatrix;
        }

        private int[,] MatrixPower(int[,] Matrix,int p)
        {
            int n = Matrix.GetLength(0);
            int m = Matrix.GetLength(1);
            int[,] PMatr = new int[n,m];
            Array.Copy(Matrix, PMatr, Matrix.Length);
            for (int i = 1; i < p; i++) PMatr = MultiplyMatrix(PMatr, Matrix);
            return PMatr;
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
                int n1,n2,w;
                n1 = Int32.Parse(rawlist[c].Split(' ')[0]);//Проверок на наличие числа в строке нет специально,так как ошибки ввода обработаются исключением
                n2 = Int32.Parse(rawlist[c].Split(' ')[1]);//А стандартные значения лучше не задавать,потом труднее искать ошибки :)
                w = Int32.Parse(rawlist[c].Split(' ')[2]);
                verteses[n1 - 1].adjances.Add(n2 - 1); 
                edges[c] = new Edge(n1 - 1,n2 - 1,w);
            }
            file.Close();
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

        private void FindCycles()//Поиск циклов
        {
            int[] color = new int[n];//Массив цветов вершин
            List<Edge> E = edges.OfType<Edge>().ToList();//Процедура использует совокупность ребер в виде списков
            for (int i = 0; i < n; i++)
            {
                for (int k = 0; k < n; k++)
                    color[k] = 1;
                List<int> cycle = new List<int>();
                //поскольку в C# нумерация элементов начинается с нуля, то для
                //удобочитаемости результатов поиска в список добавляем номер i + 1
                cycle.Add(i + 1);
                DFScycle(i, i, E, color, -1, cycle);
            }
            if (catalogCycles.Count > 0) Cyclic = true;
        }

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
                    if (!visited[i] && AdjMatr[u,i] != 0 && distance[u]!=Int32.MaxValue &&
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

        private void DFScycle(int u, int endV, List<Edge> E, int[] color, int unavailableEdge, List<int> cycle)//Модификация алгоритма поиска в глубину для нахождения циклов
        {
            //если u == endV, то эту вершину перекрашивать не нужно, иначе мы в нее не вернемся, а вернуться необходимо
            if (u != endV)
                color[u] = 2;
            else if (cycle.Count >= 2)
            {
                cycle.Reverse();
                string s = cycle[0].ToString();
                for (int i = 1; i < cycle.Count; i++)
                    s += "-" + cycle[i].ToString();
                bool flag = false; //есть ли палиндром для этого цикла графа в List<string> catalogCycles?
                for (int i = 0; i < catalogCycles.Count; i++)
                    if (catalogCycles[i].ToString() == s)
                    {
                        flag = true;
                        break;
                    }
                if (!flag)
                {
                    cycle.Reverse();
                    s = cycle[0].ToString();
                    for (int i = 1; i < cycle.Count; i++)
                        s += "-" + cycle[i].ToString();
                    catalogCycles.Add(s);
                }
                return;
            }
            for (int w = 0; w < E.Count; w++)
            {
                if (w == unavailableEdge)
                    continue;
                if (color[E[w].n2] == 1 && E[w].n1 == u)
                {
                    List<int> cycleNEW = new List<int>(cycle);
                    cycleNEW.Add(E[w].n2 + 1);
                    DFScycle(E[w].n2, endV, E, color, w, cycleNEW);
                    color[E[w].n2] = 1;
                }
                else if (color[E[w].n1] == 1 && E[w].n2 == u && !Oriented)//Только для ориентированных графов!
                {
                    List<int> cycleNEW = new List<int>(cycle);
                    cycleNEW.Add(E[w].n1 + 1);
                    DFScycle(E[w].n1, endV, E, color, w, cycleNEW);
                    color[E[w].n1] = 1;
                }
            }
        }

        private void CheckCoherence()//Определение типа связности
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (ReachMatr[i, j] == 0) Coherency = 2;//Если матрица достижимости вся в единицах - граф сильно связный
            if (Coherency == 2)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (ReachMatr[i, j] != 1 && ReachMatr[j,i] != 1) Coherency = 1;//Если треугольная в единицах - единосторонне связный
            if (Coherency == 1)
            {
                int[,] Matr = new int[n, n];
                Array.Copy(AdjMatr, Matr, AdjMatr.Length);
                 int[,] TMatr = TransposeMatrix(AdjMatr);
                //Добавляем транспонированную матрицу смежности
                 for (int i = 0; i < n; i++)
                     for (int j = 0; j < n; j++)
                         Matr[i, j] += TMatr[i, j];
                //Добавляем единичную матрицу
                         for (int i = 0; i < n; i++)
                             Matr[i, i] += 1;
                //Возводим в степень n - 1
                Matr = MatrixPower(Matr, n - 1);
                //Если нет нулей - граф слабо связный
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (Matr[i, j] < 1) Coherency = 0;
            }

        }
    }
}
