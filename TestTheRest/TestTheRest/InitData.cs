using System;
using System.IO;
using TestTheRest.Algorithms;
using TestTheRest.Models;

namespace TestTheRest
{
    public static class InitData
    {
        private static Airport[] airports = new Airport[0];
        private static Route[] routes = new Route[0];

        public static Graph graph = new();

        public static void CreateDataBase(string path)
        {
            LoadDataFiles(path);
            MapDataToGraph();
        }

        private static long GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2)
        {
            double dLat = (lat2 - lat1) / 180 * Math.PI;
            double dLong = (long2 - long1) / 180 * Math.PI;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                        + Math.Cos(lat1 / 180 * Math.PI) * Math.Cos(lat2 / 180 * Math.PI)
                        * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            //Calculate radius of earth
            // For this you can assume any of the two points.
            double radiusE = 6378135; // Equatorial radius, in metres
            double radiusP = 6356750; // Polar Radius

            //Numerator part of function
            double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
            //Denominator part of the function
            double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                            + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
            double radius = Math.Sqrt(nr / dr);

            //Calculate distance in meters.
            return (long)Math.Round(radius * c); // distance in meters
        }

        private static void LoadDataFiles(string path)
        {
            // Load airports.dat
            using (StreamReader sr = new(path + "airports.dat"))
            {
                string row;
                while ((row = sr.ReadLine()) != null)
                {
                    string[] values = row.Split(',');
                    
                    int.TryParse(values[0], out int id);
                    double.TryParse(values[0], out double latitude);
                    double.TryParse(values[0], out double longitude);
                    

                    if (id > 0 && latitude > 0 && longitude > 0)
                    {
                        Array.Resize(ref airports, airports.Length + 1);
                        airports[airports.Length - 1] = new();

                        airports[airports.Length - 1].Id = id;
                        airports[airports.Length - 1].Name = values[1].Replace("\"", "");
                        airports[airports.Length - 1].City = values[2].Replace("\"", "");
                        airports[airports.Length - 1].Country = values[3].Replace("\"", "");
                        airports[airports.Length - 1].IATA = values[4].Replace("\"", "");
                        airports[airports.Length - 1].ICAO = values[5].Replace("\"", "");
                        airports[airports.Length - 1].Latitude = latitude;
                        airports[airports.Length - 1].Longitude = longitude;
                    }
                }
            }

            // Load routes.dat
            using (StreamReader sr = new(path + "routes.dat"))
            {
                string row;
                while ((row = sr.ReadLine()) != null)
                {
                    string[] values = row.Split(',');
                    
                    int.TryParse(values[1], out int airlineId);
                    int.TryParse(values[3], out int sourceId);
                    int.TryParse(values[5], out int destinationId);
                    
                    if (airlineId > 0 && sourceId > 0 && destinationId > 0)
                    {
                        Array.Resize(ref routes, routes.Length + 1);
                        routes[routes.Length - 1] = new();

                        routes[routes.Length - 1].Airline = values[0].Replace("\"", "");
                        routes[routes.Length - 1].AirlineId = airlineId;
                        routes[routes.Length - 1].Source = values[2].Replace("\"", "");
                        routes[routes.Length - 1].SourceId = sourceId;
                        routes[routes.Length - 1].Destination = values[4].Replace("\"", "");
                        routes[routes.Length - 1].DestinationId = destinationId;
                    }
                }
            }
        }

        private static void MapDataToGraph()
        {
            foreach (Airport airport in airports)
            {
                graph.AddVertex(airport.Id, airport.IATA, airport.ICAO);
            }

            foreach (Route route in routes)
            {
                Airport sourceAirport = null;
                Airport destinationAirport = null;
                foreach (Airport airport in airports)
                {
                    if (sourceAirport == null && airport.Id == route.SourceId)
                    {
                        sourceAirport = airport;
                    }
                        
                    if (destinationAirport == null && airport.Id == route.DestinationId)
                    {
                        destinationAirport = airport;
                    }

                    if (sourceAirport != null && destinationAirport != null)
                    {
                        break;
                    }
                }

                if (sourceAirport == null || destinationAirport == null)
                {
                    continue;
                }

                graph.AddEdge(route.SourceId, route.DestinationId,
                    GetDistanceBetweenPoints(
                        sourceAirport.Latitude,
                        sourceAirport.Longitude,
                        destinationAirport.Latitude,
                        destinationAirport.Longitude));
            }
        }
    }
}
