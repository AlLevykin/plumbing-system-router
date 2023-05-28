using QuickGraph;
using QuickGraph.Algorithms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace PSR
{
    public static class Builder
    {
        private static bool CheckWalls(Module module)
        {
            if (module == null) return false;
            if (module.Walls == null) return false;
            if (module.Walls.Count == 0) return false;

            UndirectedGraph<Point, Edge<Point>> graph = new UndirectedGraph<Point, Edge<Point>>();
            foreach (Wall wall in module.Walls)
            {
                graph.AddVertex(wall.FirstPoint);
                graph.AddVertex(wall.SecondPoint);
                graph.AddEdge(new Edge<Point>(wall.FirstPoint, wall.SecondPoint));
            }

            Point root = module.Walls[0].FirstPoint;
            Func<Edge<Point>, double> EdgeCost = e => e.Source.DistanceTo(e.Target);
            var TryGetPaths = graph.ShortestPathsDijkstra(EdgeCost, root);
            foreach (Point target in graph.Vertices)
            {
                if ((root != target) && (!TryGetPaths(target, out IEnumerable<Edge<Point>> path))) return false;
            }

            return true;
        }

        private static bool CheckDrains(Module module)
        {
            if (module == null) return false;
            if (module.Drains == null) return false;
            if (module.Drains.Count == 0) return false;
            foreach (Entry entry in module.Drains)
            {
                if (entry.Diameter == 0) return false;
                int count = 0;
                foreach (Wall wall in module.Walls)
                {
                    if (wall.Length ==
                       (wall.FirstPoint.DistanceTo(new Point() { X = entry.Center.X, Y = entry.Center.Y, Z = 0 })
                       +
                       wall.SecondPoint.DistanceTo(new Point() { X = entry.Center.X, Y = entry.Center.Y, Z = 0 })))
                        count++;
                }
                if (count == 0) return false;
            }
            return true;
        }

        private static bool Route(Module module)
        {
            const double step = 5;

            Stack<Entry> entries = new Stack<Entry>();
            UndirectedGraph<Point, Edge<Point>> graph = new UndirectedGraph<Point, Edge<Point>>();
            foreach (Wall wall in module.Walls)
            {
                List<Point> points = wall.Points2D(step);
                if (points.Count == 0) continue;

                graph.AddVertex(points[0]);

                for (int i = 1; i < points.Count; i++)
                {
                    graph.AddVertex(points[i]);
                    graph.AddEdge(new Edge<Point>(points[i - 1], points[i]));
                }
            }

            Point nearestPoint = null;
            double minDistance = double.MaxValue;
            foreach (Point p in graph.Vertices)
            {
                if ((nearestPoint == null) || (module.VentStack.Center.DistanceTo(p) < minDistance))
                {
                    minDistance = module.VentStack.Center.DistanceTo(p);
                    nearestPoint = p;
                }
            }
            graph.AddVertex(module.VentStack.Center);
            graph.AddEdge(new Edge<Point>(nearestPoint, module.VentStack.Center));
            entries.Push(module.VentStack);

            foreach (Entry entry in module.Drains)
            {
                Point nearestP = null;
                double minD = double.MaxValue;
                foreach (Point p in graph.Vertices)
                {
                    if (entry.Center.DistanceTo(p) < minD && p.Z == 0)
                    {
                        minD = entry.Center.DistanceTo(p);
                        nearestP = p;
                    }
                }
                graph.AddVertex(entry.Center);
                if (entry.Center.Z != nearestP.Z)
                {
                    graph.AddEdge(new Edge<Point>(nearestP, entry.Center));
                    entries.Push(entry);
                }
                else
                {
                    Point point = new Point() { X = nearestP.X, Y = nearestP.Y, Z = entry.Center.Z };
                }
            }

            UndirectedGraph<Point, Edge<Point>> mstGraph = new UndirectedGraph<Point, Edge<Point>>();
            Func<Edge<Point>, double> EdgeCost = e => e.Source.DistanceTo(e.Target);

            while (entries.Count > 0)
            {
                Entry curEntry = entries.Pop();
                foreach (Entry entry in entries)
                {
                    var TryGetPaths = graph.ShortestPathsDijkstra(EdgeCost, curEntry.Center);
                    IEnumerable<Edge<Point>> path;
                    if (TryGetPaths(entry.Center, out path))
                    {
                        foreach (Edge<Point> edge in path)
                        {
                            mstGraph.AddVertex(edge.Source);
                            mstGraph.AddVertex(edge.Target);
                            mstGraph.AddEdge(edge);
                        }
                    }
                }
            }

            IEnumerable<Edge<Point>> mst = mstGraph.MinimumSpanningTreePrim(EdgeCost);
            graph = new UndirectedGraph<Point, Edge<Point>>();
            foreach (Edge<Point> edge in mst)
            {
                graph.AddVertex(edge.Source);
                graph.AddVertex(edge.Target);
            }
            graph.AddEdgeRange(mst);

            List<Point> errors = new List<Point>();
            List<Point> tripls = new List<Point>();
            List<Point> sockets = new List<Point>();
            List<Point> crosses = new List<Point>();
            foreach (Point point in graph.Vertices)
            {
                int adjacentDegree = graph.AdjacentDegree(point);
                switch (adjacentDegree)
                {
                    case 1:
                        sockets.Add(point);
                        break;
                    case 2:
                        IEnumerable<Edge<Point>> adjacentEdges = graph.AdjacentEdges(point);
                        break;
                    case 3:
                        tripls.Add(point);
                        break;
                    case 4:
                        crosses.Add(point);
                        break;
                    default:
                        errors.Add(point);
                        break;
                }
            }

            return true;
        }

        public static bool Build(Module module, Action<string> statusCallback = null)
        {
            statusCallback?.Invoke("Начат расчет системы водоотведения.");
            statusCallback?.Invoke("Проверка исходных данных.");
            if (!CheckWalls(module))
            {
                statusCallback?.Invoke("Контур стен в помещении задан неверно.");
                return false;
            }
            if (!CheckDrains(module))
            {
                statusCallback?.Invoke("Размещение потребителей задано неверно.");
                return false;
            }
            statusCallback?.Invoke("Трассировка.");
            if (!Route(module))
            {
                statusCallback?.Invoke("Трассировка завершилась с ошибкой.");
                return false;
            }
            statusCallback?.Invoke("Подбор труб и редукций.");
            statusCallback?.Invoke("Подбор фиттингов.");
            statusCallback?.Invoke("Расчет системы водоотведения завершен.");

            return true;
        }
    }
}
