using System.Collections;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallMovementManager : MonoBehaviour
{
    public float jumpHeight = 5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float crushScale = 0.5f;
    public float transitionDuration = 1f;
    public float sphereCastRadius = 0.1f;

    private Rigidbody rb;
    private bool isGrounded = false;
    private Coroutine crushEffectCoroutine;
    private Collider collideObject;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _deadParticle;
    private float _falltime = 0f;
    [SerializeField]
    private float _fallThreshold = 2f;
    private Color _originalColor;
    private MeshRenderer _renderer => GetComponent<MeshRenderer>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _originalColor = _renderer.material.color;
    }

    void Update()
    {
        _falltime += Time.deltaTime;
        if (_falltime > _fallThreshold)
        {
            // Önceki rengi geri yükle
            _renderer.material.color = Color.red;
        }
        if (isGrounded)
        {
            Jump();
            isGrounded = false;
            _falltime = 0f;
            _renderer.material.color = _originalColor;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y));
    }

    IEnumerator CrushEffect()
    {
        Vector3 originalScale = new Vector3(1, 1, 1);
        transform.localScale = new Vector3(1, crushScale, 1);

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.tag == "Enemy")
        {
            if (_falltime > _fallThreshold)
            {
                collision.gameObject.transform.parent.gameObject.GetComponent<PlatformDestroyer>().ExplodePlatform();
            }
            else
            {
                Instantiate(_deadParticle, transform.position, Quaternion.identity);
                GameManager.Instance.Dead();
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<Rigidbody>().useGravity = false;
            }
        }
        else if (collision.collider.tag == "Ground")
        {
            _audioSource.Play();
            if (_falltime > _fallThreshold)
            {
                collision.gameObject.transform.parent.gameObject.GetComponent<PlatformDestroyer>().ExplodePlatform();
            }
            else
            {
                GameManager.Instance.Splasher(this.transform);
            }
            isGrounded = true;
            if (crushEffectCoroutine != null)
            {
                StopCoroutine(crushEffectCoroutine);
            }
            crushEffectCoroutine = StartCoroutine(CrushEffect());
        }
        else if (collision.collider.gameObject.tag == "Win")
        {
            _falltime = -10;
            isGrounded = false;
            GameManager.Instance.Win(); // Örneğin, YourFunction, GameManager'da tanımlı bir fonksiyon
        }
    }
}
