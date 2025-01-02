using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UserInput : MonoBehaviour, Input.UserInput.ISessionActions
{
    Input.UserInput userInput;
    SlideHolder[] slideHolders;
    FloatingDialog confirmationDialog;
    [SerializeField] AnalyticsCollector analyticsCollector;
    [SerializeField] float threshold = 0.5f;

    void Awake()
    {
        SceneManager.sceneLoaded += SetActionMap;
    }

    void Start()
    {
        slideHolders = FindObjectsOfType<SlideHolder>();
    }
	
    void SetActionMap(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lobby") return;

        userInput = new Input.UserInput();
        userInput.Session.SetCallbacks(this);
        userInput.Session.Enable();

        Canvas[] canvases = Resources.FindObjectsOfTypeAll<Canvas>();
        confirmationDialog = canvases.FirstOrDefault(canvas => canvas.gameObject.name == "Confirmation Dialog")?.GetComponent<FloatingDialog>();
    }

    public void OnChangeSlide(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 joystickValue = context.ReadValue<Vector2>();

            if (joystickValue.x >= threshold)
            {
                foreach (var slideHolder in slideHolders)
                {
                    slideHolder.NextSlide();
                }
            }

            if (joystickValue.x <= -threshold)
            {
                foreach (var slideHolder in slideHolders)
                {
                    slideHolder.PrevSlide();
                }
            }
        }

    }

    public void OnCallMenu(InputAction.CallbackContext context)
    {
        confirmationDialog.Show();
    }

    public void GoToLobby()
    {
        if (analyticsCollector != null)
        {
            if (PlayerPrefs.GetInt("STT") == 1)
            {
                analyticsCollector.SaveAnalytics();
                Debug.Log("All good");
            }
        }
        SceneManager.LoadScene("Lobby");
    }

    public void ExitDialog()
    {
        confirmationDialog.Hide();
    }
	
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= SetActionMap;
        userInput?.Session.Disable();
    }
}