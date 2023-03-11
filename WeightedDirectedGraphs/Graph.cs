 using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
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
            if (vertex == null || vertex.Neighbors.Count > 0) return false;

            vertices.Add(vertex);
            return true;
        }
        public bool AddVal(int value, float x, float y)
        {
            Vertex<T> v = new Vertex<T>(value, x, y);
            if (v == null || v.Neighbors.Count > 0) return false;

            vertices.Add(v);
            return true;
        }
        public bool RemoveVertex(Vertex<T> vertex)
        {
            for (int i = 0; i < vertices.Count;)
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
        public bool AddEdge(Vertex<T> a, Vertex<T> b)
        {
            float PosX = MathF.Abs(a.x - b.x);
            float PosY = MathF.Abs(a.y - b.y);
            float distance = 1 * MathF.Sqrt(MathF.Pow(PosX, 2) + MathF.Pow(PosY, 2));

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
            #nullable disable
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
            #nullable disable
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
            #nullable disable
            PriorityQueue<Vertex<T>, float> queue = new PriorityQueue<Vertex<T>, float>();
            for (int i = 0; i < graph.VertexCount; i++)
            {
                graph.vertices[i].isVisited = false;
                graph.vertices[i].cumulativeDistFromStart = float.MaxValue;
                graph.vertices[i].parent = null;
            }
            start.cumulativeDistFromStart = 0;
            queue.Enqueue(start, start.cumulativeDistFromStart);
            Vertex<T> curr = null;
            do
            {
                if(queue.Count == 0)
                {
                    return null;
                }
                curr = queue.Dequeue();
                curr.isVisited = true;
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
                        curr.parent = currNeighboorVertex;
                        queue.Enqueue(currNeighboorVertex, currNeighboorVertex.cumulativeDistFromStart);
                    }
                }
            } while (curr != end);
            List<Vertex<T>> path = new List<Vertex<T>>();
            do
            {
                path.Add(curr);
                if(curr.parent == null)
                {
                    break;
                }
                curr = curr.parent;
            }
            while (curr != start);
            path.Add(curr);

            return path;
        }
        public float ManhattanHeurutics(Vertex<T> node, Vertex<T> goal)
        {
            float dx = Math.Abs(node.x - goal.x);
            float dy = Math.Abs(node.y - goal.y);
            return 1 * (dx + dy);
        }
        public List<Vertex<T>> AStarSearch(Graph<T> graph, Vertex<T> start, Vertex<T> end)
        {
            PriorityQueue<Vertex<T>, float> queue = new PriorityQueue<Vertex<T>, float>();
            for (int i = 0; i < graph.VertexCount; i++)
            {
                graph.vertices[i].isVisited = false;
                graph.vertices[i].cumulativeDistFromStart = int.MaxValue;
                graph.vertices[i].finalDistance = int.MaxValue;
                graph.vertices[i].parent = null;
            }
            start.cumulativeDistFromStart = 0;
            start.finalDistance = ManhattanHeurutics(start, end);
            queue.Enqueue(start, start.finalDistance);
            Vertex<T> curr;
            do
            {
                if (queue.Count == 0)
                {
                    return null;
                }
                curr = queue.Dequeue();
                float currFinalDist = ManhattanHeurutics(curr, end);

                curr.isVisited = true;
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
                        currNeighboorVertex.finalDistance = tenativeDist + ManhattanHeurutics(currNeighboorVertex, end);
                        curr.parent = currNeighboorVertex;
                        queue.Enqueue(currNeighboorVertex, currNeighboorVertex.cumulativeDistFromStart);
                    }
                }

            } while (curr != end);
            List<Vertex<T>> path = new List<Vertex<T>>();
            do
            {
                path.Add(curr);
                if (curr.parent == null)
                {
                    break;
                }
                curr = curr.parent;
            }
            while (curr != start);
            path.Add(curr);

            return path;
        }
        public List<Vertex<T>> BellmanFord(Graph<T> graph, Vertex<T> start, Vertex<T> end)
        {
            PriorityQueue<Vertex<T>, float> queue = new PriorityQueue<Vertex<T>, float>();
            for (int i = 0; i < graph.VertexCount; i++)
            {
                graph.vertices[i].isVisited = false;
                graph.vertices[i].cumulativeDistFromStart = float.MaxValue;
                graph.vertices[i].parent = null;
            }
            start.cumulativeDistFromStart = 0;
            queue.Enqueue(start, start.cumulativeDistFromStart);
            #nullable disable
            Vertex<T> curr = null;
            do
            {
                if (queue.Count == 0)
                {
                    return null;
                }
                curr = queue.Dequeue();
                curr.isVisited = true;

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
                        curr.parent = currNeighboorVertex;
                        queue.Enqueue(currNeighboorVertex, currNeighboorVertex.cumulativeDistFromStart);
                    }
                }
                //looping thru vertices first time
                for (int i = 0; i < graph.vertices.Count; i++)
                {
                    for (int k = 0; k < graph.Edges.Count; k++)
                    {
                        Vertex<T> q = Edges[k].StartingPoint;
                        Vertex<T> e = Edges[k].EndingPoint;
                        float weight = Edges[k].Distance;
                        float endTenativeDist = e.Neighbors[i].Distance + weight;

                        if (e.cumulativeDistFromStart < endTenativeDist)
                        {
                            e.cumulativeDistFromStart = endTenativeDist;
                        }
                    }
                }
                //looping thru vertices a second time
                for (int i = 0; i < graph.vertices.Count; i++)
                {
                    for (int k = 0; k < graph.Edges.Count; k++)
                    {
                        Vertex<T> q = Edges[k].StartingPoint;
                        Vertex<T> e = Edges[k].EndingPoint;
                        float weight = Edges[k].Distance;
                        float endTenativeDist = e.Neighbors[i].Distance + weight;

                        if (e.cumulativeDistFromStart > endTenativeDist)
                        {
                            Exception NegCycle = new Exception("Negative cycle");
                            throw NegCycle;
                        }
                    }
                }

            } while (curr != end);
            List<Vertex<T>> path = new List<Vertex<T>>();
            do
            {
                path.Add(curr);
                if (curr.parent == null)
                {
                    break;
                }
                curr = curr.parent;
            }
            while (curr != start);
            path.Add(curr);

            return path;
        }
    }
}