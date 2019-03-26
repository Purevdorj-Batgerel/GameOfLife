using CommonCode;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Game_of_Life {

    public partial class ScreenSaverForm : Form {
        private int paintStatus = PaintStates.NoActivity;
        private bool drawCalled = false;

        private Point mouseXY;

        private CommonFunctions cf = new CommonFunctions();

        DonePaintingDelegate donePaintingDelegate;
        ShutDownDelegate shutDownDelegate;

        GameOfLife gameOfLife;

        private int screenNumber;
        private bool stopNow = false;

        public ScreenSaverForm(int scrn,
            DonePaintingDelegate donePaintingDelegate,
            ShutDownDelegate shutDownDelegate) {

            InitializeComponent();
            screenNumber = scrn;
            Bounds = Screen.AllScreens[screenNumber].Bounds;
            this.donePaintingDelegate = donePaintingDelegate;
            this.shutDownDelegate = shutDownDelegate;

            gameOfLife = new GameOfLife(32);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // Name explains it.
            SetStyle(ControlStyles.Opaque, true); // Background is taken care of by DrawImage.
            SetStyle(ControlStyles.UserPaint, true); // if Allpainting is on, so must this.
            SetStyle(ControlStyles.DoubleBuffer, true);
            UpdateStyles();
        }

        private void ScreenSaverForm_Load(object sender, EventArgs e) {

            Cursor.Hide();
            TopMost = false; //?
            gameOfLife.Init(Bounds.Width, Bounds.Height);
        }

        public void CloseMe() {
            stopNow = true;
            paintStatus = PaintStates.ShuttingDown;
            gameOfLife.StopNow = true;
            Close();
        }

        private void OnMouseEvent(object sender, MouseEventArgs e) {
            if (!mouseXY.IsEmpty) {
                if (mouseXY != new Point(e.X, e.Y) || e.Clicks > 0)
                    shutDownDelegate();
            }
            mouseXY = new Point(e.X, e.Y);
        }

        private void OnKeyEvent(object sender, KeyEventArgs e) {
            shutDownDelegate();
        }

        public void Draw() {
            if (stopNow == false) {
                drawCalled = true;
                paintStatus = PaintStates.OurPaintPending;
                Refresh();
            } else {
                if (paintStatus != PaintStates.ShuttingDown)
                    paintStatus = PaintStates.ShuttingDown;
            }
        }

        public int PaintStatus {
            get { return paintStatus; }
        }

        private void ScreenSaverForm_Paint(object sender, PaintEventArgs e) {
            if (stopNow == true) {
                if (paintStatus != PaintStates.ShuttingDown) {
                    paintStatus = PaintStates.ShuttingDown;
                }
                return;
            }

            if (e.ClipRectangle.Width != this.Bounds.Width ||
                e.ClipRectangle.Height != this.Bounds.Height ||
                (drawCalled == false)) {
                paintStatus = PaintStates.OtherPaint; // Unwanted paint found.
                donePaintingDelegate(screenNumber);
                return;
            }

            if (paintStatus == PaintStates.PaintInProgress) {
                return;
            }

            paintStatus = PaintStates.PaintInProgress;

            try {
                SuspendLayout();
                gameOfLife.DrawThenCalc(e.Graphics);
                ResumeLayout();

                paintStatus = PaintStates.PaintSuccessful;
                drawCalled = false;
                donePaintingDelegate(screenNumber);
            } catch (Exception ex) {
                Cursor.Show();
                throw ex;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
        }

        protected override void OnSizeChanged(EventArgs e) {
        }

        protected override void OnForeColorChanged(EventArgs e) {
        }
    }
}
