using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInRowSecondVersion
{
	public static class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Welcome to Four in a Row Game!");

			// Phase 2: Log file initialization
			string logFilePath = "Log.txt";
			InitializeLogFile(logFilePath);

			string playAgainInput = "";
			Console.WriteLine("Enter Player 1's name:");
			string player1 = Console.ReadLine();

			Console.WriteLine("Enter Player 2's name:");
			string player2 = Console.ReadLine();

			do
			{
				Console.Clear();

				PlayGame(player1, player2, logFilePath);

				Console.WriteLine("Do you want to play again? (yes/no)");
				playAgainInput = Console.ReadLine().ToLower();

				if (playAgainInput == "yes")
				{
					Console.WriteLine("First player. Do you want to use the same player name? (yes/no)");
					string sameNamesInputForFirstPlayer = Console.ReadLine().ToLower();

					if (sameNamesInputForFirstPlayer != "yes")
					{
						Console.WriteLine("Enter Player 1's name:");
						player1 = Console.ReadLine();
					}

					Console.WriteLine("Second player. Do you want to use the same player name? (yes/no)");
					string sameNamesInputForSecondPlayer = Console.ReadLine().ToLower();
			
					if (sameNamesInputForSecondPlayer != "yes")
					{
						Console.WriteLine("Enter Player 2's name:");
						player2 = Console.ReadLine();
					}
				}

			} while (playAgainInput == "yes");

			Console.WriteLine("Thanks for playing!");
			Console.ReadLine();
		}

		static void InitializeLogFile(string filePath)
		{
			if (!File.Exists(filePath))
			{
				using (StreamWriter writer = File.CreateText(filePath))
				{
					// Create an empty log file
				}
			}
		}

		static void PlayGame(string player1, string player2, string logFilePath)
		{
			// Phase 2: Using colors for coins
			char player1Coin = 'O';
			//ConsoleColor player1color = ConsoleColor.Yellow;
			
			char player2Coin = 'X';
			//ConsoleColor player2Color = ConsoleColor.Red;

			char[,] board = new char[6, 7];
			bool gameWon = false;
			bool isDraw = false;
			string currentPlayer = player1;

			do
			{
				Console.Clear();
				DrawBoard(board);

				string currentPlayerName = (currentPlayer == player1) ? player1 : player2; //write with if

				Console.WriteLine($"{currentPlayerName}'s turn. Enter column (1-7):");
				int column = GetValidColumn(board);

				int row = DropCoin(board, column, (currentPlayer == player1) ? player1Coin : player2Coin);

				// Phase 1: Check for a win
				if (CheckForWin(board, row, column))
				{
					gameWon = true;
					Console.Clear();
					DrawBoard(board);
					Console.WriteLine($"WINNER = {currentPlayerName}");

					// Phase 2: Log the game result
					LogGameResult(logFilePath, player1, player2, currentPlayerName);
				}
				else if (IsBoardFull(board))
				{
					isDraw = true;
					Console.Clear();
					DrawBoard(board);
					Console.WriteLine("DRAW");
					// Phase 2: Log the game result
					LogGameResult(logFilePath, player1, player2, "DRAW");
				}
				else
				{
					currentPlayer = (currentPlayer == player1) ? player2 : player1; // oyuncu yer degistirmesi
				}

			} while (!gameWon && !isDraw);
		}

		static void DrawBoard(char[,] board)
		{
			
			for (int i = 0; i < 6; i++)
			{
				Console.Write(i + 1 + " ");
				for (int j = 0; j < 7; j++)
				{
					Console.Write($"{board[i, j]} ");
				}
				Console.WriteLine();
			}
			Console.WriteLine("  1 2 3 4 5 6 7");

		}

		static int GetValidColumn(char[,] board)
		{
			int column;
			while (true)
			{
				if (int.TryParse(Console.ReadLine(), out column) && column >= 1 && column <= 7)
				{
					if (board[0, column - 1] == 0)
					{
						return column - 1;
					}
					else
					{
						Console.WriteLine("Column is full. Choose another column.");
					}
				}
				else
				{
					Console.WriteLine("Invalid input. Enter a number between 1 and 7.");
				}
			}
		}

		static int DropCoin(char[,] board, int column, char coin)
		{
			for (int i = 5; i >= 0; i--)
			{
				if (board[i, column] == 0)
				{
					board[i, column] = coin;
					return i;
				}
			}
			return -1; // Column is full (should not happen due to input validation)
		}

		static bool CheckForWin(char[,] board, int row, int column)
		{
			char coin = board[row, column];

			// Check horizontally
			for (int i = Math.Max(0, column - 3); i <= Math.Min(3, column + 3); i++)
			{
				if (board[row, i] == coin && board[row, i + 1] == coin && board[row, i + 2] == coin && board[row, i + 3] == coin)
				{
					return true;
				}
			}

			// Check vertically
			for (int i = Math.Max(0, row - 3); i <= Math.Min(2, row + 3); i++)
			{
				if (board[i, column] == coin && board[i + 1, column] == coin && board[i + 2, column] == coin && board[i + 3, column] == coin)
				{
					return true;
				}
			}


			// Check diagonally (from top-left to bottom-right)
			for (int i = -3; i <= 0; i++)
			{
				if (row + i >= 0 && row + i + 3 < 6 && column + i >= 0 && column + i + 3 < 7 &&
					 board[row + i, column + i] == coin && board[row + i + 1, column + i + 1] == coin &&
					 board[row + i + 2, column + i + 2] == coin && board[row + i + 3, column + i + 3] == coin)
				{
					return true;
				}
			}

			// Check diagonally (from top-right to bottom-left)
			for (int i = Math.Min(Math.Min(3, row), Math.Min(3, 6 - column)); i >= 0; i--)
			{
				if (row - i + 3 < 6 && column + i - 3 >= 0 &&
					 board[row - i, column + i] == coin && board[row - i + 1, column + i - 1] == coin &&
					 board[row - i + 2, column + i - 2] == coin && board[row - i + 3, column + i - 3] == coin)
				{
					return true;
				}
			}

			return false;
		}

		static bool IsBoardFull(char[,] board)
		{
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 7; j++)
				{
					if (board[i, j] == 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		static void LogGameResult(string logFilePath, string player1, string player2, string winner)
		{
			using (StreamWriter writer = File.AppendText(logFilePath))
			{
				string result = (winner == "DRAW") ? "Draw" : $"Winner = {winner}";
				string logEntry = $"[{DateTime.Now}] {player1} vs {player2} / {result}";
				writer.WriteLine(logEntry);
			}
		}
	}
}

// C:\Users\fatih\source\repos\FourInRowSecondVersion\bin\Debug