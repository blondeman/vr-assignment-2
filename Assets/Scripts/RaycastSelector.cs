using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastSelector : MonoBehaviour
{
    public SphereController sphere;
    public string           gestureName = "Ray";

    [Header("Ray origin (controller tip transform)")]
    public Transform rayOrigin;

    ExperimentManager _mgr;

    void Awake() => _mgr = FindObjectOfType<ExperimentManager>();

    void Update()
    {
        // Only active when the current trial requires this gesture
        if (_mgr == null) return;

        bool hit = Physics.Raycast(rayOrigin.position,
                                   rayOrigin.forward,
                                   out RaycastHit info, 10f);

        bool overSphere = hit && info.collider.gameObject == sphere.gameObject;
        sphere.SetHover(overSphere);

        if (overSphere && TriggerPressed())
            sphere.Select();
        else if (overSphere && GripPressed())
            sphere.Miss();
    }

    bool TriggerPressed()
    {
        InputDevice right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.triggerButton, out bool pressed);
        return pressed;
    }

    bool GripPressed()
    {
        InputDevice right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.gripButton, out bool pressed);
        return pressed;
    }
}