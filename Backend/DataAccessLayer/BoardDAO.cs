using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardDAO
    {
        private readonly int boardID;
        private readonly string name;
        private string owner;
        private int backlogLimit;
        private int inProgessLimit;
        private int doneLimit;
        //private LinkedList<string> members;
        


        internal int BoardID { get { return boardID; } }
        internal string Name { get { return name; } }
        internal string Owner
        {
            get { return owner; }
            set
            {
                if (IsPersist) { bc.Update(BoardID, OwnerCol, value); }
                owner = value;
            }
        }
        internal int BacklogLimit
        {
            get { return backlogLimit; }
            set {
                if (IsPersist) { bc.Update(BoardID, backlogCol, value); }
                backlogLimit = value;
            }
        }
        internal int InProgressLimit
        {
            get { return inProgessLimit; }
            set {
                if (IsPersist) { bc.Update(BoardID, inProgessCol, value); }
                inProgessLimit = value;
            }
        }
        internal int DoneLimite
        {
            get { return doneLimit; }
            set {
                if (IsPersist) { bc.Update(BoardID, doneCol, value); }
                doneLimit = value;
            }
        }


        internal readonly string IdCol = "Id";
        internal readonly string NameCol = "Name";
        internal readonly string OwnerCol = "Oner";
        internal readonly string backlogCol = "backlogLimit";
        internal readonly string inProgessCol = "inProgressLimit";
        internal readonly string doneCol = "doneLimit";
        private readonly BoardController bc;
        private readonly BoradUserController buc;
        private bool IsPersist = false;




        internal BoardDAO(int id,string owner, string name)
        {
            this.bc = new BoardController();
            this.buc = new BoradUserController();
            this.boardID = id;
            this.owner = owner;
            this.name = name;
            //this.members =  members;
            this.backlogLimit = -1;
            this.inProgessLimit = -1;
            this.doneLimit = -1;
            Persist();
        }
        internal BoardDAO(SQLiteDataReader reader)
        {
            this.bc = new BoardController();
            this.buc= new BoradUserController();
            boardID = (int)reader.GetValue(0);
            name = reader.GetString(1);
            Owner = reader.GetString(2);
            BacklogLimit = (int)reader.GetValue(3);
            InProgressLimit = (int)reader.GetValue(4);
            DoneLimite = (int)reader.GetValue(5);
            
            IsPersist = true;
        }
        internal void JoinBoard(string email)
        {
            buc.Insert(BoardID, email);
        }

        internal List<string> GetMember()
        {
            return buc.GetAllMembers(BoardID);
        }
        internal void LeaveBoard(string email) 
        {
            buc.Delete(BoardID, email);
        }
        internal void DeleteBoard()
        {
            bc.Delete(BoardID);
            buc.Delete(BoardID);
        }
        internal void Persist()
        {
            bc.Insert(this);
            IsPersist = true;
        }

    }
}
