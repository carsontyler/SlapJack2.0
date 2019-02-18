using Assignment1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SlapJackGame
{
    /// <summary>
    /// Base Hand class. Contains a list of Cards for each Player.
    /// Deals with hand manipulation (removal, addition, shuffle)
    /// </summary>
    class Hand
    {
        #region Fields

        private int _size;

        #endregion

        #region Properties

        public List<Card> Cards { get; set; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Base constructor for Hand class
        /// </summary>
        public Hand()
        {
            Cards = new List<Card>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shuffles the existing class
        /// </summary>
        public void Shuffle()
        {
            Cards = Cards.OrderBy(a => Guid.NewGuid()).ToList();
        }

        /// <summary>
        /// Sets _size to List
        /// </summary>
        public void SetSize()
        {
            _size = Cards.Count;
        }

        public int GetSize()
        {
            return _size;
        }

        /// <summary>
        /// Remove a card from the existing Hand
        /// </summary>
        /// <returns>The card that was removed to add to the game pile</returns>
        public Card RemoveCard()
        {
            var returnCard = Cards.ElementAt(0);
            Cards.RemoveAt(0);
            _size--;
            return returnCard;
        }

        /// <summary>
        /// Adds a card to the existing Hand
        /// </summary>
        /// <param name="card">The card to be added</param>
        public void AddCard(Card card)
        {
            Cards.Add(card);
            _size++;
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
