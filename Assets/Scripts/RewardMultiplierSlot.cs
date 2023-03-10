using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardMultiplierSlot : MonoBehaviour
{
    [SerializeField] GameObject coinParticlePrefab;
    [SerializeField] GameObject diamondParticlePrefab;
    [SerializeField] Transform particleOriginPoint;
    [SerializeField] Transform targetPointForCoin;
    [SerializeField] Transform targetPointForDiamond;

    [SerializeField] Text rewardMultiplierText;
    private int rewardMultiplier = 1;
    [HideInInspector] public bool lightUp;
    private int[] possibleMultipliers = { 1, 2, 5, 10, 15, 20, 25 };
    int balls;

    private void Start()
    {
        rewardMultiplierText.text = "x" + rewardMultiplier;
        RandomizeMultiplier();
    }


    private void RandomizeMultiplier()
    {
        int temp = rewardMultiplier;
        rewardMultiplier = possibleMultipliers[Random.Range(0, possibleMultipliers.Length)];

        if (temp == rewardMultiplier)
        {
            RandomizeMultiplier();
            return;
        }
       
        balls = 1;

        rewardMultiplierText.text = "x" + rewardMultiplier;
        Invoke("RandomizeMultiplier", 10);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            GameObject coinParticle = Instantiate(coinParticlePrefab, particleOriginPoint.position, Quaternion.identity, GameObject.Find("UICanvas").transform);

            if (GameManager.instance.feverMode)
                coinParticle.GetComponent<FloatingMovement>().Move(targetPointForCoin, balls * rewardMultiplier * 2, "Coin");
            else
                coinParticle.GetComponent<FloatingMovement>().Move(targetPointForCoin, balls * rewardMultiplier, "Coin");

            balls++;
            SoundManager.instance.PlaySound(SoundManager.instance.slotSound);

            other.gameObject.tag = "Untagged";
            Destroy(other.gameObject, 2);
        }

        if (other.gameObject.CompareTag("Diamond"))
        {
            GameObject diamondParticle = Instantiate(diamondParticlePrefab, particleOriginPoint.position, Quaternion.identity, GameObject.Find("UICanvas").transform);

            if (GameManager.instance.feverMode)
                diamondParticle.GetComponent<FloatingMovement>().Move(targetPointForDiamond, balls * rewardMultiplier * 2, "Diamond");
            else
                diamondParticle.GetComponent<FloatingMovement>().Move(targetPointForDiamond, balls * rewardMultiplier, "Diamond");

            balls++;
            SoundManager.instance.PlaySound(SoundManager.instance.slotSound);

            other.gameObject.tag = "Untagged";
            Destroy(other.gameObject, 2);
        }

        if (!lightUp)
        {
            lightUp = true;
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
