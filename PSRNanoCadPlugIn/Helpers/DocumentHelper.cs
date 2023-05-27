using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Teigha.DatabaseServices;
using Teigha.Geometry;

namespace PSR.Cad.Helpers
{
    public enum ReadMode
    {
        Read,
        Refresh
    }

    public class DocumentHelper
    {
        private static Point Point3DToPoint(Point3d point3D) => new() { X = Math.Round(point3D.X), Y = Math.Round(point3D.Y), Z = Math.Round(point3D.Z) };

        private static ObjectIdCollection GetEntitiesOnLayer(Document document, string layerName, string objectType)
        {
            TypedValue[] dxfCodes = {
                new TypedValue(Convert.ToInt32(DxfCode.Operator), "<and"),
                new TypedValue(Convert.ToInt32(DxfCode.LayerName), layerName),
                new TypedValue(Convert.ToInt32(DxfCode.Operator), "<or"),
                new TypedValue(Convert.ToInt32(DxfCode.Start), objectType.ToUpper()),
                new TypedValue(Convert.ToInt32(DxfCode.Operator), "or>"),
                new TypedValue(Convert.ToInt32(DxfCode.Operator), "and>")
            };
            SelectionFilter filter = new SelectionFilter(dxfCodes);
            PromptSelectionResult result = document.Editor.SelectAll(filter);

            if (result.Status == PromptStatus.OK)
            {
                return new ObjectIdCollection(result.Value.GetObjectIds());
            }

            return new ObjectIdCollection();
        }

        public static Module GetPlumbingModule(Document document, ReadMode mode = ReadMode.Read)
        {
            const string key = "PlumbingModule";

            Hashtable userdata = document.UserData;
            Module module = (userdata.ContainsKey(key) && userdata[key] != null && userdata[key].GetType() == typeof(Module)) ?
                (Module)userdata[key]
                :
                new Module();

            switch (mode)
            {
                case ReadMode.Read: break;
                case ReadMode.Refresh:
                    {
                        GetWalls(document, module);
                        GetDrains(document, module);
                        GetVentStack(document, module);
                        break;
                    }
                default: break;
            }

            return module;
        }

        private static void GetWalls(Document document, Module module)
        {
            ObjectIdCollection ids = GetEntitiesOnLayer(document, "Walls", "line");
            module.Walls.Clear();
            Transaction tr = document.TransactionManager.StartTransaction();
            using (tr)
            {
                foreach (ObjectId id in ids)
                {
                    DBObject dbo = tr.GetObject(id, OpenMode.ForWrite);
                    if (dbo.GetType() == typeof(Line))
                    {
                        Line line = (Line)dbo;
                        module.Walls.Add(new Wall
                        {
                            FirstPoint = Point3DToPoint(line.StartPoint),
                            SecondPoint = Point3DToPoint(line.EndPoint)
                        });
                    }
                }
            }
        }

        private static void GetDrains(Document document, Module module)
        {
            ObjectIdCollection ids = GetEntitiesOnLayer(document, "Drains", "circle");
            module.Drains.Clear();
            Transaction tr = document.TransactionManager.StartTransaction();
            using (tr)
            {
                foreach (ObjectId id in ids)
                {
                    DBObject dbo = tr.GetObject(id, OpenMode.ForWrite);
                    if (dbo.GetType() == typeof(Circle))
                    {
                        Circle circle = (Circle)dbo;
                        module.Drains.Add(new Entry
                        {
                            Center = Point3DToPoint(circle.Center),
                            Diameter = Math.Round(circle.Diameter)
                        });
                    }
                }
            }
        }

        private static void GetVentStack(Document document, Module module)
        {
            ObjectIdCollection ids = GetEntitiesOnLayer(document, "VentStack", "circle");
            Transaction tr = document.TransactionManager.StartTransaction();
            using (tr)
            {
                if (ids.Count > 0)
                {
                    DBObject dbo = tr.GetObject(ids[0], OpenMode.ForWrite);
                    if (dbo.GetType() == typeof(Circle))
                    {
                        Circle circle = (Circle)dbo;
                        module.VentStack = new Entry
                        {
                            Center = Point3DToPoint(circle.Center),
                            Diameter = Math.Round(circle.Diameter)
                        };
                    }
                }
            }
        }
    }
}
