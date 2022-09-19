namespace Memory_Management
{
    public partial class Form1 : Form
    {
        //sets up the arrays for program names
        //& program sizes
        public string[] ProgramNames = new string[9];
        public int[] ProgramSizes = new int[9];

        //set up variables for program run
        public int ProgramID = 0;
        public int[] LoadOrder = new int[9];
        public int[] PageTable2k = new int[16];
        public int[] PageTable4k = new int[8];
        public int[] PageTable8k = new int[4];
        public int[] PageTableSegmentation = new int[32];

        //form loading and some variable setup
        public Form1()
        {
            //gives values to ProgramNames and ProgramSizes
            ProgramNames[1] = "Internet Browser";
            ProgramNames[2] = "Music Player";
            ProgramNames[3] = "Anti Virus";
            ProgramNames[4] = "Word Processor";
            ProgramNames[5] = "Spreadsheet Editor";
            ProgramNames[6] = "Video Editor";
            ProgramNames[7] = "Small Game";
            ProgramNames[8] = "Large Game";
            //#############################
            ProgramSizes[1] = 6;
            ProgramSizes[2] = 4;
            ProgramSizes[3] = 15;
            ProgramSizes[4] = 5;
            ProgramSizes[5] = 5;
            ProgramSizes[6] = 13;
            ProgramSizes[7] = 7;
            ProgramSizes[8] = 18;

            InitializeComponent();
            //sets the alignment of text
            grid2k.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid4k.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid8k.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridSegmentation.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //sub that makes the selection of the tables transparent
            TransparentSelection();
            
            //sets up the display tables, the division is
            //there to remind me how the splits work when I
            //look back

            //2k
            grid2k.ColumnCount = 1;
            grid2k.RowCount = (32 / 2);
            //4k
            grid4k.ColumnCount = 1;
            grid4k.RowCount = (32 / 4);
            //8k
            grid8k.ColumnCount = 1;
            grid8k.RowCount = (32 / 8);
            //segmentation
            gridSegmentation.ColumnCount = 1;
            gridSegmentation.RowCount = (32 / 1);
        }

        //checkbox interaction
        //##########################################################################
        private void chkInternetBrowser_CheckedChanged_1(object sender, EventArgs e)
        {
            //sets the programID so the subs can load the correct program
            ProgramID = 1;
            //if it is checked it is loaded
            if (chkInternetBrowser.Checked == true)
            {
                LoadProgram();
            } else
            {
                UnloadProgram();
            }
        }

        private void chkMusicPlayer_CheckedChanged(object sender, EventArgs e)
        {
            //sets the programID so the subs can load the correct program
            ProgramID = 2;
            //if it is checked it is loaded
            if (chkMusicPlayer.Checked == true)
            {
                LoadProgram();
            }
            else
            {
                UnloadProgram();
            }
        }

        private void chkAntiVirus_CheckedChanged(object sender, EventArgs e)
        {
            //sets the programID so the subs can load the correct program
            ProgramID = 3;
            //if it is checked it is loaded
            if (chkAntiVirus.Checked == true)
            {
                LoadProgram();
            }
            else
            {
                UnloadProgram();
            }
        }

        private void chkWordProcessor_CheckedChanged(object sender, EventArgs e)
        {
            //sets the programID so the subs can load the correct program
            ProgramID = 4;
            //if it is checked it is loaded
            if (chkWordProcessor.Checked == true)
            {
                LoadProgram();
            }
            else
            {
                UnloadProgram();
            }
        }

        private void chkSpreadsheetEditor_CheckedChanged(object sender, EventArgs e)
        {
            //sets the programID so the subs can load the correct program
            ProgramID = 5;
            //if it is checked it is loaded
            if (chkSpreadsheetEditor.Checked == true)
            {
                LoadProgram();
            }
            else
            {
                UnloadProgram();
            }
        }

        private void chkVideoEditor_CheckedChanged(object sender, EventArgs e)
        {
            //sets the programID so the subs can load the correct program
            ProgramID = 6;
            //if it is checked it is loaded
            if (chkVideoEditor.Checked == true)
            {
                LoadProgram();
            }
            else
            {
                UnloadProgram();
            }
        }

        private void chkSmallGame_CheckedChanged(object sender, EventArgs e)
        {
            //sets the programID so the subs can load the correct program
            ProgramID = 7;
            //if it is checked it is loaded
            if (chkSmallGame.Checked == true)
            {
                LoadProgram();
            }
            else
            {
                UnloadProgram();
            }
        }

        private void chkLargeGame_CheckedChanged(object sender, EventArgs e)
        {
            //sets the programID so the subs can load the correct program
            ProgramID = 8;
            //if it is checked it is loaded
            if (chkLargeGame.Checked == true)
            {
                LoadProgram();
            }
            else
            {
                UnloadProgram();
            }
        }

        //program loading
        //##########################################################################

        public void LoadProgram()
        {
            //easier to just run the subs separately
            Lpaging8k();
            Lpaging4k();
            Lpaging2k();
            Lsegmentation();
            PanelRefresh();
            LoadOrderOnLoad();
        }

        public void Lpaging8k()
        {
            bool valid = false;
            //variables setup
            //the f casts it to a float
            float Divisor = 0f;
            int SegmentsFilled = 0, SegmentsAvailable = 0;

            //the paging number also needs to be cast as a float..
            Divisor = ProgramSizes[ProgramID] / 8f;
            //this will round it *up*, always up, and cast it 
            Divisor = (float)Math.Ceiling(Divisor);

            do
            {
                SegmentsAvailable = 0;

                //check if the table has room for the segments needed
                for (int j = 0; j < (32 / 8); j++)
                {

                    //if the value is empty available space is incremented
                    if (PageTable8k[j] == 0)
                    {
                        SegmentsAvailable++;
                    }
                }

                //if there is space
                if (SegmentsAvailable >= Divisor)
                {
                    //space and so the loop is validated
                    valid = true;
                    //loop the full table
                    for (int i = 0; i < (32 / 8); i++)
                    {
                        //if the value is empty sets it as the new val
                        if (PageTable8k[i] == 0 && SegmentsFilled < Divisor)
                        {
                            PageTable8k[i] = ProgramID;
                            SegmentsFilled++;
                        }
                    }
                    //if there isn't space it can't load
                    //massive L if you want to open an extra tab in [Internet Browser]
                }
                else
                {
                    //MessageBox.Show("4k Paging is full, please close some programs to free up memory");
                    //unchecks the most recently checked checkbox, which evidently couldn't be loaded
                    //UncheckTheUncheckable();
                    int tmp = ProgramID;
                    for (int i = 0; i <= 8; i++)
                    {
                        if (LoadOrder[i] != 0)
                        {
                            ProgramID = LoadOrder[i];
                        }
                    }
                    UncheckTheUncheckable();
                    ProgramID = tmp;
                }
            } while (valid == false);
        }

        public void Lpaging4k()
        {
            bool valid = false;
            //variables setup
            //the f casts it to a float
            float Divisor = 0f;
            int SegmentsFilled = 0, SegmentsAvailable = 0;

            //the paging number also needs to be cast as a float..
            Divisor = ProgramSizes[ProgramID] / 4f;
            //this will round it *up*, always up, and cast it 
            Divisor = (float)Math.Ceiling(Divisor);

            do
            {
                SegmentsAvailable = 0;

                //check if the table has room for the segments needed
                for (int j = 0; j < (32 / 4); j++)
                {

                    //if the value is empty available space is incremented
                    if (PageTable4k[j] == 0)
                    {
                        SegmentsAvailable++;
                    }
                }

                //if there is space
                if (SegmentsAvailable >= Divisor)
                {
                    //space and so the loop is validated
                    valid = true;
                    //loop the full table
                    for (int i = 0; i < (32 / 4); i++)
                    {
                        //if the value is empty sets it as the new val
                        if (PageTable4k[i] == 0 && SegmentsFilled < Divisor)
                        {
                            PageTable4k[i] = ProgramID;
                            SegmentsFilled++;
                        }
                    }
                    //if there isn't space it can't load
                    //massive L if you want to open an extra tab in [Internet Browser]
                }
                else
                {
                    //MessageBox.Show("4k Paging is full, please close some programs to free up memory");
                    //unchecks the most recently checked checkbox, which evidently couldn't be loaded
                    //UncheckTheUncheckable();
                    int tmp = ProgramID;
                    for (int i = 0; i <= 8; i++)
                    {
                        if (LoadOrder[i] != 0)
                        {
                            ProgramID = LoadOrder[i];
                        }
                    }
                    UncheckTheUncheckable();
                    ProgramID = tmp;
                }
            } while (valid == false);
        }

        public void Lpaging2k()
        {
            bool valid = false;
            //variables setup
            //the f casts it to a float
            float Divisor = 0f;
            int SegmentsFilled = 0, SegmentsAvailable = 0;

            //the paging number also needs to be cast as a float..
            Divisor = ProgramSizes[ProgramID] / 2f;
            //this will round it *up*, always up, and cast it 
            Divisor = (float)Math.Ceiling(Divisor);

            do
            {
                SegmentsAvailable = 0;

                //check if the table has room for the segments needed
                for (int j = 0; j < (32 / 2); j++)
                {
                    //if the value is empty available space is incremented
                    if (PageTable2k[j] == 0)
                    {
                        SegmentsAvailable++;
                    }
                }

                //if there is space
                if (SegmentsAvailable >= Divisor)
                {
                    //space and so the loop is validated
                    valid = true;
                    //loop the full table
                    for (int i = 0; i < (32 / 2); i++)
                    {
                        //if the value is empty sets it as the new val
                        if (PageTable2k[i] == 0 && SegmentsFilled < Divisor)
                        {
                            PageTable2k[i] = ProgramID;
                            SegmentsFilled++;
                        }
                    }
                    //if there isn't space it can't load
                    //massive L if you want to open an extra tab in [Internet Browser]
                }
                else
                {
                    //MessageBox.Show("2k Paging is full, please close some programs to free up memory");
                    //unchecks the most recently checked checkbox, which evidently couldn't be loaded
                    //UncheckTheUncheckable();
                    int tmp = ProgramID;
                    for (int i = 0; i <= 8; i++)
                    {
                        if (LoadOrder[i] != 0)
                        {
                            ProgramID = LoadOrder[i];
                        }
                    }
                    UncheckTheUncheckable();
                    ProgramID = tmp;
                }
            } while (valid == false);
        }

        public void Lsegmentation()
        {
            bool valid = false;
            //variables setup
            //the f casts it to a float
            float Divisor = 0f;
            int SegmentsFilled = 0, SegmentsAvailable = 0;
            Divisor = ProgramSizes[ProgramID];

            do
            {
                SegmentsAvailable = 0;

                //check if the table has room for the segments needed
                for (int j = 0; j < 32; j++)
                {
                    //if the value is empty available space is incremented
                    if (PageTableSegmentation[j] == 0)
                    {
                        SegmentsAvailable++;
                    }
                }

                //if there is space
                if (SegmentsAvailable >= Divisor)
                {
                    //space and so the loop is validated
                    valid = true;
                    //loop the full table
                    for (int i = 0; i < 32; i++)
                    {
                        //if the value is empty sets it as the new val
                        if (PageTableSegmentation[i] == 0 && SegmentsFilled < Divisor)
                        {
                            PageTableSegmentation[i] = ProgramID;
                            SegmentsFilled++;
                        }
                    }
                    //if there isn't space it can't load
                    //massive L if you want to open an extra tab in [Internet Browser]
                }
                else
                {
                    //MessageBox.Show("Segmentation is full, please close some programs to free up memory");
                    //unchecks the most recently checked checkbox, which evidently couldn't be loaded
                    //UncheckTheUncheckable();
                    int tmp = ProgramID;
                    for (int i = 0; i <= 8; i++)
                    {
                        if (LoadOrder[i] != 0)
                        {
                            ProgramID = LoadOrder[i];
                        }
                    }
                    UncheckTheUncheckable();
                    ProgramID = tmp;
                }
            } while (valid == false);
        }

        //program unloading
        //####################################################################

        public void UnloadProgram()
        {
            Usegmentation();
            Upaging2k();
            Upaging4k();
            Upaging8k();
            PanelRefresh();
            LoadOrderOnUnload();
        }

        public void Upaging8k()
        {
            //loops the pagetable
            for (int c = 0; c < (32 / 8); c++)
            {
                //checks if the page table row matches the program we are trying to remove
                if ((PageTable8k[c] == ProgramID))
                {
                    //clears the table value
                    PageTable8k[c] = 0;
                }
            }
        }

        public void Upaging4k()
        {
            //loops the pagetable
            for (int c = 0; c < (32 / 4); c++)
            {
                //checks if the page table row matches the program we are trying to remove
                if ((PageTable4k[c] == ProgramID))
                {
                    //clears the table value
                    PageTable4k[c] = 0;
                }
            }
        }

        public void Upaging2k()
        {
            //loops the pagetable
            for (int c = 0; c < (32 / 2); c++)
            {
                //checks if the page table row matches the program we are trying to remove
                if ((PageTable2k[c] == ProgramID))
                {
                    //clears the table value
                    PageTable2k[c] = 0;
                }
            }
        }

        public void Usegmentation()
        {
            //loops the pagetable
            for (int c = 0; c < 32; c++)
            {
                //checks if the page table row matches the program we are trying to remove
                if ((PageTableSegmentation[c] == ProgramID))
                {
                    //clears the table value
                    PageTableSegmentation[c] = 0;
                }
            }
        }

        //refresh subs
        //#######################################################################
        public void TransparentSelection()
        {
            //it's kinda sketch so if it's a sub I can disable and reenable it at will
            //makes the selection transparent in the tables
            grid2k.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            grid4k.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            grid8k.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            gridSegmentation.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;

            grid2k.RowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Transparent;
            grid4k.RowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Transparent;
            grid8k.RowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Transparent;
            gridSegmentation.RowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Transparent;

        }

        public void PanelRefresh()
        {
            //probably a more efficient way but if I start
            //making everything in the loop a variable
            //it will make my life hell
            //I'd rather copy and paste than have something utterly unreadable
            //if your computer can't run this maybe you should
            //stop learning about how computers work
            //and get a better one instead

            //8k paging table
            for (int eightk = 0; eightk < (32 / 8); eightk++)
            {
                switch (PageTable8k[eightk])
                {
                    default:
                        grid8k.Rows[eightk].Cells[0].Value = ProgramNames[PageTable8k[eightk]];
                        grid8k.Rows[eightk].Cells[0].Style.BackColor = Color.White;
                        break;
                    case 1:
                        grid8k.Rows[eightk].Cells[0].Value = ProgramNames[PageTable8k[eightk]];
                        grid8k.Rows[eightk].Cells[0].Style.BackColor = Color.IndianRed;
                        break;
                    case 2:
                        grid8k.Rows[eightk].Cells[0].Value = ProgramNames[PageTable8k[eightk]];
                        grid8k.Rows[eightk].Cells[0].Style.BackColor = Color.LightBlue;
                        break;
                    case 3:
                        grid8k.Rows[eightk].Cells[0].Value = ProgramNames[PageTable8k[eightk]];
                        grid8k.Rows[eightk].Cells[0].Style.BackColor = Color.MistyRose;
                        break;
                    case 4:
                        grid8k.Rows[eightk].Cells[0].Value = ProgramNames[PageTable8k[eightk]];
                        grid8k.Rows[eightk].Cells[0].Style.BackColor = Color.AntiqueWhite;
                        break;
                    case 5:
                        grid8k.Rows[eightk].Cells[0].Value = ProgramNames[PageTable8k[eightk]];
                        grid8k.Rows[eightk].Cells[0].Style.BackColor = Color.DarkSeaGreen;
                        break;
                    case 6:
                        grid8k.Rows[eightk].Cells[0].Value = ProgramNames[PageTable8k[eightk]];
                        grid8k.Rows[eightk].Cells[0].Style.BackColor = Color.Plum;
                        break;
                    case 7:
                        grid8k.Rows[eightk].Cells[0].Value = ProgramNames[PageTable8k[eightk]];
                        grid8k.Rows[eightk].Cells[0].Style.BackColor = Color.Aquamarine;
                        break;
                    case 8:
                        grid8k.Rows[eightk].Cells[0].Value = ProgramNames[PageTable8k[eightk]];
                        grid8k.Rows[eightk].Cells[0].Style.BackColor = Color.LightSteelBlue;
                        break;
                    case 9:
                        grid8k.Rows[eightk].Cells[0].Value = ProgramNames[PageTable8k[eightk]];
                        grid8k.Rows[eightk].Cells[0].Style.BackColor = Color.DarkGoldenrod;
                        break;
                }
            }

            //4k paging table
            for (int fourk = 0; fourk < (32 / 4); fourk++)
            {
                switch (PageTable4k[fourk])
                {
                    default:
                        grid4k.Rows[fourk].Cells[0].Value = ProgramNames[PageTable4k[fourk]];
                        grid4k.Rows[fourk].Cells[0].Style.BackColor = Color.White;
                        break;
                    case 1:
                        grid4k.Rows[fourk].Cells[0].Value = ProgramNames[PageTable4k[fourk]];
                        grid4k.Rows[fourk].Cells[0].Style.BackColor = Color.IndianRed;
                        break;
                    case 2:
                        grid4k.Rows[fourk].Cells[0].Value = ProgramNames[PageTable4k[fourk]];
                        grid4k.Rows[fourk].Cells[0].Style.BackColor = Color.LightBlue;
                        break;
                    case 3:
                        grid4k.Rows[fourk].Cells[0].Value = ProgramNames[PageTable4k[fourk]];
                        grid4k.Rows[fourk].Cells[0].Style.BackColor = Color.MistyRose;
                        break;
                    case 4:
                        grid4k.Rows[fourk].Cells[0].Value = ProgramNames[PageTable4k[fourk]];
                        grid4k.Rows[fourk].Cells[0].Style.BackColor = Color.AntiqueWhite;
                        break;
                    case 5:
                        grid4k.Rows[fourk].Cells[0].Value = ProgramNames[PageTable4k[fourk]];
                        grid4k.Rows[fourk].Cells[0].Style.BackColor = Color.DarkSeaGreen;
                        break;
                    case 6:
                        grid4k.Rows[fourk].Cells[0].Value = ProgramNames[PageTable4k[fourk]];
                        grid4k.Rows[fourk].Cells[0].Style.BackColor = Color.Plum;
                        break;
                    case 7:
                        grid4k.Rows[fourk].Cells[0].Value = ProgramNames[PageTable4k[fourk]];
                        grid4k.Rows[fourk].Cells[0].Style.BackColor = Color.Aquamarine;
                        break;
                    case 8:
                        grid4k.Rows[fourk].Cells[0].Value = ProgramNames[PageTable4k[fourk]];
                        grid4k.Rows[fourk].Cells[0].Style.BackColor = Color.LightSteelBlue;
                        break;
                    case 9:
                        grid4k.Rows[fourk].Cells[0].Value = ProgramNames[PageTable4k[fourk]];
                        grid4k.Rows[fourk].Cells[0].Style.BackColor = Color.DarkGoldenrod;
                        break;
                }
            }

            //2k paging table
            for (int twok = 0; twok < (32 / 2); twok++)
            {
                switch (PageTable2k[twok])
                {
                    default:
                        grid2k.Rows[twok].Cells[0].Value = ProgramNames[PageTable2k[twok]];
                        grid2k.Rows[twok].Cells[0].Style.BackColor = Color.White;
                        break;
                    case 1:
                        grid2k.Rows[twok].Cells[0].Value = ProgramNames[PageTable2k[twok]];
                        grid2k.Rows[twok].Cells[0].Style.BackColor = Color.IndianRed;
                        break;
                    case 2:
                        grid2k.Rows[twok].Cells[0].Value = ProgramNames[PageTable2k[twok]];
                        grid2k.Rows[twok].Cells[0].Style.BackColor = Color.LightBlue;
                        break;
                    case 3:
                        grid2k.Rows[twok].Cells[0].Value = ProgramNames[PageTable2k[twok]];
                        grid2k.Rows[twok].Cells[0].Style.BackColor = Color.MistyRose;
                        break;
                    case 4:
                        grid2k.Rows[twok].Cells[0].Value = ProgramNames[PageTable2k[twok]];
                        grid2k.Rows[twok].Cells[0].Style.BackColor = Color.AntiqueWhite;
                        break;
                    case 5:
                        grid2k.Rows[twok].Cells[0].Value = ProgramNames[PageTable2k[twok]];
                        grid2k.Rows[twok].Cells[0].Style.BackColor = Color.DarkSeaGreen;
                        break;
                    case 6:
                        grid2k.Rows[twok].Cells[0].Value = ProgramNames[PageTable2k[twok]];
                        grid2k.Rows[twok].Cells[0].Style.BackColor = Color.Plum;
                        break;
                    case 7:
                        grid2k.Rows[twok].Cells[0].Value = ProgramNames[PageTable2k[twok]];
                        grid2k.Rows[twok].Cells[0].Style.BackColor = Color.Aquamarine;
                        break;
                    case 8:
                        grid2k.Rows[twok].Cells[0].Value = ProgramNames[PageTable2k[twok]];
                        grid2k.Rows[twok].Cells[0].Style.BackColor = Color.LightSteelBlue;
                        break;
                    case 9:
                        grid2k.Rows[twok].Cells[0].Value = ProgramNames[PageTable2k[twok]];
                        grid2k.Rows[twok].Cells[0].Style.BackColor = Color.DarkGoldenrod;
                        break;
                }
            }

            //segmentation paging table
            for (int seg = 0; seg < 32; seg++)
            {
                switch (PageTableSegmentation[seg])
                {
                    default:
                        gridSegmentation.Rows[seg].Cells[0].Value = ProgramNames[PageTableSegmentation[seg]];
                        gridSegmentation.Rows[seg].Cells[0].Style.BackColor = Color.White;
                        break;
                    case 1:
                        gridSegmentation.Rows[seg].Cells[0].Value = ProgramNames[PageTableSegmentation[seg]];
                        gridSegmentation.Rows[seg].Cells[0].Style.BackColor = Color.IndianRed;
                        break;
                    case 2:
                        gridSegmentation.Rows[seg].Cells[0].Value = ProgramNames[PageTableSegmentation[seg]];
                        gridSegmentation.Rows[seg].Cells[0].Style.BackColor = Color.LightBlue;
                        break;
                    case 3:
                        gridSegmentation.Rows[seg].Cells[0].Value = ProgramNames[PageTableSegmentation[seg]];
                        gridSegmentation.Rows[seg].Cells[0].Style.BackColor = Color.MistyRose;
                        break;
                    case 4:
                        gridSegmentation.Rows[seg].Cells[0].Value = ProgramNames[PageTableSegmentation[seg]];
                        gridSegmentation.Rows[seg].Cells[0].Style.BackColor = Color.AntiqueWhite;
                        break;
                    case 5:
                        gridSegmentation.Rows[seg].Cells[0].Value = ProgramNames[PageTableSegmentation[seg]];
                        gridSegmentation.Rows[seg].Cells[0].Style.BackColor = Color.DarkSeaGreen;
                        break;
                    case 6:
                        gridSegmentation.Rows[seg].Cells[0].Value = ProgramNames[PageTableSegmentation[seg]];
                        gridSegmentation.Rows[seg].Cells[0].Style.BackColor = Color.Plum;
                        break;
                    case 7:
                        gridSegmentation.Rows[seg].Cells[0].Value = ProgramNames[PageTableSegmentation[seg]];
                        gridSegmentation.Rows[seg].Cells[0].Style.BackColor = Color.Aquamarine;
                        break;
                    case 8:
                        gridSegmentation.Rows[seg].Cells[0].Value = ProgramNames[PageTableSegmentation[seg]];
                        gridSegmentation.Rows[seg].Cells[0].Style.BackColor = Color.LightSteelBlue;
                        break;
                    case 9:
                        gridSegmentation.Rows[seg].Cells[0].Value = ProgramNames[PageTableSegmentation[seg]];
                        gridSegmentation.Rows[seg].Cells[0].Style.BackColor = Color.DarkGoldenrod;
                        break;
                }
            }

            //selects the top row of every column to auto
            //refresh the column
            grid2k.ClearSelection();
            grid2k.Rows[(32 / 2) - 1].Cells[0].Selected = true;
            grid4k.ClearSelection();
            grid4k.Rows[(32 / 4) - 1].Cells[0].Selected = true;
            grid8k.ClearSelection();
            grid8k.Rows[(32 / 8) - 1].Cells[0].Selected = true;
            gridSegmentation.ClearSelection();
            gridSegmentation.Rows[32 - 1].Cells[0].Selected = true;
        }

        public void UncheckTheUncheckable()
        {
            //switches the ProgramID to determine what to do
            //will uncheck the checkbox, this also triggers the
            //unload function but it isn't destructive so no worries
            switch (ProgramID)
            {
                default:
                    chkInternetBrowser.Checked = false;
                    break;
                case 2:
                    chkMusicPlayer.Checked = false;
                    break;
                case 3:
                    chkAntiVirus.Checked = false;
                    break;
                case 4:
                    chkWordProcessor.Checked = false;
                    break;
                case 5:
                    chkSpreadsheetEditor.Checked = false;
                    break;
                case 6:
                    chkVideoEditor.Checked = false;
                    break;
                case 7:
                    chkSmallGame.Checked = false;
                    break;
                case 8:
                    chkLargeGame.Checked = false;
                    break;
            }
        }

        public void LoadOrderOnLoad()
        {
            int LowestEmpty = 0;

            //remove the ProgramID from the list
            for (int c = 0; c <= 8; c++)
            {
                if (LoadOrder[c] == ProgramID)
                {
                    //clear it
                    LoadOrder[c] = 0;
                    //can't show up more than once
                    break;
                }
            }

            //gravity
            //gravity drops the rest of the column
            //loop it 8 times to get the result desired (like bubble sort)
            for (int a = 0; a < 8; a++)
            {
                //loop bottom up to find lowest empty
                for (int b = 8; b >= 0; b--)
                {
                    if (LoadOrder[b] == 0)
                    {
                        //sets it to the value
                        //exits loop
                        LowestEmpty = b;
                        break;
                    } //no if needed because as long as the removal
                      //works it's all safe
                }

                //loops empty upward
                for (int c = LowestEmpty; c >= 0; c--)
                {
                    //if the value is filled
                    if (LoadOrder[c] != 0)
                    {
                        //moves the value down
                        LoadOrder[LowestEmpty] = LoadOrder[c];
                        LoadOrder[c] = 0;
                        break;
                    }
                }
            }


            //add to the top
            for (int c = 8; c >= 0; c--)
            {
                //if the lowest slot is empty, add there
                if (LoadOrder[c] == 0)
                {
                    LoadOrder[c] = ProgramID;
                    break;
                }
            }
        }

        public void LoadOrderOnUnload()
        {
            //var setup
            int LowestEmpty = 0;

            //remove the ProgramID from the list
            for (int c = 0; c <= 8; c++)
            {
                if (LoadOrder[c] == ProgramID)
                {
                    //clear it
                    LoadOrder[c] = 0;
                    //can't show up more than once
                    break;
                }
            }

            //checks the load order for the ProgramID
            for (int j = 0; j < 8; j++)
            {
                //removes program ID
                if (LoadOrder[j] == ProgramID)
                {
                    LoadOrder[j] = 0;

                    //gravity
                    //gravity drops the rest of the column
                    //loop it 8 times to get the result desired (like bubble sort)
                    for (int a = 0; a < 8; a++)
                    {
                        //loop bottom up to find lowest empty
                        for (int b = 8; b >= 0; b--)
                        {
                            if (LoadOrder[b] == 0)
                            {
                                //sets it to the value
                                //exits loop
                                LowestEmpty = b;
                                break;
                            } //no if needed because as long as the removal
                              //works it's all safe
                        }

                        //loops empty upward
                        for (int c = LowestEmpty; c >= 0; c--)
                        {
                            //if the value is filled
                            if (LoadOrder[c] != 0)
                            {
                                //moves the value down
                                LoadOrder[LowestEmpty] = LoadOrder[c];
                                LoadOrder[c] = 0;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //literally just unchecks them all
            //this triggers the unload code for them all
            //checkboxes my beloved
            chkInternetBrowser.Checked = false;
            chkMusicPlayer.Checked = false;
            chkAntiVirus.Checked = false;
            chkWordProcessor.Checked = false;
            chkSpreadsheetEditor.Checked = false;
            chkVideoEditor.Checked = false;
            chkSmallGame.Checked = false;
            chkLargeGame.Checked = false;
        }
    }
}