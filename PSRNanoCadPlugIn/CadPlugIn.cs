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
        private void StatusInfoCallback (string status)
        {
            Document dwg = Application.DocumentManager.MdiActiveDocument;
            dwg.Editor.WriteMessage(status);
        }
        
        [CommandMethod("BuildPlumbingSystem")]
        public void BuildPlumbingSystem()
        {
            Document dwg = Application.DocumentManager.MdiActiveDocument;
            dwg.Editor.WriteMessage("Получение исходных данных о системе водоотведения.");

            Module module = Helpers.DocumentHelper.GetPlumbingModule(dwg, Helpers.ReadMode.Refresh);
            
            dwg.Editor.WriteMessage("Количество стен:{0}", module.Walls.Count);
            foreach (var wall in module.Walls) dwg.Editor.WriteMessage(wall.ToString());

            dwg.Editor.WriteMessage("Количество потребителей:{0}", module.Drains.Count);
            foreach (var drain in module.Drains) dwg.Editor.WriteMessage(drain.ToString());

            dwg.Editor.WriteMessage("Стояк:{0}", module.VentStack.ToString());

            Builder.Build(module, StatusInfoCallback);

            dwg.Editor.WriteMessage("Результаты расчета:");

            if (module.tubeLength > 0) dwg.Editor.WriteMessage("Общая длина труб:{0}", module.tubeLength);
            if (module.sockets.Count > 0)
            {
                dwg.Editor.WriteMessage("Патрубки:{0}", module.sockets.Count);
                foreach(var socket in module.sockets) dwg.Editor.WriteMessage("{0}", socket.ToString());
            }
            if (module.tripls.Count > 0)
            {
                dwg.Editor.WriteMessage("Тройники:{0}", module.tripls.Count);
                foreach (var tripl in module.tripls) dwg.Editor.WriteMessage("{0}", tripl.ToString());
            }
            if (module.crosses.Count > 0)
            {
                dwg.Editor.WriteMessage("Крестовины:{0}", module.crosses.Count);
                foreach (var cross in module.crosses) dwg.Editor.WriteMessage("{0}", cross.ToString());
            }
            if (module.angles30.Count > 0)
            {
                dwg.Editor.WriteMessage("Отводы 30 градусов:{0}", module.angles30.Count);
                foreach (var angle in module.angles30) dwg.Editor.WriteMessage("{0}", angle.ToString());
            }
            if (module.angles45.Count > 0)
            {
                dwg.Editor.WriteMessage("Отводы 45 градусов:{0}", module.angles45.Count);
                foreach (var angle in module.angles45) dwg.Editor.WriteMessage("{0}", angle.ToString());
            }
            if (module.angles90.Count > 0)
            {
                dwg.Editor.WriteMessage("Отводы 90 градусов:{0}", module.angles90.Count);
                foreach (var angle in module.angles90) dwg.Editor.WriteMessage("{0}", angle.ToString());
            }
        }
    }
}
