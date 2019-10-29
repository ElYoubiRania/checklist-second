using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checklist
{
    /* Different methods for the favorites management */
    public class Favorites
    {
        /*
         * Affiche les favoris sur la partie de gauche de l'écran
         */
        public static void displayFavorites(TreeView treeView1, ListView listFavorites, Label labelFavorites, PictureBox whiteHeart)
        {
            treeView1.Visible = false;
            listFavorites.Visible = true;
            labelFavorites.Visible = true;
            whiteHeart.Visible = true;
        }

        public static void deleteFromFavoriteList(int IDChecklist, ListView listFavorites)
        {
            foreach (ListViewItem item in listFavorites.Items)
            {
                if (item.Name.EndsWith("-" + IDChecklist))
                {
                    listFavorites.Items.Remove(item);
                    return;
                }
            }
        }

        /*
         * Rend le bouton favoris rose
         */
        public static void favoriRose(PictureBox pbFavoris)
        {
            pbFavoris.BackColor = Constantes.pink;
        }

        /*
         * Rend le bouton favoris bleu
         */
        public static void favoriBleu(PictureBox pbFavoris)
        {
            pbFavoris.BackColor = Constantes.deepBlue;
        }

        /*
         * Rend le bouton favoris bleu clair
         */
        public static void favoriBleuClair(PictureBox pbFavoris)
        {
            pbFavoris.BackColor = Constantes.bleuIconesMenu;
        }

        /*
         * Lors de l'affichage d'une checklist, vérifie si la checklist est dans les favoris, pour afficher le bouton "favoris" dans la bonne couleur
         */
        // tests avec IDUtilisateur = 01
        public static void presenceChecklistInFavorites(MySqlDataAdapter adapt, DataTable table, string IDChecklist, PictureBox favoris)
        {
            Request.useSelectRequestTable(adapt, table, SQL.selectFavorisByIDChecklist(IDChecklist));
            if (table.Rows.Count == 0)
            {
                favoriBleu(favoris);
            }
            else
            {
                favoriRose(favoris);
            }
        }

        /*
         * Change la couleur du bouton favori
         */
        public static bool changerCouleurFavori(PictureBox favoris)
        {
            if (favoris.BackColor == Constantes.deepBlue || favoris.BackColor == Constantes.bleuIconesMenu)
            {
                favoriRose(favoris);
                return true;
            }
            else if (favoris.BackColor == Constantes.pink || favoris.BackColor == Constantes.lightPink)
            {
                favoriBleu(favoris);
                return false;
            }
            return false;
        }
    }
}
