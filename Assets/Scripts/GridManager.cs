using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    private List<Vector2> pointList = new List<Vector2>();
    private List<Grid> gridList = new List<Grid>();

    private void Awake()
    {
        Instance = this;
        CreateGridBaseGrid();
    }
    void Start()
    {
        //CreateGridBaseColl();
        //CreateGridBaseList();
        
    }

    private void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //  Debug.Log(GetGridPointByMouse());
        //}
    }

    /// <summary>
    /// ������ײ����ʽ��������
    /// </summary>
    private void CreateGridBaseColl()
    {
        //����һ��Ԥ��������
        GameObject prefabGrid = new GameObject();
        prefabGrid.AddComponent<BoxCollider2D>().size=new Vector2(1,1.5f);
        prefabGrid.transform.SetParent(transform);
        prefabGrid.transform.position= transform.position;
        prefabGrid.name = 0 + "-" + 0;

        for(int i=0;i<9;i++)
        {
            for(int j=0;j<5;j++)
            {
                if (i == 0 && j == 0)
                    continue;
                GameObject grid = GameObject.Instantiate<GameObject>(prefabGrid, transform.position + new Vector3(1.33f * i, 1.63f * j, 0), Quaternion.identity, transform);
                grid.name = i + "-" + j;
            }
        }
    }

    /// <summary>
    /// ��������list����ʽ��������
    /// </summary>
    private void CreateGridBaseList()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                pointList.Add(transform.position + new Vector3(1.33f * i, 1.63f * j, 0));
            }
        }
    }

    /// <summary>
    /// ����Grid�ű�����ʽ��������
    /// </summary>
    private void CreateGridBaseGrid()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                //pointList.Add(transform.position + new Vector3(1.33f * i, 1.63f * j, 0));
                gridList.Add(new Grid(new Vector2(i, j), transform.position + new Vector3(1.33f * i, 1.63f * j, 0), false));
            }
        }
    }

    /// <summary>
    /// ͨ������ȡ���������
    /// </summary>
    public Vector2 GetGridPointByMouse()
    {
        return GetGridPointByWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    /// <summary>
    /// ͨ�����������ȡ���������
    /// </summary>
    public Vector2 GetGridPointByWorldPos(Vector2 worldPos)
    {
        return GetGridByWorldPos(worldPos).position;
    }

    /// <summary>
    /// ͨ������ȡ����
    /// </summary>
    /// <returns></returns>
    public Grid GetGridByMouse()
    {
        return GetGridByWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
    /// <summary>
    /// ͨ�����������ȡ����
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Grid GetGridByWorldPos(Vector2 worldPos)
    {
        float dis = 100000;
        Grid grid = null;
        for (int i = 0; i < gridList.Count; i++)
        {
            if (Vector2.Distance(worldPos, gridList[i].position) < dis)
            {
                dis = Vector2.Distance(worldPos, gridList[i].position);
                grid = gridList[i];
            }
        }
        return grid;
    }

    /// <summary>
    /// ͨ������Ѱ������㣬�������ϣ���0��ʼ
    /// </summary>
    /// <param name="verticalNum"></param>
    /// <returns></returns>
    public Grid GetGridByVerticalNum(int verticalNum)
    {
        for (int i = 0; i < gridList.Count; i++) 
        {
            if (gridList[i].point == new Vector2(8, verticalNum))
                return gridList[i];
        }
        return null;
    }
}
