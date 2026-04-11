using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CSVLogger : MonoBehaviour
{
    public string   groupName = "GroupA";
    public HUDPanel hud;                   // add this field, wire in Inspector

    readonly List<Trial> _rows = new();

    public void Log(Trial t) => _rows.Add(t);

    public string Save()
    {
        string path = Path.Combine(Application.persistentDataPath,
                                   $"{groupName}_Outputfile.csv");

        var sb = new StringBuilder();
        sb.AppendLine("Trial,Gesture,Distance_m,Diameter_m,ID,MovementTime_s,Hit,Timestamp");

        foreach (var t in _rows)
        {
            float id = Mathf.Log(2f * t.distance / t.diameter, 2f);
            sb.AppendLine($"{t.trialNumber},{t.gestureName}," +
                          $"{t.distance:F3},{t.diameter:F3}," +
                          $"{id:F3},{t.movementTime:F3},{t.hit},{t.startTimestamp:F2}");
        }

        File.WriteAllText(path, sb.ToString());
        Debug.Log($"CSV saved: {path}");

        hud?.ShowComplete(path);           // update HUD directly from logger
        return path;
    }
}