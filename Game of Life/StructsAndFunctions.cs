using System;
using System.Runtime.InteropServices; // So API functions can be called.

namespace CommonCode {

    public struct PaintStates {
        public const int
        ShuttingDown = -4,
        PaintError = -3,
        OtherPaint = -2,
        NoActivity = -1,
        OurPaintPending = 0,
        PaintInProgress = 1,
        PaintSuccessful = 2;
    }

    public struct RECT {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public RECT(int l, int t, int r, int b) {
            left = l;
            top = t;
            right = r;
            bottom = b;
        }
    }

    public class CommonFunctions {
        [DllImport("user32.DLL", EntryPoint = "IsWindowVisible")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, ref RECT rect);


        public bool IsWindowVisibleApi(IntPtr hWnd) {
            return IsWindowVisible(hWnd);
        }

        public bool GetClientRectApi(IntPtr hWnd, ref RECT rect) {
            return GetClientRect(hWnd, ref rect);
        }

        private Random RandIntGenerator = new Random();

        public int rand() {
            return RandIntGenerator.Next(32768); // Used 32767 Because it returns 0 to Parameter - 1;
        }

        public int RAND(int v) {
            return ((RandIntGenerator.Next(32768) % (v)) - ((v) / 2)); // WE use 32768 because we want this to work with a random in between 0 to 32767 inclusive.
        }

        public int randInRange(int MinVal, int MaxVal) {
            return RandIntGenerator.Next(MinVal, MaxVal + 1);
        }
    }
}
