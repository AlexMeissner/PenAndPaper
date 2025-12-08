using DataTransfer.Monster;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MonsterManager.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public ObservableCollection<MonsterViewModel> Monsters { get; set; } = [];
        public MonsterViewModel? Monster { get; set; }
        public Visibility OverviewVisibility => Monster is null ? Visibility.Visible : Visibility.Collapsed;
        public Visibility EditVisibility => Monster is null ? Visibility.Collapsed : Visibility.Visible;
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddMonsterCommand { get; }
        public ICommand AddImageCommand { get; }

        public static IEnumerable<SizeCategory> SoundTypeValues
        {
            get
            {
                return Enum.GetValues(typeof(SizeCategory)).Cast<SizeCategory>();
            }
        }

        private static readonly string _filePath = Assembly.GetEntryAssembly()!.Location + "../../../../../../Backend/DungeonsAndDragons/Monsters.json";

        public MainViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            EditCommand = new RelayCommandWithParameter(Edit);
            DeleteCommand = new RelayCommandWithParameter(Delete);
            AddMonsterCommand = new RelayCommand(AddMonster);
            AddImageCommand = new RelayCommand(AddImage);

            Load();
        }

        public void Save()
        {
            try
            {
                var serializedMonsters = JsonSerializer.Serialize(Monsters);
                File.WriteAllText(_filePath, serializedMonsters);

                if (Monster is not null && !Monsters.Contains(Monster))
                {
                    Monsters.Add(Monster);
                    //Monsters.OrderBy(m => m.Name);
                }

                Monster = null;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Fehler beim Speichern");
            }
        }

        public void Cancel()
        {
            Load();
            Monster = null;
        }

        public void Edit(object? sender)
        {
            if (sender is MonsterViewModel monster)
            {
                Monster = monster;
            }
        }

        public void Delete(object? sender)
        {
            if (sender is MonsterViewModel monster)
            {
                Monsters.Remove(monster);
            }
        }

        public void AddMonster()
        {
            Monster = new MonsterViewModel();
        }

        private void Load()
        {
            var serializedMonsters = File.ReadAllText(_filePath);
            var monsters = JsonSerializer.Deserialize<List<MonsterViewModel>>(serializedMonsters) ?? throw new JsonException("Could not parse monster data");

            Monsters.Clear();

            foreach (var monster in monsters)
            {
                Monsters.Add(monster);
            }
        }

        public void AddImage()
        {
            if (Monster is null) return;

            var fileDialog = new OpenFileDialog()
            {
                Filter = "Bilder (*.png;*.jpeg)|*.png;*.jpeg"
            };

            if (fileDialog.ShowDialog() == true)
            {
                var uri = new Uri(fileDialog.FileName);
                var originalImage = new BitmapImage(uri);

                Monster.Image = GetScaledDownImageData(originalImage);
            }
        }

        private byte[] GetScaledDownImageData(BitmapImage originalImage)
        {
            var minDimension = Math.Min(originalImage.Width, originalImage.Height);

            const double maxDimension = 100.0;
            var scalingFactor = minDimension / maxDimension;

            var scaledImage = new BitmapImage();
            scaledImage.BeginInit();
            scaledImage.UriSource = originalImage.UriSource;
            scaledImage.DecodePixelWidth = (int)(originalImage.Width / scalingFactor);
            scaledImage.DecodePixelHeight = (int)(originalImage.Height / scalingFactor);
            scaledImage.EndInit();

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(scaledImage));

            using MemoryStream stream = new();
            encoder.Save(stream);
            return stream.ToArray();
        }
    }
}
