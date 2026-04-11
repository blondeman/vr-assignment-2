using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Trial
{
    public int    trialNumber;
    public string gestureName;       // "Pinch" | "Ray"
    public float  distance;          // metres from origin
    public float  diameter;          // sphere diameter in metres
    public Vector3 direction;        // normalised spawn direction

    // Recorded at runtime
    [NonSerialized] public float  movementTime;
    [NonSerialized] public bool   hit;
    [NonSerialized] public float  reactionTime;
    [NonSerialized] public float  startTimestamp;
}

[CreateAssetMenu(menuName = "Experiment/TrialConfig")]
public class TrialConfig : ScriptableObject
{
    [Header("Conditions")]
    public string[]  gestures   = { "Pinch", "Ray" };
    public float[]   distances  = { 0.5f, 1.0f };
    public float[]   sizes      = { 0.05f, 0.10f };
    public int       repetitions = 2;

    [Header("Spawn directions (normalised)")]
    public Vector3[] directions = {
    new Vector3( 0,    0,    1).normalized,   // straight ahead
    new Vector3( 0.4f, 0,    1).normalized,   // slightly right
    new Vector3(-0.4f, 0,    1).normalized,   // slightly left
    new Vector3( 0,    0.2f, 1).normalized,   // slightly up
};

    public List<Trial> BuildLatinSquare()
    {
        var trials = new List<Trial>();
        int dirIdx = 0, n = 1;

        // Latin-square counterbalancing over gesture order
        for (int rep = 0; rep < repetitions; rep++)
        {
            for (int gOffset = 0; gOffset < gestures.Length; gOffset++)
            {
                // Alternate gesture order each rep (simple 2-condition LS)
                int gIdx = (rep + gOffset) % gestures.Length;
                foreach (float d in distances)
                    foreach (float s in sizes)
                    {
                        trials.Add(new Trial
                        {
                            trialNumber = n++,
                            gestureName = gestures[gIdx],
                            distance    = d,
                            diameter    = s,
                            direction   = directions[dirIdx % directions.Length]
                        });
                        dirIdx++;
                    }
            }
        }
        return trials;
    }
}