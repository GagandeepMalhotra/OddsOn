using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject LHalo;
    public GameObject RHalo;
    public GameObject LHaloBig;
    public GameObject RHaloBig;

    public Canvas menuCanvas;
    public Canvas menuCanvas2;
    public Canvas playCanvas;
    public Canvas ruleCanvas;
    public Canvas ruleTextCanvas;

    public Text playCanvasText;
    public Text ruleCanvasText;
    public Text menuCanvasText;

    void Start()
    {
        Time.timeScale = 1;
    }

    public void Update()
    {
        RHalo.SetActive(true);
        LHalo.SetActive(true);
        RHaloBig.SetActive(false);
        LHaloBig.SetActive(false);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
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

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "leftButton")
                {
                    FindObjectOfType<AudioManager>().Play("ButtonBig");
                    FindObjectOfType<AudioManager>().Play("Swipe");
                    StartCoroutine(Scale(leftButton));
                    StartCoroutine(playTransition(playCanvasText));
                }

                else if (hit.transform.name == "rightButton")
                {
                    FindObjectOfType<AudioManager>().Play("ButtonBig");
                    FindObjectOfType<AudioManager>().Play("Swipe");
                    StartCoroutine(Scale(rightButton));
                    StartCoroutine(rulesTransition(ruleCanvasText));
                }
            }
        }
    }

    public void Back()
    {
        FindObjectOfType<AudioManager>().Play("ButtonSmall");
        FindObjectOfType<AudioManager>().Play("Swipe");
        StartCoroutine(menuTransition(menuCanvasText));
    }

    IEnumerator Scale(GameObject chosenButton)
    {
        Vector3 InitialScale = new Vector3(10, 10, 10);
        chosenButton.transform.localScale = InitialScale;
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
    }

    IEnumerator rulesTransition(Text eventAnimation)
    {
        menuCanvas.gameObject.SetActive(false);
        ruleCanvas.gameObject.SetActive(true);
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
        ruleCanvas.gameObject.SetActive(false);
        ruleTextCanvas.gameObject.SetActive(true);
    }

    IEnumerator playTransition(Text eventAnimation)
    {
        menuCanvas.gameObject.SetActive(false);
        playCanvas.gameObject.SetActive(true);
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
        playCanvas.gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }

    IEnumerator menuTransition(Text eventAnimation)
    {
        ruleTextCanvas.gameObject.SetActive(false);
        menuCanvas2.gameObject.SetActive(true);
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
        menuCanvas2.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(true);
    }
}
