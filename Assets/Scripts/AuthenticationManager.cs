using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System.Threading.Tasks;

public class AuthenticationManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject signUpPanel;

    [Header("Login UI")]
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;

    [Header("Sign Up UI")]
    public TMP_InputField signUpUsernameInput;
    public TMP_InputField signUpEmailInput;
    public TMP_InputField signUpPasswordInput;

    private FirebaseAuth auth;

    async void Start()
    {
        await InitializeFirebase();
        ShowLoginPanel();
    }

    async Task InitializeFirebase()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            Debug.Log("Firebase Auth initialized");
        }
        else
        {
            Debug.LogError("Firebase dependency error: " + dependencyStatus);
        }
    }

    // =======================
    // PANEL SWITCHING
    // =======================

    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }

    public void ShowSignUpPanel()
    {
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }

    // =======================
    // LOGIN
    // =======================

    public void Login()
    {
        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Login fields empty");
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Login failed: " + task.Exception);
                    return;
                }

                FirebaseUser user = task.Result.User;
                Debug.Log("Logged in as: " + user.DisplayName);

                // TODO: Load VR scene here
            });
    }

    // =======================
    // SIGN UP (WITH USERNAME)
    // =======================

    public void SignUp()
    {
        string username = signUpUsernameInput.text;
        string email = signUpEmailInput.text;
        string password = signUpPasswordInput.text;

        if (string.IsNullOrEmpty(username) ||
            string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Sign up fields empty");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWith(async task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Sign up failed: " + task.Exception);
                    return;
                }

                FirebaseUser user = task.Result.User;

                // Set username as DisplayName
                UserProfile profile = new UserProfile
                {
                    DisplayName = username
                };

                await user.UpdateUserProfileAsync(profile);

                Debug.Log("Sign up successful: " + username);

                ShowLoginPanel();
            });
    }
}
