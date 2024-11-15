using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private const string k_moduleName1 = nameof(PlayerController);
    [Header(k_moduleName1 + "    :     Assignables"), SerializeField, Range(0, 0)]
    private byte m_header1;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private Transform[] spawnPointsArr;

    private int point;
    private Dictionary<string, Transform> spawnPointsDict;

    private void Awake()
    {
        if(instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
        InitializeComponents();
        InitializeParameters();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }
    private void InitializeComponents()
    {

    }

    private void InitializeParameters()
    {
        point = 0;
        pointText.text = $"Points : {point}";
        spawnPointsDict = spawnPointsArr.ToDictionary(tp => tp.name, tp => tp);
    }

    public void IncreasePoint(in int p_point)
    {
        point += p_point;
        pointText.text = $"Points : {point}";
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(spawnPointsDict.TryGetValue(scene.name, out Transform spawnPoint)){
            if(scene.name == "Level1") Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        }
        Debug.Log("New scene loaded: " + scene.name);
    }
}
