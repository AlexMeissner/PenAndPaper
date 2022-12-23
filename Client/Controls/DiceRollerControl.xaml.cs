using DataTransfer.Dice;
using FontAwesome5;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client.Controls
{
    public partial class DiceRollerControl : UserControl
    {
        private int RunningRolls = 0;

        public DiceRollerControl()
        {
            InitializeComponent();
        }

        public async void Show(DiceRollDto diceRollDto)
        {
            Interlocked.Increment(ref RunningRolls);

            int successes = 0;
            RolledNumberText.Text = successes.ToString();
            CharacterName.Text = diceRollDto.Name;

            DiceD4.Visibility = Visibility.Collapsed;
            DiceD6.Visibility = Visibility.Collapsed;
            DiceD8.Visibility = Visibility.Collapsed;
            DiceD10.Visibility = Visibility.Collapsed;
            DiceD12.Visibility = Visibility.Collapsed;
            DiceD20.Visibility = Visibility.Collapsed;
            Visibility = Visibility.Visible;

            const int totalTime = 2000;
            int timeGap = totalTime / diceRollDto.Succeeded.Count;

            UIElementCollection diceImages;

            switch (diceRollDto.Succeeded.Count)
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
                if (image is SvgAwesome svgAwesome)
                {
                    svgAwesome.Foreground = (Brush)new BrushConverter().ConvertFromString("#555")!;
                }
            }

            for (int index = 0; index < diceRollDto.Succeeded.Count; ++index)
            {
                if (diceImages[index] is SvgAwesome image)
                {
                    image.Foreground = diceRollDto.Succeeded[index] ? Brushes.Green : Brushes.Red;
                    successes = diceRollDto.Succeeded[index] ? successes + 1 : successes;
                    RolledNumberText.Text = successes.ToString();
                    await Task.Delay(timeGap);
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
