using RPGGameWPF.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RPGGameWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Random rnd = new Random();
		private GameState state;
		private Border[,] cells;
		private TextBlock[,] cellText;
		private int MapW = 20;
		private int MapH = 12;
		private bool inGame;
		private DispatcherTimer tickTimer;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void NewGame_Click(object sender, RoutedEventArgs e)
		{
			inGame = false;
			MenuPanel.Visibility = Visibility.Collapsed;
			ClassPanel.Visibility = Visibility.Visible;
			GamePanel.Visibility = Visibility.Collapsed;
        }

		private void Load_Click(object sender, RoutedEventArgs e)
		{
			//fájlból való betöltés
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Warrior_Click(object sender, RoutedEventArgs e)
		{
			StartNewGame(HeroClass.Warrior);
		}

		private void Mage_Click(object sender, RoutedEventArgs e)
		{
			StartNewGame(HeroClass.Warrior);
		}

		private void Archer_Click(object sender, RoutedEventArgs e)
		{
			StartNewGame(HeroClass.Warrior);
		}

		private void StartNewGame(HeroClass cls)
		{
			int seed = rnd.Next(1, int.MaxValue);
			var map = new Map(MapW, MapH);
			map.GenerateRandom(seed);
			
			var hero = new Hero(cls, new Position(1, 1));
			state = new GameState(map, hero);
			BuildMapVisual(); 
			ShowGame();
			StartTimer();
			RenderAll();
		}

		private void BuildMapVisual()
		{
			if (state == null) return;

			MapGrid.Rows = state.Map.Height;
			MapGrid.Columns = state.Map.Width;
			MapGrid.Children.Clear();
			cells = new Border[state.Map.Width, state.Map.Height];
			cellText = new TextBlock[state.Map.Width, state.Map.Height];
			for (int y = 0; y < state.Map.Height; y++)
			{
				for (int x = 0; x < state.Map.Width; x++)
				{
					var txt = new TextBlock
					{
						Text = "",
						FontSize = 20,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center
					};
					var border = new Border
					{ 
						BorderBrush = Brushes.Gray,
						BorderThickness = new Thickness(0.5),
						Child = txt,
						CornerRadius = new CornerRadius(4),
						Margin = new Thickness(1)

					};
					cells[x, y] = border;
					cellText[x, y] = txt; 

					MapGrid.Children.Add(border);
				}
			}
		}

		private void StartTimer()
		{
			StopTimer();

			tickTimer = new DispatcherTimer();
			tickTimer.Interval = TimeSpan.FromMilliseconds(320);
			tickTimer.Tick += tickTimer_Tick;
			tickTimer.Start();
		}

		private void StopTimer()
		{
			if (tickTimer == null) return;
			{
				tickTimer.Stop();
				tickTimer.Tick -= tickTimer_Tick;
				tickTimer = null;
			}
		}

		private void tickTimer_Tick(object? sender, EventArgs e)
		{
			//játék logika frissítése
			if (!inGame) return;
			if (tickTimer == null) return;
			if (state.IsGameOver) return;

			//EnemySteop();
			RenderAll();
		}

		private void RenderAll()
		{
			if (state == null) return;
			
			if (cells == null || cellText == null) return;

			//végigmegyünk a teljes térképen és kirajzoljuk az alap csempéket
			for (int y = 0; y < state.Map.Height; y++)
			{
				for (int x = 0; x < state.Map.Width; x++)
				{
					var tile = state.Map.Tiles[x, y].Type;
					cells[x,y].Background = tile switch
					{
						TileType.Wall => Brushes.DarkSlateGray,
						TileType.Exit => Brushes.DarkSeaGreen,
						_ => Brushes.Beige
					};
					cellText[x, y].Text = "";

				}
			}
		}
		private void BackToMenu_Click(object sender, RoutedEventArgs e)
		{
			inGame = false;
			MenuPanel.Visibility = Visibility.Visible;
			ClassPanel.Visibility = Visibility.Collapsed;
			GamePanel.Visibility = Visibility.Collapsed;
		}

		private void ShowGame()
		{
			inGame = true;
			MenuPanel.Visibility = Visibility.Collapsed;
			ClassPanel.Visibility = Visibility.Collapsed;
			GamePanel.Visibility = Visibility.Visible;
		}
	}
}