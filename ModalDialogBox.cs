using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ModalDialogBox : MonoBehaviour
{
    public delegate bool DisablePredicate();

    public Button buttonTemplate;
    
    private static ModalDialogBox instance;
    
    private Text messageText;
    private GridLayoutGroup buttonPanel;
    private List<Button> buttons = new List<Button>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        messageText = GetComponentInChildren<Text>();
        buttonPanel = GetComponentInChildren<GridLayoutGroup>();
    }

    private void Start()
    {
        ResetDialogBox();
    }

    private static void ResetDialogBox()
    {
        foreach(Button button in instance.buttons)
        {
            Destroy(button.gameObject);
        }

        instance.buttons.Clear();

        instance.gameObject.SetActive(false);

        GameController.Instance.TogglePause(false);
    }

    public static void SetDialogMessage(string message)
    {
        instance.messageText.text = message;
    }

    public static void AddButton(string text, UnityAction action, bool closeDialogBoxOnClick = true, DisablePredicate disablePredicate = null)
    {
        Button buttonInstance = Instantiate(instance.buttonTemplate, instance.buttonPanel.transform);

        if (action != null)
        {
            buttonInstance.onClick.AddListener(action);
        }

        if (closeDialogBoxOnClick)
        {
            buttonInstance.onClick.AddListener(ResetDialogBox);
        }

        buttonInstance.interactable = disablePredicate == null || !disablePredicate();

        buttonInstance.GetComponentInChildren<Text>().text = text;

        instance.buttons.Add(buttonInstance);
    }

    public static void ShowDialogBox()
    {
        GameController.Instance.TogglePause(true);

        instance.gameObject.SetActive(true);
    }

    public static void HideDialogBox()
    {
        instance.gameObject.SetActive(false);
    }
}