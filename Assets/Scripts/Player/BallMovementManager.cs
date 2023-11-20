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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isGrounded)
        {
            Jump();
            isGrounded = false;
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
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
        else if (collision.collider.tag == "Ground")
        {
            _audioSource.Play();
            isGrounded = true;
            if (crushEffectCoroutine != null)
            {
                StopCoroutine(crushEffectCoroutine);
            }
            crushEffectCoroutine = StartCoroutine(CrushEffect());
            GameManager.Instance.Splasher(this.transform);
        }
        else if (collision.collider.gameObject.tag == "Win")
        {
            isGrounded = false;
            GameManager.Instance.Win(); // Örneğin, YourFunction, GameManager'da tanımlı bir fonksiyon
        }
    }
}
