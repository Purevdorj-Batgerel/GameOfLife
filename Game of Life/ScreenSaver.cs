using CommonCode;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Game_of_Life {

    public delegate void DonePaintingDelegate(int screenNumber);

    public delegate void ShutDownDelegate();

    class ScreenSaver {

        public DonePaintingDelegate donePaintingDel;
        public ShutDownDelegate shutDownDel;

        int screenCount;
        bool shuttingDown = false;

        ScreenSaverForm[] sf;

        bool[] ScreenDonePainting = new bool[Screen.AllScreens.Length];

        ManualResetEvent[] manualEvents;

        public ScreenSaver() {
            manualEvents = new ManualResetEvent[Screen.AllScreens.Length + 1];
            for (int i = 0; i <= Screen.AllScreens.Length; i++) {
                manualEvents[i] = new ManualResetEvent(false);
            }
        }

        private void CloseAllScreens() {
            if (screenCount > 0) {
                screenCount = 0;
                for (int i = 0; i < Screen.AllScreens.Length; i++) {
                    if (sf[i].Created == true && sf[i].Visible == true) {
                        sf[i].CloseMe();
                        Application.DoEvents();
                    }
                    //try {
                        
                    //} catch (Exception ex) {
                    //    if (ex.Message != "LocalDataStoreSlot storage has been freed.")
                    //        throw ex;
                    //}
                }
            }
        }

        ~ScreenSaver() {
            CloseAllScreens();
        }

        public void DonePainting(int screenNumber) {
            lock (ScreenDonePainting) {
                ScreenDonePainting[screenNumber] = true;
                manualEvents[screenNumber].Set();
            }
        }

        public void IfPaintDoneResetStatuses(int screenNumber) {
            if (ScreenDonePainting[screenNumber] == true) {
                ScreenDonePainting[screenNumber] = false;
                manualEvents[screenNumber].Reset();
            }
        }

        public void ShutDown() {
            lock (manualEvents) {
                shuttingDown = true;
                manualEvents[Screen.AllScreens.Length].Set();
            }
        }

        public void RunTillShutdown() {
            donePaintingDel = new DonePaintingDelegate(DonePainting);
            shutDownDel = new ShutDownDelegate(ShutDown);
            screenCount = Screen.AllScreens.Length;

            sf = new ScreenSaverForm[screenCount];
            int i = 0;
            for(i=0; i<screenCount;i++) {
                sf[i] = new ScreenSaverForm(i, donePaintingDel, shutDownDel);
                sf[i].Show();
                sf[i].Draw();
            }

            while(screenCount > 0) {

                WaitHandle.WaitAny(manualEvents, new TimeSpan(0, 0, 0, 2), false);
                if(shuttingDown == true) {
                    CloseAllScreens();
                    continue;
                }

                try {
                    for(i=0; i<Screen.AllScreens.Length; i++) {
                        switch (sf[i].PaintStatus) {
                            case PaintStates.ShuttingDown:
                                continue;
                            case PaintStates.PaintError:
                                IfPaintDoneResetStatuses(i);
                                Application.DoEvents();
                                sf[i].Draw();
                                break;
                            case PaintStates.OtherPaint:
                                IfPaintDoneResetStatuses(i);
                                break;
                            case PaintStates.NoActivity:
                            case PaintStates.OurPaintPending:
                            case PaintStates.PaintInProgress:
                                break;
                            case PaintStates.PaintSuccessful:
                                IfPaintDoneResetStatuses(i);
                                Application.DoEvents();
                                sf[i].Draw();
                                break;
                        }
                    }
                } catch {
                    Cursor.Show();
                    throw;
                }
            }
        }
    }
}
