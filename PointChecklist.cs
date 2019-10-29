using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checklist
{
    public class PointChecklist
    {
        public int IDPoint;
        public string titre;
        public string description;
        public string numTicket;
        public string client;
        public DateTime dateCreation;
        public string userCreation;
        public int IDChecklist;
        public bool entwickler;
        public bool kunde;
        public bool berater;
        public bool technik;
        public Panel panelPoint;

        public PointChecklist(Panel panelPoint, string titre, string description, DateTime dateCreation)
        {
            this.panelPoint = panelPoint;
            this.titre = titre;
            this.description = description;
            this.dateCreation = dateCreation;
        }

        public PointChecklist(string titre, string description, DateTime dateCreation,
            bool entwickler, bool kunde, bool berater, bool technik)
        {
            this.titre = titre;
            this.description = description;
            this.dateCreation = dateCreation;
            this.entwickler = entwickler;
            this.kunde = kunde;
            this.berater = berater;
            this.technik = technik;
        }

        public PointChecklist(int IDPoint, string titre, string description, string numTicket, string client, DateTime dateCreation, string userCreation, int IDChecklist,
            bool entwickler, bool kunde, bool berater, bool technik)
        {
            this.IDPoint = IDPoint;
            this.titre = titre;
            this.description = description;
            this.numTicket = numTicket;
            this.client = client;
            this.dateCreation = dateCreation;
            this.userCreation = userCreation;
            this.IDChecklist = IDChecklist;
            this.entwickler = entwickler;
            this.kunde = kunde;
            this.berater = berater;
            this.technik = technik;
        }

    }
}
