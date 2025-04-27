using System.Collections.Generic;
using UnityEngine;

public class ConnectFourAI
{
    public int maxDepth = 5;

    public int GetBestMove(int[,] board)
    {
        int bestScore = int.MinValue;
        int bestCol = 0;

        for (int col = 0; col < 7; col++)
        {
            if (IsValidMove(board, col))
            {
                int[,] tempBoard = (int[,])board.Clone();
                MakeMove(tempBoard, col, 2); // AI = 2
                int score = Minimax(tempBoard, maxDepth - 1, false, int.MinValue, int.MaxValue);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestCol = col;
                }
            }
        }

        return bestCol;
    }

    int Minimax(int[,] board, int depth, bool maximizingPlayer, int alpha, int beta)
    {
        int winner = CheckWinner(board);
        if (winner == 2) return 1000000;  // AI win
        if (winner == 1) return -1000000; // Player win
        if (IsFull(board) || depth == 0) return EvaluateBoard(board);

        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;
            for (int col = 0; col < 7; col++)
            {
                if (IsValidMove(board, col))
                {
                    int[,] newBoard = (int[,])board.Clone();
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
                    int[,] newBoard = (int[,])board.Clone();
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

    bool IsValidMove(int[,] board, int col)
    {
        return board[0, col] == 0;
    }

    bool IsFull(int[,] board)
    {
        for (int col = 0; col < 7; col++)
            if (board[0, col] == 0) return false;
        return true;
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

    int CheckWinner(int[,] board)
    {
        // Horizontal, vertical, and diagonal check (return 1, 2, or 0)
        int rows = 6, cols = 7;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int player = board[r, c];
                if (player == 0) continue;

                // Horizontal
                if (c + 3 < cols && board[r, c + 1] == player && board[r, c + 2] == player && board[r, c + 3] == player)
                    return player;

                // Vertical
                if (r + 3 < rows && board[r + 1, c] == player && board[r + 2, c] == player && board[r + 3, c] == player)
                    return player;

                // Diagonal down-right
                if (r + 3 < rows && c + 3 < cols &&
                    board[r + 1, c + 1] == player && board[r + 2, c + 2] == player && board[r + 3, c + 3] == player)
                    return player;

                // Diagonal up-right
                if (r - 3 >= 0 && c + 3 < cols &&
                    board[r - 1, c + 1] == player && board[r - 2, c + 2] == player && board[r - 3, c + 3] == player)
                    return player;
            }
        }

        return 0; // No winner
    }

    int EvaluateBoard(int[,] board)
    {
        // Simple heuristic: count center control and potential 2/3s
        int score = 0;
        int centerCol = 3;
        for (int row = 0; row < 6; row++)
        {
            if (board[row, centerCol] == 2) score += 3;
            else if (board[row, centerCol] == 1) score -= 3;
        }
        return score;
    }
}
