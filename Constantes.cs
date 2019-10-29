using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklist
{
    public class Constantes
    {
        public static Color turquoise = Color.FromArgb(74, 217, 217);
        public static Color pink = Color.FromArgb(242, 56, 90);
        public static Color orange = Color.FromArgb(245, 165, 3);
        public static Color deepBlue = Color.FromArgb(23, 45, 68);
        public static Color gray = Color.FromArgb(212, 213, 213);
        public static Color blue = Color.FromArgb(42, 63, 84);
        public static Color green = Color.FromArgb(90, 229, 0);
        public static Color bleuIconesMenu = Color.FromArgb(50, 81, 112);
        // public static Color bleuSurvol = Color.FromArgb(53,81,108);
        public static Color lightPink = Color.FromArgb(242, 87, 115);
        public static Color lightBlue = Color.FromArgb(59, 96, 133);
        public static Color lightGreen = Color.FromArgb(103, 255, 5);

        public static Color grayText = Color.FromArgb(242, 242, 242);
        public static SolidBrush grayBrush = new SolidBrush(grayText);
        public static Color grayCheck = Color.FromArgb(239, 240, 242);
        public static Color lightGray = Color.FromArgb(245, 246, 248);

        public static string temp = "tmp";

        public static string topic = "topic";
        public static string categorie = "categorie";
        public static string filtres = "filtres";
        public static string checklist = "checklist";
        public static string point = "point";

        public static string autre = "autre";

        public static string entwickler = "Entwickler";
        public static string berater = "Berater";
        public static string technik = "Technik";
        public static string kunde = "Kunde";

        // fonction ou constantes ?
        public static string typeUser(string user)
        {
            return "type" + user;
        }

        public static string typePoint(string user)
        {
            return "point" + user;
        }

        public static string lettreEntwickler = "E";
        public static string lettreBerater = "B";
        public static string lettreTechnik = "T";
        public static string lettreKunde = "K";

        public static string textBoxTitrePoint = "TextBoxTitre";
        public static string textBoxDescriptionPoint = "TextBoxDescription";
        public static string textBoxTitreChecklist = "titreChecklist";
        public static string textBoxDescriptionChecklist = "shortDesc";
        public static string labelTitrePoint = "TitrePoint";
        public static string labelDescriptionPoint = "DescriptionPoint";
        public static string labelTitreChecklist = "TitreChecklist";
        public static string labelDescriptionChecklist = "DescriptionChecklist";
        public static string textDescriptionChecklist = "Beschreibung von die Checklist";

        // faire des constantes ou des fonctions ?
        // pour les types des points
        public static string pointEntwickler = "pointEntwickler";
        public static string pointBerater = "pointBerater";
        public static string pointTechnik = "pointTechnik";
        public static string pointKunde = "pointKunde";

        // pour les filtres
        public static string typeEntwickler = "typeEntwickler";
        public static string typeBerater = "typeBerater";
        public static string typeTechnik = "typeTechnik";
        public static string typeKunde = "typeKunde";

        public static string panelPoint = "panelPoint-";
        public static string panelChecklist = "panelChecklist-";
        public static string listViewItem = "item-";

        public static string typesPoints = "typesPoints";

        public static string points = "points";
        public static string deletePoint = "deletePoint";

        public static string addCategory = "addCategory";
        public static string addTopic = "addTopic";
        public static string addChecklist = "addChecklist";
        public static string changeCategory = "changeCategory";
        public static string changeTopic = "changeTopic";

        public static int distanceEntreDeuxPoints = 82;
        public static int locationYPremierPoint = 51;

        public static int locationYPremiereChecklist = 120; // 75;

        public static string modifCategorie = "Die Kategorie ändern";
        public static string supprCategorie = "Die Kategorie löschen";
        public static string ajoutTopic = "Ein Topic hinzufügen";

        public static string modifTopic = "Das Topic ändern";
        public static string supprTopic = "Das Topic löschen";
        public static string ajoutChecklist = "Eine Checklist hinzufügen";
        public static string ajoutCategorie = "Eine Kategorie hinzufügen";

        public static string types = "types";
        public static string favoris = "Favoris";

        public static string formatDate = "dd/MM/yyyy";
        public static string formatDate2 = "yyyy-MM-dd";

        public static int iTitre = 1;
        public static int iDescription = 2;

        public static string description = "description";
        public static string infoPoint = "InfoPoint";
    }
}
