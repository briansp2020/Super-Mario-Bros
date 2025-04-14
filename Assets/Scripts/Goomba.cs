using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;

    private AudioSource audioSource;
    public AudioClip flatSound;
    public AudioClip dieSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player player))
        {
            if (player.starpower) {
                Hit();
            } else if (collision.transform.DotTest(transform, Vector2.down)) {
                Flatten();
            } else {
                player.Hit();
            }
        } 
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Fireball"))
        {
            Destroy(collision.gameObject);
            Hit();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell")) {
            Hit();
        }
    }

    private void Flatten()
    {
        audioSource.PlayOneShot(flatSound);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite;
        GameManager.Instance.AddScore(100);
        Destroy(gameObject, 0.5f);
    }

    private void Hit()
    {
        audioSource.PlayOneShot(dieSound);
        GetComponent<SpriteRenderer>().flipY=true;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        GameManager.Instance.AddScore(100);
        Destroy(gameObject, 3f);
    }

}
