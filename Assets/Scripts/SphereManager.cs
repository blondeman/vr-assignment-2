using UnityEngine;

public class SphereController : MonoBehaviour
{
    [Header("Materials")]
    public Material blueMaterial;
    public Material greenMaterial;

    [Header("References")]
    public ExperimentManager manager;
    public Renderer     _rend;

    Trial        _current;
    bool         _active;
    bool         _hovering;

    public void SpawnSphere(Trial t)
    {
        _current  = t;
        _active   = true;
        _hovering = false;

        // Use camera position as origin so the ball is always in front of the player's eyes
        Transform cam = Camera.main.transform;
        Vector3 worldDir = cam.TransformDirection(t.direction);
        Vector3 pos = cam.position + worldDir * t.distance;

        transform.position   = pos;
        transform.localScale = Vector3.one * t.diameter;
        _rend.material = blueMaterial;
        gameObject.SetActive(true);
    }

    public void HideSphere() => gameObject.SetActive(false);

    // Called each frame by the active input handler
    public void SetHover(bool hovering)
    {
        if (!_active) return;
        if (hovering == _hovering) return;
        _hovering = hovering;
        _rend.material = hovering ? greenMaterial : blueMaterial;
    }

    // Called by the active input handler on confirmed selection
    public void Select()
    {
        if (!_active) return;
        _active = false;
        gameObject.SetActive(false);
        manager.RegisterSelection(hit: true);
    }
    
    public void Miss()
    {
        if (!_active) return;
        _active = false;
        gameObject.SetActive(false);
        manager.RegisterSelection(hit: false);
    }
}