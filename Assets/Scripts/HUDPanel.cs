using TMPro;
using UnityEngine;

public class HUDPanel : MonoBehaviour
{
    public TextMeshProUGUI trialLabel;
    public TextMeshProUGUI methodLabel;
    public TextMeshProUGUI resultLabel;
    public TextMeshProUGUI pathLabel;

    public void ShowTrial(Trial t)
    {
        trialLabel.text  = $"Trial {t.trialNumber}";
        methodLabel.text = $"Use: {t.gestureName}";
        resultLabel.text = "";
        pathLabel.text   = "";
    }

    public void ShowResult(bool hit)
    {
        resultLabel.color = hit ? Color.green : Color.red;
        resultLabel.text  = hit ? "HIT" : "MISS";
    }

    public void ShowComplete(string path)
    {
        trialLabel.text   = "Experiment complete!";
        methodLabel.text  = "";
        resultLabel.color = Color.white;
        resultLabel.text  = "";
        pathLabel.text    = $"CSV saved:\n{path}";
    }
}