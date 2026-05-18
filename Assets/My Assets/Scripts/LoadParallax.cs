using UnityEngine;

public class LoadParallax : MonoBehaviour
{
    public GameObject[] bgs;
    public Collider2D zoneBounds; // assign BoxCollider2D

    private Camera cam;
    private bool wasInside = false;

    void Start()
    {
        cam = Camera.main;

        // Start with everything disabled
        for (int i = 0; i < bgs.Length; i++)
        {
            if (bgs[i] != null)
                bgs[i].SetActive(false);
        }
    }

    void Update()
    {
        if (cam == null || zoneBounds == null) return;

        Vector2 camPos = cam.transform.position;
        Bounds b = zoneBounds.bounds;

        bool inside =
            camPos.x > b.min.x && camPos.x < b.max.x &&
            camPos.y > b.min.y && camPos.y < b.max.y;

        // Only react when state changes
        if (inside == wasInside) return;

        wasInside = inside;

        for (int i = 0; i < bgs.Length; i++)
        {
            if (bgs[i] != null)
                bgs[i].SetActive(inside);
        }
    }
}