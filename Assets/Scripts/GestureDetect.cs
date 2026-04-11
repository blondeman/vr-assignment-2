using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GestureDetect : MonoBehaviour
{
    [Header("Settings")]
    public bool useRightHand = true;

    [Header("Input Actions")]
    public InputActionProperty gripAction;
    public InputActionProperty triggerAction;
    public InputActionProperty thumbAction;

    [Header("Detection Thresholds")]
    [Range(0.0f, 1.0f)] public float gripThreshold    = 0.8f;
    [Range(0.0f, 1.0f)] public float triggerThreshold = 0.8f;
    [Range(0.0f, 1.0f)] public float thumbThreshold   = 0.8f;

    public string currentGesture = "none";

    // Callbacks other scripts can subscribe to
    public System.Action OnPointingStart;
    public System.Action OnPointingEnd;
    public System.Action OnThumbsUpStart;
    public System.Action OnThumbsUpEnd;

    string _previousGesture = "none";

    void OnEnable()
    {
        gripAction.action?.Enable();
        triggerAction.action?.Enable();
        thumbAction.action?.Enable();
    }

    void OnDisable()
    {
        gripAction.action?.Disable();
        triggerAction.action?.Disable();
        thumbAction.action?.Disable();
    }

    void Update()
    {
        float grip    = gripAction.action?.ReadValue<float>()    ?? 0f;
        float trigger = triggerAction.action?.ReadValue<float>() ?? 0f;
        float thumb   = thumbAction.action?.ReadValue<float>()   ?? 0f;

        currentGesture = DetectGesture(grip, trigger, thumb);

        if (currentGesture != _previousGesture)
        {
            // Fire end event for whatever just stopped
            if (_previousGesture == "Pointing")   OnPointingEnd?.Invoke();
            if (_previousGesture == "Thumbs up")  OnThumbsUpEnd?.Invoke();

            // Fire start event for whatever just began
            if (currentGesture == "Pointing")     OnPointingStart?.Invoke();
            if (currentGesture == "Thumbs up")    OnThumbsUpStart?.Invoke();

            _previousGesture = currentGesture;
        }
    }

    string DetectGesture(float grip, float trigger, float thumb)
    {
        bool gripping   = grip    >= gripThreshold;
        bool triggering = trigger >= triggerThreshold;
        bool thumbing   = thumb   >= thumbThreshold;

        // Pointing: grip only — trigger and thumb must be released
        if (gripping && !triggering && !thumbing)
            return "Pointing";

        // Thumbs up: grip + trigger pressed, thumb extended (not pressed)
        if (gripping && triggering && !thumbing)
            return "Thumbs up";

        return "none";
    }

    // Check gesture state any time without waiting for a callback
    public bool IsPointing()  => currentGesture == "Pointing";
    public bool IsThumbsUp()  => currentGesture == "Thumbs up";
}