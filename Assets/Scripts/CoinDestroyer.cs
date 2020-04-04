using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDestroyer : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
