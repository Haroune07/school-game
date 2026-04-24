using UnityEngine;

public class ParallaxLoop : MonoBehaviour
{
    [Header("Parallax")]
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f;

    [Header("Loop Settings")]
    public Transform[] tiles; // assign Tile_1 and Tile_2

    private Transform cam;
    private Vector3 lastCamPos;
    private float tileWidth;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;

        // get width from first tile
        tileWidth = tiles[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - lastCamPos;

        // move layer (parallax)
        transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);

        lastCamPos = cam.position;

        // loop tiles
        foreach (Transform tile in tiles)
        {
            float distance = cam.position.x - tile.position.x;

            if (Mathf.Abs(distance) >= tileWidth)
            {
                float offset = (distance > 0) ? tileWidth * tiles.Length : -tileWidth * tiles.Length;
                tile.position += new Vector3(offset, 0, 0);
            }
        }
    }
}