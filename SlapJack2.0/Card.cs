namespace Assignment1
{
    /// <summary>
    /// Base Card class. Used to create each card
    /// </summary>
    class Card
    {
        /// <summary>
        /// Containing each valid suit in a enum
        /// </summary>
        public enum Suit { Club, Spade, Heart, Diamond }

        #region Fields

        // Keep suit a private field as it is not needed anywhere but here
        private readonly Suit _suit;

        #endregion

        #region Properties 

        public int CardNum { get; set; }
        public string CardFaceImg { get; set; }
        public string CardBackImg { get; } = "Cards/CardBack.jpg";
        public bool FaceUp { get; set; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Constructor for the Card with 2 parameters. 
        /// Creates the card and assigns values
        /// Fetches the images
        /// </summary>
        /// <param name="suit">The suit to be assigned</param>
        /// <param name="cardNum">The card number to be assigned</param>
        public Card(Suit suit, int cardNum)
        {
            FaceUp = true;
            _suit = suit;
            CardNum = cardNum;
            CardFaceImg = "/image/" + (int)_suit + "-" + CardNum + ".jpg";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retries the card image depending on if the card is Face Up (suit and number) 
        /// or face down (back of card)
        /// </summary>
        /// <returns></returns>
        public string GetCardPicture()
        {
            return FaceUp ? CardFaceImg : CardBackImg;
        }

        #endregion 
    }
}