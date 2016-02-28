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
    class Edge
    {
        public int n1;
        public int n2;

        public Edge(int n1,int n2)
        {
            this.n1 = n1;
            this.n2 = n2;

        }

    }
}
