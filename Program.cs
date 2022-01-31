using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace minesweeperDiscord {
    class Program {
        private static readonly string[] Emojis = {":bomb:", ":black_large_square:", ":one:", ":two:", ":three:", ":four:", ":five:", ":six:", ":seven:", ":eight:"};


        public static void Main(string[] args) {
            int rows, cols, numMines;
            try {
                Console.Write("Enter Number of Rows: ");
                rows = int.Parse(Console.ReadLine() + "\n");
                Console.Write("Enter Number of Cols: ");
                cols = int.Parse(Console.ReadLine() + "\n");
                Console.Write("Enter Number of Mines: ");
                numMines = int.Parse(Console.ReadLine() + "\n");
                if (numMines > rows * cols) {
                    Console.WriteLine("Too many mines!");
                    return;
                }
            } catch (FormatException) {
                Console.WriteLine("Invalid Input!");
                return;
            }

            int[][] board = GenerateBoard(rows, cols, numMines);
            foreach (int[] row in board) {
                foreach (int col in row) {
                    Console.Write($"||{Emojis[col+1]}||");
                }

                Console.WriteLine();
            }
        }

        private static int[][] GenerateBoard(int rows, int cols, int numMines) {
            int[][] board = new int[rows][];
            for (int i = 0; i < rows; i++) {
                board[i] = new int[cols];
            }

            // Initialise with 0
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < cols; j++) {
                    board[i][j] = 0;
                }
            }

            // Add mines
            int[] mines = new int[numMines];
            Random rnd = new Random();
            for (int i = 0; i < numMines; i++) {
                int row = rnd.Next(0, rows);
                int col = rnd.Next(0, cols);
                if (board[row][col] == 0) {
                    mines[i] = row * cols + col;
                    board[row][col] = -1;
                }
                else {
                    i--;
                }
            }

            // Add numbers
            foreach (int mine in mines) {
                int row = mine / cols;
                int col = mine % cols;

                // Increment all neighbours if they are not mines and exists
                foreach (int neighbor in GetNeighbours(row, col, board)) {
                    int r = neighbor / board.Length;
                    int c = neighbor % board.Length;
                    if (board[r][c] != -1) {
                        board[r][c]++;
                    }
                }
            }

            return board;
        }

        private static int[] GetNeighbours(int row, int col, int[][] board) {
            int[] neighbours = new int[0];
            for (int i = row - 1; i <= row + 1; i++) {
                for (int j = col - 1; j <= col + 1; j++) {
                    if (i == row && j == col) {
                        continue;
                    }

                    try {
                        board[i][j] = board[i][j];
                        neighbours = neighbours.Append(i * board.Length + j).ToArray();
                    }
                    catch (IndexOutOfRangeException) {
                        // Ignore
                    }
                }
            }

            return neighbours;
        }
    }
}