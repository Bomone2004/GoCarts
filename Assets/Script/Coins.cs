using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField][Range(2, 100)] private int coinsMaxValue;
    [SerializeField]private bool randomCoins = true;

    public int CoinsEarned { get; private set; } = 1;

    void Start()
    {
        CoinsEarned = randomCoins ? Random.Range(1, coinsMaxValue) : coinsMaxValue;    
    }
}
