using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace Game_of_Life {
    class GameOfLife {
        public bool StopNow = false;

        private readonly int size;

        private int height;
        private int width;

        SolidBrush BlackBrush = new SolidBrush(Color.Black);
        SolidBrush WhiteBrush = new SolidBrush(Color.White);

        private bool[,] state;
        private bool[,] prevState;
        private bool stateSave = false;

        private readonly string filePath = @"D:\";
        StringBuilder sb = new StringBuilder();


        public GameOfLife(int size) {
            this.size = size;
        }

        ~GameOfLife() {
            BlackBrush.Dispose();
            WhiteBrush.Dispose();
        }

        public void Init(int ScreenWidth, int ScreenHeight) {
            height = ScreenHeight / size + 1;
            width = ScreenWidth / size + 1;

            state = new bool[width, height];
            prevState = new bool[width, height];

            Random randNum = new Random();
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    state[i, j] = randNum.Next(0, 2) % 2 == 1;
                    prevState[i, j] = randNum.Next(0, 2) % 2 == 0;
                }
            }

            //state[2, 2] = true;
            //state[2, 3] = true;
            //state[3, 2] = true;
            //state[3, 3] = true;


            //Glider
            //state[1, 2] = true;
            //state[2, 3] = true;
            //state[3, 1] = true;
            //state[3, 2] = true;
            //state[3, 3] = true;


            //Glider Gun 1
            //state[6, 2] = true;
            //state[7, 2] = true;
            //state[6, 3] = true;
            //state[7, 3] = true;
            //state[6, 12] = true;
            //state[7, 12] = true;
            //state[8, 12] = true;
            //state[5, 13] = true;
            //state[9, 13] = true;
            //state[4, 14] = true;
            //state[10, 14] = true;
            //state[4, 15] = true;
            //state[10, 15] = true;
            //state[7, 16] = true;
            //state[5, 17] = true;
            //state[9, 17] = true;
            //state[6, 18] = true;
            //state[7, 18] = true;
            //state[8, 18] = true;
            //state[7, 19] = true;
            //state[4, 22] = true;
            //state[5, 22] = true;
            //state[6, 22] = true;
            //state[4, 23] = true;
            //state[5, 23] = true;
            //state[6, 23] = true;
            //state[3, 24] = true;
            //state[7, 24] = true;
            //state[2, 26] = true;
            //state[3, 26] = true;
            //state[7, 26] = true;
            //state[8, 26] = true;
            //state[4, 36] = true;
            //state[5, 36] = true;
            //state[4, 37] = true;
            //state[5, 37] = true;


            //Glider Gun 3
            //state[30 + 3, height/2] = true;
            //state[30 + 4, height/2] = true;
            //state[30 + 5, height/2] = true;
            //state[30 + 6, height/2] = true;
            //state[30 + 7, height/2] = true;
            //state[30 + 8, height/2] = true;
            //state[30 + 9, height/2] = true;
            //state[30 + 10, height/2] = true;
            //state[30 + 12, height/2] = true;
            //state[30 + 13, height/2] = true;
            //state[30 + 14, height/2] = true;
            //state[30 + 15, height/2] = true;
            //state[30 + 16, height/2] = true;
            //state[30 + 20, height/2] = true;
            //state[30 + 21, height/2] = true;
            //state[30 + 22, height/2] = true;
            //state[30 + 29, height/2] = true;
            //state[30 + 30, height/2] = true;
            //state[30 + 31, height/2] = true;
            //state[30 + 32, height/2] = true;
            //state[30 + 33, height/2] = true;
            //state[30 + 34, height/2] = true;
            //state[30 + 35, height/2] = true;
            //state[30 + 37, height/2] = true;
            //state[30 + 38, height/2] = true;
            //state[30 + 39, height/2] = true;
            //state[30 + 40, height/2] = true;
            //state[30 + 41, height/2] = true;
        }

        private bool IsSegmentVisible(int X1, int Y1, int X2, int Y2) {
            return
                !(X1 < 0 || X2 < 0 || Y1 < 0 || Y1 < 0 ||
                X1 > width || X2 > width || Y1 > height || Y2 > height);
        }

        public void DrawThenCalc(Graphics targetGraphic) {
            targetGraphic.FillRectangle(BlackBrush, new Rectangle(0, 0, width * size, height * size));
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (state[i, j] == true) {
                        targetGraphic.FillRectangle(WhiteBrush, new Rectangle(i * size, j * size, size, size));
                    }
                }
            }
            bool[,] newState = new bool[width, height];

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    int num = CalcCell(i, j);
                    if (state[i, j] && num < 2) {
                        newState[i, j] = false;
                    } else if (state[i, j] && num > 3) {
                        newState[i, j] = false;
                    } else if (state[i, j] && (num == 2 || num == 3)) {
                        newState[i, j] = true;
                    } else if (!state[i, j] && num == 3) {
                        newState[i, j] = true;
                    }
                }
            }
            stateSave = !stateSave;
            if (stateSave) {
                if (CheckIfStatesEqual(prevState, state)) {
                    Random randNum = new Random();
                    for (int i = 0; i < width; i++) {
                        for (int j = 0; j < height; j++) {
                            state[i, j] = randNum.Next(0, 2) % 2 == 1;
                        }
                    }
                }

                prevState = state;
            }
            state = newState;
        }

        private bool CheckIfStatesEqual(bool[,] prevState, bool[,] newState) {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (prevState[i, j] != newState[i, j]) {
                        PrintToFile(prevState, newState, i, j);
                        return false;
                    }
                }
            }
            return true;
        }

        private void PrintToFile(bool[,] prevState, bool[,] newState, int x, int y) {
            for(int i=0;i<prevState.GetLength(0); i++) {
                for(int j=0;j< prevState.GetLength(1); j++) {
                    // END YAVAA
                    sb.Append((prevState[i,j]?1:0) + "" + (newState[i,j]?1:0) + " "
                        //+ i + " " + j
                        );
                }
                sb.Append("\r\n");
            }
            sb.Append("\r\n\r\n");
            File.AppendAllText(filePath + "log.txt", sb.ToString());
            sb.Clear();
        }

        private int CalcCell(int a, int b) {
            int sum = 0;
            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    if (state[(width + a + i) % width, (height + b + j) % height]) {
                        sum++;
                    }

                }
            }
            if (state[a, b]) {
                sum--;
            }
            return sum;
        }
    }
}
