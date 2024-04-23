using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour, Input.UserInput.ISessionActions
{
    Input.UserInput userInput;
    SlideHolder[] slideHolders;

    void Awake()
    {
        userInput = new Input.UserInput();
        userInput.Session.SetCallbacks(this);
        userInput.Session.Enable();
    }

    void Start()
    {
        slideHolders = FindObjectsOfType<SlideHolder>();
    }

    public void OnNextSlide(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            foreach (var slideHolder in slideHolders)
            {
                slideHolder.NextSlide();
            }
        }

    }

    public void OnPrevSlide(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            foreach (var slideHolder in slideHolders)
            {
                slideHolder.PrevSlide();
            }
        }
    }

    public void OnEndSession(InputAction.CallbackContext context)
    {
        // TODO: Proper end session
        throw new NotImplementedException();
    }
}
