using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    GameObject player; // Player objesinin referansı
    Transform explosionDirection; // Fırlatma yönlendirmesi için referans transform
    float explosionForce = 13.0f; // Fırlatma kuvveti
    Vector3 offset;
    bool destroyed = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset.x = Random.Range(-1f, 1f);
        offset.y = 1;
        offset.z = Random.Range(-1f, 1f);
    }

    void Update()
    {
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
        Destroy(this.gameObject,5f);
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
