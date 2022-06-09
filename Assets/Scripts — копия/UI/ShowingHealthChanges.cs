using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowingHealthChanges : MonoBehaviour
{
    public Text textChanges;


    public void SetHealthChanges(int change)
    {
        if (change < 0)
        {
            textChanges.color = Color.red;
            textChanges.text = $"{change}";
        }
        else if (change > 0)
        {
            textChanges.color = Color.green;
            textChanges.text = $"+{change}";
        }
    }

    public void Disappear() => Destroy(gameObject);
}
