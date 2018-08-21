using UnityEngine;

public class TimeManager : MonoBehaviour {


    private static TimeManager m_TimeManager = null;

    private float m_Amount;
    private float m_Duration;
    private float m_Timer;
    private bool m_GraduallyIncreaseTimeBackToNormal;

    private float m_OriginalFixedDeltaTime;

    private void Awake()
    {
        m_OriginalFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void OnEnable()
    {
        m_TimeManager = this;
    }

    private void Update()
    {
        if (m_Timer > 0)
        {
            m_Timer -= Time.unscaledDeltaTime;

            if (m_GraduallyIncreaseTimeBackToNormal)
            {
                GraduallyIncreaseTimeToNormal();
            }

            if (m_Timer <= 0)
            {
                ChangeTimeBackToNormal();
            }

        }


    }

    private void ChangeTimeBackToNormal()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = m_OriginalFixedDeltaTime;
    }

    private void GraduallyIncreaseTimeToNormal()
    {     
        Time.timeScale += (1 - m_Amount) / (m_Duration / Time.unscaledDeltaTime);
    }

    /// <summary>
    /// Slowdown time of entire system
    /// </summary>
    /// <param name="amount">0 to 1</param>
    /// <param name="duration"></param>
    public static void SlowdownTime(float amount, float duration, bool graduallyIncreaseTimeBackToNormal = true)
    {
        //Change time back to normal first
        m_TimeManager.ChangeTimeBackToNormal();

        //Variables
        m_TimeManager.m_GraduallyIncreaseTimeBackToNormal = graduallyIncreaseTimeBackToNormal;
        m_TimeManager.m_Timer = duration;
        m_TimeManager.m_Amount = Mathf.Clamp(amount, 0, 1);
        m_TimeManager.m_Duration = duration;

        //Slowdown
        Time.timeScale = amount;
        Time.fixedDeltaTime = Time.timeScale * m_TimeManager.m_OriginalFixedDeltaTime;

    }






}
