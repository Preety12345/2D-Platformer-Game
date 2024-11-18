using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private const string k_moduleName1 = nameof(PlayerController);
    [Header(k_moduleName1 + "    :     Assignables"), SerializeField, Range(0, 0)]
    private byte m_header1;
    [SerializeField]private Transform groundCheck;
    [SerializeField] private ParticleSystem jetSmoke;
    [SerializeField] private Image HealthFillImg;

    private int m_currentHealth;
    private int m_maxHealth;
    private float speed;
    private float jumpSpeed;
    private float climbSpeed;
    private float horizontalInput;
    private float verticalInput;
    private float groundCheckRadius;
    [SerializeField]private bool isTouchingGround;
    [SerializeField]private bool isTouchingLadder;
    private Rigidbody2D m_rb;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;
    private Vector3 respawnPoint;
    private Vector2 jetSmokeInitialPos;
    private CapsuleCollider2D m_collider;
    private readonly int ac_jump = Animator.StringToHash("Jump");
    private readonly int ac_climb = Animator.StringToHash("Climb");

    public float HorizontalInput => horizontalInput;

    private void Awake()
    {
        InitializeComponents();
        InitializeParameters();
    }

    private void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));
        isTouchingLadder = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ladder"));
        Climb();
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
            Move(horizontalInput);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
            Move(horizontalInput);
        }        
        else m_rb.velocity = new Vector2(0, m_rb.velocity.y);
        Jump();
        m_animator.SetFloat("Speed", Mathf.Abs(m_rb.velocity.x));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string name = collision.tag;
        switch(name)
        {
            case "FallDetector":
                transform.position = respawnPoint;
                break;
            case "CheckPoint":
                respawnPoint = transform.position;
                break;
            case "Portal":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
        }
    }

    private void InitializeComponents()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_collider = GetComponent<CapsuleCollider2D>();
    }

    private void InitializeParameters()
    {
        speed = 3f;
        jumpSpeed = 8f;
        horizontalInput = 0f;
        groundCheckRadius = 0.2f;
        climbSpeed = 4f;
        m_maxHealth = 100;
        m_currentHealth = m_maxHealth;
        HealthFillImg.fillAmount = 1;
        respawnPoint = transform.position;
        jetSmokeInitialPos = new Vector2(-1.09f, 1.04f);
    }

    private void Move(in float p_direction)
    {
        m_rb.velocity = new Vector2(horizontalInput * speed, m_rb.velocity.y);
        if (p_direction > 0)
        {
            if (m_spriteRenderer.flipX) m_spriteRenderer.flipX = false;
            jetSmoke.transform.localPosition = jetSmokeInitialPos;
        }

        else
        {
            if (!m_spriteRenderer.flipX) m_spriteRenderer.flipX = true;
            jetSmoke.transform.localPosition = new Vector2(1.09f, 1.04f);
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isTouchingGround)
        {
            m_rb.velocity = new Vector2(m_rb.velocity.x, jumpSpeed);
            jetSmoke.Play();
        }
        if(!isTouchingLadder)m_animator.SetBool(ac_jump, isTouchingGround);
    }

    private void Climb()
    {
        if(isTouchingLadder)
        {
            m_rb.gravityScale = 0f;
            m_collider.enabled = false;
            verticalInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0;
            m_rb.velocity = new Vector2(0, verticalInput * climbSpeed);
            
        }
        else
        {
            m_collider.enabled = true;
            m_rb.gravityScale = 1f;
        }
        m_animator.SetBool(ac_climb, !isTouchingGround && isTouchingLadder ? true : false);
    }

    public void ReceiveDamage(in int p_amount)
    {
        if (m_currentHealth != 0) { m_currentHealth -= p_amount; }
        HealthFillImg.fillAmount = (float)m_currentHealth/m_maxHealth;
    }
}
