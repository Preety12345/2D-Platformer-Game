using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField] private int point;
    [SerializeField] private TextMeshProUGUI pointText;

    private void Awake()
    {
        if(instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
        InitializeComponents();
        InitializeParameters();
    }

    private void InitializeComponents()
    {

    }

    private void InitializeParameters()
    {
        point = 0;
        pointText.text = $"Points : {point}";
    }

    public void IncreasePoint(in int p_point)
    {
        point += p_point;
        pointText.text = $"Points : {point}";
    }

    
}
