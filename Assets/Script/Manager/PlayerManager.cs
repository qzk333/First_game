using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour , ISaveManager
{
    public static PlayerManager instance;
    public Player player;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    
    public void LoadData(GameData _data)
    {
        // Ensure player reference is set
        if (player == null)
            player = FindObjectOfType<Player>();

        if (player != null)
        {
            // Only load position if it's NOT the first time (to avoid snapping to 0,0,0 on New Game)
            if (!_data.isFirstLoad)
            {
                player.transform.position = _data.playerPosition.ToVector3();
            }
            
            // Restore health
            if (player.stats != null)
            {
                player.stats.currentHealth = _data.playerHealth;
                player.stats.maxHealth.SetDefaultValue(_data.playerMaxHealth);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (player == null)
            player = FindObjectOfType<Player>();

        if (player != null)
        {
            // Save position
            _data.playerPosition = SerializableVector3.FromVector3(player.transform.position);
            
            // Save health
            if (player.stats != null)
            {
                _data.playerHealth = player.stats.currentHealth;
                _data.playerMaxHealth = player.stats.maxHealth.GetValue();
            }
        }
    }
}