using System.Collections;
using UnityEngine;

public class EnemyFlash : MonoBehaviour
{
    [Header("Flash Materials")]
    public Material materialA;
    public Material materialB;

    [Header("Timings")]
    public float stageDuration = 0.05f;

    private SpriteRenderer[] spriteRenderers;
    private Material[] originalMaterials;
    private Coroutine flashCoroutine;

    void Start()
    {
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
        ApplyMaterialToAll(materialA);
        yield return new WaitForSecondsRealtime(stageDuration);

        ApplyMaterialToAll(materialB);
        yield return new WaitForSecondsRealtime(stageDuration);

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