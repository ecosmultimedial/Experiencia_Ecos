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
    public TextMeshProUGUI questionText;
    public TMP_InputField inputField;
    public GameObject continueButton;

    [Header("Preguntas")]
    public string[] questions;
    private int currentIndex = 0;
    private string[] answers;

    [Header("Final")]
    public AudioSource finalAudio;
    public Animator infinityAnimator;

    [Header("Flash")]
    public Image flashImage;
    public float flashSpeed = 3f;

    [Header("Player")]
    public MonoBehaviour playerController;
    public Transform playerTransform;   // ← player
    public Transform focusPoint;        // ← empty donde se posiciona

    private bool writing = false;

    void Start()
    {
        answers = new string[questions.Length];
        questionPanel.SetActive(false);

        finalAudio.Stop();

        // Flash invisible al inicio
        if (flashImage != null)
        {
            Color c = flashImage.color;
            c.a = 0f;
            flashImage.color = c;
        }

        inputField.onValueChanged.AddListener(CheckInput);
        inputField.onSelect.AddListener(StartWriting);
    }

    // 🚀 CUANDO HACÉS CLICK EN EL INPUT
    void StartWriting(string text)
    {
        writing = true;

        // TELETRANSPORTA al punto
        playerTransform.position = focusPoint.position;
        playerTransform.rotation = focusPoint.rotation;

        // BLOQUEA controles
        playerController.enabled = false;

        // ACTIVA input correctamente
        inputField.ActivateInputField();
    }

    // 🚀 ABRIR PANEL
    public void OpenQuestions()
    {
        questionPanel.SetActive(true);
        ShowQuestion();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Podés moverte hasta hacer click
        playerController.enabled = true;
        writing = false;
    }

    void ShowQuestion()
    {
        questionText.text = questions[currentIndex];
        inputField.text = "";

        continueButton.SetActive(false);

        writing = false;

        // IMPORTANTE: no auto focus
        EventSystem.current.SetSelectedGameObject(null);
    }

    void CheckInput(string text)
    {
        continueButton.SetActive(text.Trim().Length > 0);
    }

    void Update()
    {
        // ENTER para avanzar (SIEMPRE bloqueado)
        if (questionPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
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

            // 🔥 CLAVE: volver a enfocar automáticamente
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
        questionPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController.enabled = true;

        StartCoroutine(FinalSequence());
    }

    IEnumerator FinalSequence()
    {
        finalAudio.Play(); // 🎧 audio

        yield return new WaitForSeconds(1f);

        infinityAnimator.SetTrigger("Activate"); // ✨ animación

        yield return new WaitForSeconds(4f);

        yield return StartCoroutine(FlashEffect()); // 💥 flash

        SceneManager.LoadScene("etapa central");
    }

    IEnumerator FlashEffect()
    {
        // Fade in
        while (flashImage.color.a < 1f)
        {
            Color c = flashImage.color;
            c.a += Time.deltaTime * flashSpeed;
            flashImage.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(0.6f);

        // Fade out
        while (flashImage.color.a > 0f)
        {
            Color c = flashImage.color;
            c.a -= Time.deltaTime * flashSpeed;
            flashImage.color = c;
            yield return null;
        }
    }

    public string[] GetAnswers()
    {
        return answers;
    }
}