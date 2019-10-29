using Checklist.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;

namespace Checklist
{
    public partial class Form1 : Form
    {
        ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));

        // Représente les instances actuelles
        private ChecklistRepo check;
        private PointChecklist punkt;

        private DataSet typesUser = new DataSet();
        private List<int> pointsSupprimes = new List<int>();

        // Première fois sans scrollbar dans le panel point
        private bool firstTime = false;

        private bool newPoint = false;

        private bool cancelLabelEdit = false;

        // peut-être faire une classe pour ça
        private TreeNode nodeTopic = new TreeNode();
        private TreeNode nodeCategory = new TreeNode();
        private TreeNode nodeChecklist = new TreeNode();

        private bool firstEdition = true;

        private string categorieAModifier = string.Empty;
        private bool first = true;
        private string topicAModifier = string.Empty;

        private bool creationChecklist;

        private string btnSettingsClick;

        private Image tagImage = Resources.icone1;
        private Image tagImage2 = Resources.icone2;
        private Bitmap tagBitmap;
        private Bitmap tagBitmap2;

        private Image tagImageTopic = Resources.circle21;
        private Bitmap tagBitmapTopic;

        private Image checkMark = Resources.checkmark;
        private Bitmap bitmapCheckMark;

        private DataSet dataSetTreeView = new DataSet();

        private DataSet dsResearch = new DataSet();

        public MySqlConnection mysql;

        private ListView listFavorites = new ListView();
        private Label labelFavorites = new Label();

        private ListView states = new ListView();

        // private bool ignorerHot = false;

        private int IDLigneDessinee = 0;
        // private bool rotationFinie = false;
        // private int indexNoeud = 0;

        public Form1()
        {
            InitializeComponent();
            // toolTip1.SetToolTip(infoChecklist,"Created by ... \n Last updated since ...");

            //string sqlConnection = "server = 127.0.0.1; user id = root; port = 3306; database = db;Password=Root123!";


            try
            {
                string sqlConnection = ConfigurationManager.ConnectionStrings["MaConnexion"].ConnectionString;
                mysql = new MySqlConnection(sqlConnection);
                mysql.Open();
            }
            catch (Exception)
            {
                 MessageBox.Show("Erreur: Impossible de se connecter à la base de données, Veuillez contrôler la configuration");
                 exitAppli();
                 
            }
            
            

            Request.connection(mysql);

            this.KeyPreview = true;

            tagBitmap = new Bitmap(tagImage);
            tagBitmap = Graphic.ResizeImage(tagBitmap, 12, 12);
            tagBitmap2 = new Bitmap(tagImage2);
            tagBitmap2 = Graphic.ResizeImage(tagBitmap2, 12, 12);

            tagBitmapTopic = new Bitmap(tagImageTopic);
            tagBitmapTopic = Graphic.ResizeImage(tagBitmapTopic, 9, 9);

            bitmapCheckMark = new Bitmap(checkMark);
            bitmapCheckMark = Graphic.ResizeImage(bitmapCheckMark, 11, 10);

            btnSettings.Enabled = false;
            // btnPrint.Enabled    = false;
            btnLogout.Enabled = false;
        }

        public void exitAppli()
        {
            Close();
        }

        public void initListViewState()
        {
            states.Columns.Add("Type");
            states.Columns.Add("ID");
            states.Columns.Add("States");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            titreChecklist.BackColor = Color.White;
            titreChecklist.ForeColor = Constantes.blue;
            shortDesc.BackColor = Color.White;
            shortDesc.ForeColor = Constantes.deepBlue;

            treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            Tree.InitialiseTreeView(treeView1, dataSetTreeView, tagBitmap);
            Filters.InitialiseUserType(typesUser);
            InitialiseHomepage();
            listFavorites = creationFavoritesList();

            listFavorites.Visible = false;
            labelFavorites.Visible = false;
            whiteHeart.Visible = false;

            PictureBox pbImage = AjoutElement.addPBImage(splitContainer2.Panel1);
            pbImage.Paint += PbImage_Paint;
            // pbImage.Image = Resources.photo;

            treeView1.PreviewKeyDown += TreeView1_PreviewKeyDown;

            /*
            try
            {
                DirectoryEntry entry = new DirectoryEntry("LPAD://pa","User","Password");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            */
            initListViewState();
        }

        /* Initialise the homepage when we start the application
         * This homepage displays the 5 last checklists
         */
        private void InitialiseHomepage()
        {
            InitialiseButtonsHomepage();
            proAlpha.Visible = true;
            labelHome.Visible = true;
            if (check != null)
            {
                check = null;
            }
            MySqlDataAdapter ad = new MySqlDataAdapter();
            DataSet dsInit = new DataSet();
            Request.useSelectRequest(ad, dsInit, SQL.selectLastChecklists());

            Label labelChecklists = AjoutElement.addLabelTitrePageAccueil();
            panelChecklists.Controls.Add(labelChecklists);

            panelChecklists.Location = new Point(50, 100);

            Label labelChecklist = AjoutElement.addLabelPageAccueil("CHECKLIST", "labelChecklist");
            Label labelDescription = AjoutElement.addLabelPageAccueil("BESCHREIBUNG", "labelDescription");

            Panel panelCheck = AjoutElement.grayPanel(Constantes.checklist, panelChecklists, labelChecklist);
            Panel panelDescription = AjoutElement.grayPanel(Constantes.description, panelChecklists, labelDescription);
            Panel panelBleu = AjoutElement.bluePanel(panelCheck.Left, panelDescription.Right, panelCheck.Bottom, panelChecklists);

            panelDescription.Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            try
            { 
                foreach (DataRow item in dsInit.Tables[0].Rows)
                 {
                addPanelChecklist(ad, dsInit, item);
                 }
            }
            catch(Exception)
            {
                Console.WriteLine("erreur de connexion");
            }

        }

        public void addPanelChecklist(MySqlDataAdapter ad, DataSet dsInit, DataRow item)
        {
            int locChecklistY = 0;
            if (Tools.firstChecklist(panelChecklists))
            {
                locChecklistY = Constantes.locationYPremiereChecklist;
            }
            else
            {
                locChecklistY = Tools.locationYNewPoint(this.panelChecklists, Constantes.panelChecklist);
            }
            removeInfosChecklist();
            Panel panelChecklist = AjoutElement.InitPanelChecklist(locChecklistY, Int32.Parse(item.ItemArray[0].ToString()));
            displayPanelChecklist(dsInit, ad, panelChecklist, item);
            panelChecklists.Controls.Add(panelChecklist);
        }

        /*
         * Initialise the homepage buttons, c'est-à-dire que les boutons qui font des actions sur une checklist sont désactivés
         * car on a aucune checklist sélectionnée
         */
        private void InitialiseButtonsHomepage()
        {
            panelPoints.Visible = false;
            panelChecklists.Visible = true;
            btnEdit.Visible = false;
            btnDelete.Visible = false;
            btnAddPoint.Visible = false;
            favoris.Visible = false;
            Favorites.favoriBleu(favoris);
            // btnPrint.Visible = false;
            btnExport.Visible = false;
        }

        /*
         * N'affiche pas les informations propres à une checklist car aucune checklist n'est affichée à l'écran
         */
        private void removeInfosChecklist()
        {
            titreChecklist.Visible = false;
            shortDesc.Visible = false;
            infoChecklist.Visible = false;
            pictureBox1.Visible = false;
        }

        /*
         * Pour la page d'accueil, quand on affiche les 5 dernières checklists, affiche le panel contenant une checklist
         * ainsi que toutes ses infos
         */
        private void displayPanelChecklist(DataSet data, MySqlDataAdapter ad, Panel panelChecklist, DataRow checklist)
        {
            Tools.resetDataTable(data, Constantes.topic);
            Request.useSelectRequestTable(ad, data.Tables[Constantes.topic], SQL.selectTopicByIDTopic("Titre", checklist.ItemArray[5].ToString()));

            Tools.resetDataTable(data, Constantes.categorie);
            Request.useSelectRequestTable(ad, data.Tables[Constantes.categorie], SQL.selectTitleCategoryByTopic(checklist.ItemArray[5].ToString()));

            Label titleCheck = AjoutElement.addLabelTitre(checklist.ItemArray[1].ToString(), Constantes.labelTitreChecklist, 10, 0, 208, panelChecklist.Height, Constantes.blue);

            panelChecklist.Click += PanelChecklist_Click;
            titleCheck.Click += Checklist_Click;

            Label descriptionChecklist = AjoutElement.addLabelDescription(checklist.ItemArray[2].ToString(),
                Constantes.labelDescriptionChecklist, new Point(230, 0), new Size(524, panelChecklist.Height), Constantes.blue);

            panelChecklist.Controls.Add(descriptionChecklist);
            // PictureBox info = AjoutElement.addInfoChecklist();
            // infosChecklist(checklist.ItemArray[0].ToString(),checklist,info,toolTipChecklists);

            Label categorie = AjoutElement.addLabelCategorie(data.Tables[Constantes.categorie].Rows[0].ItemArray[0].ToString());
            Label separator = AjoutElement.addLabelSeparator(categorie.Right + 4);
            Label topic = AjoutElement.addLabelTopic(data.Tables[Constantes.topic].Rows[0].ItemArray[0].ToString(), separator.Right + 4);

            topic.Click += Checklist_Click;
            categorie.Click += Checklist_Click;
            descriptionChecklist.Click += Checklist_Click;
            separator.Click += Checklist_Click;

            AjoutElement.addControlsPanelChecklist(panelChecklist, titleCheck, descriptionChecklist/*,info*/, topic, categorie, separator);

            titleCheck.MouseEnter += LabelChecklist_MouseEnter;
            titleCheck.MouseLeave += LabelChecklist_MouseLeave;
            topic.MouseEnter += LabelChecklist_MouseEnter;
            topic.MouseLeave += LabelChecklist_MouseLeave;
            categorie.MouseEnter += LabelChecklist_MouseEnter;
            categorie.MouseLeave += LabelChecklist_MouseLeave;
            descriptionChecklist.MouseEnter += LabelChecklist_MouseEnter;
            descriptionChecklist.MouseLeave += LabelChecklist_MouseLeave;

            categorie.BringToFront();
            topic.BringToFront();
            separator.BringToFront();
            titleCheck.SendToBack();
        }

        /*
         * Permet d'afficher une checklist à l'écran avec toutes ses infos, ses points...
         */
        private void displayChecklist(MySqlDataAdapter adapt, DataSet data)
        {
            proAlpha.Visible = false;
            labelHome.Visible = false;
            if (titreChecklist.ReadOnly == false)
            {
                titreChecklist.ReadOnly = true;
            }
            if (shortDesc.ReadOnly == false)
            {
                shortDesc.ReadOnly = true;
            }
            Graphic.textBoxChecklistWritingColor(titreChecklist, shortDesc);

            Display.infosChecklist(data.Tables[0].Rows[0][0].ToString(), data.Tables[Constantes.checklist].Rows[0], infoChecklist, toolTip1, titreChecklist, shortDesc,
                mysql, favoris, btnEdit, btnCancel, btnDelete, btnExport, btnAddPoint);

            Tools.resetDataTable(data, Constantes.points);
            infoChecklist.Location = new Point(titreChecklist.Location.X + Tools.changeLabelSize(titreChecklist) + 8, infoChecklist.Location.Y);
            Request.useSelectRequestTable(adapt, data.Tables[Constantes.points], SQL.selectPointsByIDChecklist("*", data.Tables[0].Rows[0][0].ToString()));
            check = new ChecklistRepo(data.Tables[Constantes.checklist].Rows[0]);

            Filters.restoreFilters(typesUser);
            panelPointsVisible();
            btnAddPoint.Visible = true;
            updatePoints(adapt, data);

            Tools.resetDataTable(data, Constantes.favoris);
            Favorites.presenceChecklistInFavorites(adapt, data.Tables[Constantes.favoris], check.IDChecklist.ToString(), favoris);
        }

        public void checklistClick(Panel panelChecklist)
        {
            MySqlDataAdapter adapt = new MySqlDataAdapter();
            DataSet data = new DataSet();
            data.Tables.Add(Constantes.checklist);

            string titleCategorie = string.Empty;
            string titleTopic = string.Empty;
            string titleChecklist = string.Empty;

            foreach (Control control in panelChecklist.Controls)
            {
                if (control.GetType() == typeof(Label) && control.Name == Constantes.labelTitreChecklist)
                {
                    titleChecklist = control.Text;
                }
                else if (control.GetType() == typeof(Label) && control.Name == "Topic")
                {
                    titleTopic = control.Text;
                }
                else if (control.GetType() == typeof(Label) && control.Name == "Categorie")
                {
                    titleCategorie = control.Text;
                }
            }
            Request.useSelectRequestTable(adapt, data.Tables[Constantes.checklist], SQL.selectChecklist(titleCategorie, titleTopic, titleChecklist));
            displayChecklist(adapt, data);
            Tree.expandNodes(titleCategorie, titleTopic, titleChecklist, treeView1, nodeCategory, nodeTopic, nodeChecklist);
        }

        /*
         * Vérifie si le panel contenant tous les points est visible, et si non, le rendre visible
         */
        private void panelPointsVisible()
        {
            if (panelPoints.Visible == false)
            {
                panelPoints.Visible = true;
                panelChecklists.Visible = false;
                panelChecklists.Controls.Clear();
                titreChecklist.Visible = true;
                shortDesc.Visible = true;
                infoChecklist.Visible = true;
                pictureBox1.Visible = true;
            }
            if (btnCancel.Visible == false && validate.Visible == false)
            {
                btnEdit.Visible = true;
            }
            btnDelete.Visible = true;
            favoris.Visible = true;
            // btnPrint.Visible = true;
            btnExport.Visible = true;

            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            favoris.Enabled = true;
            // btnPrint.Enabled = true;
            btnExport.Enabled = true;
        }

        /*
         * Modifie certains paramètres pour permettre l'édition de la checklist
         */
        private void editChecklist()
        {
            titreChecklist.ReadOnly = false;
            shortDesc.ReadOnly = false;
            btnAddPoint.Visible = false;
            btnEdit.Visible = false;
            infoChecklist.Visible = false;
            validate.Visible = true;
            btnCancel.Visible = true;
            creationChecklist = false;

            if (string.IsNullOrWhiteSpace(shortDesc.Text))
            {
                Display.displayPlaceholder(shortDesc);
            }
            Edition.updateEditionCheck(titreChecklist, shortDesc, splitContainer1.Panel2);
        }

        /*
         * Permet la création d'une checklist
         */
        private void createChecklist()
        {
            editChecklist();
            //panelPoints.Visible = false;
            titreChecklist.Text = string.Empty;
            shortDesc.Text = string.Empty;
            creationChecklist = true;
            titreChecklist.Focus();

            if (string.IsNullOrWhiteSpace(titreChecklist.Text))
            {
                Display.displayPlaceholder(titreChecklist);
            }
            if (string.IsNullOrWhiteSpace(shortDesc.Text))
            {
                Display.displayPlaceholder(shortDesc);
            }
        }

        /*
         * Désactive le clic sur les filtres du type d'utilisateur
         * Ces filtres sont désactivés lorsque l'on est en mode édition ou que l'on ajoute un nouveau point
         */
        private void disableFiltersClick()
        {
            foreach (Control control in panelPoints.Controls)
            {
                if (control.Name == Constantes.typeEntwickler)
                {
                    control.Click -= new EventHandler(this.pEntw_Click);
                }
                else if (control.Name == Constantes.typeBerater)
                {
                    control.Click -= new EventHandler(this.pBerater_Click);
                }
                else if (control.Name == Constantes.typeTechnik)
                {
                    control.Click -= new EventHandler(this.pTechnik_Click);
                }
                else if (control.Name == Constantes.typeKunde)
                {
                    control.Click -= new EventHandler(this.pKunde_Click);
                }
            }
        }

        /*
         * Active le clic sur les filtres du type d'utilisateur
         * Il faut les activer à la suite d'une édition de la checklist ou de l'ajout d'un nouveau point
         */
        private void activateFiltersClick()
        {
            foreach (Control control in panelPoints.Controls)
            {
                if (control.Name == Constantes.typeEntwickler)
                {
                    control.Click += new EventHandler(this.pEntw_Click);
                }
                else if (control.Name == Constantes.typeBerater)
                {
                    control.Click += new EventHandler(this.pBerater_Click);
                }
                else if (control.Name == Constantes.typeTechnik)
                {
                    control.Click += new EventHandler(this.pTechnik_Click);
                }
                else if (control.Name == Constantes.typeKunde)
                {
                    control.Click += new EventHandler(this.pKunde_Click);
                }
            }
        }

        /*
         * Repasse la checklist dans un état non éditable, en sortant du mode édition
         */
        private void nonEditableChecklist()
        {
            // voir pour annuler le titre modifié si c'est une modif
            // ou aller sur une autre page si c'est une création
            if (check != null)
            {
                if (titreChecklist.Text != check.titre)
                {
                    titreChecklist.Text = check.titre;
                }
                if (shortDesc.Text != check.description)
                {
                    shortDesc.Text = check.description;
                }
            }
            titreChecklist.ReadOnly = true;
            titreChecklist.BorderStyle = BorderStyle.None;
            shortDesc.ReadOnly = true;
            shortDesc.BorderStyle = BorderStyle.None;
            btnAddPoint.Visible = true;
            validate.Visible = false;
            ActiveControl = null;
            btnCancel.Visible = false;
            btnEdit.Visible = true;
            infoChecklist.Visible = true;
            splitContainer1.Panel2.Invalidate();
        }

        /*
         * Repasse un point dans un mode non éditable, en sortant du mode édition
         */
        private void nonEditablePoint(Panel panel)
        {
            string IDPoint = Tools.returnID(panel.Name);
            if (IDPoint != Constantes.temp)
            {
                MySqlDataAdapter adapt = new MySqlDataAdapter();
                DataSet data = new DataSet();
                DataTable table = Request.useRequestSelectPointByIDPoint(adapt, IDPoint); // problème si on annule une création de point
                data.Tables.Add(table);

                panel.Controls.Clear();
                refreshOnePoint(data, adapt, panel, IDPoint.ToString(), table.Rows[0].ItemArray[1].ToString(), table.Rows[0].ItemArray[2].ToString());
            }
        }

        /*
         * Permet de repasser tous les points dans un mode non éditable
         */
        private void nonEditablePoints()
        {
            typesUser.Tables[Constantes.typesPoints].Rows.Clear();
            int nbPointsNonSupprimes = 0;
            foreach (Control control in panelPoints.Controls)
            {
                if (control.GetType() == typeof(Panel) && control.Name.StartsWith(Constantes.panelPoint))
                {
                    Edition.cancelDeletedPoints(nbPointsNonSupprimes, control);
                    if (control.Visible == false)
                    {
                        nbPointsNonSupprimes++;
                        control.Visible = true;
                    }
                    nonEditablePoint((Panel)control);
                    control.Invalidate();
                }
            }
        }

        /*
         * Annule la modification d'une checklist
         */
        private void cancel()
        {
            // btnPrint.Enabled = true;
            btnExport.Enabled = true;
            panelPointsVisible();
            nonEditableChecklist();
            Graphic.textBoxChecklistWritingColor(titreChecklist, shortDesc);
            nonEditablePoints();
            if (check != null)
            {
                if (panelPoints.Controls.Count > 0)
                {
                    Edition.cancelCreatedPoints(panelPoints);
                    panelPoints.ScrollControlIntoView(panelPoints.Controls[0]);
                    activateFiltersClick();
                }
            }
            else
            {
                InitialiseHomepage();
            }
        }

        /*
         * Permet l'édition du titre d'un point
         */
        private void editTitlePoint(MySqlDataAdapter ad, DataSet data, Control pan, Control control, ref string IDPoint, out Point pt1, out Point pt2)
        {
            IDPoint = Tools.returnID(pan.Name);

            Request.useSelectRequestTable(ad, data.Tables[Constantes.points], SQL.selectPointByIDPoint("*", IDPoint));
            TextBox titlePoint = createTextBoxTitlePoint(pan, control);
            pan.Controls.Add(titlePoint);
            control.Visible = false;
            pt1 = new Point(titlePoint.Left, titlePoint.Bottom + 2);
            pt2 = new Point(titlePoint.Right, titlePoint.Bottom + 2);
        }

        /*
         * Permet l'édition de la description d'un point
         */
        private void editDescriptionPoint(Control pan, Control control, out Point pt1, out Point pt2)
        {
            TextBox descriptionPoint = createTextBoxDescriptionPoint(pan, control);
            pan.Controls.Add(descriptionPoint);
            control.Visible = false;

            pt1 = new Point(descriptionPoint.Left, descriptionPoint.Bottom + 2);
            pt2 = new Point(descriptionPoint.Right, descriptionPoint.Bottom + 2);
        }

        /*
         * Crée un objet TextBox pour éditer le titre d'un point
         */
        private TextBox createTextBoxTitlePoint(Control pan, Control control)
        {
            TextBox tBox = new TextBox();
            createTextBoxPoint(pan, tBox, control, Constantes.textBoxTitrePoint, 145);
            return tBox;
        }

        /*
         * Crée un objet TextBox pour éditer la description d'un point
         */
        private TextBox createTextBoxDescriptionPoint(Control pan, Control control)
        {
            TextBox tBox = new TextBox();
            createTextBoxPoint(pan, tBox, control, Constantes.textBoxDescriptionPoint, 250);
            return tBox;
        }

        /*
         * Crée un objet TextBox
         */
        private void createTextBoxPoint(Control pan, TextBox tBox, Control control, string name, int width)
        {
            tBox.Name = name;
            tBox.Location = new Point(control.Location.X + 4, control.Location.Y);
            tBox.Text = control.Text;
            tBox.Font = control.Font;
            tBox.Size = new Size(Tools.changeLabelSize(control), 18);
            tBox.BorderStyle = BorderStyle.None;
            tBox.Enter += TBox_Enter;
            tBox.Leave += TBox_Leave;
            tBox.PreviewKeyDown += TBox_PreviewKeyDown;
            tBox.KeyUp += TBox_KeyUp;
            tBox.TextChanged += TBox_TextChanged;

            if (string.IsNullOrWhiteSpace(tBox.Text))
            {
                Display.displayPlaceholder(tBox);
            }
        }

        /*
         * Permet l'édition de tous les points affichés à l'écran
         */
        private void editPoints()
        {
            DataSet data = new DataSet();
            MySqlDataAdapter ad = new MySqlDataAdapter();
            string cmd = string.Empty;
            string IDPoint = string.Empty;
            data.Tables.Add(Constantes.points);
            Control labelTitle = new Control();
            Control labelDescription = new Control();

            Point pt1Title = new Point();
            Point pt2Title = new Point();
            Point pt1Desc = new Point();
            Point pt2Desc = new Point();

            foreach (Control pan in panelPoints.Controls)
            {
                if (pan.GetType() == typeof(Panel) && pan.Name.StartsWith(Constantes.panelPoint))
                {
                    foreach (Control control in pan.Controls)
                    {
                        if (control.Name == Constantes.labelTitrePoint)
                        {
                            editTitlePoint(ad, data, pan, control, ref IDPoint, out pt1Title, out pt2Title);
                            PictureBox PBDelete = AjoutElement.addPBDelete(IDPoint);
                            PBDelete.Click += new EventHandler(btnDeletePoint_Click);
                            PBDelete.MouseEnter += btn_MouseEnter;
                            PBDelete.MouseLeave += btn_MouseLeave;
                            pan.Controls.Add(PBDelete);
                            labelTitle = control;
                        }
                        else if (control.Name == Constantes.labelDescriptionPoint)
                        {
                            editDescriptionPoint(pan, control, out pt1Desc, out pt2Desc);
                            labelDescription = control;
                        }
                        else if (control.Name.StartsWith(Constantes.pointEntwickler))
                        {
                            control.Click += new EventHandler(this.pointEntw_Click);
                        }
                        else if (control.Name.StartsWith(Constantes.pointBerater))
                        {
                            control.Click += new EventHandler(this.pointBerater_Click);
                        }
                        else if (control.Name.StartsWith(Constantes.pointTechnik))
                        {
                            control.Click += new EventHandler(this.pointTechnik_Click);
                        }
                        else if (control.Name.StartsWith(Constantes.pointKunde))
                        {
                            control.Click += new EventHandler(this.pointKunde_Click);
                        }
                        else if (control.Name == "InfoPoint")
                        {
                            control.Visible = false;
                        }
                    }
                    Graphics g = pan.CreateGraphics();
                    Pen p = new Pen(Color.LightGray);

                    g.DrawLine(p, pt1Title, pt2Title);
                    g.DrawLine(p, pt1Desc, pt2Desc);

                    pan.Controls.Remove(labelTitle);
                    pan.Controls.Remove(labelDescription);
                }
            }
        }

        /*
         * Se produit lorsque l'on ajoute une checklist
         */
        private void addChecklist(string name)
        {
            proAlpha.Visible = false;
            labelHome.Visible = false;
            check = null;
            Favorites.favoriBleu(favoris);
            if (treeView1.Visible == false)
            {
                Display.displayMenu(treeView1, listFavorites, labelFavorites, whiteHeart);
            }
            // les boutons ne devraient pas être autorisés
            panelPointsVisible();
            // btnPrint.Enabled = false;
            btnExport.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            favoris.Enabled = false;
            if (nodeTopic.Text == string.Empty)
            {
                MessageBox.Show("Choisir le topic qui contiendra la checklist à ajouter.");
            }
            else
            {
                if (nodeTopic.IsExpanded == false)
                {
                    nodeTopic.Expand();
                }
                if (nodeTopic.IsSelected == false)
                {
                    nodeTopic.TreeView.SelectedNode = nodeTopic;
                }
            }
            panelPoints.Controls.Clear();
            panelPoints.AutoScroll = true;
            typesUser.Tables[Constantes.typesPoints].Rows.Clear();
            createChecklist();
            btnSettingsClick = name;
        }

        /*
         * Permet de modifier le titre d'une catégorie
         */
        private void modifCategory()
        {
            if (nodeCategory.IsExpanded == false)
            {
                nodeCategory.Expand();
            }
            if (nodeCategory.IsSelected == false)
            {
                nodeCategory.TreeView.SelectedNode = nodeTopic; // ?
            }
            first = true;
            nodeCategory.TreeView.LabelEdit = true;
            categorieAModifier = nodeCategory.Text;
            nodeCategory.BeginEdit();
        }

        /*
         * Permet de modifier le titre d'un topic
         */
        private void modifTopic()
        {
            if (nodeTopic.IsExpanded == false)
            {
                nodeTopic.Expand();
            }
            if (nodeTopic.IsSelected == false)
            {
                nodeTopic.TreeView.SelectedNode = nodeTopic;
            }
            first = true;
            nodeTopic.TreeView.LabelEdit = true;
            topicAModifier = nodeTopic.Text;
            nodeTopic.BeginEdit();
        }

        /*
         * Permet de filtrer les points d'une checklist par rapport au type d'utilisateur
         */
        // filtre les points en fonction des types sélectionnés
        private void filterPoints()
        {
            panelPointsVisible();
            string cmd = string.Empty;
            DataSet filter = new DataSet();
            MySqlDataAdapter ad = new MySqlDataAdapter();

            Filters.verifySelectedType(ad, filter, 1, SQL.selectPointsType(check.IDChecklist, 1), typesUser, check.IDChecklist);
            Filters.verifySelectedType(ad, filter, 2, SQL.selectPointsType(check.IDChecklist, 2), typesUser, check.IDChecklist);
            Filters.verifySelectedType(ad, filter, 3, SQL.selectPointsType(check.IDChecklist, 3), typesUser, check.IDChecklist);
            Filters.verifySelectedType(ad, filter, 4, SQL.selectPointsType(check.IDChecklist, 4), typesUser, check.IDChecklist);

            if (filter.Tables.Count > 0)
            {
                List<int> enumerable = new List<int>();
                List<List<int>> tables = new List<List<int>>();
                for (int i = 0; i < filter.Tables.Count; i++)
                {
                    tables.Add(new List<int>());
                    foreach (DataRow row in filter.Tables[i].Rows)
                    {
                        tables[tables.Count - 1].Add(Int32.Parse(row.ItemArray[0].ToString()));
                    }
                }
                if (filter.Tables.Count > 1)
                {
                    enumerable = tables[1].Intersect(tables[0]).ToList<int>();
                    for (int i = 2; i < tables.Count; i++)
                    {
                        enumerable = tables[i].Intersect(enumerable).ToList<int>();
                    }
                }
                else
                {
                    enumerable = tables[0];
                }
                displayPoints(enumerable);
            }
            else
            {
                filter.Tables.Add(Constantes.points);
                Request.useSelectRequestTable(ad, filter.Tables[Constantes.points], SQL.selectPointsByIDChecklist("*", check.IDChecklist.ToString()));

                filter.Tables.Add(Constantes.checklist);
                Request.useSelectRequestTable(ad, filter.Tables[Constantes.checklist], SQL.selectChecklistByID("*", check.IDChecklist.ToString()));

                refreshPoints(filter, ad, true);
            }
        }

        /*
         * Permet de supprimer une catégorie
         * nom : deleteCategory déjà utilisé
         */
        private void supprimerCategorie(TreeNode node)
        {
            MySqlDataAdapter ad = new MySqlDataAdapter();
            DataTable dCategorie = new DataTable(Constantes.categorie);
            DataTable dTopic = new DataTable(Constantes.topic);
            DataTable dChecklist = new DataTable(Constantes.checklist);
            DataTable dPoint = new DataTable(Constantes.point);
            string categorie = node.Text;

            int IDCategorie = 0;
            int IDTopic = 0;
            int IDChecklist = 0;
            int IDPoint = 0;

            Request.useSelectRequestTable(ad, dCategorie, SQL.selectCategoryByTitle("IDCategorie", categorie));
            IDCategorie = Int32.Parse(dCategorie.Rows[0][0].ToString());
            Request.useSelectRequestTable(ad, dTopic, SQL.selectTopicsByIDCategorie("IDTopic", IDCategorie.ToString()));

            // Modifier la fonction si on enlève les colonnes IDCategorie et IDTopic
            Request.deleteFavorites(ad, IDCategorie, "IDCategorie1");

            foreach (DataRow item in dTopic.Rows)
            {
                IDTopic = Int32.Parse(item.ItemArray[0].ToString());
                DB.supprimerChecklistsDUnTopic(ad, dChecklist, dPoint, IDTopic, IDChecklist, IDPoint);
            }
            Request.deleteTopic(ad, IDCategorie, "IDCategorie");
            dTopic.Rows.Clear();

            Request.deleteCategory(ad, IDCategorie);
            dCategorie.Rows.Clear();

            DB.verificationModifsToDelete();

            if (check != null)
            {
                DataTable checklistSuppr = new DataTable();

                Request.useSelectRequestTable(ad, checklistSuppr, SQL.selectChecklistByID("*", check.IDChecklist.ToString()));
                if (checklistSuppr.Rows.Count == 0)
                {
                    check = null;
                    InitialiseHomepage();
                }
            }
            // supprimer de categorie -> topic -> checklist -> point -> type_point -> favoris
            // modif_checklist -> modif_point
        }

        /*
         * Permet de supprimer un topic
         */
        private void deleteTopic(TreeNode node)
        {
            MySqlDataAdapter ad = new MySqlDataAdapter();
            DataTable dTopic = new DataTable(Constantes.topic);
            DataTable dChecklist = new DataTable(Constantes.checklist);
            DataTable dPoint = new DataTable(Constantes.point);
            string topic = node.Text;

            int IDTopic = 0;
            int IDChecklist = 0;
            int IDPoint = 0;

            string idCategorie = "(" + SQL.selectCategoryByTitle("IDCategorie", node.Parent.Text) + ")";
            Request.useSelectRequestTable(ad, dTopic, SQL.selectTopicByTitle("IDTopic", topic, idCategorie));
            IDTopic = Int32.Parse(dTopic.Rows[0][0].ToString());

            Request.deleteFavorites(ad, IDTopic, "IDTopic1");

            DB.supprimerChecklistsDUnTopic(ad, dChecklist, dPoint, IDTopic, IDChecklist, IDPoint);

            Request.deleteTopic(ad, IDTopic, "IDTopic");
            dTopic.Rows.Clear();

            DB.verificationModifsToDelete();

            if (check != null)
            {
                DataTable checklistSuppr = new DataTable();
                Request.useSelectRequestTable(ad, checklistSuppr, SQL.selectChecklistByID("*", check.IDChecklist.ToString()));
                if (checklistSuppr.Rows.Count == 0)
                {
                    check = null;
                    InitialiseHomepage();
                }
            }
        }

        /*
         * Met à jour les points de la checklist affichée
         */
        private void updatePoints(MySqlDataAdapter adapt, DataSet data)
        {
            if (data.Tables[Constantes.points].Rows.Count == 0)
            {
                panelPoints.Controls.Clear();
            }
            else
            {
                refreshPoints(data, adapt, false);
            }
        }

        // crée un dataset avec les points de la liste pour refresh
        private void displayPoints(List<int> IDPoints)
        {
            MySqlDataAdapter adapt = new MySqlDataAdapter();
            DataSet data = new DataSet();
            data.Tables.Add(Constantes.points);
            string cmd = string.Empty;
            foreach (int point in IDPoints)
            {
                Request.useSelectRequestTable(adapt, data.Tables[Constantes.points], SQL.selectPointByIDPoint("*", point.ToString()));
            }
            string IDChecklist = string.Empty;
            if (IDPoints.Count > 0)
            {
                IDChecklist = data.Tables[Constantes.points].Rows[0][7].ToString();
            }
            else
            {
                IDChecklist = check.IDChecklist.ToString();
            }
            data.Tables.Add(Constantes.checklist);
            Request.useSelectRequestTable(adapt, data.Tables[Constantes.checklist], SQL.selectChecklistByID("*", IDChecklist));
            refreshPoints(data, adapt, true);
        }

        /*
         * Met à jour un point à l'écran
         */
        private void refreshOnePoint(DataSet data, MySqlDataAdapter adapt, Panel panelPoint, string IDpoint, string titlePoint, string DescriptionPoint)
        {
            // Initialiser les types du point : aucun type
            typesUser.Tables[Constantes.typesPoints].Rows.Add(Int32.Parse(IDpoint), false, false, false, false);

            Tools.resetDataTable(data, Constantes.types);
            // Renvoie les types du point
            Request.useSelectRequestTable(adapt, data.Tables[Constantes.types], SQL.selectTypesByIDPoint(IDpoint));

            data.Tables[Constantes.types].PrimaryKey = new DataColumn[] { data.Tables[Constantes.types].Columns[0] };

            PictureBox pbEntwickler = AjoutElement.addPBEntwickler(IDpoint);
            PictureBox pbBerater = AjoutElement.addPBBerater(IDpoint);
            PictureBox pbTechnik = AjoutElement.addPBTechnik(IDpoint);
            PictureBox pbKunde = AjoutElement.addPBKunde(IDpoint);

            initTypes(data.Tables[Constantes.types], pbEntwickler, pbBerater, pbTechnik, pbKunde,
                typesUser.Tables[Constantes.typesPoints].Rows[typesUser.Tables[Constantes.typesPoints].Rows.Count - 1]);

            PictureBox pbCheck = AjoutElement.addPBCheck();
            pbCheck.Click += new EventHandler(this.check_Click);
            Label lTitle = AjoutElement.addLabelTitre(titlePoint, Constantes.labelTitrePoint, 84, 18, 67, 23, Color.Black);
            lTitle.Size = new Size(Tools.changeLabelSize(lTitle), lTitle.Size.Height);
            Label lDescPoint = AjoutElement.addLabelDescription(DescriptionPoint, Constantes.labelDescriptionPoint, new Point(84, 47), new Size(136, 20), Color.Black);

            PictureBox pbInfo = AjoutElement.addPBInfo(lTitle.Location.X + lTitle.Width);

            List<PictureBox> listPB = new List<PictureBox>();
            listPB.Add(pbKunde);
            listPB.Add(pbTechnik);
            listPB.Add(pbCheck);
            listPB.Add(pbBerater);
            listPB.Add(pbInfo);
            listPB.Add(pbEntwickler);

            Display.infosPoint(IDpoint, pbInfo, toolTip1);
            AjoutElement.addControlsPanelPoint(panelPoint, listPB, lDescPoint, lTitle);
        }

        /*
         * Met à jour les points à afficher à l'écran
         */
        private void refreshPoints(DataSet data, MySqlDataAdapter adapt, bool filter)
        {
            panelPoints.Controls.Clear();
            panelPoints.AutoScroll = true;
            typesUser.Tables[Constantes.typesPoints].Rows.Clear();

            if (data.Tables[Constantes.points].Rows.Count > 0 || filter == true)
            {
                displayTypesUser();
            }
            Display.infosChecklist(data.Tables[Constantes.checklist].Rows[0][0].ToString(), data.Tables[Constantes.checklist].Rows[0], infoChecklist, toolTip1,
                titreChecklist, shortDesc, mysql, favoris, btnEdit, btnCancel, btnDelete, btnExport, btnAddPoint);
            foreach (DataRow point in data.Tables[Constantes.points].Rows)
            {
                Panel panelPoint = new Panel();
                panelPoints.Controls.Add(panelPoint);

                int locY = 0;
                if (Tools.firstPoint(panelPoints))
                {
                    locY = Constantes.locationYPremierPoint;
                }
                else
                {
                    locY = Tools.locationYNewPoint(panelPoints, Constantes.panelPoint);
                }
                refreshOnePoint(data, adapt, panelPoint, point.ItemArray[0].ToString(), point.ItemArray[1].ToString(), point.ItemArray[2].ToString());
                AjoutElement.initPanelPoint(panelPoint, locY, Int32.Parse(point.ItemArray[0].ToString()));
            }
            if (panelPoints.ClientRectangle != panelPoints.DisplayRectangle)
            {
                panelPoints.AutoScroll = false;
                panelPoints.VerticalScroll.Visible = true;
                panelPoints.VerticalScroll.Enabled = true;
                panelPoints.AutoScroll = true;
                firstTime = true;
            }
            else
            {
                panelPoints.AutoScroll = false;
                if (firstTime)
                {
                    firstTime = false;
                    panelPoints.VerticalScroll.Visible = true;
                    panelPoints.VerticalScroll.Enabled = true;
                }
                panelPoints.AutoScroll = true;

                // panelPoints.AutoScroll = false;
                panelPoints.VerticalScroll.Value = 0;
                panelPoints.HorizontalScroll.Value = 0;
                panelPoints.AutoScroll = false;
                panelPoints.VerticalScroll.Visible = false;
                panelPoints.VerticalScroll.Enabled = false;
            }
        }

        /*
         * Affiche les types d'utilisateur qui permettent de filtrer les points 
         */
        private void displayTypesUser()
        {
            Label sort = AjoutElement.initLabel();
            PictureBox pKunde = new PictureBox();
            PictureBox pEntw = new PictureBox();
            PictureBox pTechnik = new PictureBox();
            PictureBox pBerater = new PictureBox();

            AjoutElement.initType(panelPoints, pKunde, 252, Constantes.typeKunde);
            pKunde.Click += new EventHandler(this.pKunde_Click);
            pKunde.Paint += new PaintEventHandler(this.pKunde_Paint);

            AjoutElement.initType(panelPoints, pEntw, 168, Constantes.typeEntwickler);
            pEntw.Click += new EventHandler(this.pEntw_Click);
            pEntw.Paint += new PaintEventHandler(this.pEntw_Paint);

            AjoutElement.initType(panelPoints, pTechnik, 224, Constantes.typeTechnik);
            pTechnik.Click += new EventHandler(this.pTechnik_Click);
            pTechnik.Paint += new PaintEventHandler(this.pTechnik_Paint);

            AjoutElement.initType(panelPoints, pBerater, 196, Constantes.typeBerater);
            pBerater.Click += new EventHandler(this.pBerater_Click);
            pBerater.Paint += new PaintEventHandler(this.pBerater_Paint);

            panelPoints.Controls.Add(sort);
            toolTip1.RemoveAll();
            toolTip1.SetToolTip(pEntw, Constantes.entwickler);
            toolTip1.SetToolTip(pBerater, Constantes.berater);
            toolTip1.SetToolTip(pTechnik, Constantes.technik);
            toolTip1.SetToolTip(pKunde, Constantes.kunde);
            toolTip1.SetToolTip(favoris, "Favorites");
            toolTip1.SetToolTip(btnEdit, "Edit");
            toolTip1.SetToolTip(btnCancel, "Cancel");
            toolTip1.SetToolTip(btnDelete, "Delete");
            toolTip1.SetToolTip(btnExport, "Export");
            toolTip1.SetToolTip(btnAddPoint, "Add point");
        }

        /*
         * Initialise les types d'utilisateur du point actuel
         */
        private void initTypes(DataTable table, PictureBox pbEntwickler, PictureBox pbBerater, PictureBox pbTechnik, PictureBox pbKunde, DataRow row)
        {
            if (table.Rows.Contains(1))
            {
                row[Constantes.entwickler] = true;
            }
            else
            {
                row[Constantes.entwickler] = false;
            }
            if (table.Rows.Contains(2))
            {
                row[Constantes.berater] = true;
            }
            else
            {
                row[Constantes.berater] = false;
            }
            if (table.Rows.Contains(3))
            {
                row[Constantes.technik] = true;
            }
            else
            {
                row[Constantes.technik] = false;
            }
            if (table.Rows.Contains(4))
            {
                row[Constantes.kunde] = true;
            }
            else
            {
                row[Constantes.kunde] = false;
            }
            pbEntwickler.Paint += new PaintEventHandler(this.typeEntwickler_Paint);
            pbBerater.Paint += new PaintEventHandler(this.typeBerater_Paint);
            pbTechnik.Paint += new PaintEventHandler(this.typeTechnik_Paint);
            pbKunde.Paint += new PaintEventHandler(this.typeKunde_Paint);
        }

        private void addEventsTBox(TextBox tBox)
        {
            tBox.Enter += TBox_Enter;
            tBox.Leave += TBox_Leave;
            tBox.PreviewKeyDown += TBox_PreviewKeyDown;
            tBox.KeyUp += TBox_KeyUp;
            tBox.TextChanged += TBox_TextChanged;
        }

        /*
         * Crée la liste des favoris
         */
        private ListView creationFavoritesList()
        {
            labelFavorites = AjoutElement.creationLabelFavoris();

            listFavorites = AjoutElement.creationListeFavoris();
            listFavorites.Click += ListeFavoris_Click;

            whiteHeart.Visible = true;
            whiteHeart.Location = new Point(home.Location.X, 170);
            whiteHeart.Size = new Size(34, 32);

            splitContainer2.Panel1.Controls.Add(labelFavorites);
            splitContainer2.Panel1.Controls.Add(listFavorites);

            MySqlDataAdapter ad = new MySqlDataAdapter();
            DataTable favoris = new DataTable();
            DataTable titleChecklist = new DataTable();

            Request.useSelectRequestTable(ad, favoris, SQL.selectFavorisByIDUtilisateur());

            string cmdSelect = string.Empty;
            ListViewItem item = new ListViewItem();

            foreach (DataRow favori in favoris.Rows)
            {
                item = new ListViewItem();
                titleChecklist.Clear();
                titleChecklist.Columns.Clear();
                Request.useSelectRequestTable(ad, titleChecklist, SQL.selectChecklistByID("Titre", favori.ItemArray[3].ToString()));

                item.Text = titleChecklist.Rows[0][0].ToString();
                item.Name = Constantes.listViewItem + favori.ItemArray[3].ToString();

                // item.ImageIndex = 0;

                listFavorites.Items.Add(item);
            }
            return listFavorites;
        }

        private void refreshHomePage()
        {
            if (panelChecklists.Visible)
            {
                panelChecklists.Controls.Clear();
                InitialiseHomepage();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (btnCancel.Visible)
                {
                    cancel();
                }
            }
        }

        /*
         * Se produit lors de la fermeture de l'application
         */
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnEdit.Visible == false && btnCancel.Visible == true)
            {
                DialogResult result = Display.cancelEdition(creationChecklist);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    mysql.Close();
                }
            }
        }

        private void LabelChecklist_MouseEnter(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Panel checklist = (Panel)label.Parent;
            if (checklist.BackColor == Constantes.grayCheck)
            {
                checklist.BackColor = Constantes.lightGray;
            }
        }

        private void LabelChecklist_MouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Panel checklist = (Panel)label.Parent;
            if (checklist.BackColor == Constantes.lightGray)
            {
                checklist.BackColor = Constantes.grayCheck;
            }
        }

        public void PanelChecklist_Click(object sender, EventArgs e)
        {
            Panel panelChecklist = (Panel)sender;
            checklistClick(panelChecklist);
        }

        /*
         * Action lorsque l'on clique sur le titre d'une checklist (à partir de l'écran d'affichage des 5 dernières checklists)
         */
        private void Checklist_Click(object sender, EventArgs e)
        {
            Label labelChecklist = (Label)sender;
            Panel panelChecklist = (Panel)labelChecklist.Parent;
            checklistClick(panelChecklist);
        }

        /*
         * Dessine un noeud du treeView
         */
        // Treeview
        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            // Retrieve the node font. If the node font has not been set,
            // use the TreeView font.

            if (e.Node.NodeFont == null)
            { e.Node.NodeFont = ((TreeView)sender).Font; }

            // Console.WriteLine(e.Node.Text + " : " + e.State.ToString());

            // Draw the node text and the node backcolor.
            if (Tree.treeNodeMouseEnter(e))
            {
                /*if (ignorerHot == false)
                {*/
                Graphic.fillRectangle(e, 4, 282, Color.LightSlateGray);
                Graphic.hideRectangleHighlight(e);
                /*}
                else
                {
                    fillRectangleBackColor(e);
                }*/
            }
            /*
            // problème : celui qui a l'état "hot" reste encadré
            else if (e.State.ToString().IndexOf(TreeNodeStates.Focused.ToString()) >= 0 && e.Node.Level != 2)
            {
                if (ignorerHot)
                {
                    fillRectangle(e,4,282,Color.LightSlateGray);
                }
                else
                {
                    fillRectangleBackColor(e);
                }
            }*/
            else
            {
                Graphic.fillRectangleBackColor(e);
            }
            /*
            bool itemModifie = false;
            string ID = returnID(e.Node);

            itemModifie = false;
            foreach (ListViewItem item in states.Items)
            {
                if (item.SubItems[0].Text == e.Node.Level.ToString() && item.SubItems[1].Text == ID)
                {
                    itemModifie = true;
                    item.SubItems[2].Text = e.State.ToString();
                }
            }
            if (itemModifie == false)
            {
                ListViewItem lvItem = new ListViewItem();
                lvItem.SubItems[0].Text = e.Node.Level.ToString();
                lvItem.SubItems.Add(ID);
                lvItem.SubItems.Add(e.State.ToString());
                states.Items.Add(lvItem);
            }
            */
            // If a node tag is present, draw its icon
            // to the right of the label text.
            if (e.Node.Tag != null && e.Node.Level == 0)
            {
                e.Graphics.DrawString(e.Node.Text, e.Node.NodeFont, Constantes.grayBrush, Rectangle.Inflate(new Rectangle(e.Bounds.X, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height), 2, 0));
                Image im = (Image)(e.Node.Tag);
                e.Graphics.DrawImage(im, 246, (Tree.NodeBounds(e.Node, treeView1).Top) + 8);
                /*
                Image transparent = Resources.transparent;

                if (e.Node.Index == indexNoeud)
                {
                    if (rotationFinie == false)
                    {

                        for (int i = 0 ; i < 90 ; i++)
                        {
                            im = RotateImage2(im,1);
                            e.Node.Tag = im;
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                           // treeView1.Invalidate(e.Node.Bounds);
                           // e.Graphics.DrawRectangle(new Pen(Constantes.bleu),new Rectangle(240,NodeBounds(e.Node).Top,im.Width,im.Height));
                           // e.Graphics.DrawImage(transparent,240,NodeBounds(e.Node).Top);
                            e.Graphics.FillRectangle(new SolidBrush(Constantes.bleu),240,NodeBounds(e.Node).Top,im.Width,im.Height);
                            e.Graphics.DrawImage(im,240,NodeBounds(e.Node).Top);
                            System.Threading.Thread.Sleep(2);
                        }
                        rotationFinie = true;
                    }
                }
                else
                {
                    if (e.Node.IsExpanded)
                    {
                        im = RotateImage2(im,1);
                        e.Node.Tag = im;
                    }
                    e.Graphics.DrawImage(im,240,NodeBounds(e.Node).Top);
                }*/
                // Icon icn = (Icon)(e.Node.Tag);
                // e.Graphics.DrawIcon(icn,240,NodeBounds(e.Node).Top);  
            }
            else if (e.Node.Level == 1)
            {
                e.Graphics.DrawString(e.Node.Text, e.Node.NodeFont, Constantes.grayBrush, Rectangle.Inflate(new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height), 2, 0));
                e.Graphics.DrawImage(tagBitmapTopic, 10, (Tree.NodeBounds(e.Node, treeView1).Top + 11));
            }
            else if (e.Node.Level == 2)
            {
                e.Graphics.DrawString(e.Node.Text, e.Node.NodeFont, Constantes.grayBrush, Rectangle.Inflate(new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height), 2, 0));
                e.Graphics.DrawImage(bitmapCheckMark, 28, (Tree.NodeBounds(e.Node, treeView1).Top + 11));
                if (e.Node.IsSelected && Tools.displayedChecklist(e.Node, check))
                {
                    /*if (IDLigneDessinee == Int32.Parse(returnID(e.Node)))
                    {
                        fillRectangle(e,4,282,Color.LightSlateGray);
                        hideRectangleHighlight(e);
                        e.Graphics.DrawString(e.Node.Text,e.Node.NodeFont,Constantes.brushGris,Rectangle.Inflate(new Rectangle(e.Bounds.X + 6,e.Bounds.Y + 2,e.Bounds.Width,e.Bounds.Height),2,0));
                        e.Graphics.DrawImage(bitmapCheckMark,28,(NodeBounds(e.Node).Top + 11));
                    }*/
                    e.Graphics.DrawLine(new Pen(Constantes.pink), new Point(e.Bounds.Left + 6, e.Bounds.Bottom - 4), new Point(e.Bounds.Right, e.Bounds.Bottom - 4));
                    IDLigneDessinee = Int32.Parse(Tools.returnID(e.Node));
                }
                else if (Tools.displayedChecklist(e.Node, check))
                {
                    e.Graphics.DrawLine(new Pen(Constantes.pink), new Point(e.Bounds.Left + 6, e.Bounds.Bottom - 4), new Point(e.Bounds.Right, e.Bounds.Bottom - 4));
                    IDLigneDessinee = Int32.Parse(Tools.returnID(e.Node));
                }
                else if (e.Node.IsSelected)
                {
                    Graphic.fillRectangle(e, 4, 282, Color.LightSlateGray);
                    Graphic.hideRectangleHighlight(e);
                    e.Graphics.DrawString(e.Node.Text, e.Node.NodeFont, Constantes.grayBrush, Rectangle.Inflate(new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height), 2, 0));
                    e.Graphics.DrawImage(bitmapCheckMark, 28, (Tree.NodeBounds(e.Node, treeView1).Top + 11));
                }
            }
            if (e.Node.IsEditing)
            {
                Graphic.fillRectangleBackColor(e);
            }
        }

        // modif icone node
        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Tag = tagBitmap2;
        }

        // modif icone node
        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Tag = tagBitmap;
        }

        // Clic à un autre endroit qu'un noeud
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode node = treeView1.GetNodeAt(e.X, e.Y);
            if (node == null && e.Button == MouseButtons.Right)
            {
                Tree.InitialiseContextMenuTreeview(Constantes.autre, contextMenuStrip1);
                contextMenuStrip1.Show(treeView1, e.X, e.Y);
            }
        }

        /*
         * Se produit lors du clic sur un noeud du treeView
         */
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (btnEdit.Visible == false && btnCancel.Visible == true)
            {
                DialogResult result = Display.cancelEdition(creationChecklist);
                if (result == DialogResult.No)
                {
                    return;
                }
                else
                {
                    cancel();
                }
            }
            if (e.Node.Level == 0)
            {
                // selectedCategory = e.Node.Text;
                nodeCategory = e.Node;
                if (e.Button == MouseButtons.Left)
                {
                    if (e.Node.IsExpanded)
                    {
                        e.Node.Collapse();
                    }
                    else
                    {
                        e.Node.Expand();
                    }
                }
                //Graphics g = treeView1.CreateGraphics();
                // g.DrawRectangle(Pens.Red,new Rectangle(e.Node.Bounds.X - 2,e.Node.Bounds.Y,270,e.Node.Bounds.Height));
            }
            else if (e.Node.Level == 1)
            {
                // selectedCategory = e.Node.Parent.Text;
                nodeTopic = e.Node;
                if (e.Button == MouseButtons.Left)
                {
                    if (e.Node.IsExpanded)
                    {
                        e.Node.Collapse();
                    }
                    else
                    {
                        e.Node.Expand();
                    }
                }
                // selectedTopic = e.Node.Text;
            }
            else if (e.Node.Level == 2) // voir dans le cas d'une création
            {
                MySqlDataAdapter adapt = new MySqlDataAdapter();
                DataSet data = new DataSet();
                data.Tables.Add(Constantes.checklist);

                Request.useSelectRequestTable(adapt, data.Tables[Constantes.checklist], SQL.selectChecklistNode(e.Node));
                displayChecklist(adapt, data);
                e.Node.TreeView.Refresh();
            }
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.Level == 0)
                {
                    Tree.InitialiseContextMenuTreeview(Constantes.categorie, contextMenuStrip1);
                }
                else if (e.Node.Level == 1)
                {
                    Tree.InitialiseContextMenuTreeview(Constantes.topic, contextMenuStrip1);
                }
                else if (e.Node.Level == 2)
                {
                    // pas utilisé
                    Tree.InitialiseContextMenuTreeview(Constantes.checklist, contextMenuStrip1);
                }
                e.Node.TreeView.SelectedNode = e.Node;
                contextMenuStrip1.Show(treeView1, e.X, e.Y);
            }
        }

        private void TreeView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                cancelLabelEdit = true;
            }
        }

        /*
         * Se produit après l'édition d'un noeud du treeView
         */
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            firstEdition = false;
            if (cancelLabelEdit == false)
            {
                if (e.Label != null && e.Node.TreeView.SelectedNode != null)
                {
                    MySqlDataAdapter ad = new MySqlDataAdapter();
                    int idCategorie = 0;
                    int idTopic = 0;
                    if (btnSettingsClick == Constantes.changeCategory)
                    {
                        Tools.returnIDCategory(ad, ref idCategorie, SQL.selectCategoryByTitle("*", categorieAModifier), dsResearch);
                    }
                    else if (btnSettingsClick == Constantes.changeTopic)
                    {
                        Tools.returnIDCategoryAndTopic(ad, ref idCategorie, ref idTopic, dsResearch, nodeTopic, topicAModifier);
                    }
                    else if (btnSettingsClick == Constantes.addCategory)
                    {
                        if (e.Label.Length > 0)
                        {
                            if (DB.verifyDuplicateCategory(ad, e.Label) == false)
                            {
                                e.Node.BeginEdit();
                                first = true;
                                return;
                            }
                            Request.insertRequest(ad, SQL.insertCategory(e.Label));
                            e.Node.Tag = tagBitmap;
                        }
                        else
                        {
                            if (Tools.emptyTitle(Constantes.checklist) == false)
                            {
                                e.Node.BeginEdit();
                                first = true;
                                return;
                            }
                        }
                    }
                    else if (btnSettingsClick == Constantes.addTopic)
                    {
                        Tools.returnIDCategory(ad, ref idCategorie, SQL.selectCategoryByTitle("IDCategorie", e.Node.Parent.Text), dsResearch);
                        if (e.Label.Length > 0)
                        {
                            if (DB.verifyDuplicateTopic(ad, idCategorie, e.Label) == false)
                            {
                                e.Node.BeginEdit();
                                first = true; // utile ?
                                return;
                            }
                            Request.insertRequest(ad, SQL.insertTopic(e.Label, idCategorie));
                        }
                        else
                        {
                            if (Tools.emptyTitle(Constantes.topic) == false)
                            {
                                e.Node.BeginEdit();
                                first = true; // utile ?
                                return;
                            }
                        }
                    }
                    if (btnSettingsClick == Constantes.changeCategory || btnSettingsClick == Constantes.changeTopic)
                    {
                        string cmdUpdate = string.Empty;
                        if (btnSettingsClick == Constantes.changeCategory)
                        {
                            if (idCategorie > 0) // modifier ça
                            {
                                if (e.Label != categorieAModifier)
                                {
                                    if (e.Label.Length == 0)
                                    {
                                        if (Tools.emptyTitle(Constantes.categorie) == false)
                                        {
                                            e.Node.BeginEdit();
                                            first = true;
                                            return;
                                        }
                                    }
                                    if (DB.verifyDuplicateCategory(ad, e.Label) == false)
                                    {
                                        e.Node.BeginEdit();
                                        e.Node.Text = categorieAModifier;
                                        first = true;
                                        return;
                                    }
                                    cmdUpdate += SQL.updateTitleCategory(e.Label, idCategorie);
                                }
                                else
                                {
                                    e.Node.Text = e.Label;
                                    e.Node.EndEdit(false);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Veuillez sélectionner la catégorie à modifier.");
                                return;
                            }
                        }
                        else if (btnSettingsClick == Constantes.changeTopic) // Besoin de l'IDCategorie et du topic
                        {
                            if (idTopic != 0)
                            {
                                if (e.Label != dsResearch.Tables[Constantes.topic].Rows[0][1].ToString())
                                {
                                    if (e.Label.Length == 0)
                                    {
                                        if (Tools.emptyTitle(Constantes.topic) == false)
                                        {
                                            e.Node.BeginEdit();
                                            first = true;
                                            return;
                                        }
                                    }
                                    if (DB.verifyDuplicateTopic(ad, idCategorie, e.Label) == false)
                                    {
                                        e.Node.BeginEdit();
                                        first = true;
                                        return;
                                    }
                                    cmdUpdate += SQL.updateTitleTopic(e.Label, idTopic);
                                }
                                else
                                {
                                    e.Node.Text = e.Label;
                                    e.Node.EndEdit(false);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Veuillez sélectionner le topic à modifier.");
                                // Problème
                                return;
                            }
                        }
                        Request.updateRequest(ad, cmdUpdate);
                    }
                }
                else if (btnSettingsClick == Constantes.addTopic && Tools.emptyTitle(Constantes.topic) == false)
                {
                    e.Node.BeginEdit();
                    first = true; // utile ?
                    return;
                }
                // quand on presse sur echap, il va à chaque fois dans cette boucle et ne peut donc pas annuler
                else if (btnSettingsClick == Constantes.addCategory && Tools.emptyTitle(Constantes.categorie) == false)
                {
                    e.Node.BeginEdit();
                    first = true;
                    return;
                }
                else
                {
                    if (first)
                    {
                        e.Node.BeginEdit();
                        e.Node.Text = string.Empty;
                        if (e.Node.IsSelected)
                        {
                            e.Node.TreeView.SelectedNode = null;
                        }
                        first = false;
                    }
                    else
                    {
                        if (e.Node.Level == 0)
                        {
                            e.Node.Text = categorieAModifier;
                        }
                        else if (e.Node.Level == 1)
                        {
                            if (btnSettingsClick == Constantes.addTopic)
                            {
                                if (e.Node.TreeView.SelectedNode == null)
                                {
                                    e.Node.TreeView.Nodes.Remove(e.Node);
                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                e.Node.Text = topicAModifier;
                            }
                        }
                    }
                }
            }
            else
            {
                e.Node.EndEdit(true);
                cancelLabelEdit = false;
                if (btnSettingsClick == Constantes.addCategory || btnSettingsClick == Constantes.addTopic)
                {
                    Tree.cancelNodeCreation(e, btnSettingsClick);
                }
            }
        }

        /*
         * Se produit avant l'édition d'un noeud du treeView
         */
        private void treeView1_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (firstEdition)
            {
                if (e.Node.Level == 0)
                {
                    categorieAModifier = e.Node.Text;
                }
                else if (e.Node.Level == 1)
                {
                    topicAModifier = e.Node.Text;
                }
            }
        }

        /*
         * Se produit lorsqu'il faut repeindre un type d'utilisateur
         */
        // Paint - typeUser (checklist + points)
        private void typeUser_Paint(object sender, Graphics g, Color couleurFond, Brush couleurLettre, string lettre)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawEllipse(new Pen(Color.Black, 2), 1, 1, 20, 20);
            g.FillEllipse(new SolidBrush(couleurFond), 2, 2, 18, 18);
            g.DrawString(lettre, new Font("Calibri", 13, FontStyle.Bold), couleurLettre, 4, 0);
        }

        /*
         * Se produit après avoir appuyé sur une touche (quand on écrit dans un objet TextBox)
         */
        private void TBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                if (sender.GetType() == typeof(TextBox))
                {
                    if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
                    {
                        Display.displayPlaceholder((TextBox)sender);
                    }
                }
                else if (sender.GetType() == typeof(RichTextBox))
                {
                    if (string.IsNullOrWhiteSpace(((RichTextBox)sender).Text))
                    {
                        Display.displayPlaceholder((RichTextBox)sender);
                    }
                }
            }
            else if (e.KeyCode == Keys.Tab)
            {
                // le contrôle est un point
                if ((((Control)sender).Parent.Parent).GetType() == typeof(Panel))
                {
                    Panel panel = (Panel)(((Control)sender).Parent.Parent);
                    Edition.updateEdition(panel);
                    Edition.updateEditionCheck(titreChecklist, shortDesc, splitContainer1.Panel2);
                }
                // le contrôle est le titre ou la description de la checklist
                else if ((((Control)sender).Parent).GetType() == typeof(SplitterPanel))
                {
                    Edition.updateEditionCheck(titreChecklist, shortDesc, splitContainer1.Panel2);
                }
            }
        }

        /*
         * Se produit quand on appuie sur une touche, avant que la touche soit prise en compte
         */
        private void TBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (sender.GetType() == typeof(TextBox))
            {
                TextBox tb = (TextBox)sender;
                if (tb.ForeColor == Color.LightGray)
                {
                    tb.Text = string.Empty;
                    if (tb.Name == Constantes.textBoxTitreChecklist)
                    {
                        tb.ForeColor = Constantes.blue;
                    }
                    else
                    {
                        tb.ForeColor = Color.Black;
                    }
                }
            }
            else if (sender.GetType() == typeof(RichTextBox))
            {
                RichTextBox tb = (RichTextBox)sender;
                if (tb.ForeColor == Color.LightGray)
                {
                    tb.Text = string.Empty;
                    if (tb.Name == Constantes.textBoxDescriptionChecklist)
                    {
                        tb.ForeColor = Constantes.deepBlue;
                    }
                    else
                    {
                        tb.ForeColor = Color.Black;
                    }
                }
            }
        }

        /*
         * Se produit lorsque le focus arrive sur cet objet
         */
        private void TBox_Enter(object sender, EventArgs e)
        {
            // Faire vérifications
            Control parent = ((TextBox)sender).Parent;
            if (sender.GetType() == typeof(TextBox) && (parent.GetType() == typeof(Panel) || parent.GetType() == typeof(SplitterPanel)))
            {
                TextBox tb = (TextBox)sender;
                if (tb.ReadOnly == false)
                {
                    Graphics gr = parent.CreateGraphics();
                    Pen p = new Pen(Constantes.pink);
                    gr.DrawLine(p, new Point(tb.Left, tb.Bottom + 2), new Point(tb.Right, tb.Bottom + 2));
                }
            }
        }

        /*
         * Se produit lorsque le focus quitte l'objet
         */
        private void TBox_Leave(object sender, EventArgs e)
        {
            // Faire vérifications
            Control parent = ((TextBox)sender).Parent;
            if (sender.GetType() == typeof(TextBox) && (parent.GetType() == typeof(Panel) || parent.GetType() == typeof(SplitterPanel)))
            {
                TextBox tb = (TextBox)sender;
                if (tb.ReadOnly == false)
                {
                    Graphics gr = parent.CreateGraphics();
                    Pen p = new Pen(Color.LightGray);
                    gr.DrawLine(p, new Point(tb.Left, tb.Bottom + 2), new Point(tb.Right, tb.Bottom + 2));
                    if (string.IsNullOrWhiteSpace(tb.Text))
                    {
                        Display.displayPlaceholder(tb);
                    }
                }
            }
        }

        private void TBox_TextChanged(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Parent != null)
            {
                Size size = TextRenderer.MeasureText(box.Text, box.Font);
                box.Width = size.Width;

                Graphics g = box.Parent.CreateGraphics();
                Pen p = new Pen(Constantes.pink);
                g.Clear(splitContainer1.Panel2.BackColor);
                g.DrawLine(p, new Point(box.Left, box.Bottom + 2), new Point(box.Right, box.Bottom + 2));

                TextBox box2 = new TextBox();
                Pen p2 = new Pen(Constantes.gray);
                foreach (Control control in box.Parent.Controls)
                {
                    if (control.GetType() == typeof(TextBox) && control != box)
                    {
                        box2 = (TextBox)control;
                    }
                }
                g.DrawLine(p2, new Point(box2.Left, box2.Bottom + 2), new Point(box2.Right, box2.Bottom + 2));
            }
        }

        /*
         * Se produit lorsque le focus arrive sur la description de la checklist
         */
        private void shortDesc_Enter(object sender, EventArgs e)
        {
            Control parent = ((RichTextBox)sender).Parent;
            if (sender.GetType() == typeof(RichTextBox) && (parent.GetType() == typeof(Panel) || parent.GetType() == typeof(SplitterPanel)))
            {
                if (shortDesc.ReadOnly == false)
                {
                    Graphics gr = parent.CreateGraphics();
                    Pen p = new Pen(Constantes.pink);
                    gr.DrawRectangle(p, shortDesc.Left - 1, shortDesc.Top - 1, shortDesc.Width + 2, shortDesc.Height + 2);
                }
            }
        }

        private void shortDesc_Leave(object sender, EventArgs e)
        {
            // Faire vérifications
            Control parent = ((RichTextBox)sender).Parent;
            if (sender.GetType() == typeof(RichTextBox) && (parent.GetType() == typeof(Panel) || parent.GetType() == typeof(SplitterPanel)))
            {
                if (shortDesc.ReadOnly == false)
                {
                    Graphics gr = parent.CreateGraphics();
                    Pen p = new Pen(Color.LightGray);
                    gr.DrawRectangle(p, shortDesc.Left - 1, shortDesc.Top - 1, shortDesc.Width + 2, shortDesc.Height + 2);
                    if (string.IsNullOrWhiteSpace(shortDesc.Text))
                    {
                        Display.displayPlaceholder(shortDesc);
                    }
                }
            }
        }

        /*
         * Validation des modifications de la checklist et de ses points
         */
        private void validate_Click(object sender, EventArgs e)
        {
            panelPointsVisible();
            btnEdit.Visible = false;
            // btnPrint.Enabled = false;
            btnExport.Enabled = false;

            MySqlDataAdapter ad = new MySqlDataAdapter();
            string cmd = string.Empty;
            int idChecklist = 0;

            int idTopic = 0;

            if (creationChecklist)
            {
                Filters.restoreFilters(typesUser);
                // voir pour faire une fonction pour chaque partie de validate_Click()
                // comme il y a des return, faire return true / false dans la fonction comme pour UpdateModifChecklist()
                if (nodeTopic.Text == String.Empty)
                {
                    MessageBox.Show("Vous n'avez pas saisi le topic dans lequel sera créée la checklist.");
                    return;
                }
                // MessageBox
                // enregistrer la checklist avant de créer le premier point 
                idTopic = Tools.returnIDTopic(ad, dsResearch, nodeTopic);

                if (titreChecklist.TextLength == 0 || Tools.isPlaceHolder(titreChecklist))
                {
                    if (Tools.emptyTitle(Constantes.checklist) == false)
                    {
                        return;
                    }
                }
                else
                {
                    if (DB.verifyDuplicateChecklist(ad, idTopic, titreChecklist.Text) == false)
                    {
                        return;
                    }
                }
                AjoutElement.addNewChecklist(ad, cmd, idTopic, titreChecklist, shortDesc, mysql);
                idChecklist = (int)(ad.InsertCommand.LastInsertedId);
                check = new ChecklistRepo(idChecklist, titreChecklist.Text, shortDesc.Text, DateTime.Today, string.Empty, idTopic);
                creationChecklist = false;
            }
            else // Vérifier si une modif a été faite, sinon annuler
            {
                idTopic = check.IDTopic;
                if (Edition.updateModifChecklist(ad, cmd, idTopic, check, titreChecklist, shortDesc, mysql) == false)
                {
                    return;
                }
            }
            string IDPoint = string.Empty;
            DataSet data = new DataSet();
            DataTable points = new DataTable(Constantes.points);
            data.Tables.Add(points);
            data.Tables.Add(Constantes.types);
            infoChecklist.Location = new Point(Tools.changeLabelSize(titreChecklist) + titreChecklist.Location.X + 8, infoChecklist.Location.Y);

            if (DB.browsePanelPointsModif(ad, data, points, cmd, IDPoint, mysql, panelPoints, check, typesUser) == false)
            {
                // Remettre les placeholder qui sont supprimés dans la fonction parcourirPanelPointsModif()
                foreach (Control obj in panelPoints.Controls)
                {
                    if (obj.GetType() == typeof(Panel) && obj.Name.StartsWith(Constantes.panelPoint) && obj.Name != Tools.namePanelPointTemp())
                    {
                        foreach (Control control in obj.Controls)
                        {
                            if (control.GetType() == typeof(TextBox) && string.IsNullOrEmpty(control.Text))
                            {
                                Display.displayPlaceholder((TextBox)control);
                            }
                        }
                    }
                }
                return;
            }
            // pas besoin de la constante nouveauPoint
            if (newPoint)
            {
                // chercher control qui s'appelle panelPoint-tmp et verifier après qu'il change bien de nom une fois que l'on connait l'ID
                int idControl = 0;
                // modifier aussi le panelPoint-tmp
                while (idControl < panelPoints.Controls.Count && panelPoints.Controls[idControl].Name != Tools.namePanelPointTemp())
                {
                    idControl++;
                }
                if (idControl < panelPoints.Controls.Count)
                {
                    if (panelPoints.Controls[idControl].Name == Tools.namePanelPointTemp())
                    {
                        string title = string.Empty;
                        string descPoint = string.Empty;
                        Control infoPoint = new Control();
                        foreach (Control control in panelPoints.Controls[idControl].Controls)
                        {
                            if (control.Name == Constantes.textBoxTitrePoint)
                            {
                                if (control.Text.Length == 0 || Tools.isPlaceHolder((TextBox)control))
                                {
                                    if (Tools.emptyTitle(Constantes.point) == false)
                                    {
                                        return;
                                    }
                                }
                                else
                                {
                                    // Vérifier qu'il n'y ait pas de doublons
                                    if (DB.verifyDuplicatePoint(ad, check.IDChecklist, control.Text) == false)
                                    {
                                        return;
                                    }
                                    title = control.Text;
                                }
                            }
                            else if (control.Name == Constantes.textBoxDescriptionPoint)
                            {
                                if (Tools.isPlaceHolder((TextBox)control))
                                {
                                    descPoint = string.Empty;
                                }
                                else
                                {
                                    descPoint = control.Text;
                                }
                            }
                            else if (control.Name == Constantes.infoPoint)
                            {
                                if (control.Visible == false)
                                {
                                    infoPoint = control;
                                    // control.Visible = true;
                                }
                            }
                        }
                        if (infoPoint.Visible == false)
                        {
                            infoPoint.Visible = true;
                        }
                        Request.insertRequest(ad, SQL.insertPoint(title, descPoint, punkt.dateCreation.ToString(Constantes.formatDate2), check.IDChecklist));
                        int IDPoint2 = (int)ad.InsertCommand.LastInsertedId;
                        DB.insertTypesPoints(ad, IDPoint2, typesUser);
                        newPoint = false;
                        Tools.changeTemporaryName((Panel)panelPoints.Controls[idControl], IDPoint2);
                        // punkt.panelPoint.Dispose();
                        punkt = null;
                    }
                }
            }
            if (pointsSupprimes.Count > 0)
            {
                foreach (int point in pointsSupprimes)
                {
                    Edition.supprimerUnPoint(ad, point, panelPoints);
                }
            }
            // Voir pour afficher seulement ceux qui étaient affichés avant et pas tous les points       
            Tools.resetDataTable(data, Constantes.points);
            nonEditableChecklist();

            Request.useSelectRequestTable(ad, data.Tables[Constantes.points], SQL.selectPointsByIDChecklist("*", check.IDChecklist.ToString()));

            Tree.refreshTreeview(treeView1, dataSetTreeView, tagBitmap);
            Tree.expandNodes(check.IDChecklist, treeView1, nodeCategory, nodeTopic, nodeChecklist);
            activateFiltersClick();
            typesUser.Tables[Constantes.typesPoints].Rows.Clear();
            foreach (Control control in panelPoints.Controls)
            {
                if (control.GetType() == typeof(Panel) && control.Name.StartsWith(Constantes.panelPoint))
                {
                    DataTable table = Request.useRequestSelectPointByIDPoint(ad, Tools.returnID(control.Name));
                    DataSet dataset = new DataSet();
                    dataset.Tables.Add(table);
                    control.Invalidate();
                    control.Controls.Clear();
                    refreshOnePoint(dataset, ad, (Panel)control, Tools.returnID(control.Name),
                        table.Rows[0].ItemArray[1].ToString(), table.Rows[0].ItemArray[2].ToString());
                }
            }

            // Vérifier ça
            if (check != null || check.IDChecklist != 0)
            {
                idChecklist = check.IDChecklist;
            }
            if (idChecklist == 0 && panelPoints.Controls.Count != 0)
            {
                int i = 0;
                while (panelPoints.Controls[i] != null && panelPoints.Controls[i].Name.StartsWith(Constantes.panelPoint) == false)
                {
                    i++;
                }
                if (panelPoints.Controls[i] != null)
                {
                    DataTable tableChecklist = new DataTable();
                    Request.useSelectRequestTable(ad, tableChecklist, SQL.selectPointByIDPoint("IDChecklist", Tools.returnID(panelPoints.Controls[i].Name)));
                    idChecklist = Int32.Parse(tableChecklist.Rows[0][0].ToString());
                }
            }
            if (idChecklist > 0)
            {
                DataTable checklist = new DataTable();
                Request.useSelectRequestTable(ad, checklist, SQL.selectChecklistByID("*", idChecklist.ToString()));
                Display.infosChecklist(idChecklist.ToString(), checklist.Rows[0], infoChecklist, toolTip1, titreChecklist, shortDesc,
                mysql, favoris, btnEdit, btnCancel, btnDelete, btnExport, btnAddPoint);
            }
        }

        // clic refresh
        // refresh aussi la fenêtre
        private void refresh_Click(object sender, EventArgs e)
        {
            Tree.refreshTreeview(treeView1, dataSetTreeView, tagBitmap);
        }

        private void splitContainer2_Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.Y > treeView1.Location.Y && treeView1.Visible)
                {
                    Tree.InitialiseContextMenuTreeview(Constantes.autre, contextMenuStrip1);
                    contextMenuStrip1.Show(splitContainer1, e.X, e.Y);
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            Control control = contextMenuStrip1.SourceControl;
        }

        /*
         * Se produit lorsque l'on clique sur un élément du menu contextuel associé à un noeud du treeView
         */
        private void ContextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            TreeNode select = treeView1.SelectedNode;
            TreeNode nouveau = new TreeNode(string.Empty);
            treeView1.LabelEdit = true;
            firstEdition = true;
            if (e.ClickedItem.Text == Constantes.modifCategorie)
            {
                modifCategory();
                btnSettingsClick = Constantes.changeCategory;
                refreshHomePage();
            }
            else if (e.ClickedItem.Text == Constantes.supprCategorie)
            {
                DialogResult result = MessageBox.Show("Wollen Sie wirklich diese Kategorie löschen ?",
                    "Kategorie löschen",
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    supprimerCategorie(select);
                    Tree.refreshTreeview(treeView1, dataSetTreeView, tagBitmap);
                    refreshHomePage();
                }
            }
            else if (e.ClickedItem.Text == Constantes.ajoutTopic)
            {
                btnSettingsClick = Constantes.addTopic;
                select.Nodes.Add(nouveau);
                if (select.IsExpanded == false)
                {
                    select.Expand();
                }
                first = true;
                nouveau.BeginEdit();
            }
            else if (e.ClickedItem.Text == Constantes.modifTopic)
            {
                modifTopic();
                btnSettingsClick = Constantes.changeTopic;
                refreshHomePage();
            }
            else if (e.ClickedItem.Text == Constantes.supprTopic)
            {
                DialogResult result = MessageBox.Show("Wollen Sie wirklich diese Topic löschen ?",
                     "Topic löschen",
                     MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    deleteTopic(select);
                    Tree.refreshTreeview(treeView1, dataSetTreeView, tagBitmap);
                    refreshHomePage();
                }
            }
            else if (e.ClickedItem.Text == Constantes.ajoutChecklist)
            {
                btnSettingsClick = Constantes.addChecklist;
                addChecklist(Constantes.addChecklist);
            }
            else if (e.ClickedItem.Text == Constantes.ajoutCategorie)
            {
                btnSettingsClick = Constantes.addCategory;
                treeView1.Nodes.Add(nouveau);
                first = true;
                nouveau.BeginEdit();
            }
        }

        // paint filtre E
        private void pEntw_Paint(object sender, PaintEventArgs e)
        {
            if ((bool)typesUser.Tables[Constantes.filtres].Rows[0][1] == true)
            {
                typeUser_Paint(sender, e.Graphics, Constantes.turquoise, Brushes.White, Constantes.lettreEntwickler);
            }
            else
            {
                typeUser_Paint(sender, e.Graphics, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreEntwickler);
            }
        }

        // paint filtre B
        private void pBerater_Paint(object sender, PaintEventArgs e)
        {
            if ((bool)typesUser.Tables[Constantes.filtres].Rows[1][1])
            {
                typeUser_Paint(sender, e.Graphics, Constantes.pink, Brushes.White, Constantes.lettreBerater);
            }
            else
            {
                typeUser_Paint(sender, e.Graphics, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreBerater);
            }
        }

        // paint filtre T
        private void pTechnik_Paint(object sender, PaintEventArgs e)
        {
            if ((bool)typesUser.Tables[Constantes.filtres].Rows[2][1])
            {
                typeUser_Paint(sender, e.Graphics, Constantes.orange, Brushes.White, Constantes.lettreTechnik);
            }
            else
            {
                typeUser_Paint(sender, e.Graphics, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreTechnik);
            }
        }

        // paint filtre K
        private void pKunde_Paint(object sender, PaintEventArgs e)
        {
            if ((bool)typesUser.Tables[Constantes.filtres].Rows[3][1])
            {
                typeUser_Paint(sender, e.Graphics, Constantes.deepBlue, Brushes.White, Constantes.lettreKunde);
            }
            else
            {
                typeUser_Paint(sender, e.Graphics, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreKunde);
            }
        }

        // clic filtre E
        private void pEntw_Click(object sender, EventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            Graphics g = box.CreateGraphics();
            if ((bool)typesUser.Tables[Constantes.filtres].Rows[0][1] == true)
            {
                typeUser_Paint(sender, g, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreEntwickler);
                typesUser.Tables[Constantes.filtres].Rows[0][1] = false;
            }
            else
            {
                typeUser_Paint(sender, g, Constantes.turquoise, Brushes.White, Constantes.lettreEntwickler);
                typesUser.Tables[Constantes.filtres].Rows[0][1] = true;
            }
            filterPoints();
        }

        // clic filtre B
        private void pBerater_Click(object sender, EventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            Graphics g = box.CreateGraphics();

            if ((bool)typesUser.Tables[Constantes.filtres].Rows[1][1])
            {
                typeUser_Paint(sender, g, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreBerater);
                typesUser.Tables[Constantes.filtres].Rows[1][1] = false;
            }
            else
            {
                typeUser_Paint(sender, g, Constantes.pink, Brushes.White, Constantes.lettreBerater);
                typesUser.Tables[Constantes.filtres].Rows[1][1] = true;
            }
            filterPoints();
        }

        // clic filtre T
        private void pTechnik_Click(object sender, EventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            Graphics g = box.CreateGraphics();
            if ((bool)typesUser.Tables[Constantes.filtres].Rows[2][1])
            {
                typeUser_Paint(sender, g, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreTechnik);
                typesUser.Tables[Constantes.filtres].Rows[2][1] = false;
            }
            else
            {
                typeUser_Paint(sender, g, Constantes.orange, Brushes.White, Constantes.lettreTechnik);
                typesUser.Tables[Constantes.filtres].Rows[2][1] = true;
            }
            filterPoints();
        }

        // clic filtre K
        private void pKunde_Click(object sender, EventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            Graphics g = box.CreateGraphics();
            if ((bool)typesUser.Tables[Constantes.filtres].Rows[3][1])
            {
                typeUser_Paint(sender, g, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreKunde);
                typesUser.Tables[Constantes.filtres].Rows[3][1] = false;
            }
            else
            {
                typeUser_Paint(sender, g, Constantes.deepBlue, Brushes.White, Constantes.lettreKunde);
                typesUser.Tables[Constantes.filtres].Rows[3][1] = true;
            }
            filterPoints();
        }

        // Différentes possibilités de remplissage des types de user du point

        private void typeEntwickler_Paint(object sender, PaintEventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            DataRow nouveau = typesUser.Tables[Constantes.typesPoints].Rows.Find(Tools.returnID(box.Name));
            if ((bool)nouveau[Constantes.entwickler])
            {
                typeUser_Paint(sender, e.Graphics, Constantes.turquoise, Brushes.White, Constantes.lettreEntwickler);
            }
            else
            {
                typeUser_Paint(sender, e.Graphics, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreEntwickler);
            }
        }

        private void typeBerater_Paint(object sender, PaintEventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            DataRow nouveau = typesUser.Tables[Constantes.typesPoints].Rows.Find(Tools.returnID(box.Name));
            if ((bool)nouveau[Constantes.berater])
            {
                typeUser_Paint(sender, e.Graphics, Constantes.pink, Brushes.White, Constantes.lettreBerater);
            }
            else
            {
                typeUser_Paint(sender, e.Graphics, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreBerater);
            }
        }

        private void typeTechnik_Paint(object sender, PaintEventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            DataRow nouveau = typesUser.Tables[Constantes.typesPoints].Rows.Find(Tools.returnID(box.Name));
            if ((bool)nouveau[Constantes.technik])
            {
                typeUser_Paint(sender, e.Graphics, Constantes.orange, Brushes.White, Constantes.lettreTechnik);
            }
            else
            {
                typeUser_Paint(sender, e.Graphics, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreTechnik);
            }
        }

        private void typeKunde_Paint(object sender, PaintEventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            DataRow nouveau = typesUser.Tables[Constantes.typesPoints].Rows.Find(Tools.returnID(box.Name));
            if ((bool)nouveau[Constantes.kunde])
            {
                typeUser_Paint(sender, e.Graphics, Constantes.deepBlue, Brushes.White, Constantes.lettreKunde);
            }
            else
            {
                typeUser_Paint(sender, e.Graphics, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreKunde);
            }
        }

        // clic type user E pour modif ou creation point
        private void pointEntw_Click(object sender, EventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            Graphics g = box.CreateGraphics();
            DataRow nouveau = typesUser.Tables[Constantes.typesPoints].Rows.Find(Tools.returnID(box.Name));
            if ((bool)nouveau[Constantes.entwickler])
            {
                typeUser_Paint(sender, g, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreEntwickler);
                nouveau[Constantes.entwickler] = false;
            }
            else
            {
                typeUser_Paint(sender, g, Constantes.turquoise, Brushes.White, Constantes.lettreEntwickler);
                nouveau[Constantes.entwickler] = true;
            }
        }

        // clic type user B pour modif ou creation point
        private void pointBerater_Click(object sender, EventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            Graphics g = box.CreateGraphics();
            DataRow nouveau = typesUser.Tables[Constantes.typesPoints].Rows.Find(Tools.returnID(box.Name));
            if ((bool)nouveau[Constantes.berater])
            {
                typeUser_Paint(sender, g, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreBerater);
                nouveau[Constantes.berater] = false;
            }
            else
            {
                typeUser_Paint(sender, g, Constantes.pink, Brushes.White, Constantes.lettreBerater);
                nouveau[Constantes.berater] = true;
            }
        }

        // clic type user T pour modif ou creation point
        private void pointTechnik_Click(object sender, EventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            Graphics g = box.CreateGraphics();
            DataRow nouveau = typesUser.Tables[Constantes.typesPoints].Rows.Find(Tools.returnID(box.Name));
            if ((bool)nouveau[Constantes.technik])
            {
                typeUser_Paint(sender, g, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreTechnik);
                nouveau[Constantes.technik] = false;
            }
            else
            {
                typeUser_Paint(sender, g, Constantes.orange, Brushes.White, Constantes.lettreTechnik);
                nouveau[Constantes.technik] = true;
            }
        }

        // clic type user K pour modif ou creation point
        private void pointKunde_Click(object sender, EventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            Graphics g = box.CreateGraphics();
            DataRow nouveau = typesUser.Tables[Constantes.typesPoints].Rows.Find(Tools.returnID(box.Name));
            if ((bool)nouveau[Constantes.kunde])
            {
                typeUser_Paint(sender, g, Color.White, new SolidBrush(Constantes.gray), Constantes.lettreKunde);
                nouveau[Constantes.kunde] = false;
            }
            else
            {
                typeUser_Paint(sender, g, Constantes.deepBlue, Brushes.White, Constantes.lettreKunde);
                nouveau[Constantes.kunde] = true;
            }
        }

        /*
         * Se produit lorsque l'on clique sur le bouton permettant d'ajouter un nouveau point
         */
        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            // panelPointsVisible();
            // s'il n'a pas encore de points
            // btnPrint.Enabled = false;
            btnExport.Enabled = false;
            int points = 0;
            foreach (Control control in panelPoints.Controls)
            {
                if (control.GetType() == typeof(Panel) && control.Name.StartsWith(Constantes.panelPoint))
                {
                    points += 1;
                }
            }
            if (points == 0)
            {
                displayTypesUser();
            }
            PictureBox pbEntwickler = AjoutElement.addPBEntwickler(Constantes.temp);
            PictureBox pbBerater = AjoutElement.addPBBerater(Constantes.temp);
            PictureBox pbTechnik = AjoutElement.addPBTechnik(Constantes.temp);
            PictureBox pbKunde = AjoutElement.addPBKunde(Constantes.temp);

            typesUser.Tables[Constantes.typesPoints].Rows.Add(Constantes.temp, false, false, false, false);

            pbEntwickler.Click += new EventHandler(this.pointEntw_Click);
            pbBerater.Click += new EventHandler(this.pointBerater_Click);
            pbTechnik.Click += new EventHandler(this.pointTechnik_Click);
            pbKunde.Click += new EventHandler(this.pointKunde_Click);

            DataTable table = new DataTable();
            table.Columns.Add();
            table.PrimaryKey = new DataColumn[] { table.Columns[0] };
            initTypes(table, pbEntwickler, pbBerater, pbTechnik, pbKunde,
                typesUser.Tables[Constantes.typesPoints].Rows[typesUser.Tables[Constantes.typesPoints].Rows.Count - 1]);

            PictureBox pbCheck = AjoutElement.addPBCheck();
            pbCheck.Click += new EventHandler(this.check_Click);

            // voir pour faire une fonction ici
            TextBox tTitle = AjoutElement.addTextBoxTitlePoint();
            TextBox tDescription = AjoutElement.addTextBoxDescriptionPoint(tTitle.TabIndex + 1);

            PictureBox pbInfo = AjoutElement.addPBInfo(tTitle.Width);
            pbInfo.Visible = false;

            addEventsTBox(tTitle);
            addEventsTBox(tDescription);

            Display.testDisplayPlaceholder(tTitle, tDescription);

            Tools.changeLabelSize(tTitle);
            Tools.changeLabelSize(tDescription);

            List<PictureBox> listPB = new List<PictureBox>();
            listPB.Add(pbKunde);
            listPB.Add(pbTechnik);
            listPB.Add(pbCheck);
            listPB.Add(pbBerater);
            listPB.Add(pbInfo);
            listPB.Add(pbEntwickler);

            Panel panelPoint = new Panel();

            panelPoints.Controls.Add(panelPoint);
            panelPoints.AutoScroll = true;
            panelPoint.SuspendLayout();

            int locY = 0;
            if (Tools.firstPoint(panelPoints))
            {
                locY = Constantes.locationYPremierPoint;
            }
            else
            {
                locY = Tools.locationYNewPoint(panelPoints, Constantes.panelPoint);
            }
            Display.activateScrollBar(panelPoints);

            AjoutElement.addPanelPoint(panelPoint, tTitle, tDescription, locY, listPB);

            panelPoints.ScrollControlIntoView(panelPoint);
            panelPoints.Refresh();

            Point pt1Title = new Point(tTitle.Left, tTitle.Bottom + 2);
            Point pt2Title = new Point(tTitle.Right, tTitle.Bottom + 2);
            Point pt1Desc = new Point(tDescription.Left, tDescription.Bottom + 2);
            Point pt2Desc = new Point(tDescription.Right, tDescription.Bottom + 2);

            Graphics g = panelPoint.CreateGraphics();
            Pen p = new Pen(Color.LightGray);

            g.DrawLine(p, pt1Title, pt2Title);
            g.DrawLine(p, pt1Desc, pt2Desc);

            newPoint = true;

            btnAddPoint.Visible = false;
            validate.Visible = true;
            btnEdit.Visible = false;
            btnCancel.Visible = true;

            punkt = new PointChecklist(panelPoint, tTitle.Text, tDescription.Text, DateTime.Today);
            disableFiltersClick();
            panelPoint.Focus();
            tTitle.Focus();
        }

        /*
         * Permet de supprimer la checklist
         */
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Wollen Sie diese checklist löschen ?", "Löschen", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // voir pour faire des fonctions ici ou utiliser celles de la suppression d'un topic et d'une catégorie
                MySqlDataAdapter ad = new MySqlDataAdapter();
                DataSet dataset = new DataSet();

                Request.deleteRequest(ad, "DELETE FROM type_point WHERE IDPoint1 IN (" + SQL.selectPointsByIDChecklist("IDPoint", check.IDChecklist.ToString()) + ")");
                // Supprimer modif_point
                Request.deleteRequest(ad, "DELETE FROM modif_point WHERE IDPoint IN (" + SQL.selectPointsByIDChecklist("IDPoint", check.IDChecklist.ToString()) + ")");

                Request.deletePoints(ad, check.IDChecklist);
                Request.deleteFavorites(ad, check.IDChecklist, "IDChecklist1");

                // mise à jour des favoris
                Favorites.deleteFromFavoriteList(check.IDChecklist, listFavorites);

                // Supprimer modif_checklist
                Request.deleteModifChecklist(ad, check.IDChecklist);
                Request.deleteOneChecklist(ad, check.IDChecklist);
                check = null;

                DB.verificationModifsToDelete();
                Tree.refreshTreeview(treeView1, dataSetTreeView, tagBitmap);
                InitialiseHomepage();
            }
        }

        /*
         * Se produit lorsque l'on clique sur le bouton de suppression d'un point
         */
        private void btnDeletePoint_Click(object sender, EventArgs e)
        {
            panelPointsVisible(); // pas utile de mettre ça, il est forcément visible
            PictureBox pBox = (PictureBox)sender;
            string IDPanel = Tools.returnID(pBox.Parent.Name);
            if (pBox.Parent.Parent.GetType() == typeof(Panel) && pBox.Parent.Parent.Name == "panelPoints")
            {
                Panel panelPoints = (Panel)pBox.Parent.Parent;
                bool panelSupprime = false;
                foreach (Control control in panelPoints.Controls)
                {
                    if (panelSupprime) //&& control.Visible)
                    {
                        control.Location = new Point(control.Location.X, control.Location.Y - Constantes.distanceEntreDeuxPoints);
                    }
                    if (control == pBox.Parent)
                    {
                        panelSupprime = true;
                        control.Visible = false;
                        pointsSupprimes.Add(Int32.Parse(IDPanel));
                    }
                }
            }
            Edition.updateEdition(panelPoints);
            panelPoints.Size = new Size(panelPoints.Size.Width + 1, panelPoints.Size.Height + 1);
            panelPoints.Size = new Size(panelPoints.Size.Width - 1, panelPoints.Size.Height - 1);
            // miseAJourEditionCheck();
        }

        /*
         * Se produit lorsque l'on va éditer une checklist
         * Permet l'édition de la checklist et de ses points
         */
        private void btnEdit_Click(object sender, EventArgs e)
        {
            panelPointsVisible();
            // btnPrint.Enabled = false;
            btnExport.Enabled = false;
            pointsSupprimes.Clear();
            editChecklist();
            editPoints();
            disableFiltersClick();
        }

        /*
         * Se produit lors du clic sur le bouton d'annulation de modification d'une checklist
         */
        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancel();
        }

        /*
         * Se produit lorsque l'on clique sur le bouton permettant d'exporter une checklist et ses points
         */
        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportWord.testFichierWord(titreChecklist.Text, shortDesc.Text, panelPoints, typesUser.Tables[Constantes.typesPoints]);
        }

        private void splitContainer1_Panel2_Click(object sender, EventArgs e)
        {
            // panelPointsVisible();
            panelPoints.Focus();
        }

        /*
         * Permet d'afficher la liste des favoris
         */
        // tests : IDUtilisateur = 01
        private void favoris_Click(object sender, EventArgs e)
        {
            bool isFavori = Favorites.changerCouleurFavori(favoris);
            MySqlDataAdapter ad = new MySqlDataAdapter();
            DataTable table = new DataTable();
            Request.useSelectRequestTable(ad, table, SQL.selectTopicByIDTopic("IDCategorie", check.IDTopic.ToString()));
            if (isFavori)
            {
                string cmd = "INSERT INTO favoris (IDCategorie1, IDTopic1, IDChecklist1, IDUtilisateur) VALUES (" + table.Rows[0][0] + ","
                    + check.IDTopic + "," + check.IDChecklist + "," + 01 + ")";
                ad.InsertCommand = new MySqlCommand(cmd, mysql);
                ad.InsertCommand.ExecuteNonQuery();

                ListViewItem item = new ListViewItem();
                long IDFavoris = ad.InsertCommand.LastInsertedId;
                DataTable favori = new DataTable();
                Request.useSelectRequestTable(ad, favori, SQL.selectChecklistByID("IDChecklist, Titre", "(" + SQL.selectFavorisByID(IDFavoris) + ")"));

                item.Text = favori.Rows[0][1].ToString();
                item.Name = Constantes.listViewItem + favori.Rows[0][0].ToString();
                listFavorites.Items.Add(item);
            }
            else
            {
                Request.deleteRequest(ad, SQL.deleteFavoris("IDChecklist1", check.IDChecklist));

                foreach (ListViewItem item in listFavorites.Items)
                {
                    if (item.Name.EndsWith("-" + check.IDChecklist))
                    {
                        listFavorites.Items.Remove(item);
                    }
                }
            }
        }

        /*
         * Se produit lorsque l'on clique sur le bouton permettant d'afficher la liste des favoris
         */
        // tests : IDUtilisateur = 01 
        private void btnFavoris_Click(object sender, EventArgs e)
        {
            if (treeView1.Visible)
            {
                Favorites.displayFavorites(treeView1, listFavorites, labelFavorites, whiteHeart);
                Favorites.favoriRose(btnFavoris);
            }
            else
            {
                Display.displayMenu(treeView1, listFavorites, labelFavorites, whiteHeart);
                Favorites.favoriBleuClair(btnFavoris);
            }
        }

        /*
         * Affiche la checklist sélectionnée dans la liste des favoris
         */
        private void ListeFavoris_Click(object sender, EventArgs e)
        {
            if (btnEdit.Visible == false && btnCancel.Visible == true)
            {
                DialogResult result = Display.cancelEdition(creationChecklist);
                if (result == DialogResult.No)
                {
                    return;
                }
                else
                {
                    cancel();
                }
            }
            ListView liste = (ListView)sender;
            ListViewItem item = liste.SelectedItems[0];
            string IDChecklist = Tools.returnID(item.Name);

            MySqlDataAdapter adapt = new MySqlDataAdapter();
            DataSet data = new DataSet();
            data.Tables.Add(Constantes.checklist);

            Request.useSelectRequestTable(adapt, data.Tables[Constantes.checklist], SQL.selectChecklistByID("*", IDChecklist));
            displayChecklist(adapt, data);
        }

        // Pour cocher ou décocher un bouton

        private void check_Click(object sender, EventArgs e)
        {
            PictureBox click = (PictureBox)sender;
            click.Image = Resources.check21;
            click.Click += new EventHandler(this.notCheck_Click);
        }

        private void notCheck_Click(object sender, EventArgs e)
        {
            PictureBox click = (PictureBox)sender;
            click.Image = Resources.circle;
            click.Click += new EventHandler(this.check_Click);
        }

        /*
         * Se produit lorsque la taille du panel contenant les points change
         * Active la barre de défilement
         */
        private void panelPoints_SizeChanged(object sender, EventArgs e)
        {
            Display.activateScrollBar(panelPoints);
        }

        /*
         * Se produit lorsque l'on bouge avec la barre de défilement dans le panel qui contient les points
         */
        private void panelPoints_Scroll(object sender, ScrollEventArgs e)
        {
            Edition.updateEdition((Panel)sender);
            Edition.updateEditionCheck(titreChecklist, shortDesc, splitContainer1.Panel2);
        }

        /*
         * Se produit lorsque le panel contenant les points est redimensionné
         */
        private void panelPoints_Resize(object sender, EventArgs e)
        {
            Edition.updateEdition((Panel)sender);
            Edition.updateEditionCheck(titreChecklist, shortDesc, splitContainer1.Panel2);
        }

        /*
         * Se produit lorsque le focus arrive sur le panel qui contient les points
         */
        private void panelPoints_Enter(object sender, EventArgs e)
        {
            panelPoints.Focus();
        }

        /*
         * Se produit lorsque l'on bouge la molette de la souris
         */
        private void panelPoints_MouseWheel(object sender, MouseEventArgs e)
        {
            Panel panel = (Panel)sender;
            panel.Update();

            Edition.updateEdition(panel);
            Edition.updateEditionCheck(titreChecklist, shortDesc, splitContainer1.Panel2);
        }

        /*
         * Modifie la couleur du bouton des favoris lorsque la souris passe sur le bouton
         */
        private void btnFavoris_MouseEnter(object sender, EventArgs e)
        {
            PictureBox btn = (PictureBox)sender;
            if (btn.BackColor == Constantes.pink)
            {
                btn.BackColor = Constantes.lightPink;
            }
            else if (btn.BackColor == Constantes.bleuIconesMenu)
            {
                btn.BackColor = Constantes.lightBlue;
            }
        }

        /*
         * Rend au bouton des favoris sa couleur initiale lorsque la souris quitte la zone du bouton
         */
        private void btnFavoris_MouseLeave(object sender, EventArgs e)
        {
            PictureBox btn = (PictureBox)sender;
            if (btn.BackColor == Constantes.lightPink)
            {
                btn.BackColor = Constantes.pink;
            }
            else if (btn.BackColor == Constantes.lightBlue)
            {
                btn.BackColor = Constantes.bleuIconesMenu;
            }
        }

        private void lHome_MouseEnter(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            if (label.ForeColor == Constantes.grayText)
            {
                label.ForeColor = Color.White;
            }
        }

        private void lHome_MouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            if (label.ForeColor == Color.White)
            {
                label.ForeColor = Constantes.grayText;
            }
        }

        private void home_Click(object sender, EventArgs e)
        {
            if (panelChecklists.Visible == false)
            {
                if (btnEdit.Visible == false && btnCancel.Visible == true)
                {
                    DialogResult result = Display.cancelEdition(creationChecklist);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }
                cancel();
                if (panelChecklists.Visible == false)
                {
                    InitialiseHomepage();
                }
                Tree.refreshTreeview(treeView1, dataSetTreeView, tagBitmap);
            }
        }

        /*
         * Modifie la couleur du bouton lorsque la souris passe sur le bouton
         * Pour montrer qu'il s'agit d'un bouton
         */
        private void btn_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            if (pb.BackColor == Constantes.deepBlue)
            {
                pb.BackColor = Constantes.bleuIconesMenu;
            }
            else if (pb.BackColor == Constantes.pink)
            {
                pb.BackColor = Constantes.lightPink;
            }
            else if (pb.BackColor == Constantes.bleuIconesMenu)
            {
                pb.BackColor = Constantes.lightBlue;
            }
            else if (pb.BackColor == Constantes.green)
            {
                pb.BackColor = Constantes.lightGreen;
            }
            else if (pb.BackColor == Constantes.grayText)
            {
                pb.BackColor = Color.White;
            }
        }

        /*
         * Rend au bouton sa couleur initiale lorsque la souris quitte la zone du bouton
         */
        private void btn_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            if (pb.BackColor == Constantes.bleuIconesMenu)
            {
                pb.BackColor = Constantes.deepBlue;
            }
            else if (pb.BackColor == Constantes.lightPink)
            {
                pb.BackColor = Constantes.pink;
            }
            else if (pb.BackColor == Constantes.lightBlue)
            {
                pb.BackColor = Constantes.bleuIconesMenu;
            }
            else if (pb.BackColor == Constantes.lightGreen)
            {
                pb.BackColor = Constantes.green;
            }
            else if (pb.BackColor == Color.White)
            {
                pb.BackColor = Constantes.grayText;
            }
        }

        private void PbImage_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            e.Graphics.DrawImage(Graphic.trimImage(Resources.testImage), 4, 4, 53, 53);
            Pen p = new Pen(Constantes.grayText, 3.5F);
            e.Graphics.DrawEllipse(p, 2, 2, pb.Width - 4, pb.Height - 4);

            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, pb.Width, pb.Height);
            gp.CloseFigure();

            Region rg = new Region(gp);
            pb.Region = rg;
        }

        private void titreChecklist_TextChanged(object sender, EventArgs e)
        {
            Size size = TextRenderer.MeasureText(titreChecklist.Text, titreChecklist.Font);
            titreChecklist.Width = size.Width;
            Edition.updateEditionCheck(titreChecklist, shortDesc, splitContainer1.Panel2);
        }

        private void splitContainer1_Panel2_Enter(object sender, EventArgs e)
        {
            panelPoints.Focus();
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

