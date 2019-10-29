using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Checklist
{

    //private System.  Icon tagIcon = My.Resources.Boss;
    public partial class Form2 : Form
    {
        private Icon tagIcon = new Icon("C:/Users/philippe_m/Documents/Projet M1/Icones/zip/ico.ico");
        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.treeView1.BackColor = Color.FromArgb(42, 63, 84);
            this.treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            InitializeTreeView();
        }
        // Populates a TreeView control with example nodes. 
        private void InitializeTreeView()
        {

            treeView1.BeginUpdate();
            // un icon pour le parent
            treeView1.Nodes.Add("Vertrieb");
            treeView1.Nodes.Add("Einkauf");
            treeView1.Nodes.Add("Production");
            treeView1.Nodes[0].Tag = tagIcon;
            treeView1.Nodes[1].Tag = tagIcon;
            treeView1.Nodes[2].Tag = tagIcon;
            treeView1.Nodes[0].BackColor = Color.Gray;


            treeView1.Nodes[0].Nodes.Add("Angebot");
            treeView1.Nodes[0].Nodes.Add("Rechnung");

            // un icon pour le petit fils
            treeView1.Nodes[0].Nodes[1].Nodes.Add("Checklist 1");
            treeView1.Nodes[0].Nodes[1].Nodes[0].BackColor = Color.LightGray;

            // un icon pour le  3 fils
            treeView1.Nodes[0].Nodes.Add("Proforma");
            treeView1.Nodes[0].Nodes[2].BackColor = Color.LightGoldenrodYellow;

            treeView1.EndUpdate();
        }

        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            // Retrieve the node font. If the node font has not been set,
            // use the TreeView font.
            if (e.Node.NodeFont == null) e.Node.NodeFont = ((TreeView)sender).Font;


            // Draw the node text.
            e.Graphics.DrawString(e.Node.Text, e.Node.NodeFont, Brushes.Black,
                Rectangle.Inflate(e.Bounds, 2, 0));





            // If a node tag is present, draw its icon 
            // to the right of the label text.
            if (e.Node.Tag != null)
            {
                Icon icn = (Icon)(e.Node.Tag);
                e.Graphics.DrawIcon(icn,
            100, NodeBounds(e.Node).Top);

            }


        }
        /*   CALCUL DU RECTANGLE DE SELECTION    */
        // Returns the bounds of the specified node, including the region 
        // occupied by the node label and any node tag displayed.
        private Rectangle NodeBounds(TreeNode node)
        {
            // Set the return value to the normal node bounds.
            Rectangle bounds = node.Bounds;
            if (node.Tag != null)
            {
                // Retrieve a Graphics object from the TreeView handle
                // and use it to calculate the display width of the tag.
                Graphics g = treeView1.CreateGraphics();

                // Adjust the node bounds using the calculated value.


                //  1- mesure  largeur du node.text suivant 
                //  le NodeFont utilise & ajuste le rectangle de selection 

                int tagWidth = (int)(g.MeasureString(node.Text, node.NodeFont).Width);
                bounds.Width = tagWidth;

                //  2-mesure la largeur d'icon 
                Icon icn = (Icon)(node.Tag);
                tagWidth = icn.Width;

                // decale le sommet -x- du  rectangle de selection 

                bounds.Offset(tagWidth / 2, 0);


                /* ------ rappel :une dilatation accroit a gauche et à droite simuultanement)....*/
                //   dilate  suivant -x-  de tagWidth / 2  tel que

                bounds = Rectangle.Inflate(bounds, tagWidth / 2, 0);
                g.Dispose();
            }

            return bounds;

        }
    }
}
