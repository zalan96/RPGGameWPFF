using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGameWPF.Models
{
	public class Map
	{
		public int Width { get; }
		public int Height { get; }
		public Tile[,] Tiles { get; }
		public Position ExitPos { get; private set; }

		public Map(int width, int height)
		{
			Width = width;
			Height = height;
			Tiles = new Tile[Width, Height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
				{
					Tiles[x, y] = new Tile(TileType.Floor);
				}
            }
        }

		public bool InBounds(Position p)
		{
			//megvizsgáljuk, hogy a pozició X koordinátája nem negatív
			//és kisebb mint a pálya szélessége
			//valamint az Y koordináta nem negatív
			//és kisebb mint a pálya magassága
			return p.X >= 0 && p.Y >= 0 && p.X < Width && p.Y < Height;
		}

		public bool IsWalkable(Position p)
		{
			//pályán belül van e
			if (!InBounds(p))
				return false;

			//ha bent van és nem fal
			return Tiles[p.X, p.Y].Type != TileType.Wall;
		}

		public void GenerateRandom(int seed)
		{
			var rnd = new Random(seed);

            //először azt csináljuk, hogya padlóvá tesszük a pályát

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
				{
					Tiles[x, y].Type = TileType.Floor;
				}
            }

            //felső és alsó sor kirajzolása
            for (int x = 0; x < Width; x++)
            {
				Tiles[x, 0].Type = TileType.Wall; //felső fal
				Tiles[x, Height -1].Type = TileType.Wall; //alsó fal
            }

            //bal és a jobb oldal kirajzolása
            for (int y = 0; y < Height; y++)
            {
				Tiles[0, y].Type = TileType.Wall; //bal fal
				Tiles[Width - 1, y].Type = TileType.Wall; //jobb fal
            }

			//belső falak meghatározása (nagyjából egyheted)
			int wallCount = (Width * Height) / 7;

			//véletleszerű belső falak lerakása
			for (int i = 0; i < wallCount; i++)
			{
				//csak a belső területen sorsolunk (nem a keretfalon)
				int x = rnd.Next(1, Width - 1);
				int y = rnd.Next(1, Height - 1);

				//nem engedjük, hogy a kezdő mezőre vagy a kijáratra fal kerüljön
				if ((x == 1 && y == 1) || (x == Width - 2 && y == Height - 2))
					continue;

				//a kiválasztott mezőt fallá alakítjuk
				Tiles[x, y].Type = TileType.Wall;
			}
				//a kijárat pozíciója (jobb alsó sarok beljebb egy mezővel
				ExitPos = new Position(Width - 2, Height - 2);
				Tiles[ExitPos.X, ExitPos.Y].Type = TileType.Exit;

				//kezdő mező is járható maradjon
				Tiles[1, 1].Type = TileType.Floor;
            }
        }
}
