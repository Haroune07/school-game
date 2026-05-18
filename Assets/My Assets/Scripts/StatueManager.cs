using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatueManager : MonoBehaviour
{
    public static StatueManager Instance;

    public GameObject statueLightsParent;

    private readonly List<LightUpTorch> torches = new();

    private bool activated = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        statueLightsParent.SetActive(false);
    }

    void Update()
    {
        if (activated) return;

        if (torches.Count == 0) return;

        if (torches.All(t => t != null && t.IsLit))
        {
            ActivateStatue();
        }
    }

    public void Register(LightUpTorch torch)
    {
        if (!torches.Contains(torch))
        {
            torches.Add(torch);
        }
    }

    private void ActivateStatue()
    {
        activated = true;

        statueLightsParent.SetActive(true);

        FindFirstObjectByType<StatueInteract>().PlaySound();
    }
}