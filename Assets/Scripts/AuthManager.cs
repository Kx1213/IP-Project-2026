using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class AuthManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject loginPanel;
    public GameObject signUpPanel;

    [Header("Login Fields")]
    public TMP_InputField loginEmailField;
    public TMP_InputField loginPasswordField;
    public TMP_Text loginErrorText;

    [Header("SignUp Fields")]
    public TMP_InputField signUpUsernameField;
    public TMP_InputField signUpEmailField;
    public TMP_InputField signUpPasswordField;
    public TMP_Text signUpErrorText;

    [Header("References")]
    public DoorLockController doorController;
    public InstructionUI instructionUI;

    private FirebaseAuth auth;
    private DatabaseReference dbRef;

    private async void Start()
    {
        await InitializeFirebase();

        if (auth != null && auth.CurrentUser != null)
        {
            Debug.Log("[AuthManager] User already logged in: " + auth.CurrentUser.Email);
            loginPanel.SetActive(false);
            signUpPanel.SetActive(false);
            doorController.UnlockDoor();
            instructionUI?.OnLoginSuccess();
        }
        else
        {
            ShowLoginPanel();
            doorController.LockDoor();
        }
    }

    private async Task InitializeFirebase()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            dbRef = FirebaseDatabase.DefaultInstance.RootReference;
            Debug.Log("[AuthManager] Firebase initialized successfully.");
        }
        else
        {
            Debug.LogError("[AuthManager] Could not resolve Firebase dependencies: " + dependencyStatus);
        }
    }

    // Panel switching
    public void ShowLoginPanel()
    {
        ClearErrors();
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }

    public void ShowSignUpPanel()
    {
        ClearErrors();
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }

    private void ClearErrors()
    {
        if (loginErrorText) loginErrorText.text = "";
        if (signUpErrorText) signUpErrorText.text = "";
    }
    public void OnLoginButtonPressed()
    {
        _ = Login();
    }

    public void OnSignUpButtonPressed()
    {
        _ = SignUp();
    }

    // Login
    private async Task Login()
    {
        Debug.Log("[AuthManager] Login button pressed.");
        ClearErrors();

        string email = loginEmailField.text.Trim();
        string password = loginPasswordField.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            loginErrorText.text = "Please enter both email and password.";
            return;
        }

        try
        {
            var authResult = await auth.SignInWithEmailAndPasswordAsync(email, password);
            FirebaseUser user = authResult.User;

            Debug.Log("[AuthManager] Login success: " + user.Email);

            loginPanel.SetActive(false);
            signUpPanel.SetActive(false);
            doorController.UnlockDoor();
            instructionUI?.OnLoginSuccess();
        }
        catch (FirebaseException e)
        {
            Debug.LogError("[AuthManager] Login failed: " + e.Message);
            loginErrorText.text = GetFirebaseErrorMessage(e);
        }
    }

    // SignUp
    private async Task SignUp()
    {
        Debug.Log("[AuthManager] SignUp button pressed.");
        ClearErrors();

        string username = signUpUsernameField.text.Trim();
        string email = signUpEmailField.text.Trim();
        string password = signUpPasswordField.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            signUpErrorText.text = "Please fill in all fields.";
            return;
        }

        try
        {
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseUser newUser = authResult.User;

            Debug.Log("[AuthManager] SignUp success: " + newUser.Email);

            UserProfile profile = new UserProfile { DisplayName = username };
            await newUser.UpdateUserProfileAsync(profile);

            string userId = newUser.UserId;
            await dbRef.Child("users").Child(userId).SetRawJsonValueAsync(
                JsonUtility.ToJson(new UserData(username, email, 0))
            );

            loginPanel.SetActive(false);
            signUpPanel.SetActive(false);
            doorController.UnlockDoor();
            instructionUI?.OnLoginSuccess();
        }
        catch (FirebaseException e)
        {
            Debug.LogError("[AuthManager] SignUp failed: " + e.Message);
            signUpErrorText.text = GetFirebaseErrorMessage(e);
        }
    }

    //Proper user error messages
    private string GetFirebaseErrorMessage(FirebaseException e)
    {
        AuthError errorCode = (AuthError)e.ErrorCode;
        return errorCode switch
        {
            AuthError.InvalidEmail => "Invalid email address.",
            AuthError.WrongPassword => "Incorrect password.",
            AuthError.UserNotFound => "No account found with this email.",
            AuthError.EmailAlreadyInUse => "Email is already registered.",
            AuthError.WeakPassword => "Password is too weak.",
            _ => "Authentication failed. Please try again."
        };
    }

    //Was trying to fix some bugs here
    [System.Serializable]
    public class UserData
    {
        public string username;
        public string email;
        public int points;

        public UserData(string username, string email, int points)
        {
            this.username = username;
            this.email = email;
            this.points = points;
        }
    }
}
