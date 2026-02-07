using UnityEngine;

public class PotatoActionState : MonoBehaviour
{
    public bool scrubbed;
    public bool cutDone;
    public bool oiled;
    public bool salted;

    public void MarkCutDone() => cutDone = true;
    public void MarkOiled() => oiled = true;
    public void MarkSalted() => salted = true;
}
