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

namespace PSR
{
    public class CadPlugIn
    {
        [CommandMethod("BuildPlumbingSystem")]
        public void BuildPlumbingSystem()
        {
            Editor ed = Platform.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("Добро пожаловать в управляемый код nanoCAD.");
        }
    }
}
