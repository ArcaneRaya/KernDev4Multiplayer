using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DrawMode { avg, real, full, backToBack, inverseBackToBack }

public class VoiceLineManager : MonoBehaviour
{
    public DrawMode displayMode;
    public float amplitude;
    public int fps;
    public int barCount;
    public AudioEvent audioEvent;
    private AudioSource source;
    private float nextActionTime = 0.0f;
    private float period = 0.05f;
    private float[] spectrumData = new float[128];
    private float[] fullSpectrumData = new float[256];
    private GameObject[] spectrumBars = new GameObject[32];
    private bool active = false;

    public MessagePairVariable messagePair;
    public GameEvent onMessageDonePlaying;

    void Start()
    {
        spectrumBars = new GameObject[barCount];
        source = gameObject.GetComponent<AudioSource>();
        PopulateSpectrumBars();
        period = (1 / (float)fps);
        transform.localScale = new Vector3(0f, 0, 0.1f);
    }

    void Update()
    {


        if (source.isPlaying && active == true)
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime += period;
                GetFFT();
            }
            Visualize();
        }
        else if (active)
        {
            active = false;
            StartCoroutine(Collapse());
            messagePair.value.voiceMessage.played = true;
            onMessageDonePlaying.Raise();
        }
    }

    public void PopulateSpectrumBars()
    {
        float width = 0.75f * GetComponent<RectTransform>().sizeDelta.x;
        float barwidth = width / spectrumBars.Length;
        for (int i = 0; i < spectrumBars.Length; i++)
        {
            spectrumBars[i] = Instantiate(Resources.Load("Panel") as GameObject, transform);
            spectrumBars[i].transform.parent = this.transform;
            spectrumBars[i].transform.localScale = new Vector3(barwidth / width, 0f, 0.1f);
            spectrumBars[i].transform.localPosition = new Vector3(barwidth * i, 0, 0) - new Vector3(width / 2, 0, 0) + new Vector3(barwidth / 2f, 0, 0);
        }
    }

    public void SetupMessagePair()
    {
        StartCoroutine(Expand( messagePair.value.voiceMessage.message));
    }

    public void Setup(string name, AudioClip voiceLine)
    {
        StartCoroutine(Expand(voiceLine));
    }

    private IEnumerator Expand( AudioClip voiceLine)
    {
        string name = messagePair.value.voiceMessage.character.ToString();
        source.PlayOneShot(Resources.Load<AudioClip>(name + "VlogIntro"), 0.5f);
        Image portret = GameObject.FindGameObjectWithTag("Portret").GetComponent<Image>();
        portret.sprite = Resources.Load<Sprite>(name+"profilepic");
        yield return new WaitForSeconds(6f);
        float currTime = Time.time;
        while (transform.localScale != new Vector3(0.01f, 0.8f, 0.1f))
        {
            float ratio = Mathf.SmoothStep(0f, 0.8f, 2f*(Time.time - currTime));
            transform.localScale = new Vector3(0.01f, ratio, 0.1f);
            yield return null;
        }

        currTime = Time.time;
        while (transform.localScale != new Vector3(0.8f, 0.8f, 0.1f))
        {
            float ratio = Mathf.SmoothStep(0.01f, 0.8f, 2f * (Time.time - currTime));
            transform.localScale = new Vector3(ratio, 0.8f, 0.1f);
            yield return null;
        }

        Text nameText = GetComponentInChildren<Text>();
        for (int i = 0; i < name.Length; i++)
        {
            nameText.text += name[i];
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2f);
        active = true;
        source.PlayOneShot(voiceLine);
    }

    private IEnumerator Collapse()
    {
        Text nameText = GetComponentInChildren<Text>();
        for (int i = 0; i < nameText.text.Length; i++)
        {
            nameText.text = nameText.text.Remove(nameText.text.Length - 1 - i);
            yield return new WaitForSeconds(0.1f);
        }
        nameText.text = "";

        float currTime = Time.time;

        while (transform.localScale != new Vector3(0.01f, 0.8f, 0.1f))
        {
            float ratio = Mathf.SmoothStep(0.8f, 0.01f, 2f * (Time.time - currTime));
            transform.localScale = new Vector3(ratio, 0.8f, 0.1f);
            yield return null;
        }

        currTime = Time.time;
        while (transform.localScale != new Vector3(0.01f, 0f, 0.1f))
        {
            float ratio = Mathf.SmoothStep(0.8f, 0f, 2f * (Time.time - currTime));
            transform.localScale = new Vector3(0.01f, ratio, 0.1f);
            yield return null;
        }
        transform.localScale = new Vector3(0f, 0f, 0.1f);
    }

    private float[] GetFFT()
    {
        source.GetSpectrumData(fullSpectrumData, 0, FFTWindow.Blackman);
        for (int i = 0; i < fullSpectrumData.Length / 2; i++)
        {
            float re = fullSpectrumData[2 * i];
            float im = fullSpectrumData[2 * i + 1];
            spectrumData[i] = Mathf.Sqrt(re * re + im * im);
        }
        return spectrumData;
    }

    private void Visualize()
    {
        for (int i = 0; i < spectrumBars.Length; i++)
        {
            float avg = 0;
            if (displayMode == DrawMode.avg)
            {
                for (int x = 0; x < (spectrumData.Length / spectrumData.Length); x++)
                {
                    avg += spectrumData[(spectrumData.Length / spectrumData.Length) * i + x];
                }
                avg /= (spectrumData.Length / spectrumData.Length);
            }
            else if (displayMode == DrawMode.real)
            {
                avg = spectrumData[i];
            }
            else if (displayMode == DrawMode.full)
            {
                avg = fullSpectrumData[i];
            }
            else if (displayMode == DrawMode.backToBack)
            {
                int index = (i < (spectrumBars.Length / 2) + 1) ? (spectrumBars.Length / 2) - i : i - (spectrumBars.Length / 2) + 1;
                avg = spectrumData[index];
            }
            else if (displayMode == DrawMode.inverseBackToBack)
            {
                int index = (i < (spectrumBars.Length / 2) + 1) ? i : (spectrumBars.Length - 1) - i;
                avg = spectrumData[index];
            }

            spectrumBars[i].transform.localScale = new Vector3(spectrumBars[i].transform.localScale.x, Mathf.Clamp01(Mathf.Lerp(spectrumBars[i].transform.localScale.y, avg * amplitude, Time.deltaTime * (1 / period))), 0.1f);
        }
    }

}
