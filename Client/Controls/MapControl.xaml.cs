using DataTransfer.Dice;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.Controls
{
    public partial class MapControl : UserControl
    {
        public MapControl()
        {
            InitializeComponent();
        }

        private void OnShowDice(object sender, MouseEventArgs e)
        {
            DicePanel.Visibility = Visibility.Visible;
        }

        private async void OnHideDice(object sender, MouseEventArgs e)
        {
            const int timeToHide = 3000;
            await Task.Delay(timeToHide);
            DicePanel.Visibility = Visibility.Collapsed;
        }

        private void OnRollD4(object sender, RoutedEventArgs e)
        {
            DicePanel.Visibility = Visibility.Collapsed;

            var random = new Random();

            var diceRollDto = new DiceRollDto()
            {
                Name = "Roll D4",
                Succeeded = new List<bool>
                {
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                }
            };

            DiceRoller.Show(diceRollDto);
        }

        private void OnRollD6(object sender, RoutedEventArgs e)
        {
            DicePanel.Visibility = Visibility.Collapsed;

            var random = new Random();

            var diceRollDto = new DiceRollDto()
            {
                Name = "Roll D6",
                Succeeded = new List<bool>
                {
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                }
            };

            DiceRoller.Show(diceRollDto);
        }

        private void OnRollD8(object sender, RoutedEventArgs e)
        {
            DicePanel.Visibility = Visibility.Collapsed;

            var random = new Random();

            var diceRollDto = new DiceRollDto()
            {
                Name = "Roll D8",
                Succeeded = new List<bool>
                {
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                }
            };

            DiceRoller.Show(diceRollDto);
        }

        private void OnRollD10(object sender, RoutedEventArgs e)
        {
            DicePanel.Visibility = Visibility.Collapsed;

            var random = new Random();

            var diceRollDto = new DiceRollDto()
            {
                Name = "Roll D10",
                Succeeded = new List<bool>
                {
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                }
            };

            DiceRoller.Show(diceRollDto);
        }

        private void OnRollD12(object sender, RoutedEventArgs e)
        {
            DicePanel.Visibility = Visibility.Collapsed;

            var random = new Random();

            var diceRollDto = new DiceRollDto()
            {
                Name = "Roll D12",
                Succeeded = new List<bool>
                {
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                }
            };

            DiceRoller.Show(diceRollDto);
        }

        private void OnRollD20(object sender, RoutedEventArgs e)
        {
            DicePanel.Visibility = Visibility.Collapsed;

            var random = new Random();

            var diceRollDto = new DiceRollDto()
            {
                Name = "Roll D20",
                Succeeded = new List<bool>
                {
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                }
            };

            DiceRoller.Show(diceRollDto);
        }
    }
}