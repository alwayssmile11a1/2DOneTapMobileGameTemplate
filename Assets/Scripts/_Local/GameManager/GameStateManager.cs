using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// The Game manager is a state machine, that will switch between state according to current gamestate.
/// </summary>
public class GameStateManager : MonoBehaviour
{

    #region Static
    public static GameStateManager Instance
    {
        get
        {
            if (m_Instance != null) return m_Instance;

            m_Instance = FindObjectOfType<GameStateManager>();

            if (m_Instance != null) return m_Instance;

            //create new
            GameObject gameObject = new GameObject("GameManager");
            gameObject = Instantiate(gameObject);
            m_Instance = gameObject.AddComponent<GameStateManager>();

            return m_Instance;
        }

    }
    protected static GameStateManager m_Instance;
    #endregion

    public AState[] states;
    public AState TopState { get { if (m_StateStack.Count == 0) return null; return m_StateStack[m_StateStack.Count - 1]; } }

    protected List<AState> m_StateStack = new List<AState>();
    protected Dictionary<string, AState> m_StateDict = new Dictionary<string, AState>();


    protected void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < states.Length; ++i)
        {
            states[i].gameManager = this;
            m_StateDict.Add(states[i].GetName(), states[i]);
        }

    }

    protected void Start()
    {
        PushState(states[0].GetName());
    }

    protected void Update()
    {

        if (m_StateStack.Count > 0)
        {
            m_StateStack[m_StateStack.Count - 1].Tick();
        }

    }


    public void SwitchState(string newState)
    {
        AState state = FindState(newState);
        if (state == null)
        {
            Debug.LogError("Can't find the state named " + newState);
            return;
        }

        m_StateStack[m_StateStack.Count - 1].Exit(state);
        state.Enter(m_StateStack[m_StateStack.Count - 1]);
        m_StateStack.RemoveAt(m_StateStack.Count - 1);
        m_StateStack.Add(state);
    }

    public AState FindState(string stateName)
    {
        AState state;
        if (!m_StateDict.TryGetValue(stateName, out state))
        {
            return null;
        }

        return state;
    }

    public void PopState()
    {
        if (m_StateStack.Count < 2)
        {
            Debug.LogError("Can't pop states, only one in stack.");
            return;
        }

        m_StateStack[m_StateStack.Count - 1].Exit(m_StateStack[m_StateStack.Count - 2]);
        m_StateStack[m_StateStack.Count - 2].Enter(m_StateStack[m_StateStack.Count - 2]);
        m_StateStack.RemoveAt(m_StateStack.Count - 1);
    }

    public void PushState(string name)
    {
        AState state;
        if (!m_StateDict.TryGetValue(name, out state))
        {
            Debug.LogError("Can't find the state named " + name);
            return;
        }

        if (m_StateStack.Count > 0)
        {
            m_StateStack[m_StateStack.Count - 1].Exit(state);
            state.Enter(m_StateStack[m_StateStack.Count - 1]);
        }
        else
        {
            state.Enter(null);
        }
        m_StateStack.Add(state);
    }

}

/// <summary>
/// Represent a state in game
/// </summary>
public abstract class AState : MonoBehaviour
{
    [HideInInspector]
    public GameStateManager gameManager;

    public abstract void Enter(AState from);
    public abstract void Exit(AState to);
    public abstract void Tick();

    public abstract string GetName();
}
