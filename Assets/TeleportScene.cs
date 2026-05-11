using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportScene : MonoBehaviour
{

    public SceneAsset SceneAsset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public async Task LoadNextScene()
    {
        await SceneManager.LoadSceneAsync(SceneAsset.name);
    }

    public async void OnDestroy()
    {
       await LoadNextScene();
    }
}
