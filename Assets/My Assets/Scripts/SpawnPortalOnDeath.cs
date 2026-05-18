using UnityEngine;

public class SpawnPortalOnDeath : MonoBehaviour
{

    public GameObject portal;

    private void OnDestroy()
    {
        portal.SetActive(true);
    }
}
