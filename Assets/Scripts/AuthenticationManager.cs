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
    public TMP_Text loginErrorText;

    [Header("Sign Up UI")]
    public TMP_InputField signUpUsernameInput;
    public TMP_InputField signUpEmailInput;
    public TMP_InputField signUpPasswordInput;
    public TMP_Text signUpErrorText;

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
        ClearErrors();
    }

    public void ShowSignUpPanel()
    {
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);
        ClearErrors();
    }

    void ClearErrors()
    {
        if (loginErrorText) loginErrorText.text = "";
        if (signUpErrorText) signUpErrorText.text = "";
    }

    // =======================
    // LOGIN
    // =======================

    public async void Login()
    {
        ClearErrors();

        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            loginErrorText.text = "Please fill in all fields.";
            return;
        }

        try
        {
            var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            FirebaseUser user = result.User;

            loginPanel.SetActive(false);
        }
        catch (FirebaseException e)
        {
            loginErrorText.text = GetFirebaseErrorMessage(e);
        }
    }

    // =======================
    // SIGN UP
    // =======================

    public async void SignUp()
    {
        ClearErrors();

        string username = signUpUsernameInput.text;
        string email = signUpEmailInput.text;
        string password = signUpPasswordInput.text;

        if (string.IsNullOrEmpty(username) ||
            string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password))
        {
            signUpErrorText.text = "Please fill in all fields.";
            return;
        }

        try
        {
            var result = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseUser user = result.User;

            UserProfile profile = new UserProfile
            {
                DisplayName = username
            };

            await user.UpdateUserProfileAsync(profile);

            signUpPanel.SetActive(false);
        }
        catch (FirebaseException e)
        {
            signUpErrorText.text = GetFirebaseErrorMessage(e);
        }
    }

    // =======================
    // ERROR TRANSLATION
    // =======================

    string GetFirebaseErrorMessage(FirebaseException e)
    {
        AuthError errorCode = (AuthError)e.ErrorCode;

        switch (errorCode)
        {
            case AuthError.InvalidEmail:
                return "Invalid email address.";
            case AuthError.WrongPassword:
                return "Incorrect password.";
            case AuthError.UserNotFound:
                return "Account does not exist.";
            case AuthError.EmailAlreadyInUse:
                return "Email is already registered.";
            case AuthError.WeakPassword:
                return "Password is too weak (min 6 characters).";
            case AuthError.MissingEmail:
                return "Email is required.";
            default:
                return "Authentication failed. Please try again.";
        }
    }
}
