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
        public int id;//Номер вершин
        public int ipower;//Степень полувхода
        public int opower;//Степень полувыхода
        public int power;//Степень
        public bool isIsolated;
        public bool isHanging;
        public bool isLooped;

        public Vertex(int id)
        {
            power = 0;
            isIsolated = false;
            isLooped = false;
            isHanging = false;
            adjances = new List<int>();
            this.id = id; 

        }
    }
}
