 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using WeightedDirectedGraphs;

namespace WeightedDirectedGraphs
{
    public class Graph<T>
    {
        private List<Vertex<T>> vertices = new List<Vertex<T>>(0);

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

        public bool AddVertex(Vertex<T> vertex)
        {
            if (vertex == null || vertex.Value == null || vertex.Neighbors.Count > 0) return false;

            vertices.Add(vertex);
            return true;
        }
        public bool AddVal(T value)
        {
            Vertex<T> v = new Vertex<T>(value);
            if (v == null || v.Value == null || v.Neighbors.Count > 0) return false;

            vertices.Add(v);
            return true;
        }
        public bool RemoveVertex(Vertex<T> vertex)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int k = 0; k < vertices[i].NeighborCount; k++)
                {
                    if (vertices[i].Neighbors[k].EndingPoint == vertex)
                    {
                        vertices.Remove(vertices[i].Neighbors[k].EndingPoint);
                    }
                }
                vertices.RemoveAt(i);
                return true;
            }
            return false;

        }
        public bool AddEdge(Vertex<T> a, Vertex<T> b, float distance)
        {
            if (a == null || b == null || Vertices.Contains(a) || Vertices.Contains(b) || a.Neighbors.Any((edge) => edge.EndingPoint == b)) return false;

            Edge<T> edge = new Edge<T>(a, b, distance);
            a.Neighbors.Add(edge);
            return true;
        }
        public bool RemoveEdge(Vertex<T> a, Vertex<T> b)
        {
            if (a == null || b == null || !vertices.Contains(a) || !vertices.Contains(b)) return false;

            bool found;
            int foundIndex = 0;

            if (a.Neighbors.Find((edge) =>
            {
                found = edge.EndingPoint == b;
                if (!found) { foundIndex++; }
                return found;
            }) == null) return false;
            else
            {
                a.Neighbors.RemoveAt(foundIndex);
                return true;
            }
        }
        public Vertex<T>? Search(T value)
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
        public Edge<T>? GetEdge(Vertex<T> a, Vertex<T> b)
        {
            for (int i = 0; i < a.NeighborCount; i++)
            {
                if (a.Neighbors[i].EndingPoint.Equals(b))
                {
                    return a.Neighbors[i];
                }
            }
            return null;
        }

        //if a repeatd value is sighted dont go back, just stop the function
        //make parent variable in the vertex to look back up tree (use variable or dictionary)
        public Queue<Vertex<T>> BreathFirstSearch(Vertex<T> start, Vertex<T> end)
        {
            Queue<Vertex<T>> path = new Queue<Vertex<T>>();

            Vertex<T> temp = start;
            int maxNumberOfNodes = temp.NeighborCount;
            int prevNumberMax;
            path.Enqueue(start);

            while (true)
            {
                temp = path.Dequeue();
                for (int k = 0; k < temp.NeighborCount; k++)
                {
                    if (temp.Neighbors[k].EndingPoint.Equals(end))
                    {
                        return path;
                    }
                    for (int i = 0; i < temp.NeighborCount; i++)
                    {
                        path.Enqueue(temp.Neighbors[i].EndingPoint);
                    }
                }
            }
        }
    }
}