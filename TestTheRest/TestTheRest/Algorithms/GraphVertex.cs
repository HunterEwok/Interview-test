using System.Collections.Generic;

namespace TestTheRest.Algorithms
{
    public class GraphVertex
    {
        public int Id { get; }
        public string IATA { get; }
        public string ICAO { get; }
        public List<GraphEdge> Edges { get; }

        public GraphVertex(int id, string iata, string icao)
        {
            Id = id;
            IATA = iata;
            ICAO = icao;
            Edges = new List<GraphEdge>();
        }

        public void AddEdge(GraphEdge newEdge)
        {
            Edges.Add(newEdge);
        }

        public void AddEdge(GraphVertex vertex, long edgeWeight)
        {
            AddEdge(new GraphEdge(vertex, edgeWeight));
        }
    }
}
