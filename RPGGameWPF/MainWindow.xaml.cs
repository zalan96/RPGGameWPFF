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

			SpawnEnemies(state, seed);
			SpawnChestAndDrops(state, seed);
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

			//láda 
			for (int i = 0; i < state.Chests.Count; i++)
			{
				var chest = state.Chests[i];

				if(!chest.Opened)
					cellText[chest.Pos.X, chest.Pos.Y].Text = "📦";
			}

			for (int i = 0; i < state.GroundDrops.Count; i++)
			{
				var d = state.GroundDrops[i];

				string icon = d.Item.Type switch
				{
					ItemType.Potion => "🧪",
					ItemType.Weapon => "⚔️",
					ItemType.Armor => "🛡️",
					_ => "❓"
				};
			}

			cellText[state.Player.Pos.X, state.Player.Pos.Y].Text = "🙎‍";

			for (int i = 0; i < state.Enemies.Count; i++)
			{
				var en = state.Enemies[i];

				if (!en.IsAlive) continue;

				cellText[en.Pos.X, en.Pos.Y].Text = en.Type == EnemyType.Goblin ? "👹" : "💀";
			}

			cellText[state.Player.Pos.X, state.Player.Pos.Y].Text = "🙎‍";
		}
		private void SpawnChestAndDrops(GameState st, int seed)
		{
			//láda és földön lévő tárgyak megjelenítése
			var r = new Random(seed + 999);

			int chestCount = 4;
			for (int i = 0; i < chestCount; i++)
			{
				Position p;
				do
				{
					p = new Position(r.Next(1, st.Map.Width - 1), r.Next(1, st.Map.Height - 1));
				}
				while (!st.Map.IsWalkable(p) || (p.X == 1 && p.Y == 1) || (p.X == st.Map.ExitPos.X & p.Y == st.Map.ExitPos.Y || st.GetEnemyAt(p) != null));
				st.Chests.Add(new Chest(p));
			}
			int potions = 3; 
			for (int i = 0; i < potions; i++)
			{
				Position p;
				do
				{
					p = new Position(r.Next(1, st.Map.Width - 1), r.Next(1, st.Map.Height - 1));
				}
				while (!st.Map.IsWalkable(p) || (p.X == 1 && p.Y == 1));
				st.GroundDrops.Add(new GroundDrop(new PotionItem("Potion", ItemRarity.Common, 12), p));
			}
			
		}

		private void SpawnEnemies(GameState st, int seed)
		{
			var r = new Random(seed + 123);
			int count = 3;

            for (int i = 0; i < count; i++)
            {
				EnemyType type = (r.NextDouble() < 0.65) ? EnemyType.Goblin : EnemyType.Skeleton;

				Position p;
				do
				{
					p = new Position(r.Next(1, st.Map.Width - 1), r.Next(1, st.Map.Height - 1));
				} while (!st.Map.IsWalkable(p) || (p.X == 1 && p.Y == 1) || (p.X == st.Map.ExitPos.X & p.Y == st.Map.ExitPos.Y));
				st.Enemies.Add(new Enemy(type, p));
			}
        }

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (!inGame) return;
			if (state == null) return;
			if (state.IsGameOver) return;
			
			if (e.Key == Key.I)
			{
				return;
			}

			if(e.Key == Key.S)
			{
				return;
			}

			Direction? dir = e.Key switch
			{
				Key.Up => Direction.Up,
				Key.Down => Direction.Down,
				Key.Left => Direction.Left,
				Key.Right => Direction.Right,
				_ => null
			};

			if (dir == null) return;

			PlayerMove(dir.Value);
			RenderAll();
		}

		private void PlayerMove(Direction dir)
		{
			if (state == null) return;
			var (dx, dy) = dir switch
			{
				Direction.Up => (0, -1),
				Direction.Down => (0, 1),
				Direction.Left => (-1, 0),
				_ => (1, 0)
				
			};
			var next = state.Player.Pos.Add(dx, dy);

			if (!state.Map.IsWalkable(next))
			{
				return;
			}
			state.Player.Pos = next;
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