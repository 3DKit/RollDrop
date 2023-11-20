using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Player objesinin referansı
    public float yOffset = 2.0f; // Kamera yüksekliğine eklenmesi istenen ofset değeri

    void Start()
    {
        player = GameManager.Instance.Player.transform;
        transform.position = new Vector3(transform.position.x, player.position.y + yOffset, transform.position.z);
    }

    void Update()
    {
        if (player == null)
            return;

        if (player.position.y + yOffset < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, player.position.y + (yOffset- 0.05f), transform.position.z);
        }
    }
}
