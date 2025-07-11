using System;
using System.Collections.Generic;

namespace MemoryCardGame.GameLogic
{
    public class MemoryGame
    {
        public class Card
        {
            public char Value { get; set; }
            public bool Visible { get; set; }
            public bool Matched { get; set; }
            public bool JustFlipped { get; set; }
        }

        public Card[,] Board { get; private set; }
        public int Score { get; private set; }
        public int WrongMoves { get; private set; }
        public int Level { get; private set; } = 1;

        private int? lastRow = null;
        private int? lastCol = null;
        private List<char> symbols;

        public MemoryGame()
        {
            StartLevel(1);
        }

        private void StartLevel(int level)
{
    Level = level;
    Score = 0;
    WrongMoves = 0;
    lastRow = null;
    lastCol = null;
    int[] allowedSquares = { 4, 16, 16, 16, 36, 36, 36, 64, 64, 64 }; 
    int totalCards = allowedSquares[Math.Min(level - 1, allowedSquares.Length - 1)];

    int totalPairs = totalCards / 2;
    symbols = new List<char>();
    char symbol = 'A';

    for (int i = 0; i < totalPairs; i++)
    {
        symbols.Add(symbol);
        symbols.Add(symbol);
        symbol++;
    }

    InitializeBoard();
}
        private void InitializeBoard()
        {
            var rand = new Random();
            for (int i = symbols.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (symbols[i], symbols[j]) = (symbols[j], symbols[i]);
            }
            int size = (int)Math.Ceiling(Math.Sqrt(symbols.Count));
            Board = new Card[size, size];
            int index = 0;

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    Board[i, j] = index < symbols.Count
                        ? new Card { Value = symbols[index++], Visible = false, Matched = false, JustFlipped = false }
                        : new Card { Value = '?', Visible = true, Matched = true };
        }
        public void FlipCard(int row, int col)
        {
            var card = Board[row, col];
            if (card.Visible || card.Matched || card.Value == '?') return;

            card.Visible = true;
            card.JustFlipped = true;

            if (lastRow == null)
            {
                lastRow = row;
                lastCol = col;
            }
            else
            {
                var lastCard = Board[lastRow.Value, lastCol.Value];
                if (lastCard.Value == card.Value)
                {
                    lastCard.Matched = true;
                    card.Matched = true;
                    Score++;
                }
                else
                {
                    WrongMoves++;
                }
                lastRow = null;
                lastCol = null;
            }
        }
        public void HideUnmatched()
        {
            foreach (var c in Board)
            {
                if (!c.Matched && c.Visible && c.JustFlipped)
                    c.Visible = false;

                c.JustFlipped = false;
            }

            if (IsLevelComplete())
                StartLevel(Level + 1);
        }
        private bool IsLevelComplete()
        {
            foreach (var card in Board)
                if (!card.Matched && card.Value != '?')
                    return false;
            return true;
        }
        public object GetBoard()
        {
            var state = new List<List<object>>();
            int size = Board.GetLength(0);
            bool gameOver = WrongMoves >= 10;

            for (int i = 0; i < size; i++)
            {
                var row = new List<object>();
                for (int j = 0; j < size; j++)
                {
                    var c = Board[i, j];
                    row.Add(new
                    {
                        value = c.Value,
                        visible = c.Visible || c.Matched
                    });
                }
                state.Add(row);
            }

            return new
            {
                board = state,
                score = Score,
                level = Level,
                wrongMoves = WrongMoves,
                gameOver = gameOver
            };
        }
    }
}
