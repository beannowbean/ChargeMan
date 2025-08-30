using UnityEngine;

public interface IBoss
{
    public int Hp { get; set; }
    public float MoveSpeed { get; set; }

    public void TakeDamage(int damage);
}
