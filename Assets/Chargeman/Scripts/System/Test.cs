using UnityEngine;

public class Test : MonoBehaviour
{
    Rigidbody2D rb;
    private void OnHit(float knockbackRate, Vector2 knockbackDir, int effectId, float friction) // ³Ë¹éÀÌ¶û 
    {
        if ( knockbackRate > 0 )
        {
            rb.AddForce( knockbackDir * knockbackRate, ForceMode2D.Impulse);
            rb.linearDamping = friction;
        }
    }
}
