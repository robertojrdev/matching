using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private Text errorLog;

    private void OnEnable()
    {
        submitButton.onClick.AddListener(Submit);
    }
    private void OnDisable()
    {
        submitButton.onClick.RemoveListener(Submit);
    }

    private void Submit()
    {
        if (string.IsNullOrEmpty(nameInputField.text) || string.IsNullOrWhiteSpace(nameInputField.text))
        {
            errorLog.text = "Name cannot be empty";
            return;
        }

        GameManager.StartSession(nameInputField.text);
    }
}