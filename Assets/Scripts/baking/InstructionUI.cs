using TMPro;
using UnityEngine;

public class InstructionUI : MonoBehaviour
{
    [Header("References")]
    public CookingStepManager manager;
    public TMP_Text instructionText;

    private void Awake()
    {
        if (instructionText == null)
            instructionText = GetComponent<TMP_Text>();

        if (manager == null)
            manager = FindObjectOfType<CookingStepManager>();

        if (instructionText == null)
            Debug.LogError("[InstructionUI] No TMP_Text found. Attach this script to a TMP Text object.");

        if (manager == null)
            Debug.LogError("[InstructionUI] No CookingStepManager found. Put it on GameManager.");
    }

    private void OnEnable()
    {
        if (manager == null || instructionText == null) return;

        manager.OnStepChanged.AddListener(UpdateInstruction);
        UpdateInstruction(manager.currentStep);
    }

    private void OnDisable()
    {
        if (manager == null) return;
        manager.OnStepChanged.RemoveListener(UpdateInstruction);
    }

    private void UpdateInstruction(CookingStepManager.Step step)
    {
        instructionText.text = GetInstruction(step);
    }

    private string GetInstruction(CookingStepManager.Step step)
    {
        switch (step)
        {
            case CookingStepManager.Step.PreheatOven200:
                return "Step 1: Preheat the oven to 200°C.\nPress the PREHEAT button.";

            case CookingStepManager.Step.ScrubSkinNoPeel:
                return "Step 2: Scrub the potato clean (do not peel).\nHold it in the sink scrub zone.";

            case CookingStepManager.Step.Slice5mmDontCutThrough:
                return "Step 3: Slice into ~5mm slices.\nDo NOT cut all the way through.";

            case CookingStepManager.Step.PlaceInBakingPan:
                return "Step 4: Place the potato on the tray/pan.\nSnap it into the PanSocket.";

            case CookingStepManager.Step.BrushWithOilOrButter:
                return "Step 5: Brush oil/butter evenly over the potato.";

            case CookingStepManager.Step.SprinkleSalt:
                return "Step 6: Sprinkle salt evenly over the potato.";

            case CookingStepManager.Step.Bake40Min:
                if (manager != null && manager.bakingInProgress)
                    return "Step 7: Baking...\nPlease wait until it finishes.";

                return "Step 7: Put the tray into the oven.\nSnap into OvenSocket, then press BAKE.";

            case CookingStepManager.Step.ServeOnPlate:
                return "Step 8: Take it out and serve on the plate.\nPlace it on PlateSocket to finish the game.";

            case CookingStepManager.Step.Done:
                return "✅ Dish completed! Game finished.";

            default:
                return "";
        }
    }

    public void OnQuizCompleted()
    {
        if (instructionText != null)
            instructionText.text = "Quiz completed! Continue to the cooking task.";
    }
}
