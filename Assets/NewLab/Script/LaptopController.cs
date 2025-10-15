// File: LaptopController.cs

using UnityEngine;
using UnityEngine.SceneManagement;

public class LaptopController : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(4);

    }
}