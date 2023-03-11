using Unity.Netcode.Components;

public class MultiFrogNetworkAnimator : NetworkAnimator
{
    // Override the OnIsServerAuthoritative method
    protected override bool OnIsServerAuthoritative()
    {
        // Return false to make the client authoritative
        return false;
    }
}