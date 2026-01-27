using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class LogInButton : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField usernameInputField;
    [SerializeField]
    private TMP_InputField passwordInputField;

    [SerializeField]
    private string username = "player123";
    [SerializeField]
    private string password = "password123";

    [SerializeField]
    private TMP_Text ErrorUsername;
    [SerializeField]
    private TMP_Text ErrorPassword;

    public void OnLogInButtonClicked()
    {
        if (usernameInputField.text != username || usernameInputField.text == "")
        {
            ErrorUsername.text = "Incorrect Username";
        }
        
        if (passwordInputField.text != password || passwordInputField.text == "") 
        {
            ErrorPassword.text = "Incorrect Password";
        }
        
        if (passwordInputField.text == password && usernameInputField.text == username)
        {
            SceneManager.LoadScene("MyScene");
        }

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
