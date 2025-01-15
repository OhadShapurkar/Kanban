using System;
using System.Runtime.CompilerServices;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;
using System.Data;
using System.Threading.Tasks;



namespace BackendTests
{
    internal class Tests
    {
        private readonly ServiceFactory _sf;
        private readonly UserService _us;
        private readonly BoardService _bs;
        private readonly TaskService _ts;

        public Tests()
        {
            _sf = new ServiceFactory();
            _us = _sf._us;
            _bs = _sf._bs;
            _ts = _sf._ts;

        }
        /// <summary>
        /// This function run all the tests which tests all requirements
        /// </summary>

        public void RunAllTests()
        {
            _sf.DeleteData();
            TestRegisteration();
            TestLogin();
            TestLogout();
            TestCreateBoard();
            TestDeleteBoard();
            TestcreateTask();
            TestChangeLimit();
            TestEditsTaskTitel();
            TestEditsTaskDescription();
            TestEditsTaskDueDate();
            Testinprogresstask();
            TestGetCoulmnLimit();
            TestMoveTask();
            TestgetcolumnName();
            TestGetColumn();
            TestGetUserBoards();
            TestGetBoardName();
            TestJoinBoard();
            TestLeaveBoard();
            TestAssignTask();
            TestTransferOwnership();
            _sf.DeleteData();

        }

        /// <summary>
        /// This function test registeration to the system which tests successful registeration
        /// registeration with a weak password, registeration with invalid email, registertaion 
        /// with details of user who is already registered.
        /// </summary>
        public void TestRegisteration()
        {
            var valemail = "register@gmail.com";
            var invemail = "invalid-mail.com";
            var valpassword = "Pass123";
            var invpassword = "passw0rd";
            var result = _us.Register(valemail, valpassword);
            var response = JsonSerializer.Deserialize<Response>(result);

            if (response.ErrorMessage == null)
            {
                Console.WriteLine("Test Registeration Success: Passed");
            }
            else
            {
                Console.WriteLine($"Test Registeration Success: Failed - {response.ErrorMessage}");
            }

            _us.Logout(valemail);

            var result2 = _us.Register(valemail, invpassword);

            var response2 = JsonSerializer.Deserialize<Response>(result2);

            if (response2.ErrorMessage != null)
            {
                Console.WriteLine("Test Registeration Invalid Password Passed");
            }
            else
            {
                Console.WriteLine("Test Registeration Invalid Password Failed:  registeration should fail with weak password");
            }


            //invalid email
            var result3 = _us.Register(invemail, valpassword);




            var response3 = JsonSerializer.Deserialize<Response>(result3);

            if (response3.ErrorMessage != null)
            {
                Console.WriteLine("Test Registeration Invalid Email: Passed");
            }
            else
            {
                Console.WriteLine("Test Registeration Invalid Email Failed: registeration should fail with invalid email");
            }


            //already registered
            var result4 = _us.Register(valemail, valpassword);


            var response4 = JsonSerializer.Deserialize<Response>(result4);

            if (response4.ErrorMessage != null)
            {
                Console.WriteLine("Test Registeration already registered user: Passed");
            }
            else
            {
                Console.WriteLine("Test Registeration already registered user Failed: registeration should fail with registered user details");
            }
            _us.Logout(valemail);
            Console.WriteLine();
        }

        /// <summary>
        /// This function test login to the system which includes successful login, two login attemps,
        /// invalid/not register user information
        /// </summary>
        public void TestLogin()
        {
            var email = "login@gmail.com";
            var password = "Pass123";
            var nonExitEmail = "nonexistent@gmail.com";
            //initilize user - register user
            _us.Register(email, password);
            _us.Logout(email);
            //

            var result = _us.Login(email, password);
            var response = JsonSerializer.Deserialize<Response>(result);

            if (response.ErrorMessage == null)
            {
                Console.WriteLine("Test Login Success Passed");
            }
            else
            {
                Console.WriteLine($"Test Login Success Failed: {response.ErrorMessage}");
            }

            //test two attemps login

            var result2 = _us.Login(email, password);
            var response2 = JsonSerializer.Deserialize<Response>(result2);

            if (response2.ErrorMessage != null)
            {
                Console.WriteLine("Test Login Two Attemps Passed");
            }
            else
            {
                Console.WriteLine("Test Login Two Attemps Passed Failed: user already logged in and should be able to login again");
            }

            //test login invalid

            var result3 = _us.Login(nonExitEmail, password);
            var response3 = JsonSerializer.Deserialize<Response>(result3);

            if (response3.ErrorMessage != null)
            {
                Console.WriteLine("Test Login Invalid Passed");
            }
            else
            {
                Console.WriteLine("Test Login Invalid Failed: login should be fail with unkown user");
            }
            _us.Logout(email);
            Console.WriteLine();
        }


        /// <summary>
        /// This function test logout from the system which includes successful logout, logout not connected user
        /// </summary>

        public void TestLogout()
        {
            var email = "logout@gmail.com";
            var password = "Pass123";
            //initilize user - register user
            _us.Register(email, password);
            //
            var result = _us.Logout(email);
            var respone = JsonSerializer.Deserialize<Response>(result);

            if (respone.ErrorMessage == null)
            {
                Console.WriteLine("Test Logout Success Passed");
            }
            else
            {
                Console.WriteLine($"Test Logout Success Failed: {respone.ErrorMessage}");
            }


            //user is not connected
            var result2 = _us.Logout(email);



            var respone2 = JsonSerializer.Deserialize<Response>(result2);

            if (respone2.ErrorMessage != null)
            {
                Console.WriteLine("Test Logout not logged in Passed");
            }
            else
            {
                Console.WriteLine("Test Logout not logged in Failed: unlogged user should not be able to disconnect");
            }
            Console.WriteLine();
        }


        /// <summary>
        /// This function tests board creation which includes successful board creation, board creation with a board name
        /// which is already created by the user
        /// </summary>

        public void TestCreateBoard()
        {
            var email = "createboard@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            //initilize user - register user
            _us.Register(email, password);
            //
            var result = _bs.CreateBoard(email, boardname);
            var respone = JsonSerializer.Deserialize<Response>(result);

            if (respone.ErrorMessage == null)
            {
                Console.WriteLine("Create Board Passed");
            }
            else
            {
                Console.WriteLine($"Create Board Failed: {respone.ErrorMessage}");
            }
            //double use of board name

            var result2 = _bs.CreateBoard(email, boardname);
            var respone2 = JsonSerializer.Deserialize<Response>(result2);

            if (respone2.ErrorMessage != null)
            {
                Console.WriteLine("Create Board Twice Passed");
            }
            else
            {
                Console.WriteLine("Create Board Twice Failed: user should not be able to create two board with the same name");
            }

            _us.Logout(email);
            Console.WriteLine();

        }


        /// <summary>
        /// This function tests deletion of a board whihc includes successful delete and a deletion of a board which doesn't exits 
        /// </summary>

        public void TestDeleteBoard()
        {
            var email = "deleteboard@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            //initilize user - register user
            _us.Register(email, password);
            _bs.CreateBoard(email, boardname);
            //
            var result = _bs.DeleteBoard(email, boardname);
            var respone = JsonSerializer.Deserialize<Response>(result);

            if (respone.ErrorMessage == null)
            {
                Console.WriteLine("Delete Board Passed");
            }
            else
            {
                Console.WriteLine($"Delete Board Failed: {respone.ErrorMessage}");
            }

            //board doesnt exits
            var result2 = _bs.DeleteBoard(email, boardname);




            var respone2 = JsonSerializer.Deserialize<Response>(result2);

            if (respone2.ErrorMessage != null)
            {
                Console.WriteLine("Delete Board Twice Passed");
            }
            else
            {
                Console.WriteLine("Delete Board Twice Failed: user should not be able to delete two board that doesn't exit");
            }

            _us.Logout(email);
            Console.WriteLine();
        }
        /// <summary>
        /// This function test change limit of a column which includes successful change of limit, change of limit to unlimited,
        /// </summary>
        public void TestChangeLimit()
        {
            var email = "TestChangeLimit@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            //var taksID = 3;
            var title = "title";
            var description = "description";

            //initilize user - register user
            _us.Register(email, password);
            _bs.CreateBoard(email, boardname);
            _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));
            //

            var newLimit = 10;
            var col = 0;
            var result = _bs.ChangeLimit(email, boardname, col, newLimit);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("Change Limit Success Passed");
            }
            else
            {
                Console.WriteLine($"Change Limit Success Failed: {response.ErrorMessage}");
            }


            //change limit to unlimited 

            var newLimit2 = -1;
            var col2 = 0;
            var result2 = _bs.ChangeLimit(email, boardname, col2, newLimit2);
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage == null)
            {
                Console.WriteLine("Change Limit Unlimited Success Passed");
            }
            else
            {
                Console.WriteLine($"Change Limit Unlimited Success Failed: {response.ErrorMessage}");
            }



            //trying to change limit less than 2
            _ts.CreateTask(email, boardname, "title2", description, new DateTime(2024, 12, 15, 14, 30, 0));


            var newLimit7 = 1;
            var col7 = 0;
            var result7 = _bs.ChangeLimit(email, boardname, col7, newLimit7);
            var response7 = JsonSerializer.Deserialize<Response>(result7);
            if (response7.ErrorMessage != null)
            {
                Console.WriteLine("Change Less Than Tasks Passed");
            }
            else
            {
                Console.WriteLine("Change Less Than Tasks Failed: should'nt be able to change task limit less than tasks");
            }

            //change to negative number that is not -1
            var newLimit3 = -2;
            var col3 = 0;
            var result3 = _bs.ChangeLimit(email, boardname, col3, newLimit3);
            var response3 = JsonSerializer.Deserialize<Response>(result3);
            if (response3.ErrorMessage != null)
            {
                Console.WriteLine("Change Limit Negative -2 Passed");
            }
            else
            {
                Console.WriteLine("Change Limit Negative -2 Failed: Change limit to -2 should fail");
            }

            //change limit fake board

            var newLimit4 = 0;
            var col4 = 0;
            var result4 = _bs.ChangeLimit(email, boardname, col4, newLimit4);
            var response4 = JsonSerializer.Deserialize<Response>(result4);
            if (response4.ErrorMessage != null)
            {
                Console.WriteLine("Change Limit Board Doesn't Exits Passed");
            }
            else
            {
                Console.WriteLine("Change Limit Board Doesn't Exits Failed:  change limit to a board that doesnt exits should fail");
            }


            //false column
            var newLimit5 = 0;
            var col5 = 10;
            var result5 = _bs.ChangeLimit(email, boardname, col5, newLimit5);
            var response5 = JsonSerializer.Deserialize<Response>(result5);
            if (response5.ErrorMessage != null)
            {
                Console.WriteLine("Change Limit False Column Passed");
            }
            else
            {
                Console.WriteLine("Change Limit False Column Failed:  change limit to a false column should fake");
            }


            //false user

            var newLimit6 = 0;
            var col6 = 0;
            var email6 = "fakemail@gmail.com";
            var result6 = _bs.ChangeLimit(email6, boardname, col6, newLimit6);
            var response6 = JsonSerializer.Deserialize<Response>(result6);
            if (response6.ErrorMessage != null)
            {
                Console.WriteLine("Change Limit False User Passed");
            }
            else
            {
                Console.WriteLine("Change Limit False User Fail: change limit with false user should fail");
            }
            Console.WriteLine();

        }
        /// <summary>
        /// this function test get coulmn limit which includes successful get column limit, get column limit with a fake board name
        /// </summary>
        public void TestGetCoulmnLimit()
        {
            var email = "GetCoulmnLimit@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            //var taksID = 7;
            var title = "title";
            var description = "description";
            _us.Register(email, password);
            _bs.CreateBoard(email, boardname);
            _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));

            //


            //default limit -1

            var col = 0;
            var result = _bs.GetColumnLimit(email, boardname, col);
            var response = JsonSerializer.Deserialize<Response>(result);
            if(response.ErrorMessage == null)
            {
                Console.WriteLine("Get Column Limit Success Passed");
            }
            else
            {
                Console.WriteLine($"Get Column Limit Success Failed: {response.ErrorMessage}");
            }


            //change limit 20 and get

            var col2 = 0;

            _bs.ChangeLimit(email, boardname, col2, 20);

            var result2 = _bs.GetColumnLimit(email, boardname, col2);
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage == null)
            {
                Console.WriteLine("Get Column Limit Change Limit 20 Passed");
            }
            else
            {
                Console.WriteLine("Get Column Limit Change Limit 20 Failed: " + response2.ErrorMessage);
            }

            //fake board name




            var col3 = 0;

            var result3 = _bs.GetColumnLimit(email, boardname, col3);
            var response3 = JsonSerializer.Deserialize<Response>(result3);
            if (response3.ErrorMessage == null)
            {
                Console.WriteLine("Get Column Limit Change fake board Passed");
            }
            else
            {
                Console.WriteLine("Get Column Limit Change fake board Failed: " + response3.ErrorMessage);
            }
        }

        /// <summary>
        /// This function test create task which include succesful task creation, creation of a task with an emtpy title,
        /// creation of a board with a due date later than creation date, creation with a board which doesnt exit and 
        /// creation of a board with a user who doesnt exit
        /// </summary>
        public void TestcreateTask()
        {
            var email = "createtask@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            //initilize user - register user
            _us.Register(email, password);
            _bs.CreateBoard(email, boardname);
            //
            var title = "title";
            var description = "description";
            var result = _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("Create Task Success Passed");
            }
            else
            {
                Console.WriteLine($"Create Task Success Failed: {response.ErrorMessage}");
            }

            //empty title
            var title2 = "";
            var description2 = "description";
            var result2 = _ts.CreateTask(email, boardname, title2, description2, new DateTime(2024, 12, 15, 14, 30, 0));
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage != null)
            {
                Console.WriteLine("Create Task Empty Title Passed");
            }
            else
            {
                Console.WriteLine("Create Task Empty Title Failed: task cannot have an empty title");
            }


            //Due date before current date
            var title3 = "title";
            var description3 = "description";
            var result3 = _ts.CreateTask(email, boardname, title3, description3, new DateTime(2023, 12, 15, 14, 30, 0));
            var response3 = JsonSerializer.Deserialize<Response>(result3);
            if (response3.ErrorMessage != null)
            {
                Console.WriteLine("Create Task Due Date Before Current Time Passed");
            }
            else
            {
                Console.WriteLine("Create Task Due Date Before Current Time Failed: task due date must be later than current date");
            }

            //board doesnt exit
            var title4 = "title";
            var description4 = "description";
            var boardname4 = "notRealOne";
            var result4 = _ts.CreateTask(email, boardname4, title4, description4, new DateTime(2024, 12, 15, 14, 30, 0));
            var response4 = JsonSerializer.Deserialize<Response>(result4);
            if (response4.ErrorMessage != null)
            {
                Console.WriteLine("Create Task With Uncreated Board Passed");
            }
            else
            {
                Console.WriteLine("Create Task With Uncreated Board Failed: task should not be created with uncreated board");
            }


            //user doesnt exit
            var title5 = "title";
            var description5 = "description";
            var email5 = "notRealOne";
            var result5 = _ts.CreateTask(email5, boardname, title5, description5, new DateTime(2024, 12, 15, 14, 30, 0));
            var response5 = JsonSerializer.Deserialize<Response>(result5);
            if (response5.ErrorMessage != null)
            {
                Console.WriteLine("Create Task With Unkown User Passed");
            }
            else
            {
                Console.WriteLine("Create Task With Unkown User Failed: task should not be created with unknown user");
            }
            Console.WriteLine();



            //change limit then add task
            var coulmn = 0;
            var newLimit = 1;
            _bs.ChangeLimit(email, boardname, coulmn, newLimit);

            var title6 = "title2";
            var description6 = "description";
            var result6 = _ts.CreateTask(email, boardname, title6, description6, new DateTime(2024, 12, 15, 14, 30, 0));
            var response6 = JsonSerializer.Deserialize<Response>(result6);
            if (response6.ErrorMessage != null)
            {
                Console.WriteLine("Create Task Limited Tasks Passed");
            }
            else
            {
                Console.WriteLine("Create Task Limited Tasks Failed: task should not be created when reached to limit");
            }
            Console.WriteLine();


        }


        /// <summary>
        /// This function tests editing task's title which includes successful edit of a task and 
        /// editing to an empty title
        /// </summary>

        public void TestEditsTaskTitel()
        {
            var email = "EditTaskTitel@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            var taksID = 3;
            var title = "title";
            var description = "description";

            //initilize user - register user
            _us.Register(email, password);
            _bs.CreateBoard(email, boardname);
            _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));
            //
            var NewTitle = "newtitle";
            var result = _ts.EditTaskTitle(email, boardname, taksID, 0, NewTitle);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("Edits Task Titel Success Passed");
            }
            else
            {
                Console.WriteLine($"Edits Task Titel Success Failed: {response.ErrorMessage}");
            }

            //empty title
            var NewTitle2 = "";
            var result2 = _ts.EditTaskTitle(email, boardname, taksID, 0, NewTitle2);
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage != null)
            {
                Console.WriteLine("Edits Task Titel Empty title Passed");
            }
            else
            {
                Console.WriteLine($"Edits Task Titel Empty title Failed: {response2.ErrorMessage}");
            }

            Console.WriteLine();
        }
        //invalid board id?




        /// <summary>
        /// This function test edition of a task description which icludes successful task description edit
        /// </summary>

        public void TestEditsTaskDescription()
        {
            var email = "EditsTaskDescription@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            var taksID = 4;
            var title = "title";
            var description = "description";

            //initilize user - register user
            _us.Register(email, password);
            _bs.CreateBoard(email, boardname);
            _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));
            //
            var NewDescription = "NewDescription";
            var result = _ts.EditTaskDescription(email, boardname, taksID, 0, NewDescription);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("edit Task description Success Passed");
            }
            else
            {
                Console.WriteLine($"edit Task description Success Failed: {response.ErrorMessage}");
            }
            //invalid board id?


        }


        /// <summary>
        /// This function test edition of a task due date which includes successful edit of the due date
        /// and a due date which is after the creation date
        /// </summary>

        public void TestEditsTaskDueDate()
        {
            var email = "TestEditsTaskDueDate@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            var taksID = 5;
            var title = "title";
            var description = "description";

            //initilize user - register user
            _us.Register(email, password);
            _bs.CreateBoard(email, boardname);
            _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));
            //
            var Newdatetime = new DateTime(2025, 12, 15, 14, 30, 0);
            var result = _ts.EditTaskDueDate(email, boardname, taksID, 0, Newdatetime);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("edit Task duedate Success Passed");
            }
            else
            {
                Console.WriteLine($"edit Task duedate Success Failed: {response.ErrorMessage}");
            }


            //Due date before current date
            var newdatetime2 = new DateTime(2023, 12, 15, 14, 30, 0);
            var result3 = _ts.EditTaskDueDate(email, boardname, taksID, 0, newdatetime2);
            var response3 = JsonSerializer.Deserialize<Response>(result3);
            if (response3.ErrorMessage != null)
            {
                Console.WriteLine("Create Task Due Date Before Current Time Passed");
            }
            else
            {
                Console.WriteLine("Create Task Due Date Before Current Time Failed: task due date must be later than current date");
            }
            Console.WriteLine();
        }
        /// <summary>
        /// This function test in progress list which includes successful recieve of In progress list
        /// and test when there are no tasks in progress list
        /// </summary>
        public void Testinprogresstask()
        {
            var email = "Testinprogresstask@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            var taskID = 6;
            var title = "title";
            var description = "description";

            //initilize user - register user
            _us.Register(email, password);
            _bs.CreateBoard(email, boardname);
            _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));
            _ts.AssignTask(email, boardname, 0, taskID, email);
            _bs.MoveTask(email, boardname, 0, taskID);
            ///////
            var result = _bs.InProgressList(email);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("Create in progress list Success Passed");
            }
            else
            {
                Console.WriteLine($"Create in progress list Success Failed: {response.ErrorMessage}");
            }

            // if task is not in progress
            _bs.MoveTask(email, boardname, 3, taskID);
            var result2 = _bs.InProgressList(email);
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage == null)
            {
                Console.WriteLine("In progress list with no in progress task has passed");
            }
            else
            {
                Console.WriteLine($"In progress list has faild: there is no in progress task on board");
            }
        }

        /// <summary>
        /// This function tests move task which includes successful task move to another coulmn
        /// and move task when coulmn reached to limit
        /// </summary>
        public void TestMoveTask()
        {
            var email = "Testmovetask@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            var taskID = 8;
            var title = "title";
            var description = "description";

            //initilize user - register user
            _us.Register(email, password);
            _bs.CreateBoard(email, boardname);
            _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));
            _ts.AssignTask(email, boardname, 0, taskID, email);
            ///////
            var result = _bs.MoveTask(email, boardname, 0, taskID);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("moving Task Success Passed");
            }
            else
            {
                Console.WriteLine($"moving Task Success Failed: {response.ErrorMessage}");
            }

            //move to out limit coulumn 
            var result2 = _bs.MoveTask(email, boardname, 4, taskID);
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage == null)
            {
                Console.WriteLine("moving task has been faild: cant move task to an out limit colounm");
            }
            else
            {
                Console.WriteLine($"moving task to an out limit colounm has passed");
            }
        }
        /// <summary>
        /// This function test get column name which includes successful get coulmn name
        /// and get coulmn that doesnt exits
        /// </summary>
        public void TestgetcolumnName()
        {
            var email = "Testgetcolumnname@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            //var taskID = 6;
            var title = "title";
            var description = "description";

            //initilize user - register user
            _us.Register(email, password);
            _bs.CreateBoard(email, boardname);
            _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));
            ///////

            var result = _bs.GetColumnName(email, boardname, 0);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("get column name Success Passed");
            }
            else
            {
                Console.WriteLine($"get column name Success Failed: {response.ErrorMessage}");
            }

            //cheack with out of range column 
            var result2 = _bs.GetColumnName(email, boardname, 4);
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage == null)
            {
                Console.WriteLine("get column has been faild: colunm num out of range");
            }
            else
            {
                Console.WriteLine($"get column from out limit colounm num has passed");
            }
        }
        /// <summary>
        /// This function test get coulmn which includes successful get coulmn, and try to get
        /// a coulmn which doesnt exits
        /// </summary>
        public void TestGetColumn()
        {
            var email = "Testgetcolumn@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            //var taskID = 7;
            var title = "title";
            var description = "description";

            //initilize user - register user
            _us.Register(email, password);
            _bs.CreateBoard(email, boardname);
            _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));
            ///////

            var result = _bs.GetColumn(email, boardname, 0);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("get column name Success Passed");
            }
            else
            {
                Console.WriteLine($"get column name Success Failed: {response.ErrorMessage}");
            }

            //cheack with out of range column 
            var result2 = _bs.GetColumn(email, boardname, 4);
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage == null)
            {
                Console.WriteLine("get column has been faild: colunm num out of range");
            }
            else
            {
                Console.WriteLine($"get column from out limit colounm num has passed");
            }
        }


        /// <summary>
        /// this function tests get user board which includes successful get user board
        /// </summary>
        public void TestGetUserBoards()
        {
            var email = "TestGetUserBoards@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            //var taskID = 8;
            var title = "title";
            var description = "description";

            //initilize user - register user
            _us.Register(email, password);
            _bs.CreateBoard(email, boardname);
            _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));
            ///////

            var result = _bs.GetUserBoards(email);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("get users boards Success Passed");
            }
            else
            {
                Console.WriteLine($"get users boards Success Failed: {response.ErrorMessage}");
            }

            //cheack for fake user
            var fakeemial = "emial2";
            var result2 = _bs.GetUserBoards(fakeemial);
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage == null)
            {
                Console.WriteLine("get user boards has been faild: fake user");
            }
            else
            {
                Console.WriteLine($"get user boards with a fake user has passed");
            }
        }
        /// <summary>
        /// this function test get board name which includes successful get board name and get board name with a fake board ID
        /// </summary>

        public void TestGetBoardName()
        {
            var email = "TestGetBoardName@gmail.com";
            var password = "Pass123";
            var boardname = "board";
            //var taskID = 9;
            var title = "title";
            var description = "description";

            //initilize user - register user
            _us.Register(email, password);
            _bs.CreateBoard(email, boardname);
            _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));
            ///////

            //var boardIDsList = _bs.GetUserBoards(email);
            var BoardID = 3;
            var result = _bs.GetBoardName(BoardID);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("get board name Success Passed");
            }
            else
            {
                Console.WriteLine($"get board name Success Failed: {response.ErrorMessage}");
            }

            //cheack for fake board ID
            var fakeBoardID = 999999;
            var result2 = _bs.GetBoardName(fakeBoardID);
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage == null)
            {
                Console.WriteLine("get board name has been faild: fake board ID");
            }
            else
            {
                Console.WriteLine($"get board name with a fake board ID has passed");
            }
        }

        /// <summary>
        /// this function test join board which includes successful join board and join board with a fake board ID, fake email
        /// </summary>

        public void TestJoinBoard()
        {
            var email = "TestJoinBoard@gmail.com";
            var password = "Pass123";
            
            //var taskID = 10;
            

            //initilize user - register user
            _us.Register(email, password);
            
            ///////

            //var boardIDsList = _bs.GetUserBoards(email);
            var BoardID2 = 3;
            var result = _bs.JoinBoard(email,BoardID2);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("join board Success Passed");
            }
            else
            {
                Console.WriteLine($"join board Success Failed: {response.ErrorMessage}");
            }

            //cheack for fake board ID
            var fakeBoardID = 999999;
            var result2 = _bs.JoinBoard(email,fakeBoardID);
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage == null)
            {
                Console.WriteLine("join board has been faild: fake board ID");
            }
            else
            {
                Console.WriteLine($"join board with a fake board ID has passed");
            }

            //cheack for fake email
            var fakeEmail = "fakeemail";
            var result3 = _bs.JoinBoard(fakeEmail, fakeBoardID);
            var response3 = JsonSerializer.Deserialize<Response>(result3);
            if (response3.ErrorMessage == null)
            {
                Console.WriteLine("join board has been faild: fake email");
            }
            else
            {
                Console.WriteLine($"join board with a fake email has passed");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// this functions tests leave board which includes successful leave board and leave board with a fake board ID, fake email
        /// </summary>

        public void TestLeaveBoard()
        {
            var email = "TestLeaveBoard@gmail.com";
            var password = "Pass123";
            
            //var taskID = 11;
            

            //initilize user - register user
            _us.Register(email, password);
           
            ///////

            //var boardIDsList = _bs.GetUserBoards(email);
            _bs.JoinBoard(email, 3);
            var BoardID3 = 3;
            var result = _bs.LeaveBoard(email, BoardID3);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("Leave board Success Passed");
            }
            else
            {
                Console.WriteLine($"Leave board Success Failed: {response.ErrorMessage}");
            }

            //cheack for fake board ID
            var fakeBoardID = 999999;
            var result2 = _bs.LeaveBoard(email, fakeBoardID);
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage == null)
            {
                Console.WriteLine("Leave board has been faild: fake board ID");
            }
            else
            {
                Console.WriteLine($"Leave board with a fake board ID has passed");
            }

            //cheack for fake email
            var fakeEmail = "fakeemail";
            var result3 = _bs.LeaveBoard(fakeEmail, fakeBoardID);
            var response3 = JsonSerializer.Deserialize<Response>(result3);
            if (response3.ErrorMessage == null)
            {
                Console.WriteLine("Leave board has been faild: fake email");
            }
            else
            {
                Console.WriteLine($"Leave board with a fake email has passed");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// this functions tests assign task which includes successful assign task and assign task with a fake task ID, fake email, out of range column
        /// </summary>

        public void TestAssignTask()
        {
            var email = "TestAssignTask@gmail.com";
            var password = "Pass123";
            var emailassigner = "EmailAssignerTestAssignTask@gmail.com";
            var passwordAssigner = "Pass1234";
            var boardname = "board";
            var taskID = 13;
            var title = "title";
            var description = "description";
            var coluomord = 0;

            //initilize user - register user
            _us.Register(email, password);
            _us.Register(emailassigner,passwordAssigner);
            _bs.CreateBoard(email, boardname);
            _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));
            _bs.JoinBoard(emailassigner, 14);
            ///////

            var result = _ts.AssignTask(email,boardname,coluomord,taskID, emailassigner);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("Assign task Success Passed");
            }
            else
            {
                Console.WriteLine($"Assign task Success Failed: {response.ErrorMessage}");
            }

            //cheack for fake task ID
            var faketaskID = 999999;
            var result2 = _ts.AssignTask(email, boardname, coluomord, faketaskID, emailassigner);
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage == null)
            {
                Console.WriteLine("assign task has been faild: fake task ID");
            }
            else
            {
                Console.WriteLine($"assign task with a fake task ID has passed");
            }

            //cheack for fake email
            var fakeEmail = "fakeemail";
            var result3 = _ts.AssignTask(fakeEmail, boardname, coluomord, taskID, emailassigner);
            var response3 = JsonSerializer.Deserialize<Response>(result3);
            if (response3.ErrorMessage == null)
            {
                Console.WriteLine("assign task has been faild: fake email");
            }
            else
            {
                Console.WriteLine($"assign task with a fake email has passed");
            }

            //cheack for out of range column
            var OORcolumn = 99999;
            var result4 = _ts.AssignTask(fakeEmail, boardname, OORcolumn, taskID, emailassigner);
            var response4 = JsonSerializer.Deserialize<Response>(result4);
            if (response4.ErrorMessage == null)
            {
                Console.WriteLine("assign task has been faild: out of range column");
            }
            else
            {
                Console.WriteLine($"assign task with out of range column has passed");
            }
            Console.WriteLine();
        }


        /// <summary>
        /// this functions tests transfer ownership which includes successful transfer ownership and transfer ownership with a fake board name, fake email
        /// </summary>

        public void TestTransferOwnership()
        {
            var email = "TestTransferOwnership@gmail.com";
            var password = "Pass123";
            var newOwnerEmail = "newOwnerTestAssignTask@gmail.com";
            var passwordNewOwner = "Pass1234";
            var boardname = "board";
            //var taskID = 12;
            var title = "title";
            var description = "description";
            

            //initilize user - register user
            _us.Register(email, password);
            _us.Register(newOwnerEmail, passwordNewOwner);
            _bs.CreateBoard(email, boardname);
            _bs.JoinBoard(newOwnerEmail, 15);
            _ts.CreateTask(email, boardname, title, description, new DateTime(2024, 12, 15, 14, 30, 0));
            ///////

            var result = _bs.TransferOwnership(email,newOwnerEmail, boardname);
            var response = JsonSerializer.Deserialize<Response>(result);
            if (response.ErrorMessage == null)
            {
                Console.WriteLine("Transfer owner Success Passed");
            }
            else
            {
                Console.WriteLine($"Transfer owner Success Failed: {response.ErrorMessage}");
            }

            //cheack for fake boardname
            var fakeBoardName = "FakeBoardName";
            var result2 = _bs.TransferOwnership(email, newOwnerEmail, fakeBoardName);
            var response2 = JsonSerializer.Deserialize<Response>(result2);
            if (response2.ErrorMessage == null)
            {
                Console.WriteLine("Transfer owner has been faild: fake board name");
            }
            else
            {
                Console.WriteLine($"transfer owner with a fake board name has passed");
            }


            //cheack for fake email
            var fakeEmail = "fakeemail";
            var result3 = _bs.TransferOwnership(email, fakeEmail, boardname); ;
            var response3 = JsonSerializer.Deserialize<Response>(result3);
            if (response3.ErrorMessage == null)
            {
                Console.WriteLine("transfer owner has been faild: fake email");
            }
            else
            {
                Console.WriteLine($"transfer owner with a fake email has passed");
            }
            Console.WriteLine();
        }
    }
  }