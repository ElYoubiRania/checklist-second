using Checklist.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Checklist
{
    public partial class TestTreeview : Form
    {

        private DataSet ds = new DataSet();
        private DataSet dsTopic = new DataSet();
        private DataSet dsChecklist = new DataSet();
        private MySqlConnection mysql;
        private List<PictureBox> listPB = new List<PictureBox>();

        private Image tagImage = Resources.noun_858633_cc2;

        public TestTreeview()
        {
            InitializeComponent();
            string sqlConnection = ConfigurationManager.ConnectionStrings["MaConnexion"].ConnectionString;
            mysql = new MySqlConnection(sqlConnection);
            mysql.Open();
            panel1.Controls.Add(treeView1);

            InitializeTreeView();
        }

        private void InitializeTreeView()
        {
            treeView1.BeginUpdate();

            MySqlDataAdapter ad = new MySqlDataAdapter();
            string cmd = "SELECT * FROM categorie";
            ad.SelectCommand = new MySqlCommand(cmd, mysql);
            ad.Fill(ds);

            string topic = "";
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                treeView1.Nodes.Add(item.ItemArray[1].ToString());

                PictureBox pb = new PictureBox();
                pb.Image = tagImage;
                pb.Size = new Size(15, 15);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Location = new Point(170, treeView1.Nodes[treeView1.Nodes.Count - 1].Bounds.Y);
                int index = treeView1.Nodes.Count - 1;
                pb.Name = "pb-" + index;
                panel1.Controls.Add(pb);
                listPB.Add(pb);

                topic = SQL.selectTopicsByIDCategorie("*", item.ItemArray[0].ToString());
                ad.SelectCommand = new MySqlCommand(topic, mysql);
                dsTopic.Clear();
                ad.Fill(dsTopic);
                string checklist = "";
                foreach (DataRow itemTopic in dsTopic.Tables[0].Rows)
                {
                    treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add(itemTopic.ItemArray[0].ToString());
                    checklist = SQL.selectChecklistsByIDTopic("Titre", itemTopic.ItemArray[1].ToString());
                    ad.SelectCommand = new MySqlCommand(checklist, mysql);
                    dsChecklist.Clear();
                    ad.Fill(dsChecklist);
                    foreach (DataRow itemChecklist in dsChecklist.Tables[0].Rows)
                    {
                        treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes[treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Count - 1].Nodes.Add(itemChecklist.ItemArray[0].ToString());
                    }
                }
            }
            /*
            foreach (TreeNode node in treeView1.Nodes)
            {
                // node.Tag = tagIcon;
                node.Tag = tagImage;
            }*/
            treeView1.EndUpdate();
        }

        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            // Retrieve the node font. If the node font has not been set,
            // use the TreeView font.
            if (e.Node.NodeFont == null) e.Node.NodeFont = ((TreeView)sender).Font;
            // e.Node.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))),((int)(((byte)(213)))),((int)(((byte)(213)))));
            // Draw the node text.
            e.Graphics.DrawString(e.Node.Text, e.Node.NodeFont, Brushes.Black,
                Rectangle.Inflate(e.Bounds, 2, 0));
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            int index = e.Node.Index;
            PictureBox pBox = new PictureBox();
            foreach (PictureBox p in listPB)
            {
                if (p.Name.Substring(3) == index.ToString())
                {
                    pBox = p;
                }
            }
            for (int i = 0; i < 90; i++)
            {
                pBox.Image = RotateImage(pBox.Image, -1);
                System.Threading.Thread.Sleep(2);
                pBox.Update();
            }
            pBox.Image = Resources.noun_858633_cc2;
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            int index = e.Node.Index;
            int nbNoeudsEnfants = 0;
            bool after = false;
            PictureBox pBox = new PictureBox();
            foreach (PictureBox p in listPB)
            {
                if (p.Name.Substring(3) == index.ToString())
                {
                    pBox = p;
                    nbNoeudsEnfants = e.Node.Nodes.Count;
                    after = true;
                }
                else if (after)
                {
                    p.Location = new Point(p.Location.X, p.Location.Y + nbNoeudsEnfants * e.Node.Bounds.Height);
                }
            }
            for (int i = 0; i < 90; i++)
            {
                pBox.Image = RotateImage(pBox.Image, 1);
                System.Threading.Thread.Sleep(2);
                pBox.Update();
            }
        }

        public static Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }
    }
}
