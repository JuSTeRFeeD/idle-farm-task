using System;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Instance { get; private set; }

    public int Coins { get; private set; }

    public event Action OnCoinsChange;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
#if DEBUG
            Debug.LogError($"Should be only 1 CoinsDisplayUI instance! {nameof(PlayerInfo)} copy!");
#endif
            Destroy(gameObject);
        }
    }

    public void AddCoins(int value)
    {
        Coins += value;
        OnCoinsChange?.Invoke();
    }

    /// <returns>False - not enough balance</returns>
    public bool TakeCoins(int value)
    {
        if (Coins < value) return false;
        Coins -= value;
        OnCoinsChange?.Invoke();
        return true;
    }
}