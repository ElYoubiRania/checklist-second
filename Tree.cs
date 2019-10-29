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
    /* Different methods for the treeView */
    public class Tree
    {

        public static void InitialiseDataSetTreeView(DataSet dataSetTreeView)
        {
            dataSetTreeView.Tables.Clear();
            dataSetTreeView.Tables.Add("Categorie");
            dataSetTreeView.Tables.Add("Topic");
            dataSetTreeView.Tables.Add("Checklist");
        }

        /*
         * Initialise le treeView avec les données de la base de données
         */
        // Treeview
        // Faire un dataSet avec plusieurs tables : categories, topics et checklists
        public static void InitialiseTreeView(TreeView treeView1, DataSet dataSetTreeView, Bitmap tagBitmap)
        {
            treeView1.BeginUpdate();
            treeView1.HotTracking = true;
            treeView1.FullRowSelect = true;
            MySqlDataAdapter ad = new MySqlDataAdapter();

            InitialiseDataSetTreeView(dataSetTreeView);

            Request.useSelectRequestTable(ad, dataSetTreeView.Tables["Categorie"], SQL.selectAllCategories());
            foreach (DataRow item in dataSetTreeView.Tables["Categorie"].Rows)
            {
                treeView1.Nodes.Add(item.ItemArray[1].ToString());
                dataSetTreeView.Tables["Topic"].Clear();

                Request.useSelectRequestTable(ad, dataSetTreeView.Tables["Topic"], SQL.selectTopicsByIDCategorie("Titre, IDTopic", item.ItemArray[0].ToString()));
                string checklist = string.Empty;
                foreach (DataRow itemTopic in dataSetTreeView.Tables["Topic"].Rows)
                {
                    treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add(itemTopic.ItemArray[0].ToString());
                    dataSetTreeView.Tables["Checklist"].Clear();

                    Request.useSelectRequestTable(ad, dataSetTreeView.Tables["Checklist"], SQL.selectChecklistsByIDTopic("Titre", itemTopic.ItemArray[1].ToString()));
                    foreach (DataRow itemChecklist in dataSetTreeView.Tables["Checklist"].Rows)
                    {
                        treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes[treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Count - 1].Nodes.Add(itemChecklist.ItemArray[0].ToString());
                    }
                }
            }
            foreach (TreeNode node in treeView1.Nodes)
            {
                // node.Tag = tagIcon;
                // node.Tag = tagImage;
                node.Tag = tagBitmap;
                // node.ForeColor = Color.White;
            }
            // treeView1.ForeColor = Color.White;
            treeView1.EndUpdate();
        }

        /*
         * Initialise le menu contextuel qui est affiché lorsque l'on clique avec le clic droit sur un noeud du treeView
         */
        public static void InitialiseContextMenuTreeview(string natureElement, ContextMenuStrip contextMenuStrip1)
        {
            contextMenuStrip1.Items.Clear();
            if (natureElement == Constantes.categorie)
            {
                contextMenuStrip1.Items.Add(Constantes.modifCategorie);
                contextMenuStrip1.Items.Add(Constantes.supprCategorie);
                contextMenuStrip1.Items.Add(Constantes.ajoutTopic);
            }
            else if (natureElement == Constantes.topic)
            {
                contextMenuStrip1.Items.Add(Constantes.modifTopic);
                contextMenuStrip1.Items.Add(Constantes.supprTopic);
                contextMenuStrip1.Items.Add(Constantes.ajoutChecklist);
            }
            else if (natureElement == Constantes.autre)
            {
                contextMenuStrip1.Items.Add(Constantes.ajoutCategorie);
            }
            // peut-être laisser le menu contextuel pour permettre de supprimer la checklist de cette façon aussi ?
            /*
            else if (natureElement == Constantes.checklist)
            {
                contextMenuStrip1.Items.Add("Modifier le nom de la checklist");
                contextMenuStrip1.Items.Add("Supprimer la checklist");
            }
            */
        }

        /*
         * Réinitialise le treeView
         */
        public static void refreshTreeview(TreeView treeView1, DataSet dataSetTreeView, Bitmap tagBitmap)
        {
            treeView1.Nodes.Clear();
            dataSetTreeView.Clear();
            Tree.InitialiseTreeView(treeView1, dataSetTreeView, tagBitmap);
        }

        /*
         * Si le noeud de la checklist n'est pas affiché dans le treeView, affiche ce noeud à l'écran
         */
        public static void expandNodes(string titleCategory, string titleTopic, string titleChecklist, TreeView treeView1, TreeNode nodeCategory, TreeNode nodeTopic, TreeNode nodeChecklist)
        {
            treeView1.CollapseAll();
            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Level == 0 && node.Text == titleCategory)
                {
                    if (node.IsExpanded == false)
                    {
                        node.Expand();
                    }
                    nodeCategory = node;

                    foreach (TreeNode children in node.Nodes)
                    {
                        if (children.Level == 1 && children.Text == titleTopic)
                        {
                            if (children.IsExpanded == false)
                            {
                                children.Expand();
                            }
                            nodeTopic = children;

                            foreach (TreeNode check in children.Nodes)
                            {
                                if (check.Level == 2 && check.Text == titleChecklist)
                                {
                                    if (check.IsSelected == false)
                                    {
                                        treeView1.SelectedNode = check;
                                    }
                                    nodeChecklist = check;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void expandNodes(int IDChecklist, TreeView treeView1, TreeNode nodeCategory, TreeNode nodeTopic, TreeNode nodeChecklist)
        {
            string titreCategorie = string.Empty;
            string titreTopic = string.Empty;
            string titreChecklist = string.Empty;

            string IDTopic = string.Empty;
            string IDCategorie = string.Empty;

            MySqlDataAdapter ad = new MySqlDataAdapter();
            DataTable table = new DataTable();

            Request.useSelectRequestTable(ad, table, SQL.selectChecklistByID("Titre, IDTopic", IDChecklist.ToString()));
            titreChecklist = table.Rows[0][0].ToString();
            IDTopic = table.Rows[0][1].ToString();
            table.Clear();
            table.Columns.Clear();
            Request.useSelectRequestTable(ad, table, SQL.selectTopicByIDTopic("Titre, IDCategorie", IDTopic));
            titreTopic = table.Rows[0][0].ToString();
            IDCategorie = table.Rows[0][1].ToString();
            table.Clear();
            table.Columns.Clear();
            Request.useSelectRequestTable(ad, table, SQL.selectCategoryByID(IDCategorie));
            titreCategorie = table.Rows[0][0].ToString();
            Tree.expandNodes(titreCategorie, titreTopic, titreChecklist, treeView1, nodeCategory, nodeTopic, nodeChecklist);
        }

        /*   CALCUL DU RECTANGLE DE SELECTION    */
        // Returns the bounds of the specified node, including the region 
        // occupied by the node label and any node tag displayed.
        public static Rectangle NodeBounds(TreeNode node, TreeView treeView1)
        {
            // Set the return value to the normal node bounds.
            Rectangle bounds = node.Bounds;
            if (node.Tag != null)
            {
                // Retrieve a Graphics object from the TreeView handle
                // and use it to calculate the display width of the tag.
                Graphics g = treeView1.CreateGraphics();

                // Adjust the node bounds using the calculated value.

                // 1- mesure largeur du node.text suivant 
                // le NodeFont utilise & ajuste le rectangle de selection 

                int tagWidth = (int)(g.MeasureString(node.Text, node.NodeFont).Width);
                bounds.Width = tagWidth;

                // 2-mesure la largeur d'icon 
                Image im = (Image)(node.Tag);
                tagWidth = im.Width;
                //   Icon icn = (Icon)(node.Tag);
                //   tagWidth = icn.Width;

                // decale le sommet -x- du  rectangle de selection 
                bounds.Offset(tagWidth / 2, 0);

                /* ------ rappel : une dilatation accroît a gauche et à droite simultanément)....*/
                // dilate suivant -x- de tagWidth / 2 tel que
                bounds = Rectangle.Inflate(bounds, tagWidth / 2, 0);
                g.Dispose();
            }
            return bounds;
        }

        public static bool treeNodeMouseEnter(DrawTreeNodeEventArgs e)
        {
            return (e.State.ToString().IndexOf(TreeNodeStates.Hot.ToString()) >= 0);
        }

        public static void cancelNodeCreation(NodeLabelEditEventArgs e, string btnSettingsClick)
        {
            TreeNode parent = new TreeNode();
            if (btnSettingsClick == Constantes.addTopic)
            {
                parent = e.Node.Parent;
            }
            else
            {
                parent = null;
            }
            e.Node.TreeView.Nodes.Remove(e.Node);
            if (parent != null && parent.Nodes.Count == 0)
            {
                parent.Collapse();
            }
        }
    }
}
