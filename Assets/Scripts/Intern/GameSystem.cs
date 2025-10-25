using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }

    [Header("Prefab")]
    [SerializeField] private GameObject cellBG;
    [SerializeField] private List<Point> points;
    [SerializeField] private List<int> quantities;

    [Header("UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Board")]
    [SerializeField] private Transform boardTrans;
    [SerializeField] private int cellX;
    [SerializeField] private int cellY;

    [Header("Bottom")]
    [SerializeField] private Transform bottomTrans;
    [SerializeField] private int btcellX;

    [Header("Data")]
    public int mode;
    public bool isEnd;
    public List<Point> boardPoints;
    private Dictionary<FishType, List<Point>> bottomDict = new Dictionary<FishType, List<Point>>();
    private float time;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mode = PlayerPrefs.GetInt("mode");

        if (mode == 0)
        {
            timeText.gameObject.SetActive(false);
        }
        else if (mode == 1)
        {
            time = 60;
        }

        for (int i = 0; i < cellY; i++)
        {
            for (int j = 0; j < cellX; j++)
            {
                Vector2 spawnPoint = new Vector2(j, i) - new Vector2(cellX / 2f, cellY / 2f) + new Vector2(0.5f, 0.5f);
                GameObject cell = Instantiate(cellBG, spawnPoint, Quaternion.identity);
                cell.transform.SetParent(boardTrans);

                int ran = Random.Range(0, points.Count);
                Point point = Instantiate(points[ran], spawnPoint, Quaternion.identity);
                point.transform.SetParent(cell.transform);
                boardPoints.Add(point);

                quantities[ran]--;
                if (quantities[ran] == 0)
                {
                    quantities.RemoveAt(ran);   
                    points.RemoveAt(ran);
                }
            }
        }

        for (int i = 0; i < btcellX; i++)
        {
            Vector2 spawnPoint = new Vector2(i, 0) - new Vector2(btcellX / 2f, 0) + new Vector2(0.5f, 0.5f);
            GameObject cell = Instantiate(cellBG, spawnPoint, Quaternion.identity);
            cell.transform.SetParent(bottomTrans, false);
        }
    }

    private void Update()
    {
        if (mode != 1) return;
        if (isEnd) return;

        time -= Time.deltaTime;
        timeText.text = Mathf.CeilToInt(time) + "";

        if (time <= 0)
        {
            isEnd = true;
            losePanel.SetActive(true);
        }
    }

    public void FromBoardToBottom(Point point)
    {
        if (FindNextBottomCell() == null) return;

        point.transform.SetParent(FindNextBottomCell());
        boardPoints.Remove(point);

        if (bottomDict.ContainsKey(point.type))
        {
            bottomDict[point.type].Add(point);

            if (bottomDict[point.type].Count == 3)
            {
                foreach (Point p in bottomDict[point.type])
                {
                    DestroyImmediate(p.gameObject);
                }

                bottomDict.Remove(point.type);
            }
        }
        else
        {
            List<Point> typeList = new List<Point> { point };
            bottomDict.Add(point.type, typeList);
        }

        foreach (Point p in boardPoints)
        {
            if (p == null)
            {
                boardPoints.Remove(p);
            }
        }

        if (boardPoints.Count == 0)
        {
            isEnd = true;
            winPanel.SetActive(true);
        }

        if (FindNextBottomCell() == null && mode == 0)
        {
            isEnd = true;
            losePanel.SetActive(true);
        }
    }

    public void FromBottomToBoard(Point point)
    {
        point.transform.SetParent(point.initParent);
        boardPoints.Add(point);

        if (bottomDict.ContainsKey(point.type))
        {
            bottomDict[point.type].Remove(point);
        }
    }

    private Transform FindNextBottomCell()
    {
        foreach (Transform cell in bottomTrans)
        {
            if (cell.childCount == 0) return cell;
        }

        return null;
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
