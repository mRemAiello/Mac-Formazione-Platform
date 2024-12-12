public interface IDamageable
{
    public bool IsDead { get; }
    public bool IsAlive { get; }
    public void TakeDamage(float damage);
}