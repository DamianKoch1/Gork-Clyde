using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;


public class AnimateMaterialOffset : MonoBehaviour
{
    [SerializeField][Range(-0.01f, 0.01f)]
    private float x, y;

    private Material mat;

    void Start()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer)
        {
            mat = GetComponent<Renderer>().material;
        }
        else
        {
            mat = GetComponent<Image>().material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var prevOffset = mat.GetTextureOffset("_BaseMap");
        if (prevOffset.x > 1)
        {
            prevOffset.x = 0;
        }
        if (prevOffset.y > 1)
        {
            prevOffset.y = 0;
        }
        mat.SetTextureOffset("_BaseMap", prevOffset + new Vector2(x, y));
    }
}
