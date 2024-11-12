using UnityEngine;

public class Crystal : Object
{
    [SerializeField]private int m_point;
    protected override void OnTriggerEffect()
    {
        GameManager.instance.IncreasePoint(m_point);
        gameObject.SetActive(false);
    } 
    
}
