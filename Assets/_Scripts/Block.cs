using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Block : MonoBehaviour
    {
        private SpriteRenderer BlockSpriteRenderer;

        private int index;
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
            }
        }
        public List<Block> NeighbourBlocks;
        [SerializeField]private BlockMineStatus mineStatus;
        public BlockMineStatus MineStatus
        {
            get { return mineStatus; }
            set
            {
                mineStatus = value;
                //if(mineStatus != BlockMineStatus.Mined)
                //{
                //    MineText.text = "0";
                //}
                //else
                //{
                //    MineText.text = "1";
                //}
            }
        }
        private BlockFlagStatus FlagStatus;

        public Text MineText;

        private BlockStatus blockStatus;
        public BlockStatus BlockStatus
        {
            get { return blockStatus; }
            set
            {
                blockStatus = value;
                if(blockStatus == BlockStatus.Opened)
                {
                    if (mineStatus == BlockMineStatus.Mined)
                    {
                        BlockSpriteRenderer.sprite = GameManager.Instance.MinedBlockSprite;
                    }
                    else
                    {
                        BlockSpriteRenderer.sprite = GameManager.Instance.OpenedBlockSprite;
                    }
                }
                else if(blockStatus == BlockStatus.UnOpened)
                {
                    if (FlagStatus == BlockFlagStatus.Flagged)
                    {
                        BlockSpriteRenderer.sprite = GameManager.Instance.FlaggedBlockSprite;
                    }
                    else
                    {
                        BlockSpriteRenderer.sprite = GameManager.Instance.UnopenedBlockSprite;
                    }
                }
            }
        }

        private int numberOfNeighbouringMines;
        public int NumberOfNeighbouringMines
        {
            get { return numberOfNeighbouringMines; }
            set
            {
                numberOfNeighbouringMines = value;
                MineText.text = numberOfNeighbouringMines.ToString();
            }
        }

        private void Awake()
        {
            BlockSpriteRenderer = GetComponent<SpriteRenderer>();
            InitializeBlock();
        }

        public void InitializeBlock(int index = 0)
        {
            Index = index;
            MineText.text = "";
            NeighbourBlocks = new List<Block>();
            MineStatus = BlockMineStatus.UnMined;
            FlagStatus = BlockFlagStatus.UnFlagged;
            BlockStatus = BlockStatus.UnOpened;
            NumberOfNeighbouringMines = 0;
            HideMineText();
            
        }

        public void AddNeighbouringBlocks(Block[]Blocks)
        {
            int BoardValue = GameManager.Instance.BoardValue;

            if(index + BoardValue >= 0 && index + BoardValue < BoardValue * BoardValue)
                NeighbourBlocks.Add(Blocks[index + BoardValue]);

            if (index - BoardValue >= 0 && index - BoardValue < BoardValue * BoardValue)
                NeighbourBlocks.Add(Blocks[index - BoardValue]);

            if (index + 1 >= 0 && index + 1 < BoardValue * BoardValue)
                NeighbourBlocks.Add(Blocks[index + 1]);

            if (index - 1 >= 0 && index - 1 < BoardValue * BoardValue)
                NeighbourBlocks.Add(Blocks[index - 1]);

            if (index + BoardValue + 1 >= 0 && index + BoardValue + 1 < BoardValue * BoardValue)
                NeighbourBlocks.Add(Blocks[index + BoardValue + 1]);

            if (index + BoardValue - 1 >= 0 && index + BoardValue - 1 < BoardValue * BoardValue)
                NeighbourBlocks.Add(Blocks[index + BoardValue - 1]);

            if (index - BoardValue + 1 >= 0 && index - BoardValue + 1 < BoardValue * BoardValue)
                NeighbourBlocks.Add(Blocks[index - BoardValue + 1]);

            if (index - BoardValue - 1 >= 0 && index - BoardValue - 1 < BoardValue * BoardValue)
                NeighbourBlocks.Add(Blocks[index - BoardValue - 1]);

            CalculateNeighbouringMines();

        }

        private void CalculateNeighbouringMines()
        {
            for (int i = 0; i < NeighbourBlocks.Count; i++)
            {
                if (NeighbourBlocks[i].MineStatus == BlockMineStatus.Mined)
                    NumberOfNeighbouringMines++;
            }
        }

        public void MineBlock()
        {
            MineStatus = BlockMineStatus.Mined;
        }

        //TESTING
        public void UncoverBlock()
        {
            //BlockSpriteRenderer.color = Color.red;
            BlockStatus = BlockStatus.Opened;
            //HideMineText();
        }

        public void HideMineText()
        {
            MineText.gameObject.SetActive(false);
        }
        public void ShowMineText()
        {
            MineText.gameObject.SetActive(true);
        }

        private void OnMouseDown()
        {
            //UncoverBlock();
            GameManager.Instance.BlockClicked(this);
        }
    }

    public enum BlockMineStatus
    {
        UnMined,
        Mined
    }

    public enum BlockStatus
    {
        UnOpened,
        Opened
    }

    public enum BlockFlagStatus
    {
        UnFlagged,
        Flagged
    }
}
