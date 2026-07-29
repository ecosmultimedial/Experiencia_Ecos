using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Question3DManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject questionPanel;
    public CanvasGroup questionCanvasGroup;
    public TextMeshProUGUI questionText;
    public TMP_InputField inputField;
    public GameObject continueButton;

    [Header("Fade")]
    public float fadeDuration = 1f;

    [Header("Preguntas")]
    public string[] questions;
    private int currentIndex = 0;
    private string[] answers;

    [Header("Final")]
    public AudioSource finalAudio;
    public AudioSource musicaDeFondo;    // ← NUEVA LÍNEA
    public Animator infinityAnimator;

    [Header("Flash")]
    public Image flashImage;
    public float flashSpeed = 3f;

    [Header("Player")]
    public MonoBehaviour playerController;
    public Transform playerTransform;
    public Transform focusPoint;

    private bool writing = false;

    void Start()
    {
        answers = new string[questions.Length];

        questionPanel.SetActive(true);
        questionCanvasGroup.alpha = 0f;
        questionCanvasGroup.interactable = false;
        questionCanvasGroup.blocksRaycasts = false;

        finalAudio.Stop();

        if (flashImage != null)
        {
            Color c = flashImage.color;
            c.a = 0f;
            flashImage.color = c;
        }

        inputField.onValueChanged.AddListener(CheckInput);
        inputField.onSelect.AddListener(StartWriting);
    }

    void StartWriting(string text)
    {
        writing = true;

        playerTransform.position = focusPoint.position;
        playerTransform.rotation = focusPoint.rotation;

        playerController.enabled = false;

        inputField.ActivateInputField();
    }

    public void OpenQuestions()
    {
        ShowQuestion();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerController.enabled = true;
        writing = false;

        StartCoroutine(FadeCanvas(questionCanvasGroup, 0f, 1f, fadeDuration, true));
    }

    void ShowQuestion()
    {
        questionText.text = questions[currentIndex];
        inputField.text = "";

        continueButton.SetActive(false);

        writing = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    void CheckInput(string text)
    {
        continueButton.SetActive(text.Trim().Length > 0);
    }

    void Update()
    {
        if (questionCanvasGroup.alpha > 0.9f && Input.GetKeyDown(KeyCode.Return))
        {
            if (inputField.text.Trim().Length > 0 && writing)
            {
                NextQuestion();
            }
        }
    }

    public void NextQuestion()
    {
        if (inputField.text.Trim().Length == 0)
            return;

        answers[currentIndex] = inputField.text;

        currentIndex++;

        if (currentIndex < questions.Length)
        {
            ShowQuestion();
            StartWriting("");
        }
        else
        {
            FinishQuestions();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenQuestions();
        }
    }

    void FinishQuestions()
    {
        // Guardar respuestas en PlayerPrefs
        for (int i = 0; i < answers.Length; i++)
        {
            PlayerPrefs.SetString("Respuesta_" + i, answers[i]);
        }
        PlayerPrefs.Save();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController.enabled = true;

        StartCoroutine(FadeOutAndContinue());
    }

    IEnumerator FadeOutAndContinue()
    {
        yield return StartCoroutine(FadeCanvas(questionCanvasGroup, 1f, 0f, fadeDuration, false));
        yield return StartCoroutine(FinalSequence());
    }

    IEnumerator FinalSequence()
    {
        finalAudio.Play();

        yield return new WaitForSeconds(1f);

        infinityAnimator.SetTrigger("Activate");

        yield return new WaitForSeconds(4f);

        // Hace flash blanco rápido
        yield return StartCoroutine(QuickFlash());

        // Fade suave de 4 segundos (audio + pantalla)
        yield return StartCoroutine(SmoothTransitionToNextScene());
    }

    IEnumerator QuickFlash()
    {
        float elapsed = 0f;
        float flashDuration = 1f; // Flash rápido

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / flashDuration;

            Color c = flashImage.color;
            c.a = Mathf.Lerp(0f, 1f, progress);
            flashImage.color = c;

            yield return null;
        }

        // Asegurar que está totalmente blanco
        Color finalColor = flashImage.color;
        finalColor.a = 1f;
        flashImage.color = finalColor;
    }

    IEnumerator SmoothTransitionToNextScene()
    {
        float elapsed = 0f;
        float transitionDuration = 4f;

        float initialVolume = musicaDeFondo.volume;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / transitionDuration;

            // Audio baja linealmente
            musicaDeFondo.volume = Mathf.Lerp(initialVolume, 0f, progress);

            yield return null;
        }

        // Finalizar
        musicaDeFondo.volume = 0f;

        // Cargar escena
        SceneManager.LoadScene("etapa central");
    }

    IEnumerator FadeCanvas(CanvasGroup cg, float from, float to, float duration, bool enableAtEnd)
    {
        float elapsed = 0f;
        cg.alpha = from;

        if (enableAtEnd)
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        cg.alpha = to;

        if (!enableAtEnd)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public string[] GetAnswers()
    {
        return answers;
    }
}