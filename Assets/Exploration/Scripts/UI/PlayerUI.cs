using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    public GameObject player;
    public SynchronizedActor PlayerActor
    {
        get
        {
            return player.GetComponentInChildren<SynchronizedActor>();
        }
    }

    internal void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        PlayerActor.Stats.OnStatsChangedEvent += OnPlayerStatsChanged;
        OnPlayerStatsChanged();

    }

    private void OnPlayerStatsChanged()
    {
        EntityStatistics stats = PlayerActor.Stats;

        StatsBar.Instance.SetStat("HP", stats.Health.ToString());
        StatsBar.Instance.SetStat("ENERGY", stats.Energy.ToString());
        StatsBar.Instance.SetStat("ATTACK", String.Format("{0:P0}", stats.Attack));

    }
}
