namespace WeightedDirectedGraphs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph<int> graph = new Graph<int>();

            Vertex<int> right = new Vertex<int>(1);
            Vertex<int> left = new Vertex<int>(0);
            
            graph.AddVertex(right);
            graph.AddVertex(left);
        }
    }
}