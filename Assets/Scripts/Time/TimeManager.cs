using UnityEngine;

public class TimeManager : MonoBehaviour {


    #region Singleton
    public static TimeManager Instance
    {
        get
        {
            if (m_Instance != null) return m_Instance;

            m_Instance = FindObjectOfType<TimeManager>();

            if (m_Instance == null)
            {
                GameObject obj = new GameObject("TimeManager");

                m_Instance = obj.AddComponent<TimeManager>();
            }

            return m_Instance;
        }

    }

    private static TimeManager m_Instance;

    #endregion

    private float m_Amount;
    private float m_Duration;
    private float m_Timer;
    private bool m_GraduallyIncreaseTimeBackToNormal;

    private float m_OriginalFixedDeltaTime;

    private void Awake()
    {
        m_OriginalFixedDeltaTime = Time.fixedDeltaTime;
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

    public void ChangeTimeBackToNormal()
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
    public void SlowdownTime(float amount, float duration, bool graduallyIncreaseTimeBackToNormal = true)
    {
        //Change time back to normal first
        ChangeTimeBackToNormal();

        //Variables
        m_GraduallyIncreaseTimeBackToNormal = graduallyIncreaseTimeBackToNormal;
        m_Timer = duration;
        m_Amount = Mathf.Clamp(amount, 0, 1);
        m_Duration = duration;

        //Slowdown
        Time.timeScale = amount;
        Time.fixedDeltaTime = Time.timeScale * m_OriginalFixedDeltaTime;

    }






}
