using Checklist.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checklist
{
    public static class AjoutElement
    {
        public static PictureBox addInfoChecklist()
        {
            PictureBox pbInfo = new PictureBox();
            ((ISupportInitialize)(pbInfo)).BeginInit();
            pbInfo.Image = Properties.Resources.info;
            pbInfo.Location = new Point(600, 18);
            pbInfo.Name = "InfoPoint";
            pbInfo.Size = new Size(20, 20);
            pbInfo.SizeMode = PictureBoxSizeMode.Zoom;
            pbInfo.TabIndex = 30;
            pbInfo.TabStop = false;
            ((ISupportInitialize)(pbInfo)).EndInit();
            return pbInfo;
        }

        public static Label addLabelTopic(string topic, int x)
        {
            Label lTopic = new Label();
            lTopic.AutoSize = true;
            lTopic.Font = new Font("Segoe UI", 8.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lTopic.ForeColor = Constantes.blue;
            lTopic.Location = new Point(x, 4);
            lTopic.Name = "Topic";
            lTopic.Size = new Size(67, 23);
            lTopic.TabIndex = 28;
            lTopic.Text = topic;
            return lTopic;
        }

        public static Label addLabelCategorie(string categorie)
        {
            Label lCategorie = new Label();
            lCategorie.AutoSize = true;
            lCategorie.Font = new Font("Segoe UI", 8.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lCategorie.ForeColor = Constantes.blue;
            lCategorie.Location = new Point(10, 4);
            lCategorie.Name = "Categorie";
            lCategorie.Size = TextRenderer.MeasureText(categorie, lCategorie.Font);
            lCategorie.TabIndex = 28;
            lCategorie.Text = categorie;
            return lCategorie;
        }

        public static Label addLabelSeparator(int x)
        {
            Label lTopic = new Label();
            lTopic.AutoSize = true;
            lTopic.Font = new Font("Cambria", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lTopic.ForeColor = Constantes.blue;
            lTopic.Location = new Point(x, 5);
            lTopic.Name = "Separator";
            lTopic.TabIndex = 28;
            lTopic.Text = "→";
            lTopic.Size = TextRenderer.MeasureText(lTopic.Text, lTopic.Font);
            return lTopic;
        }

        public static void addControlsPanelChecklist(Panel panelChecklist, Label titreCheck, Label descCheck,/*PictureBox info,*/Label topic, Label categorie, Label separator)
        {
            panelChecklist.Controls.Add(titreCheck);
            panelChecklist.Controls.Add(descCheck);
            // panelChecklist.Controls.Add(info);
            panelChecklist.Controls.Add(topic);
            panelChecklist.Controls.Add(categorie);
            panelChecklist.Controls.Add(separator);
        }

        public static Label addLabelTitrePageAccueil()
        {
            Label labelChecklists = new Label();
            labelChecklists.Text = "Die 5 letzten Checkliste : ";
            labelChecklists.AutoSize = true;
            labelChecklists.Font = new Font("Segoe UI", 14.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelChecklists.ForeColor = Constantes.blue;
            labelChecklists.Location = new Point(2, 0);
            labelChecklists.Name = "labelChecklists";
            labelChecklists.Size = new Size(67, 23);
            labelChecklists.TabIndex = 28;
            return labelChecklists;
        }

        public static Label addLabelPageAccueil(string title, string name)
        {
            Label lLabel = new Label();
            lLabel.AutoSize = true;
            lLabel.Font = new Font("Segoe UI", 12.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lLabel.ForeColor = Constantes.blue;
            lLabel.Location = new Point(0, 0);
            lLabel.Name = name;
            lLabel.Size = new Size(67, 23);
            lLabel.TabIndex = 28;
            lLabel.Text = title;
            lLabel.AutoSize = false;
            lLabel.TextAlign = ContentAlignment.MiddleCenter;
            lLabel.Dock = DockStyle.Fill;
            return lLabel;
        }

        public static Label creationLabelFavoris()
        {
            Label labelFavoris = new Label();
            labelFavoris.AutoSize = true;
            labelFavoris.Font = new Font("Calibri", 14F);
            labelFavoris.ForeColor = Constantes.grayText;
            labelFavoris.Location = new Point(56, 176);
            labelFavoris.Name = "favoris";
            labelFavoris.Size = new Size(46, 23);
            labelFavoris.TabIndex = 1;
            labelFavoris.Text = "Favorites";
            return labelFavoris;
        }

        public static ListView creationListeFavoris()
        {
            ListView listeFavoris = new ListView();
            listeFavoris.Location = new Point(80, 210);
            listeFavoris.Size = new Size(220, 474);
            listeFavoris.Name = "listeFavoris";
            listeFavoris.TabIndex = 45;
            listeFavoris.View = View.List;
            listeFavoris.BackColor = Constantes.blue;
            listeFavoris.BorderStyle = BorderStyle.None;
            listeFavoris.ForeColor = Constantes.grayText;
            listeFavoris.Font = new System.Drawing.Font("Calibri", 13F);
            listeFavoris.MultiSelect = false;
            return listeFavoris;
        }

        public static Panel InitPanelChecklist(int locationY, int IDChecklist)
        {
            Panel panelChecklist = new Panel();
            panelChecklist.Location = new Point(38, locationY);
            panelChecklist.Name = Constantes.panelChecklist + IDChecklist;
            panelChecklist.Size = new Size(760, 74);
            panelChecklist.TabIndex = 30;
            panelChecklist.BorderStyle = BorderStyle.None;
            panelChecklist.BackColor = Constantes.grayCheck;
            panelChecklist.Cursor = Cursors.Hand;
            panelChecklist.Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            panelChecklist.MouseEnter += PanelChecklist_MouseEnter;
            panelChecklist.MouseLeave += PanelChecklist_MouseLeave;
            return panelChecklist;
        }

        private static void PanelChecklist_MouseEnter(object sender, EventArgs e)
        {
            Panel checklist = (Panel)sender;
            if (checklist.BackColor == Constantes.grayCheck)
            {
                checklist.BackColor = Constantes.lightGray;
            }
        }

        private static void PanelChecklist_MouseLeave(object sender, EventArgs e)
        {
            Panel checklist = (Panel)sender;
            if (checklist.BackColor == Constantes.lightGray)
            {
                checklist.BackColor = Constantes.grayCheck;
            }
        }

        // initialise le type d'user
        public static void addPBType(PictureBox pBox, Point location, string name)
        {
            ((ISupportInitialize)(pBox)).BeginInit();
            pBox.Location = location;
            pBox.Name = name;
            pBox.Size = new Size(24, 24);
            pBox.SizeMode = PictureBoxSizeMode.Zoom;
            pBox.Anchor = AnchorStyles.Right;
            pBox.TabStop = false;
            ((ISupportInitialize)(pBox)).EndInit();
        }

        // initialise le type E du point
        public static PictureBox addPBEntwickler(string IDPoint)
        {
            PictureBox pBox = new PictureBox();
            addPBType(pBox, new Point(567, 22), Tools.nameType(Constantes.pointEntwickler, IDPoint));
            return pBox;

        }

        // initialise le type B du point
        public static PictureBox addPBBerater(string IDPoint)
        {
            PictureBox pBox = new PictureBox();
            addPBType(pBox, new Point(596, 22), Tools.nameType(Constantes.pointBerater, IDPoint));
            return pBox;
        }

        // initialise le type T du point
        public static PictureBox addPBTechnik(string IDPoint)
        {
            PictureBox pBox = new PictureBox();
            addPBType(pBox, new Point(625, 22), Tools.nameType(Constantes.pointTechnik, IDPoint));
            return pBox;
        }

        // initialise le type K du point
        public static PictureBox addPBKunde(string IDPoint)
        {
            PictureBox pBox = new PictureBox();
            addPBType(pBox, new Point(654, 22), Tools.nameType(Constantes.pointKunde, IDPoint));
            return pBox;
        }

        // initialise un type d'user dans un point
        public static void initType(Panel panelPoints, PictureBox box, int locationX, string name)
        {
            box.Location = new Point(locationX, 7);
            box.Name = name;
            box.Size = new Size(23, 23);
            panelPoints.Controls.Add(box);
        }

        // initialise le label du panelPoints
        // n'utilise pas panelPoints
        public static Label initLabel()
        {
            Label sort = new Label();
            sort.AutoSize = true;
            sort.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            sort.ForeColor = Color.FromArgb(42, 63, 84);
            sort.Location = new Point(13, 12);
            sort.Name = "sort";
            sort.Size = new Size(137, 17);
            sort.TabIndex = 8;
            sort.Text = "Punkte sortiert bei :";
            return sort;
        }

        public static void initPanelPoint(Panel panelPoint, int locationY, int IDPoint)
        {
            panelPoint.Location = new Point(38, locationY);
            panelPoint.Name = Constantes.panelPoint + IDPoint;
            panelPoint.Size = new Size(760, 74);
            panelPoint.Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            panelPoint.TabIndex = 30;
        }

        // initialise le label du titre du point actuel
        public static Label addLabelTitre(string title, string name, int locationX, int locationY, int width, int height, Color color)
        {
            Label lTitre = new Label();
            lTitre.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lTitre.ForeColor = color;
            lTitre.Location = new Point(locationX, locationY);
            lTitre.Name = name;
            lTitre.Size = new Size(width, height);
            lTitre.TabIndex = 28;
            lTitre.Text = title;
            if (lTitre.Name == Constantes.labelTitreChecklist)
            {
                lTitre.Cursor = Cursors.Hand;
                lTitre.AutoSize = false;
                lTitre.TextAlign = ContentAlignment.MiddleLeft;
            }
            else
            {
                lTitre.AutoSize = true;
            }
            return lTitre;
        }

        // initialise le label de la description du point actuel
        public static Label addLabelDescription(string desc, string name, Point point, Size size, Color color)
        {
            Label lDescPoint = new Label();
            lDescPoint.Font = new Font("Segoe UI", 10F);
            lDescPoint.ForeColor = color;
            lDescPoint.Location = point;
            lDescPoint.Name = name;
            lDescPoint.Size = size;
            lDescPoint.TabIndex = 29;
            lDescPoint.Text = desc;
            lDescPoint.Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            if (lDescPoint.Name == Constantes.labelDescriptionChecklist)
            {
                lDescPoint.AutoSize = false;
                lDescPoint.TextAlign = ContentAlignment.MiddleLeft;
            }
            else
            {
                lDescPoint.AutoSize = true;
            }
            return lDescPoint;
        }

        public static TextBox addTextBoxTitlePoint()
        {
            TextBox tTitle = new TextBox();
            tTitle.Location = new Point(84, 18);
            tTitle.Name = Constantes.textBoxTitrePoint;
            tTitle.Size = new Size(150, 23);
            tTitle.BorderStyle = BorderStyle.None;
            tTitle.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tTitle.TabIndex = 100;
            return tTitle;
        }

        public static TextBox addTextBoxDescriptionPoint(int tabIndex)
        {
            TextBox tDescription = new TextBox();
            tDescription.Location = new Point(84, 45);
            tDescription.Name = Constantes.textBoxDescriptionPoint;
            tDescription.Size = new Size(250, 20);
            tDescription.BorderStyle = BorderStyle.None;
            tDescription.Font = new Font("Segoe UI", 11F);
            tDescription.TabIndex = tabIndex;
            return tDescription;
        }

        // initialise la bulle d'info du point actuel
        // rajouter l'info à afficher
        public static PictureBox addPBInfo(int width)
        {
            PictureBox pbInfo = new PictureBox();
            ((ISupportInitialize)(pbInfo)).BeginInit();
            pbInfo.Image = Properties.Resources.info;
            pbInfo.Location = new Point(width + 8, 18);
            pbInfo.Name = Constantes.infoPoint;
            pbInfo.Size = new Size(20, 20);
            pbInfo.SizeMode = PictureBoxSizeMode.Zoom;
            pbInfo.TabIndex = 30;
            pbInfo.TabStop = false;
            ((ISupportInitialize)(pbInfo)).EndInit();
            return pbInfo;
        }

        public static Panel grayPanel(string type, Panel panelChecklists, Label label)
        {
            Panel panel = new Panel();
            int x = 0;
            int taille = 170;
            if (type == Constantes.categorie)
            {
                x = 38;
            }
            else if (type == Constantes.topic)
            {
                x = 212;
            }
            else if (type == Constantes.checklist)
            {
                x = 38;
                taille = 220;
                // x = 386;
            }
            else if (type == Constantes.description)
            {
                //x = 560;
                x = 262;
                taille = 536; //238;
            }
            panel.Location = new Point(x, 40);
            panel.Name = "PanelGris";
            panel.Size = new Size(taille, 60);
            panel.TabIndex = 30;
            panel.BorderStyle = BorderStyle.None;
            panel.BackColor = Constantes.grayCheck;
            panelChecklists.Controls.Add(panel);
            panel.Controls.Add(label);
            return panel;
        }

        public static Panel bluePanel(int left, int right, int bottom, Panel panelChecklists)
        {
            Panel panel = new Panel();
            panel.Location = new Point(left, bottom);
            panel.Size = new Size(right - left, 6);
            panel.Name = "PanelBleu";
            panel.TabIndex = 30;
            panel.BorderStyle = BorderStyle.None;
            panel.BackColor = Constantes.blue;
            panel.Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            panelChecklists.Controls.Add(panel);
            return panel;
        }

        public static void addPanelPoint(Panel panelPoint, TextBox tTitle, TextBox tDescription, int locY, List<PictureBox> listPB)
        {
            foreach (PictureBox pb in listPB)
            {
                panelPoint.Controls.Add(pb);
            }
            panelPoint.Controls.Add(tDescription);
            panelPoint.Controls.Add(tTitle);
            panelPoint.Location = new Point(38, locY);
            panelPoint.Name = Tools.namePanelPointTemp();
            panelPoint.Size = new Size(760, 74);
            panelPoint.TabIndex = 30;
        }

        public static PictureBox addPBImage(Panel parent)
        {
            PictureBox pbImage = new PictureBox();
            parent.Controls.Add(pbImage);
            pbImage.Name = "Photo";
            pbImage.Location = new Point(27, 24);
            pbImage.Size = new Size(60, 60);
            pbImage.SizeMode = PictureBoxSizeMode.Zoom;
            return pbImage;
        }

        // initialise le check du point actuel
        public static PictureBox addPBCheck()
        {
            PictureBox pbCheck = new PictureBox();
            ((ISupportInitialize)(pbCheck)).BeginInit();
            pbCheck.Image = Resources.circle;
            pbCheck.Location = new Point(24, 14);
            pbCheck.Name = "check1";
            pbCheck.Size = new Size(42, 42);
            pbCheck.SizeMode = PictureBoxSizeMode.AutoSize;
            pbCheck.TabIndex = 31;
            pbCheck.TabStop = false;

            ((ISupportInitialize)(pbCheck)).EndInit();
            return pbCheck;
        }

        public static PictureBox addPBDelete(string IDPoint)
        {
            PictureBox pBox = new PictureBox();
            ((ISupportInitialize)(pBox)).BeginInit();
            pBox.Location = new Point(700, 21);
            pBox.Name = Constantes.deletePoint + IDPoint;
            pBox.Size = new Size(20, 24);
            pBox.Image = Resources.Trash_64px;
            pBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pBox.BackColor = Constantes.deepBlue;
            pBox.TabStop = false;
            ((ISupportInitialize)(pBox)).EndInit();
            return pBox;
        }

        /*
         * Ajoute les différents contrôles au Panel du point actuel
         */
        public static void addControlsPanelPoint(Panel panelPoint, List<PictureBox> listPB, Label lDescPoint, Label lTitle)
        {
            panelPoint.SuspendLayout();
            foreach (PictureBox pb in listPB)
            {
                panelPoint.Controls.Add(pb);
            }
            panelPoint.Controls.Add(lTitle);
            panelPoint.Controls.Add(lDescPoint);
        }


        /*
         * Ajoute une nouvelle checklist dans la base de données
         */
        public static void addNewChecklist(MySqlDataAdapter ad, string cmd, int idTopic, TextBox titreChecklist, RichTextBox shortDesc, MySqlConnection mysql)
        {
            // description : tester pour voir si le champ est rempli
            if (shortDesc.TextLength > 0 && Tools.isPlaceHolder(shortDesc) == false)
            {
                cmd += SQL.insertChecklist(titreChecklist.Text, shortDesc.Text, idTopic);
            }
            else
            {
                cmd += SQL.insertChecklistWithoutDescription(titreChecklist.Text, idTopic);
                shortDesc.Text = string.Empty;
            }
            // tester aussi si le titre est rempli et qu'il n'y a pas de doublons
            ad.InsertCommand = new MySqlCommand(cmd, mysql);
            ad.InsertCommand.ExecuteNonQuery();
        }
    }
}
