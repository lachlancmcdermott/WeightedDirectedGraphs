using System.Runtime.CompilerServices;

namespace WeightedDirectedGraphs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph<int> graph = new Graph<int>();
            Random rand = new Random(34);

            int s = 400;
            int y = 1600;
            int search = 120;

            #nullable disable
            //graph.AddVal(1, 1, 1);
            //graph.AddVal(0, 0, 0);
            //graph.AddEdge(graph.Search(1), graph.Search(0), 0);
            //graph.AddEdge(graph.Search(0), graph.Search(1),-40);

            #region graphCreation
            for (int i = 0; i < s; i++)
            {
                graph.AddVal(i, rand.Next(s), rand.Next(s));
            }
#nullable disable
            for (int i = 0; i < y; i++)
            {
                graph.AddEdge(graph.Search(rand.Next(s)), graph.Search(rand.Next(s)), rand.Next(s));
            }
            #endregion

            #region breathSearch
            //            Console.WriteLine("Breath: ");
            //            List<Vertex<int>> q = graph.BreathFirstSearch(graph.Search(1), graph.Search(search));
            //            if (q != null)
            //            {
            //                for (int i = 0; i < q.Count; i++)
            //                {
            //                    Console.Write($"{q[i].Value}, ");
            //                }
            //                Console.WriteLine();
            //            }
            //            else
            //            {
            //                Console.WriteLine("no path");
            //            }
            //            graph.ResetVisited(graph);
            //            #endregion

            //            #region depthSearch
            //            Console.WriteLine("Depth: ");
            //            List<Vertex<int>> d = graph.DepthFirstSearch(graph.Search(1), graph.Search(search));
            //#nullable enable
            //            if (q != null)
            //            {
            //                for (int i = 0; i < d.Count; i++)
            //                {
            //                    Console.Write($"{d[i].Value},");
            //                }
            //                Console.WriteLine();
            //            }
            //            else
            //            {
            //                Console.WriteLine("no path");
            //            }
            //            graph.ResetVisited(graph);
            #endregion

            #region dijestraSearch

            //            Console.WriteLine("Dijkstra: ");
            //            List<Vertex<int>> m = graph.DijkstraSeach(graph, graph.Search(1), graph.Search(search));
            //            if (m != null)
            //            {
            //                for (int i = 0; i < m.Count; i++)
            //                {
            //                    Console.Write($"{m[i].Value}, ");
            //                }
            //                Console.WriteLine();
            //            }
            //            else
            //            {
            //                Console.WriteLine("no path");
            //            }
            #endregion

            #region A*
            //            Console.WriteLine("A*: ");
            //            List<Vertex<int>> a = graph.AStarSearch(graph, graph.Search(1), graph.Search(search));
            //            if (a != null)
            //            {
            //                for (int i = 0; i < a.Count; i++)
            //                {
            //                    Console.Write($"{a[i].Value}, ");
            //                }
            //                Console.WriteLine();
            //            }
            //            else
            //            {
            //                Console.WriteLine("no path");
            //            }
            #endregion

            #region BellmanFord
            Console.WriteLine("Bellman Ford: ");
            List<Vertex<int>> b = graph.BellmanFord(graph, graph.Search(1));
            if(b == null)
            {
                Console.WriteLine("No negative cycles");
            }
            else
            {
                Console.WriteLine("Found negative cycle");
            }

            #endregion

        }
    }
}