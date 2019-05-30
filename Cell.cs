using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumulacja
{
    class Cell
    {
        private int state;

        public Cell()
        {
            this.state = 0;
        }

        public Cell(int state)
        {
            this.state = state;
        }

        public void setCellState(int state)
        {
            this.state = state;
        }

        public int getCellState()
        {
            return this.state;
        }

        /*
        public int GetNeighbours(Cell[] cells, int index, int numberOfCells)
        {
            int numberOfNeighbours = 0;
      
            if(index == 0)
            {
                if(cells[index + 1].state == true)
                {
                    ++numberOfNeighbours;
                }
                if(cells[numberOfCells].state == true)
                {
                    ++numberOfCells;
                }
                
            }
            else if(index == numberOfCells)
            {
                if (cells[index - 1].state == true)
                {
                    ++numberOfNeighbours;
                }
                if (cells[0].state == true)
                {
                    ++numberOfCells;
                }
            }
            else
            {
                if(cells[index + 1].state == true)
                {
                    ++numberOfNeighbours;
                }
                if (cells[index - 1].state == true)
                {
                    ++numberOfNeighbours;
                }
            }

            return numberOfNeighbours;
        }
        */
        /*public bool HasNeighboursBefore(Cell[] cells, int index, int numberOfCells)
        {
            bool state = false;

            if(index == 0)
            {
                if(cells[numberOfCells].state == true)
                {
                    state = true;
                }
            }
            else if(cells[index - 1].state == true)
            {
                state = true;
            }

            return state;
        }*/
        /*public bool HasNeighboursAfter(Cell[] cells, int index, int numberOfCells)
        {
            bool state = false;

            if (index == numberOfCells)
            {
                if (cells[0].state == true)
                {
                    state = true;
                }
            }
            else if (cells[index + 1].state == true)
            {
                state = true;
            }
            return state;
        }*/
    }
}
