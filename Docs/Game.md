# Game 類別規格

## 私有變數
- `board`  
  - 當前盤面，一維陣列 (空位置:0, Player1:1, Player2:2)。
- `activePlayerId`  
  - 當前行動玩家 ID。
- `result`  
  - 結果 enum，表示目前遊戲狀態。

## Enum: PlayerId

| 值      | 說明   |
| ------- | ------ |
| Player1 | 玩家 1 |
| Player2 | 玩家 2 |

## Enum: GameResult

| 值         | 說明         |
| ---------- | ------------ |
| None       | 尚未有結果   |
| Player1Win | 玩家 1 獲勝  |
| Player2Win | 玩家 2 獲勝  |
| DoubleWin  | 雙方同時獲勝 |
| Draw       | 平手         |

## 建構子
- `new Game()`
  - 空盤面且白子先手。

## 方法

### 取得行動玩家
- GetActivePlayerId()
  - 回傳目前輪到行動的玩家 ID。

### 落子
- Place(positionIndex)
  - 在指定位置 `positionIndex` 落子。
  - 若 `result` 不為 `None`，則此操作無效並拋出例外。

### 輪動
- Shift()
  - 使棋盤內圈、外圈皆逆時針移動 1 格。
  - 輪動後，棋盤狀態需更新。

```
track = new() { 4, 0, 1, 2, 8, 9, 5, 3, 12, 10, 6, 7, 13, 14, 15, 11 };
依據這個轉換表進行輪動
```

Example
```
⬛⬛⬛⬛
⬛⬛🔴⬛
⬛🔴⬛🔴
⬛⬛⚪⚪

輪動

⬛⬛⬛⬛
⬛🔴⬛🔴
⬛⬛🔴⚪
⬛⬛⬛⚪
```

### 結算
- Evaluate()
  - 判斷棋盤是否有玩家獲勝。
  - 橫向或縱向或斜向有 4 連者獲勝。
  - 可能同時有多方獲勝，或平手。
  - 更新 `result` 狀態。

Example
```
⬛🔴⬛⬛
⬛🔴⬛⬛
⬛🔴⬛⬛
⬛🔴⬛⬛
--
⬛⬛⬛⬛
🔴🔴🔴🔴
⬛⬛⬛⬛
⬛⬛⬛⬛
--
⬛⬛⬛🔴
⬛⬛🔴⬛
⬛🔴⬛⬛
🔴⬛⬛⬛
```

## 取得遊戲結果
- GetResult()
  - 回傳目前遊戲結果 enum（GameResult）。


-------


從設計面來說 我將 Game 以規則引擎的方式實作

指令-查詢分離原則 (CQS)
```cs
// 指令 (Commands) - 改變狀態但不回傳結果
game.Place(positionIndex);
game.Shift();
game.Evaluate();
game.SwitchPlayer();

// 查詢 (Queries) - 回傳狀態但不改變狀態
var board = game.GetBoard();
var result = game.GetResult();
var activePlayer = game.GetActivePlayerId();
```

外部協調者模式
```cs
private async UniTask Turn(int posistionIndex, PieceType pieceType)
{
    gameStageView.Place(posistionIndex, pieceType);
    await UniTask.Delay(200);
    await gameStageView.Shift();
    await UniTask.Delay(200);
}

```

這樣的設計讓外部可以：
1.	控制時序：決定何時執行哪個步驟
2.	觀察中間態：在每步之間檢查狀態
3.	客製化流程：根據需求調整遊戲流程



我會採較 細粒度控制 的寫法還有一點就是
Shift, Place 是屬於規則操作 至於我想怎麼去調用產生新玩法也是我很關注的
比如特訂條件下 Shift 2次  / 可以 Place 2次



你提到的「規則操作」與「玩法組合」的分離是很經典的遊戲設計模式。

🎮 遊戲設計的分層架構
基礎層：原子操作 (Atomic Operations)

```cs
// 你的 Game 類別提供的基礎「規則操作」
public void Place(int positionIndex)    // 原子操作：放置棋子
public void Shift()                     // 原子操作：移動棋盤
public void Evaluate()                  // 原子操作：評估勝負
public void SwitchPlayer()              // 原子操作：切換玩家

```

組合層：玩法策略 (Gameplay Strategy)
```cs
// 不同玩法的策略模式
public interface IGameplayStrategy
{
    void ExecuteTurn(Game game, int positionIndex);
}

public class StandardGameplay : IGameplayStrategy
{
    public void ExecuteTurn(Game game, int positionIndex)
    {
        game.Place(positionIndex);
        game.Shift();
        game.Evaluate();
        
        if (game.GetResult() == GameResult.None)
            game.SwitchPlayer();
    }
}

public class DoubleShiftGameplay : IGameplayStrategy
{
    public void ExecuteTurn(Game game, int positionIndex)
    {
        game.Place(positionIndex);
        
        // 特殊玩法：Shift 兩次
        game.Shift();
        game.Shift();
        
        game.Evaluate();
        
        if (game.GetResult() == GameResult.None)
            game.SwitchPlayer();
    }
}

public class DoublePlaceGameplay : IGameplayStrategy
{
    private bool firstPlaceOfTurn = true;
    
    public void ExecuteTurn(Game game, int positionIndex)
    {
        game.Place(positionIndex);
        
        if (firstPlaceOfTurn)
        {
            // 第一次放置後不移動，等待第二次放置
            firstPlaceOfTurn = false;
            return;
        }
        
        // 第二次放置後才進行後續操作
        firstPlaceOfTurn = true;
        game.Shift();
        game.Evaluate();
        
        if (game.GetResult() == GameResult.None)
            game.SwitchPlayer();
    }
}


```