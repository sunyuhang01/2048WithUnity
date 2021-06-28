using System;
using System.Collections.Generic;

namespace Console2048
{
    /// <summary>
    /// 游戏核心类
    /// </summary>
    class GameCore
    {
        private int[,] map;
        private int[] mergeArray;
        private int[] removeZeroArray;
        private List<Location> emptyLocationList; //统计空位置
        private Random random;
        private int[,] originalMap;
        public bool isChange { get; set; }
        public bool isOver { get; set; }

        public GameCore()
        {
            map = new int[4, 4];
            mergeArray = new int[4];
            removeZeroArray = new int[4];
            emptyLocationList = new List<Location>(16);
            random = new Random();
            originalMap = new int[4, 4];
            isOver = false;
        }
        public int[,] Map
        {
            get
            {
                return this.map;
            }
        }
        private void RemoveZero()
        {
            // 清空
            Array.Clear(removeZeroArray, 0, 4);
            int index = 0;
            for(int i = 0; i < mergeArray.Length; i++)
            {
                if(mergeArray[i] != 0)
                {
                    removeZeroArray[index++] = mergeArray[i];
                }
            }
            removeZeroArray.CopyTo(mergeArray, 0);

        }

        private void Merge()
        {
            RemoveZero();
            for(int i = 0; i < mergeArray.Length - 1; i++)
            {
                // 相邻相同
                if(mergeArray[i] != 0 && mergeArray[i] == mergeArray[i + 1])
                {
                    mergeArray[i] += mergeArray[i + 1];
                    mergeArray[i + 1] = 0;
                    // 积分
                    // 记录合并位置
                }
            }
            RemoveZero();
        }

        private void MoveUp()
        {
            for(int c = 0; c < map.GetLength(1); c++)
            {
                for(int r = 0; r < map.GetLength(0); r++)
                {
                    mergeArray[r] = map[r, c];
                }
                Merge();

                for(int r = 0; r < map.GetLength(0); r++)
                {
                    map[r, c] = mergeArray[r];
                }
            }
        }

        private void MoveDown()
        {
            for (int c = 0; c < map.GetLength(1); c++)
            {
                for (int r = map.GetLength(0) - 1; r >= 0; r--)
                {
                    mergeArray[3 - r] = map[r, c];
                }
                Merge();

                for (int r = map.GetLength(0) - 1; r >= 0; r--)
                {
                    map[r, c] = mergeArray[3 - r];
                }
            }
        }

        private void MoveLeft()
        {
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    mergeArray[c] = map[r, c];
                }
                Merge();

                for (int c = 0; c < map.GetLength(1); c++)
                {
                    map[r, c] = mergeArray[c];
                }
            }
        }

        private void MoveRight()
        {
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = map.GetLength(1) - 1; c >= 0; c--)
                {
                    mergeArray[3 - c] = map[r, c];
                }
                Merge();

                for (int c = map.GetLength(1) - 1; c >= 0; c--)
                {
                    map[r, c] = mergeArray[3 - c];
                }
            }
        }

        public void Move(MoveDirection direction)
        {
            isChange = false;
            // 移动前记录map --> originalMap
            Array.Copy(map, originalMap, map.Length);
            switch (direction)
            {
                case MoveDirection.Up:
                    MoveUp();
                    break;
                case MoveDirection.Down:
                    MoveDown();
                    break;
                case MoveDirection.Left:
                    MoveLeft();
                    break;
                case MoveDirection.Right:
                    MoveRight();
                    break;
            }
            // 移动后对比
            for(int r = 0; r < map.GetLength(0); r++)
            {
                for(int c = 0; c < map.GetLength(0); c++)
                {
                    if(map[r,c] != originalMap[r, c])
                    {
                        isChange = true;
                        return;
                    }
                }
            }
        }

        // 生成数字
        // 在空白位置随机产生一个2（90%）或4（10%）
        
        private void CaluculateEmpty()
        {
            // 统计空位置前先清空列表
            emptyLocationList.Clear();
            for(int r = 0; r < map.GetLength(0); r++)
            {
                for(int c = 0; c < map.GetLength(1); c++)
                {
                    if(map[r,c] == 0)
                    {
                        // 记录r c
                        emptyLocationList.Add(new Location(r, c));
                    }
                }
            }
        }
        /// <summary>
        /// 生成数字
        /// </summary>
        public void GenerateNumber(out Location? loc, out int? number)
        {
            CaluculateEmpty();
            if(emptyLocationList.Count > 0)
            {
                int randomIndex = random.Next(0, emptyLocationList.Count);
                loc = emptyLocationList[randomIndex];

                number = map[loc.Value.RIndex, loc.Value.CIndex] = random.Next(0, 10) == 1 ? 4 : 2;
            }
            else
            {
                loc = null;
                number = null;
            }
            
        }

        public void CheckGameOver()
        {
            CaluculateEmpty();
            if(emptyLocationList.Count != 0)
            {
                return;
            }
            for(int r = 0; r < map.GetLength(0); r++)
            {
                for(int c = 0; c < map.GetLength(1) - 1; c++)
                {
                    if(map[r, c] == map[r, c + 1])
                    {
                        return;
                    }
                }
            }
            for(int c = 0; c < map.GetLength(1); c++)
            {
                for(int r = 0; r < map.GetLength(0); r++)
                {
                    if(map[r,c] == map[r + 1, c])
                    {
                        return;
                    }
                }
            }
            isOver = true;

        }
    }
}
