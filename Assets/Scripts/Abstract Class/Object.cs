using UnityEngine;

public enum ObjectType
{
    consumable,
    obstacle
}

public abstract class Object : MonoBehaviour
{
    protected ObjectType m_objectType;
    protected float m_cooldownInterval;
    protected float m_cooldownTimer;
    protected float m_detectionTimer;
    protected float m_detectionInterval;
    protected Transform m_transform;
    protected BoxCollider2D m_boxCollider;

    private void OnValidate()
    {
        InitializeComponents();
        
    }
    protected virtual void Awake() 
    {
        InitializeParameters();
    }
    protected virtual void InitializeComponents()
    {
        m_transform = GetComponent<Transform>();
        m_boxCollider = GetComponent<BoxCollider2D>();
    }
    protected virtual void InitializeParameters() 
    {
        m_detectionTimer = 0;
        m_cooldownTimer = m_cooldownInterval;
    }
    protected virtual void OnTriggerEffect() { }
    protected virtual void DetectCollision() { }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player") OnTriggerEffect();
    }
}
