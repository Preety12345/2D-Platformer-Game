using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private const string k_moduleName1 = nameof(PlayerController);
    [Header(k_moduleName1 + "    :     Assignables"), SerializeField, Range(0, 0)]
    private byte m_header1;
    [SerializeField]private Transform groundCheck;
    [SerializeField] private Image HealthFillImg;
    [SerializeField]private LayerMask groundLayer;

    [SerializeField] private int m_currentHealth;
    private int m_maxHealth;
    private float speed;
    private float jumpSpeed;
    private float direction;
    private float groundCheckRadius;
    private bool isTouchingGround;
    private Rigidbody2D m_rb;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;
    private Vector3 respawnPoint;

    private void Awake()
    {
        InitializeComponents();
        InitializeParameters();
    }

    private void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");
        if(Input.GetKey(KeyCode.D))
        {
            direction = 1f;
            Move(direction);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = -1f;
            Move(direction);
        }
        else m_rb.velocity = new Vector2(0, m_rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isTouchingGround)
        {
            m_rb.velocity = new Vector2(m_rb.velocity.x, jumpSpeed);
        }
        m_animator.SetFloat("Speed", Mathf.Abs(m_rb.velocity.x));
        m_animator.SetBool("OnGround", isTouchingGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
        else if(collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
        
        else if(collision.tag == "Portal")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            respawnPoint = transform.position;
        }
    }

    private void InitializeComponents()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void InitializeParameters()
    {
        speed = 3f;
        jumpSpeed = 8f;
        direction = 0f;
        groundCheckRadius = 0.2f;
        m_maxHealth = 100;
        m_currentHealth = m_maxHealth;
        HealthFillImg.fillAmount = 1;
        respawnPoint = transform.position;
    }

    private void Move(in float p_direction)
    {
        m_rb.velocity = new Vector2(direction * speed, m_rb.velocity.y);
        if(p_direction>0)
        {
            if (m_spriteRenderer.flipX) m_spriteRenderer.flipX = false;
        }
        else
        {
            if (!m_spriteRenderer.flipX) m_spriteRenderer.flipX = true;
        }
    }

    public void ReceiveDamage(in int p_amount)
    {
        if (m_currentHealth != 0) { m_currentHealth -= p_amount; }
        HealthFillImg.fillAmount = (float)m_currentHealth/m_maxHealth;
    }
}
