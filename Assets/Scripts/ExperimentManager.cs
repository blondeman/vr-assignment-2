using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExperimentManager : MonoBehaviour
{
    [Header("Config")]
    public TrialConfig config;

    [Header("References")]
    public SphereController sphereController;
    public HUDPanel         hud;
    public CSVLogger        logger;

    public UnityEvent<Trial> OnTrialStart;
    public UnityEvent<Trial> OnTrialEnd;

    List<Trial> _trials;
    int         _current;
    bool        _running;

    void Start()
    {
        _trials = config.BuildLatinSquare();
        StartNextTrial();
    }

    void StartNextTrial()
    {
        if (_current >= _trials.Count)
        {
            EndExperiment(); return;
        }
        var t = _trials[_current];
        t.startTimestamp = Time.time;
        sphereController.SpawnSphere(t);
        hud.ShowTrial(t);
        OnTrialStart.Invoke(t);
        _running = true;
    }

    // Called by SphereController when the sphere is selected
    public void RegisterSelection(bool hit)
    {
        if (!_running) return;
        _running = false;

        var t = _trials[_current];
        t.movementTime = Time.time - t.startTimestamp;
        t.hit          = hit;
        logger.Log(t);
        hud.ShowResult(hit);

        _current++;
        Invoke(nameof(StartNextTrial), 1.2f);   // short pause between trials
    }

    void EndExperiment()
    {
        string path = logger.Save();
        hud.ShowComplete(path);
        sphereController.HideSphere();
    }
}