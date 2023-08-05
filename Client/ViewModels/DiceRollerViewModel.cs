﻿using Client.Services;
using DataTransfer.Dice;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    [TransistentService]
    public class DiceRollerViewModel : BaseViewModel
    {
        private int RunningRolls = 0;

        private readonly IAudioPlayer _audioPlayer;

        private static readonly Color _defaultColor = (Color)ColorConverter.ConvertFromString("White");
        private static readonly Color _successColor = (Color)ColorConverter.ConvertFromString("Green");
        private static readonly Color _failColor = (Color)ColorConverter.ConvertFromString("Red");

        public string Name { get; private set; } = string.Empty;
        public string RolledNumber { get; private set; } = string.Empty;
        public Visibility Visibility { get; private set; } = Visibility.Collapsed;
        public Visibility D4Visibility { get; private set; } = Visibility.Collapsed;
        public Visibility D6Visibility { get; private set; } = Visibility.Collapsed;
        public Visibility D8Visibility { get; private set; } = Visibility.Collapsed;
        public Visibility D10Visibility { get; private set; } = Visibility.Collapsed;
        public Visibility D12Visibility { get; private set; } = Visibility.Collapsed;
        public Visibility D20Visibility { get; private set; } = Visibility.Collapsed;
        public SolidColorBrush[] Colors { get; private set; } = new SolidColorBrush[20];

        public DiceRollerViewModel(IAudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;

            for (int index = 0; index < 20; ++index)
            {
                Colors[index] = new SolidColorBrush();
            }
        }

        public async Task Show(DiceRollResultDto diceRollResult)
        {
            Interlocked.Increment(ref RunningRolls);

            int successes = 0;
            Name = diceRollResult.Name;
            RolledNumber = successes.ToString();

            D4Visibility = Visibility.Collapsed;
            D6Visibility = Visibility.Collapsed;
            D8Visibility = Visibility.Collapsed;
            D10Visibility = Visibility.Collapsed;
            D12Visibility = Visibility.Collapsed;
            D20Visibility = Visibility.Collapsed;
            Visibility = Visibility.Visible;

            switch (diceRollResult.Succeeded.Count)
            {
                case 4:
                    D4Visibility = Visibility.Visible;
                    break;
                case 6:
                    D6Visibility = Visibility.Visible;
                    break;
                case 8:
                    D8Visibility = Visibility.Visible;
                    break;
                case 10:
                    D10Visibility = Visibility.Visible;
                    break;
                case 12:
                    D12Visibility = Visibility.Visible;
                    break;
                case 20:
                    D20Visibility = Visibility.Visible;
                    break;
                default:
                    throw new Exception("Invalid number of dice roll result");
            }

            for (int i = 0; i < Colors.Length; i++)
            {
                Colors[i].Color = _defaultColor;
            }

            for (int index = 0; index < diceRollResult.Succeeded.Count; ++index)
            {
                successes = diceRollResult.Succeeded[index] ? successes + 1 : successes;

                if (index == diceRollResult.Succeeded.Count - 1 && (successes == 1 || successes == diceRollResult.Succeeded.Count))
                {
                    const int rollMinId = 11;
                    const int rollMaxId = 12;
                    int soundId = successes == 1 ? rollMinId : rollMaxId;
                    _audioPlayer.Play(soundId);
                }
                else
                {
                    if (diceRollResult.Succeeded[index])
                    {
                        _audioPlayer.Play(9);
                    }
                    else
                    {
                        _audioPlayer.Play(10);
                    }
                }

                Colors[index].Color = diceRollResult.Succeeded[index] ? _successColor : _failColor;
                RolledNumber = successes.ToString();
                await Task.Delay(200);
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