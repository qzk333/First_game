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
        // TODO: 实现加载玩家数据的逻辑
    }

    public void SaveData(ref GameData _data)
    {
        // TODO: 实现保存玩家数据的逻辑
    }
}