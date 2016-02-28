using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Lab1
{
    /// <summary>
    /// Класс для представления вершин графа
    /// </summary>
    public class Vertex
    {
        public List<int> adjances;//Список индексов смежных вершин
        public int ipower;
        public int opower;
        public int power;
        public bool isIsolated;
        public bool isHanging;
        public bool isLooped;

        public Vertex()
        {
            power = 0;
            isIsolated = false;
            isLooped = false;
            isHanging = false;
            adjances = new List<int>();

        }
    }
}
