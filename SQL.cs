using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checklist
{
    public class SQL
    {
        /* REQUETES INSERT */
        /* type_point */
        public static string insertTypePoint(int type, string IDPoint)
        {
            return "INSERT INTO type_point VALUES (" + type + ", " + IDPoint + ")";
        }

        /* point */
        public static string insertPoint(string titre, string description, string date, int IDChecklist)
        {
            return "INSERT INTO point (Titre, Description, Date_Creation, IDChecklist) VALUES ('"
                + titre + "','" + description + "', '" + date + "', " + IDChecklist + ")";
        }

        /* checklist */
        public static string insertChecklist(string titre, string description, int IDTopic)
        {
            return "INSERT INTO checklist (Titre, Description, Date_creation, IDTopic) VALUES ('"
                    + titre + "','" + description + "','" + DateTime.Today.ToString(Constantes.formatDate2) + "','" + IDTopic + "')";
        }

        public static string insertChecklistWithoutDescription(string titre, int IDTopic)
        {
            return "INSERT INTO checklist (Titre, Date_creation, IDTopic) VALUES ('"
                    + titre + "','" + DateTime.Today.ToString(Constantes.formatDate2) + "','" + IDTopic + "')";
        }

        /* topic */
        public static string insertTopic(string titre, int IDCategorie)
        {
            return "INSERT INTO topic (Titre, IDCategorie) VALUES ('" + titre + "','" + IDCategorie + "')";
        }

        /* categorie */
        public static string insertCategory(string titre)
        {
            return "INSERT INTO categorie (Titre) VALUES ('" + titre + "')";
        }

        /* modification */
        public static string insertModificationToday()
        {
            return "INSERT INTO modification (Date, User) VALUES ('" + DateTime.Today.ToString(Constantes.formatDate2) + "' , 'utilisateur')";
        }


        /* REQUETES SELECT */
        /* point */
        public static string selectPointsByIDChecklist(string champ, string IDChecklist)
        {
            return "SELECT " + champ + " FROM point WHERE IDChecklist = " + IDChecklist;
        }

        public static string selectPointByIDPoint(string champ, string IDPoint)
        {
            return "SELECT " + champ + " FROM point WHERE IDPoint = " + IDPoint;
        }

        public static string selectPointByTitle(int IDChecklist, string title)
        {
            return selectPointsByIDChecklist("*", IDChecklist.ToString()) + " AND Titre = '" + title + "'";
        }

        /* type_point */
        public static string selectPointsType(int IDChecklist, int type)
        {
            return "SELECT IDPoint1 FROM type_point WHERE IDPoint1 IN ("
                + selectPointsByIDChecklist("IDPoint", IDChecklist.ToString()) + ") AND IDType = " + type;
        }

        public static string selectTypesByIDPoint(string IDPoint)
        {
            return "SELECT IDType FROM type_point WHERE IDPoint1 = " + IDPoint;
        }

        /* checklist */
        public static string selectChecklist(string titreCategorie, string titreTopic, string titreChecklist)
        {
            return "SELECT * FROM checklist WHERE IDTopic = "
                    + "(SELECT IDTopic FROM topic WHERE IDCategorie = "
                    + "(SELECT IDCategorie FROM Categorie WHERE Titre = '" + titreCategorie + "')"
                    + " AND Titre = '" + titreTopic + "')"
                    + " AND Titre = '" + titreChecklist + "'";
        }

        public static string selectChecklistNode(TreeNode nodeChecklist)
        {
            return selectChecklist(nodeChecklist.Parent.Parent.Text, nodeChecklist.Parent.Text, nodeChecklist.Text);
        }

        // peut-être mettre en paramètre le nombre de checklists si jamais on veut changer
        public static string selectLastChecklists()
        {
            return "SELECT * FROM checklist ORDER BY Date_creation DESC LIMIT 0,5";
        }

        public static string selectChecklistsByIDTopic(string champ, string IDTopic)
        {
            return "SELECT " + champ + " FROM checklist WHERE IDTopic =" + IDTopic;
        }

        public static string selectChecklistByID(string champ, string IDChecklist)
        {
            return "SELECT " + champ + " FROM checklist WHERE IDChecklist = " + IDChecklist;
        }

        public static string selectChecklistByTitle(int IDTopic, string titleChecklist)
        {
            return "SELECT * FROM checklist WHERE IDTopic = " + IDTopic + " AND Titre = '" + titleChecklist + "'";
        }

        /* topic */
        public static string selectTopicsByIDCategorie(string champ, string IDCategorie)
        {
            return "SELECT " + champ + " FROM topic WHERE IDCategorie = " + IDCategorie;
        }

        public static string selectTopicByIDTopic(string champ, string IDTopic)
        {
            return "SELECT " + champ + " FROM topic WHERE IDTopic = " + IDTopic;
        }

        public static string selectTopicByTitle(string champ, string titre, string IDCategorie)
        {
            return "SELECT " + champ + " FROM topic WHERE IDCategorie = " + IDCategorie + " AND Titre = '" + titre + "'";
        }

        /* categorie */
        public static string selectAllCategories()
        {
            return "SELECT * FROM categorie";
        }

        public static string selectTitleCategoryByTopic(string IDTopic)
        {
            return "SELECT Titre FROM categorie WHERE IDCategorie = (SELECT IDCategorie FROM topic WHERE IDTopic = " + IDTopic + ")";
        }

        public static string selectCategoryByTitle(string champ, string titre)
        {
            return "SELECT " + champ + " FROM categorie WHERE Titre = '" + titre + "'";
        }

        public static string selectCategoryByID(string IDCategorie)
        {
            return "SELECT Titre FROM categorie WHERE IDCategorie = " + IDCategorie;
        }

        /* modification */
        // plus tard, rajouter string utilisateur dans les paramètres
        public static string selectModification()
        {
            return "SELECT IDModification FROM modification WHERE Date = '" + DateTime.Today.ToString(Constantes.formatDate2)
                + "' AND User = 'utilisateur'";
        }

        public static string selectDateModif(string IDModification)
        {
            return "SELECT Date FROM modification WHERE IDModification = " + IDModification;
        }

        public static string selectAllModifications()
        {
            return "SELECT IDModification FROM modification";
        }

        /* modif_checklist */
        public static string selectModifChecklistByIDChecklist(string IDChecklist)
        {
            return "SELECT IDModification FROM modif_checklist WHERE IDCheck = " + IDChecklist;
        }

        public static string selectModifChecklistByIDModif(string IDModif)
        {
            return "SELECT * FROM modif_checklist WHERE IDModification = " + IDModif;
        }

        /* modif_point */
        public static string selectModifPointByIDPoint(string IDPoint)
        {
            return "SELECT IDModification1 FROM modif_point WHERE IDPoint = " + IDPoint;
        }

        public static string selectModifPointByIDModif(string IDModif)
        {
            return "SELECT * FROM modif_point WHERE IDModification1 = " + IDModif;
        }

        /* favoris */
        public static string selectFavorisByIDChecklist(string IDChecklist)
        {
            return "SELECT * FROM favoris WHERE IDChecklist1 = " + IDChecklist + " AND IDUtilisateur = 01";
        }

        public static string selectFavorisByIDUtilisateur()
        {
            return "SELECT * FROM favoris WHERE IDUtilisateur = 01";
        }

        public static string selectFavorisByID(long IDFavoris)
        {
            return "SELECT IDChecklist1 FROM favoris WHERE IDFavoris = " + IDFavoris;
        }

        public static string selectAll(string nameTable, string champ, string valeur)
        {
            return "SELECT * FROM " + nameTable + " WHERE " + champ + " = " + valeur;
        }


        /* REQUETES UPDATE */
        /* topic */
        public static string updateTitleTopic(string titre, int IDTopic)
        {
            return "UPDATE topic SET Titre = '" + titre + "' WHERE IDTopic = " + IDTopic;
        }

        /* categorie */
        public static string updateTitleCategory(string titre, int IDCategorie)
        {
            return "UPDATE categorie SET Titre = '" + titre + "' WHERE IDCategorie = " + IDCategorie;
        }

        public static string updatePoint(string text, string champ, string IDPoint)
        {
            return "UPDATE point SET " + champ + " = '" + text + "' WHERE IDPoint = " + IDPoint;
        }


        /* REQUETES DELETE */
        /* type_point */
        public static string deleteTypesPoint(int IDPoint)
        {
            return "DELETE FROM type_point WHERE IDPoint1 = " + IDPoint;
        }

        public static string deleteOneTypePoint(int type, string IDPoint)
        {
            return "DELETE FROM type_point WHERE IDType = " + type + " AND IDPoint1 = " + IDPoint;
        }

        /* modification */
        public static string deleteModification(string IDModif)
        {
            return "DELETE FROM modification WHERE IDModification = " + IDModif;
        }

        /* modif_checklist */
        public static string deleteModifChecklist(int IDChecklist)
        {
            return "DELETE FROM modif_checklist WHERE IDCheck = " + IDChecklist;
        }

        /* modif_point */
        public static string deleteModifPoint(int IDPoint)
        {
            return "DELETE FROM modif_point WHERE IDPoint = " + IDPoint;
        }

        /* point */
        public static string deletePointsChecklist(int IDChecklist)
        {
            return "DELETE FROM point WHERE IDChecklist = " + IDChecklist;
        }

        public static string deletePoint(int IDPoint)
        {
            return "DELETE FROM point WHERE IDPoint = " + IDPoint;
        }

        /* checklist */
        public static string deleteChecklistsTopic(int IDTopic)
        {
            return "DELETE FROM checklist WHERE IDTopic = " + IDTopic;
        }

        public static string deleteChecklist(int IDChecklist)
        {
            return "DELETE FROM checklist WHERE IDChecklist = " + IDChecklist;
        }

        /* topic */
        public static string deleteTopic(string champ, int ID)
        {
            return "DELETE FROM topic WHERE " + champ + " = " + ID;
        }

        /* categorie */
        public static string deleteCategorie(int IDCategorie)
        {
            return "DELETE FROM categorie WHERE IDCategorie = " + IDCategorie;
        }

        /* favoris */
        public static string deleteFavoris(string champ, int ID)
        {
            return "DELETE FROM favoris WHERE " + champ + " = " + ID;
        }
    }
}
