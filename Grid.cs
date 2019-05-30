using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumulacja
{
    class Grid
    {
        public Cell[,] Map;
        int Map_height, Map_width;
        int[] statesTable;
        public int numberOfStates;
        int maxOfLiczbaZarodkow;
        int[,] previousIteration;
        public int[,] energyMap;

        public Grid()
        {

        }

        public Grid(int Map_width, int Map_height, int numberOfStates)
        {
            this.numberOfStates = numberOfStates + 1;
            this.Map_width = Map_width;
            this.Map_height = Map_height;
            this.statesTable = new int[this.numberOfStates];
            previousIteration = new int[Map_height, Map_width];
            energyMap = new int[Map_height, Map_width];

            this.Map = new Cell[Map_height, Map_width];


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    this.Map[i, j] = new Cell();
                }
            }

            for (int i = 0; i < numberOfStates; i++)
            {
                this.statesTable[i] = 0;
            }
        }

        public Cell[,] getMap()
        {
            return Map;
        }

        public int getMapEnergyState(int x, int y)
        {
            return energyMap[x, y];
        }

        public void calculatePreviousIteration()
        {
            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    previousIteration[i, j] = this.Map[i, j].getCellState();
                }
            }
        }

        public void changeMapState(int i, int j, int state)
        {
            this.Map[i, j].setCellState(state);
        }

        int[] neighbourhoodPeriodicalVonNeuman(int i, int j)
        {
            int[] neighbourhood = new int[4];

            //winkle
            if (j == 0 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, 0];
                neighbourhood[1] = previousIteration[0, Map_width - 1];
                neighbourhood[2] = previousIteration[0, 1];
                neighbourhood[3] = previousIteration[1, 0];
            }
            else if (j == Map_width - 1 && i == 0)
            {

                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[0, Map_width - 2];
                neighbourhood[2] = previousIteration[0, 0];
                neighbourhood[3] = previousIteration[1, Map_width - 1];
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[2] = previousIteration[Map_height - 1, 1];
                neighbourhood[3] = previousIteration[0, 0];
            }

            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[2] = previousIteration[Map_height - 1, 0];
                neighbourhood[3] = previousIteration[0, Map_width - 1];
            }
            //krawedzie lewa
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, Map_width - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[Map_height - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, 0];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[0, j];
            }

            //reszta
            else
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            return neighbourhood;
        }

        public void UpdateVectorPeriodicalVonNeuman()
        {
            int[] neighbourhood = new int[4];
            calculatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodPeriodicalVonNeuman(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 1; l < numberOfStates; l++)
                            {
                                if (neighbourhood[k] == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }

                        Map[i, j].setCellState(maxOfLiczbaZarodkow);
                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }

                    }
                }
            }
        }

        int[] neighbourhoodAbsorbingMoore(int i, int j)
        {
            int[] neighbourhood = new int[8];

            //winkle
            if (j == 0 && i == 0)
            {
                neighbourhood[0] = 0;
                neighbourhood[1] = 0;
                neighbourhood[2] = 0;
                neighbourhood[3] = 0;
                neighbourhood[4] = previousIteration[0, 1];

                neighbourhood[5] = 0;
                neighbourhood[6] = previousIteration[1, 0];
                neighbourhood[7] = previousIteration[1, 1];
            }
            else if (j == Map_width - 1 && i == 0)
            {
                neighbourhood[0] = 0;
                neighbourhood[1] = 0;
                neighbourhood[2] = 0;

                neighbourhood[3] = previousIteration[0, Map_width - 2];
                neighbourhood[4] = 0;

                neighbourhood[5] = previousIteration[1, Map_width - 2];
                neighbourhood[6] = previousIteration[1, Map_width - 1];
                neighbourhood[7] = 0;
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = 0;
                neighbourhood[1] = previousIteration[Map_height - 2, 0];
                neighbourhood[2] = previousIteration[Map_height - 2, 1];

                neighbourhood[3] = 0;
                neighbourhood[4] = previousIteration[Map_height - 1, 1];

                neighbourhood[5] = 0;
                neighbourhood[6] = 0;
                neighbourhood[7] = 0;
            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[2] = 0;

                neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[4] = 0;

                neighbourhood[5] = 0;
                neighbourhood[6] = 0;
                neighbourhood[7] = 0;
            }
            //krawedzie lewa
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = 0;
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = 0;
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = 0;
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }
            // góna krawędź
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = 0;
                neighbourhood[1] = 0;
                neighbourhood[2] = 0;

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }
            // prawa krawędź
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = 0;

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = 0;

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = 0;
            }
            // dolna krawędź
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = 0;
                neighbourhood[6] = 0;
                neighbourhood[7] = 0;
            }

            //reszta
            else
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }

            return neighbourhood;
        }

        public void UpdateVectorAbsorbingMoore()
        {

            int[] neighbourhood = new int[8];
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {

                        neighbourhood = neighbourhoodAbsorbingMoore(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 8; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k] == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }
        }

        int[] neighbourhoodPeriodicalHeksaRandom(int i, int j)
        {
            int[] neighbourhood = new int[6];
            Random randomHeksaGenerator = new Random();

            switch (randomHeksaGenerator.Next(2))
            {

                case 0:
                    if (j == 0 && i == 0)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                        neighbourhood[1] = previousIteration[Map_height - 1, 0];


                        neighbourhood[2] = previousIteration[0, Map_width - 1];
                        neighbourhood[3] = previousIteration[0, 1];


                        neighbourhood[4] = previousIteration[1, 0];
                        neighbourhood[5] = previousIteration[1, 1];
                    }
                    else if (j == Map_width - 1 && i == 0)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];


                        neighbourhood[2] = previousIteration[0, Map_width - 2];
                        neighbourhood[3] = previousIteration[0, 0];


                        neighbourhood[4] = previousIteration[1, Map_width - 1];
                        neighbourhood[5] = previousIteration[1, 0];
                    }
                    else if (j == 0 && i == Map_height - 1)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                        neighbourhood[1] = previousIteration[Map_height - 2, 0];


                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                        neighbourhood[3] = previousIteration[Map_height - 1, 1];


                        neighbourhood[4] = previousIteration[0, 0];
                        neighbourhood[5] = previousIteration[0, 1];
                    }
                    else if (j == Map_width - 1 && i == Map_height - 1)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                        neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];


                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[3] = previousIteration[Map_height - 1, 0];


                        neighbourhood[4] = previousIteration[0, Map_width - 1];
                        neighbourhood[5] = previousIteration[0, 0];
                    }
                    else if (j == 0 && i != 0 && i != (Map_height - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];


                        neighbourhood[2] = previousIteration[i, Map_width - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];


                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }
                    else if (j != 0 && i == 0 && j != (Map_width - 1))
                    {
                        neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                        neighbourhood[1] = previousIteration[Map_height - 1, j];


                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];


                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }
                    else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];


                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, 0];


                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, 0];
                    }
                    else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];


                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];


                        neighbourhood[4] = previousIteration[0, j];
                        neighbourhood[5] = previousIteration[0, j + 1];
                    }

                    else
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];


                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];


                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }
                    break;
                case 1:
                    if (j == 0 && i == 0)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 1, 0];
                        neighbourhood[1] = previousIteration[Map_height - 1, 1];

                        neighbourhood[2] = previousIteration[0, Map_width - 1];
                        neighbourhood[3] = previousIteration[0, 1];

                        neighbourhood[4] = previousIteration[1, Map_width - 1];
                        neighbourhood[5] = previousIteration[1, 0];

                    }
                    else if (j == Map_width - 1 && i == 0)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                        neighbourhood[1] = previousIteration[Map_height - 1, 0];

                        neighbourhood[2] = previousIteration[0, Map_width - 2];
                        neighbourhood[3] = previousIteration[0, 0];

                        neighbourhood[4] = previousIteration[1, Map_width - 2];
                        neighbourhood[5] = previousIteration[1, Map_width - 1];

                    }
                    else if (j == 0 && i == Map_height - 1)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 2, 0];
                        neighbourhood[1] = previousIteration[Map_height - 2, 1];

                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                        neighbourhood[3] = previousIteration[Map_height - 1, 1];

                        neighbourhood[4] = previousIteration[0, Map_width - 1];
                        neighbourhood[5] = previousIteration[0, 0];

                    }
                    else if (j == Map_width - 1 && i == Map_height - 1)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                        neighbourhood[1] = previousIteration[Map_height - 2, 0];

                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[3] = previousIteration[Map_height - 1, 0];

                        neighbourhood[4] = previousIteration[0, Map_width - 2];
                        neighbourhood[5] = previousIteration[0, Map_width - 1];

                    }
                    else if (j == 0 && i != 0 && i != (Map_height - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, Map_width - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, Map_width - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j != 0 && i == 0 && j != (Map_width - 1))
                    {

                        neighbourhood[0] = previousIteration[Map_height - 1, j];
                        neighbourhood[1] = previousIteration[Map_height - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, 0];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, 0];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[0, j - 1];
                        neighbourhood[5] = previousIteration[0, j];

                    }

                    else
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    break;
            }

            return neighbourhood;
        }

        public void UpdateVectorPeriodicalHeksaRadnom()
        {

            int[] neighbourhood = new int[6];
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodPeriodicalHeksaRandom(i, j);

                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k] == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }
        }

        int[] neighbourhoodAbsorbingHeksaRandom(int i, int j)
        {
            int[] neighbourhood = new int[6];
            Random randomHeksaGenerator = new Random();


            switch (randomHeksaGenerator.Next(2))
            {

                case 0:
                    if (j == 0 && i == 0)
                    {

                        neighbourhood[0] = 0;
                        neighbourhood[1] = 0;

                        neighbourhood[2] = 0;
                        neighbourhood[3] = previousIteration[0, 1];

                        neighbourhood[4] = 0;
                        neighbourhood[5] = previousIteration[1, 0];

                    }
                    else if (j == Map_width - 1 && i == 0)
                    {

                        neighbourhood[0] = 0;
                        neighbourhood[1] = 0;

                        neighbourhood[2] = previousIteration[0, Map_width - 2];
                        neighbourhood[3] = 0;

                        neighbourhood[4] = previousIteration[1, Map_width - 2];
                        neighbourhood[5] = previousIteration[1, Map_width - 1];

                    }
                    else if (j == 0 && i == Map_height - 1)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 2, 0];
                        neighbourhood[1] = previousIteration[Map_height - 2, 1];

                        neighbourhood[2] = 0;
                        neighbourhood[3] = previousIteration[Map_height - 1, 1];

                        neighbourhood[4] = 0;
                        neighbourhood[5] = 0;

                    }
                    else if (j == Map_width - 1 && i == Map_height - 1)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                        neighbourhood[1] = 0;

                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[3] = 0;

                        neighbourhood[4] = 0;
                        neighbourhood[5] = 0;

                    }
                    //edges
                    else if (j == 0 && i != 0 && i != (Map_height - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = 0;
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = 0;
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j != 0 && i == 0 && j != (Map_width - 1))
                    {

                        neighbourhood[0] = 0;
                        neighbourhood[1] = 0;

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = 0;

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = 0;

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = 0;
                        neighbourhood[5] = 0;

                    }

                    else
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    break;
                case 1:
                    if (j == 0 && i == 0)
                    {
                        neighbourhood[0] = 0;
                        neighbourhood[1] = 0;

                        neighbourhood[2] = 0;
                        neighbourhood[3] = previousIteration[0, 1];

                        neighbourhood[4] = previousIteration[1, 0];
                        neighbourhood[5] = previousIteration[1, 1];
                    }
                    else if (j == Map_width - 1 && i == 0)
                    {
                        neighbourhood[0] = 0;
                        neighbourhood[1] = 0;

                        neighbourhood[2] = previousIteration[0, Map_width - 2];
                        neighbourhood[3] = 0;

                        neighbourhood[4] = previousIteration[1, Map_width - 1];
                        neighbourhood[5] = 0;
                    }
                    else if (j == 0 && i == Map_height - 1)
                    {
                        neighbourhood[0] = 0;
                        neighbourhood[1] = previousIteration[Map_height - 2, 0];

                        neighbourhood[2] = 0;
                        neighbourhood[3] = previousIteration[Map_height - 1, 1];

                        neighbourhood[4] = 0;
                        neighbourhood[5] = 0;
                    }
                    else if (j == Map_width - 1 && i == Map_height - 1)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                        neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];

                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[3] = 0;

                        neighbourhood[4] = 0;
                        neighbourhood[5] = 0;
                    }

                    else if (j == 0 && i != 0 && i != (Map_height - 1))
                    {
                        neighbourhood[0] = 0;
                        neighbourhood[1] = previousIteration[i - 1, j];

                        neighbourhood[2] = 0;
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }

                    else if (j != 0 && i == 0 && j != (Map_width - 1))
                    {
                        neighbourhood[0] = 0;
                        neighbourhood[1] = 0;

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }

                    else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = 0;

                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = 0;
                    }

                    else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = 0;
                        neighbourhood[5] = 0;
                    }

                    else
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }
                    break;
            }


            return neighbourhood;
        }

        public void UpdateVectorAbsorbingHeksaRandom()
        {

            int[] neighbourhood = new int[6];
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodAbsorbingHeksaRandom(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k] == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }

        }

        int[] neighbourhoodAbsorbingHeksaRight(int i, int j)
        {
            int[] neighbourhood = new int[6];


            if (j == 0 && i == 0)
            {
                neighbourhood[0] = 0;
                neighbourhood[1] = 0;

                neighbourhood[2] = 0;
                neighbourhood[3] = previousIteration[0, 1];

                neighbourhood[4] = previousIteration[1, 0];
                neighbourhood[5] = previousIteration[1, 1];
            }
            else if (j == Map_width - 1 && i == 0)
            {
                neighbourhood[0] = 0;
                neighbourhood[1] = 0;

                neighbourhood[2] = previousIteration[0, Map_width - 2];
                neighbourhood[3] = 0;

                neighbourhood[4] = previousIteration[1, Map_width - 1];
                neighbourhood[5] = 0;
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = 0;
                neighbourhood[1] = previousIteration[Map_height - 2, 0];

                neighbourhood[2] = 0;
                neighbourhood[3] = previousIteration[Map_height - 1, 1];

                neighbourhood[4] = 0;
                neighbourhood[5] = 0;
            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];

                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[3] = 0;

                neighbourhood[4] = 0;
                neighbourhood[5] = 0;
            }

            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = 0;
                neighbourhood[1] = previousIteration[i - 1, j];

                neighbourhood[2] = 0;
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }

            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = 0;
                neighbourhood[1] = 0;

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }

            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = 0;

                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = 0;
            }

            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = 0;
                neighbourhood[5] = 0;
            }

            else
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }



            return neighbourhood;
        }

        public void UpdateVectorAbsorbingHeksaRight()
        {

            int[] neighbourhood = new int[6];
            calculatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodAbsorbingHeksaRight(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k] == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }
        }

        int[] neighbourhoodPeriodicalHeksaRight(int i, int j)
        {
            int[] neighbourhood = new int[6];

            if (j == 0 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, 0];


                neighbourhood[2] = previousIteration[0, Map_width - 1];
                neighbourhood[3] = previousIteration[0, 1];


                neighbourhood[4] = previousIteration[1, 0];
                neighbourhood[5] = previousIteration[1, 1];
            }
            else if (j == Map_width - 1 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];


                neighbourhood[2] = previousIteration[0, Map_width - 2];
                neighbourhood[3] = previousIteration[0, 0];


                neighbourhood[4] = previousIteration[1, Map_width - 1];
                neighbourhood[5] = previousIteration[1, 0];
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 2, 0];


                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[3] = previousIteration[Map_height - 1, 1];


                neighbourhood[4] = previousIteration[0, 0];
                neighbourhood[5] = previousIteration[0, 1];
            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];


                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[3] = previousIteration[Map_height - 1, 0];


                neighbourhood[4] = previousIteration[0, Map_width - 1];
                neighbourhood[5] = previousIteration[0, 0];
            }
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[i - 1, j];


                neighbourhood[2] = previousIteration[i, Map_width - 1];
                neighbourhood[3] = previousIteration[i, j + 1];


                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, j];


                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];


                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];


                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, 0];


                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, 0];
            }
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];


                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];


                neighbourhood[4] = previousIteration[0, j];
                neighbourhood[5] = previousIteration[0, j + 1];
            }

            else
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];


                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];


                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }
            return neighbourhood;
        }

        public void UpdateVectorPeriodicalHeksaRight()
        {
            int[] neighbourhood = new int[6];
            calculatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodPeriodicalHeksaRight(i, j);
                        //////////////////////////////////////////////////////


                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k] == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }
        }

        int[] neighbourhoodAbsorbingHeksaLeft(int i, int j)
        {
            int[] neighbourhood = new int[6];

            if (j == 0 && i == 0)
            {

                neighbourhood[0] = 0;
                neighbourhood[1] = 0;

                neighbourhood[2] = 0;
                neighbourhood[3] = previousIteration[0, 1];

                neighbourhood[4] = 0;
                neighbourhood[5] = previousIteration[1, 0];

            }
            else if (j == Map_width - 1 && i == 0)
            {

                neighbourhood[0] = 0;
                neighbourhood[1] = 0;

                neighbourhood[2] = previousIteration[0, Map_width - 2];
                neighbourhood[3] = 0;

                neighbourhood[4] = previousIteration[1, Map_width - 2];
                neighbourhood[5] = previousIteration[1, Map_width - 1];

            }
            else if (j == 0 && i == Map_height - 1)
            {

                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                neighbourhood[1] = previousIteration[Map_height - 2, 1];

                neighbourhood[2] = 0;
                neighbourhood[3] = previousIteration[Map_height - 1, 1];

                neighbourhood[4] = 0;
                neighbourhood[5] = 0;

            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {

                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = 0;

                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[3] = 0;

                neighbourhood[4] = 0;
                neighbourhood[5] = 0;

            }
            //edges
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = 0;
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = 0;
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {

                neighbourhood[0] = 0;
                neighbourhood[1] = 0;

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = 0;

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = 0;

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = 0;
                neighbourhood[5] = 0;

            }

            else
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }

            return neighbourhood;
        }

        public void UpdateVectorAbsorbingHeksaLeft()
        {
            int[] neighbourhood = new int[6];
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodAbsorbingHeksaLeft(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k] == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }
        }

        int[] neighbourhoodPeriodicalHeksaLeft(int i, int j)
        {
            int[] neighbourhood = new int[6];

            if (j == 0 && i == 0)
            {

                neighbourhood[0] = previousIteration[Map_height - 1, 0];
                neighbourhood[1] = previousIteration[Map_height - 1, 1];

                neighbourhood[2] = previousIteration[0, Map_width - 1];
                neighbourhood[3] = previousIteration[0, 1];

                neighbourhood[4] = previousIteration[1, Map_width - 1];
                neighbourhood[5] = previousIteration[1, 0];

            }
            else if (j == Map_width - 1 && i == 0)
            {

                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, 0];

                neighbourhood[2] = previousIteration[0, Map_width - 2];
                neighbourhood[3] = previousIteration[0, 0];

                neighbourhood[4] = previousIteration[1, Map_width - 2];
                neighbourhood[5] = previousIteration[1, Map_width - 1];

            }
            else if (j == 0 && i == Map_height - 1)
            {

                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                neighbourhood[1] = previousIteration[Map_height - 2, 1];

                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[3] = previousIteration[Map_height - 1, 1];

                neighbourhood[4] = previousIteration[0, Map_width - 1];
                neighbourhood[5] = previousIteration[0, 0];

            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {

                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 2, 0];

                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[3] = previousIteration[Map_height - 1, 0];

                neighbourhood[4] = previousIteration[0, Map_width - 2];
                neighbourhood[5] = previousIteration[0, Map_width - 1];

            }
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, Map_width - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, Map_width - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {

                neighbourhood[0] = previousIteration[Map_height - 1, j];
                neighbourhood[1] = previousIteration[Map_height - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, 0];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, 0];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[0, j - 1];
                neighbourhood[5] = previousIteration[0, j];

            }

            else
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }

            return neighbourhood;
        }

        public void UpdateVectorPeriodicalHeksaLeft()
        {
            int[] neighbourhood = new int[6];
            calculatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodPeriodicalHeksaLeft(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k] == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }

        }

        int[] neighbourhoodAbsorbingVonNeuman(int i, int j)
        {
            int[] neighbourhood = new int[4];

            //winkle
            if (j == 0 && i == 0)
            {
                neighbourhood[0] = 0;
                neighbourhood[1] = 0;
                neighbourhood[2] = previousIteration[0, 1];
                neighbourhood[3] = previousIteration[1, 0];
            }
            else if (j == Map_width - 1 && i == 0)
            {

                neighbourhood[0] = 0;
                neighbourhood[1] = previousIteration[0, Map_width - 2];
                neighbourhood[2] = 0;
                neighbourhood[3] = previousIteration[1, Map_width - 1];
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                neighbourhood[1] = 0;
                neighbourhood[2] = previousIteration[Map_height - 1, 1];
                neighbourhood[3] = 0;
            }

            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[2] = 0;
                neighbourhood[3] = 0;
            }
            //krawedzie lewa
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = 0;
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = 0;
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = 0;
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = 0;
            }

            //reszta
            else
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }
            return neighbourhood;
        }

        public void UpdateVectorAbsorbingVonNeuman()
        {
            int[] neighbourhood = new int[4];
            calculatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodAbsorbingVonNeuman(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 1; l < numberOfStates; l++)
                            {
                                if (neighbourhood[k] == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }

                        Map[i, j].setCellState(maxOfLiczbaZarodkow);
                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }

                    }
                }
            }

        }

        int[] neighbourhoodPeriodicalMoore(int i, int j)
        {
            int[] neighbourhood = new int[8];

            //winkle
            if (j == 0 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, 0];
                neighbourhood[2] = previousIteration[Map_height - 1, 1];

                neighbourhood[3] = previousIteration[0, Map_width - 1];
                neighbourhood[4] = previousIteration[0, 1];

                neighbourhood[5] = previousIteration[1, Map_width - 1];
                neighbourhood[6] = previousIteration[1, 0];
                neighbourhood[7] = previousIteration[1, 1];
            }
            else if (j == Map_width - 1 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[2] = previousIteration[Map_height - 1, 0];

                neighbourhood[3] = previousIteration[0, Map_width - 2];
                neighbourhood[4] = previousIteration[0, 0];

                neighbourhood[5] = previousIteration[1, Map_width - 2];
                neighbourhood[6] = previousIteration[1, Map_width - 1];
                neighbourhood[7] = previousIteration[1, 0];
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 2, 0];
                neighbourhood[2] = previousIteration[Map_height - 2, 1];

                neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[4] = previousIteration[Map_height - 1, 1];

                neighbourhood[5] = previousIteration[0, Map_width - 1];
                neighbourhood[6] = previousIteration[0, 0];
                neighbourhood[7] = previousIteration[0, 1];
            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[2] = previousIteration[Map_height - 2, 0];

                neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[4] = previousIteration[Map_height - 1, 0];

                neighbourhood[5] = previousIteration[0, Map_width - 2];
                neighbourhood[6] = previousIteration[0, Map_width - 1];
                neighbourhood[7] = previousIteration[0, 0];
            }
            //krawedzie lewa
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, Map_width - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, Map_width - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }
            // góna krawędź
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, j];
                neighbourhood[2] = previousIteration[Map_height - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }
            // prawa krawędź
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, 0];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, 0];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, 0];
            }
            // dolna krawędź
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[0, j - 1];
                neighbourhood[6] = previousIteration[0, j];
                neighbourhood[7] = previousIteration[0, j + 1];
            }

            //reszta
            else
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }

            return neighbourhood;
        }

        public void UpdateVectorPeriodicalMoore()
        {
            int[] neighbourhood = new int[8];
            calculatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodPeriodicalMoore(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 8; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k] == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }
        }

        int[] neighbourhoodPeriodicalPentagonal(int i, int j)
        {
            int[] neighbourhood = new int[5];

            Random randomPentagonalGenerator = new Random();
            int whichPentagonal = 0;

            whichPentagonal = randomPentagonalGenerator.Next(0, 3);

            switch (whichPentagonal)
            {
                case 0:
                    {
                        //winkle lewa góra
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, 0];
                            neighbourhood[2] = previousIteration[0, Map_width - 1];
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = previousIteration[1, 0];

                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[2] = previousIteration[0, Map_width - 2];
                            neighbourhood[3] = previousIteration[1, Map_width - 2];
                            neighbourhood[4] = previousIteration[1, Map_width - 1];

                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[3] = previousIteration[0, Map_width - 1];
                            neighbourhood[4] = previousIteration[0, 0];

                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[3] = previousIteration[0, Map_width - 2];
                            neighbourhood[4] = previousIteration[0, Map_width - 1];

                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, Map_width - 1];
                            neighbourhood[3] = previousIteration[i + 1, Map_width - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[0, j - 1];
                            neighbourhood[4] = previousIteration[0, j];
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];
                        }

                        break;
                    }
                case 1:
                    {
                        //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, 0];
                            neighbourhood[1] = previousIteration[Map_height - 1, 1];
                            neighbourhood[2] = previousIteration[0, 1];
                            neighbourhood[3] = previousIteration[1, 0];
                            neighbourhood[4] = previousIteration[1, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, 0];
                            neighbourhood[2] = previousIteration[0, 0];
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = previousIteration[1, 0];
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, 0];
                            neighbourhood[1] = previousIteration[Map_height - 2, 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, 1];
                            neighbourhood[3] = previousIteration[0, 0];
                            neighbourhood[4] = previousIteration[0, 1];
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = previousIteration[Map_height - 1, 0];
                            neighbourhood[3] = previousIteration[0, Map_width - 1];
                            neighbourhood[4] = previousIteration[0, 0];
                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, j];
                            neighbourhood[1] = previousIteration[Map_height - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, 0];
                            neighbourhood[2] = previousIteration[i, 0];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, 0];
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[0, j];
                            neighbourhood[4] = previousIteration[0, j + 1];
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        break;
                    }
                case 2:
                    {
                        //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[0, Map_width - 1];
                            neighbourhood[1] = previousIteration[0, 1];
                            neighbourhood[2] = previousIteration[1, Map_width - 1];
                            neighbourhood[3] = previousIteration[1, 0];
                            neighbourhood[4] = previousIteration[1, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[0, Map_width - 2];
                            neighbourhood[1] = previousIteration[0, 0];
                            neighbourhood[2] = previousIteration[1, Map_width - 2];
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = previousIteration[1, 0];
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, 1];
                            neighbourhood[2] = previousIteration[0, Map_width - 1];
                            neighbourhood[3] = previousIteration[0, 0];
                            neighbourhood[4] = previousIteration[0, 1];
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 1, 0];
                            neighbourhood[2] = previousIteration[0, Map_width - 2];
                            neighbourhood[3] = previousIteration[0, Map_width - 1];
                            neighbourhood[4] = previousIteration[0, 0];
                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i, Map_width - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, Map_width - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, 0];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, 0];
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[0, j - 1];
                            neighbourhood[3] = previousIteration[0, j];
                            neighbourhood[4] = previousIteration[0, j + 1];
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }

                        break;
                    }
                case 3:
                    {
                        //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, 0];
                            neighbourhood[2] = previousIteration[Map_height - 1, 1];
                            neighbourhood[3] = previousIteration[0, Map_width - 1];
                            neighbourhood[4] = previousIteration[0, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, 0];
                            neighbourhood[3] = previousIteration[0, Map_width - 2];
                            neighbourhood[4] = previousIteration[0, 0];
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = previousIteration[Map_height - 2, 1];
                            neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[4] = previousIteration[Map_height - 1, 1];
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[2] = previousIteration[Map_height - 2, 0];

                            neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[4] = previousIteration[Map_height - 1, 0];
                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, Map_width - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, j];
                            neighbourhood[2] = previousIteration[Map_height - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, 0];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, 0];
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }

                        break;
                    }
                default:
                    break;
            }

            return neighbourhood;
        }

        public void UpdateVectorPeriodicalPentagonal()
        {
            int[] neighbourhood = new int[5];
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodPeriodicalPentagonal(i, j);
                        for (int k = 0; k < 5; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k] == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }

                        Map[i, j].setCellState(maxOfLiczbaZarodkow);
                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }

                    }
                }
            }

        }

        int[] neighbourhoodAbsorbingPentagonal(int i, int j)
        {
            int[] neighbourhood = new int[5];


            Random randomPentagonalGenerator = new Random();
            int whichPentagonal = 0;
            whichPentagonal = randomPentagonalGenerator.Next(0, 4);

            switch (whichPentagonal)
            {
                case 0:
                    {
                        //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = 0;
                            neighbourhood[2] = 0;
                            neighbourhood[3] = 0;
                            neighbourhood[4] = previousIteration[1, 0];

                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = 0;
                            neighbourhood[2] = previousIteration[0, Map_width - 2];
                            neighbourhood[3] = previousIteration[1, Map_width - 2];
                            neighbourhood[4] = previousIteration[1, Map_width - 1];

                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = 0;
                            neighbourhood[3] = 0;
                            neighbourhood[4] = 0;

                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[3] = 0;
                            neighbourhood[4] = 0;

                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = 0;
                            neighbourhood[3] = 0;
                            neighbourhood[4] = previousIteration[i + 1, j];

                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = 0;
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];

                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];

                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = 0;
                            neighbourhood[4] = 0;

                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];

                        }
                        break;

                    }
                case 1:
                    { //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = 0;
                            neighbourhood[2] = previousIteration[0, 1];
                            neighbourhood[3] = previousIteration[1, 0];
                            neighbourhood[4] = previousIteration[1, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = 0;
                            neighbourhood[2] = 0;
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = 0;
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, 0];
                            neighbourhood[1] = previousIteration[Map_height - 2, 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, 1];
                            neighbourhood[3] = 0;
                            neighbourhood[4] = 0;
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[1] = 0;
                            neighbourhood[2] = 0;
                            neighbourhood[3] = 0;
                            neighbourhood[4] = 0;
                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = 0;
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = 0;
                            neighbourhood[2] = 0;
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = 0;
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = 0;
                            neighbourhood[4] = 0;
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }

                        break;
                    }
                case 2:
                    { //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = previousIteration[0, 1];
                            neighbourhood[2] = 0;
                            neighbourhood[3] = previousIteration[1, 0];
                            neighbourhood[4] = previousIteration[1, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[0, Map_width - 2];
                            neighbourhood[1] = 0;
                            neighbourhood[2] = previousIteration[1, Map_width - 2];
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = 0;
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = previousIteration[Map_height - 1, 1];
                            neighbourhood[2] = 0;
                            neighbourhood[3] = 0;
                            neighbourhood[4] = 0;
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[1] = 0;
                            neighbourhood[2] = 0;
                            neighbourhood[3] = 0;
                            neighbourhood[4] = 0;
                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = 0;
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = 0;
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = 0;
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = 0;
                            neighbourhood[3] = 0;
                            neighbourhood[4] = 0;
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }

                        break;
                    }
                case 3:
                    { //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = 0;
                            neighbourhood[2] = 0;
                            neighbourhood[3] = 0;
                            neighbourhood[4] = previousIteration[0, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = 0;
                            neighbourhood[2] = 0;
                            neighbourhood[3] = previousIteration[0, Map_width - 2];
                            neighbourhood[4] = 0;
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = previousIteration[Map_height - 2, 1];
                            neighbourhood[3] = 0;
                            neighbourhood[4] = previousIteration[Map_height - 1, 1];
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[2] = 0;
                            neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[4] = 0;

                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = 0;
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = 0;
                            neighbourhood[1] = 0;
                            neighbourhood[2] = 0;
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = 0;
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = 0;
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }

                        break;
                    }
                default:
                    break;
            }

            return neighbourhood;
        }

        public void UpdateVectorAbsorbingPentagonal()
        {

            int[] neighbourhood = new int[5];
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodAbsorbingPentagonal(i, j);
                        for (int k = 0; k < 5; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k] == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }

                        Map[i, j].setCellState(maxOfLiczbaZarodkow);
                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }


                    }
                }

            }
        }

        public void calculateMonteCarlo(int boundaryCondition, int neighbourType)
        {
            bool[,] visitMap = new bool[Map_height, Map_width];
            Random mcGenerator = new Random();
            int x, y;
            int energy = 0;
            int randomNeighbour = 0;
            int energyRandomNeighbour = 0;
            int iterator = 0;
            int[] neighbours;

            calculatePreviousIteration();
            while (iterator < (Map_height * Map_width))
            {

                x = mcGenerator.Next(0, Map_height);
                y = mcGenerator.Next(0, Map_width);

                if (visitMap[x, y] == false)
                {
                    if (boundaryCondition == 0) //periodic
                    {
                        switch (neighbourType)
                        {
                            case 0: //VonNeuman
                                {
                                    neighbours = neighbourhoodPeriodicalVonNeuman(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != Map[x, y].getCellState())
                                        {
                                            energy++;
                                        }
                                    }
                                    energyMap[x, y] = energy;
                                    randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)];

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != randomNeighbour)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y] = randomNeighbour;
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing  */
                                    }

                                    break;
                                }
                            case 1: //Moore'a
                                {
                                    neighbours = neighbourhoodPeriodicalMoore(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != Map[x, y].getCellState())
                                        {
                                            energy++;
                                        }
                                    }

                                    energyMap[x, y] = energy;
                                    randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)];

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != randomNeighbour)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y] = randomNeighbour;
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing  */
                                    }

                                    break;
                                }
                            case 2: //PentaRandom
                                {
                                    neighbours = neighbourhoodPeriodicalPentagonal(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != Map[x, y].getCellState())
                                        {
                                            energy++;
                                        }
                                    }
                                    energyMap[x, y] = energy;
                                    randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)];

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != randomNeighbour)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y] = randomNeighbour;
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing  */
                                    }

                                    break;
                                }
                            case 3: //heksaLeft
                                {
                                    neighbours = neighbourhoodPeriodicalHeksaLeft(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != Map[x, y].getCellState())
                                        {
                                            energy++;
                                        }
                                    }
                                    energyMap[x, y] = energy;
                                    randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)];

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != randomNeighbour)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y] = randomNeighbour;
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing  */
                                    }

                                    break;
                                }
                            case 4: //heksaRight
                                {
                                    neighbours = neighbourhoodPeriodicalHeksaRight(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != Map[x, y].getCellState())
                                        {
                                            energy++;
                                        }
                                    }
                                    energyMap[x, y] = energy;
                                    randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)];

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != randomNeighbour)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y] = randomNeighbour;
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing  */
                                    }

                                    break;
                                }
                            case 5: //heksaRand
                                {
                                    neighbours = neighbourhoodPeriodicalHeksaRandom(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != Map[x, y].getCellState())
                                        {
                                            energy++;
                                        }
                                    }

                                    energyMap[x, y] = energy;
                                    randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)];

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != randomNeighbour)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y] = randomNeighbour;
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing  */
                                    }

                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (neighbourType)
                        {
                            case 0: //VonNeuman
                                {
                                    neighbours = neighbourhoodAbsorbingVonNeuman(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != Map[x, y].getCellState() && neighbours[i] != 0)
                                        {
                                            energy++;
                                        }
                                    }

                                    energyMap[x, y] = energy;
                                    do
                                    {
                                        randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)];
                                    } while (randomNeighbour == 0);

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != randomNeighbour && neighbours[i] != 0)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y] = randomNeighbour;
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing  */
                                    }

                                    break;
                                }
                            case 1: //Moore'a
                                {
                                    neighbours = neighbourhoodAbsorbingMoore(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != Map[x, y].getCellState() && neighbours[i] != 0)
                                        {
                                            energy++;
                                        }
                                    }

                                    energyMap[x, y] = energy;
                                    do
                                    {
                                        randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)];
                                    } while (randomNeighbour == 0);

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != randomNeighbour && neighbours[i] != 0)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y] = randomNeighbour;
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing  */
                                    }

                                    break;
                                }
                            case 2: //PentaRandom
                                {
                                    neighbours = neighbourhoodAbsorbingPentagonal(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != Map[x, y].getCellState() && neighbours[i] != 0)
                                        {
                                            energy++;
                                        }
                                    }
                                    energyMap[x, y] = energy;
                                    do
                                    {
                                        randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)];
                                    } while (randomNeighbour == 0);

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != randomNeighbour && neighbours[i] != 0)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y] = randomNeighbour;
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing  */
                                    }

                                    break;
                                }
                            case 3: //heksaLeft
                                {
                                    neighbours = neighbourhoodAbsorbingHeksaLeft(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != Map[x, y].getCellState() && neighbours[i] != 0)
                                        {
                                            energy++;
                                        }
                                    }
                                    energyMap[x, y] = energy;
                                    do
                                    {
                                        randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)];
                                    } while (randomNeighbour == 0);

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != randomNeighbour && neighbours[i] != 0)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y] = randomNeighbour;
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing  */
                                    }

                                    break;
                                }
                            case 4: //heksaRight
                                {
                                    neighbours = neighbourhoodAbsorbingHeksaRight(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != Map[x, y].getCellState() && neighbours[i] != 0)
                                        {
                                            energy++;
                                        }
                                    }

                                    energyMap[x, y] = energy;
                                    do
                                    {
                                        randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)];
                                    } while (randomNeighbour == 0);

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != randomNeighbour && neighbours[i] != 0)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y] = randomNeighbour;
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing  */
                                    }

                                    break;
                                }
                            case 5: //heksaRand
                                {
                                    neighbours = neighbourhoodAbsorbingHeksaRandom(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != Map[x, y].getCellState() && neighbours[i] != 0)
                                        {
                                            energy++;
                                        }
                                    }

                                    energyMap[x, y] = energy;
                                    do
                                    {
                                        randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)];
                                    } while (randomNeighbour == 0);

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i] != randomNeighbour && neighbours[i] != 0)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y] = randomNeighbour;
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing  */
                                    }

                                    break;
                                }
                            default:
                                break;
                        }

                    }
                    visitMap[x, y] = true;
                    ++iterator;
                }
                energy = 0;
                energyRandomNeighbour = 0;
            }
        }

    }
}
