using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TestTheRest.Algorithms;
using TestTheRest.Models;

namespace TestTheRest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouteSelectionController : ControllerBase
    {
        private readonly int maxFlights = -1;

        public RouteSelectionController()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();

            IConfigurationSection section = config.GetSection("MaxFligths");

            if (section != null && !string.IsNullOrEmpty(section.Value))
            {
                int.TryParse(section.Value, out maxFlights);
            }

            if (maxFlights <= 0)
            {
                Console.WriteLine("Wrong stops quantity");
            }
        }

        [HttpGet("{source}/{destination}")]
        public Response Get(string source, string destination)
        {
            if (source.Length != 3 && source.Length != 4)
            {
                return new Response(false, HttpStatusCode.BadRequest, "Wrong length source airport IATA/ICAO", null);
            }
            else if (destination.Length != 3 && destination.Length != 4)
            {
                return new Response(false, HttpStatusCode.BadRequest, "Wrong length destination airport IATA/ICAO", null);
            }      

            int sourceId = -1;
            int destinationId = -1;

            foreach (GraphVertex vertex in InitData.graph.Vertices)
            {
                if (sourceId == -1 &&
                    ( (source.Length == 3 && source == vertex.IATA) ||
                      (source.Length == 4 && source == vertex.ICAO) ))
                {
                    sourceId = vertex.Id;
                }

                if (destinationId == -1 &&
                    ( (destination.Length == 3 && destination == vertex.IATA) ||
                      (destination.Length == 4 && destination == vertex.ICAO) ))
                {
                    destinationId = vertex.Id;
                }

                if (sourceId != -1 && destinationId != -1)
                {
                    break;
                }
            }

            if (sourceId == -1)
            {
                return new Response(false, HttpStatusCode.NotFound, "Source airport is not found", null);
            }
            else if (destinationId == -1)
            {
                return new Response(false, HttpStatusCode.NotFound, "Destination airport is not found", null);
            }

            string path = new Dijkstra(InitData.graph).FindShortestPath(sourceId, destinationId, maxFlights);

            return !string.IsNullOrEmpty(path) ? new Response(true, HttpStatusCode.OK, null, path) :
                new Response(true, HttpStatusCode.NotFound,
                    "No way to get destination by " + maxFlights.ToString() + " flights",
                    null);
        }
    }
}
