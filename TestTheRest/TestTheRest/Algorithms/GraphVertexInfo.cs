namespace TestTheRest.Algorithms
{
    public class GraphVertexInfo
    {
        public GraphVertex Vertex { get; set; }
        public bool IsUnvisited { get; set; }
        public long EdgesWeightSum { get; set; }
        public GraphVertex PreviousVertex { get; set; }

        public GraphVertexInfo(GraphVertex vertex)
        {
            Vertex = vertex;
            IsUnvisited = true;
            EdgesWeightSum = long.MaxValue;
            PreviousVertex = null;
        }
    }
}