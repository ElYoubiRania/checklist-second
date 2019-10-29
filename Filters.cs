using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklist
{
    /* Different methods for the filters management */
    public class Filters
    {
        /* Initialisation d'un DataSet contenant les types d'utilisateur
         * Table "filtres" : contient les valeurs des types d'utilisateur qui permettent de filtrer les points
         * Table "typesPoints" : contient les valeurs des types d'utilisateur pour chaque point de la checklist
         */
        public static void InitialiseUserType(DataSet typesUser)
        {
            typesUser.Tables.Add(Constantes.filtres);
            typesUser.Tables[Constantes.filtres].Columns.Add("Type", typeof(string));
            typesUser.Tables[Constantes.filtres].Columns.Add("Etat", typeof(bool));
            typesUser.Tables[Constantes.filtres].Rows.Add(Constantes.entwickler, false);
            typesUser.Tables[Constantes.filtres].Rows.Add(Constantes.berater, false);
            typesUser.Tables[Constantes.filtres].Rows.Add(Constantes.technik, false);
            typesUser.Tables[Constantes.filtres].Rows.Add(Constantes.kunde, false);
            typesUser.Tables.Add(Constantes.typesPoints);

            typesUser.Tables[Constantes.typesPoints].Columns.Add("IDPoint", typeof(string));
            typesUser.Tables[Constantes.typesPoints].Columns.Add(Constantes.entwickler, typeof(bool));
            typesUser.Tables[Constantes.typesPoints].Columns.Add(Constantes.berater, typeof(bool));
            typesUser.Tables[Constantes.typesPoints].Columns.Add(Constantes.technik, typeof(bool));
            typesUser.Tables[Constantes.typesPoints].Columns.Add(Constantes.kunde, typeof(bool));

            typesUser.Tables[Constantes.typesPoints].PrimaryKey = new DataColumn[] { typesUser.Tables[Constantes.typesPoints].Columns["IDPoint"] };
        }

        // A tester
        // Voir avec Rows si on peut mettre autre chose qu'un chiffre pour eviter de se tromper si ça change
        public static void verifySelectedType(MySqlDataAdapter ad, DataSet filter, int type, string cmd, DataSet typesUser, int IDChecklist)
        {
            if ((bool)typesUser.Tables[Constantes.filtres].Rows[type - 1][1])
            {
                filter.Tables.Add(type.ToString());
                Request.useSelectRequestTable(ad, filter.Tables[type.ToString()], SQL.selectPointsType(IDChecklist, type));
            }
        }

        /*
         * Remet les filtres à zéro
         */
        public static void restoreFilters(DataSet typesUser)
        {
            typesUser.Tables[Constantes.filtres].Rows[0][1] = false;
            typesUser.Tables[Constantes.filtres].Rows[1][1] = false;
            typesUser.Tables[Constantes.filtres].Rows[2][1] = false;
            typesUser.Tables[Constantes.filtres].Rows[3][1] = false;
        }
    }
}
