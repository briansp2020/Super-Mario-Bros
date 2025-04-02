using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
        FireFlower,
    }

    public Type type;

    private AudioSource audioSource;
    public AudioClip spawnSound;
    public AudioClip collectSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (spawnSound != null )
        {
            audioSource.PlayOneShot(spawnSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player)) {
            Collect(player);
        }
    }

    private void Collect(Player player)
    {
        AudioSource playerAudioSource = player.GetComponent<AudioSource>();
        if (type != Type.Starpower )
        {
            playerAudioSource.PlayOneShot(collectSound);
        }

        switch (type)
        {
            case Type.Coin:
                GameManager.Instance.AddCoin();
                break;

            case Type.ExtraLife:
                GameManager.Instance.AddLife();
                break;

            case Type.MagicMushroom:
                if (player.small)
                {
                    player.Grow();
                }
                break;

            case Type.Starpower:
                player.Starpower(collectSound);
                break;

            case Type.FireFlower:
                if (!player.fire)
                {
                    player.ActivateFire();
                }
                break;
        }

        Destroy(gameObject);
    }

}
