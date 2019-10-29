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
    public class DB
    {
        /*
         * Parcourt le Panel contenant les points, pour voir si des points ont été modifiés
         */
        public static bool browsePanelPointsModif(MySqlDataAdapter ad, DataSet data, DataTable points, string cmd, string IDPoint,
            MySqlConnection mysql, Panel panelPoints, ChecklistRepo check, DataSet typesUser)
        {
            int updates = 0;
            foreach (Control obj in panelPoints.Controls)
            {
                // Si le panel n'est pas un type d'utilisateur
                // if (pan.GetType() == typeof(Panel) && pan.Size != new Size(23,23))
                // modifier le "panelPoint-tmp"
                if (obj.GetType() == typeof(Panel) && obj.Name.StartsWith(Constantes.panelPoint) && obj.Name != Tools.namePanelPointTemp())
                {
                    updates = 0;
                    points.Clear();
                    IDPoint = Tools.returnID(obj.Name);

                    Request.useSelectRequestTable(ad, data.Tables[Constantes.points], SQL.selectPointByIDPoint("*", IDPoint));
                    data.Tables[Constantes.types].Clear();
                    Request.useSelectRequestTable(ad, data.Tables[Constantes.types], SQL.selectTypesByIDPoint(IDPoint));

                    data.Tables[Constantes.types].PrimaryKey = new DataColumn[] { data.Tables[Constantes.types].Columns[0], };
                    foreach (Control control in obj.Controls)
                    {
                        if (control.Name == Constantes.textBoxTitrePoint)
                        {
                            if (control.Text != points.Rows[0].ItemArray[1].ToString())
                            {
                                if (control.Text.Length == 0 || Tools.isPlaceHolder((TextBox)control))
                                {
                                    if (Tools.emptyTitle(Constantes.point) == false)
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    if (DB.verifyDuplicatePoint(ad, check.IDChecklist, control.Text) == false)
                                    {
                                        control.Focus();
                                        return false;
                                    }
                                    Request.updatePoint(ad, control, points, ref updates, Constantes.iTitre);
                                }
                            }
                        }
                        else if (control.Name == Constantes.textBoxDescriptionPoint)
                        {
                            if (Tools.isPlaceHolder((TextBox)control))
                            {
                                control.Text = string.Empty;
                            }
                            if (control.Text != points.Rows[0].ItemArray[2].ToString())
                            {
                                Request.updatePoint(ad, control, points, ref updates, Constantes.iDescription);
                            }
                        }
                        else if (control.Name.StartsWith(Constantes.pointEntwickler))
                        {
                            DB.updateTypePoint(ad, data, cmd, IDPoint, 1, Constantes.entwickler, ref updates, typesUser);
                        }
                        else if (control.Name.StartsWith(Constantes.pointBerater))
                        {
                            DB.updateTypePoint(ad, data, cmd, IDPoint, 2, Constantes.berater, ref updates, typesUser);
                        }
                        else if (control.Name.StartsWith(Constantes.pointTechnik))
                        {
                            DB.updateTypePoint(ad, data, cmd, IDPoint, 3, Constantes.technik, ref updates, typesUser);
                        }
                        else if (control.Name.StartsWith(Constantes.pointKunde))
                        {
                            DB.updateTypePoint(ad, data, cmd, IDPoint, 4, Constantes.kunde, ref updates, typesUser);
                        }
                    }
                    if (updates > 0)
                    {
                        DB.dateModif(ad, false, points.Rows[0].ItemArray[0].ToString(), mysql);
                    }
                }
            }
            return true;
        }

        /*
         * Insère les types d'utilisateur du point dans la base de données
         */
        public static void insertTypesPoints(MySqlDataAdapter ad, int IDPoint2, DataSet typesUser)
        {
            DataRow nouveau = typesUser.Tables[Constantes.typesPoints].Rows.Find(Constantes.temp);
            Request.insertType(ad, nouveau, IDPoint2, Constantes.entwickler, 1);
            Request.insertType(ad, nouveau, IDPoint2, Constantes.berater, 2);
            Request.insertType(ad, nouveau, IDPoint2, Constantes.technik, 3);
            Request.insertType(ad, nouveau, IDPoint2, Constantes.kunde, 4);
        }

        public static void supprimerChecklistsDUnTopic(MySqlDataAdapter ad, DataTable dChecklist, DataTable dPoint, int IDTopic, int IDChecklist, int IDPoint)
        {
            Request.useSelectRequestTable(ad, dChecklist, SQL.selectChecklistsByIDTopic("IDChecklist", IDTopic.ToString()));

            foreach (DataRow itemCheck in dChecklist.Rows)
            {
                IDChecklist = Int32.Parse(itemCheck.ItemArray[0].ToString());
                Request.useSelectRequestTable(ad, dPoint, SQL.selectPointsByIDChecklist("IDPoint", IDChecklist.ToString()));

                foreach (DataRow itemPoint in dPoint.Rows)
                {
                    IDPoint = Int32.Parse(itemPoint.ItemArray[0].ToString());
                    Request.deleteTypesPoint(ad, IDPoint);
                    Request.deleteModifPoint(ad, IDPoint);
                }
                Request.deletePoints(ad, IDChecklist);
                dPoint.Rows.Clear();

                // On devrait vérifier s'il y a encore des modifications avec cet ID
                Request.deleteModifChecklist(ad, IDChecklist);
            }
            Request.deleteChecklists(ad, IDTopic);
            dChecklist.Rows.Clear();
        }

        /*
         * Vérifie s'il y a des doublons pour une catégorie
         */
        public static bool verifyDuplicateCategory(MySqlDataAdapter ad, string title)
        {
            DataTable tCategories = new DataTable();

            Request.useSelectRequestTable(ad, tCategories, SQL.selectCategoryByTitle("*", title));
            /*
            string cmdCategorie = Requetes.selectCategoryByTitle("*",title);
            ad.SelectCommand = new MySqlCommand(cmdCategorie,mysql);
            ad.Fill(tCategories); */
            if (tCategories.Rows.Count > 0)
            {
                DialogResult result;
                result = MessageBox.Show("Es ist schon eine Kategorie mit dem selben Titel. Könnten Sie der Titel ändern ?", "Duplikat", MessageBoxButtons.OK);
                if (result == DialogResult.OK)
                {
                    return false;
                }
            }
            return true;
        }

        /*
         * Vérifie s'il y a des doublons pour un topic, c'est-à-dire si un topic de la même catégorie a le même titre
         */
        public static bool verifyDuplicateTopic(MySqlDataAdapter ad, int idCategorie, string titleTopic)
        {
            // Vérifier qu'il n'y ait pas de doublons
            DataTable tTopics = new DataTable();

            Request.useSelectRequestTable(ad, tTopics, SQL.selectTopicByTitle("*", titleTopic, idCategorie.ToString()));

            if (tTopics.Rows.Count > 0)
            {
                DialogResult result;
                result = MessageBox.Show("Es ist schon ein Topic mit dem selben Titel in die Kategorie. Könnten Sie der Titel ändern ?", "Duplikat", MessageBoxButtons.OK);
                if (result == DialogResult.OK)
                {
                    return false;
                }
            }
            return true;
        }

        /*
         * Vérifie s'il y a des doublons pour une checklist, c'est-à-dire si une checklist du même topic a le même titre
         */
        public static bool verifyDuplicateChecklist(MySqlDataAdapter ad, int idTopic, string titreChecklist)
        {
            // Vérifier qu'il n'y ait pas de doublons
            DataTable tChecklists = new DataTable();

            Request.useSelectRequestTable(ad, tChecklists, SQL.selectChecklistByTitle(idTopic, titreChecklist));

            if (tChecklists.Rows.Count > 0)
            {
                DialogResult result;
                result = MessageBox.Show("Es ist schon eine Checklist mit dem selben Titel in das Topic. Könnten Sie der Titel ändern ?", "Duplikat", MessageBoxButtons.OK);
                if (result == DialogResult.OK)
                {
                    return false;
                }
                /* else
                 {
                     // création : ne plus afficher la checklist qu'on voulait créer comme on ne la crée plus
                     // idem pour la modif
                 } */
            }
            return true;
        }

        /*
         * Vérifie s'il y a des doublons pour un point, c'est-à-dire si un point de la même checklist a le même titre
         */
        public static bool verifyDuplicatePoint(MySqlDataAdapter ad, int IDChecklist, string titrePoint)
        {
            // Vérifier qu'il n'y ait pas de doublons
            DataTable tPoints = new DataTable();

            Request.useSelectRequestTable(ad, tPoints, SQL.selectPointByTitle(IDChecklist, titrePoint));

            if (tPoints.Rows.Count > 0)
            {
                DialogResult result;
                // dire de quel point il s'agit
                result = MessageBox.Show("Es ist schon einen Punkt mit dem selben Titel in die Checklist. Könnten Sie der Titel ändern ?", "Duplikat", MessageBoxButtons.OK);
                if (result == DialogResult.OK)
                {
                    return false;
                }
            }
            return true;
        }

        public static void verificationModifsToDelete()
        {
            MySqlDataAdapter ad = new MySqlDataAdapter();
            DataTable tableModifs = new DataTable();

            Request.useSelectRequestTable(ad, tableModifs, SQL.selectAllModifications());

            DataTable tableVerif = new DataTable();
            foreach (DataRow modif in tableModifs.Rows)
            {
                string IDModif = modif[0].ToString();
                tableVerif.Columns.Clear();
                tableVerif.Rows.Clear();
                tableVerif.Clear();

                Request.useSelectRequestTable(ad, tableVerif, SQL.selectModifChecklistByIDModif(IDModif));

                if (tableVerif.Rows.Count == 0)
                {
                    tableVerif.Columns.Clear();
                    tableVerif.Rows.Clear();
                    tableVerif.Clear();

                    Request.useSelectRequestTable(ad, tableVerif, SQL.selectModifPointByIDModif(IDModif));

                    if (tableVerif.Rows.Count == 0)
                    {
                        Request.deleteRequest(ad, SQL.deleteModification(IDModif));
                    }
                }
            }
        }

        /*
         * Modifie le type d'utilisateur d'un point
         */
        public static void updateTypePoint(MySqlDataAdapter ad, DataSet data, string cmd, string IDPoint, int type, string nameType, ref int updates, DataSet typesUser)
        {
            if (data.Tables[Constantes.types].Rows.Contains(type) && (bool)typesUser.Tables[Constantes.typesPoints].Rows.Find(IDPoint)[nameType] == false)
            {
                Request.supprimerTypesPoint(ad, cmd, IDPoint, type);
                updates = updates + 1;
            }
            else if (data.Tables[Constantes.types].Rows.Contains(type) == false && (bool)typesUser.Tables[Constantes.typesPoints].Rows.Find(IDPoint)[nameType])
            {
                Request.insertTypePoint(ad, cmd, IDPoint, type);
                updates = updates + 1;
            }
        }

        // rajouter string utilisateur
        public static void dateModif(MySqlDataAdapter ad, bool checklist, string ID, MySqlConnection mysql)
        {
            string nameTable = string.Empty;
            string nameID = string.Empty;
            string IDModification = string.Empty;
            int IDModif = 0;
            if (checklist)
            {
                nameTable = "modif_checklist";
                nameID = "IDCheck";
                IDModification = "IDModification";
            }
            else
            {
                nameTable = "modif_point";
                nameID = "IDPoint";
                IDModification = "IDModification1";
            }
            // problème s'il y a deux modifications le même jour
            DataSet modification = new DataSet();
            modification.Tables.Add("Modification");
            modification.Tables.Add("Lien");

            Request.useSelectRequestTable(ad, modification.Tables["Modification"], SQL.selectModification());

            // S'il y a déjà une modification le même jour avec le même utilisateur
            // Il faudrait récupérer l'ID et vérifier si le lien existe
            // si oui, ne rien faire
            // si non, récupérer l'ID et créer juste le lien
            // Si non, créer la modif puis le lien
            if (modification.Tables["Modification"].Rows.Count > 0)
            {
                IDModif = Int32.Parse(modification.Tables["Modification"].Rows[0][0].ToString());

                string rechercheID = "SELECT * FROM " + nameTable + " WHERE " + IDModification + " = " + IDModif + " AND " + nameID + " = " + ID;
                Request.useSelectRequestTable(ad, modification.Tables["Lien"], rechercheID);
                /*
                ad.SelectCommand = new MySqlCommand(rechercheID,mysql);
                ad.Fill(modification.Tables["Lien"]); */
                if (modification.Tables["Lien"].Rows.Count > 0)
                {
                    return;
                }
            }
            else
            {
                string modif = SQL.insertModificationToday();
                ad.InsertCommand = new MySqlCommand(modif, mysql);
                ad.InsertCommand.ExecuteNonQuery();
                IDModif = Int32.Parse(ad.InsertCommand.LastInsertedId.ToString());
            }
            // IDPoint est unique donc il faut d'abord supprimer la modif (s'il y en a une) avant d'en recréer une nouvelle
            modification.Tables.Add("VerifID");
            Request.useSelectRequestTable(ad, modification.Tables["VerifID"], SQL.selectAll(nameTable, nameID, ID));

            if (modification.Tables["VerifID"].Rows.Count > 0)
            {
                Request.deleteModif(ad, nameTable, nameID, ID);
            }
            Request.insertModifChecklist(ad, nameTable, IDModification, nameID, IDModif, ID);
        }

    }
}
