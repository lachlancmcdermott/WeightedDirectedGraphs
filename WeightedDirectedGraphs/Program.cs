using System.Runtime.CompilerServices;

namespace WeightedDirectedGraphs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph<int> graph = new Graph<int>();
            Random rand = new Random(100);

            int s = 400;
            int y = 1600;
            int search = 100;

            for (int i = 0; i < s; i++)
            {
                graph.AddVal(i);
            }
#nullable disable
            for (int i = 0; i < y; i++)
            {
                graph.AddEdge(graph.Search(rand.Next(s)), graph.Search(rand.Next(s)), 5);
            }


            Console.WriteLine("Breath: ");
            List<Vertex<int>> q = graph.BreathFirstSearch(graph.Search(1), graph.Search(search));
            if (q != null)
            {
                for (int i = 0; i < q.Count; i++)
                {
                    Console.Write($"{q[i].Value}, ");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("no path");
            }
            graph.ResetVisited(graph);
            Console.WriteLine("Depth: ");
            List<Vertex<int>> d = graph.DepthFirstSearch(graph.Search(1), graph.Search(search));
#nullable enable
            if (q != null)
            {
                for (int i = 0; i < d.Count; i++)
                {
                    Console.Write($"{d[i].Value},");
                }
            }
            else
            {
                Console.WriteLine("no path");
            }
        }
    }
}