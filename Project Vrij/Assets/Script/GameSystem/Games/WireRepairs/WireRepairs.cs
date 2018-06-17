//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkedGames;

public class WireRepairs : GameScreen
{
    /*   private GameInfoWireRepairs InternalGameInfo
       {
           get
           {
               return currentGame as GameInfoWireRepairs;
           }
       }

       public GameObject endPointContainer;
       public GameObject beginPointContainer;
       public GameObject[] endPointPrefab;
       public GameObject[] beginPointPrefab;

       private List<EndPointUI> endPoints;
   */
    protected override void InternalSetup()
    {
        throw new System.NotImplementedException();
        /*
        if (InternalGameInfo == null)
        {
            throw new System.NullReferenceException("No GameInfo provided");
        }
        SpawnWires(InternalGameInfo.wireAmount);
        */
    }
    /*
    private void SpawnWires(int wireAmount)
    {
        float gameWidth = (CanvasManager.DefaultCanvasSize.x / 10) * 8;
        float distanceBetweenPoints = gameWidth / (wireAmount - 1);
        float xOffset = -gameWidth / 2;
        List<int> endPointIDs = new List<int>();
        for (int i = 0; i < wireAmount; i++)
        {
            GameObject beginPointObj = Instantiate(beginPointPrefab[i % beginPointPrefab.Length], beginPointContainer.transform);
            beginPointObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset, 0);
            BeginPointUI beginPoint = beginPointObj.AddComponent<BeginPointUI>();
            beginPoint.id = i;
            endPointIDs.Add(i);
            xOffset += distanceBetweenPoints;
        }
        endPoints = new List<EndPointUI>();
        xOffset = -gameWidth / 2;
        while (endPointIDs.Count > 0)
        {
            int randomPosition = Random.Range(0, endPointIDs.Count);
            int id = endPointIDs[randomPosition];
            endPointIDs.RemoveAt(randomPosition);
            GameObject endPointObj = Instantiate(endPointPrefab[id % endPointPrefab.Length], endPointContainer.transform);
            endPointObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset, 0);
            EndPointUI endPoint = endPointObj.AddComponent<EndPointUI>();
            endPoint.id = id;
            xOffset += distanceBetweenPoints;
            endPoint.PointChanged += OnPointChanged;
            endPoints.Add(endPoint);
        }
    }

    private void OnPointChanged()
    {
        foreach (var point in endPoints)
        {
            if (!point.isCorrect)
            {
                return;
            }
        }

        Debug.Log("finished!");
        Finished();
    }
    */
}
