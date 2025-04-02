using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteRenderer : MonoBehaviour
{
    private PlayerMovement movement;
    private Player player;
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public Sprite action = null;
    public AnimatedSprite run;

    private void Awake()
    {
        movement = GetComponentInParent<PlayerMovement>();
        player = GetComponentInParent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        run.enabled = movement.running;
        if (movement.jumping) {
            spriteRenderer.sprite = jump;
        } else if (player.throwingFireball){
            spriteRenderer.sprite = action;
        } else if (movement.sliding) {
            spriteRenderer.sprite = slide;
        } else if (!movement.running) {
            spriteRenderer.sprite = idle;
        }
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        run.enabled = false;
    }

}
