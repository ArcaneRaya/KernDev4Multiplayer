using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildDebugger : MonoBehaviour
{
    private class LogItem
    {
        public float lifeTime;
        public string message;
    }

    private static BuildDebugger Instance
    {
        get
        {
            if (internalInstance == null)
            {
                internalInstance = CreateInstance();
            }
            return internalInstance;
        }
    }
    private static BuildDebugger internalInstance;

    private const bool useDebug = false;
    private const float LOGITEMLIFETIME = 10;
    private Text logText;
    private int logIndex;
    private List<LogItem> logItems;

    public static void Log(string logText)
    {
        if (!useDebug)
        {
            return;
        }
        //if (Instance.logText.text != "")
        //{
        //    Instance.logText.text += "\n";
        //}
        //Instance.logText.text += Instance.logIndex.ToString() + " | ";
        //Instance.logText.text += logText;
        LogItem newItem = new LogItem();
        newItem.message = logText;
        Instance.logItems.Add(newItem);
        Instance.logIndex++;
    }

    private void Update()
    {
        for (int i = logItems.Count - 1; i >= 0; i--)
        {
            logItems[i].lifeTime += Time.deltaTime;
            if (logItems[i].lifeTime > LOGITEMLIFETIME)
            {
                logItems.RemoveAt(i);
            }
        }
        if (logItems.Count == 0)
        {
            DestroyDebugger();
        }
        logText.text = "";
        foreach (var item in logItems)
        {
            logText.text += item.message;
            logText.text += "\n";
        }
    }

    private void DestroyDebugger()
    {
        Destroy(Instance.gameObject);
    }

    private static BuildDebugger CreateInstance()
    {
        GameObject obj = new GameObject("BuildDebugger");
        DontDestroyOnLoad(obj);
        BuildDebugger buildDebugger = obj.AddComponent<BuildDebugger>();
        buildDebugger.logItems = new List<LogItem>();

        GameObject canvasObject = new GameObject("Canvas");
        canvasObject.transform.SetParent(obj.transform);
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.targetDisplay = 0;
        canvas.sortingOrder = 10000;
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(2048, 1536);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
        canvasObject.AddComponent<GraphicRaycaster>();

        GameObject planeObject = new GameObject("Plane");
        planeObject.transform.SetParent(canvasObject.transform);
        RectTransform planeTransform = planeObject.AddComponent<RectTransform>();
        planeTransform.anchoredPosition = Vector2.zero;
        planeTransform.anchorMin = new Vector2(0.05f, 0.75f);
        planeTransform.anchorMax = new Vector2(.25f, 0.9f);
        planeObject.AddComponent<Image>();
        planeObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);

        GameObject logTextObject = new GameObject("LogText");
        logTextObject.transform.SetParent(planeObject.transform);
        RectTransform logTextTransform = logTextObject.AddComponent<RectTransform>();
        logTextTransform.anchoredPosition = Vector2.zero;
        logTextTransform.anchorMin = new Vector2(.2f, .2f);
        logTextTransform.anchorMax = new Vector2(.8f, .8f);
        buildDebugger.logText = logTextObject.AddComponent<Text>();
        buildDebugger.logText.text = "";
        buildDebugger.logText.color = Color.black;
        buildDebugger.logText.fontSize = 30;

        //GameObject closeButtonObject = new GameObject("CloseButton");
        //closeButtonObject.transform.SetParent(planeObject.transform);
        //RectTransform closeButtonTransform = closeButtonObject.AddComponent<RectTransform>();
        //closeButtonTransform.anchorMin = new Vector2(0.5f, 0);
        //closeButtonTransform.anchorMax = new Vector2(0.5f, 0);
        //closeButtonTransform.anchoredPosition = new Vector2(0, 150);
        //closeButtonTransform.sizeDelta = new Vector2(400, 80);
        //Image buttonBG = closeButtonObject.AddComponent<Image>();
        //buttonBG.color = Color.grey;
        //Button closeButton = closeButtonObject.AddComponent<Button>();
        //closeButton.onClick.AddListener(buildDebugger.DestroyDebugger);
        //GameObject buttonTextObject = new GameObject("ButtonText");
        //buttonTextObject.transform.SetParent(closeButtonObject.transform);
        //RectTransform buttonTextTransform = buttonTextObject.AddComponent<RectTransform>();
        //buttonTextTransform.anchorMin = Vector2.zero;
        //buttonTextTransform.anchorMax = new Vector2(1, 1);
        //buttonTextTransform.anchoredPosition = Vector2.zero;
        ////buttonTextTransform.sizeDelta
        //Text buttonText = buttonTextObject.AddComponent<Text>();
        //buttonText.color = Color.black;
        //buttonText.text = "close";
        //buttonText.fontSize = 40;
        //buttonText.alignment = TextAnchor.MiddleCenter;

        buildDebugger.logText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        //buttonText.font = buildDebugger.logText.font;

        buildDebugger.logIndex = 0;

        return buildDebugger;
    }
}
