using System;

namespace Checklist
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lHome = new System.Windows.Forms.Label();
            this.home = new System.Windows.Forms.PictureBox();
            this.whiteHeart = new System.Windows.Forms.PictureBox();
            this.user = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btnLogout = new System.Windows.Forms.PictureBox();
            this.refresh = new System.Windows.Forms.PictureBox();
            this.btnFavoris = new System.Windows.Forms.PictureBox();
            this.btnSettings = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelHome = new System.Windows.Forms.Label();
            this.proAlpha = new System.Windows.Forms.PictureBox();
            this.favoris = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.PictureBox();
            this.panelPoints = new System.Windows.Forms.Panel();
            this.validate = new System.Windows.Forms.PictureBox();
            this.shortDesc = new System.Windows.Forms.RichTextBox();
            this.titreChecklist = new System.Windows.Forms.TextBox();
            this.infoChecklist = new System.Windows.Forms.PictureBox();
            this.btnExport = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.PictureBox();
            this.btnEdit = new System.Windows.Forms.PictureBox();
            this.btnAddPoint = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelChecklists = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.home)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.whiteHeart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFavoris)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.proAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.favoris)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.validate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoChecklist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.labelHome);
            this.splitContainer1.Panel2.Controls.Add(this.proAlpha);
            this.splitContainer1.Panel2.Controls.Add(this.favoris);
            this.splitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer1.Panel2.Controls.Add(this.panelPoints);
            this.splitContainer1.Panel2.Controls.Add(this.validate);
            this.splitContainer1.Panel2.Controls.Add(this.shortDesc);
            this.splitContainer1.Panel2.Controls.Add(this.titreChecklist);
            this.splitContainer1.Panel2.Controls.Add(this.infoChecklist);
            this.splitContainer1.Panel2.Controls.Add(this.btnExport);
            this.splitContainer1.Panel2.Controls.Add(this.btnDelete);
            this.splitContainer1.Panel2.Controls.Add(this.btnEdit);
            this.splitContainer1.Panel2.Controls.Add(this.btnAddPoint);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.panelChecklists);
            this.splitContainer1.Panel2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.splitContainer1.Panel2.Click += new System.EventHandler(this.splitContainer1_Panel2_Click);
            this.splitContainer1.Panel2.Enter += new System.EventHandler(this.splitContainer1_Panel2_Enter);
            this.splitContainer1.Size = new System.Drawing.Size(3216, 1760);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.SplitterWidth = 11;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(63)))), ((int)(((byte)(84)))));
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(63)))), ((int)(((byte)(84)))));
            this.splitContainer2.Panel1.Controls.Add(this.lHome);
            this.splitContainer2.Panel1.Controls.Add(this.home);
            this.splitContainer2.Panel1.Controls.Add(this.whiteHeart);
            this.splitContainer2.Panel1.Controls.Add(this.user);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.treeView1);
            this.splitContainer2.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer2_Panel1_Paint);
            this.splitContainer2.Panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.splitContainer2_Panel1_MouseClick);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(45)))), ((int)(((byte)(68)))));
            this.splitContainer2.Panel2.Controls.Add(this.btnLogout);
            this.splitContainer2.Panel2.Controls.Add(this.refresh);
            this.splitContainer2.Panel2.Controls.Add(this.btnFavoris);
            this.splitContainer2.Panel2.Controls.Add(this.btnSettings);
            this.splitContainer2.Panel2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitContainer2.Size = new System.Drawing.Size(300, 1760);
            this.splitContainer2.SplitterDistance = 1709;
            this.splitContainer2.SplitterWidth = 10;
            this.splitContainer2.TabIndex = 2;
            // 
            // lHome
            // 
            this.lHome.AutoSize = true;
            this.lHome.Font = new System.Drawing.Font("Calibri", 14F);
            this.lHome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.lHome.Location = new System.Drawing.Point(149, 291);
            this.lHome.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lHome.Name = "lHome";
            this.lHome.Size = new System.Drawing.Size(140, 58);
            this.lHome.TabIndex = 45;
            this.lHome.Text = "Home";
            this.lHome.Click += new System.EventHandler(this.home_Click);
            this.lHome.MouseEnter += new System.EventHandler(this.lHome_MouseEnter);
            this.lHome.MouseLeave += new System.EventHandler(this.lHome_MouseLeave);
            // 
            // home
            // 
            this.home.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.home.Image = global::Checklist.Properties.Resources.home1;
            this.home.Location = new System.Drawing.Point(43, 274);
            this.home.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.home.Name = "home";
            this.home.Size = new System.Drawing.Size(88, 72);
            this.home.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.home.TabIndex = 44;
            this.home.TabStop = false;
            this.home.Click += new System.EventHandler(this.home_Click);
            this.home.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.home.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // whiteHeart
            // 
            this.whiteHeart.Image = global::Checklist.Properties.Resources.whiteHeart;
            this.whiteHeart.Location = new System.Drawing.Point(32, 36);
            this.whiteHeart.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.whiteHeart.Name = "whiteHeart";
            this.whiteHeart.Size = new System.Drawing.Size(115, 88);
            this.whiteHeart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.whiteHeart.TabIndex = 43;
            this.whiteHeart.TabStop = false;
            this.whiteHeart.Visible = false;
            // 
            // user
            // 
            this.user.AutoSize = true;
            this.user.Font = new System.Drawing.Font("Calibri", 14F);
            this.user.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.user.Location = new System.Drawing.Point(301, 136);
            this.user.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.user.Name = "user";
            this.user.Size = new System.Drawing.Size(112, 58);
            this.user.TabIndex = 1;
            this.user.Text = "User";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 14F);
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label3.Location = new System.Drawing.Point(301, 69);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(257, 58);
            this.label3.TabIndex = 0;
            this.label3.Text = "Wilkommen";
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(63)))), ((int)(((byte)(84)))));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeView1.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.treeView1.ForeColor = System.Drawing.Color.White;
            this.treeView1.ItemHeight = 30;
            this.treeView1.LineColor = System.Drawing.Color.White;
            this.treeView1.Location = new System.Drawing.Point(43, 405);
            this.treeView1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowLines = false;
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.ShowRootLines = false;
            this.treeView1.Size = new System.Drawing.Size(249, 1186);
            this.treeView1.TabIndex = 41;
            this.treeView1.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_BeforeLabelEdit);
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeCollapse);
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.treeView1.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView1_DrawNode);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(81)))), ((int)(((byte)(112)))));
            this.btnLogout.Location = new System.Drawing.Point(584, 2);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(91, 86);
            this.btnLogout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnLogout.TabIndex = 3;
            this.btnLogout.TabStop = false;
            this.btnLogout.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.btnLogout.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // refresh
            // 
            this.refresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(81)))), ((int)(((byte)(112)))));
            this.refresh.Location = new System.Drawing.Point(408, 7);
            this.refresh.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(93, 76);
            this.refresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.refresh.TabIndex = 2;
            this.refresh.TabStop = false;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            this.refresh.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.refresh.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // btnFavoris
            // 
            this.btnFavoris.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(81)))), ((int)(((byte)(112)))));
            this.btnFavoris.Location = new System.Drawing.Point(256, 12);
            this.btnFavoris.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnFavoris.Name = "btnFavoris";
            this.btnFavoris.Size = new System.Drawing.Size(83, 72);
            this.btnFavoris.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnFavoris.TabIndex = 1;
            this.btnFavoris.TabStop = false;
            this.btnFavoris.Click += new System.EventHandler(this.btnFavoris_Click);
            this.btnFavoris.MouseEnter += new System.EventHandler(this.btnFavoris_MouseEnter);
            this.btnFavoris.MouseLeave += new System.EventHandler(this.btnFavoris_MouseLeave);
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(81)))), ((int)(((byte)(112)))));
            this.btnSettings.Location = new System.Drawing.Point(101, 10);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(88, 74);
            this.btnSettings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnSettings.TabIndex = 0;
            this.btnSettings.TabStop = false;
            this.btnSettings.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.btnSettings.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(272, 148);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 49);
            this.label2.TabIndex = 1;
            this.label2.Text = "User";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label1.Location = new System.Drawing.Point(272, 88);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 49);
            this.label1.TabIndex = 0;
            this.label1.Text = "Wilkommen,";
            // 
            // labelHome
            // 
            this.labelHome.AutoSize = true;
            this.labelHome.Font = new System.Drawing.Font("Calibri", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.labelHome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(45)))), ((int)(((byte)(68)))));
            this.labelHome.Location = new System.Drawing.Point(1008, 83);
            this.labelHome.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelHome.Name = "labelHome";
            this.labelHome.Size = new System.Drawing.Size(240, 97);
            this.labelHome.TabIndex = 46;
            this.labelHome.Text = "Home";
            // 
            // proAlpha
            // 
            this.proAlpha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.proAlpha.Image = global::Checklist.Properties.Resources.proalpha_logo_RGB_03__1_;
            this.proAlpha.Location = new System.Drawing.Point(2468, 1593);
            this.proAlpha.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.proAlpha.Name = "proAlpha";
            this.proAlpha.Size = new System.Drawing.Size(371, 138);
            this.proAlpha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.proAlpha.TabIndex = 46;
            this.proAlpha.TabStop = false;
            // 
            // favoris
            // 
            this.favoris.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.favoris.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(45)))), ((int)(((byte)(68)))));
            this.favoris.Location = new System.Drawing.Point(2479, 24);
            this.favoris.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.favoris.Name = "favoris";
            this.favoris.Size = new System.Drawing.Size(93, 79);
            this.favoris.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.favoris.TabIndex = 45;
            this.favoris.TabStop = false;
            this.favoris.Click += new System.EventHandler(this.favoris_Click);
            this.favoris.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.favoris.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(56)))), ((int)(((byte)(90)))));
            this.btnCancel.Location = new System.Drawing.Point(2596, 24);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 76);
            this.btnCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnCancel.TabIndex = 42;
            this.btnCancel.TabStop = false;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.btnCancel.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // panelPoints
            // 
            this.panelPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPoints.AutoScroll = true;
            this.panelPoints.Location = new System.Drawing.Point(133, 444);
            this.panelPoints.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.panelPoints.Name = "panelPoints";
            this.panelPoints.Size = new System.Drawing.Size(2705, 990);
            this.panelPoints.TabIndex = 41;
            this.panelPoints.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panelPoints_Scroll);
            this.panelPoints.SizeChanged += new System.EventHandler(this.panelPoints_SizeChanged);
            this.panelPoints.Enter += new System.EventHandler(this.panelPoints_Enter);
            this.panelPoints.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panelPoints_MouseWheel);
            this.panelPoints.Resize += new System.EventHandler(this.panelPoints_Resize);
            // 
            // validate
            // 
            this.validate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.validate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(229)))), ((int)(((byte)(0)))));
            this.validate.Location = new System.Drawing.Point(1367, 1548);
            this.validate.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.validate.Name = "validate";
            this.validate.Size = new System.Drawing.Size(112, 100);
            this.validate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.validate.TabIndex = 40;
            this.validate.TabStop = false;
            this.validate.Visible = false;
            this.validate.Click += new System.EventHandler(this.validate_Click);
            this.validate.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.validate.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // shortDesc
            // 
            this.shortDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.shortDesc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.shortDesc.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.shortDesc.Location = new System.Drawing.Point(173, 310);
            this.shortDesc.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.shortDesc.Name = "shortDesc";
            this.shortDesc.ReadOnly = true;
            this.shortDesc.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.shortDesc.Size = new System.Drawing.Size(2633, 119);
            this.shortDesc.TabIndex = 39;
            this.shortDesc.Text = "Short description";
            this.shortDesc.Enter += new System.EventHandler(this.shortDesc_Enter);
            this.shortDesc.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TBox_KeyUp);
            this.shortDesc.Leave += new System.EventHandler(this.shortDesc_Leave);
            this.shortDesc.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TBox_PreviewKeyDown);
            // 
            // titreChecklist
            // 
            this.titreChecklist.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.titreChecklist.Cursor = System.Windows.Forms.Cursors.Default;
            this.titreChecklist.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.titreChecklist.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(63)))), ((int)(((byte)(84)))));
            this.titreChecklist.Location = new System.Drawing.Point(173, 196);
            this.titreChecklist.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.titreChecklist.Name = "titreChecklist";
            this.titreChecklist.ReadOnly = true;
            this.titreChecklist.Size = new System.Drawing.Size(779, 80);
            this.titreChecklist.TabIndex = 37;
            this.titreChecklist.Text = "Checklist 1";
            this.titreChecklist.TextChanged += new System.EventHandler(this.titreChecklist_TextChanged);
            this.titreChecklist.Enter += new System.EventHandler(this.TBox_Enter);
            this.titreChecklist.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TBox_KeyUp);
            this.titreChecklist.Leave += new System.EventHandler(this.TBox_Leave);
            this.titreChecklist.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TBox_PreviewKeyDown);
            // 
            // infoChecklist
            // 
            this.infoChecklist.Image = global::Checklist.Properties.Resources.info;
            this.infoChecklist.Location = new System.Drawing.Point(1000, 188);
            this.infoChecklist.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.infoChecklist.Name = "infoChecklist";
            this.infoChecklist.Size = new System.Drawing.Size(53, 48);
            this.infoChecklist.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.infoChecklist.TabIndex = 23;
            this.infoChecklist.TabStop = false;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(45)))), ((int)(((byte)(68)))));
            this.btnExport.Location = new System.Drawing.Point(2796, 24);
            this.btnExport.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 79);
            this.btnExport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnExport.TabIndex = 5;
            this.btnExport.TabStop = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            this.btnExport.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.btnExport.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(45)))), ((int)(((byte)(68)))));
            this.btnDelete.Location = new System.Drawing.Point(2700, 24);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 79);
            this.btnDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnDelete.TabIndex = 4;
            this.btnDelete.TabStop = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.btnDelete.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.btnDelete.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(45)))), ((int)(((byte)(68)))));
            this.btnEdit.Location = new System.Drawing.Point(2593, 24);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(88, 76);
            this.btnEdit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnEdit.TabIndex = 2;
            this.btnEdit.TabStop = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            this.btnEdit.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.btnEdit.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // btnAddPoint
            // 
            this.btnAddPoint.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(45)))), ((int)(((byte)(68)))));
            this.btnAddPoint.Location = new System.Drawing.Point(1367, 1548);
            this.btnAddPoint.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnAddPoint.Name = "btnAddPoint";
            this.btnAddPoint.Size = new System.Drawing.Size(112, 100);
            this.btnAddPoint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnAddPoint.TabIndex = 1;
            this.btnAddPoint.TabStop = false;
            this.btnAddPoint.Click += new System.EventHandler(this.btnAddPoint_Click);
            this.btnAddPoint.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.btnAddPoint.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Enabled = false;
            this.pictureBox1.Image = global::Checklist.Properties.Resources.Chat_32px;
            this.pictureBox1.Location = new System.Drawing.Point(56, 200);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(93, 69);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panelChecklists
            // 
            this.panelChecklists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelChecklists.AutoScroll = true;
            this.panelChecklists.BackColor = System.Drawing.Color.Transparent;
            this.panelChecklists.Location = new System.Drawing.Point(133, 188);
            this.panelChecklists.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.panelChecklists.Name = "panelChecklists";
            this.panelChecklists.Size = new System.Drawing.Size(2705, 1245);
            this.panelChecklists.TabIndex = 42;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextMenuStrip1_ItemClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(3216, 1760);
            this.Controls.Add(this.splitContainer1);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "Form1";
            this.Text = "Checklist";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.home)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.whiteHeart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFavoris)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.proAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.favoris)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.validate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoChecklist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox btnAddPoint;
        private System.Windows.Forms.PictureBox btnEdit;
        private System.Windows.Forms.PictureBox btnDelete;
        private System.Windows.Forms.PictureBox btnExport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label user;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox infoChecklist;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox btnSettings;
        private System.Windows.Forms.PictureBox btnFavoris;
        private System.Windows.Forms.PictureBox refresh;
        private System.Windows.Forms.PictureBox btnLogout;
        private System.Windows.Forms.TextBox titreChecklist;
        private System.Windows.Forms.RichTextBox shortDesc;
        private System.Windows.Forms.PictureBox validate;
        /* private System.Windows.Forms.Panel pKunde;
         private System.Windows.Forms.Panel pTechnik;
         private System.Windows.Forms.Panel pBerater;
         private System.Windows.Forms.Panel pEntw;
         private System.Windows.Forms.Label sort;*/
        private System.Windows.Forms.Panel panelPoints;
        private System.Windows.Forms.PictureBox btnCancel;
        private System.Windows.Forms.Panel panelChecklists;
        private System.Windows.Forms.PictureBox whiteHeart;
        private System.Windows.Forms.PictureBox favoris;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.PictureBox home;
        private System.Windows.Forms.Label lHome;
        private System.Windows.Forms.PictureBox proAlpha;
        private System.Windows.Forms.Label labelHome;
    }
}

