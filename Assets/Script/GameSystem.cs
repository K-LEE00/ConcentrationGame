using System.Collections;
using System.Collections.Generic;

namespace GameSystem{

    public enum GameStatus
    {
        Title,
        StageCreate,
        CardPlacement,
        FirstPhase,
        SecondPhase,
        DecisionPhase,
        GameEnd,
        GamePause,
    }

    public enum CardStatus
    {
        Close,
        Open,
        Destroy,
    }

    public enum GameUI
    {
        Title,
        Create,
        Play,
        End,
        All,
    }
}