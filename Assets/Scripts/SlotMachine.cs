using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    public static SlotMachine instance;
    [SerializeField] private Sprite[] rewardSprites;

    [SerializeField] GameObject slot1;
    [SerializeField] GameObject slot2;
    [SerializeField] GameObject slot3;

    [SerializeField] List<GameObject> rewardSlots;


    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject diamondPrefab;

    [SerializeField] Button slotMachineButton;

    [HideInInspector] public bool checkSlots;

    private int randomTimes = 40;
    private int temp1 = 0;
    private int temp2 = 0;
    private int temp3 = 0;

    private int[] rewardAmount = { 5, 10, 15, 20 };

    private void Awake()
    {
        checkSlots = true;
        instance = this;
    }

    public IEnumerator StartSlotMachine()
    {
        checkSlots = false;
        slotMachineButton.interactable = false;

        for (int i = 0; i < rewardSlots.Count; i++)
        {
            rewardSlots[i].transform.GetChild(1).gameObject.SetActive(true);
            rewardSlots[i].transform.GetChild(1).GetComponent<Animator>().enabled = true;
        }

        yield return new WaitForSeconds(5);

        ResetSlotMachine();
        CancelInvoke();

        slot1.transform.GetChild(0).gameObject.SetActive(true);
        slot2.transform.GetChild(0).gameObject.SetActive(true);
        slot3.transform.GetChild(0).gameObject.SetActive(true);

        SoundManager.instance.PlaySlotMachineSound();
        RandomizeSlot1();
        RandomizeSlot2();
        RandomizeSlot3();

        Invoke("EnableSlotMachineButton", 18);
    }

    private void EnableSlotMachineButton()
    {
        slotMachineButton.interactable = true;
    }

    private void RandomizeSlot1()
    {
        if (temp1 < randomTimes)
        {
            temp1++;
            slot1.transform.GetChild(0).GetComponent<Image>().sprite = rewardSprites[Random.Range(0, rewardSprites.Length)];
            Invoke("RandomizeSlot1", 0.2f);
        }
    }

    private void RandomizeSlot2()
    {
        if (temp2 < randomTimes)
        {
            temp2++;
            slot2.transform.GetChild(0).GetComponent<Image>().sprite = rewardSprites[Random.Range(0, rewardSprites.Length)];
            Invoke("RandomizeSlot2", 0.2f);
        }
    }

    private void RandomizeSlot3()
    {
        if (temp3 < randomTimes)
        {
            temp3++;
            slot3.transform.GetChild(0).GetComponent<Image>().sprite = rewardSprites[Random.Range(0, rewardSprites.Length)];
            Invoke("RandomizeSlot3", 0.2f);
        }

        else
            GenerateReward();     
    }

    private void GenerateReward()
    {
        SoundManager.instance.StopSlotMachineSound();

        int reward = Random.Range(0, 3);
        int rewardValue = Random.Range(0, rewardAmount.Length);

        slot1.transform.GetChild(1).gameObject.SetActive(true);
        slot2.transform.GetChild(1).gameObject.SetActive(true);
        slot3.transform.GetChild(1).gameObject.SetActive(true);

        slot1.transform.GetChild(1).GetComponent<Text>().text = "+" + rewardAmount[rewardValue];
        slot2.transform.GetChild(1).GetComponent<Text>().text = "+" + rewardAmount[rewardValue];
        slot3.transform.GetChild(1).GetComponent<Text>().text = "+" + rewardAmount[rewardValue];

        slot1.transform.GetChild(0).GetComponent<Image>().sprite = rewardSprites[reward];
        slot2.transform.GetChild(0).GetComponent<Image>().sprite = rewardSprites[reward];
        slot3.transform.GetChild(0).GetComponent<Image>().sprite = rewardSprites[reward];

        if (rewardSprites[reward].name.Equals("Coin"))
            StartCoroutine(SpawnCoins(rewardAmount[rewardValue]));
        if (rewardSprites[reward].name.Equals("Diamond"))
            StartCoroutine(SpawnDiamonds(rewardAmount[rewardValue]));
        if (rewardSprites[reward].name.Equals("Basketball"))
            StartCoroutine(AddBasketballs(rewardAmount[rewardValue]));

    }

    IEnumerator SpawnCoins(int amount)
    {
        yield return new WaitForSeconds(2);

        for (int i = 0; i < amount; i++)
        {
            Instantiate(coinPrefab, new Vector3(0, -7.73f, 7.54f), Quaternion.identity);
        }
    }

    IEnumerator SpawnDiamonds(int amount)
    {
        yield return new WaitForSeconds(2);

        for (int i = 0; i < amount; i++)
        {
            Instantiate(diamondPrefab, new Vector3(0, -7.73f, 7.54f), Quaternion.identity);
        }

    }

    IEnumerator AddBasketballs(int amount)
    {
        yield return new WaitForSeconds(2);
        GameManager.instance.AddBasketballs(amount);
    }


    private void ResetSlotMachine()
    {
        for (int i = 0; i < rewardSlots.Count; i++)
        {
            rewardSlots[i].transform.GetChild(1).GetComponent<Animator>().enabled = false;
            rewardSlots[i].GetComponent<RewardMultiplierSlot>().lightUp = false;
            rewardSlots[i].transform.GetChild(1).gameObject.SetActive(false);
        }
          
        slot1.transform.GetChild(0).gameObject.SetActive(false);
        slot2.transform.GetChild(0).gameObject.SetActive(false);
        slot3.transform.GetChild(0).gameObject.SetActive(false);

        slot1.transform.GetChild(1).gameObject.SetActive(false);
        slot2.transform.GetChild(1).gameObject.SetActive(false);
        slot3.transform.GetChild(1).gameObject.SetActive(false);

        temp1 = 0;
        temp2 = 0;
        temp3 = 0;
        checkSlots = true;
    }

    private void Update()
    {
        if (rewardSlots[0].GetComponent<RewardMultiplierSlot>().lightUp
            && rewardSlots[1].GetComponent<RewardMultiplierSlot>().lightUp
            && rewardSlots[2].GetComponent<RewardMultiplierSlot>().lightUp
            && rewardSlots[3].GetComponent<RewardMultiplierSlot>().lightUp
            && rewardSlots[4].GetComponent<RewardMultiplierSlot>().lightUp
            && rewardSlots[5].GetComponent<RewardMultiplierSlot>().lightUp
            && rewardSlots[6].GetComponent<RewardMultiplierSlot>().lightUp && checkSlots)
        {
            StartCoroutine(StartSlotMachine());
        }
    }

}
