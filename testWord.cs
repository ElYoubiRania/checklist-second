using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace Checklist
{
    public partial class testWord : Form
    {
        public testWord()
        {
            InitializeComponent();
            testFichierWord();
        }

        private void testFichierWord()
        {
            Microsoft.Office.Interop.Word.Application msWord = new Microsoft.Office.Interop.Word.Application();
            msWord.Visible = true; // mettez cette variable à true si vous souhaitez visualiser les opérations.
            object missing = System.Reflection.Missing.Value;

            Microsoft.Office.Interop.Word.Document nvDoc;
            // Choisir le template
            // object templateName = @"proALPHA\proALPHA_Konzept.dot";
            object templateName = @"Test2.dotx";
            // Créer le document
            nvDoc = msWord.Documents.Add(ref templateName, ref missing, ref missing, ref missing);

            /*
            HashSet<string> hash = new HashSet<string>();
            foreach (Microsoft.Office.Interop.Word.FormField f in nvDoc.FormFields)
            {
                hash.Add(f.Name);
            }
            
            // Opérations 
            // Les champs de formulaire définis dans le modèle se nomment "Nom" et "Prenom".
            object field = "Text1";
            nvDoc.FormFields.get_Item(ref field).Result = "1";
            field = "Text2";
            nvDoc.FormFields.get_Item(ref field).Result = "2";
            */
            /*
            // Le texte que je transfère
            string monContenu = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl
				{\f0\fswiss\fcharset0 Arial;}{\f1\fnil\fcharset0 Arial;}
				{\f2\fmodern\fprq1\fcharset0 Courier;}}
				{\colortbl ;\red0\green0\blue0;}
				{\*\generator Msftedit 5.41.15.1503;}
				\cf1\b\i\f2 Le texte que je transfère\cf0\b0\i0\f0}";
            DataObject clipData = new DataObject(DataFormats.Rtf,monContenu);
            Clipboard.SetDataObject(clipData,false);
            */
            /*
            string monContenu = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl
				{\f0\fswiss\fcharset0 Arial;}{\f1\fnil\fcharset0 Arial;}
				{\f2\fmodern\fprq1\fcharset0 Courier;}}
				{\colortbl ;\red0\green0\blue0;}
				{\*\generator Msftedit 5.41.15.1503;}
				\cf1\b\i\f2 Le texte que je transfère\cf0\b0\i0\f0}";
            DataObject clipData = new DataObject(DataFormats.Rtf,monContenu);
            Clipboard.SetDataObject(clipData,false);
            msWord.Selection.Paste();
            */

            object field = "Text1";
            nvDoc.FormFields.get_Item(ref field).Result = "Checklist : ";

            Microsoft.Office.Interop.Word.Paragraph description = nvDoc.Content.Paragraphs.Add(ref missing);
            description.Range.Text = "Description";
            description.Range.InsertParagraphAfter();


            // Attribuer le nom
            object fileName = @"C:\Users\philippe_m\Documents\Projet M1\TestWord\test.doc";
            // Sauver le document
            nvDoc.SaveAs(ref fileName, ref missing, ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing);
            // Fermer le document
            nvDoc.Close(ref missing, ref missing, ref missing);

            msWord.Quit(ref missing, ref missing, ref missing);
        }
    }
}
