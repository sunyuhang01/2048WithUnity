using Console2048;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MoveDirection = Console2048.MoveDirection;

/// <summary>
/// 游戏控制器
/// </summary>
public class GameController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public int moveThreshold = 50;
    private GameCore core;
    private NumberSprite[,] spriteActionArray; //sprite行为类的二维数组
    private void Start()
    {
        core = new GameCore();
        spriteActionArray = new NumberSprite[4, 4];

        Button btn = this.transform.parent.Find("Quit").GetComponent<Button>();
        btn.onClick.AddListener(Application.Quit);

        Init();

        GenerateNewNumber();
        GenerateNewNumber();
    }

    private void Init()
    {
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                CreateSprite(i, j);
            }
        }
    }

    private void CreateSprite(int r, int c)
    {
        GameObject go = new GameObject(r.ToString() + c.ToString());
        go.AddComponent<Image>();
        NumberSprite action = go.AddComponent<NumberSprite>(); //Awake立即执行 Start下一帧执行
        //将引用存储到二维数组中
        spriteActionArray[r, c] = action;
        action.SetImage(0);
        //创建的游戏对象，scale默认为1。设置false为相对与父亲的，默认（true）相对世界
        go.transform.SetParent(this.transform, false);
    }
    /// <summary>
    /// 产生新数字
    /// </summary>
    private void GenerateNewNumber()
    {
        Location? loc;
        int? number;
        core.GenerateNumber(out loc, out number);
        //根据位置获取脚本对象引用
        spriteActionArray[loc.Value.RIndex, loc.Value.CIndex].SetImage(number.Value);
        //播放生成效果
        spriteActionArray[loc.Value.RIndex, loc.Value.CIndex].CreateEffect();
    }

    private void Update()
    {
        //如果地图有更新
        if (core.isChange)
        {
            //更新界面
            UpdateMap();
            //产生新数字
            GenerateNewNumber();
            //判断游戏是否结束
            core.CheckGameOver();
            if (core.isOver)
            {
                //游戏结束
            }

            core.isChange = false;
        }
    }

    private void UpdateMap()
    {
        for(int r = 0; r < 4; r++)
        {
            for(int c = 0; c < 4; c++)
            {
                spriteActionArray[r,c].SetImage(core.Map[r, c]);
            }
        }
    }
    private Vector2 startPoint;
    private bool isDown = false;
    //点击时执行
    public void OnPointerDown(PointerEventData eventData)
    {
        startPoint = eventData.position;
        isDown = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDown) return;
        Vector2 offset = eventData.position - startPoint;
        float x = Mathf.Abs(offset.x);
        float y = Mathf.Abs(offset.y);

        MoveDirection? dir = null;
        //水平移动
        if(x > y && x >= moveThreshold)
        {
            dir = offset.x > 0 ? MoveDirection.Right : MoveDirection.Left;
        }
        //垂直移动
        if(x <= y && y >= moveThreshold)
        {
            dir = offset.y > 0 ? MoveDirection.Up : MoveDirection.Down;
        }
        if(dir != null)
        {
            core.Move(dir.Value);
            isDown = false;
        }
        
    }
    
}
