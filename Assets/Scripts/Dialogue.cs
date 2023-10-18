using UnityEngine;

/// <summary>
/// Dialgue datat structure
/// </summary>
[System.Serializable]
public class Dialogue
{
    public string characterName;
    [TextArea(3, 10)]
    public string[] sentences;
}
