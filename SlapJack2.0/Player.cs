using Assignment1;
using System.Collections.Generic;

namespace SlapJackGame
{
    /// <summary>
    /// Base Player class. Contains a Hand and deals with button presses. 
    /// User and Computers will share the same class
    /// </summary>
    class Player
    {
        #region Fields

        private bool _isComputer;

        #endregion

        #region Properties

        public Hand Hand { get; set; }

        // bool for if player runs out of cards. Last time to slap in.
        public bool LastChance = false;

        // Boolean if the player is removed from the game. Better than 
        public bool RemovedFromGame = false;

        #endregion

        #region Constructors 

        /// <summary>
        /// Base constructor for the Player.
        /// </summary>
        public Player(bool _computer)
        {
            Hand = new Hand();
            SetPlayer(_computer);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Removes a card from the hand and returns the removed card to the game pile
        /// Reacts to a button press
        /// </summary>
        /// <returns>The top card from the hand</returns>
        public Card FlipCard()
        {
            return Hand.RemoveCard();
        }

        /// <summary>
        /// Reacts to a button press
        /// Will either:
        /// 1) Add the game pile to the players hand (if they were the first to slap and it was a Jack)
        /// 2) Remove one card from the Hand (if they slapped first and the card was not a Jack)
        /// 3) Do nothing (if they didn't slap first)
        /// 
        /// Arguments: 
        /// - isJack: boolean variable passed in that says if the slapped card is a jack or not
        /// - _gamePile: List of Cards (which holds all cards on the Board) passed in to add
        ///              to the player's hand
        /// </summary>
        public void Slap(bool isJack, List<Card> _gamePile)
        {
            if (isJack)
                //Shuffle hand
                Hand.Shuffle();
            //Add cards to hand
            //AddCards(_gamePile); // We do this in the Board class
            else
                //Remove card from player's hand
                Hand.RemoveCard();
        }

        /// <summary>
        /// Method that is run if the player slaps on a jack. Adds all cards currently in the game pile
        /// 
        /// Arguments: 
        /// - _gamePile: List of Cards (which holds all cards on the Board) passed in to add
        ///              to the player's hand
        /// </summary>
        public void AddCards(List<Card> _gamePile)
        {
            //Run the Add Card method in the Hand class for each item in the list
            for (int count = 0; count < _gamePile.Count; count++)
                Hand.AddCard(_gamePile[count]);
        }

        /// <summary>
        /// Sets Player as a computer or human player.
        /// </summary>
        public void SetPlayer(bool _computer)
        {
            _isComputer = _computer;
        }

        /// <summary>
        /// Get whether the Player is a computer or human player
        /// </summary>
        /// <returns></returns>
        public bool GetIsComputer()
        {
            return _isComputer;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
