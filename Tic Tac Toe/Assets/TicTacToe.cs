using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : MonoBehaviour
{
    private string[,] board;
    const float gridWidth = 60f;
    const float gridHeight = 60f;
    const float gridMargin = 10f;
    private float screenCenterX;
    private float screenCenterY;
    private float boardWidth;
    private float boardHeight;

    private float boardX;
    private float boardY;

    const int rows = 3;
    const int cols = 3;

    public GUISkin skin;

    private bool playersTurn;
    private bool firstTurn;
    private Vector2Int playerLastMove;
    private Vector2Int[] delta;
    private int[] playerScore, computerScore;
    private string winner;
    private int moves;

    void ResetGame()
    {
        playersTurn = false;
        firstTurn = true;
        board = new string[rows, cols];
        playerScore = new int[rows + cols + 2];
        computerScore = new int[rows + cols + 2];
        winner = null;
        moves = 0;
    }
    void Start()
    {
        ResetGame();
        delta = new Vector2Int[4] { new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(0, 1) };
        screenCenterX = Screen.width / 2;
        screenCenterY = Screen.height / 2;
        boardWidth = rows * gridWidth + (rows - 1) * gridMargin;
        boardHeight = cols * gridHeight + (cols - 1) * gridMargin;

        boardX = screenCenterX - boardWidth / 2;
        boardY = screenCenterY - boardHeight / 2;
    }

    bool InRange(int row, int col)
    {
        return row >= 0 && row < rows && col >= 0 && col < cols;
    }

    void AddScore(int[] score, int row, int col)
    {
        if (row == col)
        {
            score[rows + cols]++;
        }

        if (row + col == rows - 1)
        {
            score[rows + cols + 1]++;
        }

        score[row]++;
        score[rows + col]++;
        moves++;
        CheckBoard();
    }

    void GameBoard()
    {
        float boardWidth = rows * gridWidth + (rows - 1) * gridMargin;
        float boardHeight = cols * gridHeight + (cols - 1) * gridMargin;

        float boardX = screenCenterX - boardWidth / 2;
        float boardY = screenCenterY - boardHeight / 2;

        if (!playersTurn && winner == null)
        {
            int row = -1, col = -1;
            if (firstTurn)
            {
                row = Random.Range(0, rows);
                col = Random.Range(0, cols);
                firstTurn = false;
            } else
            {
                for (int i = 0; i < delta.Length; i++)
                {
                    Vector2Int tmp = delta[i];
                    int r = Random.Range(i, delta.Length);
                    delta[i] = delta[r];
                    delta[r] = tmp;
                }

                for (int i = 0; i < delta.Length; ++i)
                {
                    Vector2Int tryMove = delta[i] + playerLastMove;
                    int tryRow = tryMove.x;
                    int tryCol = tryMove.y;
                    if (InRange(tryRow, tryCol) && board[tryRow, tryCol] == null)
                    {
                        row = tryRow;
                        col = tryCol;
                        break;
                    }
                }

                if (row == -1 && col == -1)
                {
                    for (int r = 0; r < rows; ++r) { 
                        for (int c = 0; c < cols; ++c) { 
                            if (board[r, c] == null)
                            {
                                row = r;
                                col = c;
                                break;
                            }
                        }
                    }
                }
            }
            playersTurn = !playersTurn;
            board[row, col] = "O";
            AddScore(computerScore, row, col);
        }

        for (int row = 0; row < 3; ++row)
        {
            for (int col = 0; col < 3; ++col)
            {
                float gridX = boardX + row * (gridWidth + gridMargin);
                float gridY = boardY + col * (gridHeight + gridMargin);

                bool clicked = GUI.Button(new Rect(gridX, gridY, gridWidth, gridHeight), board[row, col]);

                if (clicked && board[row, col] == null && playersTurn && winner == null)
                {
                    board[row, col] = "X";
                    AddScore(playerScore, row, col);
                    playerLastMove = new Vector2Int(row, col);
                    playersTurn = !playersTurn;
                }
            }
        }
    }

    void Controls()
    {
        float controlWidth = boardWidth;
        float controlHeight = gridHeight;

        float statusTextX = boardX;
        float statusTextY = boardY - (gridHeight + gridMargin);

        float resetButtonX = boardX;
        float resetButtonY = boardY + rows * (gridHeight + gridMargin);

        if (GUI.Button(new Rect(resetButtonX, resetButtonY, controlWidth, controlHeight), "Restart")) {
            ResetGame();
        }

        string message;
        if (winner == null)
        {
            message = playersTurn ? "Your turn" : "Computer's turn";
        } else
        {
            if (winner == "You")
            {
                message = winner + " win!";
            } else
            {
                message = winner + " wins!";
            }
        }
        GUI.Label(new Rect(statusTextX, statusTextY, controlWidth, controlHeight), message);
    }

    void CheckBoard()
    {
        for (int i = 0; i < playerScore.Length; ++i)
        {
            if (playerScore[i] == rows)
            {
                winner = "You";
            }

            if (computerScore[i] == rows)
            {
                winner = "Computer";
            }
        }

        if (moves == rows * cols && winner == null)
        {
            winner = "Nobody";
        }
    }
    void OnGUI()
    {
        GUI.skin = skin;
        GameBoard();
        Controls();
    }
}
