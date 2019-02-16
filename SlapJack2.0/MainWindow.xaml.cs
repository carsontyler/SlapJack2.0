using Assignment1;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        private Board _board;
        string _gamePileImage = "pack://application:,,,/image/CardBack.jpg";
        private delegate void NoArgDelegate();
        private Player _player;

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
            NameLabel.Background.Opacity = 0.5;
            GameTitleLabel.Background.Opacity = 0.5;
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
                NameLabel.Foreground = new SolidColorBrush(Colors.Red);
            else
                BeginExecute();
        }

        private void BeginExecute()
        {
            BeginButton.Visibility = Visibility.Hidden;
            GameTitleLabel.Visibility = Visibility.Hidden;
            NameTxtBox.Visibility = Visibility.Hidden;
            NameLabel.Visibility = Visibility.Hidden;
            SlapButton.Visibility = Visibility.Visible;
            SlapButton.IsEnabled = false;
            FlipButton.Visibility = Visibility.Visible;
            CardsRemaining.Visibility = Visibility.Visible;
            CardsRemainingLabel.Visibility = Visibility.Visible;
            PlayerHand.Visibility = Visibility.Visible;
            CompHand1.Visibility = Visibility.Visible;
            CompHand2.Visibility = Visibility.Visible;
            CompHand3.Visibility = Visibility.Visible;
            PlayerName.Visibility = Visibility.Visible;
            PlayerName.Content = NameTxtBox.Text;
            SlapJack_Game.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/image/game_background.jpg")));
            QuestionMark.Visibility = Visibility.Hidden;
            QuestionMark2.Visibility = Visibility.Visible;
        }

        private void GameHander()
        {
            var card = _player.FlipCard();
            Application.Current.Dispatcher.Invoke(delegate
            {
                AddToGamePile(card);
            });
            if(SlapButton.IsEnabled == false)
            {
                SlapButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// When the user presses the 'Flip Button' 
        /// Flips a card by removing the top card from the user's deck and adding it to the board's game pile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FlipButton_Click(object sender, RoutedEventArgs e)
        {
            FlipButtonExecute();
            if (SlapButton.IsEnabled == false)
            {
                SlapButton.IsEnabled = true;
            }
            CardsRemaining.Text = _board.Players.FirstOrDefault(a => !a.GetIsComputer()).Hand.Cards.Count.ToString();
            FlipButton.IsEnabled = false;
            SlapButton.IsEnabled = true;
            
            // If the player is a computer and has Any cards in their hand
            foreach (var player in _board.Players.Where(a => a.GetIsComputer() && a.Hand.Cards.Any()))
            {
                _player = player;
                await Task.Delay(2000);
                GameHander();
            }

            FlipButton.IsEnabled = true;
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
                Application.Current.Dispatcher.Invoke(delegate
                {
                    AddToGamePile(card);
                });
            }
        }

        private void AddToGamePile(Card card)
        {
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
            SlapJack_Game.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, (NoArgDelegate)delegate { });
        }

        /// <summary>
        /// When the user presses the 'Slap' Button
        /// Checks if the top card of the game pile is a Jack and executes depending on that card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SlapButton_Click(object sender, RoutedEventArgs e)
        {
            SlapButtonExecute();
            if(_board.GamePile.Count == 0)
            {
                SlapButton.IsEnabled = false;
            }
            SlapJack_Game.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, (NoArgDelegate)delegate { });
            await Task.Delay(1000);
        }

        private void SlapButtonExecute()
        {
            bool slap;
            if (!SlapButton.IsEnabled)
                return;
           slap = _board.UserSlap();
            if (!_board.GamePile.Any())
                GamePile.Children.Clear();
            CardsRemaining.Text = _board.Players.FirstOrDefault(a => !a.GetIsComputer()).Hand.Cards.Count.ToString();
            if(!slap)
            {
                SlapButton.IsEnabled = false;
            }
               
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

        /// <summary>
        /// Opens a new help window with instructions on how to play the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuestionMark_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var instructions = "Welcome to the Slap Jack Game! \nHOW TO WIN: \n    Be the last player to run out of cards!" +
                "\n\nCONTROLS: \n    There are 3 different ways to slap the game pile: click the 'Slap' button, click the game pile of cards, or " +
                "press the Spacebar. Upon winning (or losing), you will be prompted to either start a new game or exit the app." +
                "\n\nHOW TO PLAY: \n    1. Enter your name in the text box and press 'Let's Begin!'" +
                "\n    2. Once the cards are dealt out, cards will automatically be dealt out, starting with you (the bottom card) and moving " +
                "in a clockwise fashion. \n    3. When a Jack (of any suit) is flipped onto the center game pile, be the first one to slap the pile! " +
                "\n    4. The first player to slap the pile collects the game pile and shuffles them into their hand. \n    5. Continue flipping " +
                "and slapping on Jacks until you run out of cards. The last player to run out of cards wins the game! " +
                "\n    6. If you slap the deck when a Jack is not flipped, you must choose a random card from your hand and pass it to the player " +
                "on your left. \n     7. If you run out of cards, you are still in the game until a Jack is dealt. If a Jack is dealt and you " +
                "are not the first player to slap it, you lose! If you do slap the Jack, you collect the game pile and continue on!";
            MessageBox.Show(instructions, "How to Play");
        }

        #endregion
    }
}
