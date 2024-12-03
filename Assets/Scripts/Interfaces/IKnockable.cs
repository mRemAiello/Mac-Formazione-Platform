using UnityEngine;

public interface IKnockable
{
    public bool IsKnockable { get; }
    public float KnockBackForce { get; }
    public float KnockBackTime { get; }
    public float StunTime { get; }

    //
    public void KnockBack(Transform enemyTransform, float knockBackForce, float knockBackTime, float stunForce);
}