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

            graph.AddEdge(right, left, 10);
            //graph.RemoveEdge(right, left);

            Edge<int> e = graph.GetEdge(right, left); 

            Vertex<int> s = graph.Search(1);
        }
    }
}