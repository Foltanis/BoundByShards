using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
public class FireballController : MonoBehaviour, IFreezableReceiver, IFireballSignalReceiver
{
    private Rigidbody2D rb;
    private Collider2D myCollider;

    private Vector2 direction;
    private float speed;
    private int damage;
    private GameObject caster;
    private Collider2D casterCollider;

    private GameObject target;
    private NavMeshAgent agent;
    private float repathTimer = 0f;
    private float repathInterval = 0.2f;

    private bool ignoreSignals;

    private FireballMoveMode moveMode = FireballMoveMode.Straight;
    private Vector2 straightDirection;

    [SerializeField] private GameObject explosionParticlesPrefab;

    public enum FireballMoveMode
    {
        Straight,
        Navigating
    }

    public void Init(Vector2 dir, float speed, int dmg, GameObject caster)
    {
        direction = dir;
        this.speed = speed;
        damage = dmg;
        this.caster = caster;
        casterCollider = caster.GetComponent<Collider2D>();

        ignoreSignals = CharacterManager.Instance.Get(CharacterType.Mage) == caster ? true : false;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        rb.linearVelocity = direction * speed;
        Physics2D.IgnoreCollision(myCollider, casterCollider, true);

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        agent.autoBraking = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        agent.enabled = false;

        target = CharacterManager.Instance.Get(CharacterType.Mage);

        SoundManager.PlaySound(SoundType.FIREBALL, gameObject, 0.3f);
    }

    void Update()
    {
        if (moveMode != FireballMoveMode.Navigating || target == null)
            return;

        repathTimer += Time.deltaTime;

        if (repathTimer >= repathInterval)
        {
            repathTimer = 0f;
            agent.SetDestination(target.transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var hp = col.gameObject.GetComponent<Health>();
        if (hp != null)
            hp.TakeDamage(damage);

        if (explosionParticlesPrefab != null)
            Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);

        SoundManager.PlaySound(SoundType.FIREBALL_HIT, gameObject, 0.3f);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SoundManager.StopSound(SoundType.FIREBALL, gameObject);
    }

    public void OnEnemySeen(GameObject enemy)
    {
        if (ignoreSignals)
            return;

        target = enemy;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        agent.enabled = true;

        // Wait one frame for agent to be placed on NavMesh
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(target.transform.position);
            Debug.Log("Fireball switching to navigating mode towards " + enemy.name);
            moveMode = FireballMoveMode.Navigating;
        }
        else
        {
            // If not on NavMesh, use Warp to place it
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
                agent.SetDestination(target.transform.position);
                Debug.Log("Fireball switching to navigating mode towards " + enemy.name);
                moveMode = FireballMoveMode.Navigating;
            }
            else
            {
                // Can't find NavMesh, stay in straight mode
                Debug.LogWarning("Fireball cannot find NavMesh, staying in straight mode");
                agent.enabled = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.linearVelocity = straightDirection * speed;
            }
        }
    }
    public void OnEnemyLost()
    {
        if (ignoreSignals || moveMode != FireballMoveMode.Navigating)
            return;

        // Get current velocity from agent before disabling
        if (agent.enabled && agent.isOnNavMesh)
        {
            Vector3 vel = agent.velocity;
            if (vel.sqrMagnitude > 0.01f)
                straightDirection = vel.normalized;
        }

        agent.enabled = false;

        // Switch back to Dynamic and apply velocity
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = straightDirection * speed;

        Debug.Log("Fireball switching back to straight mode");
        moveMode = FireballMoveMode.Straight;
    }

    public void CastOnFreeze()
    {
        Destroy(gameObject);
    }

    public void CastOnUnfreeze()
    {
        // No action needed on unfreeze for the fireball
    }

    private void OnEnable()
    {
        FireballSignalBroadcaster.Register(this);
    }

    private void OnDisable()
    {
        FireballSignalBroadcaster.Unregister(this);
    }
}
