using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{

    spawningScript dropper;
    public int maxDepth = 5;
    GameObject gridObject;
    Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        dropper = GameObject.Find("Dropper").GetComponent<spawningScript>();
        gridObject = GameObject.Find("Grid");
        grid = gridObject.GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void play(GameObject chip)
    {
        int col = GetBestMove(grid.updateGrid());
        Debug.Log("AI choosing column: " + col);
        dropper.dropAt(chip,col);
    }

   

    public int GetBestMove(int[,] board)
    {
        // 1. Check for immediate AI win
        for (int col = 0; col < 7; col++)
        {
            if (IsValidMove(board, col))
            {
                int[,] tempBoard = (int[,])board.Clone();
                MakeMove(tempBoard, col, 2);
                if (CheckWinner(tempBoard) == 2)
                {
                    return col + 1;
                }
            }
        }

        // 2. Check for player threats (MUST RETURN if found)
        for (int col = 0; col < 7; col++)
        {
            if (IsValidMove(board, col))
            {
                int[,] tempBoard = (int[,])board.Clone();
                MakeMove(tempBoard, col, 1);
                if (CheckWinner(tempBoard) == 1)
                {
                    return col + 1; // This is the critical blocking move
                }
            }
        }

        // 3. Proceed with minimax if no immediate wins/blocks
        int bestScore = int.MinValue;
        int bestCol = 3; // Default to center

        for (int col = 0; col < 7; col++)
        {
            if (IsValidMove(board, col))
            {
                int[,] tempBoard = (int[,])board.Clone();
                MakeMove(tempBoard, col, 2);
                int score = Minimax(tempBoard, maxDepth - 1, false, int.MinValue, int.MaxValue);

                if (score > bestScore ||
                   (score == bestScore && Mathf.Abs(col - 3) < Mathf.Abs(bestCol - 3)))
                {
                    bestScore = score;
                    bestCol = col;
                }
            }
        }

        return bestCol + 1;
    }

    int Minimax(int[,] board, int depth, bool maximizingPlayer, int alpha, int beta)
    {
        int winner = CheckWinner(board);
        if (winner == 2) return 1000000 + depth;  // AI win
        if (winner == 1) return -1000000 - depth; // Player win
        if (IsFull(board) || depth == 0) return EvaluateBoard(board);

        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;
            for (int col = 0; col < 7; col++)
            {
                if (IsValidMove(board, col))
                {
                    int[,] newBoard = CloneBoard(board);
                    MakeMove(newBoard, col, 2);
                    int eval = Minimax(newBoard, depth - 1, false, alpha, beta);
                    maxEval = Mathf.Max(maxEval, eval);
                    alpha = Mathf.Max(alpha, eval);
                    if (beta <= alpha) break;
                }
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            for (int col = 0; col < 7; col++)
            {
                if (IsValidMove(board, col))
                {
                    int[,] newBoard = CloneBoard(board);
                    MakeMove(newBoard, col, 1);
                    int eval = Minimax(newBoard, depth - 1, true, alpha, beta);
                    minEval = Mathf.Min(minEval, eval);
                    beta = Mathf.Min(beta, eval);
                    if (beta <= alpha) break;
                }
            }
            return minEval;
        }
    }

    int[,] CloneBoard(int[,] board)
    {
        return (int[,])board.Clone();
    }

    

    bool IsFull(int[,] board)
    {
        for (int col = 0; col < 7; col++)
            if (board[0, col] == 0) return false;
        return true;
    }



    void PrintBoard(int[,] board)
    {
        string output = "Current Board (0=empty, 1=player, 2=AI):\n";
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                output += board[row, col] + " ";
            }
            output += "\n";
        }
        Debug.Log(output);
    }

    bool IsValidMove(int[,] board, int col)
    {
        return col >= 0 && col < 7 && board[0, col] == 0;
    }

    void MakeMove(int[,] board, int col, int player)
    {
        for (int row = 5; row >= 0; row--)
        {
            if (board[row, col] == 0)
            {
                board[row, col] = player;
                return;
            }
        }
    }

    public int CheckWinner(int[,] board)
    {
        // Horizontal
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                int player = board[row, col];
                if (player != 0 &&
                    player == board[row, col + 1] &&
                    player == board[row, col + 2] &&
                    player == board[row, col + 3])
                {
                    return player;
                }
            }
        }

        // Vertical
        for (int col = 0; col < 7; col++)
        {
            for (int row = 0; row < 3; row++)
            {
                int player = board[row, col];
                if (player != 0 &&
                    player == board[row + 1, col] &&
                    player == board[row + 2, col] &&
                    player == board[row + 3, col])
                {
                    return player;
                }
            }
        }

        // Diagonal down-right
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                int player = board[row, col];
                if (player != 0 &&
                    player == board[row + 1, col + 1] &&
                    player == board[row + 2, col + 2] &&
                    player == board[row + 3, col + 3])
                {
                    return player;
                }
            }
        }

        // Diagonal up-right
        for (int row = 3; row < 6; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                int player = board[row, col];
                if (player != 0 &&
                    player == board[row - 1, col + 1] &&
                    player == board[row - 2, col + 2] &&
                    player == board[row - 3, col + 3])
                {
                    return player;
                }
            }
        }

        return 0;
    }



    int EvaluateBoard(int[,] board)
    {
        int score = 0;

        // Center column preference (column 4 in 1-7, which is index 3 in array)
        for (int row = 0; row < 6; row++)
        {
            if (board[row, 3] == 2) score += 3;
            else if (board[row, 3] == 1) score -= 3;
        }

        // Evaluate all possible lines
        score += EvaluateLines(board, 2); // AI
        score -= EvaluateLines(board, 1); // Player

        return score;
    }


    int EvaluateLines(int[,] board, int player)
    {
        int score = 0;
        int opponent = player == 1 ? 2 : 1;
        // Check horizontal, vertical, and diagonal segments
        // This is similar to CheckWinner but scores partial lines

        // Horizontal
        for (int r = 0; r < 6; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                score += EvaluateWindow(new int[] { board[r, c], board[r, c + 1], board[r, c + 2], board[r, c + 3] }, player);
            }
        }

        // Vertical
        for (int c = 0; c < 7; c++)
        {
            for (int r = 0; r < 3; r++)
            {
                score += EvaluateWindow(new int[] { board[r, c], board[r + 1, c], board[r + 2, c], board[r + 3, c] }, player);
            }
        }

        // Diagonal (down-right)
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                score += EvaluateWindow(new int[] { board[r, c], board[r + 1, c + 1], board[r + 2, c + 2], board[r + 3, c + 3] }, player);
            }
        }

        // Diagonal (up-right)
        for (int r = 3; r < 6; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                score += EvaluateWindow(new int[] { board[r, c], board[r - 1, c + 1], board[r - 2, c + 2], board[r - 3, c + 3] }, player);
            }
        }

        return score;
    }

    int EvaluateWindow(int[] window, int player)
    {
        int score = 0;
        int opponent = player == 1 ? 2 : 1;
        int playerCount = 0;
        int emptyCount = 0;
        int opponentCount = 0;

        foreach (int cell in window)
        {
            if (cell == player) playerCount++;
            else if (cell == opponent) opponentCount++;
            else emptyCount++;
        }

        // Score player opportunities
        if (playerCount == 4) score += 100000;
        else if (playerCount == 3 && emptyCount == 1) score += 100;
        else if (playerCount == 2 && emptyCount == 2) score += 10;
        else if (playerCount == 1 && emptyCount == 3) score += 1;

        // Penalize opponent opportunities more aggressively
        if (opponentCount == 3 && emptyCount == 1) score -= 1000;  // Very bad if opponent can win
        else if (opponentCount == 2 && emptyCount == 2) score -= 100;
        else if (opponentCount == 1 && emptyCount == 3) score -= 10;

        return score;
    }

}
