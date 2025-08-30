using UnityEngine;

public class HitHandler : MonoBehaviour // 매개변수 일일이 저장하기 귀찮아서 테스트!!
{
    Rigidbody2D rb;
    public virtual void OnHit() // 그냥 피격 효과만 전달하고 싶을때
    {
        // 근데 이거 virtual로 만드는 의미가있음????? 난일단잘모르겠는데 혹시모르지
    }
    public virtual void OnHit(int effectId) // 적중 시 특정 효과(버프)를 발동시키고 싶을 때
    {

    }

    public virtual void OnHit(float knockbackRate, Vector2 knockbackDir, float friction) // 물리적 효과 발동시키고 싶을 떄
    {
        if (knockbackRate > 0)
        {
            rb.AddForce(knockbackDir * knockbackRate, ForceMode2D.Impulse);
            rb.linearDamping = friction;
        }
    }

    public virtual void OnHit(float knockbackRate, Vector2 knockbackDir, int effectId, float friction) // 넉백이랑 
    {
        if ( knockbackRate > 0 )
        {
            rb.AddForce( knockbackDir * knockbackRate, ForceMode2D.Impulse);
            rb.linearDamping = friction;
        }
    }
}
