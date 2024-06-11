using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    private bool shouldRetreat = false;

    void Start()
    {
        GameObject TMPplayer = GameObject.FindWithTag("Player");
        player = TMPplayer.transform;
    }

    void Update()
    {
        if (!shouldRetreat)
        {
            MoveTowardsPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            StartCoroutine(RetreatAndResume());
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    IEnumerator RetreatAndResume()
    {
        shouldRetreat = true;
        float retreatTime = 3f; // Время отступления
        while (retreatTime > 0)
        {
            Vector3 retreatDirection = (transform.position - player.position).normalized;
            transform.position += retreatDirection * moveSpeed * Time.deltaTime;
            retreatTime -= Time.deltaTime;
            yield return null; // Ждем следующий кадр
        }

        shouldRetreat = false;
    }
}
