using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checklist
{
    public static class ExportWord
    {
        public static void testFichierWord(string titre, string desc, Panel panel, DataTable types)
        {
            Microsoft.Office.Interop.Word.Application msWord = new Microsoft.Office.Interop.Word.Application();
            msWord.Visible = false; // mettez cette variable à true si vous souhaitez visualiser les opérations.
            object missing = System.Reflection.Missing.Value;

            Microsoft.Office.Interop.Word.Document nvDoc;
            // Choisir le template
            // object templateName = @"proALPHA\proALPHA_Konzept.dot";
            object templateName = @"Test3.dotx";
            // Créer le document
            nvDoc = msWord.Documents.Add(ref templateName, ref missing, ref missing, ref missing);

            object field = "Text1";
            nvDoc.FormFields.get_Item(ref field).Result = "Checklist : " + titre;

            Microsoft.Office.Interop.Word.Paragraph description = nvDoc.Content.Paragraphs.Add(ref missing);
            object style3 = "Schwache Hervorhebung";
            description.Range.set_Style(style3);
            description.Range.Font.Size = 12;
            description.Range.Text = "Beschreibung : " + desc + Environment.NewLine;
            description.Range.InsertParagraphAfter();

            string titrePoint = string.Empty;
            string descPoint = string.Empty;
            string typesPoint = string.Empty;


            Microsoft.Office.Interop.Word.Paragraph liste = nvDoc.Content.Paragraphs.Add(ref missing);
            object style5 = "Buchtitel";
            liste.Range.set_Style(style5);
            liste.Range.Font.Size = 13;
            liste.Range.Text = "Punkte Liste : " + Environment.NewLine;
            liste.Range.InsertParagraphAfter();

            string IDPoint = string.Empty;

            foreach (Control control in panel.Controls)
            {
                if (control.GetType() == typeof(Panel) && control.Name.StartsWith(Constantes.panelPoint))
                {
                    typesPoint = string.Empty;
                    IDPoint = Tools.returnID(control.Name);
                    DataRow row = types.Rows.Find(IDPoint);
                    foreach (Control point in control.Controls)
                    {
                        if (point.Name == Constantes.labelTitrePoint)
                        {
                            titrePoint = point.Text;
                        }
                        else if (point.Name == Constantes.labelDescriptionPoint)
                        {
                            descPoint = point.Text;
                        }
                        else if (point.Name.StartsWith(Constantes.pointEntwickler))
                        {
                            if ((bool)types.Rows.Find(IDPoint)[Constantes.entwickler])
                            {
                                if (typesPoint != string.Empty)
                                {
                                    typesPoint += " - ";
                                }
                                typesPoint += "Entwickler";
                            }
                        }
                        else if (point.Name.StartsWith(Constantes.pointBerater))
                        {
                            if ((bool)types.Rows.Find(IDPoint)[Constantes.berater])
                            {
                                if (typesPoint != string.Empty)
                                {
                                    typesPoint += " - ";
                                }
                                typesPoint += "Berater";
                            }
                        }
                        else if (point.Name.StartsWith(Constantes.pointTechnik))
                        {
                            if ((bool)types.Rows.Find(IDPoint)[Constantes.technik])
                            {
                                if (typesPoint != string.Empty)
                                {
                                    typesPoint += " - ";
                                }
                                typesPoint += "Technik";
                            }
                        }
                        else if (point.Name.StartsWith(Constantes.pointKunde))
                        {
                            if ((bool)types.Rows.Find(IDPoint)[Constantes.kunde])
                            {
                                if (typesPoint != string.Empty)
                                {
                                    typesPoint += " - ";
                                }
                                typesPoint += "Kunde";
                            }
                        }
                    }
                    Microsoft.Office.Interop.Word.Paragraph tPoint = nvDoc.Content.Paragraphs.Add(ref missing);
                    object style1 = "Intensive Hervorhebung";
                    tPoint.Range.set_Style(style1);
                    tPoint.Range.Font.Size = 14;
                    tPoint.Range.Text = titrePoint;
                    tPoint.Range.InsertParagraphAfter();

                    Microsoft.Office.Interop.Word.Paragraph tUserPoint = nvDoc.Content.Paragraphs.Add(ref missing);
                    object style4 = "Intensives Zitat";
                    tUserPoint.Range.set_Style(style4);
                    tUserPoint.Range.Font.Size = 10;
                    tUserPoint.Range.Font.Bold = 0;
                    tUserPoint.Range.Font.Italic = 1;

                    tUserPoint.Range.Text = typesPoint;
                    tUserPoint.Range.InsertParagraphAfter();

                    Microsoft.Office.Interop.Word.Paragraph dPoint = nvDoc.Content.Paragraphs.Add(ref missing);
                    object style2 = "Hervorhebung";
                    dPoint.Range.set_Style(style2);
                    dPoint.Range.Font.Size = 11;
                    dPoint.Range.Text = descPoint;
                    dPoint.Range.InsertParagraphAfter();
                }
            }

            // Attribuer le nom
            object fileName = @"C:\Users\philippe_m\Documents\Projet M1\TestWord\test.doc";
            // Sauver le document
            nvDoc.SaveAs(ref fileName, ref missing, ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing);

            // Fermer le document
            // nvDoc.Close(ref missing,ref missing,ref missing);

            msWord.Documents.Open(ref fileName, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

            // msWord.Quit(ref missing,ref missing,ref missing);
        }

        private static void quitterWord(Microsoft.Office.Interop.Word.Application msWord, Microsoft.Office.Interop.Word.Document nvDoc, object missing)
        {
            nvDoc.Close(ref missing, ref missing, ref missing);
            msWord.Quit(ref missing, ref missing, ref missing);
        }
    }
}
