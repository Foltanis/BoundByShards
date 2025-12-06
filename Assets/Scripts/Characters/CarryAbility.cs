using UnityEngine;
using UnityEngine.InputSystem;

public class CarryAbility : MonoBehaviour
{
    private bool carryingMage = false;
    private bool mageIsNearby = false;
    private PlayerInput playerInput;

    private GameObject mage;
    private Rigidbody2D mageRb;
    private BoxCollider2D mageCollider;
    private MageCarryHandler mCH;
    private float mageGravityScale;

    public Transform carryPoint; 
    public float followSpeed = 20f;
    //public float carryHeightOffset = 1.0f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["CarryMage"].performed += OnCarryPressed;

    }

    private void Start()
    {
        mage = CharacterManager.Instance.Get(CharacterType.Mage);
        mageRb = mage.GetComponent<Rigidbody2D>();
        mageCollider = mage.GetComponent<BoxCollider2D>();
        mageGravityScale = mageRb.gravityScale;

        // logic is connected to mage carry handler
        mCH = mage.GetComponent<MageCarryHandler>();
    }

    private void FixedUpdate()
    {
        if (carryingMage)
        {
            mage.transform.position = carryPoint.position;

            // TODO Keep rotation aligned
            mageRb.rotation = transform.rotation.x;
        }


    }

    private void OnCarryPressed(InputAction.CallbackContext ctx)
    {
        if (!carryingMage)
        {
            TryPickupMage();
        }
        else
        {
            DropMage();
        }
    }

    private void TryPickupMage()
    {
        if (mageIsNearby)
        {
            mCH.SetCarrier(this);

            mage.transform.SetParent(carryPoint);
            mage.transform.localPosition = Vector3.zero;

            mageRb.gravityScale = 0f;

            carryingMage = true;
        }
    }

    public void DropMage()
    {
        Debug.Log("Dropping mage");
        mage.transform.SetParent(null);
        
        mageRb.gravityScale = mageGravityScale;

        carryingMage = false;
    }

    // Ensure mage is dropped if Connslime is disabled
    private void OnDisable()
    {
        DropMage();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == mage)
        {
            mageIsNearby = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == mage)
        {
            mageIsNearby = false;
        }
    }

}
