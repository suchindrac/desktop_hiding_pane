using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;

namespace HidePane
{
    /*
     * Create a class which is a Child of Form
     * 
     */
    public partial class Form1 : Form, IMessageFilter
    {
        private Rectangle rect;
        private Rectangle full_rect;
        // private Region region;
        int screen_width = SystemInformation.VirtualScreen.Width;
	int screen_height = SystemInformation.VirtualScreen.Height;
	int toggle = 1;
	int invert = 0;
	
	[DllImport("user32.dll")]
	private static extern bool SetForegroundWindow(IntPtr hWnd);
	
	[System.Runtime.InteropServices.DllImport("user32.dll")]
	private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
	
	[System.Runtime.InteropServices.DllImport("user32.dll")]
	private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

	[DllImport("user32.dll", SetLastError = true)]
	static extern IntPtr SetFocus(IntPtr hWnd);

	[DllImport("user32.dll")]
	[return:MarshalAs(UnmanagedType.Bool)]
	public static extern bool InvertRect(IntPtr hDC, ref System.Drawing.Rectangle lprc);
	
	
       	enum KeyModifier
	{
	        None = 0,
	        Alt = 1,
		Control = 2,
		Shift = 4,
		WinKey = 8
	}
	
	public bool PreFilterMessage(ref Message m)
	{
		const int WM_KEYUP = 0x101;
		if (m.Msg == WM_KEYUP)
   		{
   			return true;
   		} else {
   			return false;
   		}
   		
   	}
        
        public Form1()
        {
        	Application.AddMessageFilter(this);
        	
        	int id = 0;

		this.FormBorderStyle = FormBorderStyle.None;
            	/*
             	* Initialize component
             	* 
             	*/
            	InitializeComponent();

            	/*
            	 * Form background, border and transparency
            	 * 
            	 */
            	this.Bounds = Screen.PrimaryScreen.Bounds;
            	this.TopMost = true;
            	this.Size = new Size(screen_width, screen_height);
            	this.BackColor = Color.White;
            	this.TransparencyKey  = Color.Black;
		            	    	
            	this.Opacity = 0.6;
            	RegisterHotKey(this.Handle, id, (int)KeyModifier.Shift, Keys.Space.GetHashCode());
            	this.Show();
	}
	
	protected override CreateParams CreateParams
	{
	    get
	    {
		CreateParams createParams = base.CreateParams;
		createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT

		return createParams;
	    }
	}
	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
	       	switch(keyData)
	       	{
			case Keys.V:
				if (invert == 0)
				{
					InvertRegion();
					invert = 1;
				} else {
					InvertRegion();
					invert = 0;
				}
				
				break;
	       		case Keys.I:
	       			this.Opacity = this.Opacity + 0.05;
	       			break;
	       		case Keys.D:
	       			this.Opacity = this.Opacity - 0.05;
	       			break;
	       		case Keys.H:
	    	   		this.rect.Size = new Size(500, 500);
	    	   		Region region_high = new Region(this.full_rect);
				region_high.Exclude(this.rect);
				this.Region = region_high;
				region_high.Dispose();
				break;	       		
			case Keys.L:
	    	   		this.rect.Size = new Size(500, 20);
	    	   		Region region_low = new Region(this.full_rect);
				region_low.Exclude(this.rect);
				this.Region = region_low;
				region_low.Dispose();
				break;	       					
	       		case Keys.Insert:
	       			if (this.toggle == 0)
	       			{
	       				break;
	       			}	       		
	       			this.rect.Inflate(0, 5);
	       			Region region_insert = new Region(this.full_rect);
				region_insert.Exclude(this.rect);
				this.Region = region_insert;
				region_insert.Dispose();
	  			break;
	       		case Keys.Delete:
	       			if (this.toggle == 0)
	       			{
	       				break;
	       			}	       		
	       			this.rect.Inflate(0, -5);
	       			Region region_delete = new Region(this.full_rect);
				region_delete.Exclude(this.rect);
				this.Region = region_delete;
				region_delete.Dispose();
	  			break;
	       		case Keys.PageUp:
	       			if (this.toggle == 0)
	       			{
	       				break;
	       			}	       		
	       			this.rect.Inflate(5, 0);
	       			Region region_pageup = new Region(this.full_rect);
				region_pageup.Exclude(this.rect);
				this.Region = region_pageup;
				region_pageup.Dispose();
	  			break;
	       		case Keys.PageDown:
	       			if (this.toggle == 0)
	       			{
	       				break;
	       			}	       		
	       			this.rect.Inflate(-5, 0);
	       			Region region_pagedown = new Region(this.full_rect);
				region_pagedown.Exclude(this.rect);
				this.Region = region_pagedown;
				region_pagedown.Dispose();
	  			break;
		   	case Keys.Down:
	       			if (this.toggle == 0)
	       			{
	       				break;
	       			}	       		
		   		this.rect.Offset(0, 10);
		   		Region region_down = new Region(this.full_rect);
		   		region_down.Exclude(this.rect);
		   		this.Region = region_down;
	  			region_down.Dispose();
	  			
		   	    	break;
		   	case Keys.Up:
	       			if (this.toggle == 0)
	       			{
	       				break;
	       			}	       		

		   		this.rect.Offset(0, -10);
		   		Region region_up = new Region(this.full_rect);
		   		region_up.Exclude(this.rect);
		   		this.Region = region_up;
				region_up.Dispose();
		   	    	break;
		   	case Keys.Right:
	       			if (this.toggle == 0)
	       			{
	       				break;
	       			}	       		
		   		this.rect.Offset(10, 0);
		   		Region region_right = new Region(this.full_rect);
		   		region_right.Exclude(this.rect);
		   		this.Region = region_right;
		   		region_right.Dispose();
		   		break;
		   	case Keys.Left:
	       			if (this.toggle == 0)
	       			{
	       				break;
	       			}	       		
		   		this.rect.Offset(-10, 0);
		   		Region region_left = new Region(this.full_rect);
		   		region_left.Exclude(this.rect);
		   		this.Region = region_left;
		   		region_left.Dispose();
		   		break;
		   	case Keys.Q:
		   		this.Close();
                		Application.Exit();
                		break;
                	case Keys.T:
                		if (this.toggle == 1)
                		{
					Region region_toggle = new Region(this.full_rect);
					this.Region = region_toggle;
					region_toggle.Dispose();
					this.toggle = 0;
				} else {
					int xpos = this.rect.X;
					int ypos = this.rect.Y;
					Region region_toggle = new Region(this.full_rect);
					// this.rect.Location = new Point(xpos, ypos);
					region_toggle.Exclude(this.rect);
					this.Region = region_toggle;
					region_toggle.Dispose();
					this.toggle = 1;
				}
				break;
            
		}	
		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void InvertRegion()
	{
		// Graphics g = this.CreateGraphics();
		// bool result = InvertRect( g.GetHdc(), ref this.rect);
		ControlPaint.FillReversibleRectangle(this.rect, Color.Black);
	  	ControlPaint.DrawReversibleFrame(this.rect, Color.Black, FrameStyle.Dashed);
	}

	/*
	 * Unregister HotKey when form closes
	 *
	 */
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 0);       // Unregister hotkey with id 0 before closing the form. You might want to call this more than once with different id values if you are planning to register more than one hotkey.
        }

	/*
	 * This function is run whenever a message is sent to a window
	 *
	 */
        protected override void WndProc(ref Message m)
        {
	    	base.WndProc(ref m);
	    	
            	if (m.Msg == 0x0312)
            	{
                	/*
                 	* Note that the three lines below are not needed if you only want to register one hotkey.
                 	* The below lines are useful in case you want to register multiple keys, which you can use a switch with the id as argument, or if you want to know which key/modifier was pressed for some particular reason
                 	*
                 	*/

                	Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
                	KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
                	int id = m.WParam.ToInt32();                                        // The id of the hotkey that was pressed.

			/*
			 * Below sleep is important for the hotkeys to function
			 *
			 */
			SetFocus(this.Handle);
			SetForegroundWindow(this.Handle);
		}
        }
	
        #region Windows Form Designer generated code
	
	private void InitializeComponent()
	{

            	// 
            	// Form1
            	// 
            	
            	this.AutoSize = false;
            	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            	this.ClientSize = new System.Drawing.Size(screen_width, screen_height);
            	this.Name = "Form1";
            	this.Text = "Form1";
            	this.ResumeLayout(false);
           	this.PerformLayout();
           	this.full_rect = new Rectangle(Point.Empty, this.Size);
           	this.rect = new Rectangle(Point.Empty, new Size(100, 100));
		Region region = new Region(this.full_rect);
				
		region.Exclude(this.rect);
		
		this.Region = region;
	}	
	
	#endregion

	[STAThread]
	static void Main()
	{
        	Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new Form1());
	}
    }
}
