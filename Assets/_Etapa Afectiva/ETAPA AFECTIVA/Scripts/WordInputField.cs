using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class WordInputField : MonoBehaviour
{
    [Header("ConfiguraciÛn")]
    public string palabraCorrecta;

    [Header("Sonidos")]
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;

    [Header("Referencias")]
    public HojaController hojaController;

    private TMP_InputField inputField;
    private AudioSource audioSource;
    private Image underlineImage;
    private bool estaCorrect = false;
    private bool sonidoIncorrectoReproducido = false;

    private Color colorNeutral = Color.white;
    private Color colorCorrecto = Color.green;
    private Color colorIncorrecto = Color.red;

    void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        inputField.interactable = true;

        Transform bg = transform.Find("Background");
        if (bg != null)
        {
            underlineImage = bg.GetComponent<Image>();
            RectTransform rt = underlineImage.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 0);
            rt.pivot = new Vector2(0.5f, 0);
            rt.sizeDelta = new Vector2(0, 2);
            rt.anchoredPosition = new Vector2(0, 0);
            underlineImage.color = colorNeutral;
        }

        inputField.onValueChanged.AddListener(ValidarPalabra);
    }

    string QuitarAcentos(string texto)
    {
        string con = "·ÈÌÛ˙¡…Õ”⁄‡ËÏÚ˘¿»Ã“Ÿ";
        string sin = "aeiouAEIOUaeiouAEIOU";
        StringBuilder sb = new StringBuilder(texto);
        for (int i = 0; i < con.Length; i++)
            sb.Replace(con[i], sin[i]);
        return sb.ToString();
    }

    void ValidarPalabra(string valorActual)
    {
        if (string.IsNullOrEmpty(valorActual))
        {
            SetColor(colorNeutral);
            estaCorrect = false;
            sonidoIncorrectoReproducido = false;
            hojaController.ActualizarEstado();
            return;
        }

        string escrita = QuitarAcentos(valorActual.Trim().ToLower());
        string esperada = QuitarAcentos(palabraCorrecta.Trim().ToLower());

        if (escrita.Length < esperada.Length)
        {
            SetColor(colorNeutral);
            estaCorrect = false;
            sonidoIncorrectoReproducido = false;
            hojaController.ActualizarEstado();
            return;
        }

        bool correcto = escrita == esperada;

        if (correcto)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            if (!estaCorrect)
                PlaySound(sonidoCorrecto);

            estaCorrect = true;
            sonidoIncorrectoReproducido = false;
            StartCoroutine(FlashVerde());
            inputField.interactable = false;
        }
        else
        {
            SetColor(colorIncorrecto);
            if (!sonidoIncorrectoReproducido)
            {
                PlaySound(sonidoIncorrecto);
                sonidoIncorrectoReproducido = true;
            }
            estaCorrect = false;
        }

        hojaController.ActualizarEstado();
    }

    IEnumerator FlashVerde()
    {
        float duracion = 1.5f;
        float tiempo = 0f;

        Color verdeClaro = new Color(0.4f, 3f, 0.4f); // HDR, valor > 1 activa el bloom
        Color blanco = Color.white;

        // Fade de blanco a verde brillante
        while (tiempo < duracion / 2)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / (duracion / 2);
            SetColor(Color.Lerp(blanco, verdeClaro, t));
            yield return null;
        }

        // Fade de verde brillante a blanco
        tiempo = 0f;
        while (tiempo < duracion / 2)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / (duracion / 2);
            SetColor(Color.Lerp(verdeClaro, blanco, t));
            yield return null;
        }

        if (estaCorrect)
            SetColor(colorNeutral);
    }

    void SetColor(Color color)
    {
        if (underlineImage != null)
            underlineImage.color = color;
        if (inputField.textComponent != null)
            inputField.textComponent.color = color == colorNeutral ? Color.white : color;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    public bool EstaCompleto() => estaCorrect;
}