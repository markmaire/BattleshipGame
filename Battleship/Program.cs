using BattleshipLibrary;
using BattleshipLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();
            PlayerInfoModel activePlayer = CreatePlayer("Player 1");  //usersname, shiplocations, shotgrid
            PlayerInfoModel opponent = CreatePlayer("Player 2");

            PlayerInfoModel winner = null;
            do
            {
                //display grid from player 1 on where they fired.
                DisplayShotGrid(activePlayer);

                //ask activePlayer for a shot
                //determine if it is a valid shot
                //determine shot results
                RecordPlayerShot(activePlayer, opponent);

                //determine if game is over
                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

                //if over, set activePlayer as winner
                //else swap positions (activePlayer to opponent)
                if(doesGameContinue == true)
                {
                    //Use tuple to swap positions
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {

                }
            } while (winner == null);
            IdentifyWinner(winner);
            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations to {winner.UsersName} for winning!");
            Console.WriteLine($"{winner.UsersName} took {GameLogic.GetShotCount(winner)} shots!");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            bool isValidShot = false;
            string row = "";
            int column = 0;
            do
            {
                //Ask for a shot i.e. "B2" not "B" "2"
                string shot = AskForShot();
                //Determine what row and column
                (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                isValidShot = GameLogic.ValidateSpot(activePlayer, row, column);
                if(isValidShot == false)
                {
                    Console.WriteLine("Invalid shot location. Try again.");
                }


            } while (isValidShot == false);
            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);
        }

        private static string AskForShot()
        {
            Console.Write("Please enter your shot selection: ");
            string output = Console.ReadLine();
            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;


            foreach (var gridSpot in activePlayer.ShotGrid)
            {

                if(gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($"{gridSpot.SpotLetter}{gridSpot.SpotNumber}");
                }

                else if(gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write("X");
                }

                else if(gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write("O");
                }

            }
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("+++ Welcome to BATTLESHIP +++");
            Console.WriteLine("---- Created by Mark Maire -----");
            Console.WriteLine();
        }
        

        private static PlayerInfoModel CreatePlayer(string playerTitle)  //Player 1 and Player 2
        {
            PlayerInfoModel output = new PlayerInfoModel();

            Console.WriteLine($"Player information for {playerTitle}: ");

            //ask user for their name, directs to method that will return the username.
            output.UsersName = AskForUsersName();

            //load up shot grid A1, A2...E5
            GameLogic.InitializeGrid(output);


            //ask user for their 5 ship placements
            PlaceShips(output);

            
            Console.Clear();

            return output; //playerinfomodel
        }

        private static string AskForUsersName()
        {
            Console.WriteLine("What is your name?");
            string output = Console.ReadLine();
            return output;
        }

        private static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.Write($"Where do you want to place your ship number {model.ShipLocations.Count + 1}?");
                string location = Console.ReadLine();

                bool isValidLocation = GameLogic.PlaceShip(model, location);

                if(isValidLocation == false)
                {
                    Console.WriteLine("That was not a valid location, please try again.");
                }

            } while (model.ShipLocations.Count < 5);
        }

    }
}
