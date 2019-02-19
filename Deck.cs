using Assignment1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SlapJackGame
{
    /// <summary>
    /// Base Deck class. Used by the Board and is the base deck for the game
    /// </summary>
    class Deck
    {
        #region Fields

        #endregion

        #region Properties

        internal List<Card> Cards { get; set; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Base Constructor with no parameters
        /// </summary>
        public Deck()
        {
            GenerateDeck();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the deck and adds cards to the deck
        /// </summary>
        private void GenerateDeck()
        {
            Cards = new List<Card>();
            for (int i = 0; i <= 3; i++)
                for (int j = 1; j <= 13; j++)
                    Cards.Add(new Card((Card.Suit)i, j));
            Shuffle();
        }

        /// <summary>
        /// Shuffles the existing deck
        /// </summary>
        public void Shuffle()
        {
            Cards = Cards.OrderBy(a => Guid.NewGuid()).ToList();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
