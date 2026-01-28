using System;

namespace RPGGameWPF.Models
{
	public class GroundDrop
	{
		public Position Pos { get; set; }
		public Item Item { get; set; }
		public GroundDrop(Item item, Position pos)
		{
			Pos = pos;
			Item = item;
		}
	}

	public class Chest
	{
		public Position Pos { get; set; }
		public bool Opened { get; private set; }

		public Chest(Position pos)
		{
			Pos = pos;
			Opened = false;
		}

		public void Open()
		{
			Opened = true;
		}
	}

	public class GameState
	{
		private Hero hero;

		public Map Map { get; }
		public Hero Player { get; }
		public List<Enemy> Enemies { get; }
		public List<GroundDrop> GroundDrops { get; }
		public List<Chest> Chests { get; }
		public Quest ActiveQuest { get; }
		public int Gold { get; set; }
		public bool IsGameOver { get; set; }
		public bool IsWin { get; set; }

	public GameState(Map map, Hero player, List<Enemy> enemies, List<Chest> chests, Quest activeQuest)
		{
			Map = map;
			Player = player;
			Enemies = new List<Enemy>();
			GroundDrops = new List<GroundDrop>();
			Chests = chests;
			ActiveQuest = new Quest("Ölj meg 3 goblint", 3);
			Gold = 0;
			IsGameOver = false;
			IsWin = false;
		}

		public GameState(Map map, Hero hero)
		{
			Map = map;
			this.hero = hero;
		}
	}
}
