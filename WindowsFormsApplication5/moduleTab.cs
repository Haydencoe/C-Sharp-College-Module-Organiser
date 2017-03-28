using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Timers;

namespace collegeModules
{
    public partial class moduleTab : UserControl
    {

        public static List<string> MyAssignments = new List<string>();//List for storing the assignments

        public  moduleTab()
        {
            InitializeComponent();

            comboBox1_Load();//Calls the loading of the comboBox method

            

            string selectedText = "";

            string toLoad = homePage.moduleValue;//sets the string to the selected module value

            try {

               selectedText = File.ReadAllText(@"Resources\Modules\" + toLoad + ".txt");//Reads in the chosen module's details


            }

            catch
            {

                MessageBox.Show("Failed to add module!");//If loading fails error is caught.
            }



            richTextBox1.Text = selectedText;  //  Fills tabPage rich text box with the loaded module details

                     
               

                string checker = "(?<=Assignment\\s*)\\d+/\\d+/\\d+";//regex string to look for assignment dates


            MatchCollection strOutput = Regex.Matches(selectedText, checker);//finds matchs in text against the checker string

            if (strOutput.Count > 0)
            {
                MyAssignments.Clear();
                 
                var list = strOutput.Cast<Match>().Select(match => match.Value).ToArray();//Puts the Regex matches into an Array
                 
                int i = 0;

                foreach(string full in list)

                {

                   
                    i++;
                    string fullSentence;

                    DateTime startDate = DateTime.Parse(full);//converts the date to a DateTime format

                    if (DateTime.Now > startDate)
                    {
                        fullSentence =  "Assignment" + i + " is due: " + full + "   Assignment is over due!!";//String if over due to be added to homepage assignment listBox
                       

                        MyAssignments.Add(full + "         " + toLoad+" Assignment " + i + " Over Due!");//string if over due to be added to the module tab listBox

                        listBox1.Items.Add(fullSentence);//adds string to listBox

                    }

                    else
                    {
                        fullSentence = "Assignment" + i + " is due: " + full;//String if not over due to be added to homepage assignment listBox


                        MyAssignments.Add(full + "         " + toLoad + " Assignment " + i +" "  );//string if not over due to be added to the module tab listBox


                        listBox1.Items.Add(fullSentence);
                    }


                    
                }//End of forEach loop

            }

           

            }//End of method



        private void button1_Click(object sender, EventArgs e)//Load Notes Button
        {
            string toLoad = homePage.moduleValue;

            string loadedText = "";

            var selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);//gets selected item from comboBox

            string driveLetter = AppDomain.CurrentDomain.BaseDirectory;

           
                string filePath = string.Format(driveLetter + @"\Resources\Notes\{0} Notes\{1}.txt", toLoad, selected);//path to load from


            try
            {
               loadedText = string.Format(File.ReadAllText(filePath));//Reads in note
            }
            
            catch
            {
                MessageBox.Show("Failed to Load Selected Notes!");//catchs error if notes cannot be loaded
            }

            //Changes note saved title to shortened version
            string newString = selected.Replace(toLoad+" ", "");

            newString = newString.Replace("Notes", "");

            textBox1.Text = newString;

            richTextBox2.Text = loadedText;

        }

        public void button2_Click(object sender, EventArgs e)//Save Notes Button
        {
            if (textBox1.Text == "")

            {
                MessageBox.Show("Please enter a title for your note!");//Tells the user to add a title for the note
            }

            else
            {

                string toLoad = homePage.moduleValue;//Gets the title of the current module

                string driveLetter = AppDomain.CurrentDomain.BaseDirectory;

                Directory.CreateDirectory(driveLetter + "\\Resources\\Notes\\" + toLoad + " Notes");//Creates folder if not already there

                string filePath = string.Format( "\\Resources\\Notes\\{0} Notes\\{1} {2} Notes.txt", toLoad, toLoad, textBox1.Text);//Path to save to 
          
                try
                
                {
                    File.WriteAllText(filePath, richTextBox2.Text);//Saves notes
                    MessageBox.Show("Notes saved!");
                }

                catch
                {
                    MessageBox.Show("Failed to save file!");//Catch error in saving notes
                }
                
                
                this.comboBox1.Items.Clear();

                dynamic dir = string.Format(driveLetter + @"\Resources\Notes\{0} Notes", toLoad);//Path to load notes from

                foreach (string file in Directory.GetFiles(dir))
                {

                    this.comboBox1.Items.Add(Path.GetFileNameWithoutExtension(file));//Loads availble notes to note comboBox

                }
                
            } //End of else statement           
            
        }//End of method 

   
        private void button3_Click(object sender, EventArgs e)//Delete Button
        {
            string driveLetter = AppDomain.CurrentDomain.BaseDirectory;

            var selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);

            string toLoad = homePage.moduleValue;


            dynamic dir = string.Format(driveLetter + @"\Resources\Notes\{0} Notes\{1}.txt",toLoad, selected);

            File.Delete(dir);//Deletes path file

            comboBox1.SelectedIndex = -1;

            comboBox1.Items.Clear();//Clearing all notes
            textBox1.Clear();//Clears the deleted notes from textbox
            richTextBox2.Clear();//Clears the deleted notes from richtextbox

            comboBox1_Load();//Invokes the combox1_Load method so the comboBox is loaded with the updated list of notes 


        }//End of button_3 click method 


        public void comboBox1_Load()

        {


           string toLoad = homePage.moduleValue;
           string driveLetter = AppDomain.CurrentDomain.BaseDirectory;

            try
            {

                dynamic dir = string.Format(driveLetter + @"\Resources\Notes\{0} Notes", toLoad);


                
                    foreach (string file in Directory.GetFiles(dir))
                    {
                        
                    string fileNoExt = Path.GetFileNameWithoutExtension(file);

                        if (fileNoExt.Length > 0)

                            this.comboBox1.Items.Add(Path.GetFileNameWithoutExtension(fileNoExt));//Loads in module notes to comboBox1.


                       else  if (fileNoExt.Length == 0)
                        this.comboBox1.Items.Add("No current notes for this module.");//Lets the user know if there are no notes for the module.

                }




            }//End of try statement
            catch 
            {
                this.comboBox1.Items.Add("No current notes for this module.");//Lets the user know if there are no notes for the module.
            }

        }


    }//End of namespace
}//End of class 
