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
    public class Request
    {

        public static MySqlConnection mysql;

        public static void connection(MySqlConnection mysqlco)
        {
            mysql = mysqlco;
        }

        public static void insertRequest(MySqlDataAdapter ad, string commande)
        {
            ad.InsertCommand = new MySqlCommand(commande, mysql);
            ad.InsertCommand.ExecuteNonQuery();
        }

        public static void updateRequest(MySqlDataAdapter ad, string commande)
        {
            ad.UpdateCommand = new MySqlCommand(commande, mysql);
            ad.UpdateCommand.ExecuteNonQuery();
        }

        public static void deleteRequest(MySqlDataAdapter ad, string commande)
        {
            ad.DeleteCommand = new MySqlCommand(commande, mysql);
            ad.DeleteCommand.ExecuteNonQuery();
        }

        /*
         * Renvoie le résultat d'une requête de recherche dans un DataSet
         */
        public static void useSelectRequest(MySqlDataAdapter ad, DataSet data, string commande)
        {
            try
            {
                ad.SelectCommand = new MySqlCommand(commande, mysql);
                ad.Fill(data);
            }
            catch (Exception)
            {
                Console.WriteLine("erreur de connexion");
            }
        }

        /*
         * Renvoie le résultat d'une requête de recherche dans une DataTable
         */
        public static void useSelectRequestTable(MySqlDataAdapter ad, DataTable table, string commande)
        {

            try
            {
                ad.SelectCommand = new MySqlCommand(commande, mysql);
                ad.Fill(table);
            }
            catch (Exception)
            {
                Console.WriteLine("erreur de connexion");
            }
            
        }

        /*
         * Renvoie le résultat de l'exécution d'une requête permettant de rechercher un point dans la base de données grâce à son ID
         */
        public static DataTable useRequestSelectPointByIDPoint(MySqlDataAdapter adapt, string IDPoint)
        {
            DataTable table = new DataTable(Constantes.points);
            string cmd = SQL.selectPointByIDPoint("*", IDPoint);
            adapt.SelectCommand = new MySqlCommand(cmd, mysql);
            adapt.Fill(table);
            return table;
        }

        /*
         * Insère le type d'utilisateur dans la base de données si le point contient ce type d'utilisateur
         */
        public static void insertType(MySqlDataAdapter ad, DataRow nouveau, int IDPoint, string name, int IDType)
        {
            if ((bool)nouveau[name])
            {
                string cmdType = SQL.insertTypePoint(IDType, IDPoint.ToString());
                ad.InsertCommand = new MySqlCommand(cmdType, mysql);
                ad.InsertCommand.ExecuteNonQuery();
            }
        }

        /*
         * Met à jour une caractéristique d'un point
         */
        public static void updatePoint(MySqlDataAdapter ad, Control control, DataTable points, ref int updates, int nature)
        {
            string cmd = string.Empty;
            if (nature == Constantes.iTitre)
            {
                cmd = SQL.updatePoint(control.Text, "Titre", points.Rows[0].ItemArray[0].ToString());
            }
            else if (nature == Constantes.iDescription)
            {
                cmd = SQL.updatePoint(control.Text, "Description", points.Rows[0].ItemArray[0].ToString());
            }
            ad.UpdateCommand = new MySqlCommand(cmd, mysql);
            ad.UpdateCommand.ExecuteNonQuery();
            updates = updates + 1;
        }

        /*
 * Insère le type d'utilisateur d'un point dans la base de données
 */
        public static void insertTypePoint(MySqlDataAdapter ad, string cmd, string IDPoint, int type)
        {
            cmd = SQL.insertTypePoint(type, IDPoint);
            ad.InsertCommand = new MySqlCommand(cmd, mysql);
            ad.InsertCommand.ExecuteNonQuery();
        }

        /*
         * Supprime le type d'utilisateur d'un point dans la base de données
         * nom : deleteTypesPoint déjà utilisé
         */
        public static void supprimerTypesPoint(MySqlDataAdapter ad, string cmd, string IDPoint, int type)
        {
            cmd = SQL.deleteOneTypePoint(type, IDPoint);
            ad.DeleteCommand = new MySqlCommand(cmd, mysql);
            ad.DeleteCommand.ExecuteNonQuery();
        }

        public static void deleteModif(MySqlDataAdapter ad, string nameTable, string nameID, string ID)
        {
            string deleteModif = "DELETE FROM " + nameTable + " WHERE " + nameID + " = " + ID;
            ad.DeleteCommand = new MySqlCommand(deleteModif, mysql);
            ad.DeleteCommand.ExecuteNonQuery();
        }

        public static void insertModifChecklist(MySqlDataAdapter ad, string nameTable, string IDModification, string nameID, int IDModif, string ID)
        {
            string modifChecklist = "INSERT INTO " + nameTable + " (" + IDModification + ", " + nameID + ") VALUES (" + IDModif + "," + ID + ")";
            ad.InsertCommand = new MySqlCommand(modifChecklist, mysql);
            ad.InsertCommand.ExecuteNonQuery();
        }

        public static void deleteTypesPoint(MySqlDataAdapter ad, int IDPoint)
        {
            string deleteTypePoint = SQL.deleteTypesPoint(IDPoint);
            ad.DeleteCommand = new MySqlCommand(deleteTypePoint, mysql);
            ad.DeleteCommand.ExecuteNonQuery();
        }

        public static void deletePoints(MySqlDataAdapter ad, int IDChecklist)
        {
            string deletePoint = SQL.deletePointsChecklist(IDChecklist);
            ad.DeleteCommand = new MySqlCommand(deletePoint, mysql);
            ad.DeleteCommand.ExecuteNonQuery();
        }

        public static void deleteOnePoint(MySqlDataAdapter ad, int IDPoint)
        {
            string cmdDelete = SQL.deletePoint(IDPoint);
            ad.DeleteCommand = new MySqlCommand(cmdDelete, mysql);
            ad.DeleteCommand.ExecuteNonQuery();
        }

        public static void deleteChecklists(MySqlDataAdapter ad, int IDTopic)
        {
            string deleteChecklist = SQL.deleteChecklistsTopic(IDTopic);
            ad.DeleteCommand = new MySqlCommand(deleteChecklist, mysql);
            ad.DeleteCommand.ExecuteNonQuery();
        }

        public static void deleteOneChecklist(MySqlDataAdapter ad, int IDChecklist)
        {
            string deleteChecklist = SQL.deleteChecklist(IDChecklist);
            ad.DeleteCommand = new MySqlCommand(deleteChecklist, mysql);
            ad.DeleteCommand.ExecuteNonQuery();
        }

        public static void deleteTopic(MySqlDataAdapter ad, int ID, string nameID)
        {
            string deleteTopic = SQL.deleteTopic(nameID, ID);
            ad.DeleteCommand = new MySqlCommand(deleteTopic, mysql);
            ad.DeleteCommand.ExecuteNonQuery();
        }

        public static void deleteFavorites(MySqlDataAdapter ad, int ID, string nameID)
        {
            string deleteFavoris = SQL.deleteFavoris(nameID, ID);
            ad.DeleteCommand = new MySqlCommand(deleteFavoris, mysql);
            ad.DeleteCommand.ExecuteNonQuery();
        }

        public static void deleteCategory(MySqlDataAdapter ad, int IDCategorie)
        {
            string deleteCategory = SQL.deleteCategorie(IDCategorie);
            ad.DeleteCommand = new MySqlCommand(deleteCategory, mysql);
            ad.DeleteCommand.ExecuteNonQuery();
        }

        public static void deleteModifChecklist(MySqlDataAdapter ad, int IDChecklist)
        {
            string cmdModif = SQL.deleteModifChecklist(IDChecklist);
            ad.DeleteCommand = new MySqlCommand(cmdModif, mysql);
            ad.DeleteCommand.ExecuteNonQuery();
        }

        public static void deleteModifPoint(MySqlDataAdapter ad, int IDPoint)
        {
            string cmdModifPoint = SQL.deleteModifPoint(IDPoint);
            ad.DeleteCommand = new MySqlCommand(cmdModifPoint, mysql);
            ad.DeleteCommand.ExecuteNonQuery();
        }

    }
}
