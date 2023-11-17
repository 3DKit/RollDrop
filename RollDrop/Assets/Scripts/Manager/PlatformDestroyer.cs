using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    public GameObject player;
    public PlatformController platformController;
    Transform explosionDirection;
    float explosionForce = 13.0f;
    Vector3 offset;
    bool destroyed = false;

    void Start()
    {
        offset.x = Random.Range(-1f, 1f);
        offset.y = 1;
        offset.z = Random.Range(-1f, 1f);
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }
        // Eğer platformun yüksekliği topun yüksekliğinden büyükse
        if (transform.position.y > player.transform.position.y && !destroyed)
        {
            ExplodePlatform();
            destroyed = true;
        }
    }

    void ExplodePlatform()
    {
        this.transform.SetParent(null);
        Destroy(this.gameObject, 5f);
        platformController.AddProgress();
        // Tüm alt objeleri döngü ile kontrol et
        foreach (Transform child in transform)
        {
            Rigidbody childRb = child.gameObject.AddComponent<Rigidbody>();
            MeshCollider meshCollider = child.GetComponent<MeshCollider>();

            // Mesh collider'ı devre dışı bırak
            if (meshCollider != null)
            {
                meshCollider.enabled = false;
            }

            // Rigid body ekle ve fırlatma kuvveti uygula (belirli bir yönde)
            if (childRb != null)
            {
                Vector3 explosionDir = ((this.transform.position - child.position) + offset).normalized;
                childRb.AddForce(explosionDir * -explosionForce, ForceMode.VelocityChange);
            }
            Destroy(child.gameObject, 4f);
        }
    }
}
