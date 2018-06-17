using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialFeed : MonoBehaviour
{
    //public GameManager gameManagerLink;
    //public GameObject FeedItemDisplayPrefab;
    //public GameObject contentObject;
    //public int lastItemID;
    //public float bottomButtonOffset;

    //private bool updatingDisplay;

    //private void Start()
    //{
    //    lastItemID = -1;
    //}

    //public void UpdateView(float scrollPosition)
    //{
    //    if (scrollPosition < 0.1f && !updatingDisplay)
    //    {
    //        updatingDisplay = true;
    //        RequestNextItem();
    //    }
    //}

    //public void AddFeedItems(int amount)
    //{
    //    for (int i = 0; i < amount; i++)
    //    {
    //        RequestNextItem();
    //    }
    //}

    //private void RequestNextItem()
    //{
    //    FeedItem item;
    //    if (lastItemID > -1)
    //    {
    //        item = gameManagerLink.GetSocialFeedItem(lastItemID - 1);
    //    }
    //    else
    //    {
    //        item = gameManagerLink.GetSocialFeedItem();
    //    }
    //    if (item != null)
    //    {
    //        LoadFeedItem(item);
    //    }
    //}

    //private void LoadFeedItem(FeedItem item)
    //{
    //    lastItemID = item.ID;
    //    GameObject newFeedItem = Instantiate(FeedItemDisplayPrefab, contentObject.transform);
    //    newFeedItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -contentObject.GetComponent<RectTransform>().sizeDelta.y + bottomButtonOffset);
    //    newFeedItem.GetComponent<FeedItemDisplay>().Setup(item);
    //    contentObject.GetComponent<RectTransform>().sizeDelta +=
    //        new Vector2(0, FeedItemDisplayPrefab.GetComponent<RectTransform>().sizeDelta.y);
    //    updatingDisplay = false;
    //}
}
