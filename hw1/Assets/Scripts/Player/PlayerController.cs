using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerConfig config;
    [SerializeField] private Transform groundCheck;

    public int HP => hp;
    public int MaxHP => config != null ? config.maxHealth : 0;

    private int hp;
    private bool isDead = false;
    private int jumpsUsed = 0;
    public System.Action<int, int> OnHealthChanged; // current, max

    private Rigidbody rb;
    private int currentLaneIndex = 1; // 0..2 (по умолчанию середина)

    private float inputCooldown = 0.15f;
    private float nextInputTime = 0f;

    private void Awake()
    {
        hp = config.maxHealth;
        OnHealthChanged?.Invoke(hp, config.maxHealth);
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

private void Update()
{
    if (config == null) return;

    var k = Keyboard.current;
    if (k == null) return;

    if (k.aKey.wasPressedThisFrame || k.leftArrowKey.wasPressedThisFrame)
        TryMoveLane(-1);

    if (k.dKey.wasPressedThisFrame || k.rightArrowKey.wasPressedThisFrame)
        TryMoveLane(+1);

    if (k.spaceKey.wasPressedThisFrame)
        TryJump();
}

private void TryJump()
{
    if (!CanJump()) return;

    jumpsUsed++;

    Vector3 v = rb.linearVelocity;
    v.y = 0f;
    rb.linearVelocity = v;

    rb.AddForce(Vector3.up * config.jumpImpulse, ForceMode.Impulse);
}

private bool CanJump()
{
    if (IsGrounded())
    {
        jumpsUsed = 0;
        return true;
    }

    return jumpsUsed < config.maxJumps;
}

private bool IsGrounded()
{
    if (groundCheck == null) return false;

    return Physics.CheckSphere(
        groundCheck.position,
        config.groundCheckRadius,
        config.groundMask
    );
}

public void TakeDamage(int damage)
{
    if (isDead) return;

    hp -= damage;
    OnHealthChanged?.Invoke(hp, config.maxHealth);
    Debug.Log("HP: " + hp);

    if (hp <= 0)
    {
        hp = 0;
        Die();
    }
}

private void Die()
{
    isDead = true;
    Debug.Log("Player died");
    GameManager.I?.StopAndRestart(0.5f);
}

   private void FixedUpdate()
    {
    if (config == null) return;

    float mult = GameManager.I != null ? GameManager.I.SpeedMultiplier : 1f;

    // 1️⃣ Движение вперёд через velocity
    Vector3 v = rb.linearVelocity;
    v.z = config.forwardSpeed * mult;
    rb.linearVelocity = v;

    // 2️⃣ Переключение линии через MovePosition
    float targetX = config.lanesX[Mathf.Clamp(currentLaneIndex, 0, config.lanesX.Length - 1)];
    float newX = Mathf.MoveTowards(rb.position.x, targetX, config.laneChangeSpeed * Time.fixedDeltaTime);

    Vector3 nextPos = rb.position;
    nextPos.x = newX;

    rb.MovePosition(nextPos);
    }

    private void TryMoveLane(int dir)
    {
        int newIndex = currentLaneIndex + dir;
        if (newIndex < 0 || newIndex >= config.lanesX.Length) return;
        currentLaneIndex = newIndex;
    }
    public void Heal(int amount)
{
    if (isDead) return;
    amount = Mathf.Max(0, amount);
    hp = Mathf.Min(hp + amount, config.maxHealth);
    OnHealthChanged?.Invoke(hp, config.maxHealth);
}
}