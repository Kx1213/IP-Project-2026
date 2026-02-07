using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    // 0-based steps
    // 0 Preheat, 1 Scrub, 2 Cut, 3 Put on tray, 4 Oil+Salt, 5 Bake, 6 Serve+Parsley
    public int step;

    [TextArea(2,5)]
    public string instruction;

    public void Start()
    {
        SetStep(0);
    }

    public bool IsStep(int s) => step == s;

    public void CompleteStep(int s)
    {
        if (step != s)
        {
            Debug.Log($"❌ Wrong order. Current Step = {step + 1}");
            return;
        }

        SetStep(step + 1);
    }

    void SetStep(int s)
    {
        step = s;
        instruction = s switch
        {
            0 => "Step 1: Press Preheat to 200°C.",
            1 => "Step 2: Scrub the potato at the sink.",
            2 => "Step 3: Cut the potato (do not cut all the way).",
            3 => "Step 4: Place the potato on the baking tray.",
            4 => "Step 5: Brush oil and sprinkle salt.",
            5 => "Step 6: Put tray into oven and press Bake.",
            6 => "Step 7: Serve on plate and add parsley.",
            _ => "✅ Done! Recipe completed."
        };

        Debug.Log(instruction);
    }
}
