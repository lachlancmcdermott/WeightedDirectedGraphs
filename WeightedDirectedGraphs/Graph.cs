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

                for (int i = 0; i < vertices.Count; i++)
                {
                    for (int j = 0; j < vertices[i].NeighborCount; j++)
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

        public void AddVertex(Vertex<T> vertex)
        {
            if (vertex == null || vertex.NeighborCount > 0 || vertices.Contains(vertex) == false) return;
            vertices.Add(vertex);
        }

        public bool RemoveVertex(Vertex<T> vertex)
        {
            if(vertices.Contains(vertex))
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    for (int k = 0; k < vertices[i].NeighborCount; k++)
                    {
                        if(vertices[i].Neighbors[k].EndingPoint == vertex)
                        {
                            vertices.Remove(vertices[i].Neighbors[k].EndingPoint);
                        }
                    }
                }
                vertices.Remove(vertex);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddEdge(Vertex<T> a, Vertex<T> b, float distance)
        {
            if(a.Neighbors..Contains(b) && ) return false;
            if (a == null || b == null || Vertices.Contains(a) || Vertices.Contains(b)) return false;

            Edge<T> edge = new Edge<T>(a, b, distance);
            a.Neighbors.Add(edge); 
            return true;
        }

        public bool RemoveEdge(Vertex<T> a, Vertex<T> b)
        {
            if (a != null && b != null && b.Neighbors.Contains(a) && a.Neighbors.Contains(b))
            {
                a.Neighbors.Remove(b);
                b.Neighbors.Remove(a);
            }
            return false;
        }

        public Vertex<T> Search(T value)
        {
            int k = -1;

            for (int i = 0; i < Vertices.Count; i++)
            {
                if (Vertices[i].Value.Equals(value))
                {
                    k = i;
                }
            }
            if (k == -1)
            {
                return null;
            }
            else return Vertices[k];
        }

    }
}
