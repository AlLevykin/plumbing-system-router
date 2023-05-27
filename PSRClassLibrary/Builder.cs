using QuickGraph;
using QuickGraph.Algorithms;
using System;
using System.Collections.Generic;

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
                    if (wall.Length() == (wall.FirstPoint.DistanceTo(entry.Center) + wall.SecondPoint.DistanceTo(entry.Center))) count++;
                }
                if (count == 0) return false;
            }
            return true;
        }

        public static bool Build(Module module, Action<string> statusCallback = null) 
        {
            statusCallback?.Invoke("Начат расчет системы водоотведения.");
            statusCallback?.Invoke("Проверка исходных данных");
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
            statusCallback?.Invoke("Расчет минимального покрывающего дерева");
            statusCallback?.Invoke("Подбор фиттингов");
            statusCallback?.Invoke("Подбор редукций");
            statusCallback?.Invoke("Подбор труб");
            statusCallback?.Invoke("Расчет системы водоотведения завершен.");

            return true;
        }
    }
}
