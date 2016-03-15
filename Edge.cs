using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Lab1
{
    /// <summary>
    /// Класс для представления рёбер графа
    /// </summary>
    public class Edge
    {
        public int n1;//Вершина источник
        public int n2;//Вершина назначения
        public int w;//Вес ребра

        public Edge(int n1,int n2,int w)
        {
            this.n1 = n1;
            this.n2 = n2;
            this.w = w;
        }

    }
}
