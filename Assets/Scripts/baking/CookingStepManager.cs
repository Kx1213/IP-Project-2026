using UnityEngine;
using UnityEngine.Events;
using Firebase.Database;
using Firebase.Auth;

public class CookingStepManager : MonoBehaviour
{
    public enum Step
    {
        PreheatOven200,          // Step 1
        ScrubSkinNoPeel,         // Step 2
        Slice5mmDontCutThrough,  // Step 3
        PlaceInBakingPan,        // Step 4
        BrushWithOilOrButter,    // Step 5a
        SprinkleSalt,            // Step 5b
        Bake40Min,               // Step 6
        ServeOnPlate,            // Step 7a
        GarnishParsley,          // Step 7b
        Done
    }

    [Header("Current Step")]
    public Step currentStep = Step.PreheatOven200;

    [Header("Events")]
    public UnityEvent<Step> OnStepChanged;

    [Header("Points")]
    public int pointsPerStep = 200;

    FirebaseAuth auth;
    DatabaseReference db;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public bool IsStep(Step step)
    {
        return currentStep == step;
    }

    /// <summary>
    /// Call this ONLY when the correct action is completed
    /// </summary>
    public void AdvanceStep(Step expectedCurrent)
    {
        // ❌ Wrong step → no progress, no points
        if (currentStep != expectedCurrent)
            return;

        // ✅ Award points for this step
        AddPoints(pointsPerStep);

        // Move to next step
        currentStep = (Step)((int)currentStep + 1);

        Debug.Log("STEP -> " + currentStep);
        OnStepChanged?.Invoke(currentStep);
    }

    // =======================
    // FIREBASE POINTS
    // =======================

    void AddPoints(int amount)
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogError("User not logged in — cannot award points");
            return;
        }

        string uid = auth.CurrentUser.UserId;
        DatabaseReference pointsRef = db.Child("users").Child(uid).Child("points");

        pointsRef.RunTransaction(mutableData =>
        {
            int currentPoints = 0;

            if (mutableData.Value != null)
                int.TryParse(mutableData.Value.ToString(), out currentPoints);

            mutableData.Value = currentPoints + amount;
            return TransactionResult.Success(mutableData);
        });

        Debug.Log($"+{amount} points awarded for cooking step");
    }
}
