using System.Collections.Generic;

namespace TestTheRest.Algorithms
{
    public class Dijkstra
    {
        private readonly Graph graph;
        private List<GraphVertexInfo> infos;

        public Dijkstra(Graph graph)
        {
            this.graph = graph;
        }

        private void InitInfo()
        {
            infos = new List<GraphVertexInfo>();

            foreach (GraphVertex v in graph.Vertices)
            {
                infos.Add(new GraphVertexInfo(v));
            }
        }
        
        private GraphVertexInfo GetVertexInfo(GraphVertex v)
        {
            foreach (GraphVertexInfo i in infos)
            {
                if (i.Vertex.Id == v.Id)
                {
                    return i;
                }
                    
            }

            return null;
        }

        private GraphVertexInfo FindUnvisitedVertexWithMinSum()
        {
            long minValue = long.MaxValue;
            GraphVertexInfo minVertexInfo = null;

            foreach (GraphVertexInfo i in infos)
            {
                if (i.IsUnvisited && i.EdgesWeightSum < minValue)
                {
                    minVertexInfo = i;
                    minValue = i.EdgesWeightSum;
                }
            }

            return minVertexInfo;
        }

        public string FindShortestPath(int sourceId, int destinationId, int maxSteps)
        {
            return FindShortestPath(graph.FindVertex(sourceId), graph.FindVertex(destinationId), maxSteps);
        }

        private string FindShortestPath(GraphVertex sourceVertex, GraphVertex destinationVertex, int maxSteps)
        {
            InitInfo();
            GraphVertexInfo first = GetVertexInfo(sourceVertex);
            first.EdgesWeightSum = 0;
            bool isDestinationReached = false;

            while (!isDestinationReached)
            {
                GraphVertexInfo current = FindUnvisitedVertexWithMinSum();
                if (current == null)
                {
                    break;
                }

                SetSumToNextVertex(current);

                if (current.Vertex.Id == destinationVertex.Id)
                {
                    isDestinationReached = true;
                }
            }

            return GetPath(sourceVertex, destinationVertex, maxSteps);
                
        }

        private void SetSumToNextVertex(GraphVertexInfo info)
        {
            info.IsUnvisited = false;

            foreach (GraphEdge e in info.Vertex.Edges)
            {
                GraphVertexInfo nextInfo = GetVertexInfo(e.ConnectedVertex);
                long sum = info.EdgesWeightSum + e.EdgeWeight;

                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Vertex;
                }
            }
        }

        private string GetPath(GraphVertex sourceVertex, GraphVertex destinationVertex, int maxSteps)
        {
            string path = destinationVertex.IATA;
            int i = 0;

            while (sourceVertex != destinationVertex && ++i <= maxSteps)
            {
                destinationVertex = GetVertexInfo(destinationVertex).PreviousVertex;
                path = destinationVertex.IATA + "->" + path;
            }

            return i <= maxSteps ? path : null;
        }
    }
}
