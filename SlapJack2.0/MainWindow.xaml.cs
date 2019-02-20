using Assignment1;
using System;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
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
        SpeechSynthesizer _synthesizer = new SpeechSynthesizer();

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

        #region Private Methods

        /// <summary>
        /// When the user presses 'Let's Begin' on the startup screen
        /// Switches from the start up screen to the game screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Begin_Click(Object sender, RoutedEventArgs e)
        {
            _synthesizer.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult);
            _synthesizer.Volume = 100;  // (0 - 100)
            _synthesizer.Rate = 0;     // (-10 - 10)           

            if (String.IsNullOrEmpty(NameTxtBox.Text))
            {
                NameLabel.Foreground = new SolidColorBrush(Colors.Red);
                if (VolumeYN2.IsChecked ?? false) _synthesizer.SpeakAsync("Please enter player's name");
            }
            else
            {
                BeginExecute();
            }
        }

        /// <summary>
        /// When the user clicks 'Let's Begin!'. 
        /// Sets the visibilty of various components to display the game board.
        /// </summary>
        private void BeginExecute()
        {
            AutoFlipYN.IsChecked = false;
            BeginButton.Visibility = Visibility.Collapsed;
            GameTitleLabel.Visibility = Visibility.Collapsed;
            NameTxtBox.Visibility = Visibility.Collapsed;
            NameLabel.Visibility = Visibility.Collapsed;
            QuestionMark.Visibility = Visibility.Collapsed;
            SlapButton.Visibility = Visibility.Visible;
            FlipButton.Visibility = Visibility.Visible;
            CardsRemaining.Visibility = Visibility.Visible;
            CardsRemainingLabel.Visibility = Visibility.Visible;
            PlayerHand.Visibility = Visibility.Visible;
            CompHand1.Visibility = Visibility.Visible;
            CompHand2.Visibility = Visibility.Visible;
            CompHand3.Visibility = Visibility.Visible;
            PlayerName.Visibility = Visibility.Visible;
            QuestionMark2.Visibility = Visibility.Visible;
            AutoFlipYN.Visibility = Visibility.Visible;
            Rect2.Visibility = Visibility.Visible;
            Rect3.Visibility = Visibility.Visible;
            VolumeYN2.Visibility = Visibility.Visible;
            PlayerActiveHand.Visibility = Visibility.Visible;
            PlayerName.Content = NameTxtBox.Text;
            SlapJack_Game.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/image/game_background.jpg")));
            SlapButton.IsEnabled = false;

            SoundPlayer player = new SoundPlayer(SlapJack2._0.Properties.Resources.ahem_x);
            player.Play();

            System.Threading.Thread.Sleep(2000);
            if (VolumeYN2.IsChecked ?? false)
            {
                _synthesizer.SpeakAsync("Welcome " + NameTxtBox.Text);
                _synthesizer.SpeakAsync("Best of luck");
                _synthesizer.SpeakAsync("Flip the card to Begin and get ready to slap.");
            }
        }

        /// <summary>
        /// Handles the automation of the computers
        /// </summary>
        private void GameHandler()
        {
            var card = _player.FlipCard();

            if (_player.Hand.GetSize() == 0)
                _player.LastChance = true;
            else
                _player.LastChance = false;

            switch (_board.Players.IndexOf(_player))
            {
                case 1:
                    CompHand1.Visibility = _player.LastChance ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case 2:
                    CompHand2.Visibility = _player.LastChance ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case 3:
                    CompHand3.Visibility = _player.LastChance ? Visibility.Collapsed : Visibility.Visible;
                    break;
                default:
                    break;
            }

            Application.Current.Dispatcher.Invoke(delegate
            {
                AddToGamePile(card);
            });
            if (SlapButton.IsEnabled == false)
                SlapButton.IsEnabled = true;
        }

        /// <summary>
        /// When the user presses the 'Flip Button' 
        /// Flips a card by removing the top card from the user's deck and adding it to the board's game pile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FlipButton_Click(object sender, RoutedEventArgs e)
        {
            if (!FlipButton.IsVisible) return;
            //if (AutoFlipYN.IsChecked ?? false)
            //    return;
            PlayerActiveHand.Visibility = Visibility.Collapsed;
            FlipButtonExecute();
            if (SlapButton.IsEnabled == false)
                SlapButton.IsEnabled = true;

            CardsRemaining.Text = _board.Players.FirstOrDefault(a => !a.GetIsComputer()).Hand.Cards.Count.ToString();
            FlipButton.IsEnabled = false;
            SlapButton.IsEnabled = true;
            
            // If the player is a computer and has Any cards in their hand
            foreach (var player in _board.Players.Where(a => a.GetIsComputer() && a.Hand.Cards.Any() && !a.RemovedFromGame))
            {
                switch (_board.Players.IndexOf(player))
                {
                    case 1:
                        Comp1ActiveHand.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        Comp2ActiveHand.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        Comp3ActiveHand.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
                //Simulate the computer slap
                await Task.Delay(new Random().Next(500, 1000)).ContinueWith(t => _board.ComputerSlap(_player));

                if (!_board.GamePile.Any())
                    GamePile.Children.Clear();
                CardsRemaining.Text = _board.Players.FirstOrDefault(a => !a.GetIsComputer()).Hand.Cards.Count.ToString();

                _player = player;
                await Task.Delay(2000);
                GameHandler();
                switch (_board.Players.IndexOf(player))
                {
                    case 1:
                        Comp1ActiveHand.Visibility = Visibility.Collapsed;
                        break;
                    case 2:
                        Comp2ActiveHand.Visibility = Visibility.Collapsed;
                        break;
                    case 3:
                        Comp3ActiveHand.Visibility = Visibility.Collapsed;
                        break;
                    default:
                        break;
                }
            }

            PlayerActiveHand.Visibility = !_board.Players.FirstOrDefault(a => !a.GetIsComputer()).LastChance ? Visibility.Visible : Visibility.Collapsed;

            //Simulate the computer slap after the last computer has gone
            await Task.Delay(new Random().Next(500, 1000)).ContinueWith(t => _board.ComputerSlap(_player));
            if (!_board.GamePile.Any())
                GamePile.Children.Clear();

            /*
            if (_board.Players.FirstOrDefault(a => !a.GetIsComputer()).LastChance && _board.GamePile.Any())
            {
                EndOfGamePopupWindow(false);
                return;
            }
            */

            if (AutoFlipYN.IsChecked ?? false)
            {
                await Task.Delay(2000);
                FlipButton_Click(sender, e);
            }
            else
                FlipButton.IsEnabled = true;
        }

        /// <summary>
        /// Executes the Flip Button
        /// </summary>
        private void FlipButtonExecute()
        {
            //If the user is out of cards, they lose, no only if they didn't get the slap
            if (_board.Players.Any(a => !a.GetIsComputer() && a.RemovedFromGame))
                EndOfGamePopupWindow(false);

            //User flips card
            if (_board.Players.FirstOrDefault(a => !a.GetIsComputer()).Hand.Cards.Count != 0)
            {
                var card = _board.Players.FirstOrDefault(a => !a.GetIsComputer()).FlipCard();
                Application.Current.Dispatcher.Invoke(delegate
                {
                    AddToGamePile(card);
                });
            }

            //Check if user is out of cards
            OutOfCardsCheckUser();
        }

        /// <summary>
        /// Method that checks if the user is out of cards in their hand. If they are,
        /// Their Last Chance is set to true
        /// </summary>
        /// <param name="currentPlayer"></param>
        private void OutOfCardsCheckUser()
        {
            if (_board.Players.FirstOrDefault(a => !a.GetIsComputer()).Hand.GetSize() == 0)
            {
                PlayerHand.Visibility = Visibility.Collapsed;

                _synthesizer.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult);
                _synthesizer.Volume = 100;  // (0 - 100)
                _synthesizer.Rate = 0;     // (-10 - 10)

                if (VolumeYN2.IsChecked ?? false) _synthesizer.SpeakAsync("You're out of cards!");

                MessageBoxResult outOfCards = MessageBox.Show("You're out of cards! You are on your last chance. If you fail another slap, you'll lose!");
                _board.Players.FirstOrDefault(a => !a.GetIsComputer()).LastChance = true;

                FlipButton.IsEnabled = false;
            }
            //If the user slaps successfully on a last chance, restore card image and change last chance to false
            else
            {
                PlayerHand.Visibility = Visibility.Visible;
                _board.Players.FirstOrDefault(a => !a.GetIsComputer()).LastChance = false;
            }
        }

        /// <summary>
        /// Adds a card to the middle game pile
        /// </summary>
        /// <param name="card"></param>
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
            _synthesizer.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult);
            _synthesizer.Volume = 100;  // (0 - 100)
            _synthesizer.Rate = 0;     // (-10 - 10)

            if (VolumeYN2.IsChecked ?? false) _synthesizer.SpeakAsync("Slapped");

            SlapButtonExecute();

            //Check if user is out of cards
            OutOfCardsCheckUser();

            //Check if user is the last one standing
            if (!_board.Players.Any(a => !a.RemovedFromGame && a.GetIsComputer()))
            {
                EndOfGamePopupWindow(true);
                return;
            }

            if (_board.GamePile.Count == 0)
                SlapButton.IsEnabled = false;

            SlapJack_Game.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, (NoArgDelegate)delegate { });
            await Task.Delay(1000);
        }

        /// <summary>
        /// Executes the Slap Button
        /// </summary>
        private void SlapButtonExecute()
        {
            /// bool to test if player slapped on jack
            bool RightSlap;
            if (!SlapButton.IsEnabled)
                return;

            RightSlap = _board.UserSlap();

            //If user is on last chance and failed userSlap, game is over
            if (!RightSlap && _board.Players.FirstOrDefault(a => !a.GetIsComputer()).LastChance)
                EndOfGamePopupWindow(false);


            if (!_board.GamePile.Any())
                GamePile.Children.Clear();

            


            CardsRemaining.Text = _board.Players.FirstOrDefault(a => !a.GetIsComputer()).Hand.Cards.Count.ToString();

            foreach (var player in _board.Players.Where(a => !a.RemovedFromGame))
                if (player.LastChance == true)
                    player.RemovedFromGame = true;

            ///if slapped on something besides jack disable slap button
            if (!RightSlap)
                SlapButton.IsEnabled = false;
        }

        /// <summary>
        /// Redundant methods to flip (with the 'f' key) and slap (with the Space key)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlapJgame_KeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key.ToString();
            if (key == "Space" && SlapButton.IsVisible && SlapButton.IsEnabled)
                SlapButtonExecute();
            else if (key.ToLower() == "f" && FlipButton.IsVisible && FlipButton.IsEnabled)
            {
                FlipButton.IsEnabled = false;
                FlipButtonExecute();
            }
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

        /// <summary>
        /// Reacts to the 'Automatically Flip?' button. Disables the Flip button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoFlipYN_Checked(object sender, RoutedEventArgs e)
        {
            if (AutoFlipYN.IsChecked ?? false)
                if (FlipButton.IsEnabled)
                {
                    FlipButton.IsEnabled = false;
                    FlipButton_Click(sender, e);
                }
                else // This is needed twice as the above command won't disable the button if you click 
                    FlipButton.IsEnabled = false;
            else
                FlipButton.IsEnabled = true;
        }

        /// <summary>
        /// Pop up window to either start a new game or exit the program
        /// </summary>
        private void EndOfGamePopupWindow(bool winnerYN)
        {
            if (winnerYN)
                MessageBox.Show("You have won!");
            else
                MessageBox.Show("You're out of cards!");

            if (MessageBox.Show("Would you like to play again?", "End of Game", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _board = new Board();
                CardsRemaining.Text = _board.Players.FirstOrDefault(a => !a.GetIsComputer()).Hand.Cards.Count.ToString();
                BeginExecute();
            }
            else
            {
                Close();
            }
        }

        private void VolumeYN_Checked(object sender, RoutedEventArgs e)
        {
            if (_board != null)
                _board.VolumeYN = VolumeYN2.IsChecked ?? false;
        }
        #endregion
    }
}
