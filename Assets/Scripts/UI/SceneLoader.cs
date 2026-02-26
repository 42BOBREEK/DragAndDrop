using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneToGoName;

    public void GoToScene()
    {
        SceneManager.LoadScene(_sceneToGoName);
    }
}
