// Ricky Moctezuma - Game Engine Scripting Spring 2024
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Battleship
{

    public class GameManager : MonoBehaviour
    {
        // Grid that shows the position of ships
        [SerializeField] private int[,] grid;

        // Represents where the player has fired
        private bool[,] hits;

        // Total rows and columns we have
        private int nRows;
        private int nCols;
        // Current row/column we are on
        private int current_row;
        private int current_col;
        // Correctly hit ships
        private int score;
        // Total time game has been running
        private int time;

        // Parent of all cells
        [SerializeField] Transform gridRoot;
        // Template cell used to populate the grid
        [SerializeField] GameObject cellPrefab;
        // Text object for win text
        [SerializeField] GameObject winLabel;
        // Text object for timer
        [SerializeField] TMPro.TextMeshProUGUI timeLabel;
        // Text object for score
        [SerializeField] TMPro.TextMeshProUGUI scoreLabel;

        private void Awake()
        {
            grid = RandomArray();
            // Initialize rows/cols to help us with our operations
            nRows = grid.GetLength(0);
            nCols = grid.GetLength(1);
            // Create identical 2D array to grid that is of the type bool instead of int
            hits = new bool[nRows, nCols];

            // Populate the grid using a loop
            // Needs to execute as many times to fill up the grid
            // Can figure that out by calculating rows * cols
            for (int i = 0; i < nRows * nCols; i++)
            {
                // Create an instance of the prefab and child it to the gridRoot
                Object clone = Instantiate(cellPrefab, gridRoot);
                // Add names to the cells to help differentiate the generated objects
                clone.name = string.Format("Cell{0}", i);
            }

            SelectCurrentCell();
            InvokeRepeating("IncrementTime", 1f, 1f);
        }


        Transform GetCurrentCell()
        {
            // You can figure out the child index of the cell that is a part of the grid by calculating (row*Cols) + col
            int index = (current_row * nCols) + current_col;
            // Return the child by index
            return gridRoot.GetChild(index);
        }

        void SelectCurrentCell()
        {
            // Get the current cell
            Transform cell = GetCurrentCell();
            // Set the "Cursor" image on
            Transform cursor = cell.Find("Cursor");
            cursor.gameObject.SetActive(true);
        }

        void UnselectCurrentCell()
        {
            // Get the current cell
            Transform cell = GetCurrentCell();
            // Set the "Cursor" image off
            Transform cursor = cell.Find("Cursor");
            cursor.gameObject.SetActive(false);
        }

        public void MoveHorizontal(int amt)
        {
            // Since we are moving to a new cell, we need to unselect the previous one
            UnselectCurrentCell();

            // Update the column
            current_col += amt;
            // Make sure the column stays within the bounds of the grid
            current_col = Mathf.Clamp(current_col, 0, nCols - 1);

            // Select the new cell
            SelectCurrentCell();
        }

        public void MoveVertical(int amt)
        {
            // Since we are moving to a new cell, we need to unselect the previous one
            UnselectCurrentCell();

            // Update the row
            current_row += amt;
            // Make sure the row stays within the bounds of the grid
            current_row = Mathf.Clamp(current_row, 0, nRows - 1);
            
            // Select the new cell
            SelectCurrentCell();
        }

        void ShowHit()
        {
            // Get the current cell
            Transform cell = GetCurrentCell();
            // Set the "Hit" image on
            Transform hit = cell.Find("Hit");
            hit.gameObject.SetActive(true);
        }

        void ShowMiss()
        {
            // Get the current cell
            Transform cell = GetCurrentCell();
            // Set the "Miss" image on
            Transform miss = cell.Find("Miss");
            miss.gameObject.SetActive(true);
        }

        void IncrementScore()
        {
            // Add 1 to the score
            score++;
            // Update the score label with the current score
            scoreLabel.text = string.Format("Score: {0}", score);
        }

        public void Fire()
        {
            // Checks if the cell in the hits data is true or false
            // If it's true that means we already fired a shot in the current cell, and we shouldn't do anything
            if (hits[current_row, current_col]) return;

            // Mark this cell as being fired upon
            hits[current_row, current_col] = true;

            // If this cell is a ship
            if (grid[current_row, current_col] == 1)
            {
                // Hit it, increment the score, and check if the game needs to be ended
                ShowHit();
                IncrementScore();
                TryEndGame();
            }
            // If the cell is open water
            else
            {
                // Show miss
                ShowMiss();
            }
        }

        void TryEndGame()
        {
            // Check every row
            for (int row = 0; row < nRows; row++)
            {
                // And check every column
                for (int col = 0; col < nCols; col++)
                {
                    // If a cell is not a ship then we can ignore it
                    if (grid[row, col] == 0) continue;
                    // If a cell is a ship and it hasn't been scored, then do a simple return because we haven't finished the game
                    if (hits[row, col] == false) return;
                }
            }

            // If the loop successfully completes then all ships have been destroyed and show the winLabel
            winLabel.SetActive(true);
            // Stop the timer
            CancelInvoke("IncrementTime");
        }

        void IncrementTime()
        {
            // Add 1 to the time
            time++;
            // Update the time label with current time
            // Format it mm:ss where m is the minute and s is the seconds
            // ss should always display 2 digits, mm should only display as many digits as necessary
            timeLabel.text = string.Format("{0}:{1}", time / 60, (time % 60).ToString("00"));
        }

        // MY WORK (after following tutorial)

        // Almost the same as GetCurrentCell(), but returns the cell at the specified input coordinates instead of the global current coordinates. Used to clear hits/misses in Restart()
        Transform GetCellAt(int row, int col)
        {
            // You can figure out the child index of the cell that is a part of the grid by calculating (row*Cols) + col
            int index = (row * nCols) + col;
            // Return the child by index
            return gridRoot.GetChild(index);
        }

        // Function to generate a 5x5 grid with 10 randomly placed ships
        int[,] RandomArray()
        {
            int[,] output_grid = new int[5, 5];

            int count = 0, x = 0, y = 0;
            Random rnd = new Random();

            // Run 10 times (place 10 ships total)
            while (count < 10)
            {
                // Get a set of random coordinates
                x = rnd.Next(0, 5);
                y = rnd.Next(0, 5);

                // If the generated coords already had a ship, keep generating a new pair of coords until an empty space is found
                while (output_grid[x, y] == 1)
                {
                    x = rnd.Next(0, 5);
                    y = rnd.Next(0, 5);
                }

                // Place a ship at the coordinates and increment the count
                output_grid[x, y] = 1;
                count++;

            };
            return output_grid;
        }

        // Function to reset the gamestate
        public void Restart()
        {
            // Deselect the selected cell and reset the current row and column
            UnselectCurrentCell();
            current_row = 0;
            current_col = 0;

            // Generate a new array of ship locations and a new empty hits array
            grid = RandomArray();
            hits = new bool[nRows, nCols];

            // Reset score and update label accordingly
            score = 0;
            scoreLabel.text = string.Format("Score: {0}", score);

            // Stop the timer, reset its value, update the timer label, and restart the timer
            CancelInvoke("IncrementTime");
            time = 0;
            timeLabel.text = string.Format("{0}:{1}", time / 60, (time % 60).ToString("00"));
            InvokeRepeating("IncrementTime", 1f, 1f);

            // Disable the win label in case it was active
            winLabel.SetActive(false);

            // Iterate across each cell in the whole grid and disable any hit or miss icons that were present
            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    Transform cell = GetCellAt(row, col);
                    cell.Find("Miss").gameObject.SetActive(false);
                    cell.Find("Hit").gameObject.SetActive(false);
                }
            }

            // Reset the cursor to the first cell
            SelectCurrentCell();

        }
    }
}