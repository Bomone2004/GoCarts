using System;
using UnityEngine;

public class CoinGrabber : MonoBehaviour
{
    public static Action<int> CoinsUpdated;

    [SerializeField] AudioClip coinSound;

    private int _coins;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            _coins = other.GetComponent<Coins>().CoinsEarned;
            CoinsUpdated?.Invoke(_coins);
            AudioSource.PlayClipAtPoint(coinSound, other.transform.position);
        }
    }
}
