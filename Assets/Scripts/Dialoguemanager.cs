using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Put this on a manager GameObject in your scene (e.g. an empty "DialogueManager" object).
// It controls the dialogue UI panel you already built.
public class DialogueManager : MonoBehaviour
{
    // Simple singleton so any script (StrangerInteraction, puzzle objects, etc.)
    // can call DialogueManager.Instance.StartDialogue(...) without a manual reference.
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    [Tooltip("The parent panel/GameObject that gets enabled/disabled.")]
    public GameObject dialoguePanel;

    [Tooltip("Text field that shows who's speaking.")]
    public Text speakerText;

    [Tooltip("Text field that shows the actual line, typed out letter by letter.")]
    public Text dialogueText;

    [Header("Settings")]
    [Tooltip("Seconds between each letter appearing.")]
    public float textSpeed = 0.03f;

    [Tooltip("Seconds to pause on a fully-typed line before auto-advancing to the next one.")]
    public float autoAdvanceDelay = 1.5f;

    [Tooltip("Key to instantly finish typing the current line (skip the letter-by-letter effect). Lines still auto-advance either way.")]
    public KeyCode skipKey = KeyCode.E;

    private Coroutine lineRoutine;
    private bool isTyping;
    private bool skipRequested;

    private DialogueLine[] currentLines;
    private int currentLineIndex;
    private System.Action onDialogueComplete;

    // Lets other scripts (like PlayerInteractor) check if dialogue is currently
    // open, so they can avoid re-triggering interactions while it's active.
    public bool IsDialogueOpen => dialoguePanel != null && dialoguePanel.activeSelf;

    void Awake()
    {
        Instance = this;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!IsDialogueOpen) return;

        // Pressing the key while a line is still typing jumps straight to the
        // full line - it does NOT advance to the next line, since that
        // happens automatically after autoAdvanceDelay.
        if (isTyping && Input.GetKeyDown(skipKey))
        {
            skipRequested = true;
        }
    }

    // Call this from anywhere, e.g. DialogueManager.Instance.StartDialogue(lines);
    // onComplete is optional - pass something here if you need to run code
    // right when the dialogue box closes (e.g. re-enable player movement).
    public void StartDialogue(DialogueLine[] lines, System.Action onComplete = null)
    {
        if (lines == null || lines.Length == 0) return;

        currentLines = lines;
        currentLineIndex = 0;
        onDialogueComplete = onComplete;

        dialoguePanel.SetActive(true);
        ShowLine(currentLines[currentLineIndex]);
    }

    void ShowLine(DialogueLine line)
    {
        if (lineRoutine != null)
        {
            StopCoroutine(lineRoutine);
        }
        lineRoutine = StartCoroutine(PlayLineRoutine(line));
    }

    // Types the line out letter by letter, then automatically waits
    // autoAdvanceDelay seconds and moves on (or closes the dialogue box
    // if this was the last line).
    IEnumerator PlayLineRoutine(DialogueLine line)
    {
        if (speakerText != null)
        {
            speakerText.text = line.speaker;
        }

        dialogueText.text = "";
        isTyping = true;
        skipRequested = false;

        foreach (char letter in line.text)
        {
            if (skipRequested)
            {
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        dialogueText.text = line.text; // make sure the full line is showing
        isTyping = false;

        yield return new WaitForSeconds(autoAdvanceDelay);

        AdvanceToNextLine();
    }

    void AdvanceToNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= currentLines.Length)
        {
            EndDialogue();
        }
        else
        {
            ShowLine(currentLines[currentLineIndex]);
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentLines = null;
        currentLineIndex = 0;

        System.Action callback = onDialogueComplete;
        onDialogueComplete = null;
        callback?.Invoke();
    }
}