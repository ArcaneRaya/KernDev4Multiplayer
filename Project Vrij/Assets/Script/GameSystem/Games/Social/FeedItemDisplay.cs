using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedItemDisplay : MonoBehaviour
{
    public RawImage image;
    //public Text title;
    public Text content;

    public void Setup(FeedItem item)
    {
        image.texture = item.Image;
        content.text = item.Message;
    }
}
