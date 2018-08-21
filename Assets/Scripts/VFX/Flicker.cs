using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Flicker : MonoBehaviour {

    public float duration = 0.1f;
    public float timeBetweenFlickering = 0.1f;
    public Color defaultFlickerColor = new Color(1f, 100/255f, 100/255f, 1f);

    private SpriteRenderer m_SpriteRenderer;
    private Color m_OriginalColor;
    private Coroutine m_Coroutine;


    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_OriginalColor = m_SpriteRenderer.color;

    }

    private void OnEnable()
    {
        StopFlickering();
    }


    public void StartFlickering()
    {
        InternalFlickering(duration, timeBetweenFlickering);
    }

    public void StartDefaultColorFlicker()
    {
        InternalFlickering(duration, timeBetweenFlickering, defaultFlickerColor);
    }

    public Coroutine StartFlickering(float duration, float timeBetweenFlickering)
    {
        return InternalFlickering(duration, timeBetweenFlickering);
    }


    /// <summary>
    /// Color flickering, default value is (255,100,100,255).
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="timeBetweenFlickering"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public Coroutine StartColorFickering(float duration, float timeBetweenFlickering, Color? color = null)
    {     
        return InternalFlickering(duration, timeBetweenFlickering, color??defaultFlickerColor);
    }


    public void StopFlickering()
    {
        m_SpriteRenderer.color = m_OriginalColor;

        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
        }
    }

    private Coroutine InternalFlickering(float duration, float timeBetweenFlickering, Color? color = null)
    {
        this.duration = duration;
        this.timeBetweenFlickering = timeBetweenFlickering;

        //stop previous corountine
        StopFlickering();


        //start new coroutine
        m_Coroutine = StartCoroutine(Flickering(color));

        return m_Coroutine;
    }

    private IEnumerator Flickering(Color? color = null)
    {

        float timer = 0f;
        float sinceLastChange = 0.0f;

        int state = 1;

        Color flickerColor;


        if (!color.HasValue)
        {
            Color transparent = m_OriginalColor;
            transparent.a = 0.2f;
            flickerColor = transparent;
            m_SpriteRenderer.color = transparent;
        }
        else
        {
            flickerColor = color.Value;
            m_SpriteRenderer.color = color.Value;
        }


        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
            sinceLastChange += Time.deltaTime;
            if (sinceLastChange > timeBetweenFlickering)
            {
                sinceLastChange -= timeBetweenFlickering;
                state = 1 - state;
                m_SpriteRenderer.color = state == 1 ? flickerColor : m_OriginalColor;
            }
        }

        m_SpriteRenderer.color = m_OriginalColor;
    }



}
