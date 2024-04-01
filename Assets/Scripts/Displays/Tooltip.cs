using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerFields;
    public TextMeshProUGUI contentFields;
    public List<Image> imageField;
    public LayoutElement layoutElement;
    public int characterWarpLimit;

    private void Update()
    {
        //int headerLength = headerFields.text.Length;
        //int contentLength = 0;
        //int imageLength = 0;
        //if (contentFields != null) { contentLength = contentFields.text.Length; }
        //if (imageField.Count != 0) { imageLength = imageField.Count * (int)imageField[0].rectTransform.sizeDelta[0]; }
        //Debug.Log(imageLength);


        //layoutElement.enabled = (headerLength > characterWarpLimit || contentLength > characterWarpLimit) ? true : false;
    }
}
