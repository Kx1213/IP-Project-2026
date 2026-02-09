using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;

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

    [Header("Door Lock")]
    public DoorLockController door;

    private FirebaseAuth auth;
    private DatabaseReference db;

    async void Start()
    {
        await InitializeFirebase();
        ShowLoginPanel();

        // Lock door on start
        if (door)
            door.LockDoor();
    }

    async Task InitializeFirebase()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            db = FirebaseDatabase.DefaultInstance.RootReference;
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
            await auth.SignInWithEmailAndPasswordAsync(email, password);

            loginPanel.SetActive(false);

            // Unlock door after successful login
            if (door)
                door.UnlockDoor();
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
            // Create Auth account
            var result = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseUser user = result.User;

            // Set display name
            UserProfile profile = new UserProfile
            {
                DisplayName = username
            };
            await user.UpdateUserProfileAsync(profile);

            // Save to Realtime Database
            string uid = user.UserId;

            Dictionary<string, object> userData = new Dictionary<string, object>
            {
                { "username", username },
                { "email", email },
                { "points", 0 }
            };

            await db.Child("users").Child(uid).UpdateChildrenAsync(userData);

            signUpPanel.SetActive(false);

            // Unlock door after successful signup
            if (door)
                door.UnlockDoor();
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
