using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public Sprite UnopenedBlockSprite;
        public Sprite OpenedBlockSprite;
        public Sprite FlaggedBlockSprite;
        public Sprite MinedBlockSprite;
        public GameObject BlockPrefab;
        public Transform BlockHolder;
        public int BoardValue;
        public int NumberOfMines;

        public Camera Camera;

        public Vector3 shiftAfterGeneration;
        public Vector2 blockExtent;

        public float xShift;
        public float yShift;

        public Block[] blocks;

        private List<Block> BlocksVisited;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            blocks = new Block[BoardValue * BoardValue];
            BlocksVisited = new List<Block>();
            GenerateBoard();
            BlockHolder.transform.position = new Vector3(shiftAfterGeneration.x, shiftAfterGeneration.y, BlockHolder.transform.position.z);
            MineBlocks();
            SetNeighbouringMines();
        }

        //Generate board
        private void GenerateBoard()
        {
            Vector2 pos = Vector2.zero;
            int index = 0;
            for (int i = 0; i < BoardValue; i++)
            {
                pos.y = 0;
                for (int j = 0; j < BoardValue; j++)
                {
                    GameObject block = Instantiate(BlockPrefab, BlockHolder);
                    //logical grid
                    blocks[index] = block.GetComponent<Block>();
                    blocks[index].Index = index;
                    index++;
                    //physical grid
                    block.transform.position = pos;
                    pos.y = pos.y + yShift;
                }
                pos.x = pos.x + xShift;
            }
        }

        private void MineBlocks()
        {
            List<int> indexOfMines = new List<int>();
            for (int i = 0; i < NumberOfMines; i++)
            {
                int mineIndex = Random.Range(0, BoardValue * BoardValue);
                while(indexOfMines.Contains(mineIndex))
                {
                    mineIndex = Random.Range(0, BoardValue * BoardValue);
                }
                indexOfMines.Add(mineIndex);
                blocks[mineIndex].MineBlock();
            }
        }

        private void SetNeighbouringMines()
        {
            for (int i = 0; i < BoardValue * BoardValue; i++)
            {
                blocks[i].AddNeighbouringBlocks(blocks);
            }
        }

        //public void UncoverBlock()
        //{
        //    for (int i = 0; i < BoardValue * BoardValue; i++)
        //    {
        //        blocks[i].UncoverBlock();
        //    }
        //}

        public void BlockClicked(Block b)
        {
            if (b.MineStatus == BlockMineStatus.Mined)
            {

            }
                //GameOver;
            else
            {
                UncoverBlock(b);
                BlockOpen();
            }
        }

        private void UncoverBlock(Block b)
        {
            //b.UncoverBlock();
            //BlocksToUncover.Add(b);
            if(b.NumberOfNeighbouringMines > 0)
            {
                BlocksVisited.Add(b);
                return;
            }

            if (BlocksVisited.Contains(b))
            {
                //b.ShowMineText();
                return;
            }
            else
            {
                BlocksVisited.Add(b);

                for (int i = 0; i < b.NeighbourBlocks.Count; i++)
                {
                    UncoverBlock(b.NeighbourBlocks[i]);
                }
            }
        }

        private void BlockOpen()
        {
            for (int i = 0; i < BlocksVisited.Count; i++)
            {
                BlocksVisited[i].UncoverBlock();
                if(BlocksVisited[i].NumberOfNeighbouringMines > 0)
                {
                    BlocksVisited[i].ShowMineText();
                }
            }
        }

        //Listen for input
        //Open tiles
        //Determine if game is over or won
        //etc.
    }
}
