 using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public bool AddVal(int value)
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
            Edge<T> edge = new Edge<T>(a, b, distance);
            if (a == null || b == null || !Vertices.Contains(a) || !Vertices.Contains(b) || a.Neighbors.Any((edge) => edge.EndingPoint == b)) return false;

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
        public List<Vertex<T>> BreathFirstSearch(Vertex<T> start, Vertex<T> end)
        {
            Queue<Vertex<T>> queue = new Queue<Vertex<T>>();
            Vertex<T> curr = start;
            List<Vertex<T>> path = new List<Vertex<T>>();

            queue.Enqueue(start);

            do
            {
                if(queue.Count == 0)
                {
                    return null;
                }
                curr = queue.Dequeue();
                for (int i = 0; i < curr.NeighborCount; i++)
                {
                    if (!curr.Neighbors[i].EndingPoint.isVisited)
                    {
                        curr.Neighbors[i].EndingPoint.parent = curr;
                        curr.Neighbors[i].EndingPoint.isVisited = true;
                        queue.Enqueue(curr.Neighbors[i].EndingPoint);
                    }
                }
            } while (curr != end);

            do
            {
                path.Add(curr);
                curr = curr.parent;
            }
            while (curr != start);
            path.Add(curr);
            
            return path;
        }
        public List<Vertex<T>> DepthFirstSearch(Vertex<T> start, Vertex<T> end)
        {
            Stack<Vertex<T>> stack = new Stack<Vertex<T>>();
            Vertex<T> curr = start;
            stack.Push(start);

            do
            {
                if (stack.Count == 0)
                {
                    return null;
                }
                curr = stack.Pop();
                for (int i = 0; i < curr.NeighborCount; i++)
                {
                    if (!curr.Neighbors[i].EndingPoint.isVisited)
                    {
                        curr.Neighbors[i].EndingPoint.parent = curr;
                        stack.Push(curr.Neighbors[i].EndingPoint);
                        curr.Neighbors[i].EndingPoint.isVisited = true;
                    }
                }


            } while (curr != end);
            List<Vertex<T>> path = new List<Vertex<T>>();
            do
            {
                path.Add(curr);
                curr = curr.parent;
            }
            while (curr != start);
            path.Add(curr);

            return path;
        }
        public void ResetVisited(Graph<T> graph)
        {
            for (int i = 0; i < graph.VertexCount; i++)
            {
                graph.vertices[i].isVisited = false;
            }
        }

        public List<Vertex<T>> DijkstraSeach(Graph<T> graph, Vertex<T> start, Vertex<T> end)
        {
            PriorityQueue<Vertex<T>, int> queue = new PriorityQueue<Vertex<T>, int>();
            for (int i = 0; i < graph.VertexCount; i++)
            {
                graph.vertices[i].isVisited = false;
                graph.vertices[i].cumulativeDistFromStart = int.MaxValue;
                graph.vertices[i].parent = null;
            }
            start.cumulativeDistFromStart = 0;
            queue.Enqueue(start, start.cumulativeDistFromStart);
            Vertex<T> curr;
            do
            {
                curr = queue.Dequeue();
                curr.isVisited = true;
                //start
                //foreach (var edge in current.Neighbors)
                for (int i = 0; i < curr.NeighborCount; i++)
                {
                    var currNeighboorVertex = curr.Neighbors[i].EndingPoint;
                    float tenativeDist = curr.Neighbors[i].Distance + curr.cumulativeDistFromStart;

                    if (tenativeDist < curr.Neighbors[i].Distance)
                    {
                        curr.Neighbors[i].EndingPoint.cumulativeDistFromStart = curr.cumulativeDistFromStart;
                        currNeighboorVertex.isVisited = false;
                    }

                    if (currNeighboorVertex.isVisited == false)
                    {
                        queue.Enqueue(currNeighboorVertex, currNeighboorVertex.cumulativeDistFromStart);
                    }
                }
            } while (curr != end);

            return null;
        }
    }
}

    
//if (tentative < curr.Neighbors[i].Distance)
//{
//    info[neighbor] = (current, tentative);
//    neighbor.isVisited = false;
//}
