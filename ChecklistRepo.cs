using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklist
{

    public class ChecklistRepo
    {
        public int IDChecklist;
        public string titre;
        public string description;
        public DateTime dateCreation;
        public string userCreation;
        public int IDTopic;

        public ChecklistRepo(int IDChecklist, string titre, string description, DateTime dateCreation, string userCreation, int IDTopic)
        {
            this.IDChecklist = IDChecklist;
            this.titre = titre;
            this.description = description;
            this.dateCreation = dateCreation;
            this.userCreation = userCreation;
            this.IDTopic = IDTopic;
        }

        public ChecklistRepo(DataRow row)
        {
            this.IDChecklist = Int32.Parse(row[0].ToString());
            this.titre = row[1].ToString();
            this.description = row[2].ToString();
            this.dateCreation = DateTime.Parse(row[3].ToString());
            this.userCreation = row[4].ToString();
            this.IDTopic = Int32.Parse(row[5].ToString());
        }

        public ChecklistRepo(int IDChecklist, string titre)
        {
            this.IDChecklist = IDChecklist;
            this.titre = titre;
        }
    }
}
