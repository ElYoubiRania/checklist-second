using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Checklist
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GetConnectionStringByName("MaConnexion");
            

        }

        static string GetConnectionStringByName(string name)
        {
            
            string returnValue = null;

            // Recherchez le nom dans la section connectionStrings.

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["MaConnexion"];

            //Si trouvé, retourne la chaîne de connexion et lance la forme.

            if (settings != null)
            {

                try
                {
                    returnValue = settings.ConnectionString;
                    Application.Run(new Form1());
                }
                catch(Exception)
                {
                    Console.WriteLine();
                }
            }
            //Sinon retourne un Message d'erreur

            else
            {
                MessageBox.Show("veuillez créer un fichier de configuration");
            }

            return returnValue;
        }
    }

    
}
