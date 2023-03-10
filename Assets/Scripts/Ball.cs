using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject diamondPrefab;

    bool spawn;
    private bool goal = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            SoundManager.instance.PlaySound(SoundManager.instance.goalSound);
            goal = true;
            other.gameObject.GetComponent<Animator>().enabled = true;
            other.gameObject.GetComponent<Animator>().Play("Goal", -1, 0);

            GameManager.instance.streak++;
            GameManager.instance.ShowGoalFloatingText();

            if (!GameManager.instance.stopFill)
            {
                GameManager.instance.barFillAmount += 0.05f;
                GameManager.instance.FillBar();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") && !spawn)
        {
            spawn = true;
            SpawnReward();
        }
    }

    private void SpawnReward()
    {
        if (!goal)
        {
            GameManager.instance.streak = 0;
            GameManager.instance.ShowMissFloatingText();

            if(gameObject.CompareTag("BasketballCoin"))
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            else
                Instantiate(diamondPrefab, transform.position, Quaternion.identity);

            DestroyBall();
        }
        else
        {
            int random = Random.Range(3, 5);
            if (random == 3)
            {
                if (gameObject.CompareTag("BasketballCoin"))
                {
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(diamondPrefab, transform.position, Quaternion.identity);
                    Instantiate(diamondPrefab, transform.position, Quaternion.identity);
                    Instantiate(diamondPrefab, transform.position, Quaternion.identity);
                }

                DestroyBall();
            }

            else
            {
                if (gameObject.CompareTag("BasketballCoin"))
                {
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(diamondPrefab, transform.position, Quaternion.identity);
                    Instantiate(diamondPrefab, transform.position, Quaternion.identity);
                    Instantiate(diamondPrefab, transform.position, Quaternion.identity);
                    Instantiate(diamondPrefab, transform.position, Quaternion.identity);
                }

                DestroyBall();
            }
        }
    }

    private void DestroyBall()
    {
        Destroy(gameObject);
    }
}
