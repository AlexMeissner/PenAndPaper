using Client.Services;
using DataTransfer.Dice;
using FontAwesome.Sharp;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static Client.Services.ServiceExtension;

namespace Client.Controls
{
    [TransistentService]
    public partial class DiceRollerControl : UserControl
    {
        private int RunningRolls = 0;

        private readonly IAudioPlayer AudioPlayer;

        public DiceRollerControl(IAudioPlayer audioPlayer)
        {
            AudioPlayer = audioPlayer;

            InitializeComponent();
        }

        public async Task Show(DiceRollResultDto diceRollResult)
        {
            if (diceRollResult.Name is null || diceRollResult.Succeeded is null)
            {
                return;
            }

            Interlocked.Increment(ref RunningRolls);

            int successes = 0;
            RolledNumberText.Text = successes.ToString();
            CharacterName.Text = diceRollResult.Name;

            DiceD4.Visibility = Visibility.Collapsed;
            DiceD6.Visibility = Visibility.Collapsed;
            DiceD8.Visibility = Visibility.Collapsed;
            DiceD10.Visibility = Visibility.Collapsed;
            DiceD12.Visibility = Visibility.Collapsed;
            DiceD20.Visibility = Visibility.Collapsed;
            Visibility = Visibility.Visible;

            UIElementCollection diceImages;

            switch (diceRollResult.Succeeded.Count)
            {
                case 4:
                    DiceD4.Visibility = Visibility.Visible;
                    diceImages = DiceD4.Children;
                    break;
                case 6:
                    DiceD6.Visibility = Visibility.Visible;
                    diceImages = DiceD6.Children;
                    break;
                case 8:
                    DiceD8.Visibility = Visibility.Visible;
                    diceImages = DiceD8.Children;
                    break;
                case 10:
                    DiceD10.Visibility = Visibility.Visible;
                    diceImages = DiceD10.Children;
                    break;
                case 12:
                    DiceD12.Visibility = Visibility.Visible;
                    diceImages = DiceD12.Children;
                    break;
                case 20:
                    DiceD20.Visibility = Visibility.Visible;
                    diceImages = DiceD20.Children;
                    break;
                default:
                    throw new Exception("Invalid number of dice roll result");
            }

            foreach (var image in diceImages)
            {
                if (image is IconImage svgAwesome)
                {
                    svgAwesome.Foreground = (Brush)new BrushConverter().ConvertFromString("#777")!;
                }
            }

            for (int index = 0; index < diceRollResult.Succeeded.Count; ++index)
            {
                successes = diceRollResult.Succeeded[index] ? successes + 1 : successes;

                if (index == diceRollResult.Succeeded.Count - 1 && (successes == 1 || successes == diceRollResult.Succeeded.Count))
                {
                    const int rollMinId = 11;
                    const int rollMaxId = 12;
                    int soundId = successes == 1 ? rollMinId : rollMaxId;
                    AudioPlayer.Play(soundId);
                }
                else
                {
                    if (diceRollResult.Succeeded[index])
                    {
                        AudioPlayer.Play(9);
                    }
                    else
                    {
                        AudioPlayer.Play(10);
                    }
                }

                if (diceImages[index] is IconImage image)
                {
                    image.Foreground = diceRollResult.Succeeded[index] ? Brushes.Green : Brushes.Red;
                    RolledNumberText.Text = successes.ToString();
                    await Task.Delay(200);
                }
            }

            const int timeToHide = 10000;
            await Task.Delay(timeToHide);

            Interlocked.Decrement(ref RunningRolls);

            if (RunningRolls == 0)
            {
                Visibility = Visibility.Collapsed;
            }
        }
    }
}
