using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SlapJackGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        Board _board;
        string _gamePileImage = "pack://application:,,,/image/CardBack.jpg";

        #endregion

        #region Properties 

        public string GamePileImage { get { return _gamePileImage; } set { _gamePileImage = value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for the main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _board = new Board();
            CardsRemaining.Text = _board.Players.FirstOrDefault(a => !a.GetIsComputer()).Hand.Cards.Count.ToString();
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// When the user presses 'Let's Begin' on the startup screen
        /// Switches from the start up screen to the game screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Begin_Click(Object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NameTxtBox.Text))
            {
                nameLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                BeginExecute();
            }
            
        }

        private void BeginExecute()
        {
            beginButtin.Visibility = Visibility.Hidden;
            gameTittleLabel.Visibility = Visibility.Hidden;
            NameTxtBox.Visibility = Visibility.Hidden;
            nameLabel.Visibility = Visibility.Hidden;
            SlapButton.Visibility = Visibility.Visible;
            SlapButton.IsEnabled = false;
            FlipButton.Visibility = Visibility.Visible;
            CardsRemaining.Visibility = Visibility.Visible;
            CardsRemainingLabel.Visibility = Visibility.Visible;
            PlayerHand.Visibility = Visibility.Visible;
            CompHand1.Visibility = Visibility.Visible;
            CompHand2.Visibility = Visibility.Visible;
            CompHand3.Visibility = Visibility.Visible;
            playerName.Visibility = Visibility.Visible;
            playerName.Text = NameTxtBox.Text;
            SlapJack_Game.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/image/game_background.jpg")));
        }

        private void GameHander()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                foreach (var player in _board.Players)
                {
                    if (!player.GetIsComputer())
                    {
                        System.Threading.Thread.SpinWait(100000);
                    }
                }
            }
        }

        /// <summary>
        /// When the user presses the 'Flip Button' 
        /// Flips a card by removing the top card from the user's deck and adding it to the board's game pile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlipButton_Click(object sender, RoutedEventArgs e)
        {
            FlipButtonExecute();
        }

        private void FlipButtonExecute()
        {
            if (_board.Players.FirstOrDefault(a => !a.GetIsComputer()).Hand.GetSize() == 0)
            {
                MessageBoxResult outOfCards = MessageBox.Show("You're out of cards!");
            }
            else
            {
                var card = _board.Players.FirstOrDefault(a => !a.GetIsComputer()).FlipCard();
                _board.AddToGamePile(card);

                Border blackBorder = new Border
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    Width = 82,
                    Height = 120
                };
                Image cardImage = new Image
                {
                    Width = 92,
                    Height = 120
                };
                Uri imageUri = new Uri(card.GetCardPicture(), UriKind.Relative);
                BitmapImage imageBitmap = new BitmapImage(imageUri);
                cardImage.Source = imageBitmap;
                blackBorder.Child = cardImage;
                Canvas.SetTop(blackBorder, 9);
                Canvas.SetLeft(blackBorder, 0);
                GamePile.Children.Add(blackBorder);
            }

            
            

            CardsRemaining.Text = _board.Players.FirstOrDefault(a => !a.GetIsComputer()).Hand.Cards.Count.ToString();
            FlipButton.IsEnabled = false;
            SlapButton.IsEnabled = true;
        }

        /// <summary>
        /// When the user presses the 'Slap' Button
        /// Checks if the top card of the game pile is a Jack and executes depending on that card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlapButton_Click(object sender, RoutedEventArgs e)
        {
            SlapButtonExecute();
        }

        private void SlapButtonExecute()
        {
            if (!SlapButton.IsEnabled)
                return;
            _board.UserSlap();
            FlipButton.IsEnabled = true;
            SlapButton.IsEnabled = false;
            CardsRemaining.Text = _board.Players.FirstOrDefault(a => !a.GetIsComputer()).Hand.Cards.Count.ToString();
        }

        /// <summary>
        /// Redundant methods to flip (with the 'f' key) and slap (with the Space key)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlapJgame_KeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key.ToString();
            if (key == "Space")
                SlapButtonExecute();
            else if (key.ToLower() == "f")
                FlipButtonExecute();
        }

        #endregion
    }
}
