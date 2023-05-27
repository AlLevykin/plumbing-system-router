#if NCAD
using Teigha.DatabaseServices;
using Teigha.Runtime;
using Teigha.Geometry;
using HostMgd.ApplicationServices;
using HostMgd.EditorInput;

using Platform = HostMgd;
using PlatformDb = Teigha;
#else
  using Autodesk.AutoCAD.ApplicationServices;
  using Autodesk.AutoCAD.DatabaseServices;
  using Autodesk.AutoCAD.EditorInput;
  using Autodesk.AutoCAD.Geometry;
  using Autodesk.AutoCAD.Runtime;
  using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

  using Platform = Autodesk.AutoCAD;
  using PlatformDb = Autodesk.AutoCAD;
#endif

namespace PSR.Cad
{
    public class PlugIn
    {
        [CommandMethod("BuildPlumbingSystem")]
        public void BuildPlumbingSystem()
        {
            Document dwg = Platform.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            dwg.Editor.WriteMessage("Начат расчет системы водоотведения.");

            Module module = Helpers.DocumentHelper.GetPlumbingModule(dwg, Helpers.ReadMode.Refresh);
            
            dwg.Editor.WriteMessage("Количество стен:{0}", module.Walls.Count);
            foreach (var wall in module.Walls) dwg.Editor.WriteMessage(wall.ToString());

            dwg.Editor.WriteMessage("Количество потребителей:{0}", module.Drains.Count);
            foreach (var drain in module.Drains) dwg.Editor.WriteMessage(drain.ToString());

            dwg.Editor.WriteMessage("Стояк:{0}", module.VentStack.ToString());
        }
    }
}
