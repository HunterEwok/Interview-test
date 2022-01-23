using System.Collections.Generic;

namespace TestTheRest.Algorithms
{
    public class Graph
    {
        public List<GraphVertex> Vertices { get; }

        public Graph()
        {
            Vertices = new List<GraphVertex>();
        }

        public void AddVertex(int id, string IATA, string ICAO)
        {
            Vertices.Add(new GraphVertex(id, IATA, ICAO));
        }

        public GraphVertex FindVertex(int vertexId)
        {
            foreach (GraphVertex v in Vertices)
            {
                if (v.Id == vertexId)
                {
                    return v;
                }
            }

            return null;
        }

        public void AddEdge(int sourceVertex, int destinationVertex, long weight = 0)
        {
            GraphVertex v1 = FindVertex(sourceVertex);
            GraphVertex v2 = FindVertex(destinationVertex);

            if (v1 != null && v2 != null)
            {
                v1.AddEdge(v2, weight);
                v2.AddEdge(v1, weight);
            }
        }
    }
}
