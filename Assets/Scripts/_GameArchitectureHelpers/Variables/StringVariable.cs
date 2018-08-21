using UnityEngine;


[CreateAssetMenu(menuName = "Variables/StringVariable", order = 2)]
public class StringVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    [SerializeField]
    private string value = "";

    public string Value
    {
        get { return value; }
        set { this.value = value; }
    }

    public static implicit operator string(StringVariable stringVariable)
    {
        return stringVariable.Value;
    }


}
