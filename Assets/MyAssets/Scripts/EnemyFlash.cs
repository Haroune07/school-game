using System.Collections;
using UnityEngine;

public class EnemyFlash : MonoBehaviour
{
    [Header("Flash Materials")]
    public Material materialA; // Drag WhiteFlashMat here
    public Material materialB; // Drag BlackFlashMat here

    [Header("Timings")]
    public float stageDuration = 0.05f; // Duration for each color phase

    private SpriteRenderer[] spriteRenderers;
    private Material[] originalMaterials;
    private Coroutine flashCoroutine;

    void Start()
    {
        // Grab all child sprites
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        originalMaterials = new Material[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalMaterials[i] = spriteRenderers[i].material;
        }
    }

    public void TriggerFlash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(DoubleFlashRoutine());
    }

    private IEnumerator DoubleFlashRoutine()
    {
        // --- STAGE 1: Flash Color A (White) ---
        ApplyMaterialToAll(materialA);
        yield return new WaitForSecondsRealtime(stageDuration);

        // --- STAGE 2: Flash Color B (Black) ---
        ApplyMaterialToAll(materialB);
        yield return new WaitForSecondsRealtime(stageDuration);

        // --- STAGE 3: Revert to Normal ---
        RevertMaterials();
    }

    private void ApplyMaterialToAll(Material mat)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
            {
                spriteRenderers[i].material = mat;
            }
        }
    }

    private void RevertMaterials()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
            {
                spriteRenderers[i].material = originalMaterials[i];
            }
        }
    }
}