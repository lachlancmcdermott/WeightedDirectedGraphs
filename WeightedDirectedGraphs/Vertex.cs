using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeightedDirectedGraphs
{
    public class Vertex<T>
    {
        public T Value { get; set; }

        private List<Edge<T>> neighbors; 
        public List<Edge<T>> Neighbors
        {
            get { return neighbors; }
        }

        public int NeighborCount => neighbors.Count;
        //public int NeighBorCount { get { return neighbors.Count; } }
        public Vertex(T value)
        {
            neighbors = new List<Edge<T>>(0);
            Value = value;
        }
    }
}