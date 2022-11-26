using Interfaces;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IUIManagerInterface
{
    private static IUIManagerInterface instance;
    [SerializeField]
    private Image m_blackscreen;
    [SerializeField]
    private TextMeshProUGUI m_victory_text;
    [SerializeField]
    private TextMeshProUGUI m_objective_text;

    private void Awake()
    {
        instance = this;
    }

    public static IUIManagerInterface GetInstance()
    {
        return instance;
    }

    public void ShowBlackScreen()
    {
        IEnumerator ShowBlackScreen()
        {
            float transition_duration = 2f;
            m_blackscreen.raycastTarget = true;
            for (float i = 0; i <= transition_duration; i += Time.deltaTime)
            {
                m_blackscreen.color = new Color(0, 0, 0, i / transition_duration);
                yield return new WaitForEndOfFrame();
            }
        }
        StartCoroutine(ShowBlackScreen());
    }

    public void HideBlackScreen()
    {
        IEnumerator HideBlackScreen()
        {
            float transition_duration = 2f;
            m_blackscreen.raycastTarget = true;
            for (float i = transition_duration; i >= 0; i -= Time.deltaTime)
            {
                m_blackscreen.color = new Color(0, 0, 0, i / transition_duration);
                yield return new WaitForEndOfFrame();
            }
        }
        StartCoroutine(HideBlackScreen());
    }

    public void ShowVictoryText()
    {
        m_victory_text.gameObject.SetActive(true);
    }

    public void HideObjectiveText()
    {
        m_objective_text.gameObject.SetActive(false);
    }
}
