using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using DataAccess;
using DbLinq.Data.Linq;

namespace SQLiteWithLinq
{
    public partial class Form1 : Form
    {
        //database object //FKSupport=True;foreign keys=true;EnforceFKConstraints=Yes|True|1;
        string _connection = @"foreign keys=True;Data Source=K:\My Documents\Visual Studio 2010\Projects\SQLiteWithLinq\SQLiteWithLinq\Scripts\TestWithLinq.db3;DbLinqProvider=sqlite;";

        public Form1()
        {
            InitializeComponent();
        }

        private Main GetContext()
        {
            var context = new Main(new SQLiteConnection(_connection));
            return context;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintTestTable();

            //Insert();
            //Update();
            //Delete();
            //DeleteAll();
            //ViolateForeignKey();
            //InsertComplexModel();
            //DeleteComplexModel(1);

            PrintTestTable();
        }

        private void DeleteComplexModel(int TestTableID)
        {
            using (var context = GetContext())
            {
                try
                {
                    DeleteTestTableByID(context, TestTableID);

                    context.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void DeleteTestTableByID(Main context, int TestTableID)
        {
            //delete testtable record
            var testtables = from p in context.TestTable
                             where p.ID == TestTableID
                             select p;

            foreach (var TestTableItem in testtables)
            {
                context.TestTable.DeleteOnSubmit(TestTableItem);

                DeletePersonByID(context, TestTableItem.ID);
            }
        }

        private void DeletePersonByID(Main context, int TestTableID)
        {
            //delete associated persons
            var Persons = from p in context.Person
                          where p.TestTableID == TestTableID
                          select p;

            foreach (var PersonItem in Persons)
            {
                context.Person.DeleteOnSubmit(PersonItem);
            }
        }

        private void InsertComplexModel()
        {
            using (var context = GetContext())
            {
                try
                {
                    var test = GetTestTable();
                    context.TestTable.InsertOnSubmit(test);

                    context.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void PrintTestTable()
        {
            using (var context = GetContext())
            {
                var query = from p in context.TestTable select p;

                foreach (var item in query)
                {
                    Console.WriteLine("ID = " + item.ID + " | Name = " + item.Name + " | Desc = " + item.Description);
                }
            }
        }

        private void Insert()
        {
            using (var context = GetContext())
            {
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var person = new DbLinq.Data.Linq.EntitySet<Person>();
                        person.Add(new Person() { Person1 = "test" });

                        var tod = new TestTable() { Name = "tod", Description = "Record Insert.", Person = person};
                        context.TestTable.InsertOnSubmit(tod);
                    }

                    context.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private new void Update()
        {
            using (var context = GetContext())
            {
                var tod = from p in context.TestTable
                          where p.ID == 5
                          select p;

                foreach (var item in tod)
                {
                    item.Description = "Record Update.";
                }
                
                context.SubmitChanges();
            }
        }

        private void Delete()
        {
            using (var context = GetContext())
            {
                var list = from p in context.TestTable
                          where p.ID < 5
                          select p;

                foreach (var item in list)
                {
                    context.TestTable.DeleteOnSubmit(item);
                }
                
                context.SubmitChanges();
            }
        }

        private void DeleteAll()
        {
            //TestTable
            using (var context = GetContext())
            {
                var all = (from p in context.TestTable
                           select p);

                foreach (var item in all)
                {
                    context.TestTable.DeleteOnSubmit(item);
                }
                
                context.SubmitChanges();
            }

            //Person
            using (var context = GetContext())
            {
                var all = (from p in context.Person
                           select p);

                foreach (var item in all)
                {
                    context.Person.DeleteOnSubmit(item);
                }

                context.SubmitChanges();
            }
        }

        private void ViolateForeignKey()
        {
            using (var context = GetContext())
            {
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var person = new Person() { Person1 = "test" };
                        context.Person.InsertOnSubmit(person);
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        var test = new TestTable() { Name = "chris", Description = "desc" };
                        context.TestTable.InsertOnSubmit(test);
                    }

                    context.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private TestTable GetTestTable()
        {
            var person = new EntitySet<Person>();
            person.Add(new Person() { Person1 = "test" });
            person.Add(new Person() { Person1 = "test2" });

            var table = new TestTable() { Name = "Complex", Description = "Record Insert.", Person = person };
            return table;
        }
    }
}
