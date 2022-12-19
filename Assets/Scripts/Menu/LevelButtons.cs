using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtons : MonoBehaviour
{
    private const int MAIN_MENU_BUILD_INDEX = 0;

    public void OnBackClick()
    {
        SceneManager.LoadScene(MAIN_MENU_BUILD_INDEX);
    }
}
