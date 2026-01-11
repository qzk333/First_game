using System.Collections;
using UnityEngine;
using TMPro;

// Listens for Boss1 (Yu Jin) defeat, spawns an NPC messenger, and shows a choice/endings flow.
public class Boss1DefeatSequence : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private GameObject messengerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnBehindDistance = 2.5f; // distance behind player
    [SerializeField] private float delayBeforeSequence = 2f; // delay after boss death
    [SerializeField] private float delayAfterSpawn = 1.5f; // delay before showing choice panel

    [Header("Dialog & Choice UI")]
    [SerializeField] private GameObject choicePanel; // Panel with Yes/No buttons
    [SerializeField] private TextMeshProUGUI dialogText;
    [TextArea]
    [SerializeField] private string dialogLine = "将军不好了，荆州城被吕蒙偷袭了！是否回防荆州？";

    [Header("Ending UI")]
    [SerializeField] private GameObject endingPanel;
    [SerializeField] private TextMeshProUGUI endingText;
    [SerializeField] private TextMeshProUGUI continueHintText;
    [SerializeField] private float typingSpeed = 0.05f;
    [TextArea] [SerializeField] private string endingYesText = "你率军回防荆州，挫败了吕蒙的偷袭，百姓欢呼，江东震动。";
    [TextArea] [SerializeField] private string endingNoText = "你选择继续追击，荆州陷落，百姓涂炭，史书将记载这段遗憾。";
    [Header("Final Panels")]
    [SerializeField] private GameObject endingYesPanel;
    [SerializeField] private GameObject endingNoPanel;

    private GameObject spawnedMessenger;
    private bool waitingForContinue;
    private bool endingWasYes;
    private Coroutine typingRoutine;

    private void OnEnable()
    {
        Enemy_Boss.OnBoss1Defeated += HandleBossDefeated;
    }

    private void OnDisable()
    {
        Enemy_Boss.OnBoss1Defeated -= HandleBossDefeated;
    }

    private void HandleBossDefeated()
    {
        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        if (delayBeforeSequence > 0)
            yield return new WaitForSeconds(delayBeforeSequence);

        // Spawn messenger
        if (spawnedMessenger == null && messengerPrefab != null)
        {
            Vector3 pos = transform.position;

            Transform playerTf = PlayerManager.instance != null ? PlayerManager.instance.player?.transform : null;
            if (playerTf != null)
            {
                int playerFacing = 1;
                var p = playerTf.GetComponent<Player>();
                if (p != null)
                    playerFacing = p.facingDir;
                else if (playerTf.localScale.x < 0)
                    playerFacing = -1;

                pos = playerTf.position - new Vector3(playerFacing * spawnBehindDistance, 0, 0);
            }
            else if (spawnPoint != null)
            {
                pos = spawnPoint.position;
            }

            spawnedMessenger = Instantiate(messengerPrefab, pos, Quaternion.identity);
        }

        if (delayAfterSpawn > 0)
            yield return new WaitForSeconds(delayAfterSpawn);

        // Show dialog line
        if (dialogText != null)
            dialogText.text = dialogLine;

        if (choicePanel != null)
            choicePanel.SetActive(true);

        yield return null;
    }

    public void OnChooseYes()
    {
        ShowEnding(true);
    }

    public void OnChooseNo()
    {
        ShowEnding(false);
    }

    private void ShowEnding(bool isYes)
    {
        if (choicePanel != null)
            choicePanel.SetActive(false);

        if (endingPanel != null)
            endingPanel.SetActive(true);

        endingWasYes = isYes;
        string text = isYes ? endingYesText : endingNoText;
        StartTyping(text);
    }

    private void StartTyping(string text)
    {
        waitingForContinue = false;
        if (continueHintText != null)
            continueHintText.gameObject.SetActive(false);

        if (typingRoutine != null)
            StopCoroutine(typingRoutine);
        typingRoutine = StartCoroutine(TypeText(text));
    }

    private IEnumerator TypeText(string text)
    {
        if (endingText == null)
            yield break;

        endingText.text = "";
        foreach (char c in text)
        {
            endingText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        waitingForContinue = true;
        if (continueHintText != null)
            continueHintText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!waitingForContinue)
            return;

        if (Input.anyKeyDown)
        {
            waitingForContinue = false;
            if (endingPanel != null)
                endingPanel.SetActive(false);

            if (endingYesPanel != null)
                endingYesPanel.SetActive(endingWasYes);
            if (endingNoPanel != null)
                endingNoPanel.SetActive(!endingWasYes);
        }
    }
}
