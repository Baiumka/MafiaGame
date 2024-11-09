using System.Collections.Generic;
using UnityEngine;

public class ColorStore: MonoBehaviour
{
    [SerializeField] private Color MAFIA_BACKGROUND_COLOR_;
    [SerializeField] private Color MAFIA_TEXT_COLOR_;
    [SerializeField] private Color CITIZEN_BACKGROUND_COLOR_;
    [SerializeField] private Color CITIZEN_TEXT_COLOR_;
    [SerializeField] private Color NONE_BACKGROUND_COLOR_;
    [SerializeField] private Color NONE_TEXT_COLOR_;

    public Color MAFIA_BACKGROUND_COLOR { get => MAFIA_BACKGROUND_COLOR_; }
    public Color MAFIA_TEXT_COLOR { get => MAFIA_TEXT_COLOR_;  }
    public Color CITIZEN_BACKGROUND_COLOR { get => CITIZEN_BACKGROUND_COLOR_; }
    public Color CITIZEN_TEXT_COLOR { get => CITIZEN_TEXT_COLOR_;  }
    public Color NONE_BACKGROUND_COLOR { get => NONE_BACKGROUND_COLOR_;  }
    public Color NONE_TEXT_COLOR { get => NONE_TEXT_COLOR_; }

    public static ColorStore store;

    private void Awake()
    {
        if(store == null)
        {
            store = this;
        }
        else
        {
            Destroy(this);
        }
    }

}