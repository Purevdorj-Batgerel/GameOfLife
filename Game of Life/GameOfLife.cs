using System;
using System.Diagnostics;
using System.Drawing;

namespace Game_of_Life {
    class GameOfLife {
        public bool StopNow = false;

        private int size;

        private int height;
        private int width;

        SolidBrush BlackBrush = new SolidBrush(Color.Black);
        SolidBrush WhiteBrush = new SolidBrush(Color.White);

        private int[,] state;

        public GameOfLife(int size) {
            this.size = size;
        }

        ~GameOfLife() {
            BlackBrush.Dispose();
            WhiteBrush.Dispose();
        }

        public void Init(int ScreenWidth, int ScreenHeight) {
            this.height = ScreenHeight / size + 1;
            this.width = ScreenWidth / size + 1;

            state = new int[width, height];

            Random randNum = new Random();
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    state[i, j] = randNum.Next(0, 2);
                }
            }

            //state[2, 2] = 1;
            //state[2, 3] = 1;
            //state[3, 2] = 1;
            //state[3, 3] = 1;


            //Glider
            //state[1, 2] = 1;
            //state[2, 3] = 1;
            //state[3, 1] = 1;
            //state[3, 2] = 1;
            //state[3, 3] = 1;


            //Glider Gun 1
            //state[6, 2] = 1;
            //state[7, 2] = 1;
            //state[6, 3] = 1;
            //state[7, 3] = 1;
            //state[6, 12] = 1;
            //state[7, 12] = 1;
            //state[8, 12] = 1;
            //state[5, 13] = 1;
            //state[9, 13] = 1;
            //state[4, 14] = 1;
            //state[10, 14] = 1;
            //state[4, 15] = 1;
            //state[10, 15] = 1;
            //state[7, 16] = 1;
            //state[5, 17] = 1;
            //state[9, 17] = 1;
            //state[6, 18] = 1;
            //state[7, 18] = 1;
            //state[8, 18] = 1;
            //state[7, 19] = 1;
            //state[4, 22] = 1;
            //state[5, 22] = 1;
            //state[6, 22] = 1;
            //state[4, 23] = 1;
            //state[5, 23] = 1;
            //state[6, 23] = 1;
            //state[3, 24] = 1;
            //state[7, 24] = 1;
            //state[2, 26] = 1;
            //state[3, 26] = 1;
            //state[7, 26] = 1;
            //state[8, 26] = 1;
            //state[4, 36] = 1;
            //state[5, 36] = 1;
            //state[4, 37] = 1;
            //state[5, 37] = 1;


            //Glider Gun 3
            //state[3, 10] = 1;
            //state[4, 10] = 1;
            //state[5, 10] = 1;
            //state[6, 10] = 1;
            //state[7, 10] = 1;
            //state[8, 10] = 1;
            //state[9, 10] = 1;
            //state[10, 10] = 1;
            //state[12, 10] = 1;
            //state[13, 10] = 1;
            //state[14, 10] = 1;
            //state[15, 10] = 1;
            //state[16, 10] = 1;
            //state[20, 10] = 1;
            //state[21, 10] = 1;
            //state[22, 10] = 1;
            //state[29, 10] = 1;
            //state[30, 10] = 1;
            //state[31, 10] = 1;
            //state[32, 10] = 1;
            //state[33, 10] = 1;
            //state[34, 10] = 1;
            //state[35, 10] = 1;
            //state[37, 10] = 1;
            //state[38, 10] = 1;
            //state[39, 10] = 1;
            //state[40, 10] = 1;
            //state[41, 10] = 1;
        }

        private bool IsSegmentVisible(int X1, int Y1, int X2, int Y2) {
            return
                !(X1 < 0 || X2 < 0 || Y1 < 0 || Y1 < 0 ||
                X1 > width || X2 > width || Y1 > height || Y2 > height);
        }

        public void drawThenCalc(Graphics targetGraphic) {
            
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    SolidBrush brush = BlackBrush;
                    if(state[i,j] == 1) {
                        brush = WhiteBrush;
                    }
                    targetGraphic.FillRectangle(brush, new Rectangle(i * size, j * size, size, size));
                }
            }
            int[,] newState = new int[width, height];

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    int num = CalcCell(i, j);
                    if (state[i, j] == 1 && num < 2) {
                        newState[i, j] = 0;
                    } else if (state[i, j] == 1 && num > 3) {
                        newState[i, j] = 0;
                    } else if (state[i, j] == 1 && (num ==2 || num ==3)) {
                        newState[i, j] = 1;
                    } else if (state[i, j] == 0 && num == 3) {
                        newState[i, j] = 1;
                    }
                }
            }

            state = newState;
        }

        private int CalcCell(int a, int b) {
            int sum = 0;
            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    sum += state[ (width + a+i) % width , (height + b+j) % height ];
                }
            }
            sum -= state[a,b];
            return sum;
        }
    }
}
