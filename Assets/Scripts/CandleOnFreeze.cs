using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Candle : MonoBehaviour, IFreezableReceiver
{
    [SerializeField] private Sprite unlitSprite;

    private SpriteRenderer sr;
    private Animator animator;
    private Light2D candleLight;
    private bool isExtinguished = false;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        candleLight = GetComponent<Light2D>();
    }

    public void CastOnFreeze()
    {
        if (isExtinguished)
            return;

        isExtinguished = true;

        // turn off light
        if (candleLight != null)
            candleLight.enabled = false;

        // stop animation
        if (animator != null)
            animator.enabled = false;

        // swap sprite
        if (sr != null && unlitSprite != null)
            sr.sprite = unlitSprite;

        // spawn smoke
    }

    public void CastOnUnfreeze()
    {
        
    }
}
