using TMPro;
using UnityEngine;

public class InstructionUI : MonoBehaviour
{
    [Header("References")]
    public CookingStepManager manager;
    public TMP_Text instructionText;
    public AuthManager authManager;
    public QuizController quizController;

    private enum InstructionState
    {
        WaitForLogin,
        GoToQuizRoom,
        CookingSteps,
        Done
    }

    private InstructionState currentState = InstructionState.WaitForLogin;

    private void Awake()
    {


        // Error logging for missing components
        if (instructionText == null)
            Debug.LogError("[InstructionUI] No TMP_Text found.");
        if (manager == null)
            Debug.LogError("[InstructionUI] No CookingStepManager found.");
        if (authManager == null)
            Debug.LogError("[InstructionUI] No AuthManager found.");
        if (quizController == null)
            Debug.LogError("[InstructionUI] No QuizController found.");
    }

    private void OnEnable()
    {
        if (manager != null) manager.OnStepChanged.AddListener(UpdateInstruction);

        if (quizController != null && quizController.quizPanel != null)
            quizController.quizPanel.SetActive(false);

        UpdateInstructionText();
    }

    private void OnDisable()
    {
        if (manager != null)
            manager.OnStepChanged.RemoveListener(UpdateInstruction);
    }

    private void UpdateInstruction(CookingStepManager.Step step)
    {
        if (currentState == InstructionState.CookingSteps && instructionText != null)
            instructionText.text = GetInstruction(step);
    }

    private void UpdateInstructionText()
    {
        if (instructionText == null) return;

        switch (currentState)
        {
            case InstructionState.WaitForLogin:
                instructionText.text = "Please login or sign up to start.";
                break;

            case InstructionState.GoToQuizRoom:
                instructionText.text = "Walk into the room on your right to take the quiz.";
                break;

            case InstructionState.CookingSteps:
                if (manager != null)
                    instructionText.text = GetInstruction(manager.currentStep);
                break;

            case InstructionState.Done:
                instructionText.text = "All tasks completed!";
                break;
        }
    }

    // Call from AuthManager after logging in successfully
    public void OnLoginSuccess()
    {
        currentState = InstructionState.GoToQuizRoom;
        UpdateInstructionText();
    }

    // Call when player enters the quiz room
    public void OnEnterQuizRoom()
    {
        if (quizController != null && quizController.quizPanel != null)
            quizController.quizPanel.SetActive(true);
    }

    // Call from QuizController after done with quiz
    public void OnQuizCompleted()
    {
        currentState = InstructionState.CookingSteps;
        UpdateInstructionText();
    }

    private string GetInstruction(CookingStepManager.Step step)
    {
        switch (step)
        {
            case CookingStepManager.Step.PreheatOven200:
                return "Step 2: Preheat the oven to 200°C.\nPress the PREHEAT button.";

            case CookingStepManager.Step.ScrubSkinNoPeel:
                return "Step 3: Scrub the potato clean (do not peel).\nHold it in the sink scrub zone.";

            case CookingStepManager.Step.Slice5mmDontCutThrough:
                return "Step 4: Slice into ~5mm slices.\nDo NOT cut all the way through.";

            case CookingStepManager.Step.PlaceInBakingPan:
                return "Step 5: Place the potato on the tray/pan.\nSnap it into the PanSocket.";

            case CookingStepManager.Step.BrushWithOilOrButter:
                return "Step 6: Brush oil/butter evenly over the potato.";

            case CookingStepManager.Step.SprinkleSalt:
                return "Step 7: Sprinkle salt evenly over the potato.";

            case CookingStepManager.Step.Bake40Min:
                if (manager != null && manager.bakingInProgress)
                    return "Step 8: Baking...\nPlease wait until it finishes.";
                return "Step 8: Put the tray into the oven.\nSnap into OvenSocket, then press BAKE.";

            case CookingStepManager.Step.ServeOnPlate:
                return "Step 9: Take it out and serve on the plate.\nPlace it on PlateSocket to finish the game.";

            case CookingStepManager.Step.Done:
                return "Dish completed! Game finished.";

            default:
                return "";
        }
    }
}
