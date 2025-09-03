using UnityEngine;

public interface IBoss
{
    public int Hp { get; set; }
    public int MaxHp { get; }

    public void TakeDamage(int damage);
    public bool isTesting { get; }
}
