using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class AuthManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject signupPanel;

    [Header("Login UI")]
    public InputField loginEmailInput;
    public InputField loginPasswordInput;

    [Header("Signup UI")]
    public InputField signupEmailInput;
    public InputField signupPasswordInput;
    public InputField signupUsernameInput;

    FirebaseAuth auth;
    FirebaseUser user;
    DatabaseReference dbRef;

    void Start()
    {
        // Initialize Firebase
        auth = FirebaseAuth.DefaultInstance;
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        // Show login first
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
    }

    // ================= LOGIN =================
    public void LoginButton()
    {
        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;

        StartCoroutine(Login(email, password));
    }

    IEnumerator Login(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogWarning($"Login failed: {loginTask.Exception}");
        }
        else
        {
            user = loginTask.Result.User;
            Debug.Log("Login successful: " + user.Email);
            StartGame();
        }
    }

    // ================= SIGN UP =================
    public void SignupButton()
    {
        string email = signupEmailInput.text;
        string password = signupPasswordInput.text;
        string username = signupUsernameInput.text;

        StartCoroutine(SignUp(email, password, username));
    }

    IEnumerator SignUp(string email, string password, string username)
    {
        var signUpTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => signUpTask.IsCompleted);

        if (signUpTask.Exception != null)
        {
            Debug.LogWarning($"Signup failed: {signUpTask.Exception}");
        }
        else
        {
            user = signUpTask.Result.User;

            // Save username to database
            dbRef.Child("users").Child(user.UserId).Child("username").SetValueAsync(username);

            Debug.Log("Signup successful. User created: " + user.Email);

            // After sign up → go back to login
            signupPanel.SetActive(false);
            loginPanel.SetActive(true);
        }
    }

    // ================= PANEL SWITCH =================
    public void GoToSignupPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
    }

    public void GoToLoginPanel()
    {
        signupPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    // ================= START GAME =================
    void StartGame()
    {
        Debug.Log("Game Start!");
        // Example:
        // SceneManager.LoadScene("GameScene");
        // or enable your game UI etc.
    }
}
