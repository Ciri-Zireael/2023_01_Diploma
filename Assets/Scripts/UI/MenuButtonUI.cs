using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonUI : MonoBehaviour
{
    [SerializeField] private string firstSimulation = "Classroom";
    [SerializeField] private string secondSimulation = "Lecture room";

    public void LoadFirstSimulation()
    {
        SceneManager.LoadScene(firstSimulation);
    }

    public void LoadSecondSimulation()
    {
        SceneManager.LoadScene(secondSimulation);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
