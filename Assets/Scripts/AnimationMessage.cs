using UnityEngine;

public class AnimationMessenger : MonoBehaviour
{
    private Anonimus playerScript;

    void Start()
    {
        playerScript = GetComponentInParent<Anonimus>();
    }

    public void EndAttack()
    {
        if (playerScript != null)
        {
            playerScript.OnAttackEnded();
        }
    }
}
