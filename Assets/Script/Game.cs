using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject leftButton;
    public GameObject rightButton;

    public Text Score;
    public Text Odds;
    public Text Broke;
    public Text Cash;
    public Text Timer;
    public Text Lefts;
    public Text Rights;
    public Text Pb;
    public Text CashInText;

    public Button cashButton;
    public Button backButton;

    public GameObject cashHalo;
    public GameObject cashHaloBig;
    public GameObject LHalo;
    public GameObject RHalo;
    public GameObject LHaloBig;
    public GameObject RHaloBig;

    public float shakeStrength = 1;
    public int scoreNum;
    public int pbNum;
    public int oddNum = 2;
    public int num = 10;
    public int numlefts;
    public int numrights;
    public bool clickAllow = true;
    public bool clickAllow2 = true;
    public bool Pause = false;
    public float currentTime;
    public int seconds;

    public ParticleSystem backgroundLights;

    public Canvas gameCanvas;
    public Canvas brokeCanvas;
    public Canvas cashCanvas;
    public Canvas pauseCanvas;

    void Start()
    {
        CashInText.color = new Color32(255, 223, 0, 127);
        cashButton.interactable = false;
        cashHalo.SetActive(false);
        brokeCanvas.gameObject.SetActive(false);
        Score.text = "SCORE: 0";
        Odds.text = "ODDS: 1/2";
        gameCanvas.gameObject.SetActive(true);
        RandUpdate();
        StartCoroutine(time());
    }

    void Restart()
    {
        var ps = backgroundLights.emission;
        ps.rateOverTime = 20;
        Pause = false;
        cashHalo.SetActive(false);
        cashHaloBig.SetActive(false);
        CashInText.color = new Color32(255, 223, 0, 127);
        cashButton.interactable = false;
        numrights = 0;
        numlefts = 0;
        currentTime = 0;
        seconds = 0;
        Timer.text = "TIME: " + currentTime + " s";
        Lefts.text = "LEFTS: 0";
        Rights.text = "RIGHTS: 0";
        scoreNum = 0;
        Score.text = "SCORE: " + scoreNum;
        oddNum = 2;
        Odds.text = "ODDS: 1/" + oddNum;
        brokeCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        RandUpdate();
    }

    void Update()
    {
        if (((Input.GetKeyDown("space")) || (Input.GetKeyDown("p")) || (Input.GetKeyDown("backspace")) || (Input.GetKeyDown("escape"))) && clickAllow == true && clickAllow2 == true)
        {
            if (Pause == false)
            {
                FindObjectOfType<AudioManager>().Play("ButtonSmall");
                Time.timeScale = 0;
                pauseCanvas.gameObject.SetActive(true);
                Pause = true;
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("ButtonSmall");
                pauseCanvas.gameObject.SetActive(false);
                Pause = false;
                Time.timeScale = 1;
            }
        }

        transform.position = (Vector3)(Random.insideUnitCircle * scoreNum/7.5f);
        if (shakeStrength > 0.1)
        {
            Mathf.Clamp(shakeStrength -= Time.deltaTime, 0, scoreNum);
        }
        else if (shakeStrength < scoreNum)
        {
            Mathf.Clamp(shakeStrength += Time.deltaTime, 0, scoreNum);
        }

        RHalo.SetActive(true);
        LHalo.SetActive(true);
        RHaloBig.SetActive(false);
        LHaloBig.SetActive(false);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && Pause == false)
        {
            if (hit.transform.name == "leftButton")
            {
                LHalo.SetActive(false);
                LHaloBig.SetActive(true);
            }

            else if (hit.transform.name == "rightButton")
            {
                RHalo.SetActive(false);
                RHaloBig.SetActive(true);
            }
        }

        if (Input.GetMouseButtonDown(0) && clickAllow == true && clickAllow2 == true && Pause == false)
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "leftButton")
                {
                    LRButtonClick(leftButton, 0, Lefts, "LEFTS: ", numlefts);
                    numlefts = numlefts + 1;
                }

                else if (hit.transform.name == "rightButton")
                {
                    LRButtonClick(rightButton, 1, Rights, "RIGHTS: ", numrights);
                    numrights = numrights + 1;
                }
            }
        }
    }

    public void PauseExit()
    {
        FindObjectOfType<AudioManager>().Play("ButtonSmall");
        SceneManager.LoadScene(0);
    }

    public void CashButtonEvent()
    {
        if (scoreNum > pbNum){
            if (cashHaloBig.activeInHierarchy == false)
            {
                cashHalo.SetActive(false);
                cashHaloBig.SetActive(true);
            }
            else
            {
                cashHalo.SetActive(true);
                cashHaloBig.SetActive(false);
            }
        }
    }

    public void CashButtonClick()
    {
        if (scoreNum > pbNum)
        {
            if (CashInText.color == new Color32(255, 223, 0, 255))
            {
                CashInText.color = new Color32(255, 223, 0, 200);
            }
            else
            {
                CashInText.color = new Color32(255, 223, 0, 255);
            }

        }
    }


    void LRButtonClick(GameObject chosenButton, int correctNum, Text side, string chosenSide, int chosenNum)
    {
        FindObjectOfType<AudioManager>().Play("ButtonBig");
        StartCoroutine(Scale(chosenButton));
        if (num == correctNum)
        {
            Success();
            RandUpdate();
            if (scoreNum * 50 <= 250)
            {
                var ps = backgroundLights.emission;
                ps.rateOverTime = scoreNum * 50;
            }
            IncrementSide(side, chosenSide, chosenNum);
            if (scoreNum > pbNum)
            {
                cashHalo.SetActive(true);
                CashInText.color = new Color32 (255,223,0,255);
                cashButton.interactable = true;
            }
            else
            {
                CashInText.color = new Color32 (255, 223, 0, 127);
                cashButton.interactable = false;
            }
        }
        else
        {
            Fail();
        }
    }

    void IncrementSide(Text side, string chosenSide, int chosenNum)
    {
        chosenNum = chosenNum + 1;
        side.text = chosenSide + chosenNum;
    }

    void RandUpdate()
    {
        num = Random.Range(0, 2);
    }

    void Success()
    {
        scoreNum = scoreNum + 1;
        Score.text = "SCORE: " + scoreNum;
        oddNum = oddNum * 2;
        Odds.text = "ODDS: 1/" + oddNum;
        RandUpdate();
    }

    void Fail()
    {
        FindObjectOfType<AudioManager>().Play("Broke");
        StartCoroutine(BrokeTransition(Broke));
    }

    private IEnumerator time()
    {
        while (true)
        {
            timeCount();
            yield return new WaitForSeconds(1);
        }
    }
    void timeCount()
    {
        Timer.text = "TIME: " + currentTime + " s";
        currentTime += 1;
    }

    public void CashIn()
    {
        FindObjectOfType<AudioManager>().Play("ButtonSmall");
        if (scoreNum > pbNum)
        {
            pbNum = scoreNum;
            Pb.text = "PB: " + pbNum;
            FindObjectOfType<AudioManager>().Play("CashIn");
            StartCoroutine(CashTransition(Cash));
        }
    }

    IEnumerator Scale(GameObject chosenButton)
    {
        Vector3 InitialScale = new Vector3(10, 10, 10);
        chosenButton.transform.localScale = InitialScale;
        clickAllow = false;
        float progress = 0;
        Vector3 FinalScale = new Vector3(10, 10, 5);
        while (progress < 1)
        {
            chosenButton.transform.localScale = Vector3.Lerp(InitialScale, FinalScale, progress);
            progress += Time.deltaTime * 15f;
            yield return null;
        }
        progress = 0;
        while (progress < 1)
        {
            chosenButton.transform.localScale = Vector3.Lerp(FinalScale, InitialScale, progress);
            progress += Time.deltaTime * 15f;
            yield return null;
        }
        clickAllow = true;
    }

    IEnumerator BrokeTransition(Text eventAnimation)
    {
        gameCanvas.gameObject.SetActive(false);
        brokeCanvas.gameObject.SetActive(true);
        clickAllow2 = false;
        Vector3 InitialPosition = new Vector3(0, 500, 0);
        eventAnimation.GetComponent<RectTransform>().localPosition = InitialPosition;
        float progress = 0;
        Vector3 MidPosition = new Vector3(0, 0, 0);
        Vector3 FinalPosition = new Vector3(0, -500, 0);
        while (progress < 1)
        {
            eventAnimation.GetComponent<RectTransform>().localPosition = Vector3.Lerp(InitialPosition, MidPosition, progress);
            progress += Time.deltaTime * 5f;
            yield return null;
        }
        progress = 0;
        yield return new WaitForSecondsRealtime(0.4f);
        while (progress < 1)
        {
            eventAnimation.GetComponent<RectTransform>().localPosition = Vector3.Lerp(MidPosition, FinalPosition, progress);
            progress += Time.deltaTime * 5f;
            yield return null;
        }
        eventAnimation.GetComponent<RectTransform>().localPosition = InitialPosition;
        brokeCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        clickAllow2 = true;
        Restart();
    }

    IEnumerator CashTransition(Text eventAnimation)
    {
        gameCanvas.gameObject.SetActive(false);
        cashCanvas.gameObject.SetActive(true);
        clickAllow2 = false;
        Vector3 InitialPosition = new Vector3(0, 500, 0);
        eventAnimation.GetComponent<RectTransform>().localPosition = InitialPosition;
        float progress = 0;
        Vector3 MidPosition = new Vector3(0, 0, 0);
        Vector3 FinalPosition = new Vector3(0, -500, 0);
        while (progress < 1)
        {
            eventAnimation.GetComponent<RectTransform>().localPosition = Vector3.Lerp(InitialPosition, MidPosition, progress);
            progress += Time.deltaTime * 5f;
            yield return null;
        }
        progress = 0;
        yield return new WaitForSecondsRealtime(0.4f);
        while (progress < 1)
        {
            eventAnimation.GetComponent<RectTransform>().localPosition = Vector3.Lerp(MidPosition, FinalPosition, progress);
            progress += Time.deltaTime * 5f;
            yield return null;
        }
        eventAnimation.GetComponent<RectTransform>().localPosition = InitialPosition;
        cashCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        clickAllow2 = true;
        Restart();
    }
}
