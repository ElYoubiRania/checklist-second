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
    class Tools
    {
        public static string returnID(string name)
        {
            return name.Substring(name.IndexOf("-") + 1);
        }

        public static string returnID(TreeNode node)
        {
            MySqlDataAdapter ad = new MySqlDataAdapter();
            DataTable table = new DataTable();
            if (node.Level == 0)
            {
                Request.useSelectRequestTable(ad, table, SQL.selectCategoryByTitle("IDCategorie", node.Text));
            }
            else if (node.Level == 1)
            {
                Request.useSelectRequestTable(ad, table, SQL.selectTopicByTitle("IDTopic", node.Text, "(" + SQL.selectCategoryByTitle("IDCategorie", node.Parent.Text) + ")"));
            }
            else if (node.Level == 2)
            {
                Request.useSelectRequestTable(ad, table, SQL.selectChecklistNode(node));
            }
            return table.Rows[0][0].ToString();
        }

        public static string replaceTmpByID(string nom, int ID)
        {
            return nom.Replace(Constantes.temp, ID.ToString());
        }

        public static string namePanelPointTemp()
        {
            return Constantes.panelPoint + Constantes.temp;
        }

        public static string nameType(string name, string IDPoint)
        {
            return name + "-" + IDPoint;
        }

        /*
         * Vérifie s'il s'agit du premier panel de la liste
         */
        public static bool firstPanel(Panel panel, string name)
        {
            foreach (Control control in panel.Controls)
            {
                if (control.GetType() == typeof(Panel) && control.Name.StartsWith(name))
                {
                    return false;
                }
            }
            return true;
        }

        /*
         * Vérifie s'il s'agit du premier point de la liste
         */
        public static bool firstPoint(Panel panel)
        {
            return firstPanel(panel, Constantes.panelPoint);
        }

        /*
         * Vérifie s'il s'agit de la première checklist de la liste
         */
        public static bool firstChecklist(Panel panel)
        {
            return firstPanel(panel, Constantes.panelChecklist);
        }

        /*
         * Renvoie la position d'un nouveau point
         */
        public static int locationYNewPoint(Panel panel, string name)
        {
            int locationMax = -1000;
            foreach (Control control in panel.Controls)
            {
                if (control.GetType() == typeof(Panel) && control.Name.StartsWith(name))
                {
                    if (control.Location.Y > locationMax)
                    {
                        locationMax = control.Location.Y;
                    }
                }
            }
            return locationMax + Constantes.distanceEntreDeuxPoints;
        }

        /*
         * Lors de la création d'un point, un nom temporaire lui est donné
         * Modifie le nom avec l'ID du point une fois le point créé
         */
        public static void changeTemporaryName(Panel panelPointTemporaire, int IDPoint)
        {
            panelPointTemporaire.Name = replaceTmpByID(panelPointTemporaire.Name, IDPoint);
            foreach (Control control in panelPointTemporaire.Controls)
            {
                if (control.Name.Contains(Constantes.temp))
                {
                    control.Name = replaceTmpByID(control.Name, IDPoint);
                }
            }
        }

        public static int changeLabelSize(object label)
        {
            Size sizeLabel = new Size();
            Label lab = new Label();
            TextBox textB = new TextBox();

            if (label.GetType() == typeof(Label))
            {
                lab = (Label)label;
            }
            else if (label.GetType() == typeof(TextBox))
            {
                textB = (TextBox)label;
            }

            if (label.GetType() == typeof(Label))
            {
                // sizeLabel = g.MeasureString(lab.Text,lab.Font);
                sizeLabel = TextRenderer.MeasureText(lab.Text, lab.Font);
            }
            else if (label.GetType() == typeof(TextBox))
            {
                sizeLabel = TextRenderer.MeasureText(textB.Text, textB.Font);
                textB.Size = new Size(sizeLabel.Width, textB.Height);
            }
            int width = sizeLabel.Width;
            return width;
        }

        public static bool isPanelPoint(Control control, int IDPoint)
        {
            return (control.GetType() == typeof(Panel) && control.Name.StartsWith(Constantes.panelPoint) && control.Name.EndsWith(IDPoint.ToString()));
        }

        /*
         * Affiche un message à l'écran indiquant que le titre est vide
         */
        public static bool emptyTitle(string genre)
        {
            string message = "Veuillez saisir le titre ";
            if (genre == Constantes.point)
            {
                message += "du point.";
            }
            else if (genre == Constantes.checklist)
            {
                message += "de la checklist.";
            }
            else if (genre == Constantes.topic)
            {
                message += "du topic.";
            }
            else if (genre == Constantes.categorie)
            {
                message += "de la catégorie.";
            }
            DialogResult result;
            result = MessageBox.Show(message, "Titre manquant", MessageBoxButtons.OK);

            if (result == DialogResult.OK)
            {
                return false;
            }
            return true;
        }

        public static bool displayedChecklist(TreeNode node, ChecklistRepo check)
        {
            MySqlDataAdapter ad = new MySqlDataAdapter();
            DataTable table = new DataTable();
            Request.useSelectRequestTable(ad, table, SQL.selectChecklistNode(node));
            if (check != null && table.Rows[0][0].ToString() == check.IDChecklist.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
         * Vérifie si le texte écrit dans la zone est un place holder
         */
        public static bool isPlaceHolder(TextBox tb)
        {
            // vérifier aussi qu'il y ait écrit Titre ou Description ?
            return tb.ForeColor == Color.LightGray;
        }

        /*
         * Vérifie si le texte écrit dans la zone est un place holder
         */
        public static bool isPlaceHolder(RichTextBox tb)
        {
            return tb.ForeColor == Color.LightGray && tb.Text == Constantes.textDescriptionChecklist;
        }

        /*
         * Réinitialise la DataTable donnée en paramètre
         */
        public static void resetDataTable(DataSet data, string nameTable)
        {
            if (data.Tables.Contains(nameTable))
            {
                data.Tables[nameTable].Clear();
                try
                {
                    data.Tables[nameTable].Columns.Clear();
                }
                catch (ArgumentException) { }
            }
            else
            {
                data.Tables.Add(nameTable);
            }
        }

        /*
         * Renvoie l'ID du topic actuel par rapport à la catégorie et au topic sélectionnés
         */
        public static int returnIDTopic(MySqlDataAdapter ad, DataSet dsResearch, TreeNode nodeTopic)
        {
            string cmdCategorie = "(" + SQL.selectCategoryByTitle("IDCategorie", nodeTopic.Parent.Text) + ")";

            resetDataTable(dsResearch, Constantes.topic);
            Request.useSelectRequestTable(ad, dsResearch.Tables[Constantes.topic], SQL.selectTopicByTitle("*", nodeTopic.Text, cmdCategorie));

            return Int32.Parse(dsResearch.Tables[Constantes.topic].Rows[0].ItemArray[0].ToString());
        }

        public static void returnIDCategory(MySqlDataAdapter ad, ref int idCategory, string commande, DataSet dsResearch)
        {
            Tools.resetDataTable(dsResearch, Constantes.categorie);
            Request.useSelectRequestTable(ad, dsResearch.Tables[Constantes.categorie], commande);
            if (dsResearch.Tables[Constantes.categorie].Rows.Count > 0)
            {
                idCategory = Int32.Parse(dsResearch.Tables[Constantes.categorie].Rows[0][0].ToString());
            }
        }

        public static void returnIDCategoryAndTopic(MySqlDataAdapter ad, ref int idCategory, ref int idTopic, DataSet dsResearch, TreeNode nodeTopic, string topicAModifier)
        {
            string cmdCategorie = "(" + SQL.selectCategoryByTitle("IDCategorie", nodeTopic.Parent.Text) + ")";
            Tools.resetDataTable(dsResearch, Constantes.topic);
            Request.useSelectRequestTable(ad, dsResearch.Tables[Constantes.topic], SQL.selectTopicByTitle("IDTopic, IDCategorie", topicAModifier, cmdCategorie));
            if (dsResearch.Tables[Constantes.topic].Rows.Count > 0)
            {
                idTopic = Int32.Parse(dsResearch.Tables[Constantes.topic].Rows[0][0].ToString());
                idCategory = Int32.Parse(dsResearch.Tables[Constantes.topic].Rows[0][1].ToString());
            }
        }
    }
}
