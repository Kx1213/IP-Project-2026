using System.Collections;
using UnityEngine;


public class OvenController : MonoBehaviour
{
    public RecipeManager manager;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor ovenSocket;

    public float bakeSeconds = 10f;

    public GameObject potatoRaw;
    public GameObject potatoCooked;

    private bool baking;

    private void Awake()
    {
        if (manager == null)
            manager = FindFirstObjectByType<RecipeManager>();
    }

    // ✅ This WILL now appear in Unity Events
    public void StartBake()
    {
        Debug.Log("StartBake called");

        if (!manager.IsStep(5))
        {
            Debug.Log("❌ Not baking step yet");
            return;
        }

        // ✅ Correct way (compatible with your XR Toolkit)
        if (!ovenSocket.hasSelection)
        {
            Debug.Log("❌ No tray in oven");
            return;
        }

        if (!baking)
            StartCoroutine(BakeRoutine());
    }

    private IEnumerator BakeRoutine()
    {
        baking = true;
        Debug.Log("🔥 Baking...");

        yield return new WaitForSeconds(bakeSeconds);

        if (potatoRaw != null)
            potatoRaw.SetActive(false);

        if (potatoCooked != null)
            potatoCooked.SetActive(true);

        manager.CompleteStep(5);

        baking = false;
        Debug.Log("✅ Baking complete");
    }
}
