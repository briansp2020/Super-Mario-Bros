using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CapsuleCollider2D capsuleCollider { get; private set; }
    public PlayerMovement movement { get; private set; }
    public DeathAnimation deathAnimation { get; private set; }

    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    public PlayerSpriteRenderer fireRenderer;
    private PlayerSpriteRenderer activeRenderer;

    private AudioSource audioSource;
    public AudioClip shrinkSound;
    public AudioClip dieSound;
    public AudioClip fireballSound;

    public GameObject fireball;
    public int fireballCount = 0;
    public bool throwingFireball { get; private set; } = false;

    public bool big => bigRenderer.enabled;
    public bool small => smallRenderer.enabled;
    public bool fire => fireRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    public bool starpower { get; private set; }

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        movement = GetComponent<PlayerMovement>();
        deathAnimation = GetComponent<DeathAnimation>();
        audioSource = GetComponent<AudioSource>();
        activeRenderer = smallRenderer;
    }

    private void Update()
    {
        if (fire && Input.GetButtonDown("Fire3") && fireballCount < 2)
        {
            audioSource.PlayOneShot(fireballSound);
            fireballCount++;
            throwingFireball = true;
            GameObject createdFireball = Instantiate(fireball, transform.position + new Vector3(0.5f, 0.5f, 0), transform.rotation);
            createdFireball.GetComponent<Fireball>().player = this;
            Invoke(nameof(SetThrowingFireballToFalse), 0.1f);
        }
    }

    //Only used to set the throwingFireball boolean to false
    private void SetThrowingFireballToFalse()
    {
        throwingFireball = false;
    }

    public void Hit()
    {
        if (!dead && !starpower)
        {
            if (big) {
                Shrink();
            } else if (small) {
                Death();
            } else{
                audioSource.PlayOneShot(shrinkSound);
                MakeBig(fireRenderer);
            }
        }
    }

    public void Death()
    {
        Camera.main.GetComponent<Music>().StopMusic();
        audioSource.PlayOneShot(dieSound);
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }

    public void NoAnimationDeath()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - 100);
        Death();
    }

    public void MakeBig(PlayerSpriteRenderer before)
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        fireRenderer.enabled = false;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(TransformAnimation(before, bigRenderer));
    }

    public void Grow()
    {
        MakeBig(smallRenderer);
    }

    public void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        fireRenderer.enabled = false;
        activeRenderer = smallRenderer;

        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);

        audioSource.PlayOneShot(shrinkSound);

        StartCoroutine(TransformAnimation(bigRenderer, smallRenderer));
    }

    public void ActivateFire()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        fireRenderer.enabled = true;
        activeRenderer = fireRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(TransformAnimation(bigRenderer, fireRenderer));
    }

    private IEnumerator TransformAnimation(PlayerSpriteRenderer before, PlayerSpriteRenderer after)
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        fireRenderer.enabled = false;
        activeRenderer.enabled = true;

        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                before.enabled = !before.enabled;
                after.enabled = !before.enabled;
            }

            yield return null;
        }

        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        fireRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

    public void Starpower(AudioClip starMusic, float duration = 10)
    {
        Music music = Camera.main.GetComponent<Music>();
        music.PlayOverrideMusic(starMusic, duration);

        StartCoroutine(StarpowerAnimation());
    }

    private IEnumerator StarpowerAnimation()
    {
        starpower = true;

        float elapsed = 0f;
        float duration = 10f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0) {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;
        starpower = false;
    }

}
