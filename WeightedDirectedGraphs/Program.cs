namespace WeightedDirectedGraphs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph<int> graph = new Graph<int>();
            Random rand = new Random(100);

            Vertex<int> A = new Vertex<int>(rand.Next(10));
            Vertex<int> B = new Vertex<int>(rand.Next(10));

            for (int i = 0;i < 12; i++)
            {
                graph.AddEdge(A, B, rand.Next(10));
            }

            Queue<Vertex<int>> q = graph.BreathFirstSearch(A, B);
        }
    }
}