using UnityEngine;

// A single line of dialogue - used by the Stranger, the intro monologue,
// and any future dialogue (photos, tape recorder, mirror room, etc).
[System.Serializable]
public struct DialogueLine
{
    public string speaker;

    [TextArea(2, 4)]
    public string text;
}