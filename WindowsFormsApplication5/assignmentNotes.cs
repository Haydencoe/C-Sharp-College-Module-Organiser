using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace collegeModules
{
    public partial class assignmentNotes : Form
    {
        

        string newString { get; set; }

        public assignmentNotes()
        {
            InitializeComponent();

            string assignmentNumber = homePage.myAssignmentListBoxValue;

            try
            {

                assignmentNumber = assignmentNumber.Remove(0, 19);//Removes due date from string


                if (assignmentNumber.Length > 25)//Removes overdue only if its there

                {
                    string txt = assignmentNumber;

                    newString = txt.Remove(txt.Length - 9, 9);//Removes the over due
                }

                else
                {
                    newString = assignmentNumber;//if the there is no over due then only the due date is removed.
                }

                label2.Text = newString + " Notes";
            }//End of try 

            catch
            {
                label2.Text = "Assignment Title:";
            }

            string loadedText = "";



            string driveLetter = AppDomain.CurrentDomain.BaseDirectory;


            string filePath = string.Format(driveLetter + @"\Resources\Notes\Assignment Notes\{0}.txt", label2.Text);//file path for reading in the notes




            try
            {
                loadedText = string.Format(File.ReadAllText(filePath));//Loads rich text box with aved notes 
                richTextBox1.Text = loadedText;

               

            }

            catch
           {
                richTextBox1.Text = "No current notes.";//Enter this statement into rich text box if there are no saved notes.
            }


           

        }//End of method

        private void button1_Click(object sender, EventArgs e)//Close Editor Button
        {
            this.Hide();//Hides pane
        }

        private void button2_Click(object sender, EventArgs e)//Save Notes Button
        {



            string driveLetter = AppDomain.CurrentDomain.BaseDirectory;

            Directory.CreateDirectory(driveLetter + "\\Resources\\Notes\\Assignment Notes");//Creates folder if folder doesn't already exists 

            string filePath = string.Format(driveLetter + "\\Resources\\Notes\\Assignment Notes\\{0} Notes.txt", newString);//Path to save assignment notes to.

            try

            {
                File.WriteAllText(filePath, richTextBox1.Text);//Write the notes to the file path
                MessageBox.Show("Notes saved!");//Lets the user know the notes have been saved.

            }

            catch
            {
                MessageBox.Show("Failed to save file!");//Lets the user know if the notes couln't be saved. 
            }

        }




    }//End of Class 
}//End of Namespace