namespace TestTheRest.Algorithms
{
    public class GraphEdge
    {
        public GraphVertex ConnectedVertex { get; }
        public long EdgeWeight { get; }

        public GraphEdge(GraphVertex connectedVertex, long weight)
        {
            ConnectedVertex = connectedVertex;
            EdgeWeight = weight;
        }
    }
}
