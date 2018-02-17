using UnityEngine;
using UnityEditor;
using RoaringFangs.EditorInternal;
using System.IO;
using System.Linq;
using System.Collections.Generic;

[InitializeOnLoad]
static class AssemblyController
{
    static readonly string OdinMCSDefine = "-define:ODIN_INSPECTOR";
    static bool HasOdinInspector()
    {
        string path = Path.Combine(Application.dataPath, "Plugins", "Sirenix", "Odin Inspector");
        return Directory.Exists(path);
    }
    static bool AddDefineToMCS(ref List<string> mcs, string define)
    {
        if (!mcs.Select(s => s.Trim()).Contains(define))
        {
            mcs.Add(define);
            return true;
        }
        return false;
    }
    static AssemblyController()
    {
        //Debug.Log("Starting patching...");
        var patcher = new AssemblyPatcher();
        patcher.Run(Application.dataPath);
        //Debug.Log("Patching complete.");

        var mcs_path = Path.Combine(Application.dataPath, "mcs.rsp");
        var mcs = File.ReadAllLines(mcs_path).ToList();

        if (HasOdinInspector())
            AddDefineToMCS(ref mcs, OdinMCSDefine);

        File.WriteAllLines(mcs_path, mcs.ToArray());
    }
}
