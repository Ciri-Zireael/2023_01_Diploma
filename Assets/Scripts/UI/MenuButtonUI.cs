using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonUI : MonoBehaviour
{
    [SerializeField] private string newSimulation = "Test scene";
    public void newSimulationButton()
    {
        SceneManager.LoadScene(newSimulation);
    }
}
