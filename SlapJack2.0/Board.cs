using Assignment1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SlapJackGame
{
    /// <summary>
    /// Main game handler. Will hold Players, Deck, and the game pile
    /// </summary>
    class Board
    {
        #region Fields

        private List<Card> _gamePile = new List<Card>();

        #endregion

        #region Properties

        public Deck Deck { get; set; }
        public List<Player> Players { get; set; }
        public List<Card> GamePile { get { return _gamePile; } set { _gamePile = value; } }

        #endregion

        #region Constructors 

        /// <summary>
        /// Base constructor
        /// </summary>
        public Board()
        {
            Deck = new Deck();
            Players = new List<Player>();
            GamePile = new List<Card>();
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                    Players.Add(new Player(false));
                else
                    Players.Add(new Player(true));
            }
            Deal();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deal each card in the deck to each player evenly, until the deck is gone
        /// </summary>
        public void Deal()
        {
            int i = 0;
            foreach (var card in Deck.Cards)
            {
                Players.ElementAt(i).Hand.AddCard(card);
                i++;
                if (i == 4)
                    i = 0;
            }
            // Clear the deck
            Deck.Cards.RemoveAll(a => a != null);
        }

        /// <summary>
        /// When a player slaps a jack, awards that player the cards, and then clear the game pile.
        /// </summary>
        public void ClearGamePile(Player winner)
        {
            //Award the cards
            winner.AddCards(GamePile);

            //Clear the game pile
            GamePile.RemoveAll(a => a != null);
        }

        /// <summary>
        /// Add a card to the game pile, such as when a player flips their card
        /// </summary>
        /// <param name="card">The card to be added</param>
        public void AddToGamePile(Card card)
        {
            GamePile.Add(card);
        }

        /// <summary>
        /// Check if the most recently added card is a Jack
        /// </summary>
        /// <returns>Boolean if the most recently added card is a Jack</returns>
        public bool CheckCardAsJack()
        {
            return GamePile.ElementAt(GamePile.Count).CardNum == 11;
        }

        /// <summary>
        /// Execute on a User Slap Button press. May be used for a computer slap
        /// </summary>
        public void UserSlap()
        {
            if (GamePile.ElementAt(GamePile.Count - 1).CardNum == 11)
            {
                Players.FirstOrDefault(a => !a.GetIsComputer()).Slap(true, GamePile);
                ClearGamePile(Players.FirstOrDefault(a => !a.GetIsComputer()));
            }
            else
            {
                MessageBox.Show("You slapped on a card that's not a Jack. You lose one card!", "Incorrect Slap", MessageBoxButton.OK);
                var card = Players.FirstOrDefault(a => !a.GetIsComputer()).Hand.RemoveCard();
                Players.FirstOrDefault(a => a.GetIsComputer()).Hand.AddCard(card);
            }
        }

        /// <summary>
        /// Method that checks for a computer's slap. If the card is a Jack
        /// </summary>
        /// <param name="temp"></param>
        public void ComputerSlap(Player currentComputer)
        {
            //Check for Jack
            if (GamePile.Any() && GamePile.ElementAt(GamePile.Count - 1).CardNum == 11)
            {
                //Make computer wait, and then check to see if pile still exists
                //If it does, computer gets the slap

                if (!(GamePile.Count == 0))
                {
                    //currentComputer.Slap(true, GamePile);
                    MessageBox.Show("Computer got the slap!");
                    currentComputer.Slap(true, GamePile);
                    ClearGamePile(currentComputer);
                }
            }
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
