using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checklist
{
    public class Edition
    {
        /*
         * Met à jour l'écran en mode édition (pour le titre de la checklist)
         */
        public static void updateEditionCheck(TextBox titreChecklist, RichTextBox shortDesc, SplitterPanel panel)
        {
            // Control controle = titreChecklist.Parent;
            // Graphics g = controle.CreateGraphics(); 
            if (titreChecklist.ReadOnly == false)
            {
                Graphics g = panel.CreateGraphics();
                Pen p = new Pen(Color.LightGray);
                Pen p2 = new Pen(Color.LightGray);

                if (titreChecklist.Focused)
                {
                    p = new Pen(Constantes.pink);
                }
                if (shortDesc.Focused)
                {
                    p2 = new Pen(Constantes.pink);
                }
                g.Clear(panel.BackColor);
                g.DrawLine(p, new Point(titreChecklist.Left, titreChecklist.Bottom + 2), new Point(titreChecklist.Right, titreChecklist.Bottom + 2));
                g.DrawRectangle(p2, shortDesc.Left - 1, shortDesc.Top - 1, shortDesc.Width + 2, shortDesc.Height + 2);
            }
        }

        /*
         * Met à jour l'écran quand on est en mode édition
         */
        public static void updateEdition(Panel panel)
        {
            foreach (Control control in panel.Controls)
            {
                if (control.GetType() == typeof(Panel) && control.Name.StartsWith(Constantes.panelPoint) && control.Visible)
                {
                    bool focusTitle = false;
                    bool focusDesc = false;
                    Point pt1Title = new Point();
                    Point pt2Title = new Point();
                    Point pt1Desc = new Point();
                    Point pt2Desc = new Point();
                    foreach (Control contr in control.Controls)
                    {
                        if (contr.Name.StartsWith(Constantes.textBoxTitrePoint))
                        {
                            pt1Title = new Point(contr.Left, contr.Bottom + 2);
                            pt2Title = new Point(contr.Right, contr.Bottom + 2);
                            if (contr.Focused)
                            {
                                focusTitle = true;
                            }
                        }
                        else if (contr.Name.StartsWith(Constantes.textBoxDescriptionPoint))
                        {
                            pt1Desc = new Point(contr.Left, contr.Bottom + 2);
                            pt2Desc = new Point(contr.Right, contr.Bottom + 2);
                            if (contr.Focused)
                            {
                                focusDesc = true;
                            }
                        }
                    }
                    Graphics g = ((Panel)control).CreateGraphics();

                    Pen p = new Pen(Color.LightGray);
                    Pen pFocus = new Pen(Constantes.pink);
                    if (focusTitle)
                    {
                        g.DrawLine(pFocus, pt1Title, pt2Title);
                    }
                    else
                    {
                        g.DrawLine(p, pt1Title, pt2Title);
                    }
                    if (focusDesc)
                    {
                        g.DrawLine(pFocus, pt1Desc, pt2Desc);
                    }
                    else
                    {
                        g.DrawLine(p, pt1Desc, pt2Desc);
                    }
                }
            }
        }

        /*
         * Met à jour dans la base de données les modifications effectuées dans la checklist
         */
        public static bool updateModifChecklist(MySqlDataAdapter ad, string cmd, int idTopic, ChecklistRepo check, TextBox titreChecklist, RichTextBox shortDesc,
            MySqlConnection mysql)
        {
            // verifier qu'il n'y ait pas de doublons
            cmd = string.Empty;

            string description = string.Empty;
            if (Tools.isPlaceHolder(shortDesc) == false)
            {
                description = shortDesc.Text;
            }

            // Enlever placeHolder == false
            // pour le titre, si c'est un placeholder, il est vide, donc il faudrait un message d'erreur
            // pour la description, si c'est un placeholder, il faut vérifier s'il était vide au départ
            // voir aussi pour les points
            /* if ((titreChecklist.Text != check.titre && isPlaceHolder(titreChecklist) == false) || 
                 (shortDesc.Text != check.description && isPlaceHolder(shortDesc) == false))*/
            if ((titreChecklist.Text != check.titre) || (description != check.description))
            {
                cmd = "UPDATE checklist SET ";
                // le titre ne peut pas être vide au départ
                if (titreChecklist.Text != check.titre)
                {
                    if (titreChecklist.Text.Length == 0 || Tools.isPlaceHolder(titreChecklist))
                    {
                        if (Tools.emptyTitle(Constantes.checklist) == false)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (DB.verifyDuplicateChecklist(ad, idTopic, titreChecklist.Text) == false)
                        {
                            return false;
                        }
                        cmd += "Titre = '" + titreChecklist.Text + "'";
                    }
                    if (check.description != description)
                    {
                        cmd += ", Description = '" + description + "'";
                        check.description = description;
                    }
                    check.titre = titreChecklist.Text;
                }
                else
                {
                    if (check.description != description)
                    {
                        cmd += "Description = '" + description + "'";
                        check.description = description;
                    }
                }
                cmd += " WHERE IDChecklist = " + check.IDChecklist;
            }
            if (cmd != string.Empty)
            {
                ad.UpdateCommand = new MySqlCommand(cmd, mysql);
                ad.UpdateCommand.ExecuteNonQuery();
                DB.dateModif(ad, true, check.IDChecklist.ToString(), mysql);
            }
            return true;
        }

        /*
         * Supprime un point de la base de données une fois la modification validée
         * nom : deleteOnePoint déjà utilisé
         */
        public static void supprimerUnPoint(MySqlDataAdapter ad, int point, Panel panelPoints)
        {
            Request.deleteTypesPoint(ad, point);
            Request.deleteModifPoint(ad, point);
            Request.deleteOnePoint(ad, point);
            Display.deletePanelPoint(point, panelPoints);
        }

        /*
         * Annule les points supprimés, lorsque l'on annule la modification de la checklist
         */
        public static void cancelDeletedPoints(int nbPointsNonSupprimes, Control control)
        {
            if (nbPointsNonSupprimes > 0)
            {
                control.Location = new Point(control.Location.X, control.Location.Y + nbPointsNonSupprimes * 82);
            }
        }

        /*
         * Annule les points qui ont été créés lorsque l'on annule l'ajout d'un point
         */
        public static void cancelCreatedPoints(Panel panelPoints)
        {
            Control remove = new Control();
            // ne marche que pour un point car on ne peut en créer qu'un en même temps
            foreach (Control control in panelPoints.Controls)
            {
                if (control.GetType() == typeof(Panel) && control.Name == Tools.namePanelPointTemp())
                {
                    remove = control;
                }
            }
            panelPoints.Controls.Remove(remove);
        }
    }
}
