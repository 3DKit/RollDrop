using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        SphereCastGroundCheck();

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

    void SphereCastGroundCheck()
    {
        RaycastHit hit;
        Vector3 sphereCastOrigin = transform.position + new Vector3(sphereCastRadius, 0f, 0f);
        Vector3 invertsphereCastOrigin = transform.position - new Vector3(sphereCastRadius, 0f, 0f);
        if (Physics.SphereCast(sphereCastOrigin, 0f, Vector3.down, out hit, 0.31f) || Physics.SphereCast(invertsphereCastOrigin, 0f, Vector3.down, out hit, 0.31f))
        {
            Debug.DrawRay(sphereCastOrigin, Vector3.down * hit.distance, Color.green); // Işının isabet ettiği yeri yeşil renkte çiz
            Debug.DrawRay(invertsphereCastOrigin, Vector3.down * hit.distance, Color.green); // Işının isabet ettiği yeri yeşil renkte çiz
            if (hit.collider.tag == "Ground")
            {
                isGrounded = true;
            }else if(hit.collider.gameObject.tag == "Win"){
                isGrounded = false;
            }
        }
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (crushEffectCoroutine != null)
            {
                StopCoroutine(crushEffectCoroutine);
            }
            crushEffectCoroutine = StartCoroutine(CrushEffect());
        }
    }
}
