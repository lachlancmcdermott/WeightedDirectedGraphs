using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeightedDirectedGraphs
{
    public class Vertex<T>
    {
        public int cumulativeDistFromStart;
        public bool isVisited = false;
        public Vertex<T> parent;
        public int Value { get; set; }

        private List<Edge<T>> neighbors; 
        public List<Edge<T>> Neighbors
        {
            get { return neighbors; }
        }

        public int NeighborCount => neighbors.Count;
        //public int NeighBorCount { get { return neighbors.Count; } }
        public Vertex(int value)
        {
            neighbors = new List<Edge<T>>(0);
            Value = value;
        }
    }
}