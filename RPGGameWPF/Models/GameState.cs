using System;
using System.Security.Cryptography.X509Certificates;

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
		public Enemy? GetEnemyAt(Position pos)
		{
			for (int i = 0; i < Enemies.Count; i++)
			{
				if (Enemies[i].IsAlive && Enemies[i].Pos.X == pos.X && Enemies[i].Pos.Y == pos.Y) return Enemies[i];
			}
		}

		public GroundDrop? GetDropAt(Position p)
		{
			for (int i = 0; i < GroundDrops.Count; i++)
			{
				if (GroundDrops[i].Pos.X == p.X && GroundDrops[i].Pos.Y == p.Y) return GroundDrops[i];
			}
			return null;
		}

		public Chest? GetChestAt(Position p)
		{
			for (int i = 0; i < Chests.Count; i++)
			{
				if (Chests[i].Pos.X == p.X && Chests[i].Pos.Y == p.Y) return Chests[i];
			}
			return null;
		}
	}
}
