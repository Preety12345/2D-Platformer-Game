using UnityEngine;

public class Spike : Object
{
    private const string k_moduleName1 = nameof(Spike);
    [Header(k_moduleName1 + "    :     Assignables"), SerializeField, Range(0, 0)]
    private byte m_header1;
    [SerializeField] private int m_damage;
    [SerializeField] private Vector2 m_detectionRadius;

    protected override void InitializeParameters()
    {
        base.InitializeParameters();
        m_cooldownInterval = 1f;
    }

    private void Update()
    {
        m_detectionTimer += Time.deltaTime;
        if(m_detectionTimer > m_detectionInterval)
        {
            DetectCollision();
            m_detectionTimer = 0f;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(m_boxCollider.bounds.center, m_detectionRadius);
    }
    protected override void DetectCollision()
    {
        Collider2D collider = Physics2D.OverlapBox(m_transform.position, m_detectionRadius, 90f, LayerMask.GetMask("Player"));
        if (collider != null && collider.TryGetComponent(out PlayerController player))
        {
            m_cooldownTimer += Time.deltaTime;
            if (m_cooldownTimer >= m_cooldownInterval)
            {
                player.ReceiveDamage(m_damage);
                m_cooldownTimer = 0;
            }
        }
        else m_cooldownTimer = m_cooldownInterval;
    }
}
