using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checklist
{
    /* Different methods to manage the display */
    public class Display
    {

        /*
         * Affiche le treeView sur la partie de gauche de l'écran
         */
        public static void displayMenu(TreeView treeView1, ListView listFavorites, Label labelFavorites, PictureBox whiteHeart)
        {
            treeView1.Visible = true;
            listFavorites.Visible = false;
            labelFavorites.Visible = false;
            whiteHeart.Visible = false;
        }

        /*
         * Affiche les infos complémentaires du point
         */
        public static void infosPoint(string IDPoint, PictureBox pbInfoPoint, ToolTip toolTip)
        {
            // rajouter l'utilisateur
            string info = "Created by : ... : ";
            MySqlDataAdapter ad = new MySqlDataAdapter();
            DataTable tablePoint = new DataTable();
            DataTable tableModifPoint = new DataTable();
            DataTable tableModif = new DataTable();

            Request.useSelectRequestTable(ad, tablePoint, SQL.selectPointByIDPoint("*", IDPoint));
            if (tablePoint.Rows.Count > 0)
            {
                DateTime date = DateTime.Parse(tablePoint.Rows[0][5].ToString());
                info += date.ToString(Constantes.formatDate);
            }
            Request.useSelectRequestTable(ad, tableModifPoint, SQL.selectModifPointByIDPoint(IDPoint));
            if (tableModifPoint.Rows.Count > 0)
            {
                Request.useSelectRequestTable(ad, tableModif, SQL.selectDateModif(tableModifPoint.Rows[0][0].ToString()));
                if (tableModif.Rows.Count > 0)
                {
                    info += "\n Last modification : " + ((DateTime)(tableModif.Rows[0][0])).ToString(Constantes.formatDate);
                }
            }
            toolTip.SetToolTip(pbInfoPoint, info);
        }

        /*
         * Affiche les infos supplémentaires de la checklist affichée à l'écran
         */
        public static void infosChecklist(string IDChecklist, DataRow row, PictureBox PBInfoChecklist, ToolTip toolTip, TextBox titreChecklist, RichTextBox shortDesc,
            MySqlConnection mysql, PictureBox favoris, PictureBox btnEdit, PictureBox btnCancel, PictureBox btnDelete, PictureBox btnExport, PictureBox btnAddPoint)
        {
            titreChecklist.Text = row.ItemArray[1].ToString();
            shortDesc.Text = row.ItemArray[2].ToString();
            string info = "Created by : ";
            info += row.ItemArray[4].ToString();
            info += " : ";
            if (row.ItemArray[3].ToString() != string.Empty)
            {
                DateTime date = DateTime.Parse(row.ItemArray[3].ToString());
                info += date.ToString(Constantes.formatDate);
            }
            MySqlDataAdapter ad = new MySqlDataAdapter();
            DateTime dateModif = new DateTime();

            // chercher les modifs des points
            string modifPoint = "SELECT IDModification1 FROM modif_point WHERE IDPoint IN ( SELECT IDPoint FROM point WHERE IDChecklist = " + IDChecklist + ")";
            string cmdModif = "SELECT * FROM modification WHERE IDModification IN (" + modifPoint + ") ORDER BY Date DESC";
            DataTable tableModifsPoints = new DataTable();
            ad.SelectCommand = new MySqlCommand(cmdModif, mysql);
            ad.Fill(tableModifsPoints);
            if (tableModifsPoints.Rows.Count > 0)
            {
                dateModif = DateTime.Parse(tableModifsPoints.Rows[0][1].ToString());
            }
            // ajouter la modif s'il y en a une
            DataTable modif = new DataTable();
            Request.useSelectRequestTable(ad, modif, SQL.selectModifChecklistByIDChecklist(IDChecklist));

            if (modif.Rows.Count > 0)
            {
                // voir si on prend la modification de la checklist ou si on la compare avec celle de ses points pour voir laquelle est la plus récente
                DataTable tableModif = new DataTable();
                Request.useSelectRequestTable(ad, tableModif, SQL.selectDateModif(modif.Rows[0][0].ToString()));

                if (dateModif != DateTime.MinValue)
                {
                    int resultat = DateTime.Compare(DateTime.Parse(tableModifsPoints.Rows[0][1].ToString()), DateTime.Parse(tableModif.Rows[0][0].ToString()));
                    if (resultat > 0)
                    {
                        dateModif = DateTime.Parse(tableModifsPoints.Rows[0][1].ToString());
                    }
                    else
                    {
                        dateModif = DateTime.Parse(tableModif.Rows[0][0].ToString());
                    }
                }
                else
                {
                    dateModif = DateTime.Parse(tableModif.Rows[0][0].ToString());
                }
            }
            if (dateModif != DateTime.MinValue)
            {
                // mettre l'utilisateur plus tard (donc dans le SELECT, il ne faudra pas juste mettre la date)
                info += "\nLast modification : " + dateModif.ToString(Constantes.formatDate);
            }
            toolTip.SetToolTip(PBInfoChecklist, info);
            toolTip.SetToolTip(favoris, "Favorites");
            toolTip.SetToolTip(btnEdit, "Edit");
            toolTip.SetToolTip(btnCancel, "Cancel");
            toolTip.SetToolTip(btnDelete, "Delete");
            toolTip.SetToolTip(btnExport, "Export");
            toolTip.SetToolTip(btnAddPoint, "Add point");
        }

        /*
         * Affiche le place holder correspondant à la zone de texte vide
         */
        public static void displayPlaceholder(TextBox tb)
        {
            if (tb.Name == Constantes.textBoxTitrePoint)
            {
                tb.Text = "Titel";
                tb.ForeColor = Color.LightGray;
            }
            else if (tb.Name == Constantes.textBoxDescriptionPoint)
            {
                tb.Text = "Beschreibung";
                tb.ForeColor = Color.LightGray;
                Tools.changeLabelSize(tb);
            }
            else if (tb.Name == Constantes.textBoxTitreChecklist)
            {
                tb.Text = "Titel von die Checklist";
                tb.ForeColor = Color.LightGray;
            }
        }

        /*
         * Affiche le place holder correspondant à la zone de texte vide
         */
        public static void displayPlaceholder(RichTextBox tb)
        {
            if (tb.Name == Constantes.textBoxTitrePoint)
            {
                tb.Text = "Titel";
                tb.ForeColor = Color.LightGray;
            }
            else if (tb.Name == Constantes.textBoxDescriptionPoint)
            {
                tb.Text = "Beschreibung";
                tb.ForeColor = Color.LightGray;
            }
            else if (tb.Name == Constantes.textBoxTitreChecklist)
            {
                tb.Text = "Titel von die Checklist";
                tb.ForeColor = Color.LightGray;
            }
            else if (tb.Name == Constantes.textBoxDescriptionChecklist)
            {
                tb.Text = Constantes.textDescriptionChecklist;
                tb.ForeColor = Color.LightGray;
            }
        }

        /*
         * Vérifie s'il faut afficher un place holder dans la zone de texte
         */
        public static void testDisplayPlaceholder(TextBox tTitle, TextBox tDescription)
        {
            if (String.IsNullOrWhiteSpace(tTitle.Text))
            {
                displayPlaceholder(tTitle);
            }
            if (String.IsNullOrWhiteSpace(tDescription.Text))
            {
                displayPlaceholder(tDescription);
            }
        }


        /*
         * Delete the panel who contains the point to delete
         */
        public static void deletePanelPoint(int IDPoint, Panel panelPoints)
        {
            foreach (Control control in panelPoints.Controls)
            {
                if (Tools.isPanelPoint(control, IDPoint))
                {
                    panelPoints.Controls.Remove(control);
                }
            }
        }

        /*
         * Active la barre de défilement du Panel
         */
        public static void activateScrollBar(Panel panel)
        {
            if (panel.VerticalScroll.Enabled == false)
            {
                panel.AutoScroll = false;
                panel.VerticalScroll.Enabled = true;
                panel.AutoScroll = true;
            }
        }

        public static DialogResult cancelEdition(bool creationChecklist)
        {
            string message = string.Empty;
            string title = string.Empty;
            if (creationChecklist)
            {
                message = "Wollen Sie die Erstellung diese Checklist abbrechen ?";
                title = "Erstellung eine Checklist";
            }
            else
            {
                message = "Wollen Sie die Änderung diese Checklist abbrechen";
                title = "Änderung eine Checklist";
            }
            DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.YesNo);
            return result;
        }



    }
}
