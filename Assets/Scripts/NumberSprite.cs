using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 附加到每个方格中，用于定义方格行为
/// </summary>
public class NumberSprite : MonoBehaviour
{
    private Image img;
    public void Awake()
    {
        img = GetComponent<Image>();
    }
    public void SetImage(int number)
    {
        //2 ==> 精灵 ==> 设置到image中
        //读取单个用Load 读取所有用LoadAll
        img.sprite = ResourceManager.LoadSprite(number);
    }

    //移动 合并 生成效果

    public void CreateEffect()
    {
        //由小到大(0 -> 1)
        iTween.ScaleFrom(gameObject, Vector3.zero, 0.3f);
    }
}
