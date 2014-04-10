using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;


namespace GZM.TP.WinWidget
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
    {

        #region Static Member
        //Valeur de la config
        static string htmlfn = Properties.Settings.Default.widgetUrl;  //URL
        static int ww = Properties.Settings.Default.widgetWidth;  //Window Width
        static int hh = Properties.Settings.Default.widgetHeight;  //Window Height
        
        static int mww = 10;  //Margin Width
        static int mhh = 10;  //Margin Height

        static Color bgcolor = Color.Black;

        #region move window while drag in the client area

        /// <summary>
        /// Mouse Button Down
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;

        /// <summary>
        /// 
        /// </summary>
        public const int HTCAPTION = 0x2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        #endregion

        #endregion


        /// <summary>
        /// 
        /// </summary>
		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Event
		private void Form1_Load(object sender, System.EventArgs e)
		{
            this.TopMost = true; //Always on top
            this.mnuAlwaysOnTop.Checked = true;

            this.ShowInTaskbar = true; // Dans la barre des taches
            this.mnuShowInTaskbar.Checked = true;


            //Set de la transparence
            this.mnuTransparency0.Checked = true;
            this.mnuTransparency25.Checked = false;
            this.mnuTransparency50.Checked = false;
            this.mnuTransparency75.Checked = false;


            UriBuilder myUri = new UriBuilder(htmlfn);
            webBrowser1.Url = myUri.Uri;


			this.Width = ww;
			this.Height = hh;
			this.BackColor = bgcolor;

            resizeBrowser();


            //=== Commence le décompte du "hidding" du titre
            this.timer1.Enabled = true;

		}

        private void Form1_MouseLeave(object sender, System.EventArgs e)
        {
                this.timer1.Enabled = true;
        }

        private void Form1_MouseEnter(object sender, System.EventArgs e)
        {
            showTitle();
            this.timer1.Enabled = false;
        }

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            resizeBrowser();
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            hideTitle();
            this.timer1.Enabled = false;
        }

        /// <summary>
        /// Met le titre de la page Web dans la title bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser doc = (WebBrowser)sender;

            if (doc.DocumentTitle != null) this.Text = doc.DocumentTitle;

        }

        #region menu actions

        // Exit ...
        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Toujours Visible
        private void mnuAlwaysOnTop_Click(object sender, EventArgs e)
        {

            //=== Si coché
            if (mnuAlwaysOnTop.Checked)
            {
                this.TopMost = false; //Always on top       
                mnuAlwaysOnTop.Checked = false; //=== On décoche

            }
            else
            {
                this.TopMost = true; //Always on top
                mnuAlwaysOnTop.Checked = true; //=== On décoche
            }

        }

        // Afficher dans la barre de taches
        private void mnuShowInTaskbar_Click(object sender, EventArgs e)
        {
            //=== Si coché
            if (mnuShowInTaskbar.Checked)
            {
                this.ShowInTaskbar = false; //Apparait dans la barre d'outil
                mnuShowInTaskbar.Checked = false; //=== On décoche

            }
            else
            {
                this.ShowInTaskbar = true; //Apparait dans la barre d'outil
                mnuShowInTaskbar.Checked = true;
            }
        }


        private void mnuShowMenu_Click(object sender, EventArgs e)
        {
            //=== Si coché
            if (mnuShowMenu.Checked)
            {
                hideTitle();
                mnuShowMenu.Checked = false; //=== On décoche

            }
            else
            {
                showTitle();
                mnuShowMenu.Checked = true; //=== On coche
            }
        }


        private void mnuTransparency75_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.25;
            this.mnuTransparency0.Checked = false;
            this.mnuTransparency25.Checked = false;
            this.mnuTransparency50.Checked = false;
            this.mnuTransparency75.Checked = true;

        }

        private void mnuTransparency50_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.50;
            this.mnuTransparency0.Checked = false;
            this.mnuTransparency25.Checked = false;
            this.mnuTransparency50.Checked = true;
            this.mnuTransparency75.Checked = false;
        }

        private void mnuTransparency25_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.75;
            this.mnuTransparency0.Checked = false;
            this.mnuTransparency25.Checked = true;
            this.mnuTransparency50.Checked = false;
            this.mnuTransparency75.Checked = false;
        }

        private void mnuTransparency0_Click(object sender, EventArgs e)
        {
            this.Opacity = 1;
            this.mnuTransparency0.Checked = true;
            this.mnuTransparency25.Checked = false;
            this.mnuTransparency50.Checked = false;
            this.mnuTransparency75.Checked = false;
        }
        #endregion

		#endregion
        
        #region Member

        private void resizeBrowser()
        {
            webBrowser1.Left = mww / 2;
            webBrowser1.Top = mhh / 2;

            webBrowser1.Width = this.ClientSize.Width - mww;
            webBrowser1.Height = this.ClientSize.Height - mhh;
        }

        #region show / hide title

        private void hideTitle()
		{
            if (!this.mnuShowMenu.Checked)
            {
                GraphicsPath gPath = new System.Drawing.Drawing2D.GraphicsPath();
                int hh = (this.Height - this.ClientSize.Height);
                int ww = mww / 2;
                gPath.AddRectangle(new RectangleF(0 + 3, hh, webBrowser1.Width + mww, webBrowser1.Height + mhh - 3));
                this.Region = new System.Drawing.Region(gPath);
            }
		}

		private void showTitle()
		{
			GraphicsPath gPath = new System.Drawing.Drawing2D.GraphicsPath();
			gPath.AddRectangle(new RectangleF(0, 0, this.Width, this.Height));
			this.Region = new System.Drawing.Region(gPath);
		}

		#endregion

        #endregion
                
        #region main function and parameter parsing
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            for (int ii = 0; ii < args.Length && args[ii][0] == '-'; ++ii)
            {
                if (args[ii].Equals("-html") && ii + 1 < args.Length)
                {
                    htmlfn = args[++ii];
                }
                else if (args[ii].Equals("-width") && ii + 1 < args.Length)
                {
                    try
                    {
                        ww = Convert.ToInt32(args[++ii]);
                    }
                    catch { }
                }
                else if (args[ii].Equals("-height") && ii + 1 < args.Length)
                {
                    try
                    {
                        hh = Convert.ToInt32(args[++ii]);
                    }
                    catch { }
                }
                else if (args[ii].Equals("-bgcolor") && ii + 1 < args.Length)
                {
                    try
                    {
                        ColorConverter converter = new ColorConverter();
                        bgcolor = (Color)converter.ConvertFrom(args[++ii]);
                    }
                    catch { }
                }
            }

            Application.Run(new Form1());
        }
        #endregion

        #region Windows Form Designer generated code
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem mnuFile;
        private System.Windows.Forms.MenuItem mnuExit;
        private MenuItem mnuShowMenu;
        private MenuItem mnuDisplay;
        private MenuItem mnuAlwaysOnTop;
        private WebBrowser webBrowser1;
        private MenuItem mnuShowInTaskbar;
        private MenuItem mnuTransparency;
        private MenuItem mnuTransparency75;
        private MenuItem mnuTransparency50;
        private MenuItem mnuTransparency25;
        private MenuItem mnuTransparency0;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.mnuDisplay = new System.Windows.Forms.MenuItem();
            this.mnuAlwaysOnTop = new System.Windows.Forms.MenuItem();
            this.mnuShowInTaskbar = new System.Windows.Forms.MenuItem();
            this.mnuTransparency = new System.Windows.Forms.MenuItem();
            this.mnuTransparency75 = new System.Windows.Forms.MenuItem();
            this.mnuTransparency50 = new System.Windows.Forms.MenuItem();
            this.mnuTransparency25 = new System.Windows.Forms.MenuItem();
            this.mnuTransparency0 = new System.Windows.Forms.MenuItem();
            this.mnuShowMenu = new System.Windows.Forms.MenuItem();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.mnuDisplay});
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuExit});
            this.mnuFile.Text = "&Fichier";
            // 
            // mnuExit
            // 
            this.mnuExit.Index = 0;
            this.mnuExit.Text = "Quitter";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuDisplay
            // 
            this.mnuDisplay.Index = 1;
            this.mnuDisplay.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuAlwaysOnTop,
            this.mnuShowInTaskbar,
            this.mnuTransparency,
            this.mnuShowMenu});
            this.mnuDisplay.Text = "&Affichage";
            // 
            // mnuAlwaysOnTop
            // 
            this.mnuAlwaysOnTop.Checked = true;
            this.mnuAlwaysOnTop.Index = 0;
            this.mnuAlwaysOnTop.Text = "Toujour Visible";
            this.mnuAlwaysOnTop.Click += new System.EventHandler(this.mnuAlwaysOnTop_Click);
            // 
            // mnuShowInTaskbar
            // 
            this.mnuShowInTaskbar.Checked = true;
            this.mnuShowInTaskbar.Index = 1;
            this.mnuShowInTaskbar.Text = "Visible dans la barre de tâches";
            this.mnuShowInTaskbar.Click += new System.EventHandler(this.mnuShowInTaskbar_Click);
            // 
            // mnuTransparency
            // 
            this.mnuTransparency.Index = 2;
            this.mnuTransparency.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuTransparency75,
            this.mnuTransparency50,
            this.mnuTransparency25,
            this.mnuTransparency0});
            this.mnuTransparency.Text = "Transparence";
            // 
            // mnuTransparency75
            // 
            this.mnuTransparency75.Checked = true;
            this.mnuTransparency75.Index = 0;
            this.mnuTransparency75.RadioCheck = true;
            this.mnuTransparency75.Text = "75%";
            this.mnuTransparency75.Click += new System.EventHandler(this.mnuTransparency75_Click);
            // 
            // mnuTransparency50
            // 
            this.mnuTransparency50.Index = 1;
            this.mnuTransparency50.RadioCheck = true;
            this.mnuTransparency50.Text = "50%";
            this.mnuTransparency50.Click += new System.EventHandler(this.mnuTransparency50_Click);
            // 
            // mnuTransparency25
            // 
            this.mnuTransparency25.Index = 2;
            this.mnuTransparency25.RadioCheck = true;
            this.mnuTransparency25.Text = "25%";
            this.mnuTransparency25.Click += new System.EventHandler(this.mnuTransparency25_Click);
            // 
            // mnuTransparency0
            // 
            this.mnuTransparency0.Index = 3;
            this.mnuTransparency0.RadioCheck = true;
            this.mnuTransparency0.Text = "0%";
            this.mnuTransparency0.Click += new System.EventHandler(this.mnuTransparency0_Click);
            // 
            // mnuShowMenu
            // 
            this.mnuShowMenu.Checked = true;
            this.mnuShowMenu.DefaultItem = true;
            this.mnuShowMenu.Index = 3;
            this.mnuShowMenu.Text = "&Barre de menus";
            this.mnuShowMenu.Click += new System.EventHandler(this.mnuShowMenu_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(12, 12);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(268, 131);
            this.webBrowser1.TabIndex = 1;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 155);
            this.Controls.Add(this.webBrowser1);
            this.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Text = "HTML Widget";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseEnter += new System.EventHandler(this.Form1_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.Form1_MouseLeave);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);

        }
        #endregion
	}
}



