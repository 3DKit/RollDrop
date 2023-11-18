using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Player objesinin referansı
    public float yOffset = 2.0f; // Kamera yüksekliğine eklenmesi istenen ofset değeri

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player referansı atanmamış!");
            return;
        }

        // Kameranın başlangıç yüksekliğini player'ın yüksekliğine eşitle, ofset ekleyerek
        transform.position = new Vector3(transform.position.x, player.position.y + yOffset, transform.position.z);
    }

    void Update()
    {
        if (player == null)
            return;

        if (player.position.y + yOffset < transform.position.y)
        {
            // Kameranın yüksekliğini topun yüksekliğine eşitle, ofset ekleyerek
            transform.position = new Vector3(transform.position.x, player.position.y + yOffset, transform.position.z);
        }
    }
}
