using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightedDirectedGraphs
{
    public class Graph<T>
    {
        private List<Vertex<T>> vertices;
        public IReadOnlyList<Vertex<T>> Vertices => vertices;

        public IReadOnlyList<Edge<T>> Edges
        {
            get
            {
                var edges = new List<Edge<T>>();

                for (int i = 0; i < Count; i++)
                {
                    for (int j = 0; j < vertices[i].Count; j++)
                    {
                        edges.Add(vertices[i].Neighbors[j]);
                    }
                }

                return edges;
            }
        }
        public int VertexCount => vertices.Count;

        public Graph() 
        {

        }
    }
}
