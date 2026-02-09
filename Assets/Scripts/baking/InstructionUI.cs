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
    }

    private void OnEnable()
    {
        if (manager != null)
        {
            manager.OnStepChanged.AddListener(UpdateInstruction);
            UpdateInstruction(manager.currentStep); // show first step immediately
        }
    }

    private void OnDisable()
    {
        if (manager != null)
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
                return "Preheat the oven to 200°C";

            case CookingStepManager.Step.ScrubSkinNoPeel:
                return "Scrub the potato skin clean. Do not peel.";

            case CookingStepManager.Step.Slice5mmDontCutThrough:
                return "Slice the potato into 5mm slices.\nDo not cut all the way through.";

            case CookingStepManager.Step.PlaceInBakingPan:
                return "Place the sliced potato into the baking pan.";

            case CookingStepManager.Step.BrushWithOilOrButter:
                return "Brush the potato evenly with oil or butter.";

            case CookingStepManager.Step.SprinkleSalt:
                return "Sprinkle salt evenly over the potato.";

            case CookingStepManager.Step.Bake40Min:
                return "Place the pan into the oven and press Bake.";

            case CookingStepManager.Step.ServeOnPlate:
                return "Serve the baked potato on a plate.";

            case CookingStepManager.Step.GarnishParsley:
                return "Garnish with parsley.";

            case CookingStepManager.Step.Done:
                return "Dish complete! Well done 👏";

            default:
                return "";
        }
    }
}
