using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("BALL VARIABLES")]
    [SerializeField] GameObject staticBall;
    [SerializeField] GameObject coinBallPrefab;
    [SerializeField] GameObject diamondBallPrefab;
    [SerializeField] float throwForceX;
    [SerializeField] float throwForceZ;
    [SerializeField] float throwForceY;
    Vector2 startPos, endPos, direction;

    [Header("Basket")]
    [SerializeField] GameObject basket;

    [Header("Obstacle")]
    [SerializeField] GameObject obstacle;


    [Header("UI")]
    [SerializeField] Text ballsText;
    [SerializeField] Text goalFloatingText;
    [SerializeField] Text missFloatingText;
    [SerializeField] Image barFilled;
    [SerializeField] GameObject fader;
    [SerializeField] Button enlargeBasketButton;
    [SerializeField] Button clearObstaclesButton;
    [SerializeField] GameObject enlargeBasketPopUp;
    [SerializeField] GameObject clearObstaclePopUp;
    [SerializeField] GameObject buyBasketballPopUp;
    [SerializeField] GameObject slotMachinePopUp;
    [SerializeField] GameObject notEnoughResourcesPopUp;


    [SerializeField] List<GameObject> feverModeComponents;

    private bool enableThrow = true;
    [HideInInspector] public int streak = 0; 
    private int balls;
    [HideInInspector] public float barFillAmount = 0;
    [HideInInspector] public bool stopFill;
    [HideInInspector] public bool feverMode;
    public float enlargeBasketTimer = 60;
    public float clearObstaclesTimer = 60;
    private bool uiEnabled;
    

    private void Awake()
    {
        instance = this;


        if (PlayerPrefs.HasKey("Balls"))       
            balls = PlayerPrefs.GetInt("Balls");     
        else        
            balls = 50;       
        ballsText.text = "Balls: " + balls;


        if (PlayerPrefs.HasKey("BarFilled"))       
            barFillAmount = PlayerPrefs.GetFloat("BarFilled");
        FillBar();
    }

    private void Start()
    {
        Time.timeScale = 2;
    }

    void Update()
    {
        if (balls > 0 && enableThrow && !uiEnabled)
        {
            if(!staticBall.activeSelf)
                staticBall.SetActive(true);

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.currentSelectedGameObject)
            {
                startPos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0) && !EventSystem.current.currentSelectedGameObject)
            {
                SoundManager.instance.PlaySound(SoundManager.instance.swooshSound);

                endPos = Input.mousePosition;
                direction = startPos - endPos;
                GameObject ball = Instantiate(coinBallPrefab, new Vector3(0, -6.41f, -1.84f), Quaternion.identity);

                int random = Random.Range(0, 5);
                if (random == 4)
                    ball.tag = "BasketballDiamond";
                else
                    ball.tag = "BasketballCoin";

                if (feverMode)
                {
                    ball.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                    ball.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Play();
                }

                ball.GetComponent<Rigidbody>().isKinematic = false;
                ball.GetComponent<Rigidbody>().AddForce(-direction.x * throwForceX, throwForceY, throwForceZ);
                ball.transform.DOScale(new Vector3(38, 38, 38), 1f).SetEase(Ease.Linear);
                staticBall.SetActive(false);
                DeductBalls(1);
                enableThrow = false;
                Invoke("EnableThrow", 1);
            }
        }

        if(balls == 0)
        {
            if(staticBall.activeSelf)
                staticBall.SetActive(false);

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.currentSelectedGameObject)
            {
                if(!fader.activeSelf && !buyBasketballPopUp.activeSelf)
                    OpenPopUp(buyBasketballPopUp);
            }
        }
    }

    private void EnableThrow()
    {
        staticBall.SetActive(true);

        if (feverMode)
        {
            staticBall.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            staticBall.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Play();
        }

        enableThrow = true;
    }

    private void DeductBalls(int amount)
    {
        balls -= amount;
        ballsText.text = "Balls: " + balls;
        PlayerPrefs.SetInt("Balls", balls);
    }

    public void ShowGoalFloatingText()
    {
        goalFloatingText.gameObject.SetActive(true);
        goalFloatingText.gameObject.GetComponent<Animator>().Play("FloatingText", -1, 0);
        goalFloatingText.text = "PERFECT x " + streak;
    }

    public void ShowMissFloatingText()
    {
        missFloatingText.gameObject.SetActive(true);
        missFloatingText.gameObject.GetComponent<Animator>().Play("FloatingText", -1, 0);
    }

    public void FillBar()
    {
        if (!stopFill)
        {
            barFilled.DOFillAmount(barFillAmount, 0.3f).OnComplete(delegate() {
                if (barFillAmount >= 1)
                {
                    stopFill = true;
                    barFillAmount = 0;
                    FillBar();
                }
                PlayerPrefs.SetFloat("BarFilled", barFillAmount);
                return;
            });
        }

        else
        {
            EnableFeverMode();
            barFilled.DOFillAmount(barFillAmount, 30f).OnComplete(delegate ()
            {
                DisabelFeverMode();
                stopFill = false;
            });
        }
    }

    private void EnableFeverMode()
    {
        SoundManager.instance.ChangeBG(SoundManager.instance.bgFeverMusic);

        feverMode = true;
        for (int i = 0; i < feverModeComponents.Count; i++)
            feverModeComponents[i].GetComponent<ParticleSystem>().Play();
    }

    private void DisabelFeverMode()
    {
        SoundManager.instance.ChangeBG(SoundManager.instance.bgMusic);

        feverMode = false;
        for (int i = 0; i < feverModeComponents.Count; i++)
            feverModeComponents[i].GetComponent<ParticleSystem>().Stop();
    }

    public void OpenPopUp(GameObject popUp)
    {
        uiEnabled = true;
        fader.GetComponent<Image>().DOFade(0, 0).OnComplete(delegate (){
            fader.SetActive(true);
            fader.GetComponent<Image>().DOFade(0.7f, 1).OnComplete(delegate ()
            {
                popUp.SetActive(true);
                popUp.GetComponent<Animator>().Play("PopUpOpen", -1, 0);                    
            });
        });
    }
    public void ClosePopUp(GameObject popUp)
    {
        uiEnabled = false;
        fader.SetActive(false);
        popUp.SetActive(false);
    }

    public void EnlargeBasket(int amount)
    {
        if (amount <= RewardManager.instance.diamonds)
        {
            SoundManager.instance.PlaySound(SoundManager.instance.buyProp);
            basket.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 3);
            RewardManager.instance.AddDiamonds(-amount);
            enlargeBasketButton.interactable = false;
            enlargeBasketButton.transform.GetChild(1).gameObject.GetComponent<Image>().fillAmount = 1;
            enlargeBasketButton.transform.GetChild(1).gameObject.SetActive(true);
            enlargeBasketTimer = 60;
            ClosePopUp(enlargeBasketPopUp);
            EnlargeBasketTimer();
        }
        else
        {
            SoundManager.instance.PlaySound(SoundManager.instance.buyError);
            notEnoughResourcesPopUp.SetActive(true);
            notEnoughResourcesPopUp.GetComponent<Animator>().Play("NotEnoughResources", -1, 0);
        }
    }
    private void EnlargeBasketTimer()
    {
        if (enlargeBasketTimer > 0)
        {
            enlargeBasketTimer -= 1;
            enlargeBasketButton.transform.GetChild(1).gameObject.GetComponent<Image>().fillAmount -=  1f / 60f;
            Invoke("EnlargeBasketTimer", 2);
        }

        else
        {
            enlargeBasketButton.interactable = true;
            enlargeBasketButton.transform.GetChild(1).gameObject.SetActive(false);
            basket.transform.DOScale(new Vector3(1f, 1f, 1f), 3);
        }
    }

    public void ClearObstacles(int amount)
    {
        if (amount <= RewardManager.instance.diamonds)
        {
            SoundManager.instance.PlaySound(SoundManager.instance.buyProp);
            obstacle.SetActive(false);
            RewardManager.instance.AddDiamonds(-amount);
            clearObstaclesButton.interactable = false;
            clearObstaclesButton.transform.GetChild(1).gameObject.GetComponent<Image>().fillAmount = 1;
            clearObstaclesButton.transform.GetChild(1).gameObject.SetActive(true);
            clearObstaclesTimer = 60;
            ClosePopUp(clearObstaclePopUp);
            ClearObstaclesTimer();
        }

        else
        {
            SoundManager.instance.PlaySound(SoundManager.instance.buyError);
            notEnoughResourcesPopUp.SetActive(true);
            notEnoughResourcesPopUp.GetComponent<Animator>().Play("NotEnoughResources", -1, 0);
        }
    }
    private void ClearObstaclesTimer()
    {
        if (clearObstaclesTimer > 0)
        {
            clearObstaclesTimer -= 1;
            clearObstaclesButton.transform.GetChild(1).gameObject.GetComponent<Image>().fillAmount -= 1f / 60f;
            Invoke("ClearObstaclesTimer", 2);
        }

        else
        {
            clearObstaclesButton.interactable = true;
            clearObstaclesButton.transform.GetChild(1).gameObject.SetActive(false);
            obstacle.SetActive(true);
        }
    }

    public void BuyBasketballs(int amount)
    {
        if (RewardManager.instance.coins >= amount)
        {
            SoundManager.instance.PlaySound(SoundManager.instance.buyProp);
            RewardManager.instance.AddCoins(-amount);
            balls = 50;
            ballsText.text = "Balls: " + balls;
            PlayerPrefs.SetInt("Balls", balls);
            ClosePopUp(buyBasketballPopUp);
        }

        else
        {
            SoundManager.instance.PlaySound(SoundManager.instance.buyError);
            notEnoughResourcesPopUp.SetActive(true);
            notEnoughResourcesPopUp.GetComponent<Animator>().Play("NotEnoughResources", -1, 0);
        }
    }
    public void AddBasketballs(int amount)
    {
        balls += amount;
        ballsText.text = "Balls " + balls;
        PlayerPrefs.SetInt("Balls", balls);
    }

    public void ActivateSlotMachine(int amount)
    {
        if (RewardManager.instance.diamonds >= amount)
        {
            SoundManager.instance.PlaySound(SoundManager.instance.buyProp);
            SlotMachine.instance.StartCoroutine(SlotMachine.instance.StartSlotMachine());
            RewardManager.instance.AddDiamonds(-amount);
            ClosePopUp(slotMachinePopUp);
        }

        else
        {
            SoundManager.instance.PlaySound(SoundManager.instance.buyError);
            notEnoughResourcesPopUp.SetActive(true);
            notEnoughResourcesPopUp.GetComponent<Animator>().Play("NotEnoughResources", -1, 0);
        }
    }
}
